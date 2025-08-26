# Corporate Management System - Quick API Test Script
# This script demonstrates basic CRUD operations using PowerShell

$baseUrl = "http://localhost:5000/api"

Write-Host "🚀 Corporate Management System API Testing Script" -ForegroundColor Green
Write-Host "Make sure the API is running at: $baseUrl" -ForegroundColor Yellow
Write-Host ""

# Function to make HTTP requests with error handling
function Invoke-ApiRequest {
    param(
        [string]$Method,
        [string]$Uri,
        [hashtable]$Body = $null,
        [string]$Description
    )
    
    Write-Host "📡 $Description" -ForegroundColor Cyan
    Write-Host "   $Method $Uri" -ForegroundColor Gray
    
    try {
        $headers = @{ "Content-Type" = "application/json" }
        
        if ($Body) {
            $jsonBody = $Body | ConvertTo-Json -Depth 10
            $response = Invoke-RestMethod -Uri $Uri -Method $Method -Body $jsonBody -Headers $headers
        } else {
            $response = Invoke-RestMethod -Uri $Uri -Method $Method -Headers $headers
        }
        
        Write-Host "   ✅ Success" -ForegroundColor Green
        Write-Host ""
        return $response
    }
    catch {
        Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
        return $null
    }
}

# Test 1: Check if API is running
Write-Host "=" * 50
Write-Host "TEST 1: API Health Check"
Write-Host "=" * 50

$seedStats = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/seed/stats" -Description "Getting database statistics"

if ($seedStats -and $seedStats.success) {
    Write-Host "📊 Current Database Stats:" -ForegroundColor Yellow
    $seedStats.data | Format-Table -AutoSize
} else {
    Write-Host "⚠️  API might not be running. Please start the application first:" -ForegroundColor Red
    Write-Host "   dotnet run --project CMS.API" -ForegroundColor Yellow
    exit
}

# Test 2: Seed sample data if database is empty
Write-Host "=" * 50
Write-Host "TEST 2: Database Setup"
Write-Host "=" * 50

if ($seedStats.data.Users -eq 0) {
    Write-Host "📥 Database is empty. Seeding sample data..." -ForegroundColor Yellow
    $seedResult = Invoke-ApiRequest -Method "POST" -Uri "$baseUrl/seed/sample-data" -Description "Seeding sample data"
    
    if ($seedResult -and $seedResult.success) {
        Write-Host "✅ Sample data seeded successfully!" -ForegroundColor Green
        Start-Sleep -Seconds 2
        
        # Get updated stats
        $updatedStats = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/seed/stats" -Description "Getting updated database statistics"
        if ($updatedStats -and $updatedStats.success) {
            Write-Host "📊 Updated Database Stats:" -ForegroundColor Yellow
            $updatedStats.data | Format-Table -AutoSize
        }
    }
} else {
    Write-Host "✅ Database already contains data. Proceeding with tests..." -ForegroundColor Green
}

# Test 3: User Management
Write-Host "=" * 50
Write-Host "TEST 3: User Management (CRUD)"
Write-Host "=" * 50

# Get all users
$users = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/users?page=1&limit=5" -Description "Getting all users (first 5)"

if ($users -and $users.success) {
    Write-Host "👥 Users found: $($users.data.pagination.total)" -ForegroundColor Yellow
    $users.data.users | Select-Object Id, FirstName, LastName, Email, Role, DepartmentName | Format-Table -AutoSize
    
    if ($users.data.users.Count -gt 0) {
        $firstUser = $users.data.users[0]
        
        # Get specific user
        $userDetail = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/users/$($firstUser.Id)" -Description "Getting user details for $($firstUser.FirstName) $($firstUser.LastName)"
        
        if ($userDetail -and $userDetail.success) {
            Write-Host "👤 User Details:" -ForegroundColor Yellow
            $userDetail.data | Format-List
        }
    }
}

# Test 4: Department Management
Write-Host "=" * 50
Write-Host "TEST 4: Department Management"
Write-Host "=" * 50

$departments = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/departments" -Description "Getting all departments"

if ($departments -and $departments.Count -gt 0) {
    Write-Host "🏢 Departments found: $($departments.Count)" -ForegroundColor Yellow
    $departments | Select-Object Id, Name, Budget, @{Name='Users';Expression={if($_.User){$_.User.Count}else{0}}} | Format-Table -AutoSize
}

# Test 5: Asset Management
Write-Host "=" * 50
Write-Host "TEST 5: Asset Management"
Write-Host "=" * 50

$assets = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/assets" -Description "Getting all assets"

if ($assets -and $assets.Count -gt 0) {
    Write-Host "🏗️ Assets found: $($assets.Count)" -ForegroundColor Yellow
    $assets | Select-Object Id, Name, Category, Value, Status, @{Name='Assigned';Expression={if($_.AssignedUser){$_.AssignedUser.FirstName + ' ' + $_.AssignedUser.LastName}else{'Unassigned'}}} | Format-Table -AutoSize
}

# Test 6: Expense Management
Write-Host "=" * 50
Write-Host "TEST 6: Expense Management"
Write-Host "=" * 50

$expenses = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/expenses" -Description "Getting all expenses"

if ($expenses -and $expenses.Count -gt 0) {
    Write-Host "💰 Expenses found: $($expenses.Count)" -ForegroundColor Yellow
    $expenses | Select-Object Id, Name, Amount, Status, Category, @{Name='Submitter';Expression={$_.User.FirstName + ' ' + $_.User.LastName}} | Format-Table -AutoSize
}

# Test 7: Dashboard Analytics
Write-Host "=" * 50
Write-Host "TEST 7: Dashboard Analytics"
Write-Host "=" * 50

$dashboard = Invoke-ApiRequest -Method "GET" -Uri "$baseUrl/dashboard/overview" -Description "Getting dashboard overview"

if ($dashboard) {
    Write-Host "📊 Dashboard Overview:" -ForegroundColor Yellow
    Write-Host "   👥 Users: Total=$($dashboard.Users.Total), Active=$($dashboard.Users.Active)" -ForegroundColor White
    Write-Host "   🏗️ Assets: Total=$($dashboard.Assets.Total), Value=$($dashboard.Assets.TotalValue)" -ForegroundColor White
    Write-Host "   💰 Expenses: This Month=$($dashboard.Expenses.ThisMonth), Pending=$($dashboard.Expenses.PendingApproval)" -ForegroundColor White
    Write-Host "   🏢 Departments: Total=$($dashboard.Departments.Total)" -ForegroundColor White
}

# Test 8: Create New Records (Demonstrate POST operations)
Write-Host "=" * 50
Write-Host "TEST 8: Creating New Records"
Write-Host "=" * 50

if ($departments -and $departments.Count -gt 0) {
    $testDepartmentId = $departments[0].Id
    
    # Create new user
    $newUser = @{
        email = "test.user.$(Get-Date -Format 'yyyyMMddHHmmss')@company.com"
        firstName = "Test"
        lastName = "User"
        role = 2
        departmentId = $testDepartmentId
    }
    
    $createdUser = Invoke-ApiRequest -Method "POST" -Uri "$baseUrl/users" -Body $newUser -Description "Creating new test user"
    
    if ($createdUser -and $createdUser.success) {
        Write-Host "👤 Created user: $($createdUser.data.FirstName) $($createdUser.data.LastName)" -ForegroundColor Green
        
        # Create asset request for the new user
        $assetRequest = @{
            assetName = "Test Laptop"
            reason = "For testing purposes"
            specifications = "Standard business laptop"
            userId = $createdUser.data.Id
            departmentId = $testDepartmentId
            durationType = 0
            requestedDate = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        }
        
        $createdRequest = Invoke-ApiRequest -Method "POST" -Uri "$baseUrl/assetrequests" -Body $assetRequest -Description "Creating asset request for test user"
        
        if ($createdRequest -and $createdRequest.success) {
            Write-Host "📋 Created asset request for test laptop" -ForegroundColor Green
        }
    }
}

Write-Host "=" * 50
Write-Host "🎉 API Testing Complete!" -ForegroundColor Green
Write-Host "=" * 50
Write-Host ""
Write-Host "🌐 Access the Swagger documentation at: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host "📚 Full testing guide available in: TESTING_GUIDE.md" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Use Postman or Swagger for detailed testing" -ForegroundColor White
Write-Host "2. Test the external authentication integration" -ForegroundColor White
Write-Host "3. Validate business workflows end-to-end" -ForegroundColor White

# Corporate Management System - Testing Guide

## 🚀 Quick Start Guide for Testing the Backend API

### 1. **Setup and Run the Application**

```powershell
# Navigate to the project directory
cd "c:\Users\MuBeeN\Downloads\CorporateManagementSystem_Backend_With_Auth\CorporateManagementSystem"

# Make sure database is updated with latest migrations
dotnet ef database update --project CMS.Infrastructure --startup-project CMS.API

# Run the application
dotnet run --project CMS.API
```

The API will be available at: **http://localhost:5000** or **https://localhost:5001**
Swagger Documentation: **http://localhost:5000/swagger**

---

### 2. **Seed Sample Data for Testing**

Before testing CRUD operations, load sample data:

**POST** `http://localhost:5000/api/seed/sample-data`

Response:
```json
{
    "success": true,
    "message": "Sample data has been seeded successfully",
    "timestamp": "2025-08-25T10:30:00Z"
}
```

---

### 3. **Test All CRUD Operations**

#### **A. 👥 USERS MANAGEMENT**

**Get All Users** (with pagination and filters)
```
GET http://localhost:5000/api/users?page=1&limit=10&active=true
```

**Get Specific User**
```
GET http://localhost:5000/api/users/{user-id}
```

**Create New User**
```
POST http://localhost:5000/api/users
Content-Type: application/json

{
    "email": "new.employee@company.com",
    "firstName": "New",
    "lastName": "Employee",
    "role": 2,
    "departmentId": "{department-id}"
}
```

**Sync User from External System**
```
POST http://localhost:5000/api/users/sync
Content-Type: application/json

{
    "externalUserId": "ext_005",
    "externalSystemName": "ExternalAuth",
    "email": "synced.user@company.com",
    "firstName": "Synced",
    "lastName": "User",
    "role": 2,
    "departmentId": "{department-id}",
    "active": true
}
```

#### **B. 🏢 DEPARTMENTS MANAGEMENT**

**Get All Departments**
```
GET http://localhost:5000/api/departments
```

**Get Specific Department**
```
GET http://localhost:5000/api/departments/{department-id}
```

**Create New Department**
```
POST http://localhost:5000/api/departments
Content-Type: application/json

{
    "name": "Research & Development",
    "description": "Product research and development",
    "budget": 400000
}
```

**Update Department**
```
PUT http://localhost:5000/api/departments/{department-id}
Content-Type: application/json

{
    "id": "{department-id}",
    "name": "Research & Development",
    "description": "Advanced product research and development",
    "budget": 450000
}
```

**Delete Department**
```
DELETE http://localhost:5000/api/departments/{department-id}
```

#### **C. 🏗️ ASSETS MANAGEMENT**

**Get All Assets**
```
GET http://localhost:5000/api/assets
```

**Get Specific Asset**
```
GET http://localhost:5000/api/assets/{asset-id}
```

**Create New Asset**
```
POST http://localhost:5000/api/assets
Content-Type: application/json

{
    "name": "MacBook Pro M3",
    "category": "Computer",
    "serialNumber": "MP2024001",
    "departmentId": "{department-id}",
    "value": 2500,
    "purchaseDate": "2025-08-01T00:00:00Z",
    "condition": 0,
    "location": "IT Office - Desk 5"
}
```

**Update Asset Status**
```
PUT http://localhost:5000/api/assets/{asset-id}
Content-Type: application/json

{
    "id": "{asset-id}",
    "name": "MacBook Pro M3",
    "category": "Computer",
    "serialNumber": "MP2024001",
    "departmentId": "{department-id}",
    "assignedTo": "{user-id}",
    "status": 0,
    "value": 2500,
    "purchaseDate": "2025-08-01T00:00:00Z",
    "condition": 0,
    "location": "IT Office - Desk 5"
}
```

#### **D. 📋 ASSET REQUESTS**

**Get All Asset Requests**
```
GET http://localhost:5000/api/assetrequests
```

**Create Asset Request**
```
POST http://localhost:5000/api/assetrequests
Content-Type: application/json

{
    "assetName": "4K Monitor",
    "reason": "Need for graphic design work",
    "specifications": "27-inch 4K monitor with USB-C",
    "userId": "{user-id}",
    "departmentId": "{department-id}",
    "durationType": 0,
    "requestedDate": "2025-08-25T00:00:00Z"
}
```

#### **E. 💰 EXPENSES MANAGEMENT**

**Get All Expenses**
```
GET http://localhost:5000/api/expenses
```

**Create New Expense**
```
POST http://localhost:5000/api/expenses
Content-Type: application/json

{
    "name": "Conference Registration",
    "amount": 599.99,
    "userId": "{user-id}",
    "departmentId": "{department-id}",
    "date": "2025-08-25T00:00:00Z",
    "type": 0,
    "category": "Training",
    "description": "Registration for tech conference"
}
```

**Update Expense Status (Approve/Reject)**
```
PUT http://localhost:5000/api/expenses/{expense-id}
Content-Type: application/json

{
    "id": "{expense-id}",
    "name": "Conference Registration",
    "amount": 599.99,
    "userId": "{user-id}",
    "departmentId": "{department-id}",
    "date": "2025-08-25T00:00:00Z",
    "status": 1,
    "type": 0,
    "category": "Training",
    "description": "Registration for tech conference"
}
```

#### **F. 💸 REIMBURSEMENTS**

**Get All Reimbursements**
```
GET http://localhost:5000/api/reimbursements
```

**Create Reimbursement Request**
```
POST http://localhost:5000/api/reimbursements
Content-Type: application/json

{
    "name": "Client Meeting Travel",
    "amount": 320.50,
    "userId": "{user-id}",
    "departmentId": "{department-id}",
    "date": "2025-08-20T00:00:00Z",
    "type": 1,
    "description": "Travel expenses for client meeting in another city"
}
```

#### **G. 📊 DASHBOARD & ANALYTICS**

**Get Dashboard Overview**
```
GET http://localhost:5000/api/dashboard/overview
```

**Get Financial Trends**
```
GET http://localhost:5000/api/dashboard/financial-trends?months=6
```

**Get Asset Metrics**
```
GET http://localhost:5000/api/dashboard/asset-metrics
```

**Get Department Performance**
```
GET http://localhost:5000/api/dashboard/department-performance
```

---

### 4. **Status Codes & Enums Reference**

#### User Roles:
- 0 = Employee
- 1 = Manager  
- 2 = Admin

#### Asset Status:
- 0 = Assigned
- 1 = Unassigned
- 2 = Maintenance
- 3 = Retired

#### Asset Condition:
- 0 = Excellent
- 1 = Good
- 2 = Fair
- 3 = Poor

#### Request Status:
- 0 = Pending
- 1 = Approved
- 2 = Rejected
- 3 = Fulfilled

#### Expense Status:
- 0 = Pending
- 1 = Approved
- 2 = Rejected

#### Reimbursement Status:
- 0 = Pending
- 1 = Approved
- 2 = Rejected
- 3 = Paid

---

### 5. **Database Management for Testing**

**Check Database Stats**
```
GET http://localhost:5000/api/seed/stats
```

**Clear All Data** (⚠️ Use with caution!)
```
DELETE http://localhost:5000/api/seed/clear-data
```

**Re-seed Sample Data**
```
POST http://localhost:5000/api/seed/sample-data
```

---

### 6. **Sample Test Workflow**

1. **Setup**: Run the application and seed sample data
2. **View Data**: Check dashboard overview to see seeded data
3. **Create**: Add new users, departments, assets, expenses
4. **Read**: Fetch and filter data using various endpoints
5. **Update**: Modify asset assignments, approve expenses, update user roles
6. **Delete**: Remove unnecessary records
7. **Analytics**: Check dashboard metrics and trends

---

### 7. **Testing Tools Recommendations**

- **Postman**: For comprehensive API testing with collections
- **Swagger UI**: Available at `/swagger` for interactive testing
- **curl**: For command-line testing
- **REST Client** (VS Code extension): For in-editor testing

---

### 8. **Common Test Scenarios**

**Employee Onboarding Flow:**
1. Create new department (if needed)
2. Create/sync new user from external system
3. Request assets for new employee
4. Approve asset requests
5. Assign assets to employee

**Expense Management Flow:**
1. Employee submits expense
2. Manager reviews and approves/rejects
3. Finance processes approved expenses
4. Generate expense reports

**Asset Lifecycle Flow:**
1. Purchase new asset (create asset record)
2. Assign to employee
3. Schedule maintenance
4. Update asset condition
5. Retire when no longer useful

This comprehensive testing setup allows you to validate all CRUD operations and business workflows in your Corporate Management System!

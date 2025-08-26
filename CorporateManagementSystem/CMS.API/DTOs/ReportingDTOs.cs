using System;
using System.Collections.Generic;
using CMS.Core.Entities;

namespace CMS.API.DTOs
{
    // Ledger DTOs
    public class LedgerEntryDto
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public EntryType ReferenceType { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public Guid? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid PostedBy { get; set; }
        public string PostedByName { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateLedgerEntryDto
    {
        public Guid ReferenceId { get; set; }
        public EntryType ReferenceType { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTime PostingDate { get; set; }
    }

    public class LedgerSummaryDto
    {
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal Balance { get; set; }
    }

    // Reporting DTOs
    public class ExpenseReportDto
    {
        public ReportMetadataDto ReportMetadata { get; set; }
        public ExpenseReportSummaryDto Summary { get; set; }
        public List<CategoryBreakdownDto> CategoryBreakdown { get; set; }
        public List<MonthlyTrendDto> MonthlyTrend { get; set; }
    }

    public class AssetReportDto
    {
        public ReportMetadataDto ReportMetadata { get; set; }
        public AssetReportSummaryDto Summary { get; set; }
        public List<CategoryBreakdownDto> CategoryBreakdown { get; set; }
        public List<DepreciationDto> DepreciationSchedule { get; set; }
    }

    public class FinancialSummaryDto
    {
        public FinancialSummaryOverviewDto Summary { get; set; }
        public List<DepartmentBreakdownDto> DepartmentBreakdown { get; set; }
        public TrendsDto Trends { get; set; }
    }

    public class ReportMetadataDto
    {
        public string Period { get; set; }
        public string Department { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string GeneratedBy { get; set; }
    }

    public class ExpenseReportSummaryDto
    {
        public decimal TotalExpenses { get; set; }
        public decimal ApprovedExpenses { get; set; }
        public decimal PendingExpenses { get; set; }
        public decimal RejectedExpenses { get; set; }
        public decimal BudgetUtilization { get; set; }
    }

    public class AssetReportSummaryDto
    {
        public decimal TotalValue { get; set; }
        public int TotalCount { get; set; }
        public int AssignedCount { get; set; }
        public int UnassignedCount { get; set; }
        public int MaintenanceCount { get; set; }
        public decimal DepreciationValue { get; set; }
    }

    public class FinancialSummaryOverviewDto
    {
        public decimal TotalExpenses { get; set; }
        public decimal TotalReimbursements { get; set; }
        public decimal TotalAssetValue { get; set; }
        public decimal BudgetUtilization { get; set; }
        public string ReportPeriod { get; set; }
    }

    public class CategoryBreakdownDto
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class MonthlyTrendDto
    {
        public string Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class DepreciationDto
    {
        public Guid AssetId { get; set; }
        public string AssetName { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal DepreciationAmount { get; set; }
        public DateTime DepreciationDate { get; set; }
    }

    public class DepartmentBreakdownDto
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal Expenses { get; set; }
        public decimal Reimbursements { get; set; }
        public decimal AssetValue { get; set; }
        public decimal BudgetUtilization { get; set; }
    }

    public class TrendsDto
    {
        public decimal ExpenseGrowth { get; set; }
        public decimal ReimbursementGrowth { get; set; }
        public decimal AssetGrowth { get; set; }
    }
}

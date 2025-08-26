using System;
using System.Collections.Generic;
using CMS.Core.Entities;

namespace CMS.API.DTOs
{
    // Expense DTOs
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime Date { get; set; }
        public ExpenseStatus Status { get; set; }
        public ExpenseType Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ReceiptUrl { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string RejectionReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ExpenseDocumentDto> Documents { get; set; } = new List<ExpenseDocumentDto>();
    }

    public class CreateExpenseDto
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public ExpenseType Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ReceiptFile { get; set; } // Base64 encoded file
    }

    public class ExpenseDocumentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public int? FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class ExpenseSummaryDto
    {
        public decimal TotalAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal RejectedAmount { get; set; }
    }

    public class ApproveExpenseDto
    {
        public string ApprovalNotes { get; set; }
    }

    public class RejectExpenseDto
    {
        public string RejectionReason { get; set; }
    }

    // Reimbursement DTOs
    public class ReimbursementDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime Date { get; set; }
        public ReimbursementStatus Status { get; set; }
        public ReimbursementType Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string[] ReceiptUrls { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string RejectionReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReimbursementDto
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public ReimbursementType Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string[] ReceiptFiles { get; set; } // Base64 encoded files
    }

    public class ApproveReimbursementDto
    {
        public string ApprovalNotes { get; set; }
    }

    public class RejectReimbursementDto
    {
        public string RejectionReason { get; set; }
    }

    // Complaint and Suggestion DTOs
    public class ComplaintSuggestionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public FeedbackType Type { get; set; }
        public string Category { get; set; }
        public PriorityLevel Priority { get; set; }
        public FeedbackStatus Status { get; set; }
        public Guid SubmittedBy { get; set; }
        public string SubmitterName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid? AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public string Resolution { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateComplaintSuggestionDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public FeedbackType Type { get; set; }
        public string Category { get; set; }
        public PriorityLevel Priority { get; set; }
    }

    public class AssignFeedbackDto
    {
        public Guid AssignedTo { get; set; }
    }

    public class ResolveFeedbackDto
    {
        public string Resolution { get; set; }
    }
}

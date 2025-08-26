using System;
using System.Collections.Generic;
using CMS.Core.Entities;

namespace CMS.API.DTOs
{
    // Asset DTOs
    public class AssetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SerialNumber { get; set; }
        public Guid? AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public AssetStatus Status { get; set; }
        public decimal Value { get; set; }
        public DateTime PurchaseDate { get; set; }
        public AssetCondition Condition { get; set; }
        public string Location { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAssetDto
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string SerialNumber { get; set; }
        public Guid DepartmentId { get; set; }
        public decimal Value { get; set; }
        public DateTime PurchaseDate { get; set; }
        public AssetCondition Condition { get; set; }
        public string Location { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public object Specifications { get; set; }
    }

    public class AssetSummaryDto
    {
        public decimal TotalValue { get; set; }
        public int AssignedCount { get; set; }
        public int UnassignedCount { get; set; }
        public int MaintenanceCount { get; set; }
        public int RetiredCount { get; set; }
    }

    public class AssignAssetDto
    {
        public Guid UserId { get; set; }
        public string Notes { get; set; }
    }

    // Asset Request DTOs
    public class AssetRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string AssetName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; }
        public DurationType DurationType { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Specifications { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public RequestStatus Status { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string RejectionReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAssetRequestDto
    {
        public string AssetName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; }
        public DurationType DurationType { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Specifications { get; set; }
        public Guid DepartmentId { get; set; }
    }

    public class ApproveAssetRequestDto
    {
        public string ApprovalNotes { get; set; }
    }

    public class RejectAssetRequestDto
    {
        public string RejectionReason { get; set; }
    }

    // Asset Maintenance DTOs
    public class AssetMaintenanceDto
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public string AssetName { get; set; }
        public Guid ReportedBy { get; set; }
        public string ReportedByName { get; set; }
        public string IssueDescription { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public PriorityLevel Priority { get; set; }
        public MaintenanceStatus Status { get; set; }
        public Guid? AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAssetMaintenanceDto
    {
        public Guid AssetId { get; set; }
        public string IssueDescription { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public PriorityLevel Priority { get; set; }
        public decimal? EstimatedCost { get; set; }
    }

    public class CompleteMaintenanceDto
    {
        public decimal? ActualCost { get; set; }
        public string Notes { get; set; }
    }
}

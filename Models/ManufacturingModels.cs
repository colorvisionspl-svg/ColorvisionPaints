using System;
using System.Collections.Generic;

namespace ColorvisionPaintsERP.Models
{
    /* ─────────────────────────────────────────────────
       ENUMS
    ───────────────────────────────────────────────── */
    public static class FormulationStatus
    {
        public const string Draft    = "Draft";
        public const string Active   = "Active";
        public const string Archived = "Archived";
    }

    public static class ProductionOrderStatus
    {
        public const string Planned    = "Planned";
        public const string Released   = "Released";
        public const string InProgress = "In-Progress";
        public const string QCPending  = "QC-Pending";
        public const string Completed  = "Completed";
        public const string Closed     = "Closed";
    }

    public static class BatchStatus
    {
        public const string Pending    = "Pending";
        public const string InProgress = "In-Progress";
        public const string QCPending  = "QC-Pending";
        public const string Passed     = "Passed";
        public const string Failed     = "Failed";
        public const string Rework     = "Rework";
    }

    public static class QCResult
    {
        public const string Pass        = "Pass";
        public const string Fail        = "Fail";
        public const string Conditional = "Conditional";
    }

    public static class LineStatus
    {
        public const string Active      = "Active";
        public const string Maintenance = "Maintenance";
        public const string Idle        = "Idle";
    }

    /* ─────────────────────────────────────────────────
       FORMULATION MODELS
    ───────────────────────────────────────────────── */
    public class FormulationIngredient
    {
        public string MaterialId        { get; set; } = "";
        public string MaterialName      { get; set; } = "";
        public int    Sequence          { get; set; }
        public double QuantityKg        { get; set; }
        public double PercentByWeight   { get; set; }
        public double ToleranceMin      { get; set; } = 2;
        public double ToleranceMax      { get; set; } = 2;
        public string Unit              { get; set; } = "kg";
        public string Notes             { get; set; } = "";
    }

    public class ProcessStep
    {
        public int    StepNumber         { get; set; }
        public string Title              { get; set; } = "";
        public string Instruction        { get; set; } = "";
        public int    DurationMinutes    { get; set; }
        public string TemperatureRange   { get; set; } = "";
        public string EquipmentRequired  { get; set; } = "";
    }

    public class Formulation
    {
        public string   Id                     { get; set; } = "";
        public string   FormulationCode        { get; set; } = "";
        public string   ProductId              { get; set; } = "";
        public string   ProductName            { get; set; } = "";
        public string   ShadeId                { get; set; } = "";
        public string   ShadeName              { get; set; } = "";
        public int      Version                { get; set; } = 1;
        public double   StandardBatchSizeKg    { get; set; } = 1000;
        public double   ExpectedYieldPercent   { get; set; } = 96;
        public double   ProcessingTimeHours    { get; set; }
        public string   Status                 { get; set; } = FormulationStatus.Draft;
        public string   ApprovedBy             { get; set; } = "";
        public DateTime ApprovedAt             { get; set; }
        public List<FormulationIngredient> Ingredients  { get; set; } = new();
        public List<ProcessStep>           ProcessSteps { get; set; } = new();
        public string   CreatedBy              { get; set; } = "";
        public DateTime CreatedAt              { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt              { get; set; } = DateTime.UtcNow;
    }

    /* ─────────────────────────────────────────────────
       PRODUCTION ORDER MODELS
    ───────────────────────────────────────────────── */
    public class ProductionOrder
    {
        public string   Id                   { get; set; } = "";
        public string   OrderNumber          { get; set; } = "";
        public string   ProductVariantId     { get; set; } = "";
        public string   ProductName          { get; set; } = "";
        public string   ShadeName            { get; set; } = "";
        public string   FormulationId        { get; set; } = "";
        public int      FormulationVersion   { get; set; } = 1;
        public double   PlannedQuantityKg    { get; set; }
        public int      PlannedBatches       { get; set; }
        public string   ProductionLineId     { get; set; } = "";
        public string   ProductionLineName   { get; set; } = "";
        public string   Status               { get; set; } = ProductionOrderStatus.Planned;
        public DateTime PlannedStartDate     { get; set; }
        public DateTime PlannedEndDate       { get; set; }
        public DateTime? ActualStartDate     { get; set; }
        public DateTime? ActualEndDate       { get; set; }
        public string   CreatedBy            { get; set; } = "";
        public DateTime CreatedAt            { get; set; } = DateTime.UtcNow;
    }

    /* ─────────────────────────────────────────────────
       PRODUCTION BATCH MODELS
    ───────────────────────────────────────────────── */
    public class ProductionBatch
    {
        public string   Id                { get; set; } = "";
        public string   ProductionOrderId { get; set; } = "";
        public string   BatchNumber       { get; set; } = "";
        public double   BatchSizeKg       { get; set; }
        public string   ProductionLineId  { get; set; } = "";
        public string   OperatorId        { get; set; } = "";
        public string   Status            { get; set; } = BatchStatus.Pending;
        public DateTime StartTime         { get; set; }
        public DateTime? EndTime          { get; set; }
        public double   YieldKg           { get; set; }
        public double   WasteKg           { get; set; }
        public string   ProductName       { get; set; } = "";
        public DateTime CreatedAt         { get; set; } = DateTime.UtcNow;
    }

    public class BatchConsumption
    {
        public string   Id                   { get; set; } = "";
        public string   BatchId              { get; set; } = "";
        public string   MaterialId           { get; set; } = "";
        public string   MaterialName         { get; set; } = "";
        public string   RawMaterialStockId   { get; set; } = "";
        public double   PlannedQty           { get; set; }
        public double   ActualQty            { get; set; }
        public double   VarianceQty          => ActualQty - PlannedQty;
        public string   Unit                 { get; set; } = "kg";
        public DateTime IssuedAt             { get; set; } = DateTime.UtcNow;
    }

    /* ─────────────────────────────────────────────────
       QC INSPECTION MODELS
    ───────────────────────────────────────────────── */
    public class QCTestResult
    {
        public string TestName       { get; set; } = "";
        public string TargetValue    { get; set; } = "";
        public double MinAcceptable  { get; set; }
        public double MaxAcceptable  { get; set; }
        public double? ActualValue   { get; set; }
        public string Unit           { get; set; } = "";
        public string Result         { get; set; } = "";
    }

    public class QCInspection
    {
        public string   Id                { get; set; } = "";
        public string   BatchId           { get; set; } = "";
        public string   ProductionOrderId { get; set; } = "";
        public string   BatchNumber       { get; set; } = "";
        public string   ProductName       { get; set; } = "";
        public string   ShadeName         { get; set; } = "";
        public double   YieldKg           { get; set; }
        public string   InspectorId       { get; set; } = "";
        public string   InspectorName     { get; set; } = "";
        public DateTime InspectionDate    { get; set; } = DateTime.UtcNow;
        public string   OverallResult     { get; set; } = "";
        public List<QCTestResult> TestResults { get; set; } = new();
        public string   Remarks           { get; set; } = "";
        public string   CoaUrl            { get; set; } = "";
        public DateTime CreatedAt         { get; set; } = DateTime.UtcNow;
    }

    /* ─────────────────────────────────────────────────
       PRODUCTION LINE MODEL
    ───────────────────────────────────────────────── */
    public class ProductionLine
    {
        public string Id              { get; set; } = "";
        public string Name            { get; set; } = "";
        public double CapacityKgPerDay { get; set; }
        public string Status          { get; set; } = LineStatus.Idle;
        public string CurrentBatchId  { get; set; } = "";
        public string Notes           { get; set; } = "";

        // Computed in view
        public int ProgressPercent    { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ColorvisionPaintsERP.Models
{
    /* ═══════════════════════════════════════════════════════════
       RAW MATERIALS
       ═══════════════════════════════════════════════════════════ */
    public class RawMaterial
    {
        public string Id { get; set; } = string.Empty;
        public string MaterialCode { get; set; } = string.Empty; // RM-0001
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Pigment / Resin / Solvent / Additive / Packaging
        public string Unit { get; set; } = string.Empty; // kg / L / drums / bags
        public double ReorderLevel { get; set; }
        public double SafetyStock { get; set; }
        public int ShelfLifeDays { get; set; }
        public string MsdsUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    /* ═══════════════════════════════════════════════════════════
       VENDORS
       ═══════════════════════════════════════════════════════════ */
    public class Vendor
    {
        public string Id { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty; // VND-0001
        public string CompanyName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string GstNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public List<string> MaterialsSupplied { get; set; } = new();
        public int PaymentTermsDays { get; set; }
        public int QualityRating { get; set; } // 1-5
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    /* ═══════════════════════════════════════════════════════════
       PURCHASE ORDERS
       ═══════════════════════════════════════════════════════════ */
    public class PurchaseOrder
    {
        public string Id { get; set; } = string.Empty;
        public string PoNumber { get; set; } = string.Empty; // CV/PO/2025-26/0001
        public string VendorId { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = POStatus.Draft;
        public List<OrderItem> Items { get; set; } = new();
        public double Subtotal { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderItem
    {
        public string MaterialId { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double UnitRate { get; set; }
        public double TaxPercent { get; set; }
        public double LineTotal { get; set; }
    }

    public static class POStatus
    {
        public const string Draft = "Draft";
        public const string Sent = "Sent";
        public const string Partial = "Partial";
        public const string Received = "Received";
        public const string Cancelled = "Cancelled";
    }

    /* ═══════════════════════════════════════════════════════════
       GRN (Goods Receipt Note)
       ═══════════════════════════════════════════════════════════ */
    public class GRN
    {
        public string Id { get; set; } = string.Empty;
        public string GrnNumber { get; set; } = string.Empty; // CV/GRN/2025-26/0001
        public string PoId { get; set; } = string.Empty;
        public string PoNumber { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public DateTime ReceivedDate { get; set; } = DateTime.Now;
        public List<GRNItem> Items { get; set; } = new();
        public string Status { get; set; } = GRNStatus.QCPending;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class GRNItem
    {
        public string MaterialId { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public double OrderedQty { get; set; }
        public double ReceivedQty { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public static class GRNStatus
    {
        public const string QCPending = "QC Pending";
        public const string QCApproved = "QC Approved";
        public const string QCRejected = "QC Rejected";
    }

    /* ═══════════════════════════════════════════════════════════
       PURCHASE BILLS
       ═══════════════════════════════════════════════════════════ */
    public class PurchaseBill
    {
        public string Id { get; set; } = string.Empty;
        public string BillNumber { get; set; } = string.Empty;
        public string GrnId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public string VendorInvoiceNumber { get; set; } = string.Empty;
        public DateTime VendorInvoiceDate { get; set; }
        public DateTime BillDate { get; set; } = DateTime.Now;
        public List<BillItem> Items { get; set; } = new();
        public double Subtotal { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
        public string MatchStatus { get; set; } = "Pending"; // Matched / Mismatch / Pending
        public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid / Partial / Paid
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class BillItem
    {
        public string MaterialId { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double UnitRate { get; set; }
        public double TaxPercent { get; set; }
        public double LineTotal { get; set; }
    }

    /* ═══════════════════════════════════════════════════════════
       RAW MATERIAL STOCK
       ═══════════════════════════════════════════════════════════ */
    public class RawMaterialStock
    {
        public string Id { get; set; } = string.Empty;
        public string MaterialId { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ReceivedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double Quantity { get; set; }
        public double Available { get; set; }
        public double ReservedQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public double CostPerUnit { get; set; }
        public string WarehouseZone { get; set; } = string.Empty; // Raw Material / Hazardous / Packaging
        public string Status { get; set; } = StockStatus.Quarantine; // Quarantine / Approved / In-Use / Expired
        public string GrnId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public static class StockStatus
    {
        public const string Quarantine = "Quarantine";
        public const string Approved = "Approved";
        public const string InUse = "In-Use";
        public const string Expired = "Expired";
    }
}

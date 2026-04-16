using System;
using System.Collections.Generic;

namespace ColorvisionPaintsERP.Models
{
    /* ── ENUMS ── */
    public static class QRStatus
    {
        public const string Unscanned = "Unscanned";
        public const string Scanned   = "Scanned";
        public const string Expired   = "Expired";
        public const string Voided    = "Voided";
    }

    public static class PainterTier
    {
        public const string Bronze   = "Bronze";
        public const string Silver   = "Silver";
        public const string Gold     = "Gold";
        public const string Platinum = "Platinum";

        public static string FromPoints(double pts) => pts switch {
            >= 35000 => Platinum,
            >= 15000 => Gold,
            >= 5000  => Silver,
            _        => Bronze
        };
    }

    public static class TxnType   { public const string Earn = "earn"; public const string Redeem = "redeem"; }
    public static class TxnStatus { public const string Pending = "Pending"; public const string Processing = "Processing"; public const string Completed = "Completed"; public const string Failed = "Failed"; }
    public static class FraudType { public const string HighVelocity = "high_velocity"; public const string GeoAnomaly = "geo_anomaly"; public const string BulkScan = "bulk_scan"; public const string DuplicateDevice = "duplicate_device"; }
    public static class FlagStatus { public const string Pending = "Pending"; public const string Reviewed = "Reviewed"; public const string Dismissed = "Dismissed"; public const string Blocked = "Blocked"; }

    /* ─────────────────────────────────────────────────
       QR CODE
    ───────────────────────────────────────────────── */
    public class QrCode
    {
        public string   SerialNumber        { get; set; } = "";
        public string   ProductVariantId    { get; set; } = "";
        public string   ProductName         { get; set; } = "";
        public string   BatchNumber         { get; set; } = "";
        public DateTime ManufacturingDate   { get; set; }
        public int      PointValue          { get; set; }
        public string   Status              { get; set; } = QRStatus.Unscanned;
        public string   ScannedByPainterId  { get; set; } = "";
        public DateTime? ScannedAt          { get; set; }
        public double?  ScanGpsLat          { get; set; }
        public double?  ScanGpsLng          { get; set; }
        public string   ScanCity            { get; set; } = "";
        public string   ScanState           { get; set; } = "";
        public string   ScanPincode         { get; set; } = "";
        public DateTime CreatedAt           { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate          { get; set; }
    }

    /* ─────────────────────────────────────────────────
       PAINTER
    ───────────────────────────────────────────────── */
    public class Painter
    {
        public string   Id                  { get; set; } = "";
        public string   Mobile              { get; set; } = "";
        public string   Name                { get; set; } = "";
        public string   City                { get; set; } = "";
        public string   State               { get; set; } = "";
        public string   AadhaarLast4        { get; set; } = "";
        public string   UpiId               { get; set; } = "";
        public string   Tier                { get; set; } = PainterTier.Bronze;
        public double   TotalPointsEarned   { get; set; }
        public double   TotalPointsRedeemed { get; set; }
        public double   CurrentPoints       { get; set; }
        public DateTime RegisteredAt        { get; set; } = DateTime.UtcNow;
        public DateTime? LastScanAt         { get; set; }
        public bool     IsVerified          { get; set; }
        public bool     IsBlocked           { get; set; }
        public string   BlockReason         { get; set; } = "";
    }

    /* ─────────────────────────────────────────────────
       PAINTER TRANSACTION
    ───────────────────────────────────────────────── */
    public class PainterTransaction
    {
        public string   Id                { get; set; } = "";
        public string   PainterId         { get; set; } = "";
        public string   PainterName       { get; set; } = "";
        public string   Type              { get; set; } = TxnType.Earn;
        public double   Points            { get; set; }
        public double   Amount            { get; set; }
        public string   QrCodeSerial      { get; set; } = "";
        public string   ProductName       { get; set; } = "";
        public string   RedemptionType    { get; set; } = "";
        public string   UpiId             { get; set; } = "";
        public string   UpiTransactionId  { get; set; } = "";
        public double   TdsDeducted       { get; set; }
        public string   Status            { get; set; } = TxnStatus.Pending;
        public DateTime CreatedAt         { get; set; } = DateTime.UtcNow;
    }

    /* ─────────────────────────────────────────────────
       REWARD CAMPAIGN
    ───────────────────────────────────────────────── */
    public class RewardCampaign
    {
        public string   Id                { get; set; } = "";
        public string   Name              { get; set; } = "";
        public string   Description       { get; set; } = "";
        public DateTime StartDate         { get; set; }
        public DateTime EndDate           { get; set; }
        public List<string> TargetProductIds { get; set; } = new();
        public List<string> TargetCities  { get; set; } = new();
        public double   BonusMultiplier   { get; set; } = 1;
        public int      BonusPoints       { get; set; }
        public bool     IsActive          { get; set; }
        public DateTime CreatedAt         { get; set; } = DateTime.UtcNow;
    }

    /* ─────────────────────────────────────────────────
       FRAUD FLAG
    ───────────────────────────────────────────────── */
    public class FraudFlag
    {
        public string   Id                { get; set; } = "";
        public string   Type              { get; set; } = "";
        public string   PainterId         { get; set; } = "";
        public string   PainterName       { get; set; } = "";
        public string   DeviceFingerprint { get; set; } = "";
        public int      ScanCount         { get; set; }
        public string   TimeWindow        { get; set; } = "";
        public string   GpsCoordinates    { get; set; } = "";
        public DateTime FlaggedAt         { get; set; } = DateTime.UtcNow;
        public string   Status            { get; set; } = FlagStatus.Pending;
        public string   ReviewedBy        { get; set; } = "";
    }

    /* ─────────────────────────────────────────────────
       VIEW MODELS
    ───────────────────────────────────────────────── */
    public class QRGenerationSummary
    {
        public int    TotalGenerated    { get; set; }
        public int    ScannedThisMonth  { get; set; }
        public double ScanRatePercent   { get; set; }
        public int    PointsIssuedMtd   { get; set; }
    }
}

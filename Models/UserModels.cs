namespace ColorvisionPaintsERP.Models
{
    /// <summary>
    /// Represents a user in the ColorVision Paints ERP system.
    /// Maps to Firestore: users/{uid}
    /// </summary>
    public class UserProfile
    {
        public string Uid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Role { get; set; } = "DealerPortalUser";
        public string TerritoryId { get; set; } = string.Empty;
        public string ReportingTo { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Available roles in the ERP system
    /// </summary>
    public static class UserRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string FinanceManager = "FinanceManager";
        public const string SalesHead = "SalesHead";
        public const string AreaManager = "AreaManager";
        public const string SalesRep = "SalesRep";
        public const string WarehouseManager = "WarehouseManager";
        public const string DealerPortalUser = "DealerPortalUser";

        public static readonly string[] All = new[]
        {
            SuperAdmin, FinanceManager, SalesHead, AreaManager,
            SalesRep, WarehouseManager, DealerPortalUser
        };
    }

    /// <summary>
    /// Module permission set
    /// </summary>
    public class ModulePermission
    {
        public bool View { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Approve { get; set; }
    }

    /// <summary>
    /// Role permissions - maps to Firestore: roles_permissions/{role}
    /// </summary>
    public class RolePermissions
    {
        public ModulePermission Dealers { get; set; } = new();
        public ModulePermission Sales { get; set; } = new();
        public ModulePermission Inventory { get; set; } = new();
        public ModulePermission Manufacturing { get; set; } = new();
        public ModulePermission Rewards { get; set; } = new();
        public ModulePermission Settings { get; set; } = new();
    }

    // --- View Models ---
    public class EmailLoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class PhoneLoginViewModel
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class OtpVerifyViewModel
    {
        public string Code { get; set; } = string.Empty;
        public string SessionInfo { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    // --- Firebase API Response Models ---
    public class FirebaseAuthResponse
    {
        public string IdToken { get; set; } = string.Empty;
        public string LocalId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ExpiresIn { get; set; } = string.Empty;
    }

    public class FirebaseOtpResponse
    {
        public string SessionInfo { get; set; } = string.Empty;
    }

    public class FirebaseErrorResponse
    {
        public FirebaseError Error { get; set; } = new();
    }

    public class FirebaseError
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

namespace IzmPortal.Domain.Enums;

public enum AuditAction
{
    Create,
    Update,
    Delete,

    Activate,
    Deactivate,

    ChangeRole,
    ResetPassword,

    Login,
    Logout,
    ChangePassword
}


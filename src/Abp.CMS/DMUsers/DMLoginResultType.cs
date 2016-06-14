namespace Abp.DMUsers
{
    public enum DMLoginResultType : byte
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,
        
        InvalidPassword,
        
        UserIsNotActive,

        InvalidTenancyName,
        
        TenantIsNotActive,

        UserEmailIsNotConfirmed,

        UnknownExternalLogin
    }
}
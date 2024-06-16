namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantBaseObjects;

public class UsersCreate
{
    public class Root
    {
        public User User { get; set; }
        public int MerchantId { get; set; }
        public int BusinessUnitId { get; set; }
        public List<string> Roles { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string UniqueId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CultureName { get; set; }
        public string RoleName { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDataRestricted { get; set; }
        public bool IsDataPrivateRestricted { get; set; }
    }
}
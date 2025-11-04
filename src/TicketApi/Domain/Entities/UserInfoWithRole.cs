using CoreLib.Common;

namespace Domain.Entities
{
    public class UserInfoWithRole
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public UserRoles role { get; set; }
    }
}

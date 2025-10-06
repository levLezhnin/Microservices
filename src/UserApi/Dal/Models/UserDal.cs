using CoreLib.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Dal.Models
{
    [Table("users")]
    public record UserDal : BaseEntityDal<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; init; }
        public required string PasswordHash { get; init; }

        public required string Role { get; init; }
        [ForeignKey("Role")]
        public UserRoleDal UserRole { get; init; }
        
        [DataType(DataType.DateTime)]
        public required DateTime CreatedAt { get; init; }
        
        public required bool IsActive { get; set; }
    }
}

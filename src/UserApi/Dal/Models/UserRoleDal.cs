using CoreLib.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Dal.Models
{
    [Table("user_roles")]
    public record UserRoleDal : BaseEntityDal<Guid>
    {
        [Key]
        public required string Role { get; init; }
    }
}

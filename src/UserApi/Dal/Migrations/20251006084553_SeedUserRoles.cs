using Microsoft.EntityFrameworkCore.Migrations;
using UserApi.Dal.Models;

#nullable disable

namespace UserApi.Dal.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var roles = Enum.GetValues(typeof(ExistingRoles))
                .Cast<ExistingRoles>()
                .ToArray();
            object[,] vals = new object[roles.Length, 2];

            for (int i = 0; i < roles.Length; ++i)
            {
                vals[i, 0] = Guid.NewGuid();
                vals[i, 1] = roles[i].ToString();
            }

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "Id", "Role" },
                values: vals
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            object[] keyValues = Enum.GetValues(typeof(ExistingRoles))
                .Cast<ExistingRoles>()
                .Select(role => role.ToString())
                .ToArray();

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "Role",
                keyValues: keyValues
            );
        }
    }
}

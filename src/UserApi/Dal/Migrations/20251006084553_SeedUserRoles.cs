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
            var values = new List<object[]>();
            foreach (ExistingRoles role in Enum.GetValues(typeof(ExistingRoles)))
            {
                values.Add(new object[]{ role.ToString(), Guid.NewGuid() });
            }

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "Role", "Id" },
                values: values.ToArray()
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "Role",
                keyValues: ["User", "Support", "Admin"]
            );
        }
    }
}

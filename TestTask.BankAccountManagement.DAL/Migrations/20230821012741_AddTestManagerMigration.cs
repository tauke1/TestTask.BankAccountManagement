using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.BankAccountManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTestManagerMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO Managers (Login, PinHashed) VALUES('test', '$2a$11$OG1Ay3GXNwviEcsSQBiTae/ZhU5QMXVRDvzQzqgw2mUA3zJP9fvV6')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Managers WHERE Login = 'test'");
        }
    }
}

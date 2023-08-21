using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.BankAccountManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountTypeSettingsDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO AccountTypeSettings (AccountType, MaxTransactionsPerDay)
                                       VALUES
                                           (0, 20),
                                           (1, 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AccountTypeSettings WHERE AccountType IN (0, 1)");
        }
    }
}

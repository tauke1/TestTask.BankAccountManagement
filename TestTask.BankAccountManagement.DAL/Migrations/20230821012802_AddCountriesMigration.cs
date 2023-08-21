using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.BankAccountManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCountriesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Countries (Name, Code) 
                VALUES 
                ('England', 'gb'),
                ('Kazakhstan', 'kz')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Countries WHERE Code IN ('gb', 'kz')");
        }
    }
}

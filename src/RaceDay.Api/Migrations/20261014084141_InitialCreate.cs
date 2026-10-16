using Microsoft.EntityFrameworkCore.Migrations;

namespace RaceDay.Api.Migrations;
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        throw new InvalidOperationException("The Part 1 baseline cannot be reversed because its tables and data predate this migration.");
    }
}

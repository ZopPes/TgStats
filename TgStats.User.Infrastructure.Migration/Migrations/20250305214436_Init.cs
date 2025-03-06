using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgStats.User.Infrastructure.Migrations.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserEntity",
            columns: table => new
            {
                ID = table.Column<Guid>(type: "uuid", nullable: false),
                Password = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_UserEntity", x => x.ID));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserEntity");
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberVault.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCableTypesAndFibers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CableTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FiberCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CableTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fibers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CableId = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fibers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fibers_Cables_CableId",
                        column: x => x.CableId,
                        principalTable: "Cables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CableTypes_Name",
                table: "CableTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fibers_CableId_Number",
                table: "Fibers",
                columns: new[] { "CableId", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CableTypes");

            migrationBuilder.DropTable(
                name: "Fibers");
        }
    }
}

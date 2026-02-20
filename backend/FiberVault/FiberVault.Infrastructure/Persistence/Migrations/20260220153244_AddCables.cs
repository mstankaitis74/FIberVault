using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FiberVault.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FromNodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToNodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<LineString>(type: "geometry(LineString,4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cables_Nodes_FromNodeId",
                        column: x => x.FromNodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cables_Nodes_ToNodeId",
                        column: x => x.ToNodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cables_FromNodeId",
                table: "Cables",
                column: "FromNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cables_Path",
                table: "Cables",
                column: "Path")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_Cables_ToNodeId",
                table: "Cables",
                column: "ToNodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cables");
        }
    }
}

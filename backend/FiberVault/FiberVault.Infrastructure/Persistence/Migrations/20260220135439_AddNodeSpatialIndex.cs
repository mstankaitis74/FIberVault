using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberVault.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNodeSpatialIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Nodes_Location",
                table: "Nodes",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "GIST");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nodes_Location",
                table: "Nodes");
        }
    }
}

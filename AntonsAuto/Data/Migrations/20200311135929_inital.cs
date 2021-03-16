using Microsoft.EntityFrameworkCore.Migrations;

namespace AntonsAuto.Data.Migrations
{
    public partial class inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bilfabrikant",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bilfabrikant", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BilModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(nullable: true),
                    AntalKilometer = table.Column<int>(nullable: false),
                    BilfabrikantID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BilModel_Bilfabrikant_BilfabrikantID",
                        column: x => x.BilfabrikantID,
                        principalTable: "Bilfabrikant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BilModel_BilfabrikantID",
                table: "BilModel",
                column: "BilfabrikantID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BilModel");

            migrationBuilder.DropTable(
                name: "Bilfabrikant");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimentosManual.Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COSIF",
                schema: "dbo",
                columns: table => new
                {
                    COD_COSIF = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DES_COSIF = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    STA_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COSIF", x => x.COD_COSIF);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO",
                schema: "dbo",
                columns: table => new
                {
                    COD_PRODUTO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DES_PRODUTO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    STA_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.COD_PRODUTO);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_COSIF",
                schema: "dbo",
                columns: table => new
                {
                    COD_PRODUTO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    COD_COSIF = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    COD_CLASSIFICACAO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STA_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_COSIF", x => new { x.COD_PRODUTO, x.COD_COSIF });
                    table.ForeignKey(
                        name: "FK_PRODUTO_COSIF_COSIF_COD_COSIF",
                        column: x => x.COD_COSIF,
                        principalSchema: "dbo",
                        principalTable: "COSIF",
                        principalColumn: "COD_COSIF",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUTO_COSIF_PRODUTO_COD_PRODUTO",
                        column: x => x.COD_PRODUTO,
                        principalSchema: "dbo",
                        principalTable: "PRODUTO",
                        principalColumn: "COD_PRODUTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MOVIMENTO_MANUAL",
                schema: "dbo",
                columns: table => new
                {
                    DAT_MES = table.Column<int>(type: "int", nullable: false),
                    DAT_ANO = table.Column<int>(type: "int", nullable: false),
                    NUM_LANCAMENTO = table.Column<long>(type: "bigint", nullable: false),
                    COD_PRODUTO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    COD_COSIF = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DES_DESCRICAO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DAT_MOVIMENTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COD_USUARIO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    VAL_VALOR = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOVIMENTO_MANUAL", x => new { x.DAT_MES, x.DAT_ANO, x.NUM_LANCAMENTO });
                    table.ForeignKey(
                        name: "FK_MOVIMENTO_MANUAL_COSIF_COD_COSIF",
                        column: x => x.COD_COSIF,
                        principalSchema: "dbo",
                        principalTable: "COSIF",
                        principalColumn: "COD_COSIF",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MOVIMENTO_MANUAL_PRODUTO_COD_PRODUTO",
                        column: x => x.COD_PRODUTO,
                        principalSchema: "dbo",
                        principalTable: "PRODUTO",
                        principalColumn: "COD_PRODUTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_COSIF_COD_COSIF",
                schema: "dbo",
                table: "PRODUTO_COSIF",
                column: "COD_COSIF");

            migrationBuilder.CreateIndex(
                name: "IX_MOVIMENTO_MANUAL_COD_COSIF",
                schema: "dbo",
                table: "MOVIMENTO_MANUAL",
                column: "COD_COSIF");

            migrationBuilder.CreateIndex(
                name: "IX_MOVIMENTO_MANUAL_COD_PRODUTO",
                schema: "dbo",
                table: "MOVIMENTO_MANUAL",
                column: "COD_PRODUTO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MOVIMENTO_MANUAL", schema: "dbo");
            migrationBuilder.DropTable(name: "PRODUTO_COSIF", schema: "dbo");
            migrationBuilder.DropTable(name: "COSIF", schema: "dbo");
            migrationBuilder.DropTable(name: "PRODUTO", schema: "dbo");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimentosManual.Infrastructure.Migrations
{
    public partial class AtualizarMovimentoManualMapeamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VAL_VALOR",
                schema: "dbo",
                table: "MOVIMENTO_MANUAL",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VAL_VALOR",
                schema: "dbo",
                table: "MOVIMENTO_MANUAL",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}

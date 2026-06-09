using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "InvoiceLines");

            migrationBuilder.RenameColumn(
                name: "MyDataUid",
                table: "Invoices",
                newName: "MyDataError");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "Invoices",
                newName: "Series");

            migrationBuilder.DropColumn(
                name: "MyDataMark",
                table: "Invoices");

            migrationBuilder.AddColumn<long>(
                name: "MyDataMark",
                table: "Invoices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "IssueDate",
                table: "Invoices",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Aa",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceType",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "InvoiceLines",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "LineNumber",
                table: "InvoiceLines",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aa",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "LineNumber",
                table: "InvoiceLines");

            migrationBuilder.RenameColumn(
                name: "Series",
                table: "Invoices",
                newName: "InvoiceNumber");

            migrationBuilder.RenameColumn(
                name: "MyDataError",
                table: "Invoices",
                newName: "MyDataUid");

            migrationBuilder.AlterColumn<string>(
                name: "MyDataMark",
                table: "Invoices",
                type: "text",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssueDate",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "InvoiceLines",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "VatRate",
                table: "InvoiceLines",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentApi.DataAccess.Migrations
{
	public partial class InitialSetup : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Accounts",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(nullable: false),
					CreationDate = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Accounts", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Transactions",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					AccountId = table.Column<int>(nullable: false),
					Date = table.Column<DateTime>(nullable: false),
					Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
					TransactionType = table.Column<int>(nullable: false),
					TransactionStatus = table.Column<int>(nullable: false),
					CreationDate = table.Column<DateTime>(nullable: false),
					LastUpdateDate = table.Column<DateTime>(nullable: false),
					ClosedReason = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Transactions", x => x.Id);
					table.ForeignKey(
						name: "FK_Transactions_Accounts_AccountId",
						column: x => x.AccountId,
						principalTable: "Accounts",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Transactions_AccountId",
				table: "Transactions",
				column: "AccountId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Transactions");

			migrationBuilder.DropTable(
				name: "Accounts");
		}
	}
}
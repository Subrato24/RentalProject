using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserProject_1.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustDetail",
                columns: table => new
                {
                    CustomerDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deposit = table.Column<int>(type: "int", nullable: false),
                    JoiningDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProof = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustDetail", x => x.CustomerDetailId);
                });

            migrationBuilder.CreateTable(
                name: "CustStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rent = table.Column<double>(type: "float", nullable: false),
                    Ebill = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Due = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustStatus_CustDetail_CustomerDetailId",
                        column: x => x.CustomerDetailId,
                        principalTable: "CustDetail",
                        principalColumn: "CustomerDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustStatus_CustomerDetailId",
                table: "CustStatus",
                column: "CustomerDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustStatus");

            migrationBuilder.DropTable(
                name: "CustDetail");
        }
    }
}

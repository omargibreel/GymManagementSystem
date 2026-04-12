using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHealthRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberShips_Members_MemberId",
                table: "MemberShips");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShips_Plans_PlanId",
                table: "MemberShips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips");

            migrationBuilder.RenameTable(
                name: "MemberShips",
                newName: "Memberships");

            migrationBuilder.RenameIndex(
                name: "IX_MemberShips_PlanId",
                table: "Memberships",
                newName: "IX_Memberships_PlanId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Members",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Members",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships",
                columns: new[] { "MemberId", "PlanId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Members_MemberId",
                table: "Memberships",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Members_MemberId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships");

            migrationBuilder.RenameTable(
                name: "Memberships",
                newName: "MemberShips");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_PlanId",
                table: "MemberShips",
                newName: "IX_MemberShips_PlanId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Members",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Members",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips",
                columns: new[] { "MemberId", "PlanId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShips_Members_MemberId",
                table: "MemberShips",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShips_Plans_PlanId",
                table: "MemberShips",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkspacePromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_HostProfiles_HostId",
                table: "Promotions");

            migrationBuilder.CreateTable(
                name: "WorkSpacePromotions",
                columns: table => new
                {
                    WorkSpaceId = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpacePromotions", x => new { x.WorkSpaceId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_WorkSpacePromotions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkSpacePromotions_WorkSpaces_WorkSpaceId",
                        column: x => x.WorkSpaceId,
                        principalTable: "WorkSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpacePromotions_PromotionId",
                table: "WorkSpacePromotions",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_HostProfiles_HostId",
                table: "Promotions",
                column: "HostId",
                principalTable: "HostProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_HostProfiles_HostId",
                table: "Promotions");

            migrationBuilder.DropTable(
                name: "WorkSpacePromotions");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_HostProfiles_HostId",
                table: "Promotions",
                column: "HostId",
                principalTable: "HostProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

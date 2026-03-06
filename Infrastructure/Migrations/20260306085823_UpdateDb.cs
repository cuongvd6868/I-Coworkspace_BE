using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HostProfile_AspNetUsers_UserId",
                table: "HostProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_HostProfile_HostId",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaces_HostProfile_HostId",
                table: "WorkSpaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HostProfile",
                table: "HostProfile");

            migrationBuilder.RenameTable(
                name: "HostProfile",
                newName: "HostProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_HostProfile_UserId",
                table: "HostProfiles",
                newName: "IX_HostProfiles_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WorkSpaceId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HostProfiles",
                table: "HostProfiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_WorkSpaceId",
                table: "Notifications",
                column: "WorkSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_HostProfiles_AspNetUsers_UserId",
                table: "HostProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_WorkSpaces_WorkSpaceId",
                table: "Notifications",
                column: "WorkSpaceId",
                principalTable: "WorkSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_HostProfiles_HostId",
                table: "Promotions",
                column: "HostId",
                principalTable: "HostProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaces_HostProfiles_HostId",
                table: "WorkSpaces",
                column: "HostId",
                principalTable: "HostProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HostProfiles_AspNetUsers_UserId",
                table: "HostProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_WorkSpaces_WorkSpaceId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_HostProfiles_HostId",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaces_HostProfiles_HostId",
                table: "WorkSpaces");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_WorkSpaceId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HostProfiles",
                table: "HostProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "WorkSpaceId",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "HostProfiles",
                newName: "HostProfile");

            migrationBuilder.RenameIndex(
                name: "IX_HostProfiles_UserId",
                table: "HostProfile",
                newName: "IX_HostProfile_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HostProfile",
                table: "HostProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HostProfile_AspNetUsers_UserId",
                table: "HostProfile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_HostProfile_HostId",
                table: "Promotions",
                column: "HostId",
                principalTable: "HostProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaces_HostProfile_HostId",
                table: "WorkSpaces",
                column: "HostId",
                principalTable: "HostProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeAndAddSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_WorkSpaceRooms_WorkSpaceRoomId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_AspNetUsers_SubmittedByUserId",
                table: "SupportTickets");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "SupportTickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SupportTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SupportTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SupportTicketReplies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_AppUserId",
                table: "SupportTickets",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_WorkSpaceRooms_WorkSpaceRoomId",
                table: "Bookings",
                column: "WorkSpaceRoomId",
                principalTable: "WorkSpaceRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_AspNetUsers_AppUserId",
                table: "SupportTickets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_AspNetUsers_SubmittedByUserId",
                table: "SupportTickets",
                column: "SubmittedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_WorkSpaceRooms_WorkSpaceRoomId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_AspNetUsers_AppUserId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_AspNetUsers_SubmittedByUserId",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_AppUserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SupportTicketReplies");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_WorkSpaceRooms_WorkSpaceRoomId",
                table: "Bookings",
                column: "WorkSpaceRoomId",
                principalTable: "WorkSpaceRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_AspNetUsers_SubmittedByUserId",
                table: "SupportTickets",
                column: "SubmittedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

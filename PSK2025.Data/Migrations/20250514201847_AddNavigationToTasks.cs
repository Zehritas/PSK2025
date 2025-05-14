using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSK2025.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_Projectid",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "Projectid",
                table: "Tasks",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_Projectid",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Tasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId1",
                table: "Tasks",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId1",
                table: "Tasks",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId1",
                table: "Tasks",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId1",
                table: "Tasks",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId1",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId1",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ProjectId1",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserId1",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Tasks",
                newName: "Projectid");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_Projectid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_Projectid",
                table: "Tasks",
                column: "Projectid",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionInventaire.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvitationTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvitationTokens",
                columns: table => new
                {
                    IdInvitation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUtilisateur = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUtilisation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Utilisee = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationTokens", x => x.IdInvitation);
                    table.ForeignKey(
                        name: "FK_InvitationTokens_AspNetUsers_IdUtilisateur",
                        column: x => x.IdUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvitationTokens_Email",
                table: "InvitationTokens",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationTokens_IdUtilisateur",
                table: "InvitationTokens",
                column: "IdUtilisateur",
                unique: true,
                filter: "[IdUtilisateur] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationTokens_Token",
                table: "InvitationTokens",
                column: "Token",
                unique: true);
        }
    }
}

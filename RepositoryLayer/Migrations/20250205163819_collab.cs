using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class collab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollaboratorEntity",
                columns: table => new
                {
                    CollaboratorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    NotesId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CollaboratorNotesNotesId = table.Column<int>(nullable: true),
                    CollaboratorUsersUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorEntity", x => x.CollaboratorId);
                    table.ForeignKey(
                        name: "FK_CollaboratorEntity_NotesEntity_CollaboratorNotesNotesId",
                        column: x => x.CollaboratorNotesNotesId,
                        principalTable: "NotesEntity",
                        principalColumn: "NotesId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CollaboratorEntity_Users_CollaboratorUsersUserId",
                        column: x => x.CollaboratorUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorEntity_CollaboratorNotesNotesId",
                table: "CollaboratorEntity",
                column: "CollaboratorNotesNotesId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorEntity_CollaboratorUsersUserId",
                table: "CollaboratorEntity",
                column: "CollaboratorUsersUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaboratorEntity");
        }
    }
}

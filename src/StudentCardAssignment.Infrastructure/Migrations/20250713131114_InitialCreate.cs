using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentCardAssignment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardReadModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    MaskedCardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false),
                    AssignedStudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedStudentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedStudentEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false),
                    DaysUntilExpiry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardReadModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccurredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentReadModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HasAssignedCard = table.Column<bool>(type: "bit", nullable: false),
                    AssignedCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedCardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentReadModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_AssignedStudentId",
                table: "CardReadModels",
                column: "AssignedStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_CardId",
                table: "CardReadModels",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_CardNumber",
                table: "CardReadModels",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_CardType",
                table: "CardReadModels",
                column: "CardType");

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_ExpiresAt",
                table: "CardReadModels",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_IsActive",
                table: "CardReadModels",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_IsAssigned",
                table: "CardReadModels",
                column: "IsAssigned");

            migrationBuilder.CreateIndex(
                name: "IX_CardReadModels_Status",
                table: "CardReadModels",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EventStore_AggregateId",
                table: "EventStore",
                column: "AggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_EventStore_AggregateId_Version",
                table: "EventStore",
                columns: new[] { "AggregateId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventStore_CreatedAt",
                table: "EventStore",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EventStore_EventType",
                table: "EventStore",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_StudentReadModels_Email",
                table: "StudentReadModels",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentReadModels_HasAssignedCard",
                table: "StudentReadModels",
                column: "HasAssignedCard");

            migrationBuilder.CreateIndex(
                name: "IX_StudentReadModels_IsActive",
                table: "StudentReadModels",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_StudentReadModels_LastName_FirstName",
                table: "StudentReadModels",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentReadModels_Status",
                table: "StudentReadModels",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StudentReadModels_StudentId",
                table: "StudentReadModels",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentReadModels_StudentNumber",
                table: "StudentReadModels",
                column: "StudentNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardReadModels");

            migrationBuilder.DropTable(
                name: "EventStore");

            migrationBuilder.DropTable(
                name: "StudentReadModels");
        }
    }
}

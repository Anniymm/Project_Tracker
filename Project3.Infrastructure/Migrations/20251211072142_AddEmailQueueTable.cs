using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailQueueTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_working_hours_service_providers_ProviderId",
                table: "working_hours");

            migrationBuilder.EnsureSchema(
                name: "logging");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "working_hours",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "working_hours",
                newName: "provider_id");

            migrationBuilder.RenameColumn(
                name: "DayOfWeek",
                table: "working_hours",
                newName: "day_of_week");
            migrationBuilder.RenameIndex(
                name: "IX_working_hours_ProviderId",
                table: "working_hours",
                newName: "IX_working_hours_provider_id");

            migrationBuilder.CreateTable(
                name: "blocked_times",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocked_times", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blocked_times_service_providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "service_providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "email_queue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EmailNotificationType = table.Column<int>(type: "integer", nullable: false),
                    ScheduledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    SentAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_queue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_email_queue_appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                schema: "logging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FailureReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationLogs_appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_working_hours_start_before_end",
                table: "working_hours",
                sql: "\"start_time\" < \"end_time\"");

            migrationBuilder.CreateIndex(
                name: "IX_blocked_times_ProviderId",
                table: "blocked_times",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_email_queue_AppointmentId",
                table: "email_queue",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_AppointmentId",
                schema: "logging",
                table: "NotificationLogs",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_working_hours_service_providers_provider_id",
                table: "working_hours",
                column: "provider_id",
                principalTable: "service_providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_working_hours_service_providers_provider_id",
                table: "working_hours");

            migrationBuilder.DropTable(
                name: "blocked_times");

            migrationBuilder.DropTable(
                name: "email_queue");

            migrationBuilder.DropTable(
                name: "NotificationLogs",
                schema: "logging");

            migrationBuilder.DropCheckConstraint(
                name: "CK_working_hours_start_before_end",
                table: "working_hours");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "working_hours",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "provider_id",
                table: "working_hours",
                newName: "ProviderId");

            migrationBuilder.RenameColumn(
                name: "day_of_week",
                table: "working_hours",
                newName: "DayOfWeek");

            migrationBuilder.RenameIndex(
                name: "IX_working_hours_provider_id",
                table: "working_hours",
                newName: "IX_working_hours_ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_working_hours_service_providers_ProviderId",
                table: "working_hours",
                column: "ProviderId",
                principalTable: "service_providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

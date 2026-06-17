using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedMockUsers : Migration
    {
        private static readonly string[] UserColumns =
        [
            "Id",
            "Name",
            "IsActive",
            "CreatedAt",
            "UpdatedAt",
            "LastActivityAt",
            "UserName",
            "NormalizedUserName",
            "Email",
            "NormalizedEmail",
            "EmailConfirmed",
            "PasswordHash",
            "SecurityStamp",
            "ConcurrencyStamp",
            "PhoneNumberConfirmed",
            "TwoFactorEnabled",
            "LockoutEnabled",
            "AccessFailedCount",
            "LockoutEnd",
        ];

        private static readonly string[] UserIds =
        [
            "3d7b96ab-1359-4f9c-99ae-dde353357171",
            "d649d8e4-43de-4568-8732-d29873080923",
            "aef8f992-d0fd-411d-aaa5-7eb543ec9fd5",
            "b9583418-b558-4dd2-9144-ec8c740a7dce",
            "e22bff0a-4c80-4a58-9ef1-a62ba88cf014",
            "469e249a-267f-438f-8191-953ca44e075a",
            "65513b1f-78b3-4fa0-a275-db0799c03c05",
            "12e1cffc-bda7-4241-ae49-c609c56ce02a",
            "51109894-7671-499f-b79e-c69c373034e8",
            "f28192b2-b278-43f1-86da-e220541b9021",
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createdAt = new DateTime(2026, 5, 1, 10, 0, 0, DateTimeKind.Utc);
            var updatedAt = new DateTime(2026, 6, 1, 10, 0, 0, DateTimeKind.Utc);
            var blockedUntil = new DateTimeOffset(2099, 12, 31, 23, 59, 59, TimeSpan.Zero);

            InsertUser(migrationBuilder, UserIds[0], "Alice Johnson", "alice@example.com", emailConfirmed: true, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-1));
            InsertUser(migrationBuilder, UserIds[1], "Bob Smith", "bob@example.com", emailConfirmed: true, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-2));
            InsertUser(migrationBuilder, UserIds[2], "Carol White", "carol@example.com", emailConfirmed: false, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-3));
            InsertUser(migrationBuilder, UserIds[3], "David Brown", "david@example.com", emailConfirmed: false, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-4));
            InsertUser(migrationBuilder, UserIds[4], "Emma Davis", "emma@example.com", emailConfirmed: true, isActive: false, createdAt, updatedAt, updatedAt.AddDays(-5), blockedUntil);
            InsertUser(migrationBuilder, UserIds[5], "Frank Miller", "frank@example.com", emailConfirmed: true, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-6));
            InsertUser(migrationBuilder, UserIds[6], "Grace Lee", "grace@example.com", emailConfirmed: false, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-7));
            InsertUser(migrationBuilder, UserIds[7], "Henry Wilson", "henry@example.com", emailConfirmed: true, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-8));
            InsertUser(migrationBuilder, UserIds[8], "Ivy Moore", "ivy@example.com", emailConfirmed: false, isActive: false, createdAt, updatedAt, updatedAt.AddDays(-9), blockedUntil);
            InsertUser(migrationBuilder, UserIds[9], "Jack Taylor", "jack@example.com", emailConfirmed: true, isActive: true, createdAt, updatedAt, updatedAt.AddDays(-10));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var userId in UserIds)
            {
                migrationBuilder.DeleteData(
                    table: "AspNetUsers",
                    keyColumn: "Id",
                    keyValue: userId);
            }
        }

        private static void InsertUser(
            MigrationBuilder migrationBuilder,
            string id,
            string name,
            string email,
            bool emailConfirmed,
            bool isActive,
            DateTime createdAt,
            DateTime updatedAt,
            DateTime lastActivityAt,
            DateTimeOffset? lockoutEnd = null)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: UserColumns,
                values: new object[]
                {
                    id,
                    name,
                    isActive,
                    createdAt,
                    updatedAt,
                    lastActivityAt,
                    email,
                    email.ToUpperInvariant(),
                    email,
                    email.ToUpperInvariant(),
                    emailConfirmed,
                    null,
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    false,
                    false,
                    !isActive,
                    0,
                    lockoutEnd!,
                });
        }
    }
}

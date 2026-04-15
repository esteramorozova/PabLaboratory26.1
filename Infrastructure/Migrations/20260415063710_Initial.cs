using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeactivatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Address_Street = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Address_City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Address_PostalCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Address_Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Address_Type = table.Column<string>(type: "TEXT", nullable: false),
                    Address_Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ContactType = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    NIP = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    REGON = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    KRS = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Industry = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EmployeeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    AnnualRevenue = table.Column<decimal>(type: "TEXT", nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PrimaryContactId = table.Column<Guid>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    MiddleName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EmployerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_Contact_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contact_Contact_PrimaryContactId",
                        column: x => x.PrimaryContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    ContactId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    KRS = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Mission = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    PrimaryContactId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Contact_PrimaryContactId",
                        column: x => x.PrimaryContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    ContactId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10000000-0000-0000-0000-000000000001", "20000000-0000-0000-0000-000000000001", null, "Administrator", "ADMINISTRATOR" },
                    { "10000000-0000-0000-0000-000000000002", "20000000-0000-0000-0000-000000000002", null, "SalesManager", "SALESMANAGER" },
                    { "10000000-0000-0000-0000-000000000003", "20000000-0000-0000-0000-000000000003", null, "Salesperson", "SALESPERSON" },
                    { "10000000-0000-0000-0000-000000000004", "20000000-0000-0000-0000-000000000004", null, "SupportAgent", "SUPPORTAGENT" },
                    { "10000000-0000-0000-0000-000000000005", "20000000-0000-0000-0000-000000000005", null, "ReadOnly", "READONLY" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DeactivatedAt", "Department", "Email", "EmailConfirmed", "FirstName", "FullName", "LastLoginAt", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 0, "30000000-0000-0000-0000-000000000002", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), null, "Administracja", "admin@wsei.edu.pl", true, "Jan", "Jan Kowalski", null, "Kowalski", true, null, "ADMIN@WSEI.EDU.PL", "ADMIN@WSEI.EDU.PL", "AQAAAAIAAYagAAAAEC8DQ4eH7ICLtmAealgp456gDxD5xYkNalo0rIA+7fo5kSRwdJIbAB+0h5YYmYgLbA==", null, false, "30000000-0000-0000-0000-000000000001", 0, false, "admin@wsei.edu.pl" },
                    { "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", 0, "30000000-0000-0000-0000-000000000004", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), null, "Sprzedaż", "anna.sales@wsei.edu.pl", true, "Anna", "Anna Nowak", null, "Nowak", true, null, "ANNA.SALES@WSEI.EDU.PL", "ANNA.SALES@WSEI.EDU.PL", "AQAAAAIAAYagAAAAEC8DQ4eH7ICLtmAealgp456gDxD5xYkNalo0rIA+7fo5kSRwdJIbAB+0h5YYmYgLbA==", null, false, "30000000-0000-0000-0000-000000000003", 0, false, "anna.sales@wsei.edu.pl" }
                });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Address_City", "Address_Country", "Address_Id", "Address_PostalCode", "Address_Street", "Address_Type", "BirthDate", "ContactType", "CreatedAt", "Email", "EmployerId", "FirstName", "Gender", "LastName", "MiddleName", "OrganizationId", "Phone", "Position", "Status", "UpdatedAt" },
                values: new object[] { new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f"), "Kraków", "Poland", new Guid("11111111-1111-1111-1111-111111111111"), "25-009", "ul. Św. Filipa 17", "Correspondence", new DateTime(2001, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Person", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "adam@wsei.edu.pl", null, "Adam", "Male", "Nowak", null, null, "123456789", "Programista", "Active", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Address_City", "Address_Country", "Address_Id", "Address_PostalCode", "Address_Street", "Address_Type", "AnnualRevenue", "ContactType", "CreatedAt", "Email", "EmployeeCount", "Industry", "KRS", "NIP", "Name", "Phone", "PrimaryContactId", "REGON", "Status", "UpdatedAt", "Website" },
                values: new object[] { new Guid("516a34d7-ccfb-4f20-85f3-62bd0f3af271"), "Lublin", "Poland", new Guid("33333333-3333-3333-3333-333333333333"), "20-038", "ul. Projektowa 4", "Main", null, "Company", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "biuro@wsei.edu.pl", null, "edukacja", null, null, "WSEI", "123567123", null, null, "Active", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://wsei.edu.pl" });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Address_City", "Address_Country", "Address_Id", "Address_PostalCode", "Address_Street", "Address_Type", "BirthDate", "ContactType", "CreatedAt", "Email", "EmployerId", "FirstName", "Gender", "LastName", "MiddleName", "OrganizationId", "Phone", "Position", "Status", "UpdatedAt" },
                values: new object[] { new Guid("b4dcb17c-f875-43f8-9d66-36597895a466"), "Kraków", "Poland", new Guid("22222222-2222-2222-2222-222222222222"), "30-001", "ul. Grodzka 1", "Main", new DateTime(2001, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Person", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "ewa@wsei.edu.pl", null, "Ewa", "Female", "Kowalska", null, null, "123123123", "Tester", "Blocked", new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "10000000-0000-0000-0000-000000000001", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { "10000000-0000-0000-0000-000000000003", "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_EmployerId",
                table: "Contact",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_OrganizationId",
                table: "Contact",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_PrimaryContactId",
                table: "Contact",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_ContactId",
                table: "Note",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_PrimaryContactId",
                table: "Organizations",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ContactId",
                table: "Tag",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Organizations_OrganizationId",
                table: "Contact",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Organizations_OrganizationId",
                table: "Contact");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}

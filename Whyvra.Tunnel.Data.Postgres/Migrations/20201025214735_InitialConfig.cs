using System;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Whyvra.Tunnel.Data.Postgres.Migrations
{
    public partial class InitialConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetworkAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<ValueTuple<IPAddress, int>>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_NetworkAddresses", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: true),
                    AssignedRange = table.Column<ValueTuple<IPAddress, int>>(nullable: false),
                    Dns = table.Column<IPAddress>(nullable: false),
                    Endpoint = table.Column<string>(nullable: false),
                    ListenPort = table.Column<int>(nullable: false),
                    PublicKey = table.Column<string>(maxLength: 44, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Servers", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    FirstName = table.Column<string>(maxLength: 64, nullable: false),
                    LastName = table.Column<string>(maxLength: 64, nullable: false),
                    Uid = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: true),
                    AssignedIp = table.Column<ValueTuple<IPAddress, int>>(nullable: false),
                    IsRevoked = table.Column<bool>(nullable: false),
                    PublicKey = table.Column<string>(maxLength: 44, nullable: false),
                    ServerId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerNetworkAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NetworkAddressId = table.Column<int>(nullable: false),
                    ServerId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerNetworkAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerNetworkAddresses_NetworkAddresses_NetworkAddressId",
                        column: x => x.NetworkAddressId,
                        principalTable: "NetworkAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerNetworkAddresses_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<JsonDocument>(nullable: true),
                    EventType = table.Column<string>(maxLength: 16, nullable: false),
                    SourceAddress = table.Column<string>(maxLength: 45, nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    TableId = table.Column<string>(maxLength: 32, nullable: false),
                    RecordId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientNetworkAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(nullable: false),
                    NetworkAddressId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientNetworkAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientNetworkAddresses_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientNetworkAddresses_NetworkAddresses_NetworkAddressId",
                        column: x => x.NetworkAddressId,
                        principalTable: "NetworkAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NetworkAddresses",
                columns: new[] { "Id", "Address", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new System.ValueTuple<System.Net.IPAddress, int>(System.Net.IPAddress.Parse("0.0.0.0"), 0), new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Unspecified).AddTicks(7650), new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Unspecified).AddTicks(7650) },
                    { 2, new System.ValueTuple<System.Net.IPAddress, int>(System.Net.IPAddress.Parse("::"), 0), new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Unspecified).AddTicks(7650), new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Unspecified).AddTicks(7650) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "Uid", "UpdatedAt", "Username" },
                values: new object[] { 1, new DateTime(2020, 10, 25, 21, 47, 35, 65, DateTimeKind.Unspecified).AddTicks(993), "system@example.com", "System", "User", new Guid("e3adf55b-7430-42c1-ae62-758d7b644fdb"), new DateTime(2020, 10, 25, 21, 47, 35, 65, DateTimeKind.Unspecified).AddTicks(993), "system_user" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientNetworkAddresses_ClientId",
                table: "ClientNetworkAddresses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientNetworkAddresses_NetworkAddressId_ClientId",
                table: "ClientNetworkAddresses",
                columns: new[] { "NetworkAddressId", "ClientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ServerId",
                table: "Clients",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AssignedIp_ServerId",
                table: "Clients",
                columns: new[] { "AssignedIp", "ServerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name_ServerId",
                table: "Clients",
                columns: new[] { "Name", "ServerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkAddresses_Address",
                table: "NetworkAddresses",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerNetworkAddresses_ServerId",
                table: "ServerNetworkAddresses",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerNetworkAddresses_NetworkAddressId_ServerId",
                table: "ServerNetworkAddresses",
                columns: new[] { "NetworkAddressId", "ServerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_Name",
                table: "Servers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uid",
                table: "Users",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientNetworkAddresses");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ServerNetworkAddresses");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "NetworkAddresses");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}

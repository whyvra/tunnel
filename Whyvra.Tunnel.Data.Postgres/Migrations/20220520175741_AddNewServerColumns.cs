using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Whyvra.Tunnel.Data.Postgres.Migrations
{
    public partial class AddNewServerColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Servers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 16);

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "Servers",
                type: "character varying(44)",
                maxLength: 44,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(44)",
                oldMaxLength: 44)
                .Annotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Servers",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "ListenPort",
                table: "Servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "Servers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<IPAddress>(
                name: "Dns",
                table: "Servers",
                type: "inet",
                nullable: false,
                oldClrType: typeof(IPAddress),
                oldType: "inet")
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Servers",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Servers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<ValueTuple<IPAddress, int>>(
                name: "AssignedRange",
                table: "Servers",
                type: "cidr",
                nullable: false,
                oldClrType: typeof(ValueTuple<IPAddress, int>),
                oldType: "cidr")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ServerNetworkAddresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ServerNetworkAddresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NetworkAddresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NetworkAddresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ClientNetworkAddresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ClientNetworkAddresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            // Create new table with the new columns
            migrationBuilder.CreateTable(
                name: "ef_temp_Servers",
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
                    StatusApi = table.Column<string>(maxLength: 128, nullable: true),
                    RenderToDisk = table.Column<bool>(defaultValue: false, nullable: false),
                    AddFirewallRules = table.Column<bool>(defaultValue: true, nullable: false),
                    NetworkInterface = table.Column<string>(defaultValue: "eth0", maxLength: 16, nullable: false),
                    CustomConfiguration = table.Column<string>(nullable: true),
                    WireGuardInterface = table.Column<string>(maxLength: 16, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_ef_temp_Servers", x => x.Id));

            // Copy data to new table
            migrationBuilder.Sql(
                @"INSERT INTO ""ef_temp_Servers"" (""Id"", ""AssignedRange"", ""CreatedAt"", ""Description"", ""Dns"", ""Endpoint"", ""ListenPort"", ""Name"", ""PublicKey"", ""UpdatedAt"")
                SELECT ""Id"", ""AssignedRange"", ""CreatedAt"", ""Description"", ""Dns"", ""Endpoint"", ""ListenPort"", ""Name"", ""PublicKey"", ""UpdatedAt""
                FROM ""Servers"";");

            // Drop foreign keys on main table
            migrationBuilder.DropForeignKey("FK_Clients_Servers_ServerId", "Clients");
            migrationBuilder.DropForeignKey("FK_ServerNetworkAddresses_Servers_ServerId", "ServerNetworkAddresses");

            // Drop table
            migrationBuilder.DropTable("Servers");

            // Rename table
            migrationBuilder.RenameTable("ef_temp_Servers", newName: "Servers");
            migrationBuilder.RenameSequence("ef_temp_Servers_Id_seq", newName: "Servers_Id_seq");
            migrationBuilder.DropPrimaryKey("PK_ef_temp_Servers", table: "Servers");
            migrationBuilder.AddPrimaryKey("PK_Servers", "Servers", "Id");

            // Set sequence value for primary key
            migrationBuilder.Sql(
                @"SELECT setval(pg_get_serial_sequence('""Servers""', 'Id'),
                    COALESCE(max(""Id"") + 1, 1),
                    false)
                FROM ""Servers"";");

            // Recreate foreign keys and indexes
            migrationBuilder.AddForeignKey("FK_Clients_Servers_ServerId", "Clients", "ServerId", "Servers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey("FK_ServerNetworkAddresses_Servers_ServerId", "ServerNetworkAddresses", "ServerId", "Servers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.CreateIndex("IX_Servers_Name", "Servers", "Name", unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_WireGuardInterface",
                table: "Servers",
                column: "WireGuardInterface",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Servers_WireGuardInterface",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "AddFirewallRules",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "CustomConfiguration",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "NetworkInterface",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "RenderToDisk",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "StatusApi",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "WireGuardInterface",
                table: "Servers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Servers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone")
                .OldAnnotation("Relational:ColumnOrder", 16);

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "Servers",
                type: "character varying(44)",
                maxLength: 44,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(44)",
                oldMaxLength: 44)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Servers",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "ListenPort",
                table: "Servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "Servers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<IPAddress>(
                name: "Dns",
                table: "Servers",
                type: "inet",
                nullable: false,
                oldClrType: typeof(IPAddress),
                oldType: "inet")
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Servers",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Servers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone")
                .OldAnnotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<ValueTuple<IPAddress, int>>(
                name: "AssignedRange",
                table: "Servers",
                type: "cidr",
                nullable: false,
                oldClrType: typeof(ValueTuple<IPAddress, int>),
                oldType: "cidr")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ServerNetworkAddresses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ServerNetworkAddresses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NetworkAddresses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NetworkAddresses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Clients",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Clients",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ClientNetworkAddresses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ClientNetworkAddresses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaService.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProfileAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber_Value",
                table: "Profiles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Profiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Profiles");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber_Value",
                table: "Profiles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EloisStore.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The existing schema was created by EnsureCreated, without migration history.
            // IF NOT EXISTS also supports databases created from the current Product model.
            migrationBuilder.Sql("""
                ALTER TABLE "Products" ADD COLUMN IF NOT EXISTS "ImageUrl" text;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");
        }
    }
}

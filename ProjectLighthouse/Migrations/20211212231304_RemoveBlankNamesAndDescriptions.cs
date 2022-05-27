using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectLighthouse.Migrations
{
    public partial class RemoveBlankNamesAndDescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Slots SET Name = 'Unnamed Level' WHERE Name = ''");
            migrationBuilder.Sql("UPDATE Slots SET Description = 'This level has no description.' WHERE Description = ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Slots SET Name = '' WHERE Name = 'Unnamed Level'");
            migrationBuilder.Sql("UPDATE Slots SET Description = '' WHERE Description = 'This level has no description.'");
        }
    }
}

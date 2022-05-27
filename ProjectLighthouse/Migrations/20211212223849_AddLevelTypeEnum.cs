using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectLighthouse.Migrations
{
    public partial class AddLevelTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Migrate old string-based leveltype to new enum
            // See LBPUnion.ProjectLighthouse.Types.Levels.LevelType
            migrationBuilder.Sql("UPDATE Slots SET LevelType = '0' WHERE LevelType = ''");
            migrationBuilder.Sql("UPDATE Slots SET LevelType = '1' WHERE LevelType = 'versus'");
            migrationBuilder.Sql("UPDATE Slots SET LevelType = '2' WHERE LevelType = 'cutscene'");
            
            migrationBuilder.AlterColumn<int>(
                name: "LevelType",
                table: "Slots",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LevelType",
                table: "Slots",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

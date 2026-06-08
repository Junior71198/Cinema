using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AddingActors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into actors (name, description, logo, status) values ('quisque', 'nulla', 'accumsan', 1);\r\ninsert into actors (name, description, logo, status) values ('habitasse platea', 'venenatis', 'nascetur', 0);\r\ninsert into actors (name, description, logo, status) values ('eget tempus', 'penatibus', 'mus', 1);\r\ninsert into actors (name, description, logo, status) values ('sagittis', 'dolor', 'at', 1);\r\ninsert into actors (name, description, logo, status) values ('viverra', 'tortor id nulla', 'vitae', 1);\r\ninsert into actors (name, description, logo, status) values ('phasellus in', 'orci mauris lacinia', 'a', 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from actors");

        }
    }
}

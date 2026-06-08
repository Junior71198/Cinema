using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AddingCenemaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into cenemas (name, description, logo, status) values ('Sunsetl Screens', 'Donec semper sapien a libero.', 'in', 1);\r\ninsert into cenemas (name, description, logo, status) values ('Magicl Lantern Cinema', 'Maecenas pulvinar lobortis est.', 'purus', 1);\r\ninsert into cenemas (name, description, logo, status) values ('Moonlbeam Movies', 'Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.', 'diam', 1);\r\ninsert into cenemas (name, description, logo, status) values ('Sunlset Screens', 'Proin at turpis a pede posuere nonummy.', 'ornare', 1);\r\ninsert into cenemas (name, description, logo, status) values ('Drleamland Theater', 'In hac habitasse platea dictumst.', 'at', 1);\r\ninsert into cenemas (name, description, logo, status) values ('Sltarlight Cinema', 'Mauris sit amet eros.', 'varius', 0);\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from cenemas");

        }
    }
}

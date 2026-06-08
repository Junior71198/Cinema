using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AddingCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into categories (name, description, status) values ('Fantasy', 'Duis at velit eu est congue elementum.', 0);insert into categories (name, description, status) values ('Comedy', 'Integer pede justo, lacinia eget, tincidunt eget, tempus vel, pede.', 0);insert into categories (name, description, status) values ('Thriller', 'Integer a nibh.', 0);insert into categories (name, description, status) values ('Drama', 'Aenean auctor gravida sem.', 0);insert into categories (name, description, status) values ('Drama', 'Nam ultrices, libero non mattis pulvinar, nulla pede ullamcorper augue, a suscipit nulla elit ac nulla.', 1);insert into categories (name, description, status) values ('Comedy', 'Phasellus id sapien in sapien iaculis congue.', 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from categories");
        }
    }
}

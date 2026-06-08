using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class Addingmovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into movies (name, description, mainImg, status, price, discount, rate, categoryId, cinemaId, dateTime) values ('Alice in Murderland', 'Quisque porta volutpat erat.', 'eget', 1, 376.41, 49.93, 3.5, 2, 26, '1/4/2025');\r\ninsert into movies (name, description, mainImg, status, price, discount, rate, categoryId, cinemaId, dateTime) values ('Someone Marry Barry', 'Etiam pretium iaculis justo.', 'id', 0, 170.45, 22.69, 4.4, 3, 27, '4/7/2027');\r\ninsert into movies (name, description, mainImg, status, price, discount, rate, categoryId, cinemaId, dateTime) values ('Very Long Engagement, A (Un long dimanche de fiançailles)', 'Maecenas tincidunt lacus at velit.', 'sed', 1, 291.25, 30.06, 4.3, 1, 28, '7/26/2027');\r\ninsert into movies (name, description, mainImg, status, price, discount, rate, categoryId, cinemaId, dateTime) values ('Starred Up', 'Quisque arcu libero, rutrum ac, lobortis vel, dapibus at, diam.', 'nisi', 0, 158.75, 25.32, 2.7, 3, 29, '8/22/2026');\r\ninsert into movies (name, description, mainImg, status, price, discount, rate, categoryId, cinemaId, dateTime) values ('Ed Hardy: Tattoo the World ', 'Fusce consequat.', 'vivamus', 0, 248.71, 14.6, 4.4, 6, 30, '10/21/2026');\r\ninsert into movies (name, description, mainImg, status, price, discount, rate, categoryId, cinemaId, dateTime) values ('Arnulf Rainer', 'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam.', 'augue', 1, 493.37, 9.6, 2.8, 1, 31, '9/11/2025');\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from movies");
        }
    }
}

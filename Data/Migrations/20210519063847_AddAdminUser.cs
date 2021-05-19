using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagementWithIdentity.Data.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) VALUES (N'd6d25e3f-b9ff-45b2-9cd9-d3387a157337', N'Admin', N'ADMIN', N'Admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAEAACcQAAAAEJ0OsXuI/m9sjMPo/BP6mWW5/NsEdyoZMwlGSmXcFl4Ncyp8KCLkGA/VNZa6MrP1yg==', N'QBVNRKT43ZBTOSWEKAKMFN2GHJSN2CWQ', N'0869f100-11a2-47d2-adc4-15b1877e8907', NULL, 0, 0, NULL, 1, 0, N'Mariam', N'Amr Mostafa', NULL)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = 'd6d25e3f-b9ff-45b2-9cd9-d3387a157337' ");
        }
    }
}

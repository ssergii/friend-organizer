namespace FriendOrganizer.DataAccess.Migrations
{
    using Model;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDBContext context)
        {
            context.Friends.AddOrUpdate(
                x => x.Id,
                new Friend { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "j.kowalski@wp.pl" },
                new Friend { Id = 2, FirstName = "Adam", LastName = "Mickiewicz", Email = "a.mickiewicz@.onet.pl" },
                new Friend { Id = 3, FirstName = "Kazimierz", LastName = "Wielki", Email = "k.wielki@gmail.com" },
                new Friend { Id = 4, FirstName = "Andrzj", LastName = "Nowak", Email = "a.nowal@gmail.com" });
        }
    }
}

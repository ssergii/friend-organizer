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
                new Friend { FirstName = "Jan", LastName = "Kowalski", Email = "j.kowalski@wp.pl" },
                new Friend { FirstName = "Adam", LastName = "Mickiewicz", Email = "a.mickiewicz@.onet.pl" },
                new Friend { FirstName = "Kazimierz", LastName = "Wielki", Email = "k.wielki@gmail.com" },
                new Friend { FirstName = "Andrzj", LastName = "Nowak", Email = "a.nowal@gmail.com" });
        }
    }
}

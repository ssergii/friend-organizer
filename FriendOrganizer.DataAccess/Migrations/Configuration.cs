namespace FriendOrganizer.DataAccess.Migrations
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDBContext context)
        {
            context.Friends.AddOrUpdate(
                x => x.LastName,
                new Friend { FirstName = "Jan", LastName = "Kowalski", Email = "j.kowalski@wp.pl" },
                new Friend { FirstName = "Adam", LastName = "Mickiewicz", Email = "a.mickiewicz@onet.pl" },
                new Friend { FirstName = "Kazimierz", LastName = "Wielki", Email = "k.wielki@gmail.com" },
                new Friend { FirstName = "Andrzj", LastName = "Nowak", Email = "a.nowal@gmail.com" });

            context.ProgrammingLanguages.AddOrUpdate(
                x => x.Id,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "PHP" },
                new ProgrammingLanguage { Name = "JavaScript" },
                new ProgrammingLanguage { Name = "SQL " });

            context.SaveChanges();

            context.PhoneNumbers.AddOrUpdate(
                x => x.Id,
                new PhoneNumber { Number = "+48 884 131 499", FriendId = context.Friends.First().Id });

            context.Meetings.AddOrUpdate(
                x => x.Title,
                new Meeting
                {
                    Title = "Watching Soccer",
                    DateFrom = DateTime.Now,
                    DateTo = DateTime.Now.AddDays(3),
                    Friends = new List<Friend>()
                    {
                        context.Friends.Single(x => x.Id == 1),
                        context.Friends.Single(x => x.Id == 2)
                    }
                });
        }
    }
}

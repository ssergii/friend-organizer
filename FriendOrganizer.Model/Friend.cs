﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        public Friend()
        {
            PhoneNumbers = new Collection<PhoneNumber>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50,  MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(50, MinimumLength = 2)]
        public string Email { get; set; }

        public int? FavoritLanguageId { get; set; }

        public ProgrammingLanguage FavoritLanguage { get; set; }

        public IEnumerable<PhoneNumber> PhoneNumbers { get; set; }
    }
}

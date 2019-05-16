﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.Model
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        [Required]
        [Phone]
        public string Number { get; set; }

        public int FriendId { get; set; }
        public Friend Friend { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

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

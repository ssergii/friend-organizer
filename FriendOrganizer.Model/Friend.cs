using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
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
    }
}

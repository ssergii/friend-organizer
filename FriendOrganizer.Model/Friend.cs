using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50,  MinimumLength = 2, ErrorMessage = "First name length is between 2 and 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name length is between 2 and 50 characters")]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Email length is between 2 and 50 characters")]
        public string Email { get; set; }
    }
}

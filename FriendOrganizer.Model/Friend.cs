namespace FriendOrganizer.Model
{
    public class Friend
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}

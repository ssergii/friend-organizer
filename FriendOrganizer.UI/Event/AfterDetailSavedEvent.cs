using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    class AfterDetailSavedEvent : PubSubEvent<AfterDetailSavedEventArgs> { }

    class AfterDetailSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
        public string VMName { get; set; }
    }
}

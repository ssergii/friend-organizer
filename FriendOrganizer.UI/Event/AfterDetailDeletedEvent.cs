using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    class AfterDetailDeletedEvent : PubSubEvent<AfterDetailDeletedEventArgs> { }

    class AfterDetailDeletedEventArgs
    {
        public int Id { get; set; }
        public string VMName { get; set; }
    }
}

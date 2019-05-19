using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    class OpenDetailViewEvent : PubSubEvent<OpenDetailViewEventArgs> { }

    class OpenDetailViewEventArgs
    {
        public int? Id { get; set; }
        public string VMName { get; set; }
    }
}

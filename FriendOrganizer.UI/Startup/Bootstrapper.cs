using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<FriendDataService>().As<IDataService<Friend>>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendOrganizerDBContext>().AsSelf();

            return builder.Build();
        }
    }
}

﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using FriendOrganizer.UI.View.Services;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : BaseViewModel, IFriendDetailViewModel
    {
        #region fields and properties
        private IRepository<Friend> _friendRepository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        private FriendWrapper _friend;
        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        private bool _hasChanges;
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges == value)
                    return;

                _hasChanges = value;
                OnPropertyChanged();
                (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }
        #endregion

        public FriendDetailViewModel(
            IRepository<Friend> repository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _friendRepository = repository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
        }

        public async Task LoadByIdAsync(int? id)
        {
            var model = id.HasValue ?
                await _friendRepository.GetByIdAsync(id.Value) : CreateFriend();


            Friend = new FriendWrapper(model);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                    HasChanges = _friendRepository.HasChanges();

                if (e.PropertyName == nameof(Friend.HasErrors))
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
            };

            (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        #region commands
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExcute);

                return _saveCommand;
            }
        }

        private bool OnSaveCanExcute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();

            HasChanges = _friendRepository.HasChanges();

            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new DelegateCommand(OnDeleteCommandExecute);

                return _deleteCommand;
            }
        }

        private async void OnDeleteCommandExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog(
                $"Do you really want to delete {Friend.FirstName} {Friend.LastName}",
                "Question");

            if (result == MessageDialogResult.Cancel)
                return;

            _friendRepository.Remove(Friend.Model);
            await _friendRepository.SaveAsync();

            _eventAggregator.GetEvent<AfterFriendDeleteEvent>().Publish(Friend.Id);
        }
        #endregion

        private Friend CreateFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);

            return friend;
        }
    }
}

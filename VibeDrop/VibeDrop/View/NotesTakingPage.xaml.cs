using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using VibeDrop.Helpers;
using VibeDrop.ViewModel;
using Xamarin.Forms;

namespace VibeDrop.View
{
    public partial class NotesTakingPage : ContentPage
    {
        NoteViewModel vm;
        public NotesTakingPage()
        {
            InitializeComponent();
            BindingContext = vm = new NoteViewModel();
            ListViewNotes.ItemTapped += (sender, e) =>
            {
                if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                    ListViewNotes.SelectedItem = null;
            };

            if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Command = vm.LoadNotesCommand
                });
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CrossConnectivity.Current.ConnectivityChanged += ConnecitvityChanged;
            OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;


            if (vm.Notes.Count == 0 && Settings.IsLoggedIn)
                vm.LoadNotesCommand.Execute(null);
            else
            {
                await vm.LoginAsync();
                if (Settings.IsLoggedIn)
                    vm.LoadNotesCommand.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CrossConnectivity.Current.ConnectivityChanged -= ConnecitvityChanged;
        }

        void ConnecitvityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                OfflineStack.IsVisible = !e.IsConnected;
            });
        }
    }
}

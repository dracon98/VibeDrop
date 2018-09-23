using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using VibeDrop.Helpers;
using VibeDrop.Model;
using VibeDrop.Services;
using Xamarin.Forms;

namespace VibeDrop.ViewModel
{
    public class NoteViewModel : BaseViewModel                    
    {
        AzureService azureService;
        public NoteViewModel()
        {
            azureService = DependencyService.Get<AzureService>();
        }

        public ObservableRangeCollection<Note> Notes { get; } = new ObservableRangeCollection<Note>();
        public ObservableRangeCollection<Grouping<string, Note>> NotesGrouped { get; } = new ObservableRangeCollection<Grouping<string, Note>>();

        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }


        bool atHome;
        public bool AtHome
        {
            get => atHome;
            set => SetProperty(ref atHome, value);
        }


        string location;
        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }


        ICommand loadNotesCommand;
        public ICommand LoadNotesCommand =>
        loadNotesCommand ?? (loadNotesCommand = new Command(async () => await ExecuteLoadNotesCommandAsync()));

        async Task ExecuteLoadNotesCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;


            try
            {
                LoadingMessage = "Loading Notes...";
                IsBusy = true;
                var notes = await azureService.GetNotes();
                Notes.ReplaceRange(notes);


                SortNotes();


            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync notes, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }


        }

        void SortNotes()
        {
            var groups = from note in Notes
                orderby note.DateUtc descending
                            group note by note.DateDisplay
                into noteGroup
                         select new Grouping<string, Note>($"{Group.Key} ({noteGroup.Count()})", noteGroup);


            NotesGrouped.ReplaceRange(groups);
        }

        ICommand addNoteCommand;
        public ICommand AddNoteCommand =>
            addNoteCommand ?? (addNoteCommand = new Command(async () => await ExecuteAddNoteCommandAsync()));

        async Task ExecuteAddNoteCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;

            try
            {

                if (string.IsNullOrWhiteSpace(Location) && !AtHome)
                {
                    await Application.Current.MainPage.DisplayAlert("Needs Location", "Please enter a location before adding the note.", "OK");
                    return;
                }
                LoadingMessage = "Adding Note...";
                IsBusy = true;


                var note = await azureService.AddNote(AtHome, location);
                Location = string.Empty;
                AtHome = false;
                Notes.Add(note);
                SortNotes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }

        }

        public Task<bool> LoginAsync()
        {
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);


            return azureService.LoginAsync();
        }
    }
}

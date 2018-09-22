using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using VibeDrop.Helpers;
using VibeDrop.Model;
using VibeDrop.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureService))]
namespace VibeDrop.Services
{
    public class AzureService
    {

        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<Note> noteTaking;

#if AUTH
        public static bool UseAuth { get; set; } = true;
#else
        public static bool UseAuth { get; set; } = false;
#endif

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://ENTER-APP-SERVICE-NAME.azurewebsites.net";
#if AUTH
            Client = new MobileServiceClient(appUrl, new AuthHandler());

            if (!string.IsNullOrWhiteSpace (Settings.AuthToken) && !string.IsNullOrWhiteSpace (Settings.UserId)) {
                Client.CurrentUser = new MobileServiceUser (Settings.UserId);
                Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }
#else
            //Create our client

            Client = new MobileServiceClient(appUrl);

#endif

            //InitializeDatabase for path
            var path = "syncstore.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<Note>();

            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store);

            //Get our sync table that will call out to azure
            noteTaking = Client.GetSyncTable<Note>();
        }

        public async Task SyncNote()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await noteTaking.PullAsync("allNotes", noteTaking.CreateQuery());

                await Client.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync notes, that is alright as we have offline capabilities: " + ex);
            }

        }

        public async Task<IEnumerable<Note>> GetNotes()
        {
            //Initialize & Sync
            await Initialize();
            await SyncNote();

            return await noteTaking.OrderBy(c => c.DateUTC).ToEnumerableAsync(); ;

        }

        public async Task<Note> AddNote(bool atHome, string location)
        {
            await Initialize();

            var note = new Note
            {
                DateUTC = DateTime.UtcNow,
                MadeAtHome = atHome,
                OS = Device.RuntimePlatform,
                Location = location ?? string.Empty
            };

            await noteTaking.InsertAsync(note);

            await SyncNote();
            //return note
            return note;
        }

        public async Task<bool> LoginAsync()
        {

            await Initialize();

            var provider = MobileServiceAuthenticationProvider.Twitter;
            var uriScheme = "note";


#if __ANDROID__
            var user = await Client.LoginAsync(Forms.Context, provider, uriScheme);

#elif __IOS__
            CoffeeCups.iOS.AppDelegate.ResumeWithURL = url => url.Scheme == uriScheme && Client.ResumeWithURL(url);
            var user = await Client.LoginAsync(GetController(), provider, uriScheme);
            
#else
            var user = await Client.LoginAsync(provider, uriScheme);

#endif
            if (user == null)
            {
                Settings.AuthToken = string.Empty;
                Settings.UserId = string.Empty;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Login Error", "Unable to login, please try again", "OK");
                });
                return false;
            }
            else
            {
                Settings.AuthToken = user.MobileServiceAuthenticationToken;
                Settings.UserId = user.UserId;
            }

            return true;
        }


#if __IOS__
         UIKit.UIViewController GetController()
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var root = window.RootViewController;
            if (root == null)
                return null;

            var current = root;
            while (current.PresentedViewController != null)
            {
                current = current.PresentedViewController;
            }

            return current;
        }
#endif

    }
}

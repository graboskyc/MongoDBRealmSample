using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Realms;
using Realms.Sync;
using Credentials = Realms.Sync.Credentials;

namespace RealmTester
{
    public partial class MainPage : ContentPage
    {
        Realm _realm; 

        public MainPage()
        {
            Task.Run(async () => { await Initialize(); });
        }

        private async Task Initialize()
        {
            InitializeComponent();
            await Task.Yield();

            _realm = await OpenRealm();
            var Entries = _realm.All<Beer>();
            var EList = Entries.ToList();
            lbl_status.Text = "";
            foreach (var b in EList)
            {
                lbl_status.Text = lbl_status.Text + " " + b.Name;
            }
            lbl_status.Text = lbl_status.Text + " All Done!";
        }

        private async Task<Realm> OpenRealm()
        {
            User user;
            //var user = User.Current;
            //if (user != null)
            //{
            //    var configuration = new FullSyncConfiguration(new Uri(Constants.RealmPath, UriKind.Relative), user);

                // User has already logged in, so we can just load the existing data in the Realm.
            //   return Realm.GetInstance(configuration);
           //}

            // When that is called in the page constructor, we need to allow the UI operation
            // to complete before we can display a dialog prompt.
            //await Task.Yield();

            // var response = await UserDialogs.Instance.PromptAsync(new PromptConfig
            //{
            //    Title = "Login",
            //     Message = "Please enter your nickname",
            //    OkText = "Login",
            //     IsCancellable = true,
            //});

           var credentials = Credentials.Anonymous();
            //var credentials = Credentials.Nickname("yomamma");



            try
            {
                //UserDialogs.Instance.ShowLoading("Logging in...");

                user = await User.LoginAsync(credentials, new Uri(Constants.AuthUrl));

                //UserDialogs.Instance.ShowLoading("Loading data");

                var configuration = new FullSyncConfiguration(new Uri(Constants.RealmPath, UriKind.Relative), user);

                // First time the user logs in, let's use GetInstanceAsync so we fully download the Realm
                // before letting them interract with the UI.
                var realm = Realm.GetInstance(configuration);
                //var realm = await Realm.GetInstanceAsync(configuration);

                //UserDialogs.Instance.HideLoading();

                return realm;
            }
            catch (Exception ex)
            {
                lbl_status.Text = ex.Message;

                // Try again
                return await OpenRealm();
            }
        }
    }
}

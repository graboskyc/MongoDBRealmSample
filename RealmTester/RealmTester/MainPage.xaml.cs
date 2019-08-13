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

            lbl_status.Text = "Loading...";
            _realm = await OpenRealm();
            var Entries = _realm.All<Beer>().OrderBy(b => b.Name);
            var EList = Entries.ToList();

            lbl_status.Text = lbl_status.Text + " All Done!";
        }

        private async Task<Realm> OpenRealm()
        {
            var credentials = Credentials.Anonymous();

            try
            {
                User user = await User.LoginAsync(credentials, new Uri(Constants.AuthUrl));
                var configuration = new FullSyncConfiguration(new Uri(Constants.RealmPath, UriKind.Relative), user);
                var realm = Realm.GetInstance(configuration);

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

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

        public MainPage()
        {
            Task.Run(async () => { await Initialize(); });
        }

        private async Task Initialize()
        {
            InitializeComponent();
            await Task.Yield();

            lbl_status.Text = "Ready...";

        }

        async void btnInsert_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                Realm locRealmInst = await OpenRealm();
                locRealmInst.Write(() =>
                {
                    locRealmInst.Add(new Beer
                    {
                        Name = "Test Beer",
                        Brewery = "Your Mom",
                        Style = "Tasty"
                    });
                });
                locRealmInst.Refresh();
                lbl_status.Text = "Inserted something";
            } 
            catch (Exception ex)
            {
                lbl_status.Text = ex.Message;
            }
        }

        async void btnReads_Clicked(object sender, System.EventArgs e)
        {
            lbl_status.Text = "";
            Realm locRealmInst = await OpenRealm();
            var Entries = locRealmInst.All<Beer>().OrderBy(b => b.Name);
            var EList = Entries.ToList();
            foreach(var b in EList)
            {
                lbl_status.Text = lbl_status.Text + b.Name + ", ";
            }
        }

        private async Task<Realm> OpenRealm()
        {
            var credentials = Credentials.Anonymous();
            //var credentials = Credentials.UsernamePassword("testacct", "testpasswd", false);

            try
            {
                User user = await User.LoginAsync(credentials, new Uri(Constants.AuthUrl));
                var configuration = new FullSyncConfiguration(new Uri(Constants.RealmPath), user);
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

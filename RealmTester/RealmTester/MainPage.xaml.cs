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
        Credentials _credentials = null;

        public MainPage()
        {
            Task.Run(async () => { await Initialize(); });
        }

        private async Task Initialize()
        {
            InitializeComponent();
            await Task.Yield();

            lbl_status.Text = "Ready...";
            stk_login.IsVisible = true;
            stk_form.IsVisible = false;
        }

        void btnLogin_Clicked(object sender, System.EventArgs e)
        {
            _credentials = Credentials.UsernamePassword(txt_un.Text, txt_pw.Text, false);
            stk_login.IsVisible = false;
            stk_form.IsVisible = true;
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
                        Name = txt_name.Text,
                        Brewery = txt_brew.Text,
                        Style = txt_style.Text
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
            try
            {
                User user = await User.LoginAsync(_credentials, new Uri(Constants.AuthUrl));
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

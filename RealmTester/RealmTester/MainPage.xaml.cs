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

            if (App.Current.Properties.ContainsKey("username") && App.Current.Properties.ContainsKey("password"))
            {
                stk_login.IsVisible = false;
                stk_form.IsVisible = true;
                _credentials = Credentials.UsernamePassword((string)App.Current.Properties["username"], (string)App.Current.Properties["password"], false);
            }
            else
            {
                stk_login.IsVisible = true;
                stk_form.IsVisible = false;
            }
        }

        void btnLogin_Clicked(object sender, System.EventArgs e)
        {
            _credentials = Credentials.UsernamePassword(txt_un.Text, txt_pw.Text, false);

            stk_login.IsVisible = false;
            stk_form.IsVisible = true;

            if (App.Current.Properties.ContainsKey("username"))
            {
                App.Current.Properties["username"] = txt_un.Text;
            }
            else
            {
                App.Current.Properties.Add("username", txt_un.Text);
            }

            if (App.Current.Properties.ContainsKey("password"))
            {
                App.Current.Properties["password"] = txt_pw.Text;
            }
            else
            {
                App.Current.Properties.Add("password", txt_pw.Text);
            }
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
            lbl_status.Text = String.Join("\r\n", EList);
        }

        async void btnPerm_Clicked(object sender, System.EventArgs e)
        {
            Credentials creds = Credentials.UsernamePassword("realmroot","realmroot1234", false);
            User user = await User.LoginAsync(creds, new Uri(Constants.AuthUrl));
            var condition = PermissionCondition.Default; 
            await user.ApplyPermissionsAsync(condition, Constants.RealmPath + "beer", AccessLevel.Read);
        }

        private async Task<Realm> OpenRealm()
        {
            try
            {
                _credentials = Credentials.UsernamePassword((string)App.Current.Properties["username"], (string)App.Current.Properties["password"], false);
                User user = await User.LoginAsync(_credentials, new Uri(Constants.AuthUrl));
                var configuration = new FullSyncConfiguration(new Uri(Constants.RealmPath + ddl_realm.SelectedItem.ToString()), user);
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

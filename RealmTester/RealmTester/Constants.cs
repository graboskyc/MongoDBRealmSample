using System;

namespace RealmTester
{
    public static class Constants
    {
        private static string InstanceAddress = "grabosky.us1a.cloud.realm.io";
        private static string Realm = "~/myprivatebeerlist";
        //private static string Realm = "untappd";
        public static string AuthUrl = "https://" + InstanceAddress;
        public static string RealmPath = "realms://" + InstanceAddress + "/" + Realm;
    }
}

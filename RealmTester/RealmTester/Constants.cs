using System;

namespace RealmTester
{
    public static class Constants
    {
        private static string InstanceAddress = "grabosky.us1a.cloud.realm.io";
        public static string AuthUrl = "https://" + InstanceAddress;
        public static string RealmPath = "realms://" + InstanceAddress + "/";
    }
}

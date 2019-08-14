using System.Collections.Generic;
using Realms;
using System;

namespace RealmTester
{

    public class Beer : RealmObject
    {
        [MapTo("name")]
        public string Name { get; set; }
        [MapTo("style")]
        public string Style { get; set;  }
        [MapTo("brewery")]
        public string Brewery { get; set; }
        [MapTo("drank")]
        public IList<DateTimeOffset> Drank { get; }

        public override string ToString()
        {
            return string.Format("{0} (a {1}) from {2}", Name, Style, Brewery);
        }
    }
}
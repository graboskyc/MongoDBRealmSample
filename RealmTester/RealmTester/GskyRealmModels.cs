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
    }
}
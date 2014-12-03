using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    [Serializable()]
    public class Test
    {
        private string what;
        private string woot;
        public Test()
        {
            what = "whaaat";
            woot = "woooot";

        }
    }

    [Serializable()]
    public class TestClass
    {
        private string someString;
        public string SomeString
        {
            get { return someString; }
            set { someString = value; }
        }

        private List<string> settings = new List<string>();
        public List<string> Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        // These will be ignored
        [NonSerialized()]
        private int willBeIgnored1 = 1;
        private int willBeIgnored2 = 1;

    }
}

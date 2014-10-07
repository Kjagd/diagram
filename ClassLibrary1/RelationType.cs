using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    class RelationType
    {
        private string name { get; set; }
        private string description { get; set; }

        public RelationType(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}

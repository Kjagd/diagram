using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class RelationType
    {
        private string Name { get; set; }
        private string Description { get; set; }

        public RelationType(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}

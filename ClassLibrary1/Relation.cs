using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    class Relation
    {
        private Klass From { get; set; }
        private Klass To { get; set; }
        private RelationType Type { get; set; }
        private RelationMultiplicity Multiplicity { get; set; }


        public Relation(Klass from, Klass to, RelationType type)
        {
            this.From = from;
            this.To = to;
            this.Type = type;
        }
    }
}

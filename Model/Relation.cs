using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Relation
    {
        public Klass From { get; set; }
        public Klass To { get; set; }
        private RelationMultiplicity Multiplicity { get; set; }


        public Relation(Klass from, Klass to)
        {
            this.From = from;
            this.To = to;
        }
    }
}

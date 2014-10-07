using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    class Relation
    {
        private Klass from { get; set; }
        private Klass to { get; set; }
        private RelationType type { get; set; }
        private RelationMultiplicity multiplicity { get; set; }
        private Role role { get; set; }


        public Relation(Klass from, Klass to, RelationType type)
        {
            this.from = from;
            this.to = to;
            this.type = type;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Inheritance : Relation
    {
        public Inheritance(Klass @from, Klass to) : base(@from, to)
        {
            
        }
    }
}

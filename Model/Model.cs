using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    [Serializable()]
    public class Model
    {
        public Collection<Klass> Klasses { get; set; }
        //public Collection<Relation> Relations { get; set; }

    }
}

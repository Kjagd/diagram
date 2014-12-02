using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{

    public class Method : ICloneable
    {
        public String MethodName { get; set; }
        public String AccessModifier { get; set; }

        public Method(string methodName, string accessModifier)
        {
            MethodName = methodName;
            AccessModifier = accessModifier;
        }

        public object Clone()
        {
            Method f = (Method)this.MemberwiseClone();
            return f;
        }
    }
}

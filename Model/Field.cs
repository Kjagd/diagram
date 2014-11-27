using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{

    public class Field : ICloneable
    {
        public String FieldName { get; set; }
        public String AccessModifier { get; set; }

        public Field(string fieldName, string accessModifier)
        {
            FieldName = fieldName;
            AccessModifier = accessModifier;
        }

        public object Clone()
        {
            Field f = (Field)this.MemberwiseClone();
            return f;
        }
    }
}

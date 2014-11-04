using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{

    public class Field
    {
        public String FieldName { get; set; }
        public String AccessModifier { get; set; }

        public Field(string fieldName, string accessModifier)
        {
            FieldName = fieldName;
            AccessModifier = accessModifier;
        }
    }
}

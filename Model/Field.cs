using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    [Serializable]
    public class Field : ICloneable, ISerializable
    {
        public string FieldName { get; set; }
        public string AccessModifier { get; set; }

        public Field(string fieldName, string accessModifier)
        {
            FieldName = fieldName;
            AccessModifier = accessModifier;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("fieldname", FieldName, typeof(string));
            info.AddValue("accessmodifier", AccessModifier, typeof(string));

        }
        
        // The special constructor is used to deserialize values. 
        public Field(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            FieldName = (string)info.GetValue("fieldname", typeof(string));
            AccessModifier = (string)info.GetValue("accessmodifier", typeof(string));
        }

        public object Clone()
        {
            Field f = (Field)this.MemberwiseClone();
            return f;
        }
    }
}

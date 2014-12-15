using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    [Serializable]
    public class Method : ICloneable, ISerializable
    {
        public String MethodName { get; set; }
        public String AccessModifier { get; set; }

        public Method(string methodName, string accessModifier)
        {
            MethodName = methodName;
            AccessModifier = accessModifier;
        }

        // The special constructor is used to deserialize values. 
        public Method(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            MethodName = (string)info.GetValue("methodname", typeof(string));
            AccessModifier = (string)info.GetValue("accessmodifier", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("methodname", MethodName, typeof(String));
            info.AddValue("accessmodifier", AccessModifier, typeof(String));
        }

        public object Clone()
        {
            Method f = (Method)this.MemberwiseClone();
            return f;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Diagram
{
    public class Klass
    {

        // View properties
        public float Width { get; set; }
        public float Height { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float CenterX { get { return X + Width/2; } }
        public float CenterY { get { return Y + Height/2; } }

        public string Package { get; set; }
        public string Name { get; set; }

        ArrayList attributes = new ArrayList();
        ArrayList operations = new ArrayList();

        // Visibility of attributes and operations

        public Klass(string name)
        {
            Name = name;
            Width = 150;
            Height = 150;
        }

        public void addAttribute(string attribute)
        {
            attributes.Add(attribute);
        }

        public void addOperations(string operation)
        {
            operations.Add(operation);
        }


    }
}

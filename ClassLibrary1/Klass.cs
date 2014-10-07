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
        private string package { get; set; }
        private string name {get;set;}

        ArrayList attributes = new ArrayList();
        ArrayList operations = new ArrayList();
        
        // Visibility of attributes and operations

        public Klass(string name)
        {
            this.name = name;
        }

        public void addAttribute(string attribute) {
            attributes.Add(attribute);
        }

        public void addOperations(string operation) {
            operations.Add(operation);
        }


    }
}

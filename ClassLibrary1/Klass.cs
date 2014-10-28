﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Diagram
{
    public class Klass : NotifyBase
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

        public ObservableCollection<Field> Fields { get; set; }
        //public ObservableCollection<Method> Methods { get; set; }

        // Visibility of attributes and operations

        public Klass(string name)
        {
            Name = name;
            Width = 150;
            Height = 150;

            Fields = new ObservableCollection<Field>();
        }

        public void AddField(Field field)
        {
            Fields.Add(field);
        }

        public void AddMethod(string operation)
        {
           // Methods.Add(operation);
        }


    }
}

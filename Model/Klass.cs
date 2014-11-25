using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using GalaSoft.MvvmLight;

namespace Diagram
{
    public class Klass : ViewModelBase, ICloneable
    {

        private float _x;
        private float _y;
        private bool _isSelected;
        private float _borderThickness;
        // View properties
        public float Width { get; set; }
        public float Height { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
                BorderThickness = _isSelected ? 1 : 0;  
            }
        }

        public float BorderThickness
        {
            get { return _borderThickness; }
            set { _borderThickness = value; RaisePropertyChanged(); }
        }

        public float X
        {
            get { return _x; }
            set
            {
                _x = value; RaisePropertyChanged();
            }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; RaisePropertyChanged(); }
        }

        public float CenterX
        {
            get { return X + Width / 2; }
        }

        public float CenterY
        {
            get { return Y + Height / 2; }
        }

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



        public object Clone()
        {
            Klass newKlass = (Klass) this.MemberwiseClone();
            return newKlass;
        }
    }
}

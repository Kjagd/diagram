using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diagram
{
    public class Klass : ViewModelBase, ICloneable
    {
        private bool _isSelected;
        private float _borderThickness;
        private float _height;        
        private float _width;
        private Point _position;
        // View properties
        public Collection<Relation> Relations { get; private set; }

        public string Package { get; set; }
        public string Name { get; set; }

        public ObservableCollection<Field> Fields { get; set; }
        //public ObservableCollection<Method> Methods { get; set; }

        public ICommand TitleTextChanged { get; set; }
        public ICommand NewFieldCommand { get; set; }

        public Klass(string name)
        {
            Relations = new Collection<Relation>();
            Name = name;
            Width = 150;
            Height = 100;

            Fields = new ObservableCollection<Field>();

            TitleTextChanged = new RelayCommand<EventArgs>(ChangeTitle);
            NewFieldCommand = new RelayCommand(AddField);

        }

        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                NotifyRelations();
            }
        }

        public float Height
        {
            get { return _height; }
            set
            {
                _height = value; 
                RaisePropertyChanged();
                NotifyRelations();
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set 
            { 
                _isSelected = value;
                BorderThickness = _isSelected ? 1 : 0;  
            }
        }

        public float BorderThickness
        {
            get { return _borderThickness; }
            set 
            { 
                _borderThickness = value; 
                RaisePropertyChanged();
            }
        }

        public float X
        {
            get { return (float) _position.X; }
            set
            {
                _position.X = value; RaisePropertyChanged();
                NotifyRelations();
            }
        }

        public float Y
        {
            get { return (float) _position.Y; }
            set
            {
                _position.Y = value; RaisePropertyChanged();
                NotifyRelations();
            }
        }

        public float CenterX
        {
            get { return X + Width / 2; }
        }

        public float CenterY
        {
            get { return Y + Height / 2; }
        }

        private void AddField()
        {
            Fields.Add(new Field("", "+"));
            Height += 15;
        }

        public void ChangeTitle(EventArgs eventArgs)
        {
            Console.WriteLine("yoyo");
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
            Klass k = new Klass(this.Name) { X = 200, Y = 200 };

            foreach (Field f in this.Fields)
            {
                k.AddField((Field)f.Clone());
            }

            k.Height = this.Height;
            k.Width = this.Width;

            return k;
        }
        private void NotifyRelations()
        {
            foreach (Relation r in Relations)
            {
                r.Notify();
            }
        }
    }
}

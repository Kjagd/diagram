using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;

namespace Diagram
{
    [Serializable]
    public class Klass : ViewModelBase, ICloneable, ISerializable
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
        public ObservableCollection<Method> Methods { get; set; }
        //public ObservableCollection<Method> Methods { get; set; }

        public ICommand TitleTextChanged { get; set; }
        public ICommand NewFieldCommand { get; set; }
        public ICommand NewMethodCommand { get; set; }

        public ICommand DeleteFieldCommand { get; set; }

        public Klass(string name)
        {
            Relations = new Collection<Relation>();
            Name = name;
            Width = 150;
            Height = 80;

            Fields = new ObservableCollection<Field>();
            Methods = new ObservableCollection<Method>();

            NewFieldCommand = new RelayCommand(AddField);
            NewMethodCommand = new RelayCommand(AddMethod);
            DeleteFieldCommand = new RelayCommand<MouseButtonEventArgs>(DeleteField);

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("Package", Package, typeof(string));
            info.AddValue("borderThickness", _borderThickness, typeof(float));
            info.AddValue("height", _height, typeof(float));
            info.AddValue("width", _width, typeof(float));
            info.AddValue("position", _position, typeof(Point));

            info.AddValue("Relations", Relations, typeof(Collection<Relation>));
            info.AddValue("Fields", Fields, typeof(ObservableCollection<Field>));
            info.AddValue("Methods", Methods, typeof(ObservableCollection<Method>));

        }

        // The special constructor is used to deserialize values. 
        public Klass(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            Name = (string) info.GetValue("Name", typeof(string));
            Package = (string)info.GetValue("Package", typeof(string));
            _borderThickness = (float)info.GetValue("borderThickness", typeof(float));
            _height = (float)info.GetValue("height", typeof(float));
            _width = (float)info.GetValue("width", typeof(float));
            _position = (Point)info.GetValue("position", typeof(Point));

            Relations = (Collection<Relation>)info.GetValue("Relations", typeof(Collection<Relation>));
            Fields = (ObservableCollection<Field>)info.GetValue("Fields", typeof(ObservableCollection<Field>));
            Methods = (ObservableCollection<Method>)info.GetValue("Methods", typeof(ObservableCollection<Method>));

            NewFieldCommand = new RelayCommand(AddField);
            NewMethodCommand = new RelayCommand(AddMethod);
        }

        private void DeleteField(MouseButtonEventArgs e)
        {
            var frameworkElement = (FrameworkElement) e.MouseDevice.Target;
            if (frameworkElement.DataContext is Method)
            {
                Method m = (Method) frameworkElement.DataContext;
                Methods.Remove(m);
                Height -= 15;
            }
            if (frameworkElement.DataContext is Field)
            {
                Field f = (Field) frameworkElement.DataContext;
                Fields.Remove(f);
                Height -= 15;
            }
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
                _position.X = value >= 0 ? value : 0;
                RaisePropertyChanged();
                NotifyRelations();
            }
        }

        public float Y
        {
            get { return (float) _position.Y; }
            set
            {
                _position.Y = value >= 0 ? value : 0;
                RaisePropertyChanged();
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

        private void AddMethod()
        {
            Methods.Add(new Method("", "+"));
            Height += 15;
        }
        private void AddField()
        {
            Fields.Add(new Field("", "+"));
            Height += 15;
        }

        public void AddField(Field field)
        {
            Fields.Add(field);
            Height += 15;
        }

        public void AddMethod(Method method)
        {
            Methods.Add(method);
            Height += 15;
        }

        public object Clone()
        {
            Klass k = new Klass(this.Name) { X = 200, Y = 200 };

            foreach (Field f in this.Fields)
            {
                k.AddField((Field)f.Clone());
            }

            foreach (Method m in this.Methods)
            {
                k.AddMethod((Method)m.Clone());
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

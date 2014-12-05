using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace Diagram
{
    [Serializable]
    public class Relation : ViewModelBase, ISerializable
    {
        public enum Type
        {
            Inheritance,
            Reference,
            Composition
        }

        private Point _center;

        private Point _from;

        private Point _knack1;
        private Point _knack2;
        private Point _to;
        private Point _fromLabelPoint;
        private Point _toLabelPoint;

        private Visibility _contextVisibility = Visibility.Hidden;

        public Type RelationType { get; set; }

        private RelationMultiplicity Multiplicity { get; set; }

        // The special constructor is used to deserialize values. 
        public Relation(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            RelationType = (Type) info.GetValue("type", typeof (Type));
            From = (Klass) info.GetValue("From", typeof (Klass));
            To = (Klass) info.GetValue("To", typeof (Klass));
        }

        public Relation(Type type)
        {
            RelationType = type;
        }

        public Klass From { get; set; }
        public Klass To { get; set; }

        public Point FromLabelPos
        {
            get
            {
                _fromLabelPoint.X = IsSideways()
                    ? (To.X > From.X ? FromPos.X + 5: FromPos.X - 30)
                    : FromPos.X + 5;
                _fromLabelPoint.Y = !IsSideways()
                    ? (To.Y > From.Y ? FromPos.Y + 10 : FromPos.Y - 20)
                    : FromPos.Y + 5;
                    return _fromLabelPoint;
            }
        }
        public Point ToLabelPos
        {
            get
            {
                _toLabelPoint.X = IsSideways()
                    ? (To.X < From.X ? ToPos.X + 15: ToPos.X - 30)
                    : ToPos.X + 5;
                _toLabelPoint.Y = !IsSideways()
                    ? (To.Y < From.Y ? ToPos.Y + 10 : ToPos.Y - 30)
                    : ToPos.Y + 5;
                    return _toLabelPoint;
            }
        }


        public Visibility ContextVisibility
        {
            get { return _contextVisibility; }
            set
            {
                _contextVisibility = value;
                RaisePropertyChanged();
            }
        }

        public Point FromPos
        {
            get
            {
                float fx = From.X;
                float fy = From.Y;

                float tx = To.X;
                float ty = To.Y;

                _from.Y =
                    IsSideways() ? fy + From.Height / 2 :
                    ty < fy ? fy :
                    fy + From.Height;
                _from.X =
                    !(IsSideways()) ? fx + From.Width / 2 :
                    tx > fx ? fx + From.Width :
                    fx;
                return _from;
            }
        }

        public Point ToPos
        {
            get
            {
                float fx = From.X;
                float fy = From.Y;

                float tx = To.X;
                float ty = To.Y;

                _to.Y =
                    IsSideways() ? ty + To.Height / 2 :
                    ty < fy ? ty + To.Height :
                    ty;
                _to.X =
                    !(IsSideways()) ? tx + To.Width / 2 :
                    tx > fx ? tx :
                    tx + To.Width;
                return _to;
            }
        }

        public Point Knack1
        {
            get
            {
                if (IsSideways())
                {
                    _knack1.X = FromPos.X + (ToPos.X - FromPos.X)/2;
                    _knack1.Y = FromPos.Y;
                }
                else
                {
                    _knack1.Y = FromPos.Y + (ToPos.Y - FromPos.Y)/2;
                    _knack1.X = FromPos.X;
                }
                return _knack1;
            }
        }

        public Point Knack2
        {
            get
            {
                if (IsSideways())
                {
                    _knack2.X = FromPos.X + (ToPos.X - FromPos.X)/2;
                    _knack2.Y = ToPos.Y;
                }
                else
                {
                    _knack2.X = ToPos.X;
                    _knack2.Y = FromPos.Y + (ToPos.Y - FromPos.Y)/2;
                }

                return _knack2;
            }
        }

        public Point LineCenter
        {
            get
            {
                if (IsSideways())
                {
                    _center.X = _knack1.X - 10;
                    _center.Y = _knack1.Y + (_knack2.Y - _knack1.Y)/2 - 10;
                }
                else
                {
                    _center.X = _knack1.X + (_knack2.X - _knack1.X) / 2 - 10;
                    _center.Y = _knack1.Y - 10;
                }
                return _center;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("type", RelationType, typeof (Type));
            info.AddValue("From", From, typeof (Klass));
            info.AddValue("To", To, typeof (Klass));
        }

        private Boolean IsSideways()
        {
            float fy = From.Y;
            float ty = To.Y;

            return ty + To.Height + 20 > fy && ty - 20 < fy + From.Height;
        }


        public void Set(Klass from, Klass to)
        {
            From = from;
            To = to;
            Console.WriteLine("Test");
            from.Relations.Add(this);
            to.Relations.Add(this);
        }

        public void Notify()
        {
            RaisePropertyChanged("FromPos");
            RaisePropertyChanged("ToPos");
            RaisePropertyChanged("Knack1");
            RaisePropertyChanged("Knack2");
            RaisePropertyChanged("LineCenter");
            RaisePropertyChanged("FromLabelPos");
            RaisePropertyChanged("ToLabelPos");
        }

        // Throws NullPointer if called before Set
        public void UnSet()
        {
            From.Relations.Remove(this);
            To.Relations.Remove(this);

            From = null;
            To = null;
        }
    }
}

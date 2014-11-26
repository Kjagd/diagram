using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;

namespace Diagram
{
    public class Relation : ViewModelBase
    {
        public enum Type
        {
            Inheritance
        };
        public Klass From { get; set; }
        public Klass To { get; set; }

        private Point _from;
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

        private Point _to;
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

        private Point _knack1;

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

        private Point _knack2;

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

        private Boolean IsSideways()
        {
            var fy = From.Y;
            var ty = To.Y;

            return ty + To.Height + 20 > fy && ty - 20 < fy + From.Height;
        }

        public Type RelationType { get; set; }

        private RelationMultiplicity Multiplicity { get; set; }


        public Relation(Klass from, Klass to)
        {
            this.From = from;
            this.To = to;

            from.Relations.Add(this);
            to.Relations.Add(this);
        }
        public void Notify()
        {
            RaisePropertyChanged("FromPos");
            RaisePropertyChanged("ToPos");
            RaisePropertyChanged("Knack1");
            RaisePropertyChanged("Knack2");
        }
    }
}

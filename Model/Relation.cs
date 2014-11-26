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
                    ty > fy ? fy :
                    ty < fy && ty > fy - From.Height ? fy + From.Height / 2 : 
                    fy + From.Height;
                _from.X = 
                    !(ty < fy && ty > fy - From.Height) ? fx + From.Width / 2 :
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
                    ty > fy ? ty :
                    ty < fy && ty > ty - To.Height ? ty - To.Height / 2 :
                    ty + To.Height;
                _to.X =
                    !(ty < fy && ty > fy - From.Height) ? tx + To.Width / 2 :
                    tx > fx ? tx + To.Width :
                    tx;
                return _to;
            }
        }

        public Type RelationType { get; set; }

        private RelationMultiplicity Multiplicity { get; set; }


        public Relation(Klass from, Klass to)
        {
            this.From = from;
            this.To = to;
        }

        public void Notify()
        {
            RaisePropertyChanged("FromPos");
            RaisePropertyChanged("ToPos");
        }
    }
}

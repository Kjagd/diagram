using Diagram;
using GalaSoft.MvvmLight;

namespace DiagramTool.ViewModel
{
    public class KlassViewModel : ViewModelBase
    {

        private float x;
        public float X { get { return x; } set { x = value; } }

        private float y;
        public float Y { get { return y; } set { y = value; } }

        private float width;
        public float Width { get { return width; } set { width = value; } }

        private float height;
        public float Height { get { return height; } set { height = value; } }

        private Klass klass;
        public Klass Klass { get { return klass; } set { klass = value; } }

        public KlassViewModel()
        {
            width = 150;
            height = 150;
        }
    }
}
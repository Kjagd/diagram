using Diagram;
using GalaSoft.MvvmLight;

namespace DiagramTool.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>
        public KlassViewModel()
        {
            width = 150;
            height = 150;
        }
    }
}
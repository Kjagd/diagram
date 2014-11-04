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
    public class NodeViewModel : ViewModelBase
    {

        public Klass MyKlass { get; set; }

        public NodeViewModel()
        {
        }
    }
}
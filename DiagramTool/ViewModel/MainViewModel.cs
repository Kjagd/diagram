using Diagram;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.ObjectModel;

namespace DiagramTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public ObservableCollection<KlassViewModel> Klasses { get; set; }
        public ObservableCollection<Relation> Relations { get; set; } 

        public MainViewModel()
        {
            Klasses = new ObservableCollection<KlassViewModel>();
            var k = new KlassViewModel {X = 200, Y = 200, Klass = new Klass("Lel")};
            Klasses.Add(k);

            var c = new KlassViewModel {Klass = new Klass("YourMom")};
            Klasses.Add(c);
 
            Relations = new ObservableCollection<Relation>();
            var r = new Inheritance(k.Klass, c.Klass);
        }
    }
}
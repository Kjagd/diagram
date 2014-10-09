using Diagram;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.ObjectModel;

namespace DiagramTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public ObservableCollection<KlassViewModel> Klasses { get; set; }

        public MainViewModel()
        {
            Klasses = new ObservableCollection<KlassViewModel>();
            var k = new KlassViewModel {X = 200, Y = 200, Klass = new Klass("Lel")};
            Klasses.Add(k);

            k = new KlassViewModel {Klass = new Klass("YourMom")};
            Klasses.Add(k);
        }
    }
}
using Diagram;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.ObjectModel;

namespace DiagramTool.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        public ObservableCollection<KlassViewModel> Klasses { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Klasses = new ObservableCollection<KlassViewModel>();
            KlassViewModel k = new KlassViewModel();
            k.X = 200;
            k.Y = 200;
            k.Klass = new Klass("Lel");
            Klasses.Add(k);

            k = new KlassViewModel();
            k.Klass = new Klass("YourMom");
            Klasses.Add(k);
            
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
    }
}
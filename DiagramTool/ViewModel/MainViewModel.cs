using System;
using Diagram;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.ObjectModel;

namespace DiagramTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public ObservableCollection<Klass> Klasses { get; set; }
        public ObservableCollection<Relation> Relations { get; set; } 

        public MainViewModel()
        {
            Klasses = new ObservableCollection<Klass>();
            var k = new Klass("lel") {X = 200, Y = 200};
            
            Klasses.Add(k);
            
            k.AddField(new Field("test", "+"));
            k.AddField(new Field("test2", "+"));
            k.AddField(new Field("test3", "+"));


            var c = new Klass("You're mom");
            Klasses.Add(c);
 
            Relations = new ObservableCollection<Relation>();
            var r = new Inheritance(k, c);
            Relations.Add(r);

            
        }
    }
}
using System;
using System.Security.Authentication.ExtendedProtection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Diagram;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace DiagramTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public ICommand MouseDownCommand { get; private set; }
        public ICommand MouseMoveCommand { get; private set; }
        public ICommand MouseUpCommand { get; private set; }

        public ObservableCollection<Klass> Klasses { get; set; }
        public ObservableCollection<Relation> Relations { get; set; }

        public Point MoveKlassPoint;

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


            MouseDownCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClass);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClass);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMoveClass);

        }

        public void MouseMoveClass(MouseEventArgs e)
        {
            System.Console.Write("pls");
            if (Mouse.Captured != null)
            {
                FrameworkElement frameworkElement = (FrameworkElement) e.MouseDevice.Target;
                Klass draggedKlass = (Klass) frameworkElement.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(frameworkElement);
                Point mousePos = Mouse.GetPosition(canvas);
                if (MoveKlassPoint == default(Point)) MoveKlassPoint = mousePos;
                draggedKlass.X = (int) mousePos.X;
                draggedKlass.Y = (int) mousePos.Y;
            }
        }

        public void MouseUpClass(MouseButtonEventArgs e)
        {
            e.MouseDevice.Target.ReleaseMouseCapture();
        }

        public void MouseDownClass(MouseButtonEventArgs e)
        {
            e.MouseDevice.Target.CaptureMouse();
            System.Console.Write("Hey");
        }
        private static T FindParentOfType<T>(DependencyObject o) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(o);
            if (parent == null) return null;
            return parent is T ? parent as T : FindParentOfType<T>(parent);
        }
    }

}
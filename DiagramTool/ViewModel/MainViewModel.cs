using System;
using System.Security.Authentication.ExtendedProtection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Diagram;
using DiagramTool.Command;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using ICommand = System.Windows.Input.ICommand;

namespace DiagramTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();
        public Point MoveKlassPoint;
        public ICommand MouseDownCommand { get; private set; }
        public ICommand MouseMoveCommand { get; private set; }
        public ICommand MouseUpCommand { get; private set; }

        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }

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


            MouseDownCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClass);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClass);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMoveClass);

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);
        }

        public void MouseMoveClass(MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {
                FrameworkElement frameworkElement = (FrameworkElement) e.MouseDevice.Target;
                Klass draggedKlass = (Klass) frameworkElement.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(frameworkElement);
                Point mousePos = Mouse.GetPosition(canvas);
                if (MoveKlassPoint == default(Point)) MoveKlassPoint = mousePos;
                draggedKlass.X = (int) (mousePos.X-draggedKlass.Width/2);
                draggedKlass.Y = (int) (mousePos.Y-draggedKlass.Height/2);
            }
        }

        public void MouseUpClass(MouseButtonEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement) e.MouseDevice.Target;
            Klass draggedKlass = (Klass) frameworkElement.DataContext;
            Canvas canvas = FindParentOfType<Canvas>(frameworkElement);
            Point mousePos = Mouse.GetPosition(canvas);
            undoRedoController.AddAndExecute(new MoveCommand(draggedKlass, (float) mousePos.X-draggedKlass.Width/2,
                (float) mousePos.Y-draggedKlass.Height/2, (float) MoveKlassPoint.X-draggedKlass.Width/2, (float) MoveKlassPoint.Y-draggedKlass.Height/2)); 
            e.MouseDevice.Target.ReleaseMouseCapture();
            MoveKlassPoint = new Point();
        }

        public void MouseDownClass(MouseButtonEventArgs e)
        {
            //Capture for drag if it's a klass
            if (((FrameworkElement) e.MouseDevice.Target).DataContext is Klass)
            {
                e.MouseDevice.Target.CaptureMouse();
            }
        }
        private static T FindParentOfType<T>(DependencyObject o) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(o);
            if (parent == null) return null;
            return parent is T ? parent as T : FindParentOfType<T>(parent);
        }
    }

}
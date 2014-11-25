using Diagram;
using DiagramTool.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using ICommand = System.Windows.Input.ICommand;

namespace DiagramTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();
        private Klass clipboard;

        private Point MoveKlassPoint;
        private FrameworkElement movingElement;
        private Klass selectedKlass;

        public ICommand MouseDownCommand { get; private set; }
        public ICommand MouseMoveCommand { get; private set; }
        public ICommand MouseUpCommand { get; private set; }

        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }

        public ICommand NewClassCommand { get; set; }
        public ICommand DeleteClassCommand { get; set; }

        public ICommand CopyClassCommand { get; set; }
        public ICommand PasteClassCommand { get; set; }
        public ICommand CutClassCommand { get; set; }

        public ObservableCollection<Klass> Klasses { get; set; }
        public ObservableCollection<Relation> Relations { get; set; }
        public ICommand TitleTextChanged { get; set; }

        public MainViewModel()
        {
            Klasses = new ObservableCollection<Klass>();
            var k = new Klass("Persons") {X = 200, Y = 200};

            Klasses.Add(k);

            k.AddField(new Field("Jonas", "+"));
            k.AddField(new Field("Peter", "+"));
            k.AddField(new Field("Kristian", "+"));

            var c = new Klass("Stuff");
            Klasses.Add(c);

            Relations = new ObservableCollection<Relation>();
            var r = new Relation(k, c) {RelationType = Relation.Type.Inheritance};
            Relations.Add(r);

            MouseDownCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClass);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClass);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMoveClass);

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            NewClassCommand = new RelayCommand(CreateNewKlass);
            DeleteClassCommand = new RelayCommand(DeleteKlass, hasSelection);

            CopyClassCommand = new RelayCommand(CopyKlass, hasSelection);
            PasteClassCommand = new RelayCommand(PasteKlass, canPaste);
            CutClassCommand = new RelayCommand(CutKlass, hasSelection);
        }

        private bool canPaste()
        {
            return clipboard != null;
        }

        private bool hasSelection()
        {
            return selectedKlass != null;
        }

        private void CopyKlass()
        {
            clipboard = (Klass)selectedKlass.Clone();
        }

        private void PasteKlass()
        {
            undoRedoController.AddAndExecute(new NewKlassCommand(Klasses, clipboard));
            selectedKlass.IsSelected = false;
            clipboard.IsSelected = true;
            // Clear clipboard
            clipboard = null;
        }

        private void CutKlass()
        {
            CopyKlass();
            DeleteKlass();
        }
        


        private void ChangeTitle()
        {
            System.Console.WriteLine("Changed");

        }
        private void CreateNewKlass()
        {
            var newKlass = new Klass("New Klass") {X = 300, Y = 300};
            
            undoRedoController.AddAndExecute(new NewKlassCommand(Klasses, newKlass));
        }

        private void DeleteKlass()
        {
            undoRedoController.AddAndExecute(new DeleteKlassCommand(Klasses, selectedKlass));
        }

        public void MouseMoveClass(MouseEventArgs e)
        {
            if (Mouse.Captured != null && movingElement != null)
            {
                Klass draggedKlass = (Klass) movingElement.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingElement);
                Point mousePos = Mouse.GetPosition(canvas);
                if (MoveKlassPoint == default(Point)) MoveKlassPoint = mousePos;
                draggedKlass.X = (int) (mousePos.X-draggedKlass.Width/2);
                draggedKlass.Y = (int) (mousePos.Y-draggedKlass.Height/2);
            }
        }

        public void MouseUpClass(MouseButtonEventArgs e)
        {
            movingElement.Effect = null;
            var klass = movingElement.DataContext as Klass;
            if (klass != null)
            {
                Klass draggedKlass = klass;
            Canvas canvas = FindParentOfType<Canvas>(movingElement);
            Point mousePos = Mouse.GetPosition(canvas);
            undoRedoController.AddAndExecute(new MoveCommand(draggedKlass, (float) mousePos.X-draggedKlass.Width/2,
                (float) mousePos.Y-draggedKlass.Height/2, (float) MoveKlassPoint.X-draggedKlass.Width/2, (float) MoveKlassPoint.Y-draggedKlass.Height/2)); 
            e.MouseDevice.Target.ReleaseMouseCapture();
            MoveKlassPoint = new Point();
        }
        }

        public void MouseDownClass(MouseButtonEventArgs e)
        {
            //Capture for drag if it's a klass
            FrameworkElement frameworkElement = (FrameworkElement) e.MouseDevice.Target;
            if (!(frameworkElement is StackPanel))
            {
                frameworkElement = FindParentOfType<StackPanel>(frameworkElement);
            }
            if (frameworkElement.DataContext is Klass)
            {
                frameworkElement.Effect = new DropShadowEffect {BlurRadius = 20, Opacity = 0.5};
                movingElement = frameworkElement;
                if (selectedKlass != null)
                {
                    selectedKlass.IsSelected = false;
                }
                (frameworkElement.DataContext as Klass).IsSelected = true;
                selectedKlass = (frameworkElement.DataContext as Klass);
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
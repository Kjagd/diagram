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

        private Point moveKlassStartPoint;
        private Point MoveKlassAnchorPoint;
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

            var c = new Klass("Stuff") { X = 400, Y = 350 };
            Klasses.Add(c);

            Relations = new ObservableCollection<Relation>();
            var r = new Relation(k, c) {RelationType = Relation.Type.Composition};
            Relations.Add(r);

            MouseDownCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClass);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClass);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMoveClass);

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            NewClassCommand = new RelayCommand(CreateNewKlass);
            DeleteClassCommand = new RelayCommand(DeleteKlass, HasSelection);

            CopyClassCommand = new RelayCommand(CopyKlass, HasSelection);
            PasteClassCommand = new RelayCommand(PasteKlass, CanPaste);
            CutClassCommand = new RelayCommand(CutKlass, HasSelection);
        }

        private bool CanPaste()
        {
            return clipboard != null;
        }

        private bool HasSelection()
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
                Point relativePos = Mouse.GetPosition(movingElement);
                if (MoveKlassAnchorPoint == default(Point))
                {
                    MoveKlassAnchorPoint.X = relativePos.X;
                    MoveKlassAnchorPoint.Y = relativePos.Y;
                }
                draggedKlass.X += (int) (relativePos.X - MoveKlassAnchorPoint.X);
                draggedKlass.Y += (int) (relativePos.Y - MoveKlassAnchorPoint.Y);
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
            undoRedoController.AddAndExecute(new MoveCommand(draggedKlass, draggedKlass.X,
                 draggedKlass.Y, (float) moveKlassStartPoint.X, (float) moveKlassStartPoint.Y)); 
            e.MouseDevice.Target.ReleaseMouseCapture();
            MoveKlassAnchorPoint = new Point();
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
                moveKlassStartPoint.X = selectedKlass.X;
                moveKlassStartPoint.Y = selectedKlass.Y;
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
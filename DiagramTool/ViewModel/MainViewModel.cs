using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Diagram;
using DiagramTool.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Microsoft.Win32;
using ICommand = System.Windows.Input.ICommand;
using System.Runtime.Serialization;

namespace DiagramTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();
        private Klass clipboard;

        private Point moveKlassStartPoint;
        private Point MoveKlassAnchorPoint;
        private FrameworkElement movingElement;
        private Klass _selectedKlass;
        private Klass _relation1Klass;
        private Relation.Type _relationType;

        private bool _isAddingRelation;

        public ICommand MouseDownCommand { get; private set; }
        public ICommand MouseMoveCommand { get; private set; }
        public ICommand MouseUpCommand { get; private set; }

        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }

        public ICommand NewClassCommand { get; set; }
        public ICommand DeleteClassCommand { get; set; }

        public ICommand AddRelationCommand { get; set; }
        public ICommand AddInheritanceRelationCommand { get; set; }
        public ICommand AddCompositionRelationCommand { get; set; }
        public ICommand AddReferenceRelationCommand { get; set; }

        public ICommand CopyClassCommand { get; set; }
        public ICommand PasteClassCommand { get; set; }
        public ICommand CutClassCommand { get; set; }

        public ICommand NewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand ExportCommand { get; set; }

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

            MouseDownCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClass);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClass);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMoveClass);

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            NewClassCommand = new RelayCommand(CreateNewKlass);
            DeleteClassCommand = new RelayCommand(DeleteKlass, HasSelection);

            AddRelationCommand = new RelayCommand(AddRelation);
            AddCompositionRelationCommand = new RelayCommand(AddCompostion);
            AddInheritanceRelationCommand = new RelayCommand(AddInheritance);
            AddReferenceRelationCommand = new RelayCommand(AddReference);

            CopyClassCommand = new RelayCommand(CopyKlass, HasSelection);
            PasteClassCommand = new RelayCommand(PasteKlass, CanPaste);
            CutClassCommand = new RelayCommand(CutKlass, HasSelection);

            NewCommand = new RelayCommand(New);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);
            ExportCommand = new RelayCommand<Canvas>(Export);


        }

        private void Export(Canvas canvas)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Export Diagram";
            dialog.Filter = "Portable Networks Graphics files (*.png)|*.png";
            dialog.RestoreDirectory = true;
            if ((bool) dialog.ShowDialog())
            {
                Rect r = VisualTreeHelper.GetDescendantBounds(canvas);

                var encoder = new PngBitmapEncoder();
                RenderTargetBitmap bitmap = new RenderTargetBitmap((int) r.Right, (int) r.Bottom, 96, 96, PixelFormats.Default);
                bitmap.Render(ClipImageToBounds(canvas));
                BitmapFrame frame = BitmapFrame.Create(bitmap);
                encoder.Frames.Add(frame);

                using (var stream = File.Create(dialog.FileName))
                {
                    encoder.Save(stream);
                }

            }
        }

        private DrawingVisual ClipImageToBounds(Visual v)
        {
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();
            VisualBrush vb = new VisualBrush(v);
            Rect r = VisualTreeHelper.GetDescendantBounds(v);

            dc.DrawRectangle(vb, null, new Rect(new Point(), new Point(r.Right, r.Bottom)));
            dc.Close();

            return dv;
        }


        private void New()
        {
            undoRedoController.AddAndExecute(new NewDiagramCommand(Klasses,Relations));
        }
        private void Save()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save Diagram";
            dialog.Filter = "Diagram files (*.dia)|*.dia";
            dialog.RestoreDirectory = true;

            if((bool)dialog.ShowDialog())
            {
                // Create an instance of the type and serialize it.
                IFormatter formatter = new BinaryFormatter();

                FileStream s = new FileStream(dialog.FileName, FileMode.Create);
                formatter.Serialize(s, Klasses);
                s.Close();
            }

        }

        private void Load()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open Diagram";
            dialog.Filter = "Diagram files (*.dia)|*.dia";
            dialog.RestoreDirectory = true;

            if ((bool) dialog.ShowDialog())
            {
                // Load data from file
                IFormatter formatter = new BinaryFormatter();
                FileStream s = new FileStream(dialog.FileName, FileMode.Open);
                ObservableCollection<Klass> t = (ObservableCollection<Klass>) formatter.Deserialize(s);

                // Clear existing Klasses and Relations
                Klasses.Clear();
                Relations.Clear();

                // Add loaded data
                foreach (Klass k in t)
                {
                    // Add Klass
                    Klasses.Add(k);

                    // Add Relations
                    foreach (Relation r in k.Relations)
                    {
                        if (!Relations.Contains(r))
                        {
                            Relations.Add(r);
                        }
                    }
                }
            }
        }

        private void AddReference()
        {
            _relationType = Relation.Type.Reference;
            AddRelation();
        }

        private void AddInheritance()
        {
            _relationType = Relation.Type.Inheritance;
            AddRelation();
        }

        private void AddCompostion()
        {
            _relationType = Relation.Type.Composition;
            AddRelation();
        }

        private void AddRelation()
        {
            _isAddingRelation = true;
            if (_selectedKlass != null)
            {
                _selectedKlass.IsSelected = false;
                _selectedKlass = null;                
            }
            //TODO: Toggle some viewmode perhaps
        }

        private bool CanPaste()
        {
            return clipboard != null;
        }

        private bool HasSelection()
        {
            return _selectedKlass != null;
        }

        private void CopyKlass()
        {
            clipboard = (Klass)_selectedKlass.Clone();
        }

        private void PasteKlass()
        {
            undoRedoController.AddAndExecute(new NewKlassCommand(Klasses, clipboard));
            _selectedKlass.IsSelected = false;
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
            var newKlass = new Klass("New Klass") {X = 300, Y = 200};
            
            undoRedoController.AddAndExecute(new NewKlassCommand(Klasses, newKlass));
        }

        private void DeleteKlass()
        {
            undoRedoController.AddAndExecute(new DeleteKlassCommand(Klasses, _selectedKlass));
        }

        public void MouseMoveClass(MouseEventArgs e)
        {
            if (Mouse.Captured != null && movingElement != null && movingElement.DataContext is Klass)
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
            if (movingElement == null) return;
            var klass = movingElement.DataContext as Klass;
            movingElement.Effect = null;
            if (_isAddingRelation)
            {
                if (_relation1Klass == null)
                {
                    _selectedKlass = klass;
                    _selectedKlass.IsSelected = true;
                    _relation1Klass = klass;
                }
                else
                {
                    undoRedoController.AddAndExecute(new AddRelationCommand(Relations, _selectedKlass, klass, _relationType));
                    _relation1Klass = null;
                    _isAddingRelation = false;
                }
            }
            else
            {
                undoRedoController.AddAndExecute(new MoveCommand(klass, klass.X,
                    klass.Y, (float)moveKlassStartPoint.X, (float)moveKlassStartPoint.Y));
                e.MouseDevice.Target.ReleaseMouseCapture();
                MoveKlassAnchorPoint = new Point();
            }
        }

        public void MouseDownClass(MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            //Capture for drag if it's a klass
            var frameworkElement = (FrameworkElement) e.MouseDevice.Target;
            //if (!(frameworkElement is StackPanel))
            //{
                //frameworkElement = FindParentOfType<StackPanel>(frameworkElement);
            //}
            if (frameworkElement.DataContext is Klass)
            {
                frameworkElement.Effect = new DropShadowEffect {BlurRadius = 20, Opacity = 0.5};
                movingElement = frameworkElement;
                if (_isAddingRelation) return;
                if (_selectedKlass != null)
                {
                    _selectedKlass.IsSelected = false;
                }
                (frameworkElement.DataContext as Klass).IsSelected = true;
                _selectedKlass = (frameworkElement.DataContext as Klass);
                moveKlassStartPoint.X = _selectedKlass.X;
                moveKlassStartPoint.Y = _selectedKlass.Y;
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
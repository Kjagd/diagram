using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
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
        private readonly UndoRedoController _undoRedoController = UndoRedoController.GetInstance();
        private Klass _clipboard;

        private Point _moveKlassStartPoint;
        private Point _moveKlassAnchorPoint;
        private FrameworkElement _movingElement;
        private Klass _selectedKlass;
        private Klass _relation1Klass;
        private Relation.Type _relationType;

        private bool _isAddingRelation;
        private bool _isDeletingRelation;

        private string _filepath;

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
        public ICommand DeleteRelationCommand { get; set; }

        public ICommand CopyClassCommand { get; set; }
        public ICommand PasteClassCommand { get; set; }
        public ICommand CutClassCommand { get; set; }

        public ICommand NewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveAsCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        public ObservableCollection<Klass> Klasses { get; set; }
        public ObservableCollection<Relation> Relations { get; set; }
        public ICommand TitleTextChanged { get; set; }


        public MainViewModel()
        {
            Klasses = new ObservableCollection<Klass>();
            Relations = new ObservableCollection<Relation>();

            MouseDownCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClass);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClass);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMoveClass);

            UndoCommand = new RelayCommand(_undoRedoController.Undo, _undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(_undoRedoController.Redo, _undoRedoController.CanRedo);

            NewClassCommand = new RelayCommand(CreateNewKlass);
            DeleteClassCommand = new RelayCommand(DeleteKlass, HasSelection);

            AddRelationCommand = new RelayCommand(AddRelation);
            AddCompositionRelationCommand = new RelayCommand(AddCompostion);
            AddInheritanceRelationCommand = new RelayCommand(AddInheritance);
            AddReferenceRelationCommand = new RelayCommand(AddReference);

            DeleteRelationCommand = new RelayCommand(DeleteRelation, HasRelation);

            CopyClassCommand = new RelayCommand(CopyKlass, HasSelection);
            PasteClassCommand = new RelayCommand(PasteKlass, CanPaste);
            CutClassCommand = new RelayCommand(CutKlass, HasSelection);

            NewCommand = new RelayCommand(New);
            SaveCommand = new RelayCommand(Save);
            SaveAsCommand = new RelayCommand(SaveAs);
            LoadCommand = new RelayCommand(Load);
            ExportCommand = new RelayCommand<Canvas>(Export);
        }

        private bool HasRelation()
        {
            return Relations.Count > 0;
        }

        private void DeleteRelation()
        {
            _isDeletingRelation = true;
            if (_selectedKlass != null)
            {
                _selectedKlass.IsSelected = false;
                _selectedKlass = null;
            }
            foreach (var relation in Relations)
            {
                relation.ContextVisibility = Visibility.Visible;
            }
        }

        private void Export(Visual visual)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Export Diagram",
                Filter = "Portable Networks Graphics files (*.png)|*.png",
                RestoreDirectory = true
            };
            if ((bool) dialog.ShowDialog())
            {
                var r = VisualTreeHelper.GetDescendantBounds(visual);

                var encoder = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int) r.Right, (int) r.Bottom, 96, 96, PixelFormats.Default);
                bitmap.Render(ClipImageToBounds(visual));
                var frame = BitmapFrame.Create(bitmap);
                encoder.Frames.Add(frame);

                using (var stream = File.Create(dialog.FileName))
                {
                    encoder.Save(stream);
                }

            }
        }

        private DrawingVisual ClipImageToBounds(Visual v)
        {
            var dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var vb = new VisualBrush(v);
            var r = VisualTreeHelper.GetDescendantBounds(v);

            dc.DrawRectangle(vb, null, new Rect(new Point(), new Point(r.Right, r.Bottom)));
            dc.Close();

            return dv;
        }


        private void New()
        {
            _undoRedoController.AddAndExecute(new NewDiagramCommand(Klasses,Relations));
        }

        private void Save()
        {
            


            if (_filepath == null)
            {
                SaveAs();
            }
            else
            {
                Thread t = new Thread(new ThreadStart(Serialize));
                t.Start();

            }
            
        }

        private void Serialize()
        {
                // Create an instance of the type and serialize it.
                IFormatter formatter = new BinaryFormatter();

                FileStream s = new FileStream(_filepath, FileMode.Create);
                formatter.Serialize(s, Klasses);
                s.Close();
            }
            
        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save Diagram";
            dialog.Filter = "Diagram files (*.dia)|*.dia";
            dialog.RestoreDirectory = true;

            if((bool)dialog.ShowDialog())
            {
                _filepath = dialog.FileName;
                Save();
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

                try
                {
                    ObservableCollection<Klass> t = (ObservableCollection<Klass>)formatter.Deserialize(s);
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

                    _filepath = dialog.FileName;

                }
                catch (Exception)
                {
                    MessageBox.Show("Please choose a valid Diagram file.");
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
            _relation1Klass = null;
            if (_selectedKlass != null)
            {
                _selectedKlass.IsSelected = false;
                _selectedKlass = null;                
            }
            //TODO: Toggle some viewmode perhaps
        }

        private bool CanPaste()
        {
            return _clipboard != null;
        }

        private bool HasSelection()
        {
            return _selectedKlass != null;
        }

        private void CopyKlass()
        {
            _clipboard = (Klass)_selectedKlass.Clone();
        }

        private void PasteKlass()
        {
            _undoRedoController.AddAndExecute(new NewKlassCommand(Klasses, _clipboard));
            _selectedKlass.IsSelected = false;
            _clipboard.IsSelected = true;
            // Clear clipboard
            _clipboard = null;
        }

        private void CutKlass()
        {
            CopyKlass();
            DeleteKlass();
        }
        

        private void CreateNewKlass()
        {
            var newKlass = new Klass("New Klass") {X = 300, Y = 200};
            
            _undoRedoController.AddAndExecute(new NewKlassCommand(Klasses, newKlass));
        }

        private void DeleteKlass()
        {
            _undoRedoController.AddAndExecute(new DeleteKlassCommand(Klasses, Relations, _selectedKlass));
        }

        public void MouseMoveClass(MouseEventArgs e)
        {
            if (Mouse.Captured != null && _movingElement != null && _movingElement.DataContext is Klass)
            {
                Klass draggedKlass = (Klass) _movingElement.DataContext;
                Point relativePos = Mouse.GetPosition(_movingElement);
                if (_moveKlassAnchorPoint == default(Point))
                {
                    _moveKlassAnchorPoint.X = relativePos.X;
                    _moveKlassAnchorPoint.Y = relativePos.Y;
                }
                draggedKlass.X += (int) (relativePos.X - _moveKlassAnchorPoint.X);
                draggedKlass.Y += (int) (relativePos.Y - _moveKlassAnchorPoint.Y);
            }
        }

        public void MouseUpClass(MouseButtonEventArgs e)
        {
            if (_movingElement == null) return;
            var klass = _movingElement.DataContext as Klass;
            _movingElement.Effect = null;
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
                    _undoRedoController.AddAndExecute(new AddRelationCommand(Relations, _relation1Klass, klass, _relationType));
                    _relation1Klass = null;
                    _isAddingRelation = false;
                }
            }
            else
            {
                _undoRedoController.AddAndExecute(new MoveCommand(klass, klass.X,
                    klass.Y, (float)_moveKlassStartPoint.X, (float)_moveKlassStartPoint.Y));
                e.MouseDevice.Target.ReleaseMouseCapture();
                _moveKlassAnchorPoint = new Point();
            }
        }

        private void clearSelection()
        {
            if (_selectedKlass != null)
            {
                _selectedKlass.IsSelected = false;
                _selectedKlass = null;
            }
        }

        public void MouseDownClass(MouseButtonEventArgs e)
        {

            //Capture for drag if it's a klass
            var frameworkElement = (FrameworkElement) e.MouseDevice.Target;

            // Focus clicked object
            frameworkElement.Focus();
            clearSelection();


            if (frameworkElement.DataContext is Klass)
            {
                frameworkElement.Effect = new DropShadowEffect {BlurRadius = 20, Opacity = 0.5};
                _movingElement = frameworkElement;
                if (_isAddingRelation) return;
                if (_selectedKlass != null)
                {
                    _selectedKlass.IsSelected = false;
                }
                (frameworkElement.DataContext as Klass).IsSelected = true;
                _selectedKlass = (frameworkElement.DataContext as Klass);
                _moveKlassStartPoint.X = _selectedKlass.X;
                _moveKlassStartPoint.Y = _selectedKlass.Y;
                e.MouseDevice.Target.CaptureMouse();
            }
            else if (frameworkElement.DataContext is Relation)
            {
                Console.WriteLine("Relation");
                if (_isDeletingRelation)
                {
                    _isDeletingRelation = false;
                    foreach (var relation in Relations)
                    {
                        relation.ContextVisibility = Visibility.Hidden;
                    }
                    _undoRedoController.AddAndExecute(new DeleteRelationCommand(Relations, frameworkElement.DataContext as Relation));
                }
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
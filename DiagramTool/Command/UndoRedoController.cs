using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

//From the demo
namespace DiagramTool.Command
{
    public class UndoRedoController
    {
        // Part of singleton pattern.
        private static UndoRedoController controller = new UndoRedoController();

        // Undo stack.
        private readonly Stack<IUndoRedoCommand> undoStack = new Stack<IUndoRedoCommand>();
        // Redo stack.
        private readonly Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        // Part of singleton pattern.
        public UndoRedoController() { }

        // Part of singleton pattern.
        public static UndoRedoController GetInstance() { return controller; }

        // Bruges til at tilføje commander.
        public void AddAndExecute(IUndoRedoCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
            command.Execute();
        }

        // Sørger for at undo kun kan kaldes når der er kommandoer i undo stacken.
        public bool CanUndo()
        {
            return undoStack.Any();
        }

        // Udfører undo hvis det kan lade sig gøre.
        public void Undo()
        {
            if (undoStack.Count() <= 0) throw new InvalidOperationException();
            IUndoRedoCommand command = undoStack.Pop();
            redoStack.Push(command);
            command.Undo();
        }

        // Sørger for at redo kun kan kaldes når der er kommandoer i redo stacken.
        public bool CanRedo()
        {
            return redoStack.Any();
        }

        // Udfører redo hvis det kan lade sig gøre.
        public void Redo()
        {
            if (redoStack.Count() <= 0) throw new InvalidOperationException();
            IUndoRedoCommand command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();
        }
    }
}

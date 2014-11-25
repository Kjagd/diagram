using Diagram;

namespace DiagramTool.Command
{
    public class MoveCommand : IUndoRedoCommand
    {
        private Klass _node;
        private float _oldX;
        private float _oldY;
        private float _newX;
        private float _newY;

        public MoveCommand(Klass node, float newX, float newY, float oldX, float oldY)
        {
            _node = node;
            _oldX = oldX;
            _oldY = oldY;
            _newX = newX;
            _newY = newY;
        }

        public void Undo()
        {
            _node.X = _oldX;
            _node.Y = _oldY;
        }

        public void Execute()
        {
            _node.X = _newX;
            _node.Y = _newY;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diagram;

namespace Command
{
    public class MoveCommand : IUndoRedoCommand
    {
        private Klass _node;
        private float _x;
        private float _y;
        private float _newX;
        private float _newY;

        public MoveCommand(Klass node, float newX, float newY)
        {
            _node = node;
            _x = node.X;
            _y = node.Y;
            _newX = newX;
            _newY = newY;
        }

        public void Undo()
        {
            _node.X = _x;
            _node.Y = _y;
        }

        public void Execute()
        {
            _node.X = _newX;
            _node.Y = _newY;
        }
    }
}

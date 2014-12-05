using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diagram;

namespace DiagramTool.Command
{
    class DeleteRelationCommand : IUndoRedoCommand
    {
        private readonly Collection<Relation> _relations;
        private readonly Relation _toDelete;

        public DeleteRelationCommand(Collection<Relation> relations, Relation toDelete)
        {
            _relations = relations;
            _toDelete = toDelete;
        }

        public void Undo()
        {
            _relations.Add(_toDelete);
        }

        public void Execute()
        {
            _relations.Remove(_toDelete);
        }
    }
}

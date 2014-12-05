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
        private readonly Klass _from;
        private readonly Klass _to;

        public DeleteRelationCommand(Collection<Relation> relations, Relation toDelete)
        {
            _relations = relations;
            _toDelete = toDelete;
            _from = _toDelete.From;
            _to = _toDelete.To;
        }

        public void Undo()
        {
            _relations.Add(_toDelete);
            _toDelete.Set(_from, _to);
        }

        public void Execute()
        {
            _relations.Remove(_toDelete);
            _toDelete.UnSet();
        }
    }
}

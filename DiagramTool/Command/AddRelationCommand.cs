using System.Collections.ObjectModel;
using Diagram;

namespace DiagramTool.Command
{
    public class AddRelationCommand : IUndoRedoCommand
    {
        private readonly Klass _klass1;
        private readonly Klass _klass2;
        private readonly Relation _relation;
        private readonly ObservableCollection<Relation> _relations;

        public AddRelationCommand(ObservableCollection<Relation> relations, Klass klass1, Klass klass2,
            Relation.Type relationType)
        {
            _klass1 = klass1;
            _klass2 = klass2;
            _relation = new Relation(relationType);
            _relations = relations;
        }

        public void Execute()
        {
            _relation.Set(_klass1, _klass2);
            _relations.Add(_relation);
        }

        public void Undo()
        {
            _relations.Remove(_relation);
            _klass1.Relations.Remove(_relation);
            _klass2.Relations.Remove(_relation);
        }
    }
}
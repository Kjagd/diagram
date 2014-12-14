using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diagram;

namespace DiagramTool.Command
{
    public class DeleteKlassCommand : IUndoRedoCommand
    {
        private readonly ObservableCollection<Klass> _klassList;
        private readonly Collection<Relation> _relations; 
        private readonly Klass _classToDelete;
        private readonly Collection<Relation> _deletedRelations = new Collection<Relation>();

        public DeleteKlassCommand(ObservableCollection<Klass> klassList, Collection<Relation> relations, Klass klassToDelete)
        {
            _klassList = klassList;
            _classToDelete = klassToDelete;
            _relations = relations;
        }

        public void Undo()
        {
            _klassList.Add(_classToDelete);
            foreach (Relation r in _deletedRelations)
            {
                _relations.Add(r);
            }
            
        }

        public void Execute()
        {
            _klassList.Remove(_classToDelete);
            foreach (Relation r in _classToDelete.Relations)
            {
                _deletedRelations.Add(r);
                _relations.Remove(r);
            }
        }
    }
}

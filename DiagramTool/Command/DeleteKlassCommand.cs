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
        private readonly Klass _classToDelete;

        public DeleteKlassCommand(ObservableCollection<Klass> klassList, Klass klassToDelete)
        {
            _klassList = klassList;
            _classToDelete = klassToDelete;
        }

        public void Undo()
        {
            _klassList.Add(_classToDelete);
        }

        public void Execute()
        {
            _klassList.Remove(_classToDelete);
        }
    }
}

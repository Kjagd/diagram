using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diagram;

namespace DiagramTool.Command
{
    public class NewKlassCommand : IUndoRedoCommand
    {
        private readonly ObservableCollection<Klass> _klassList;
        private readonly Klass _newKlass;

        public NewKlassCommand(ObservableCollection<Klass> klassList, Klass newKlass)
        {
            _klassList = klassList;
            _newKlass = newKlass;
        }

        public void Undo()
        {
            _klassList.Remove(_newKlass);
        }

        public void Execute()
        {
            _klassList.Add(_newKlass);
        }
    }
}

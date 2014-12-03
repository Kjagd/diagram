using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diagram;

namespace DiagramTool.Command
{
    public class NewDiagramCommand : IUndoRedoCommand
    {
        private readonly ObservableCollection<Klass> _klassList;
        private readonly ObservableCollection<Relation> _relationList;

        private readonly ObservableCollection<Klass> _oldKlasses;
        private readonly ObservableCollection<Relation> _oldRelations;

        public NewDiagramCommand(ObservableCollection<Klass> klassList, ObservableCollection<Relation> relationList)
        {
            _klassList = klassList;
            _oldKlasses = new ObservableCollection<Klass>();
            foreach (Klass k in _klassList)
            {
                _oldKlasses.Add(k);
            }

            _relationList = relationList;
            _oldRelations = new ObservableCollection<Relation>();
            foreach (Relation r in _relationList)
            {
                _oldRelations.Add(r);
            }

        }

        public void Undo()
        {
            foreach (Klass k in _oldKlasses)
            {
                _klassList.Add(k);
            }
            foreach (Relation r in _oldRelations)
            {
                _relationList.Add(r);
            }
        }

        public void Execute()
        {
            _klassList.Clear();
            _relationList.Clear();
        }
    }
}

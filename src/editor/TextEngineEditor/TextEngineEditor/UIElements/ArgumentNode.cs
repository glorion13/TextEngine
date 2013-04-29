using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TextEngineEditor
{
    public class ArgumentNode : INode
    {
        private string type;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                Set(() => Type, ref type, value);
            }
        }
        public ObservableCollection<string> Types { get; set; }

        private string selection;
        public string Selection
        {
            get
            {
                return selection;
            }
            set
            {
                Set(() => Selection, ref selection, value);
            }
        }
        public ObservableCollection<INode> DropdownList { get; set; }

        public ArgumentNode()
        {
        }
    }
}

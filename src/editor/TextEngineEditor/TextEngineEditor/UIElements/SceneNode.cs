using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngineEditor
{
    public class SceneNode : ViewModelBase
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                Set(() => Name, ref name, value);
            }
        }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                Set(() => Description, ref description, value);
            }
        }

        public ObservableCollection<ActionNode> Actions { get; private set; }

        public SceneNode()
        {
            Name = "New Scene";
            Description = "This is a new scene.";
            Actions = new ObservableCollection<ActionNode>();
        }
        public SceneNode(string name)
        {
            Name = name;
            Description = "This is a new scene.";
            Actions = new ObservableCollection<ActionNode>();
        }
        public SceneNode(string name, string description)
        {
            Name = name;
            Description = description;
            Actions = new ObservableCollection<ActionNode>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngineEditor
{
    public class SceneNode : INode
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
        private int id;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                Set(() => ID, ref id, value);
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

        public ObservableCollection<INode> Actions { get; private set; }
        public ObservableCollection<INode> Resources { get; private set; }

        public SceneNode()
        {
            Name = "New Scene";
            Description = "This is a new scene.";
            Actions = new ObservableCollection<INode>();
            Resources = new ObservableCollection<INode>();
        }
        public SceneNode(string name)
        {
            Name = name;
            Description = "This is a new scene.";
            Actions = new ObservableCollection<INode>();
            Resources = new ObservableCollection<INode>();
        }
        public SceneNode(string name, string description)
        {
            Name = name;
            Description = description;
            Actions = new ObservableCollection<INode>();
            Resources = new ObservableCollection<INode>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TextEngineEditor
{
    public class ActionNode : INode
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
        private bool visible;
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                Set(() => Visible, ref visible, value);
            }
        }
        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                Set(() => Enabled, ref enabled, value);
            }
        }
        private bool active;
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                Set(() => Active, ref active, value);
            }
        }
        public ObservableCollection<ConditionNode> Conditions { get; private set; }
        public ObservableCollection<EffectNode> EffectsIfTrue { get; private set; }
        public ObservableCollection<EffectNode> EffectsIfFalse { get; private set; }
        public ObservableCollection<string> Keywords { get; private set; }
        
        public ActionNode()
        {
            Name = "New Action";
            Visible = true;
            Enabled = true;
            Active = true;
            Conditions = new ObservableCollection<ConditionNode>();
            EffectsIfTrue = new ObservableCollection<EffectNode>();
            EffectsIfFalse = new ObservableCollection<EffectNode>();
            Keywords = new ObservableCollection<string>();
        }
        public ActionNode(string name)
        {
            Name = name;
            Visible = true;
            Enabled = true;
            Active = true;
            Conditions = new ObservableCollection<ConditionNode>();
            EffectsIfTrue = new ObservableCollection<EffectNode>();
            EffectsIfFalse = new ObservableCollection<EffectNode>();
            Keywords = new ObservableCollection<string>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

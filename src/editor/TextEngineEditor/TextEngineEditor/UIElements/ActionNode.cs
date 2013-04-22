using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TextEngineEditor
{
    public class ActionNode : ViewModelBase
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
        private bool isVisible;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                Set(() => IsVisible, ref isVisible, value);
            }
        }
        public ObservableCollection<ConditionNode> Conditions { get; private set; }
        public ObservableCollection<EffectNode> EffectsIfTrue { get; private set; }
        public ObservableCollection<EffectNode> EffectsIfFalse { get; private set; }
        
        public ActionNode()
        {
            Name = "New Action";
            IsVisible = true;
            Conditions = new ObservableCollection<ConditionNode>();
            EffectsIfTrue = new ObservableCollection<EffectNode>();
            EffectsIfFalse = new ObservableCollection<EffectNode>();
        }
        public ActionNode(string name)
        {
            Name = name;
            IsVisible = true;
            Conditions = new ObservableCollection<ConditionNode>();
            EffectsIfTrue = new ObservableCollection<EffectNode>();
            EffectsIfFalse = new ObservableCollection<EffectNode>();
        }
        public ActionNode(string name, bool isVisible)
        {
            Name = name;
            IsVisible = isVisible;
            Conditions = new ObservableCollection<ConditionNode>();
            EffectsIfTrue = new ObservableCollection<EffectNode>();
            EffectsIfFalse = new ObservableCollection<EffectNode>();
        }
    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TextEngineEditor
{
    public class EffectNode : ViewModelBase
    {
        private string effectFunction;
        public string EffectFunction
        {
            get
            {
                return effectFunction;
            }
            set
            {
                Set(() => EffectFunction, ref effectFunction, value);
            }
        }

        public ObservableCollection<KeyValuePair<string, string>> arguments { get; set; }

        public EffectNode()
        {
            EffectFunction = "";
            arguments = new ObservableCollection<KeyValuePair<string, string>>();
        }
    }
}

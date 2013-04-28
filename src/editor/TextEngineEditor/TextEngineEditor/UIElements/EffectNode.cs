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
                Arguments.Clear();
                dynamic function = Mvm.PythonCore.components.customisable.effects.EffectFunctions().effectDict[EffectFunction];
                for (int i = 0; i < function.func_code.co_argcount; i++)
                {
                    if (function.func_code.co_varnames[i] != "self")
                    {
                        if (function.func_code.co_varnames[i] == "scene")
                        {
                            Arguments.Add(new KeyValuePair<string, string>("Scene", ""));
                        }
                    }
                }
            }
        }

        public TextEngineEditor.ViewModel.MainViewModel Mvm { get; set; }
        public ObservableCollection<KeyValuePair<string, string>> Arguments { get; set; }

        public EffectNode(TextEngineEditor.ViewModel.MainViewModel mvm)
        {
            Arguments = new ObservableCollection<KeyValuePair<string, string>>();
            Mvm = mvm;
            EffectFunction = "Tell player";
        }
    }
}

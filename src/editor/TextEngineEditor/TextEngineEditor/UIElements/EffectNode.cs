using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
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

        private int argumentCount;
        public int ArgumentCount
        {
            get
            {
                return argumentCount;
            }
            set
            {
                Set(() => ArgumentCount, ref argumentCount, value);
            }
        }

        public EffectNode()
        {
            EffectFunction = "";
        }
    }
}

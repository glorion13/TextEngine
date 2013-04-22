using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEngineEditor
{
    public class ConditionNode : ViewModelBase
    {
        private string conditionType;
        public string ConditionType
        {
            get
            {
                return conditionType;
            }
            set
            {
                Set(() => ConditionType, ref conditionType, value);
            }
        }

        public ConditionNode()
        {
        }
    }
}

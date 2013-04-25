using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private string leftHandSideType;
        public string LeftHandSideType
        {
            get
            {
                return leftHandSideType;
            }
            set
            {
                Set(() => LeftHandSideType, ref leftHandSideType, value);
            }
        }
        private string rightHandSideType;
        public string RightHandSideType
        {
            get
            {
                return rightHandSideType;
            }
            set
            {
                Set(() => RightHandSideType, ref rightHandSideType, value);
            }
        }

        private string leftHandSideValue;
        public string LeftHandSideValue
        {
            get
            {
                return leftHandSideValue;
            }
            set
            {
                Set(() => LeftHandSideValue, ref leftHandSideValue, value);
            }
        }
        private string rightHandSideValue;
        public string RightHandSideValue
        {
            get
            {
                return rightHandSideValue;
            }
            set
            {
                Set(() => RightHandSideValue, ref rightHandSideValue, value);
            }
        }

        public ConditionNode()
        {
            ConditionType = "";
        }
    }
}

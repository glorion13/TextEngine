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

        public ObservableCollection<string> LeftHandSideAutofill { get; set; }
        public ObservableCollection<string> RightHandSideAutofill { get; set; }

        public ConditionNode()
        {
            ConditionType = "";
            LeftHandSideAutofill = new ObservableCollection<string>();
            RightHandSideAutofill = new ObservableCollection<string>();
        }
    }
}

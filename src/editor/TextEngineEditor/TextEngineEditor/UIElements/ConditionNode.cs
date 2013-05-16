/// <copyright>
/// Copyright (c) 2013 ICRL
/// See the file license.txt for copying permission.
/// </copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TextEngineEditor
{
    public class ConditionNode : INode
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
                UpdateLeftAutofill();
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
                UpdateRightAutofill();
            }
        }

        private void UpdateLeftAutofill()
        {
            if (LeftHandSideType == "Boolean")
            {
                LeftHandSideAutofill = new ObservableCollection<string>() { "True", "False" };
            }
        }
        private void UpdateRightAutofill()
        {
            if (RightHandSideType == "Boolean")
            {
                RightHandSideAutofill = new ObservableCollection<string>() { "True", "False" };
            }
        }

        public ObservableCollection<string> LeftHandSideAutofill { get; set; }
        public ObservableCollection<string> RightHandSideAutofill { get; set; }

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

        public dynamic PythonCore { get; set; }

        public ConditionNode(dynamic pythonCore)
        {
            ConditionType = "";
            PythonCore = pythonCore;
        }
    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngineEditor
{
    public class ResourceNode : ViewModelBase
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

        private string resourceValue;
        public string Value
        {
            get
            {
                return resourceValue;
            }
            set
            {
                Set(() => Value, ref resourceValue, value);
            }
        }

        public ResourceNode(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public override string ToString()
        {
            return "";
        }
    }
}

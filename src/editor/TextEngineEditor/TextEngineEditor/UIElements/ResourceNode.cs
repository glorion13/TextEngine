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
        private int value;

        public ResourceNode(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

    }
}

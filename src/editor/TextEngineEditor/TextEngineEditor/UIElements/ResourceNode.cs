/// <copyright>
/// Copyright (c) 2013 ICRL
/// See the file license.txt for copying permission.
/// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngineEditor
{
    public class ResourceNode : INode
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

        private string type;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                Set(() => Type, ref type, value);
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
        public ResourceNode()
        {
            Name = "New Resource";
            Value = "";
            Type = "Text";
        }
        public ResourceNode(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

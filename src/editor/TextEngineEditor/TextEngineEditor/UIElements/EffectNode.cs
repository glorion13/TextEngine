using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TextEngineEditor
{
    public class EffectNode : INode
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
                if (!(EffectFunction == "") && !(EffectFunction == null))
                {
                    dynamic function = Mvm.PythonCore.components.customisable.effects.EffectFunctions().effectDict[EffectFunction];
                    for (int i = 0; i < function.func_code.co_argcount; i++)
                    {
                        if (function.func_code.co_varnames[i] != "self")
                        {
                            ArgumentNode tmpArg = new ArgumentNode();
                            if (function.func_code.co_varnames[i] == "scene")
                            {
                                tmpArg.Types = new ObservableCollection<string>() { "Scene" };
                                tmpArg.Type = "Scene";
                                tmpArg.DropdownList = Mvm.SceneNodes;
                                Arguments.Add(tmpArg);
                            }
                            else if (function.func_code.co_varnames[i] == "gresource")
                            {
                                tmpArg.Types = new ObservableCollection<string>() { "Resource" };
                                tmpArg.Type = "Resource";
                                tmpArg.DropdownList = Mvm.GlobalResourceNodes;
                                Arguments.Add(tmpArg);
                            }
                            else if (function.func_code.co_varnames[i] == "lresource")
                            {
                                tmpArg.Types = new ObservableCollection<string>() { "Resource" };
                                tmpArg.Type = "Resource";
                                tmpArg.DropdownList = Mvm.SelectedSceneNode.Resources;
                                Arguments.Add(tmpArg);
                            }
                            else if (function.func_code.co_varnames[i] == "primitive")
                            {
                                tmpArg.Types = new ObservableCollection<string>() { "Text", "Number", "Boolean" };
                                tmpArg.DropdownList = new ObservableCollection<INode>();
                                Arguments.Add(tmpArg);
                            }
                            else if (function.func_code.co_varnames[i] == "text")
                            {
                                tmpArg.Types = new ObservableCollection<string>() { "Text" };
                                tmpArg.Type = "Text";
                                tmpArg.DropdownList = new ObservableCollection<INode>();
                                Arguments.Add(tmpArg);
                            }
                            else if (function.func_code.co_varnames[i] == "number")
                            {
                                tmpArg.Types = new ObservableCollection<string>() { "Number" };
                                tmpArg.Type = "Number";
                                tmpArg.DropdownList = new ObservableCollection<INode>();
                                Arguments.Add(tmpArg);
                            }
                            else if (function.func_code.co_varnames[i] == "boolean")
                            {
                                tmpArg.Types = new ObservableCollection<string>() { "Boolean" };
                                tmpArg.Type = "Boolean";
                                tmpArg.DropdownList = new ObservableCollection<INode>() { new SceneNode("True"), new SceneNode("False") };
                                Arguments.Add(tmpArg);
                            }
                        }
                    }
                }
            }
        }

        public TextEngineEditor.ViewModel.MainViewModel Mvm { get; set; }
        public ObservableCollection<ArgumentNode> Arguments { get; set; }

        public EffectNode(TextEngineEditor.ViewModel.MainViewModel mvm)
        {
            Arguments = new ObservableCollection<ArgumentNode>();
            Mvm = mvm;
            EffectFunction = "";
        }
    }
}

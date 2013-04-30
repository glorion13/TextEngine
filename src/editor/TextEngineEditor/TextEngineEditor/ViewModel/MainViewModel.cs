using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronPython;
using IronPython.Hosting;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

namespace TextEngineEditor.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"

                // Setup Python runtime environment
                PythonEffects = new BindingList<string>();
                PythonConditions = new BindingList<string>();
                ReloadPython();

                // Initialise commands
                AddSceneCommand = new RelayCommand(AddScene);
                RemoveSceneCommand = new RelayCommand<SceneNode>(RemoveScene);
                AddGActionCommand = new RelayCommand(AddGAction);
                RemoveGActionCommand = new RelayCommand<ActionNode>(RemoveGAction);
                AddGResourceCommand = new RelayCommand(AddGResource);
                RemoveGResourceCommand = new RelayCommand<ResourceNode>(RemoveGResource);
                AddLResourceCommand = new RelayCommand(AddLResource);
                RemoveLResourceCommand = new RelayCommand<ResourceNode>(RemoveLResource);
                AddLActionCommand = new RelayCommand(AddLAction);
                RemoveLActionCommand = new RelayCommand<ActionNode>(RemoveLAction);
                AddConditionCommand = new RelayCommand(AddCondition);
                RemoveConditionCommand = new RelayCommand<ConditionNode>(RemoveCondition);
                AddEffectCommand = new RelayCommand(AddEffect);
                RemoveEffectCommand = new RelayCommand<EffectNode>(RemoveEffect);
                ExportXMLCommand = new RelayCommand(ExportAsXML);
                ImportXMLCommand = new RelayCommand(ImportFromXML);
                ReloadPythonCommand = new RelayCommand(ReloadPython);
                RunInterpreterCommand = new RelayCommand(RunInterpreter);

                // Initialise UI objects
                SceneNodes = new ObservableCollection<INode>();
                GlobalResourceNodes = new ObservableCollection<INode>();
                GlobalActionNodes = new ObservableCollection<INode>();
                ActionStash = new ObservableCollection<INode>();
                PythonResourceTypes = new BindingList<string>()
                {
                    "Text",
                    "Boolean",
                    "Number"
                };
                PythonConditionTypes = new BindingList<string>()
                {
                    "Text",
                    "Boolean",
                    "Number",
                    "Resource",
                    "Scene"
                };

                // Initialise test objects
                SceneNode scene1 = new SceneNode();
                scene1.Name = "cave";
                scene1.Description = "A big cave.";

                SceneNode scene2 = new SceneNode();
                scene2.Name = "beach";
                scene2.Description = "A sandy beach.";
                ActionNode action1 = new ActionNode();
                action1.Name = "Action 1";
                ConditionNode condition1 = new ConditionNode(PythonCore);
                ConditionNode condition2 = new ConditionNode(PythonCore);
                EffectNode effect1 = new EffectNode(this);
                action1.EffectsIfTrue.Add(effect1);
                action1.Conditions.Add(condition1);
                action1.Conditions.Add(condition2);
                scene2.Actions.Add(action1);

                // Populate UI objects
                StartingScene = scene2;
                SceneNodes.Add(scene1);
                SceneNodes.Add(scene2);
            }
        }

        public dynamic PythonCore { get; set; }

        private string gameName;
        public string GameName
        {
            get
            {
                return gameName;
            }
            set
            {
                Set(() => GameName, ref gameName, value);
            }
        }
        private string author;
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                Set(() => Author, ref author, value);
            }
        }

        private SceneNode startingScene;
        public SceneNode StartingScene
        {
            get
            {
                return startingScene;
            }
            set
            {
                Set(() => StartingScene, ref startingScene, value);
            }
        }

        private SceneNode selectedSceneNode;
        public SceneNode SelectedSceneNode
        {
            get
            {
                return selectedSceneNode;
            }
            set
            {
                Set(() => SelectedSceneNode, ref selectedSceneNode, value);
                SelectedActionNode = null;
            }
        }

        private ActionNode selectedActionNode;
        public ActionNode SelectedActionNode
        {
            get
            {
                return selectedActionNode;
            }
            set
            {
                Set(() => SelectedActionNode, ref selectedActionNode, value);
            }
        }

        private ResourceNode selectedResourceNode;
        public ResourceNode SelectedResourceNode
        {
            get
            {
                return selectedResourceNode;
            }
            set
            {
                Set(() => SelectedResourceNode, ref selectedResourceNode, value);
            }
        }

        private ActionNode selectedGActionNode;
        public ActionNode SelectedGActionNode
        {
            get
            {
                return selectedGActionNode;
            }
            set
            {
                Set(() => SelectedGActionNode, ref selectedGActionNode, value);
                SelectedActionNode = SelectedGActionNode;
            }
        }

        private ResourceNode selectedGResourceNode;
        public ResourceNode SelectedGResourceNode
        {
            get
            {
                return selectedGResourceNode;
            }
            set
            {
                Set(() => SelectedGResourceNode, ref selectedGResourceNode, value);
                SelectedResourceNode = SelectedGResourceNode;
            }
        }

        public ObservableCollection<INode> SceneNodes { get; private set; }
        public ObservableCollection<INode> GlobalResourceNodes { get; private set; }
        public ObservableCollection<INode> GlobalActionNodes { get; private set; }

        public ObservableCollection<INode> ActionStash { get; set; }

        public BindingList<string> PythonConditions { get; set; }
        public BindingList<string> PythonEffects { get; set; }
        public BindingList<string> PythonConditionTypes { get; set; }
        public BindingList<string> PythonResourceTypes { get; set; }

        public ICommand RunInterpreterCommand { get; set; }
        private void RunInterpreter()
        {
            System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
            string pythonPath = "C:\\Python27\\python.exe";
            start.FileName = pythonPath;
            start.Arguments = new System.Uri("Lib/Python/interpreter.py", System.UriKind.RelativeOrAbsolute).ToString();
            start.UseShellExecute = true;
            start.RedirectStandardOutput = false;
            System.Diagnostics.Process.Start(start);
        }

        public ICommand AddSceneCommand { get; set; }
        private void AddScene()
        {
            SceneNode tmpSceneNode = new SceneNode();
            int tmpCount = 1;
            while (SceneNodes.Any(s => s.ToString() == tmpSceneNode.Name))
            {
                tmpSceneNode.Name = "New Scene (" + tmpCount.ToString() + ")";
                tmpCount++;
            }
            if (SceneNodes.Count == 0)
            {
                StartingScene = tmpSceneNode;
            }
            SceneNodes.Add(tmpSceneNode);
            SelectedSceneNode = tmpSceneNode;
        }

        public ICommand RemoveSceneCommand { get; set; }
        private void RemoveScene(SceneNode scene)
        {
            SceneNodes.Remove(scene);
        }

        public ICommand AddGActionCommand { get; set; }
        private void AddGAction()
        {
            ActionNode tmpActionNode = new ActionNode();
            int tmpCount = 1;
            while (GlobalActionNodes.Any(a => a.ToString() == tmpActionNode.Name))
            {
                tmpActionNode.Name = "New Action (" + tmpCount.ToString() + ")";
                tmpCount++;
            }
            GlobalActionNodes.Add(tmpActionNode);
        }

        public ICommand RemoveGActionCommand { get; set; }
        private void RemoveGAction(ActionNode action)
        {
            GlobalActionNodes.Remove(action);
        }

        public ICommand AddGResourceCommand { get; set; }
        private void AddGResource()
        {
            ResourceNode tmpResourceNode = new ResourceNode();
            int tmpCount = 1;
            while (GlobalResourceNodes.Any(a => a.ToString() == tmpResourceNode.Name))
            {
                tmpResourceNode.Name = "New Resource (" + tmpCount.ToString() + ")";
                tmpCount++;
            }
            GlobalResourceNodes.Add(tmpResourceNode);
        }

        public ICommand RemoveGResourceCommand { get; set; }
        private void RemoveGResource(ResourceNode resource)
        {
            GlobalResourceNodes.Remove(resource);
        }

        public ICommand AddLActionCommand { get; set; }
        private void AddLAction()
        {
            ActionNode tmpActionNode = new ActionNode();
            int tmpCount = 1;
            while (SelectedSceneNode.Actions.Any(a => a.ToString() == tmpActionNode.Name))
            {
                tmpActionNode.Name = "New Action (" + tmpCount.ToString() + ")";
                tmpCount++;
            }
            SelectedSceneNode.Actions.Add(tmpActionNode);
        }

        public ICommand RemoveLActionCommand { get; set; }
        private void RemoveLAction(ActionNode action)
        {
            SelectedSceneNode.Actions.Remove(action);
        }

        public ICommand AddLResourceCommand { get; set; }
        private void AddLResource()
        {
            ResourceNode tmpResourceNode = new ResourceNode();
            int tmpCount = 1;
            while (SelectedSceneNode.Resources.Any(a => a.ToString() == tmpResourceNode.Name))
            {
                tmpResourceNode.Name = "New Resource (" + tmpCount.ToString() + ")";
                tmpCount++;
            }
            SelectedSceneNode.Resources.Add(tmpResourceNode);
        }

        public ICommand RemoveLResourceCommand { get; set; }
        private void RemoveLResource(ResourceNode resource)
        {
            SelectedSceneNode.Resources.Remove(resource);
        }

        public ICommand AddConditionCommand { get; set; }
        private void AddCondition()
        {
            SelectedActionNode.Conditions.Add(new ConditionNode(PythonCore));
        }

        public ICommand RemoveConditionCommand { get; set; }
        private void RemoveCondition(ConditionNode condition)
        {
            SelectedActionNode.Conditions.Remove(condition);
        }

        public ICommand AddEffectCommand { get; set; }
        private void AddEffect()
        {
            SelectedActionNode.EffectsIfTrue.Add(new EffectNode(this));
        }

        public ICommand RemoveEffectCommand { get; set; }
        private void RemoveEffect(EffectNode effect)
        {
            SelectedActionNode.EffectsIfTrue.Remove(effect);
        }

        public ICommand ExportXMLCommand { get; set; }
        private void ExportAsXML()
        {
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create("game.xml"))
            {
                writer.WriteStartDocument();
                #region <Game>
                writer.WriteStartElement("Game");

                writer.WriteElementString("GameName", GameName);
                writer.WriteElementString("Author", Author);
                writer.WriteElementString("StartingScene", StartingScene.Name);

                #region <GlobalResources>
                writer.WriteStartElement("GlobalResources");
                foreach (ResourceNode resource in GlobalResourceNodes)
                {
                    writer.WriteStartElement("Resource");
                    writer.WriteElementString("Name", resource.Name);
                    writer.WriteElementString("Type", resource.Type);
                    writer.WriteElementString("Value", resource.Value);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion <GlobalResources>

                #region <GlobalActions>
                writer.WriteStartElement("GlobalActions");
                foreach (ActionNode action in GlobalActionNodes)
                {
                    #region <Action>
                    writer.WriteStartElement("Action");
                    writer.WriteElementString("Name", action.Name);
                    writer.WriteElementString("IsVisible", action.IsVisible.ToString());

                    #region <Conditions>
                    writer.WriteStartElement("Conditions");
                    foreach (ConditionNode condition in action.Conditions)
                    {
                        #region <Condition>
                        writer.WriteStartElement("Condition");

                        writer.WriteElementString("ConditionFunction", condition.ConditionType.ToString());

                        writer.WriteStartElement("LeftHandSide");
                        if (condition.LeftHandSideType != "")
                            writer.WriteAttributeString("Type", condition.LeftHandSideType);
                        writer.WriteString(condition.LeftHandSideValue);
                        writer.WriteEndElement();

                        writer.WriteStartElement("RightHandSide");
                        if (condition.RightHandSideType != "")
                            writer.WriteAttributeString("Type", condition.RightHandSideType);
                        writer.WriteString(condition.RightHandSideValue);
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        #endregion <Condition>
                    }
                    writer.WriteEndElement();
                    #endregion <Conditions>

                    #region <Effects>
                    writer.WriteStartElement("EffectsIfTrue");
                    foreach (EffectNode effect in action.EffectsIfTrue)
                    {
                        writer.WriteStartElement("Effect");
                        writer.WriteElementString("EffectFunction", effect.EffectFunction);
                        writer.WriteStartElement("args");
                        foreach (ArgumentNode arg in effect.Arguments)
                        {
                            writer.WriteStartElement("arg");
                            if (arg.Type != "")
                                writer.WriteAttributeString("Type", arg.Type);
                            writer.WriteString(arg.Selection.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("EffectsIfFalse");
                    foreach (EffectNode effect in action.EffectsIfFalse)
                    {
                        writer.WriteStartElement("Effect");
                        writer.WriteElementString("EffectFunction", effect.EffectFunction);
                        writer.WriteStartElement("args");
                        foreach (ArgumentNode arg in effect.Arguments)
                        {
                            writer.WriteStartElement("arg");
                            if (arg.Type != "")
                                writer.WriteAttributeString("Type", arg.Type);
                            writer.WriteString(arg.Selection.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    #endregion <Effects>

                    writer.WriteEndElement();
                    #endregion <Action>
                }
                writer.WriteEndElement();
                #endregion <GlobalActions>

                #region <Scenes>
                writer.WriteStartElement("Scenes");
                foreach (SceneNode scene in SceneNodes)
                {
                    #region <Scene>
                    writer.WriteStartElement("Scene");
                    writer.WriteElementString("ID", scene.ID.ToString());
                    writer.WriteElementString("Name", scene.Name);
                    writer.WriteElementString("Description", scene.Description);
                    #region <Resources>
                    writer.WriteStartElement("Resources");
                    foreach (ResourceNode resource in scene.Resources)
                    {
                        writer.WriteStartElement("Resource");
                        writer.WriteElementString("Name", resource.Name);
                        writer.WriteElementString("Type", resource.Type);
                        writer.WriteElementString("Value", resource.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    #endregion <Resources>
                    #region <Actions>
                    writer.WriteStartElement("Actions");
                    foreach (ActionNode action in scene.Actions)
                    {
                        #region <Action>
                        writer.WriteStartElement("Action");
                        writer.WriteElementString("Name", action.Name);
                        writer.WriteElementString("IsVisible", action.IsVisible.ToString());

                        #region <Conditions>
                        writer.WriteStartElement("Conditions");
                        foreach (ConditionNode condition in action.Conditions)
                        {
                            #region <Condition>
                            writer.WriteStartElement("Condition");

                            writer.WriteElementString("ConditionFunction", condition.ConditionType.ToString());

                            writer.WriteStartElement("LeftHandSide");
                            if (condition.LeftHandSideType != "")
                                writer.WriteAttributeString("Type", condition.LeftHandSideType);
                            writer.WriteString(condition.LeftHandSideValue);
                            writer.WriteEndElement();

                            writer.WriteStartElement("RightHandSide");
                            if (condition.RightHandSideType != "")
                                writer.WriteAttributeString("Type", condition.RightHandSideType);
                            writer.WriteString(condition.RightHandSideValue);
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                            #endregion <Condition>
                        }
                        writer.WriteEndElement();
                        #endregion <Conditions>

                        #region <Effects>
                        writer.WriteStartElement("EffectsIfTrue");
                        foreach (EffectNode effect in action.EffectsIfTrue)
                        {
                            writer.WriteStartElement("Effect");
                            writer.WriteElementString("EffectFunction", effect.EffectFunction);
                            writer.WriteStartElement("args");
                            foreach (ArgumentNode arg in effect.Arguments)
                            {
                                writer.WriteStartElement("arg");
                                writer.WriteAttributeString("Type", arg.Type);
                                writer.WriteString(arg.Selection.ToString());
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("EffectsIfFalse");
                        foreach (EffectNode effect in action.EffectsIfFalse)
                        {
                            writer.WriteStartElement("Effect");
                            writer.WriteElementString("EffectFunction", effect.EffectFunction);
                            writer.WriteStartElement("args");
                            foreach (ArgumentNode arg in effect.Arguments)
                            {
                                writer.WriteStartElement("arg");
                                writer.WriteAttributeString("Type", arg.Type);
                                writer.WriteString(arg.Selection.ToString());
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        #endregion <Effects>

                        writer.WriteEndElement();
                        #endregion <Action>
                    }
                    writer.WriteEndElement();
                    #endregion <Actions>
                    writer.WriteEndElement();
                    #endregion <Scene>
                }
                writer.WriteEndElement();
                #endregion <Scenes>

                writer.WriteEndElement();
                #endregion <Game>
                writer.WriteEndDocument();
            }
        }

        public ICommand ImportXMLCommand { get; set; }
        private void ImportFromXML()
        {
            string inputURL = "game.xml";
            dynamic pythonParser = Python.CreateRuntime().ImportModule("Python/dataParser");
            dynamic gameParser = pythonParser.GameParser();
            dynamic game = gameParser.loadXMLGameData(inputURL);
            System.Windows.MessageBox.Show(game.name.ToString());
        }

        public ICommand ReloadPythonCommand { get; set; }
        private void ReloadPython()
        {
            PythonCore = Python.CreateRuntime().ImportModule("Python/core");

            PythonEffects.Clear();
            PythonConditions.Clear();

            int effectCount = PythonCore.components.customisable.getEffectCount();
            dynamic effectKeys = PythonCore.components.customisable.getEffectKeys();
            for (int i = 0; i < effectCount; i++)
            {
                PythonEffects.Add(effectKeys[i]);
            }
            int conditionCount = PythonCore.components.customisable.getConditionCount();
            dynamic conditionKeys = PythonCore.components.customisable.getConditionKeys();
            for (int i = 0; i < conditionCount; i++)
            {
                PythonConditions.Add(conditionKeys[i]);
            }
            // TODO:
            // Ensure that the pre-existing scenes don't lose their effects and conditions (if reloading python during runtime)
            // Print out log of warnings in case there some were deleted
            // If Python does not work don't load it - somehow evaluate python scripts for errors
        }
    }
}
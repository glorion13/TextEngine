using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronPython;
using IronPython.Hosting;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Scripting.Hosting;
using System.Windows;

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
                AddTrueEffectCommand = new RelayCommand(AddTrueEffect);
                RemoveTrueEffectCommand = new RelayCommand<EffectNode>(RemoveTrueEffect);
                AddFalseEffectCommand = new RelayCommand(AddFalseEffect);
                RemoveFalseEffectCommand = new RelayCommand<EffectNode>(RemoveFalseEffect);
                ExportXmlCommand = new RelayCommand(ExportAsXml);
                ImportXmlCommand = new RelayCommand(ImportFromXml);
                ReloadPythonCommand = new RelayCommand(ReloadPython);
                RunInterpreterCommand = new RelayCommand(RunInterpreter);
                ChangeToSceneLayoutCommand = new RelayCommand(ChangeToSceneLayout);
                ChangeToActionLayoutCommand = new RelayCommand(ChangeToActionLayout);
                ChangeToResourceLayoutCommand = new RelayCommand(ChangeToResourceLayout);
                ChangeToSceneLayoutWithActionsCommand = new RelayCommand(ChangeToSceneLayoutWithActions);
                ChangeToSceneLayoutWithResourcesCommand = new RelayCommand(ChangeToSceneLayoutWithResources);

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

                SceneLayoutVisibility = "Collapsed";
                ActionLayoutVisibility = "Collapsed";
                ResourceLayoutVisibility = "Collapsed";
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

        private string sceneLayoutVisibility;
        public string SceneLayoutVisibility
        {
            get
            {
                return sceneLayoutVisibility;
            }
            set
            {
                Set(() => SceneLayoutVisibility, ref sceneLayoutVisibility, value);
            }
        }
        private string actionLayoutVisibility;
        public string ActionLayoutVisibility
        {
            get
            {
                return actionLayoutVisibility;
            }
            set
            {
                Set(() => ActionLayoutVisibility, ref actionLayoutVisibility, value);
            }
        }
        private string resourceLayoutVisibility;
        public string ResourceLayoutVisibility
        {
            get
            {
                return resourceLayoutVisibility;
            }
            set
            {
                Set(() => ResourceLayoutVisibility, ref resourceLayoutVisibility, value);
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
            bool success = doXmlExport();
            if (success)
            {
                System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
                string pythonPath = "C:\\Python27\\python.exe";
                start.FileName = pythonPath;
                start.Arguments = new System.Uri("Lib/Python/interpreter.py", System.UriKind.RelativeOrAbsolute).ToString();
                start.UseShellExecute = true;
                start.RedirectStandardOutput = false;
                System.Diagnostics.Process.Start(start);
            }
        }

        public ICommand ChangeToSceneLayoutCommand { get; set; }
        private void ChangeToSceneLayout()
        {
            SceneLayoutVisibility = "Visible";
            ActionLayoutVisibility = "Collapsed";
            ResourceLayoutVisibility = "Collapsed";
        }

        public ICommand ChangeToSceneLayoutWithActionsCommand { get; set; }
        private void ChangeToSceneLayoutWithActions()
        {
            SceneLayoutVisibility = "Visible";
            ActionLayoutVisibility = "Visible";
            ResourceLayoutVisibility = "Collapsed";
        }

        public ICommand ChangeToSceneLayoutWithResourcesCommand { get; set; }
        private void ChangeToSceneLayoutWithResources()
        {
            SceneLayoutVisibility = "Visible";
            ActionLayoutVisibility = "Collapsed";
            ResourceLayoutVisibility = "Visible";
        }

        public ICommand ChangeToActionLayoutCommand { get; set; }
        private void ChangeToActionLayout()
        {
            SceneLayoutVisibility = "Collapsed";
            ActionLayoutVisibility = "Visible";
            ResourceLayoutVisibility = "Collapsed";
        }

        public ICommand ChangeToResourceLayoutCommand { get; set; }
        private void ChangeToResourceLayout()
        {
            SceneLayoutVisibility = "Collapsed";
            ActionLayoutVisibility = "Collapsed";
            ResourceLayoutVisibility = "Visible";
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

        public ICommand AddTrueEffectCommand { get; set; }
        private void AddTrueEffect()
        {
            SelectedActionNode.EffectsIfTrue.Add(new EffectNode(this));
        }

        public ICommand RemoveTrueEffectCommand { get; set; }
        private void RemoveTrueEffect(EffectNode effect)
        {
            SelectedActionNode.EffectsIfTrue.Remove(effect);
        }

        public ICommand AddFalseEffectCommand { get; set; }
        private void AddFalseEffect()
        {
            SelectedActionNode.EffectsIfFalse.Add(new EffectNode(this));
        }

        public ICommand RemoveFalseEffectCommand { get; set; }
        private void RemoveFalseEffect(EffectNode effect)
        {
            SelectedActionNode.EffectsIfFalse.Remove(effect);
        }

        public ICommand ExportXmlCommand { get; set; }
        private void ExportAsXml()
        {
            bool success = doXmlExport();
        }

        private bool doXmlExport()
        {
            if (SceneNodes.Count > 0)
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
                        writer.WriteElementString("Visible", action.Visible.ToString().Substring(0, 1).ToUpper() + action.Visible.ToString().Substring(1));
                        writer.WriteElementString("Enabled", action.Enabled.ToString().Substring(0, 1).ToUpper() + action.Enabled.ToString().Substring(1));
                        writer.WriteElementString("Active", action.Active.ToString().Substring(0, 1).ToUpper() + action.Active.ToString().Substring(1));

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
                            writer.WriteElementString("Visible", action.Visible.ToString().Substring(0, 1).ToUpper() + action.Visible.ToString().Substring(1));
                            writer.WriteElementString("Enabled", action.Enabled.ToString().Substring(0, 1).ToUpper() + action.Enabled.ToString().Substring(1));
                            writer.WriteElementString("Active", action.Active.ToString().Substring(0, 1).ToUpper() + action.Active.ToString().Substring(1));

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
                return true;
            }
            else
            {
                MessageBox.Show("This game has no scenes!");
                return false;
            }
        }

        public ICommand ImportXmlCommand { get; set; }
        private void ImportFromXml()
        {
            PythonCore = Python.CreateRuntime().ImportModule("Python/dataParser");
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
            // Print out log of warnings in case there are some that were deleted
            // If Python does not work don't load it - somehow evaluate python scripts for errors
            // General debugging and error handling
        }
    }
}
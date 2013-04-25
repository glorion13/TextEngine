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
                //var kek = pythonCore.getEffects().get("outputVisibleSceneActions").__name__;

                // Initialise commands
                AddSceneCommand = new RelayCommand(AddScene);
                RemoveSceneCommand = new RelayCommand(RemoveScene);
                AddActionCommand = new RelayCommand(AddAction);
                RemoveActionCommand = new RelayCommand<ActionNode>(RemoveAction);
                AddConditionCommand = new RelayCommand(AddCondition);
                RemoveConditionCommand = new RelayCommand<ConditionNode>(RemoveCondition);
                AddEffectCommand = new RelayCommand(AddEffect);
                RemoveEffectCommand = new RelayCommand<EffectNode>(RemoveEffect);
                ExportXMLCommand = new RelayCommand(ExportAsXML);
                ImportXMLCommand = new RelayCommand(ImportFromXML);
                ReloadPythonCommand = new RelayCommand(ReloadPython);

                // Initialise UI objects
                SceneNodes = new ObservableCollection<SceneNode>();
                GlobalResourceNodes = new ObservableCollection<ResourceNode>();
                GlobalActionNodes = new ObservableCollection<ActionNode>();
                ActionStash = new ObservableCollection<ActionNode>();
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
                ConditionNode condition1 = new ConditionNode();
                ConditionNode condition2 = new ConditionNode();
                EffectNode effect1 = new EffectNode();
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

        dynamic pythonCore { get; set; }
        dynamic effectDict { get; set; }
        dynamic conditionDict { get; set; }

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

        public ObservableCollection<SceneNode> SceneNodes { get; private set; }
        public ObservableCollection<ResourceNode> GlobalResourceNodes { get; private set; }
        public ObservableCollection<ActionNode> GlobalActionNodes { get; private set; }

        public ObservableCollection<ActionNode> ActionStash { get; set; }

        public BindingList<string> PythonConditions { get; set; }
        public BindingList<string> PythonEffects { get; set; }
        public BindingList<string> PythonConditionTypes { get; set; }

        public ICommand AddSceneCommand { get; set; }
        private void AddScene()
        {
            SceneNode tmpSceneNode = new SceneNode();
            int tmpCount = 1;
            while (SceneNodes.Any(s => s.Name == tmpSceneNode.Name))
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
        private void RemoveScene()
        {
            SceneNodes.Remove(SelectedSceneNode);
        }

        public ICommand AddActionCommand { get; set; }
        private void AddAction()
        {
            ActionNode tmpActionNode = new ActionNode();
            int tmpCount = 1;
            while (SelectedSceneNode.Actions.Any(a => a.Name == tmpActionNode.Name))
            {
                tmpActionNode.Name = "New Action (" + tmpCount.ToString() + ")";
                tmpCount++;
            }
            SelectedSceneNode.Actions.Add(tmpActionNode);
        }

        public ICommand RemoveActionCommand { get; set; }
        private void RemoveAction(ActionNode action)
        {
            SelectedSceneNode.Actions.Remove(action);
        }

        public ICommand AddConditionCommand { get; set; }
        private void AddCondition()
        {
            SelectedActionNode.Conditions.Add(new ConditionNode());
        }

        public ICommand RemoveConditionCommand { get; set; }
        private void RemoveCondition(ConditionNode condition)
        {
            SelectedActionNode.Conditions.Remove(condition);
        }

        public ICommand AddEffectCommand { get; set; }
        private void AddEffect()
        {
            SelectedActionNode.EffectsIfTrue.Add(new EffectNode());
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
                writer.WriteElementString("StartingScene", StartingScene.ID.ToString());

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
                        writer.WriteAttributeString("Type", condition.LeftHandSideType);
                        writer.WriteString(condition.LeftHandSideValue);
                        writer.WriteEndElement();

                        writer.WriteStartElement("RightHandSide");
                        writer.WriteString(condition.RightHandSideValue);
                        writer.WriteAttributeString("Type", condition.RightHandSideType);
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
                        foreach (KeyValuePair<string, string> arg in effect.arguments)
                        {
                            writer.WriteStartElement("arg");
                            writer.WriteAttributeString("Type", arg.Key);
                            writer.WriteString(arg.Value);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("EffectsIfFalse");
                    foreach (EffectNode effect in action.EffectsIfFalse)
                    {
                        writer.WriteStartElement("Effect");
                        writer.WriteElementString("EffectFunction", effect.EffectFunction);
                        foreach (KeyValuePair<string, string> arg in effect.arguments)
                        {
                            writer.WriteStartElement("arg");
                            writer.WriteAttributeString("Type", arg.Key);
                            writer.WriteString(arg.Value);
                            writer.WriteEndElement();
                        }
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
                            writer.WriteAttributeString("Type", condition.LeftHandSideType);
                            writer.WriteString(condition.LeftHandSideValue);
                            writer.WriteEndElement();

                            writer.WriteStartElement("RightHandSide");
                            writer.WriteString(condition.RightHandSideValue);
                            writer.WriteAttributeString("Type", condition.RightHandSideType);
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
                            foreach (KeyValuePair<string, string> arg in effect.arguments)
                            {
                                writer.WriteStartElement("arg");
                                writer.WriteAttributeString("Type", arg.Key);
                                writer.WriteString(arg.Value);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("EffectsIfFalse");
                        foreach (EffectNode effect in action.EffectsIfFalse)
                        {
                            writer.WriteStartElement("Effect");
                            writer.WriteElementString("EffectFunction", effect.EffectFunction);
                            foreach (KeyValuePair<string, string> arg in effect.arguments)
                            {
                                writer.WriteStartElement("arg");
                                writer.WriteAttributeString("Type", arg.Key);
                                writer.WriteString(arg.Value);
                                writer.WriteEndElement();
                            }
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
        }

        public ICommand ReloadPythonCommand { get; set; }
        private void ReloadPython()
        {
            pythonCore = Python.CreateRuntime().ImportModule("Python/core");

            PythonEffects.Clear();
            PythonConditions.Clear();

            int effectCount = pythonCore.components.customisable.getEffectCount();
            dynamic effectKeys = pythonCore.components.customisable.getEffectKeys();
            for (int i = 0; i < effectCount; i++)
            {
                PythonEffects.Add(effectKeys[i]);
            }
            int conditionCount = pythonCore.components.customisable.getConditionCount();
            dynamic conditionKeys = pythonCore.components.customisable.getConditionKeys();
            for (int i = 0; i < conditionCount; i++)
            {
                PythonConditions.Add(conditionKeys[i]);
            }

            effectDict = pythonCore.components.customisable.effects.EffectFunctions().effectDict;
            conditionDict = pythonCore.components.customisable.conditions.ConditionFunctions().conditionDict;

            // TODO:
            // Ensure that the pre-existing scenes don't lose their effects and conditions
            // Print out log of warnings in case there some were deleted
            // If Python does not work don't load it - somehow evaluate python scripts for errors
        }
    }
}
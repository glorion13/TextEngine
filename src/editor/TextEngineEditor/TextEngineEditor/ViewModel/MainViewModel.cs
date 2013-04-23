using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronPython;
using IronPython.Hosting;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;

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
                ExportCommand = new RelayCommand(ExportAsXML);
                ReloadPythonCommand = new RelayCommand(ReloadPython);

                // Initialise UI objects
                SceneNodes = new ObservableCollection<SceneNode>();

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
                action1.Conditions.Add(condition1);
                scene2.Actions.Add(action1);

                // Populate UI objects
                SceneNodes.Add(scene1);
                SceneNodes.Add(scene2);
            }
        }

        dynamic pythonCore { get; set; }
        dynamic pythonCustom { get; set; }

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
        public ObservableCollection<ResourceNode> GlobalActionNodes { get; private set; }

        public BindingList<string> PythonConditions { get; set; }
        public BindingList<string> PythonEffects { get; set; }

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
        }

        public ICommand ExportCommand { get; set; }
        private void ExportAsXML()
        {
        }

        public ICommand ReloadPythonCommand { get; set; }
        private void ReloadPython()
        {
            pythonCore = Python.CreateRuntime().ImportModule("Python/core");
            pythonCustom = Python.CreateRuntime().ImportModule("Python/customisable");

            PythonEffects.Clear();
            PythonConditions.Clear();

            int effectCount = pythonCustom.getEffectsKeysLength();
            dynamic effectKeys = pythonCustom.getEffectsKeys();
            for (int i = 0; i < effectCount; i++)
            {
                PythonEffects.Add(effectKeys[i]);
            }

            int conditionCount = pythonCustom.getConditionsKeysLength();
            dynamic conditionKeys = pythonCustom.getConditionsKeys();
            for (int i = 0; i < conditionCount; i++)
            {
                PythonConditions.Add(conditionKeys[i]);
            }

            // TODO:
            // Ensure that the pre-existing scenes don't lose their effects and conditions
            // Print out log of warnings in case there some were deleted
            // If Python does not work don't load it - somehow evaluate python scripts for errors
        }
    }
}
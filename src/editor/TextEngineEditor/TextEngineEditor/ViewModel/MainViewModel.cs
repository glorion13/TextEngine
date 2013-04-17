using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronPython;
using IronPython.Hosting;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

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
                ChangeHPCommand = new RelayCommand(ChangeHP);
                
                // Setup Python runtime environment
                dynamic python = Python.CreateRuntime();
                dynamic core = Python.CreateRuntime().ImportModule("Python/core");

                HpRes = core.getResource("HP", 50);

                SceneNodes = new BindingList<dynamic>();
                SceneNodes.Add(HpRes);
            }
        }

        public BindingList<dynamic> SceneNodes { get; private set; }

        dynamic HpRes { get; set; }

        private void ChangeHP()
        {
            HpRes.value += 1;
            SceneNodes[0] = HpRes;
        }

        //private dynamic selectedItem;
        //public dynamic SelectedItem
        //{
        //    get
        //    {
        //        return selectedItem;
        //    }
        //    set
        //    {
        //        Set(() => SelectedItem, ref selectedItem, value);
        //    }
        //}

        public ICommand ChangeHPCommand { get; set; }
    }
}
using GalaSoft.MvvmLight;

using IronPython;
using IronPython.Hosting;

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
                TestText = "test";
                dynamic python = Python.CreateRuntime();
                dynamic core = Python.CreateRuntime().ImportModule("Python/core");
                dynamic hpRes = core.getResource("HP", 50);
                int hp = hpRes.value;
                TestText = hp.ToString();
            }
        }

        private string testText;
        public string TestText
        {
            get
            {
                return testText;
            }
            set
            {
                Set(() => TestText, ref testText, value);
            }
        }
    }
}
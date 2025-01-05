using System.Windows.Media;
using System.Windows;

namespace Luna_X
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
            if (!System.IO.Directory.Exists(".\\bin"))
            {
                MessageBox.Show("Files Missing - Bin Resources Not Found.");
            }
            if (System.IO.Directory.Exists(".\\add_scripts_here"))
            {
                System.IO.Directory.CreateDirectory(".\\add_scripts_here");
            }
        } 
    }
}

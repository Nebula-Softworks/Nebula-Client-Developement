using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Luna_X.Controls.Misc
{
    /// <summary>
    /// Interaction logic for OptionPopup.xaml
    /// </summary>
    public partial class OptionPopup : UserControl
    {
        public OptionPopup(string Header, string Content, string functionname)
        {
            InitializeComponent();
            header.Text = Header;
            content.Text = Content;
            Main.Content = functionname;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Parent is Panel parentPanel)
            {
                if (Application.Current.MainWindow is MainWindow m)
                {
                    m.Blurrer.Radius = 0;
                    m.TabSystemz.Visibility = Visibility.Visible;
                }
                parentPanel.Children.Remove(this);
            }
        }


    }
}

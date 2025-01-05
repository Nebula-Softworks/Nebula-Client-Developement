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

namespace Luna_X.Controls.Script_Hub
{
    /// <summary>
    /// Interaction logic for Script_Hub_Square.xaml
    /// </summary>
    public partial class Favourite_Script_Square : UserControl
    {
        public Favourite_Script_Square(bool IsPatched, bool IsKeyed)
        {
            InitializeComponent();
            if (IsPatched)
                this.PatchedTag.Visibility = Visibility.Visible;
            if (!IsKeyed)
                return;
            this.KeySystemTag.Visibility = Visibility.Visible;
        }
    }
}

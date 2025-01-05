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
using Luna_X.Core;

namespace Luna_X.Controls.Misc
{
    /// <summary>
    /// Interaction logic for Tab_System.xaml
    /// </summary>
    public partial class Tab_System : UserControl
    {
       
        public Tab_System()
        {
            InitializeComponent();
            maintabs.Items.Add(CreateTab("--[[Welcome to Nebula Client\n\n		Here Is Luna X, The IDE Made For Our Users.\n		Powered by the Nebula Trinity Engine, The Entirety of\n		Nebula Client excels in Injection And execution.\n		Our Staff Team is also always ready to help you.\n\nEnjoy Using Nebula Client!\nhttps://dsc.gg/nebulasoftworks]]", "Untitled Script.lua"));
        }

        /// <summary>
        /// Returns the current Monaco Editor instance
        /// </summary>
        public monaco_api current_monaco()
        {
            return maintabs.SelectedContent as monaco_api;
        }

        /// <summary>
        /// Creates a new tab in the TabSystem control with a separated Monaco Editor instance
        /// </summary>
        public void add_tab_with_text(string text, string title = null)
        {
            maintabs.Items.Add(CreateTab(text, title));
        }

        /// <summary>
        /// Changes the current open Monaco Editor tab title
        /// </summary>
        public void ChangeCurrentTabTitle(string title)
        {
            if (maintabs.SelectedItem is TabItem selectedTab)
            {
                ((TextBox)selectedTab.Header).Text = title;
            }
        }

        public Task<string> GetCurrentTabTitle()
        {
            if (maintabs.SelectedItem is TabItem selectedTab)
            {
                return Task.FromResult(((TextBox)selectedTab.Header).Text);
            }

            return Task.FromResult(string.Empty);
        }

        public void ButtonTabs(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "AddTabButton":
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        maintabs.Items.Add(CreateTab(mainWindow.DefaultText));
                        if (mainWindow.Minimap) current_monaco().enable_minimap();
                    }
                    break;

                case "CloseButton":
                    try
                    {
                        maintabs.Items.Remove(maintabs.SelectedItem);
                    }
                    catch { }
                    break;
            }
        }

        public monaco_api CreateEditor(string Start) => new monaco_api(Start);

        public TabItem CreateTab(string content, string Title = null)
        {
            var m = (MainWindow)Application.Current.MainWindow;
            if (Title == null) Title = m.DefaultTitle;

            TextBox textBox = new TextBox();
            textBox.Text = Title;
            textBox.IsHitTestVisible = false;
            textBox.IsEnabled = false;
            textBox.TextWrapping = TextWrapping.NoWrap;
            textBox.Style = TryFindResource("InvisibleTextBox") as Style;
            var tab = new TabItem
            {
                Header = textBox,
                Style = TryFindResource("Tab") as Style,
                Foreground = Brushes.White,
                FontSize = 12,
                Content = CreateEditor(content),
                IsSelected = true,
                AllowDrop = true,
            };
            tab.MouseDown += (sender, e) =>
            {
                if (!(e.OriginalSource is Border))
                    return;

                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    ButtonTabs(new Button() { Name = "CloseButton" }, new RoutedEventArgs());
                }

                else
                {
                    if (e.RightButton != MouseButtonState.Pressed)
                        return;

                    textBox.IsEnabled = true;
                    if (textBox.Text.LastIndexOf(".") > 0) // Ensure there's a valid extension
                    {
                        textBox.Focus();
                        textBox.SelectionStart = 0;
                        textBox.SelectionLength = textBox.Text.LastIndexOf(".");// Select up to the extension
                    }
                    else
                    {
                        textBox.Focus();
                        textBox.SelectionStart = 0;
                        textBox.SelectAll();
                    }
                }
            };
            string oldHeader = Title;
            textBox.GotFocus += (sender, e) =>
            {
                oldHeader = textBox.Text;
                textBox.CaretIndex = textBox.Text.Length - 1;
            };
            textBox.KeyDown += (s, e) =>
            {
                if (textBox.Text == "")
                {
                    textBox.Text = oldHeader;
                    textBox.IsEnabled = false;
                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.Return:
                            textBox.IsEnabled = false;
                            break;
                        case Key.Escape:
                            textBox.Text = oldHeader;
                            goto case Key.Return;
                    }
                }
            };
            textBox.LostFocus += (sender, e) => textBox.IsEnabled = false;
            return tab;
        }

        /// <summary>
        /// Clears the Monaco Editor content and resets the title to its index.
        /// </summary>
        private void Clear_Editor(object sender, RoutedEventArgs e)
        {
            var x = maintabs.SelectedContent as monaco_api;
            try
            {
                x.SetText("");
                this.ChangeCurrentTabTitle($"Script {maintabs.Items.Count}");
            }
            catch { }
        }
    }
}

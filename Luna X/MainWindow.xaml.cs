using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Luna_X;
using Luna_X.Core;
using Luna_X.Controls.Misc;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Luna_X.Controls.Script_Hub;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace Luna_X
{
    public class jsonstrings
    {
        public class ScriptHub
        {
            public string Image { get; set; }

            public string Name { get; set; }

            public string GameName { get; set; }

            public string Script { get; set; }
        }

        public List<ScriptHub> Script;
    }

    public abstract class FileSystemItem : INotifyPropertyChanged
    {
        public string Header { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    // Folder item
    public class FolderItem : FileSystemItem
    {
        public ObservableCollection<FileSystemItem> Children { get; set; }
    }

    // File types
    public class LuaFileItem : FileSystemItem { }
    public class TextFileItem : FileSystemItem { }
    public class OtherFileItem : FileSystemItem { }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        ///  this allows for other things to access the settings variables
        ///  i should probably make them in a new class but ya
        ///  honeslty actually its just so anything can access the variables here
        ///  which in hindisght i hsouldve put in a class in /core but whatever
        /// </summary>
        /// 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Variables And Functions

        /// <summary>
        /// Settings Variables
        /// </summary>

        // int = index of table | dropdown
        // string = textbox
        // bool = toggle or checkbox
        // double = slider | number

        public bool HorizontalClock = false;
        public bool AutoAttach = false;
        public bool Minimap = true;
        public bool Stylua = true;
        public string DefaultText = "print('Let's Get This party Started!')";
        public string DefaultTitle = "Untitled Script.lua";
        public double fontSize = 14;
        public bool ligarutres = false;
        public bool Intellisense = true;
        public bool autocomplete = true;
        public bool SaveTabs = false;
        public bool antiSkid = false;
        public bool LegacyAlignment = false;
        public bool ListBoxVisibility = true;
        public bool ListBoxMenu = true;
        public double Transparency = 0;
        public bool topMost = true;
        public bool AutoFade = false;
        public string BackgroundPath = "";

        [Obsolete ("Acrylic Design discontinued in Dev Build 0.4")]
        public bool Acrylic = true;

        public string DisplayName = "Default";
        public string[] Interogations = new string[]
        {
            null, // scriptblox
            null, // pastebin
            null, // github
            null, //discord
            null, //rscripts
        };

        public int currentTheme = 1; // there won't be official themes, but plugins can create them || 1 = basic theme
        public int editorTheme = 2; // check monaco files for theme list
        public int language = 1;

        public double walkSpeed = 16;
        public double jumpPower = 50;
        public double gravity = 1;
        public float fpsUnlocker = 999;
        public double Scale = 1;

        /// <summary>
        /// other variables
        /// </summary>

        /*

        LinearGradientBrush Accent = new LinearGradientBrush
        {
            StartPoint = new System.Windows.Point(0, 0),
            EndPoint = new System.Windows.Point(1, 1),
            GradientStops = new GradientStopCollection(new GradientStop[] {
            new GradientStop((Color)ColorConverter.ConvertFromString("#FFF5B7FF"), 0),
            new GradientStop((Color)ColorConverter.ConvertFromString("#FFC690FF"), 1)
        })
        };

        LinearGradientBrush AccentTrans = new LinearGradientBrush
        {
            StartPoint = new System.Windows.Point(0, 0),
            EndPoint = new System.Windows.Point(1, 1),
            GradientStops = new GradientStopCollection(new GradientStop[] {
            new GradientStop((Color)ColorConverter.ConvertFromString("#7FF5B7FF"), 0),
            new GradientStop((Color)ColorConverter.ConvertFromString("#7FC690FF"), 1)
        })
        };*/

        static string Website = "https://nebulasoftworks.framer.ai";
        static string Distrubutor = $"{Website}"; 
        static string GithubRepo = "https://github.com/Nebula-Softworks/Nebula-Client";
        public DiscordRPC.DiscordRpcClient client;

        DispatcherTimer RGBTime;
        int RGBSpinSpeed = 4;

        public ObservableCollection<FileSystemItem> RootItems { get; set; }
        public FileSystemWatcher fileWatcher;

        DispatcherTimer clock;

        //HttpClient httpClient;

        //public string Game_SearchText = " ";

        //public bool Game_SearchText_Changed = true;

        //public double CurrentPage = 1;

        //public double MaxPage = 500;

        public ScriptHub cloud;

        /// <summary>
        /// Functions
        /// </summary>

        [Obsolete ("This removes emojis. As of Dev Build 8, Emojis are now supported hence this is useless")]
        public string CleanString(string CleanableString)
        {
            //if (CleanableString != null)
            //    return Regex.Replace(CleanableString, "[^\\u0000-\\u007F]+", "");
            //else
            //    return "";
            return CleanableString;
        }

        [DllImport("user32.dll")] // this is so it can be dragged from aero snap maximise, from some stack overflow post https://stackoverflow.com/questions/7417739/make-wpf-window-draggable-no-matter-what-element-is-clicked
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public void print(params string[] STRING_PRINT)
        {
            if (STRING_PRINT.Length == 0) return;
            for (int i = 1; i < STRING_PRINT.Length; i++)
            {
                Console.WriteLine(i);
            }
        }

        public string HttpGet(string weburl) // shittiest and most confusing function known to man || stolen from showerhead; let me daddy
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                return webClient.DownloadString(weburl);
            }
        }

        public void Redirect(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception e)
            {
                OptionPopup o = new OptionPopup("Can't Redirect You", $"Couldn't redirect you to {url}\nWould You Like us to copy the link to your clipboard?\n Error: {e}", "OK");
                o.Main.Click += delegate
                {
                    Popups.Children.Remove(o);
                    Blurrer.Radius = 0;
                    var cm = TabSystemz;
                    cm.Visibility = Visibility.Visible;
                    Clipboard.SetText(url); // source: https://stackoverflow.com/questions/899350/how-do-i-copy-the-contents-of-a-string-to-the-clipboard-in-c
                };
                o.Secondary.Click += delegate
                {
                    Popups.Children.Remove(o);
                    Blurrer.Radius = 0;
                    var cm = TabSystemz;
                    cm.Visibility = Visibility.Visible;
                };
                Popups.Children.Add(o);
                Blurrer.Radius = 30;
                var cm = TabSystemz;
                cm.Visibility = Visibility.Collapsed;
            }
        }

        [Obsolete ("Doesnt work, if you can fix it create a pull request ig")]
        public void ApplyScale(UIElement element, double scaleFactor)
        {
            // Apply ScaleTransform to the container
            var transform = new ScaleTransform(scaleFactor, scaleFactor);
            element.RenderTransform = transform;
            element.RenderTransformOrigin = new Point(0.5, 0.5);


                this.Width *= scaleFactor;
                this.Height *= scaleFactor;
        }

        /// <summary>
        /// Workspace functions
        /// a treeview is used so we can show folders as well
        /// 
        /// KNOWN ERRORS:
        /// if there is a folder with a file in it, the view still wont show without a file in the main directory.
        /// </summary>

        // loads all files and folders in the scripts folder to the tree view
        public void LoadFileTree(string directoryPath, ObservableCollection<FileSystemItem> parentCollection)
        {
            if (Directory.GetFiles(@".\add_scripts_here").Length != 0)
            {
                WorkspaceBorder.Visibility = Visibility.Visible;
                TabSystemz.Margin = new Thickness(200, 5, 5, 45);
            }
            else
            {
                WorkspaceBorder.Visibility = Visibility.Collapsed;
                TabSystemz.Margin = new Thickness(5, 5, 5, 45);
            }

            parentCollection.Clear();

            // Add folders
            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                var folder = new FolderItem
                {
                    Header = Path.GetFileName(dir),
                    Children = new ObservableCollection<FileSystemItem>()
                };
                LoadFileTree(dir, folder.Children);

                // Only add folder if it has children
                if (folder.Children.Any())
                {
                    parentCollection.Add(folder);
                }
            }

            // Add files
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                var fileName = Path.GetFileName(file);
                var fileItem = CreateFileItem(file, fileName);

                if (fileItem != null)
                {
                    parentCollection.Add(fileItem);
                }
            }
        }

        // Factory method to create appropriate file item type
        public FileSystemItem CreateFileItem(string filePath, string fileName)
        {
            string extension = Path.GetExtension(filePath)?.ToLower();
            return extension switch
            {
                ".lua" => new LuaFileItem { Header = fileName },
                ".txt" => new TextFileItem { Header = fileName },
                _ => new OtherFileItem { Header = fileName }
            };
        }

        // Initialize the FileSystemWatcher
        public void InitializeFileWatcher(string directoryPath)
        {
            fileWatcher = new FileSystemWatcher(directoryPath)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite
            };

            // Event handlers for changes
            fileWatcher.Changed += OnFileSystemChanged;
            fileWatcher.Created += OnFileSystemChanged;
            fileWatcher.Deleted += OnFileSystemChanged;
            fileWatcher.Renamed += OnFileSystemRenamed;
        }

        // Refresh the tree when a file or folder is changed, created, or deleted
        public void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                RootItems.Clear();
                LoadFileTree(@".\add_scripts_here\", RootItems);
            });
        }

        // Refresh the tree when a file or folder is renamed
        public void OnFileSystemRenamed(object sender, RenamedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                RootItems.Clear();
                LoadFileTree(@".\add_scripts_here\", RootItems);
            });
        }

        // if search, loads all the searched files
        public void LoadFilteredFileTree(string directoryPath, ObservableCollection<FileSystemItem> parentCollection, string searchText)
        {
            if (Directory.GetFiles(@".\add_scripts_here").Length != 0)
            {
                WorkspaceBorder.Visibility = Visibility.Visible;
                TabSystemz.Margin = new Thickness(200, 5, 5, 45);
            }
            else
            {
                WorkspaceBorder.Visibility = Visibility.Collapsed;
                TabSystemz.Margin = new Thickness(5, 5, 5, 45);
            }
            // Add folders
            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                var folder = new FolderItem
                {
                    Header = Path.GetFileName(dir),
                    Children = new ObservableCollection<FileSystemItem>()
                };
                LoadFilteredFileTree(dir, folder.Children, searchText);

                // Only add folder if it has children
                if (folder.Children.Any())
                {
                    /// <summary>
                    /// this makes it so as long as the foldername contains the search, it loads
                    /// this was removed since empty folders are weird and i honestly dont rlly like this
                    /// folders should load even if the names dont match since they are just a group after all
                    /// </summary>
                    //if (folder.Header.Contains(searchText))
                    //{
                    //    parentCollection.Add(folder);
                    //    return;
                    //}

                    foreach (FileSystemItem file in folder.Children)
                    {
                        if (file.Header.Contains(searchText))
                        {
                            parentCollection.Add(folder);
                            break;
                        }
                    }
                }
            }

            // Add files
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                string fileName = Path.GetFileName(file);
                if (fileName.ToLower().Contains(searchText))
                {
                    var fileItem = CreateFileItem(file, fileName);
                    if (fileItem != null)
                    {
                        parentCollection.Add(fileItem);
                    }
                }
            }
        }

        // Helper method to reconstruct the relative path of the selected file or folder
        public string GetRelativePath(FileSystemItem item)
        {
            var path = new Stack<string>();
            while (item != null)
            {
                path.Push(item.Header);
                if (item is FolderItem folderItem && RootItems.Contains(folderItem))
                {
                    break;
                }
                item = FindParent(item);
            }
            return Path.Combine(path.ToArray());
        }

        // Helper method to find the parent of an item
        public FileSystemItem FindParent(FileSystemItem child)
        {
            foreach (var folder in RootItems.OfType<FolderItem>())
            {
                if (folder.Children.Contains(child))
                    return folder;

                var parent = FindParentRecursive(folder, child);
                if (parent != null)
                    return parent;
            }
            return null;
        }

        public FileSystemItem FindParentRecursive(FolderItem folder, FileSystemItem child)
        {
            foreach (var subItem in folder.Children)
            {
                if (subItem == child)
                    return folder;

                if (subItem is FolderItem subFolder)
                {
                    var parent = FindParentRecursive(subFolder, child);
                    if (parent != null)
                        return parent;
                }
            }
            return null;
        }

        /// <summary>
        /// continue other stuff
        /// </summary>
        public async void TabSet(Grid grid)
        {
            if (BlurMain != null && TabSystemz != null && MainTabs != null)
            {
                BlurMain.Radius = 0;
                var cm = TabSystemz;
                if (cm != null) cm.Visibility = Visibility.Collapsed; // microsoft made wbeview2 absolute shit where its a whole nother window so zindex and opacity is all fucked up
                foreach (Grid tab in MainTabs.Children)
                {
                    if (tab != null && tab != grid) DisableGrid(tab);
                }
                if (Home != null) Fade(Home, halfsecond, 0, smoothEase());
                if (grid != null) EnableGrid(grid);
                if (Home != null) DisableGrid(Home);
                await Task.Delay(600);
                cm.Visibility = Visibility.Visible; // so we gotta fucking do this;
                cm.current_monaco().Visibility = Visibility.Visible;
            }

        }

        #endregion

        #region Storyboards And Animations

        // Based From Vega X / Comet || Modified With More Features by Nebula Softworks

        // VARIABLES

        /// <summary>
        /// time span variables
        /// </summary>
        public TimeSpan second = TimeSpan.FromSeconds(1);
        public TimeSpan halfsecond = TimeSpan.FromMilliseconds(500);
        public TimeSpan tenthsecond = TimeSpan.FromMilliseconds(100);
        public TimeSpan hunsecond = TimeSpan.FromMilliseconds(20);

        /// <summary>
        /// Easing Styles
        /// </summary>
        public static ExponentialEase exponentialEase(EasingMode x = EasingMode.EaseInOut) 
        { return new ExponentialEase { EasingMode = x }; }
        public static BackEase backEase(EasingMode x = EasingMode.EaseInOut)
        { return new BackEase { EasingMode = x }; }
        public static QuarticEase smoothEase(EasingMode x = EasingMode.EaseInOut)
        { return new QuarticEase { EasingMode = x }; }

        /// <summary>
        /// Smoothly Transition an Object's Opacity
        /// obj = the object to tween opacity
        /// dur = amount of time to twen | TimeSpan
        /// opac = the opacity to tween to
        /// easingStyle = the style of easing that will be applied
        /// </summary>
        public Storyboard Fade(DependencyObject obj, TimeSpan dur, Double opac = 0, IEasingFunction easingStyle = null)
        {
            if (dur == null) dur = second;
            if (easingStyle == null) easingStyle = exponentialEase();

            Storyboard fadeStoryboard = new Storyboard();
            DoubleAnimation fadeAnimation = new DoubleAnimation()
            {
                To = opac,
                Duration = dur,
                EasingFunction = easingStyle
            };
            fadeStoryboard.Children.Add(fadeAnimation);
            Storyboard.SetTarget(fadeAnimation, obj);
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath("Opacity"));

            return fadeStoryboard;
        }

        /// <summary>
        /// Smoothly Move Or Resize (Using Margins) an Object
        /// obj = the object to tween margin
        /// dur = amount of time to twen | TimeSpan
        /// margin = the margin to tween to.
        /// easingStyle = the style of easing that will be applied
        /// </summary>
        public Storyboard ObjectShift(DependencyObject obj, TimeSpan dur, Thickness margin, IEasingFunction easingStyle = null)
        {
            if (dur == null) dur = second;
            if (easingStyle == null) easingStyle = exponentialEase();

            Storyboard posStoryboard = new Storyboard();
            ThicknessAnimation posAnimation = new ThicknessAnimation()
            {
                To = margin,
                Duration = dur,
                EasingFunction = easingStyle
            };
            posStoryboard.Children.Add(posAnimation);
            Storyboard.SetTarget(posAnimation, obj);
            Storyboard.SetTargetProperty(posAnimation, new PropertyPath(MarginProperty));

            return posStoryboard;
        }

        /// <summary>
        /// Smoothly resize (using absolute numbers) an object
        /// obj = the object to tween size
        /// dur = amount of time to twen | TimeSpan
        /// height = the height to tween to
        /// width = the width to tween to
        /// easingStyle = the style of easing that will be applied
        /// heightbool = whether to not tween the height
        /// </summary>
        [Obsolete ("Absolute ass, it aint even resize, just expand width :sob: | barely used, dont reccomend for plugins")]
        public Storyboard Resize(UIElement obj, TimeSpan dur, double height, double width, IEasingFunction easingStyle = null, bool heightbool = true)
        {
            if (dur == null) dur = second; // Default to half a second
            if (easingStyle == null) easingStyle = new ExponentialEase { EasingMode = EasingMode.EaseInOut }; // Default easing

            // Create a new Storyboard
            Storyboard sizeStoryboard = new Storyboard();

            // Height Animation
            DoubleAnimation heightAnimation;
            if (heightbool != false)
            {
                heightAnimation = new DoubleAnimation()
                {
                    To = height,
                    Duration = dur,
                    EasingFunction = easingStyle
                };
                sizeStoryboard.Children.Add(heightAnimation);
                Storyboard.SetTarget(heightAnimation, obj);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
            }

            // Width Animation
            DoubleAnimation widthAnimation = new DoubleAnimation()
            { 
                To = width,
                Duration = dur,
                EasingFunction = easingStyle
            };

            // Add animations to Storyboard
            sizeStoryboard.Children.Add(widthAnimation);

            // Set the target and target properties for animations
            Storyboard.SetTarget(widthAnimation, obj);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));

            // Return the storyboard so it can be run
            return sizeStoryboard;
        }

        /// <summary>
        /// Smoothly transition the color of an object (brush, shadows etc.)
        /// obj = the obj to tween color
        /// dur = how long to tween | TimeSpan
        /// color = the color to tween too
        /// </summary>
        public Storyboard ColorShift(DependencyObject obj, TimeSpan dur, Color color)
        {
            if (dur == null) dur = second;

            Storyboard colorStoryboard = new Storyboard();
            ColorAnimation colorAnimation = new ColorAnimation()
            {
                To = color,
                Duration = second,
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut }
            };
            colorStoryboard.Children.Add(colorAnimation);
            Storyboard.SetTarget(colorAnimation, obj);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(MarginProperty));

            return colorStoryboard;
        }

        /// <summary>
        /// enable and disable panels (grids, stack panels, scroll viewers)
        /// </summary>

        public void EnableGrid(UIElement selectedGrid)
        {
            if (selectedGrid != null && selectedGrid is Panel SelectedGrid)
            {
                SelectedGrid.Margin = new Thickness(0.0, 260.0, 0.0, -260.0);
                SelectedGrid.Visibility = Visibility.Visible;
                Fade(SelectedGrid, halfsecond, 1, smoothEase()).Begin();
                ObjectShift(SelectedGrid, halfsecond, new Thickness(0.0, 0.0, 0.0, 0.0), smoothEase()).Begin();
            } else if (selectedGrid != null)
            {
                ScrollViewer Selectedgrid = (ScrollViewer)selectedGrid;
                Selectedgrid.Margin = new Thickness(0.0, 260.0, 0.0, -260.0);
                Selectedgrid.Visibility = Visibility.Visible;
                Fade(Selectedgrid, halfsecond, 1, smoothEase()).Begin();
                ObjectShift(Selectedgrid, halfsecond, new Thickness(0.0, 0.0, 0.0, 0.0), smoothEase()).Begin();
            }
        } 
        public async void DisableGrid(UIElement selectedGrid)
        {
            if(selectedGrid != null && selectedGrid is Panel SelectedGrid)
            {
                ObjectShift(SelectedGrid, halfsecond, new Thickness(0.0, 260.0, 0.0, -260.0), smoothEase()).Begin();
                Fade(SelectedGrid, halfsecond, 0, exponentialEase()).Begin();
                await Task.Delay(1000);
                SelectedGrid.Visibility = Visibility.Collapsed;
            } else if (selectedGrid != null)
            {
                ScrollViewer Selectedgrid = (ScrollViewer)selectedGrid;
                ObjectShift(Selectedgrid, halfsecond, new Thickness(0.0, 260.0, 0.0, -260.0), smoothEase()).Begin();
                Fade(Selectedgrid, halfsecond, 0, exponentialEase()).Begin();
                await Task.Delay(1000);
                Selectedgrid.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            EnableGrid(Home);
            if (BlurMain != null) BlurMain.Radius = 30;
            if (TabSystemz != null)
            {
                var cm = TabSystemz.current_monaco();
                if (cm != null) cm.Visibility = Visibility.Collapsed; // WebView2 doesnt support ZIndex and is overall weird so stuff like this must be done
            }

            /// <summary>
            /// Key Binding logic
            /// </summary>
            this.KeyDown += (sender, e) =>
            {
                // Listbox
                if (e.Key == Key.B)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control &&
                        (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        WorkspaceButton_Click(this, new RoutedEventArgs());
                    }
                }

                // Open File
                if (e.Key == Key.O)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        Open_Click(this, new RoutedEventArgs());
                    }
                }

                // save file
                if (e.Key == Key.S)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        Export_Click(this, new RoutedEventArgs());
                    }
                }
            };

            // ApplyScale(BorderBackground, Scale);

            if (LegacyAlignment == true)
            {
                //ExecButtons.HorizontalAlignment = HorizontalAlignment.Left;
            }

            Distrubutor = File.ReadAllText("./bin/redistrubutor.nebula");

            if (Distrubutor == "nebulasoftworks.framer.ai") RedistrubutorText.Visibility = Visibility.Collapsed;

            DataContext = this;
            RootItems = new ObservableCollection<FileSystemItem>();
            LoadFileTree(@".\add_scripts_here\", RootItems);

            // Initialize the FileSystemWatcher
            InitializeFileWatcher(@".\add_scripts_here\");

            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
            System.Drawing.Rectangle workingArea = screen.WorkingArea;

            this.MaxHeight = workingArea.Height;
            this.MaxWidth = workingArea.Width;

            MicaHelper.UpdateStyleAttributes(this); // the mica might be broken but ya

            /// <summary>
            /// this doesnt work for some reason, if someone can fix this, create a pull request
            /// </summary>
            client = new DiscordRPC.DiscordRpcClient("nebula_client_luna_x");
            client.Initialize();
            client.SetPresence(new DiscordRPC.RichPresence()
            {
                Details = "Nebula Client",
                State = "Using Luna X",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "https://cdn.discordapp.com/attachments/1308635550344810506/1319577351255887963/Nebula_Softworks_No_Background.png?ex=676677ad&is=6765262d&hm=52babaf35920147f4f7ca1b6a1e911f4f226bd62b1a750d72fad0c2b9dd69795&",
                    LargeImageText = "Nebula Client",
                    SmallImageKey = "https://cdn.discordapp.com/attachments/1308635550344810506/1319577351759073290/Png.png?ex=676677ad&is=6765262d&hm=a48f2fe19e6372313d5778eddcfe8521953350d80353e1be1d5882f58dfa3309&"
                }
            });

            RGBTime = new DispatcherTimer(TimeSpan.FromMilliseconds(10), DispatcherPriority.Normal, delegate
            {
                rgbRotation.Angle += RGBSpinSpeed;
                rgbRotation1.Angle += RGBSpinSpeed;
            }, System.Windows.Application.Current.Dispatcher);
            RGBTime.Start();
            SetTime();
            clock = new DispatcherTimer(TimeSpan.FromMinutes(1), DispatcherPriority.Normal, delegate
            {
                SetTime();
            }, System.Windows.Application.Current.Dispatcher);
            clock.Start();

            LoadScriptHub();

            BorderBackground.Drop += (System.Windows.DragEventHandler)((s, e) =>
            {
                try
                {
                    string[] data = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
                    string extension = System.IO.Path.GetExtension(data[0]);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(data[0]);
                    bitmapImage.EndInit();
                    switch (extension)
                    {
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                            BackgroundPath = data[0];
                            BorderBackground.Background = new ImageBrush() {
                                ImageSource = bitmapImage,
                                Stretch = Stretch.UniformToFill,
                            };
                            ContentHolder.Background = new SolidColorBrush() { Color = Color.FromArgb(188, 19, 18, 21) };
                            BackgroundImage.Visibility = Visibility.Collapsed;
                            break;
                        // gif is literally put together with sticks and glue :sob:
                        case ".gif":
                            var og = new OptionPopup("Image Error", "Gifs are very buggy and are deprecated (or atleast for now)\n it is not reccomended. are you sure you wish to continue?", "Yes");
                            og.Main.Click += delegate {
                                Popups.Children.Remove(og);
                                Blurrer.Radius = 0;
                                var cm = TabSystemz;
                                cm.Visibility = Visibility.Visible;
                                BackgroundPath = data[0];

                                BackgroundImage.Source = new Uri(data[0]);
                                BackgroundImage.Play();
                                BackgroundImage.Visibility = Visibility.Visible;
                                ContentHolder.Background = new SolidColorBrush() { Color = Color.FromArgb(188, 19, 18, 21) };
                                BorderBackground.Background = null;
                                BackgroundImage.MediaEnded += delegate
                                {
                                    BackgroundImage.Position = TimeSpan.Zero;
                                    BackgroundImage.Play();
                                };
                            }; og.Secondary.Click += delegate
                            {
                                Popups.Children.Remove(og);
                                Blurrer.Radius = 0;
                                var cm = TabSystemz;
                                cm.Visibility = Visibility.Visible;
                            };
                            Popups.Children.Add(og);
                            Blurrer.Radius = 30;
                            var cmg = TabSystemz;
                            cmg.Visibility = Visibility.Collapsed;
                            break;
                        default:
                            BackgroundPath = "";
                            var o = new OptionPopup("Image Error", "Invalid Image Extension, please convert the image to png, jpg, jpeg, or gif.", "OK");
                            o.Main.Click += delegate {
                                Popups.Children.Remove(o);
                                Blurrer.Radius = 0;
                                var cm = TabSystemz;
                                cm.Visibility = Visibility.Visible;
                            }; o.Secondary.Click += delegate
                            {
                                Popups.Children.Remove(o);
                                Blurrer.Radius = 0;
                                var cm = TabSystemz;
                                cm.Visibility = Visibility.Visible;
                            };
                            Popups.Children.Add(o);
                            Blurrer.Radius = 30;
                            var cm = TabSystemz;
                            cm.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    BackgroundPath = "";
                    MessageBox.Show($"Couldn't Read This Image: {ex}","Image Error");
                }
            });
        }

        [Obsolete ("No longer in use, replaced by topbar drag")]
        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*WindowInteropHelper h = new WindowInteropHelper(this);
            SendMessage(h.Handle, 161, 2, 0); 
            e.Handled = true;
            // check if aerosnap shit
            if (WindowState == WindowState.Normal)
            {
                MaximiseButton.Content = "\uf0c8";
                MaximiseButton.FontFamily = TryFindResource("AwesomeIcon") as FontFamily;
            }
            if (WindowState == WindowState.Maximized)
            {
                MaximiseButton.Content = "\ue923";
                MaximiseButton.FontFamily = TryFindResource("FluentIcon") as FontFamily;
            }*/
        }

        public void Drag(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper h = new WindowInteropHelper(this);
            SendMessage(h.Handle, 161, 2, 0); 
            e.Handled = true;
            if (WindowState == WindowState.Normal)
            {
                MaximiseButton.Content = "\uf0c8";
                MaximiseButton.FontFamily = TryFindResource("AwesomeIcon") as FontFamily;
            }
            if (WindowState == WindowState.Maximized)
            {
                MaximiseButton.Content = "\ue923";
                MaximiseButton.FontFamily = TryFindResource("FluentIcon") as FontFamily;
            }
        }

        public void Close_Click(object sender, RoutedEventArgs e = null)
        {
            MinHeight = 0;
            MinWidth = 0;
            var o = ObjectShift(BorderBackground, tenthsecond, new Thickness(95, 65, 95, 65));
            o.Begin();
            var f = Fade(this, halfsecond, 0);
            f.Completed += delegate { Application.Current.Shutdown(0); };
            f.Begin();
        }

        public void MaximiseClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                MaximiseButton.Content = "\ue923";
                MaximiseButton.FontFamily = TryFindResource("FluentIcon") as FontFamily;
            }
            else
            {
                WindowState = WindowState.Normal;
                MaximiseButton.Content = "\uf0c8";
                MaximiseButton.FontFamily = TryFindResource("AwesomeIcon") as FontFamily;
            }
        }

        public void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        [Obsolete ("All Events have been moved to starting Class")]
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public async void NavigationClick(object sende, RoutedEventArgs e)
        {
            var sender = (RadioButton)sende;
            switch (sender.Name)
            {
                case "HomeRadioBtn":
                    EnableGrid(Home);
                    if (BlurMain != null) BlurMain.Radius = 30;
                    if (TabSystemz != null)
                    {
                        var cm = TabSystemz.current_monaco();
                        if (cm != null) cm.Visibility = Visibility.Collapsed;
                        TabSystemz.Visibility = Visibility.Visible;
                    }
                    await Task.Delay(1000);
                    break;
                case "ExecRadioBtn":
                    TabSet(Executor);
                    await Task.Delay(1000);
                    break;
                case "CloudRadioBtn":
                    TabSet(Cloud);
                    await Task.Delay(1000);
                    break;
                case "QuickRadioBtn":
                    TabSet(QuickPanel);
                    await Task.Delay(1000);
                    break;
                case "PluginRadioBtn":
                    TabSet(Packages);
                    await Task.Delay(1000);
                    break;
                case "FriendsRadioBtn":
                    TabSet(Friends);
                    await Task.Delay(1000);
                    break;
                case "SettingsRadioBtn":
                    TabSet(Settings);
                    await Task.Delay(1000);
                    break;
            }
        }

        #region Home Tab 

        public void SetTime()
        {
            if (!HorizontalClock) {
                Clock.Text = DateTime.Now.ToString("hh\nmm");
                Clock.FontSize = 40;
                Clock.FontFamily = TryFindResource("Oswald") as FontFamily;
                Greet.Margin = new Thickness(40, 135, 40 , 40);
                Respect.Margin = new Thickness(40, 155, 40, 40); }

            else { 
                Clock.Text = DateTime.Now.ToString("hh:mm");
                Clock.FontSize = 65;
                Clock.FontFamily = TryFindResource("OswaldSemilight") as FontFamily;
                Greet.Margin = new Thickness(45, 109, 40 , 40);
                Respect.Margin = new Thickness(45, 129, 40, 40); }

            if (DateTime.Now.Hour < 12 && DateTime.Now.Hour > 6)
            {
                Greet.Text = $"Good Morning {DisplayName}";
                Respect.Text = "Have A Great Day Ahead!";
            }
            else if (DateTime.Now.Hour < 20)
            {
                Greet.Text = $"Good Afternoon {DisplayName}";
                Respect.Text = "How's your day?";
            }
            else
            {
                Greet.Text = $"Good Night {DisplayName}";
                Respect.Text = "Wish you sweet dreams.";
            }
        }

        // BUTTON INTERACTIONS

        public void SiteRedirectbtn_Click(object sender, RoutedEventArgs e)
        {
            Redirect(Website);
        }

        public void discordBtn_Click(object sender, RoutedEventArgs e)
        {
            Redirect("https://dsc.gg/nebulasoftworks");
        }

        public void reloadBtn_Click(object sender, RoutedEventArgs e)
        {
            string launcherPath = Environment.GetCommandLineArgs().Length > 1 ? Environment.GetCommandLineArgs()[1] : null;

            if (!string.IsNullOrEmpty(launcherPath))
            {
                Process.Start(launcherPath);
            }
            else Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Close_Click(this);
        }

        public void githubBtn_Click(object sender, RoutedEventArgs e)
        {
            Redirect(GithubRepo);
        }

        #endregion

        #region Executor

        public void WorkspaceSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = WorkspaceSearch.Text.ToLower();
            RootItems.Clear();
            LoadFilteredFileTree(@".\add_scripts_here\", RootItems, searchText);
        }

        [Obsolete ("Not in use as of Alpha pre-*build* 2")]
        public void ExecButtons_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        [Obsolete("Not in use as of Alpha pre-*build* 2")]
        public void ExecButtons_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        // i gave up making this look readable
        public void Extra_Click(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            var w = b.Width;
            var x = 0;
            if (w == 205)
            {
                x = 30;
                //Extra.ToolTip = "View More";
                //ViewMoreLabel.Content = "";
            }
            else 
            { 
                x = 205;
                //Extra.ToolTip = "View Less";
                //ViewMoreLabel.Content = "";
            }
            //Resize(Extra, halfsecond, Extra.Height, x, smoothEase()).Begin();
        }

        public void ExecFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var execFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "Nebula Client | File Manager - Execute File Content",
                    Filter = "Script Files (*.txt;*.lua;*.luau)|*.txt;*.lua;*.luau|All files (*.*)|*.*",
                    Multiselect = false,
                    RestoreDirectory = true,
                };
                if (execFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileContent = File.ReadAllText(execFileDialog.FileName);

                    api.RunCode(fileContent, Popups, Blurrer, TabSystemz);
                }

            }
            catch
            {
                MessageBox.Show("Error Executing File Contents", "File Manager");
            }
        }

        public void Clients_Click(object sender, RoutedEventArgs e)
        {
            api.Inject();
        }

        public async void Execute_Click(object sender , RoutedEventArgs e)
        {
            var cm = TabSystemz.current_monaco();
            string s = await cm.GetText();
            api.RunCode(s, Popups, Blurrer, TabSystemz);
        }

        public void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "Nebula Client | File Manager - Import Into Editor",
                    Filter = "Script Files (*.txt;*.lua;*.luau)|*.txt;*.lua;*.luau|All files (*.*)|*.*",
                    Multiselect = false,
                    RestoreDirectory = true,
                };
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);

                    var o = new OptionPopup("File Manager - Import Into Editor", "Would You Like To Load The File In A New Tab?", "Yes");
                    o.Secondary.Content = "No";
                    o.Secondary.Click += delegate
                    {
                        TabSystemz.current_monaco().SetText(fileContent);
                        Blurrer.Radius = 0;
                        var cm = TabSystemz;
                        cm.Visibility = Visibility.Visible;
                        Popups.Children.Remove(o);
                    };
                    o.Main.Click += delegate
                    {
                        TabSystemz.add_tab_with_text(fileContent, openFileDialog.SafeFileName);
                        Blurrer.Radius = 00;
                        var cm = TabSystemz;
                        cm.Visibility = Visibility.Visible;
                        Popups.Children.Remove(o);
                    };
                    Popups.Children.Add(o);
                    Blurrer.Radius = 30;
                    var cm = TabSystemz;
                    cm.Visibility = Visibility.Collapsed;
                }

            }
            catch
            {
                MessageBox.Show("Error Importing File Contents Into Editor","File Manager");
            }
        }

        public void Clear_Click(object sender, RoutedEventArgs e)
        {
            try { 
                TabSystemz.current_monaco().SetText(""); 
            } 
            catch {
                MessageBox.Show("Error Clearing Current Editor","Tab System");
            }
        }

        public async void Export_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog
            {
                FileName = await TabSystemz.GetCurrentTabTitle(),
                Title = "Nebula Client | File Manager - Export Contents to File",
                Filter = "Script Files (*.txt;*.lua;*.luau)|*.txt;*.lua;*.luau|All files (*.*)|*.*",
                RestoreDirectory = true,
            };

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    var cm = TabSystemz.current_monaco();
                    string text = await cm.GetText();
                    string result = text;

                    if (string.IsNullOrEmpty(result))
                    {
                        MessageBox.Show("No Content in Editor", "File Manager");
                    }
                    else
                    {
                        try
                        {
                            File.WriteAllText(filePath, result);
                            string savedFileName = Path.GetFileName(saveFileDialog.FileName);
                            //MessageBox.Show($"Saved {savedFileName} with Editor Content", "Save File Manager");
                            TabSystemz.ChangeCurrentTabTitle(savedFileName);
                        }
                        catch (Exception)
                        {
                            //MessageBox.Show("Error Saving File", "File Manager");
                        }
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Error Saving File", "File Manager");
                }
            }
        }

        public async void Copy_Click(object sender, RoutedEventArgs e)
        {
            var cm = TabSystemz.current_monaco();
            Clipboard.SetText(await cm.GetText());
        }

        public void Paste_Click(object sender, RoutedEventArgs e)
        {
            var cm = TabSystemz.current_monaco();
            cm.SetText(Clipboard.GetText());
        }

        public void Undo_Click(object sender, RoutedEventArgs e)
        {
            var cm = TabSystemz.current_monaco();
            cm.Undo();
        }

        public void Redo_Click(object sender, RoutedEventArgs e)
        {
            var cm = TabSystemz.current_monaco();
            cm.Redo();
        }

        public void WorkspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkspaceBorder.Width != 0) 
            {
                Resize(WorkspaceBorder, halfsecond, Double.NaN, 0, smoothEase(), false).Begin();
                ObjectShift(TabSystemz, halfsecond, new Thickness(5, 5, 5, 45)).Begin();
                ObjectShift(WorkspaceButton, halfsecond, new Thickness(5)).Begin();
            }
            else 
            { 
                Resize(WorkspaceBorder, halfsecond, Double.NaN, 190, smoothEase(), false).Begin();
                ObjectShift(TabSystemz, halfsecond, new Thickness(200, 5, 5, 45)).Begin();
                ObjectShift(WorkspaceButton, halfsecond, new Thickness(200, 5, 5, 5)).Begin();
            }
        }

        public void Workspace_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is FileSystemItem selectedItem && !(selectedItem is FolderItem))
            {
                string filePath = Path.Combine(@".\add_scripts_here\", GetRelativePath(selectedItem));
                if (File.Exists(filePath))
                {
                    try
                    {
                        var cm = TabSystemz.current_monaco();
                        cm.SetText(File.ReadAllText(filePath));
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region Cloud Scripts

        /// <summary>
        /// so uhh this is extremeley laggy so as of Dev Build 0.18 this was shifted to a seperate page
        /// </summary>

        //public void ScriptHubSelect_Selected(object sender, RoutedEventArgs e)
        //{
        //    switch (ScriptHubSelect.SelectedIndex)
        //    {
        //        case 0:
        //            DisableGrid(RscriptsScroller);
        //            DisableGrid(MastersMZScroller);
        //            DisableGrid(CommunityScroller);
        //            EnableGrid(ScriptBloxScroller);
        //            break;
        //        case 1:
        //            DisableGrid(ScriptBloxScroller);
        //            DisableGrid(MastersMZScroller);
        //            DisableGrid(CommunityScroller);
        //            EnableGrid(RscriptsScroller);
        //            break;
        //        case 2:
        //            DisableGrid(ScriptBloxScroller);
        //            DisableGrid(RscriptsScroller);
        //            DisableGrid(CommunityScroller);
        //            EnableGrid(MastersMZScroller);
        //            break;
        //        case 3:
        //            DisableGrid(ScriptBloxScroller);
        //            DisableGrid(RscriptsScroller);
        //            DisableGrid(MastersMZScroller);
        //            EnableGrid(CommunityScroller);
        //            break;
        //    }
        //}

        //// SUB-REGION Scriptblox
        //public async void LoadScriptBlox()
        //{
        //    httpClient = new HttpClient();
        //    await Task.Delay(0);
        //    new Thread((ThreadStart)async delegate
        //    {
        //        if (string.IsNullOrEmpty(Game_SearchText))
        //        {
        //            Game_SearchText = " ";
        //        }
        //        try
        //        {

        //            HttpResponseMessage response = await httpClient.GetAsync("https://scriptblox.com/api/script/search?q=" + Game_SearchText + "&mode=free&max=18&page=" + CurrentPage);
        //            try
        //            {
        //                HttpContent content = response.Content;
        //                try
        //                {
        //                    dynamic val = JsonConvert.DeserializeObject(await content.ReadAsStringAsync());
        //                    if (Game_SearchText_Changed)
        //                    {
        //                        Game_SearchText_Changed = false;
        //                        Double.TryParse(val.result.totalPages.ToString(), out MaxPage);
        //                    }
        //                    if (CurrentPage <= MaxPage)
        //                    {
        //                        await Dispatcher.InvokeAsync(() =>
        //                        {
        //                            foreach (dynamic item in val.result.scripts)
        //                            {
        //                                Dispatcher.Invoke(delegate
        //                                {
        //                                    bool isPatched = false;
        //                                    bool flag = false;
        //                                    if (item.ContainsKey("isPatched"))
        //                                    {
        //                                        isPatched = item.isPatched;
        //                                    }
        //                                    flag = !((!item.ContainsKey("key")) ? true : false) && (bool)item.key;
        //                                    Script_Hub_Square ScriptBloxWindow = new Script_Hub_Square(isPatched, flag);
        //                                    try
        //                                    {
        //                                        if (item.game.imageUrl.ToString().Contains("rbxcdn.com"))
        //                                        {
        //                                            ScriptBloxWindow.Image.Source = new BitmapImage(new Uri(item.game.imageUrl.ToString()));
        //                                        }
        //                                        else
        //                                        {
        //                                            ScriptBloxWindow.Image.Source = new BitmapImage(new Uri("https://scriptblox.com" + item.game.imageUrl.ToString()));
        //                                        }
        //                                    }
        //                                    catch
        //                                    {
        //                                    }
        //                                    ScriptBloxWindow.ScriptName.Content = (object)CleanString(item.title.ToString());
        //                                    ScriptBloxWindow.GameName.Content = (object)CleanString(item.game.name.ToString());
        //                                    ScriptBloxWindow.ExecuteInteract.Click += delegate
        //                                    {
        //                                        api.RunCode(item.script.ToString(), Popups, Blurrer, TabSystemz);
        //                                    };
        //                                    ScriptBloxWindow.InfoScriptB.Click += delegate
        //                                    {
        //                                        Clipboard.SetText(item.script.ToString());
        //                                    };
        //                                    if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json"))
        //                                    {
        //                                        ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
        //                                    }
        //                                    else
        //                                    {
        //                                        ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
        //                                    }
        //                                    ScriptBloxWindow.FavoriteScriptB.Click += delegate
        //                                    {
        //                                        if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json"))
        //                                        {
        //                                            try
        //                                            {
        //                                                File.Delete(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json");
        //                                                ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
        //                                                return;
        //                                            }
        //                                            catch
        //                                            {
        //                                                return;
        //                                            }
        //                                        }
        //                                        jsonstrings jsonstrings = new jsonstrings
        //                                        {
        //                                            Script = new List<jsonstrings.ScriptHub>
        //                                        {
        //                                        new jsonstrings.ScriptHub
        //                                        {
        //                                            Name = CleanString(item.title.ToString()),
        //                                            GameName = CleanString(item.game.name.ToString()),
        //                                            Image = item.game.imageUrl.ToString(),
        //                                            Script = item.script.ToString()
        //                                        }
        //                                        }
        //                                        };
        //                                        try
        //                                        {
        //                                            File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json", JsonConvert.SerializeObject((object)jsonstrings, (Newtonsoft.Json.Formatting)1));
        //                                            ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
        //                                        }
        //                                        catch
        //                                        {
        //                                        }
        //                                    };
        //                                    ScriptBloxWrapper.Children.Add(ScriptBloxWindow);
        //                                });
        //                            }
        //                        }, DispatcherPriority.Background);
        //                    }
        //                }
        //                finally
        //                {
        //                    ((IDisposable)content)?.Dispose();
        //                }
        //            }
        //            finally
        //            {
        //                ((IDisposable)response)?.Dispose();
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            await Dispatcher.InvokeAsync(() =>
        //            {
        //                var o = new OptionPopup("Script Blox Interrogation", "The Script Blox System has encountered a Connectivity Issue.\n" + ex.ToString(), "OK");
        //                o.Main.Click += delegate
        //                {
        //                    Popups.Children.Remove(o);
        //                    Blurrer.Radius = 0;
        //                    var cm = TabSystemz;
        //                    cm.Visibility = Visibility.Visible;
        //                };
        //                o.Secondary.Click += delegate
        //                {
        //                    Popups.Children.Remove(o);
        //                    Blurrer.Radius = 0;
        //                    var cm = TabSystemz;
        //                    cm.Visibility = Visibility.Visible;
        //                };
        //                Popups.Children.Add(o);
        //                Blurrer.Radius = 30;
        //                var cm = TabSystemz;
        //                cm.Visibility = Visibility.Collapsed;
        //                File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\errors\{DateTime.Now.ToLongDateString()}.error", ex.ToString());
        //                print(DateTime.Now.ToLongDateString());
        //            });
        //        }
        //        GC.Collect(2, GCCollectionMode.Forced);
        //    }).Start();

        //}

        //public void ScriptBloxSearch()
        //{
        //    CurrentPage = 1;
        //    ScriptBloxWrapper.Children?.Clear();
        //    ScriptBloxScroller?.ScrollToTop();
        //    LoadScriptBlox();
        //}
        //// ENDSUB-REGION

        //// SUB-REGION Rscripts
        //public async void LoadRscripts()
        //{
        //    httpClient = new HttpClient();
        //    await Task.Delay(0);
        //    new Thread((ThreadStart)async delegate
        //    {
        //        if (string.IsNullOrEmpty(Game_SearchText))
        //        {
        //            Game_SearchText = " ";
        //        }
        //        try
        //        {
        //            string requestUri = $"https://rscripts.net/api/v2/scripts?page={CurrentPage}&notPaid=true&orderBy=date&q={Game_SearchText}";
        //            HttpResponseMessage response = await httpClient.GetAsync(requestUri);
        //            response.EnsureSuccessStatusCode();
        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            dynamic val = JsonConvert.DeserializeObject(responseBody);

        //            if (Game_SearchText_Changed)
        //            {
        //                Game_SearchText_Changed = false;
        //                MaxPage = val.info.maxPages;
        //            }

        //            if (CurrentPage <= MaxPage)
        //            {
        //                await Dispatcher.InvokeAsync(() =>
        //                {
        //                    foreach (dynamic item in val.scripts)
        //                    {
        //                        base.Dispatcher.Invoke(delegate
        //                        {
        //                            bool isPatched = false;
        //                            bool flag = false;
        //                            flag = item.keySystem;
        //                            Script_Hub_Square RScriptsWindow = new Script_Hub_Square(isPatched, flag);
        //                            try
        //                            {
        //                                RScriptsWindow.Image.Source = new BitmapImage(new Uri(item.image.ToString()));
        //                            }
        //                            catch
        //                            {
        //                            }
        //                            RScriptsWindow.ScriptName.Content = (object)CleanString(item.title.ToString());
        //                            RScriptsWindow.GameName.Content = CleanString(item?.game?.title?.ToString()); // this for some reason always returns null so we add ? to wait for it to initialize
        //                            RScriptsWindow.ExecuteInteract.Click += delegate
        //                            {
        //                                api.RunCode(HttpGet(item.rawScript.ToString()), Popups, Blurrer, TabSystemz);
        //                            };
        //                            RScriptsWindow.InfoScriptB.Click += delegate
        //                            {
        //                                Clipboard.SetText(HttpGet(item.rawScript.ToString()));
        //                            };
        //                            if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json"))
        //                            {
        //                                RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
        //                            }
        //                            else
        //                            {
        //                                RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
        //                            }
        //                            RScriptsWindow.FavoriteScriptB.Click += delegate
        //                            {
        //                                if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json"))
        //                                {
        //                                    try
        //                                    {
        //                                        File.Delete(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json");
        //                                        RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
        //                                        return;
        //                                    }
        //                                    catch
        //                                    {
        //                                        return;
        //                                    }
        //                                }
        //                                jsonstrings jsonstrings = new jsonstrings
        //                                {
        //                                    Script = new List<jsonstrings.ScriptHub>
        //                                        {
        //                                        new jsonstrings.ScriptHub
        //                                        {
        //                                            Name = CleanString(item.title.ToString()),
        //                                            GameName = CleanString(item.game.title.ToString()),
        //                                            Image = item.image.ToString(),
        //                                            Script = HttpGet(item.rawScript.ToString())
        //                                        }
        //                                        }
        //                                };
        //                                try
        //                                {
        //                                    File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + CleanString(item.title.ToString()) + ".json", JsonConvert.SerializeObject((object)jsonstrings, (Newtonsoft.Json.Formatting)1));
        //                                    RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
        //                                }
        //                                catch
        //                                {
        //                                }
        //                            };
        //                            RScriptsWrapper.Children.Add(RScriptsWindow);
        //                        });
        //                    }
        //                }, DispatcherPriority.Background);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            await Dispatcher.InvokeAsync(() =>
        //            {
        //                var errorPopup = new OptionPopup("Rscripts API Error", $"An error occurred while fetching scripts:\n{ex.Message}", "OK");
        //                errorPopup.Main.Click += delegate
        //                {
        //                    Popups.Children.Remove(errorPopup);
        //                    Blurrer.Radius = 0;
        //                    TabSystemz.Visibility = Visibility.Visible;
        //                };
        //                errorPopup.Secondary.Click += delegate
        //                {
        //                    Popups.Children.Remove(errorPopup);
        //                    Blurrer.Radius = 0;
        //                    TabSystemz.Visibility = Visibility.Visible;
        //                };
        //                Popups.Children.Add(errorPopup);
        //                Blurrer.Radius = 30;
        //                TabSystemz.Visibility = Visibility.Collapsed;

        //                File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\errors\{DateTime.Now.ToLongDateString()}.error", ex.ToString());
        //                print(ex.ToString());
        //            });
        //        }
        //        finally
        //        {
        //            GC.Collect(2, GCCollectionMode.Forced);
        //        }
        //    }).Start();
        //}

        //public void RscriptsSearch()
        //{
        //    CurrentPage = 1;
        //    RScriptsWrapper.Children?.Clear();
        //    RscriptsScroller?.ScrollToTop();
        //    LoadRscripts();
        //}
        //// ENDSUB-REGION

        //// SUB-REGION Favourited Scripts (yes i spell it like that, im singaporean)
        //public async void LoadFavoriteScripts()
        //{
        //    await Task.Run(delegate
        //    {
        //        this.Dispatcher.Invoke((Action)(() => this.FavoriteScriptsWrapper.Children.Clear()));
        //        try
        //        {
        //            foreach (string enumerateFile in Directory.EnumerateFiles(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\", "*.json"))
        //                ((IEnumerable<JToken>)JObject.Parse(System.IO.File.ReadAllText(enumerateFile))["Script"]).ToList<JToken>().ForEach((Action<JToken>)(item => this.Dispatcher.Invoke((Action)(() =>
        //                {
        //                    if (!item[(object)"Name"].ToString().ToLower().Contains(this.ScriptHubSearch.Text.ToLower()))
        //                        return;
        //                    bool isPatched = false;
        //                    bool flag = false;
        //                    if (item.Contains("isPatched"))
        //                    {
        //                        isPatched = true;
        //                    }
        //                    flag = !((!item.Contains("key")) ? true : false);
        //                    Favourite_Script_Square FavoritedScript = new Favourite_Script_Square(isPatched, flag);
        //                    try
        //                    {
        //                        FavoritedScript.Image.Source = !item[(object)"Image"].ToString().Contains("rbxcdn.com") ? (ImageSource)new BitmapImage(new Uri("https://scriptblox.com" + item[(object)"Image"].ToString())) : (ImageSource)new BitmapImage(new Uri(item[(object)"Image"].ToString()));
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    FavoritedScript.ScriptName.Content = (object)this.CleanString(item[(object)"Name"].ToString());
        //                    FavoritedScript.GameName.Content = (object)this.CleanString(item[(object)"GameName"].ToString());
        //                    FavoritedScript.ExecuteInteract.Click += (RoutedEventHandler)((s, e) =>
        //                    {
        //                        api.RunCode(item[(object)"Script"].ToString(), Popups, Blurrer, TabSystemz);
        //                    });
        //                    FavoritedScript.FavoriteScriptB.Click += (RoutedEventHandler)((s, e) =>
        //                    {
        //                        (FavoritedScript.Parent as WrapPanel).Children.Remove((UIElement)FavoritedScript);
        //                        System.IO.File.Delete(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + this.CleanString(item[(object)"Name"].ToString()) + ".json");
        //                    });
        //                    FavoritedScript.InfoScriptB.Click += delegate
        //                    {
        //                        Clipboard.SetText(item[(object)"Script"].ToString());
        //                    };
        //                    this.FavoriteScriptsWrapper.Children.Add((UIElement)FavoritedScript);
        //                }))));
        //        }
        //        catch
        //        {
        //        }
        //        GC.Collect(2, GCCollectionMode.Forced);
        //    });
        //} 
        //// ENDSUB-REGION

        //public void ScriptHubSearch_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Return)
        //    {
        //        ScriptHubSearch.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
        //        Game_SearchText_Changed = true;
        //        Game_SearchText = ScriptHubSearch.Text;
        //        switch (ScriptHubSelect.SelectedIndex)
        //        {
        //            case 0:
        //                ScriptBloxSearch();
        //                break;
        //            case 1:
        //                RscriptsSearch();
        //                break;
        //            case 2:
        //                break;
        //        }
        //    }
        //}

        //public void SearchButton_Click(object sender, RoutedEventArgs e) => 
        //    ScriptHubSearch_KeyDown(sender, new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(Application.Current.MainWindow), 0,Key.Return));

        //public void ScriptScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        //{
        //    ScrollViewer ScriptScroller = null;
        //    switch (ScriptHubSelect.SelectedIndex)
        //    {
        //        case 0:
        //            ScriptScroller = ScriptBloxScroller;
        //            break;
        //        case 1:
        //            ScriptScroller = RscriptsScroller;
        //            break;
        //        case 2:
        //            ScriptScroller = MastersMZScroller;
        //            break;
        //        case 3:
        //            ScriptScroller = CommunityScroller;
        //            break;
        //    }
        //    if ((ScriptScroller.ScrollableHeight < 0.0 || ScriptScroller.VerticalOffset == ScriptScroller.ScrollableHeight) && CurrentPage < MaxPage)
        //    {
        //        CurrentPage++;
        //        switch (ScriptHubSelect.SelectedIndex)
        //        {
        //            case 0:
        //                LoadScriptBlox();
        //                break;
        //            case 1:
        //                LoadRscripts();
        //                break;
        //            case 2:
        //                break;
        //        }
        //    }
        //}

        //public void CloudScriptToggle(object sender, RoutedEventArgs e)
        //{
        //    switch (((Button)sender).Name)
        //    {
        //        case "CloudScriptButton":
        //            CloudScriptButton.Foreground = TryFindResource("Accent") as LinearGradientBrush;
        //            FavouriteScriptButton.Foreground = Brushes.White;
        //            EnableGrid(ScriptScrollers);
        //            DisableGrid(FavoriteScriptScroller);
        //            ScriptHubSelect.IsEnabled = true;
        //            break;
        //        case "FavouriteScriptButton":
        //            CloudScriptButton.Foreground = Brushes.White;
        //            FavouriteScriptButton.Foreground = TryFindResource("Accent") as LinearGradientBrush;
        //            DisableGrid(ScriptScrollers);
        //            EnableGrid(FavoriteScriptScroller);
        //            ScriptHubSelect.IsEnabled = false;
        //            LoadFavoriteScripts();
        //            break;
        //    }
        //}

        public void LoadScriptHub()
        {
            cloud = new ScriptHub();
        }

        #endregion
    }
}
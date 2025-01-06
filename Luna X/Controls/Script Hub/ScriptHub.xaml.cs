using Luna_X.Controls.Misc;
using Luna_X.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Luna_X;

namespace Luna_X.Controls.Script_Hub
{
    /// <summary>
    /// Interaction logic for ScriptHub.xaml
    /// since its to laggy in the mainwindow
    /// </summary>
    public partial class ScriptHub : UserControl
    {
        HttpClient httpClient;

        private string Game_SearchText = " ";

        private bool Game_SearchText_Changed = true;

        private double CurrentPage = 1;

        private double MaxPage = 500;

        // lazy to redefine animations and since my dumbass didnt put them in a class we have to do this shit
        // i might reorganize evrything in the future
        private MainWindow ani = Application.Current.MainWindow as MainWindow;


        public ScriptHub()
        {
            InitializeComponent();
        }

        private void ScriptHubSelect_Selected(object sender, RoutedEventArgs e)
        {
            switch (ScriptHubSelect.SelectedIndex)
            {
                case 0:
                    ani.DisableGrid(RscriptsScroller);
                    ani.DisableGrid(MastersMZScroller);
                    ani.DisableGrid(CommunityScroller);
                    ani.EnableGrid(ScriptBloxScroller);
                    break;
                case 1:
                    ani.DisableGrid(ScriptBloxScroller);
                    ani.DisableGrid(MastersMZScroller);
                    ani.DisableGrid(CommunityScroller);
                    ani.EnableGrid(RscriptsScroller);
                    break;
                case 2:
                    ani.DisableGrid(ScriptBloxScroller);
                    ani.DisableGrid(RscriptsScroller);
                    ani.DisableGrid(CommunityScroller);
                    ani.EnableGrid(MastersMZScroller);
                    break;
                case 3:
                    ani.DisableGrid(ScriptBloxScroller);
                    ani.DisableGrid(RscriptsScroller);
                    ani.DisableGrid(MastersMZScroller);
                    ani.EnableGrid(CommunityScroller);
                    break;
            }
        }

        // SUB-REGION Scriptblox
        private async void LoadScriptBlox()
        {
            httpClient = new HttpClient();
            await Task.Delay(0);
            await Task.Run(async delegate
            {
                if (string.IsNullOrEmpty(Game_SearchText))
                {
                    Game_SearchText = " ";
                }
                try
                {

                    HttpResponseMessage response = await httpClient.GetAsync("https://scriptblox.com/api/script/search?q=" + Game_SearchText + "&mode=free&max=18&page=" + CurrentPage);
                    try
                    {
                        HttpContent content = response.Content;
                        try
                        {
                            dynamic val = JsonConvert.DeserializeObject(await content.ReadAsStringAsync());
                            if (Game_SearchText_Changed)
                            {
                                Game_SearchText_Changed = false;
                                Double.TryParse(val.result.totalPages.ToString(), out MaxPage);
                            }
                            if (CurrentPage <= MaxPage)
                            {
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    foreach (dynamic item in val.result.scripts)
                                    {
                                        Dispatcher.Invoke(delegate
                                        {
                                            bool isPatched = false;
                                            bool flag = false;
                                            if (item.ContainsKey("isPatched"))
                                            {
                                                isPatched = item.isPatched;
                                            }
                                            flag = !((!item.ContainsKey("key")) ? true : false) && (bool)item.key;
                                            Script_Hub_Square ScriptBloxWindow = new Script_Hub_Square(isPatched, flag);
                                            try
                                            {
                                                if (item.game.imageUrl.ToString().Contains("rbxcdn.com"))
                                                {
                                                    ScriptBloxWindow.Image.ImageSource = new BitmapImage(new Uri(item.game.imageUrl.ToString()));
                                                }
                                                else
                                                {
                                                    ScriptBloxWindow.Image.ImageSource = new BitmapImage(new Uri("https://scriptblox.com" + item.game.imageUrl.ToString()));
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            ScriptBloxWindow.ScriptName.Content = (string)ani.CleanString(item.title.ToString());
                                            ScriptBloxWindow.GameName.Content = (string)ani.CleanString(item.game.name.ToString());
                                            ScriptBloxWindow.ExecuteInteract.Click += delegate
                                            {
                                                api.RunCode(item.script.ToString(), ani.Popups, ani.Blurrer, ani.TabSystemz);
                                            };
                                            ScriptBloxWindow.InfoScriptB.Click += delegate
                                            {
                                                Clipboard.SetText(item.script.ToString());
                                            };
                                            if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json"))
                                            {
                                                ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
                                            }
                                            else
                                            {
                                                ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                                            }
                                            ScriptBloxWindow.FavoriteScriptB.Click += delegate
                                            {
                                                if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json"))
                                                {
                                                    try
                                                    {
                                                        File.Delete(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json");
                                                        ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                                                        return;
                                                    }
                                                    catch
                                                    {
                                                        return;
                                                    }
                                                    finally
                                                    {
                                                        ((IDisposable)content)?.Dispose();
                                                    }
                                                }
                                                jsonstrings jsonstrings = new jsonstrings
                                                {
                                                    Script = new List<jsonstrings.ScriptHub>
                                                {
                                                new jsonstrings.ScriptHub
                                                {
                                                    Name = ani.CleanString(item.title.ToString()),
                                                    GameName = ani.CleanString(item.game.name.ToString()),
                                                    Image = item.game.imageUrl.ToString(),
                                                    Script = item.script.ToString()
                                                }
                                                }
                                                };
                                                try
                                                {
                                                    File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json", JsonConvert.SerializeObject((object)jsonstrings, (Newtonsoft.Json.Formatting)1));
                                                    ScriptBloxWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
                                                }
                                                catch
                                                {
                                                }
                                                finally
                                                {
                                                    ((IDisposable)content)?.Dispose();
                                                }
                                            };
                                            ScriptBloxWrapper.Children.Add(ScriptBloxWindow);
                                        });
                                    }
                                }, DispatcherPriority.Background);
                            }
                        }
                        finally
                        {
                            ((IDisposable)content)?.Dispose();
                        }
                    }
                    finally
                    {
                        ((IDisposable)response)?.Dispose();
                    }
                }

                catch (Exception ex)
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        var o = new OptionPopup("Script Blox Interrogation", "The Script Blox System has encountered a Connectivity Issue.\n" + ex.ToString(), "OK");
                        o.Main.Click += delegate
                        {
                            ani.Popups.Children.Remove(o);
                            ani.Blurrer.Radius = 0;
                            var cm = ani.TabSystemz;
                            cm.Visibility = Visibility.Visible;
                        };
                        o.Secondary.Click += delegate
                        {
                            ani.Popups.Children.Remove(o);
                            ani.Blurrer.Radius = 0;
                            var cm = ani.TabSystemz;
                            cm.Visibility = Visibility.Visible;
                        };
                        ani.Popups.Children.Add(o);
                        ani.Blurrer.Radius = 30;
                        var cm = ani.TabSystemz;
                        cm.Visibility = Visibility.Collapsed;
                        File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\errors\{DateTime.Now.ToLongDateString()}.error", ex.ToString());
                        ani.print(DateTime.Now.ToLongDateString());
                    });
                }
                GC.Collect(2, GCCollectionMode.Forced);
            });

        }

        private void ScriptBloxSearch()
        {
            CurrentPage = 1;
            ScriptBloxWrapper.Children.Clear();
            ScriptBloxScroller.ScrollToTop();
            LoadScriptBlox();
        }
        // ENDSUB-REGION

        // SUB-REGION Rscripts
        private async void LoadRscripts()
        {
            //httpClient = new HttpClient();
            //await Task.Delay(0);
            //await Task.Run(async delegate
            //{
            //    if (string.IsNullOrEmpty(Game_SearchText))
            //    {
            //        Game_SearchText = " ";
            //    }
            //    try
            //    {
            //        string requestUri = $"https://rscripts.net/api/v2/scripts?page={CurrentPage}&notPaid=true&orderBy=date&q={Game_SearchText}";
            //        HttpResponseMessage response = await httpClient.GetAsync(requestUri);
            //        response.EnsureSuccessStatusCode();
            //        string responseBody = await response.Content.ReadAsStringAsync();
            //        try
            //        {
            //            try
            //            {
            //                dynamic val = JsonConvert.DeserializeObject(responseBody);

            //                if (Game_SearchText_Changed)
            //                {
            //                    Game_SearchText_Changed = false;
            //                    MaxPage = val.info.maxPages;
            //                }

            //                if (CurrentPage <= MaxPage)
            //                {
            //                    await Dispatcher.InvokeAsync(() =>
            //                    {
            //                        foreach (dynamic item in val.scripts)
            //                        {
            //                            base.Dispatcher.Invoke(delegate
            //                            {
            //                                bool isPatched = false;
            //                                bool flag = false;
            //                                flag = item.keySystem;
            //                                Script_Hub_Square RScriptsWindow = new Script_Hub_Square(isPatched, flag);
            //                                try
            //                                {
            //                                    RScriptsWindow.Image.ImageSource = new BitmapImage(new Uri(item.image.ToString()));
            //                                }
            //                                catch
            //                                {
            //                                }
            //                                RScriptsWindow.ScriptName.Content = ani.CleanString(item.title.ToString());
            //                                RScriptsWindow.GameName.Content = ani.CleanString(item?.game?.title?.ToString()); // this for some reason always returns null so we add ? to wait for it to initialize
            //                                RScriptsWindow.ExecuteInteract.Click += delegate
            //                                {
            //                                    api.RunCode(ani.HttpGet(item.rawScript.ToString()), ani.Popups, ani.Blurrer, ani.TabSystemz);
            //                                };
            //                                RScriptsWindow.InfoScriptB.Click += delegate
            //                                {
            //                                    Clipboard.SetText(ani.HttpGet(item.rawScript.ToString()));
            //                                };
            //                                if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json"))
            //                                {
            //                                    RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
            //                                }
            //                                else
            //                                {
            //                                    RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
            //                                }
            //                                RScriptsWindow.FavoriteScriptB.Click += delegate
            //                                {
            //                                    if (File.Exists(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json"))
            //                                    {
            //                                        try
            //                                        {
            //                                            File.Delete(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json");
            //                                            RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
            //                                            return;
            //                                        }
            //                                        catch
            //                                        {
            //                                            return;
            //                                        }
            //                                    }
            //                                    jsonstrings jsonstrings = new jsonstrings
            //                                    {
            //                                        Script = new List<jsonstrings.ScriptHub>
            //                                    {
            //                                    new jsonstrings.ScriptHub
            //                                    {
            //                                        Name = ani.CleanString(item.title.ToString()),
            //                                        GameName = ani.CleanString(item.game.title.ToString()),
            //                                        Image = item.image.ToString(),
            //                                        Script = ani.HttpGet(item.rawScript.ToString())
            //                                    }
            //                                    }
            //                                    };
            //                                    try
            //                                    {
            //                                        File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item.title.ToString()) + ".json", JsonConvert.SerializeObject(jsonstrings, (Formatting)1));
            //                                        RScriptsWindow.FavIcon.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFAC37");
            //                                    }
            //                                    catch
            //                                    {
            //                                    }
            //                                };
            //                                RScriptsWrapper.Children.Add(RScriptsWindow);
            //                            });
            //                        }
            //                    }, DispatcherPriority.Background);
            //                }
            //            }
            //            finally
            //            {
            //                ((IDisposable)response)?.Dispose();
            //            }
            //        }
            //        finally
            //        {
            //            ((IDisposable)response)?.Dispose();
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        await Dispatcher.InvokeAsync(() =>
            //        {
            //            var errorPopup = new OptionPopup("Rscripts API Error", $"An error occurred while fetching scripts:\n{ex.Message}", "OK");
            //            errorPopup.Main.Click += delegate
            //            {
            //                ani.Popups.Children.Remove(errorPopup);
            //                ani.Blurrer.Radius = 0;
            //                ani.TabSystemz.Visibility = Visibility.Visible;
            //            };
            //            errorPopup.Secondary.Click += delegate
            //            {
            //                ani.Popups.Children.Remove(errorPopup);
            //                ani.Blurrer.Radius = 0;
            //                ani.TabSystemz.Visibility = Visibility.Visible;
            //            };
            //            ani.Popups.Children.Add(errorPopup);
            //            ani.Blurrer.Radius = 30;
            //            ani.TabSystemz.Visibility = Visibility.Collapsed;

            //            File.WriteAllText(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\errors\{DateTime.Now.ToLongDateString()}.error", ex.ToString());
            //            ani.print(ex.ToString());
            //        });
            //    }
            //    GC.Collect(2, GCCollectionMode.Forced);
            //});
        }

        private void RscriptsSearch()
        {
            CurrentPage = 1;
            RScriptsWrapper.Children.Clear();
            RscriptsScroller.ScrollToTop();
            LoadRscripts();
        }
        // ENDSUB-REGION

        // SUB-REGION Favourited Scripts (yes i spell it like that, im singaporean)
        private async void LoadFavoriteScripts()
        {
            await Task.Run(delegate
            {
                this.Dispatcher.Invoke((Action)(() => this.FavoriteScriptsWrapper.Children.Clear()));
                try
                {
                    foreach (string enumerateFile in Directory.EnumerateFiles(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\", "*.json"))
                        ((IEnumerable<JToken>)JObject.Parse(System.IO.File.ReadAllText(enumerateFile))["Script"]).ToList<JToken>().ForEach((Action<JToken>)(item => this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (!item[(object)"Name"].ToString().ToLower().Contains(this.ScriptHubSearch.Text.ToLower()))
                                return;
                            bool isPatched = false;
                            bool flag = false;
                            if (item.Contains("isPatched"))
                            {
                                isPatched = true;
                            }
                            flag = !((!item.Contains("key")) ? true : false);
                            Favourite_Script_Square FavoritedScript = new Favourite_Script_Square(isPatched, flag);
                            try
                            {
                                FavoritedScript.Image.ImageSource = !item[(object)"Image"].ToString().Contains("rbxcdn.com") ? (ImageSource)new BitmapImage(new Uri("https://scriptblox.com" + item[(object)"Image"].ToString())) : (ImageSource)new BitmapImage(new Uri(item[(object)"Image"].ToString()));
                            }
                            catch
                            {
                            }
                            FavoritedScript.ScriptName.Content = (object)ani.CleanString(item[(object)"Name"].ToString());
                            FavoritedScript.GameName.Content = (object)ani.CleanString(item[(object)"GameName"].ToString());
                            FavoritedScript.ExecuteInteract.Click += (RoutedEventHandler)((s, e) =>
                            {
                                api.RunCode(item[(object)"Script"].ToString(), ani.Popups, ani.Blurrer, ani.TabSystemz);
                            });
                            FavoritedScript.FavoriteScriptB.Click += (RoutedEventHandler)((s, e) =>
                            {
                                (FavoritedScript.Parent as WrapPanel).Children.Remove((UIElement)FavoritedScript);
                                System.IO.File.Delete(@$"C:\Users\{Environment.UserName}\AppData\Local\Nebula Softworks\Nebula Client\Luna X\cache\fs\" + ani.CleanString(item[(object)"Name"].ToString()) + ".json");
                            });
                            FavoritedScript.InfoScriptB.Click += delegate
                            {
                                Clipboard.SetText(item[(object)"Script"].ToString());
                            };
                            this.FavoriteScriptsWrapper.Children.Add((UIElement)FavoritedScript);
                        }))));
                }
                catch
                {
                }
                GC.Collect(2, GCCollectionMode.Forced);
            });
        }
        // ENDSUB-REGION

        private void ScriptHubSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ScriptHubSearch.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                Game_SearchText_Changed = true;
                Game_SearchText = ScriptHubSearch.Text;
                switch (ScriptHubSelect.SelectedIndex)
                {
                    case 0:
                        ScriptBloxSearch();
                        break;
                    case 1:
                        RscriptsSearch();
                        break;
                    case 2:
                        break;
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) =>
            ScriptHubSearch_KeyDown(sender, new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(Application.Current.MainWindow), 0, Key.Return));

        private void ScriptScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer ScriptScroller = null;
            switch (ScriptHubSelect.SelectedIndex)
            {
                case 0:
                    ScriptScroller = ScriptBloxScroller;
                    break;
                case 1:
                    ScriptScroller = RscriptsScroller;
                    break;
                case 2:
                    ScriptScroller = MastersMZScroller;
                    break;
                case 3:
                    ScriptScroller = CommunityScroller;
                    break;
            }
            if (ScriptScroller.VerticalOffset == ScriptScroller.ScrollableHeight && CurrentPage < MaxPage)
            {
                CurrentPage++;
                switch (ScriptHubSelect.SelectedIndex)
                {
                    case 0:
                        LoadScriptBlox();
                        break;
                    case 1:
                        LoadRscripts();
                        break;
                    case 2:
                        break;
                }
            }
        }

        private void CloudScriptToggle(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "CloudScriptButton":
                    CloudScriptButton.Foreground = TryFindResource("Accent") as LinearGradientBrush;
                    FavouriteScriptButton.Foreground = Brushes.White;
                    ani.EnableGridCustom(ScriptScrollers, new Thickness(0, 51, 5, 5));
                    ani.DisableGrid(FavoriteScriptScroller);
                    ScriptHubSelect.IsEnabled = true;
                    break;
                case "FavouriteScriptButton":
                    CloudScriptButton.Foreground = Brushes.White;
                    FavouriteScriptButton.Foreground = TryFindResource("Accent") as LinearGradientBrush;
                    ani.DisableGrid(ScriptScrollers);
                    ani.EnableGridCustom(FavoriteScriptScroller, new Thickness(0, 51, 5, 5));
                    ScriptHubSelect.IsEnabled = false;
                    LoadFavoriteScripts();
                    break;
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Collapsed)
            {
                ScriptBloxWrapper.Children.Clear();
                RScriptsWrapper.Children.Clear();
            }
            else
            {
                ScriptBloxScroller.ScrollToVerticalOffset(ScriptBloxScroller.VerticalOffset + 1);
            }
        }
    }
}

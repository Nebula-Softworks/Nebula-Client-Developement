using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Luna_X.Core
{
    public class monaco_api : WebView2
    {

        public bool isDOMLoaded { get; set; } = false;
        private string LatestRecievedText;
        private string ToSetText;
        private bool toSetbool;
        public bool isMinimapEnabled { get; set; }

        /// <summary>
        /// Event for when the editor is fully loaded.
        /// </summary>
        public event EventHandler EditorReady;

        public monaco_api(string Text, bool x = true)
        {
            this.CoreWebView2InitializationCompleted += monaco_api_CoreWebView2InitializationCompleted;
            ToSetText = Text;
            toSetbool = x;

            shit();
        }

        private async void shit()
        {
            await this.EnsureCoreWebView2Async();
            this.Source = new Uri(string.Format("file:///{0}/bin/Monaco/Luna X Monaco.html", System.Windows.Forms.Application.StartupPath));
            this.DefaultBackgroundColor = System.Drawing.Color.Transparent;
        }

        protected virtual void OnEditorReady()
        {
            EventHandler handler = EditorReady;
            if (handler != null)
                handler(this, new EventArgs());
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                if (mainWindow.Minimap == true) enable_minimap();
                if (mainWindow.autocomplete == false) disable_autocomplete();
                if (mainWindow.Stylua == false) SwitchAutoIndent(false);
                FontSize(mainWindow.fontSize); 
                if (mainWindow.antiSkid == true)
                {
                    Blur(30);
                    this.LostFocus += delegate
                    {
                        Blur(30);
                    };
                    this.GotFocus += delegate
                    {
                        Blur(0);
                    };
                }
                if (mainWindow.ligarutres) SwitchLig(true);
            }
        }

        public void monaco_api_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            this.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            this.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            this.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            this.CoreWebView2.Settings.AreDevToolsEnabled = false;
            this.CoreWebView2.Settings.IsZoomControlEnabled = false;
            this.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
        }

        private void CoreWebView2_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e) => LatestRecievedText = e.TryGetWebMessageAsString();

        private async void CoreWebView2_DOMContentLoaded(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs e)
        {
            await Task.Delay(1000);
            isDOMLoaded = true;
            if (toSetbool) SetText(ToSetText);
            OnEditorReady();
        }
        public async Task<string> GetText()
        {
            if (isDOMLoaded)
            {
                var script = "monaco.editor.getModels()[0].getValue()";
                var result = await CoreWebView2.ExecuteScriptAsync(script);
                var text = JsonConvert.DeserializeObject<string>(result);

                return text;
            }
            return "";
        }

        public async void SetText(string text)
        {
            
                text = text.Replace("\\", "\\\\");
                await CoreWebView2.ExecuteScriptAsync("editor.setValue('');");
                await CoreWebView2.ExecuteScriptAsync($"editor.setValue(`{text.Replace("`", "\\`")}`)");

                /*if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    await CoreWebView2.ExecuteScriptAsync($"editor.setValue('hii');");
                }    */         
            
        }

        public void AddIntellisense(string label, Types type, string description, string insert)
        {
            if (isDOMLoaded)
            {
                if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.Intellisense == true)
                {
                    string selectedType = type.ToString();
                    if (type == Types.None)
                        selectedType = "";
                    this.ExecuteScriptAsync($"AddIntellisense({label}, {selectedType}, {description}, {insert});");
                }
            }
                
        }

        public void Undo()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"Undo();");
        }

        public void Redo()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"Redo();");
        }

        public void refresh()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"Refresh();");
        }

        public void enable_minimap()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync("SwitchMinimap(true);");
            isMinimapEnabled = true;
        }

        public void disable_minimap()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync("SwitchMinimap(false);");
            isMinimapEnabled = false;
        }

        public void enable_autocomplete()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync("SwitchAutoComplete(true);");
        }

        public void disable_autocomplete()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync("SwitchAutoComplete(false);");
        }

        public void SetTheme(string name)
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"SetTheme({name});");
        }

        public void Blur(double number)
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"BlurEditor({number});");
        }

        public void SmoothScroll(bool flag)
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"SetScroll({flag});");
        }

        public void ReadOnly(bool flag)
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"SwitchReadonly({flag});");
        }

        public void FontSize(double number)
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"SwitchFontSize({number});");
        }

        public void SwitchLig(bool flag)
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"SwitchFontLigatures({flag});");
        }

        public void SwitchAutoIndent(bool flag)
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"SwitchAutoIndent({flag});");
        }
    }

    public enum Types
    {
        Class,
        Color,
        Constructor,
        Enum,
        Field,
        File,
        Folder,
        Function,
        Interface,
        Keyword,
        Method,
        Module,
        Property,
        Reference,
        Snippet,
        Text,
        Unit,
        Value,
        Variable,
        None
    }
}
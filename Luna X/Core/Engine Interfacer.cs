using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using Luna_X.Controls.Misc;
using System.Windows.Media.Effects;
using System.Windows;

namespace Luna_X.Core
{
    public static class api
    {
        public static bool isInjectedIntoSelectedProcess()
        {
            return false; 
        }

        public static async void RunCode(string Code, Grid grid, BlurEffect blur, Tab_System TabSystemz)
        {
            async Task SendScript(string script)
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "Nebula Trinity Engine", PipeDirection.Out))
                {
                    try
                    {
                        await pipeClient.ConnectAsync();

                        byte[] scriptBytes = Encoding.UTF8.GetBytes(script);

                        await pipeClient.WriteAsync(scriptBytes, 0, scriptBytes.Length);

                        pipeClient.Close();
                    }
                    catch (Exception ex)
                    {
                        OptionPopup o = new OptionPopup("Couldn't Send Script", $"{ex}", "OK");
                        o.Main.Click += delegate
                        {
                            grid.Children.Remove(o);
                            blur.Radius = 0;
                            var cm = TabSystemz.current_monaco();
                            cm.Visibility = Visibility.Visible;
                            cm.Blur(0);
                        };
                        o.Secondary.Click += delegate
                        {
                            grid.Children.Remove(o);
                            blur.Radius = 0;
                            var cm = TabSystemz.current_monaco();
                            cm.Visibility = Visibility.Visible;
                            cm.Blur(0);
                        };
                        grid.Children.Add(o);
                        blur.Radius = 30;
                        var cm = TabSystemz.current_monaco();
                        cm.Visibility = Visibility.Collapsed;
                        cm.Blur(30);
                    }
                }
            }
            await SendScript(Code);
        }

        public static void Inject(Process process = null)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "Injector.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            });
        }
    }
}

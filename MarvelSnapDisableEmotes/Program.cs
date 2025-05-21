using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using EasyHook;
namespace MarvelSnapDisableEmotes
{
    internal class Inject
    {

        [STAThread]
        public static void Main()
        {
            string path = Directory.GetCurrentDirectory() + "\\Hook\\MarvelSnapDisableEmotesHook.dll";

            while (true)
            {
                try
                {
                    RemoteHooking.Inject(Process.GetProcessesByName("SNAP")[0].Id, InjectionOptions.DoNotRequireStrongName, path, path, Directory.GetCurrentDirectory());
                    Console.WriteLine("Snap process found (You can close this window)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Snap process not found (Is the game running?)");
                    Thread.Sleep(3000);
                    continue;
                }
                break;
            }
        }
    }
}
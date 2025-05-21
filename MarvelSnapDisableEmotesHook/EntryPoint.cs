using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using EasyHook;


namespace MarvelSnapDisableEmotesHook
{
    public class EntryPoint : IEntryPoint
    {
        public IntPtr TargetAddress { get; set; }

        public Process TargetProcess { get; set; }

        public EntryPoint(RemoteHooking.IContext context, string dir)
        {
            TargetProcess = Process.GetProcessesByName("SNAP")[0];
        }

        public void EmoteNotificationHook(IntPtr GameClient, bool local, IntPtr Emote)
        {
            // no function = no emote :3
        }

        public IntPtr GetHookTarget()
        {
            Process process = Process.GetProcessesByName("SNAP")[0];
            SigScanSharp sigScanSharp = new SigScanSharp(process.Handle);
            ProcessModule targetModule = null;

            foreach (ProcessModule module in (ReadOnlyCollectionBase)process.Modules)
            {
                if (module.FileName.Contains("GameAssembly"))
                {
                    targetModule = module;
                    break;
                }
            }

            sigScanSharp.SelectModule(targetModule);
            sigScanSharp.AddPattern("EmoteNotification", "48 89 5C 24 08 48 89 6C 24 18 48 89 74 24 20 57 48 83 EC 40 C7 44 24");
            return new IntPtr((long)sigScanSharp.FindPatterns(out long _)["EmoteNotification"]);
        }

        public void Run(RemoteHooking.IContext context, string dir)
        {
            TargetAddress = GetHookTarget();

            LocalHook localHook = null;

            try
            {
                localHook = LocalHook.Create(TargetAddress, new EmoteNotification_Delegate(EmoteNotificationHook), this);
                localHook.ThreadACL.SetExclusiveACL(new int[1]);

            }
            catch (Exception)
            {
            }

            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                }
            }
            catch
            {
            }

            localHook.Dispose();
            LocalHook.Release();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void EmoteNotification_Delegate(
          IntPtr GameClient,
          bool local, IntPtr Emote);


    }
}
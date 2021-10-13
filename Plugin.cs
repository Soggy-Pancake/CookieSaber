using BS_Utils.Utilities;
using BS_Utils;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Attributes;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

[StructLayout(LayoutKind.Sequential)]
public struct MouseInput
{
    public int dx;
    public int dy;
    public uint mouseData;
    public uint dwFlags;
    public uint time;
    public IntPtr dwExtraInfo;
}

[Flags]
public enum InputType
{
    Mouse = 0,
    Keyboard = 1,
    Hardware = 2
}

[Flags]
public enum MouseEventF
{
    Absolute = 0x8000,
    HWheel = 0x01000,
    Move = 0x0001,
    MoveNoCoalesce = 0x2000,
    LeftDown = 0x02,
    LeftUp = 0x04,
    RightDown = 0x0008,
    RightUp = 0x0010,
    MiddleDown = 0x0020,
    MiddleUp = 0x0040,
    VirtualDesk = 0x4000,
    Wheel = 0x0800,
    XDown = 0x0080,
    XUp = 0x0100
}


namespace CookieSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("User32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("User32.dll")]
        private static extern IntPtr GetMessageExtraInfo();
        [DllImport("User32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public static bool ModEnabled;
        public static int ClickMethod = 0;
        public static int MouseX = 0;
        public static int MouseY = 0;

        [Init]
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            BSEvents.noteWasCut += NoteCut; //Subscribe to notecut event
            BS_Utils.Utilities.Config ConfigVariable = new BS_Utils.Utilities.Config("CookieSaber");
            ModEnabled = ConfigVariable.GetBool("CookieSaber", "Enabled", true, true);
            ClickMethod = ConfigVariable.GetInt("CookieSaber", "Method", 1, true);
            MouseX = ConfigVariable.GetInt("CookieSaber", "MouseX", 20, true);
            MouseY = ConfigVariable.GetInt("CookieSaber", "MouseY", 10, true);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Info("Ready to click some cookies!");
            GameplaySetup.instance.AddTab("CookieSaber", "CookieSaber.UI.SettingsTab.bsml", CookieSaberUI.CookieSaberUI.instance);
        }

        public void NoteCut(NoteData notedata, NoteCutInfo cutinfo, int multiplier)
        {
            Log.Info($"ModEnabled value: {ModEnabled}");
            Log.Info($"Click Method value: {ClickMethod}");
            if (ModEnabled && cutinfo.allIsOK)
            {
                if (ClickMethod == 1)
                {
                    SetCursorPos(MouseX, MouseY);
                }
                mouse_event(0x02, 0, 0, 0, 0); //Mouse Button Down
                mouse_event(0x04, 0, 0, 0, 0); //Mouse Button Up
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            BS_Utils.Utilities.Config ConfigVariable = new BS_Utils.Utilities.Config("CookieSaber");
            ConfigVariable.SetBool("CookieSaber", "Enabled",  ModEnabled);
            ConfigVariable.SetInt("CookieSaber", "Method", ClickMethod);
            ConfigVariable.SetInt("CookieSaber", "MouseX", MouseX);
            ConfigVariable.SetInt("CookieSaber", "MouseY", MouseY);
        }
    }
}
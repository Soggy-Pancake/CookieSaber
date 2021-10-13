using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using System.Collections.Generic;
using System;

namespace CookieSaberUI
{
    class CookieSaberUI : NotifiableSingleton<CookieSaberUI>
    {
        //NotifiableSingleton allows bind-value to be used
        //PersistentSingleton doesn't

        //massive thanks to Exomanz for helping with the view controller
        [UIValue("Enabled")]
        private bool Enabled
        {
            get => CookieSaber.Plugin.ModEnabled;
            set => CookieSaber.Plugin.ModEnabled = value;
        }

        [UIValue("clickMethods")] //has to be list<object> for some reason
        List<object> clickMethods = new List<object>() { "Current Position", "Absolute Position" };

        [UIValue("clickMethod")]
        private string clickMethod
        {
            get => (string)clickMethods[CookieSaber.Plugin.ClickMethod];
            set => CookieSaber.Plugin.ClickMethod = clickMethods.IndexOf(value); //Current pos = 0, Absolute pos = 1, Rel pos = 2 (not doing relative for now)
        }

        [UIValue("MouseX")]
        private int MouseX
        {
            get => CookieSaber.Plugin.MouseX;
            set { 
                CookieSaber.Plugin.MouseX = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("MouseY")]
        private int MouseY
        {
            get => CookieSaber.Plugin.MouseY;
            set { 
                CookieSaber.Plugin.MouseY = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("getCursor")]
        void getCursor() 
        { 
            CookieSaber.Plugin.GetCursorPos(out CookieSaber.Plugin.POINT mouse);
            MouseX = mouse.X;
            CookieSaber.Plugin.MouseX = mouse.X;
            MouseY = mouse.Y;
            CookieSaber.Plugin.MouseY = mouse.Y;
            NotifyPropertyChanged();
        }
    }
}

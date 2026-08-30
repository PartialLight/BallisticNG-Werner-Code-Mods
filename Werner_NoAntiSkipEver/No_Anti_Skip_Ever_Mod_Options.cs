using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using BallisticModding;
using BallisticUnityTools.Placeholders;
using BallisticUnityTools;
using BallisticNG;
using UnityEngine;
using UnityEngine.UI;
using NgUi.RaceUi;
using NgUi.MenuUi;
using NgContent;
using ModOptions = NgUi.Options.ModOptions;
using NgEvents;
using NgData;
using NgGame;
using NgLib;
using NgMusic;
using NgMp;
using NgShips;
using NgModding.Huds;
using NgModding;
using NgPickups;

namespace NoAntiSkipEverHUDOptions
{

    public class ModMenuOptions : CodeMod
    {
        private string _configPath;

        private static readonly KeyCode[] AllKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        private static readonly string[] AllKeyCodeNames = Array.ConvertAll(AllKeyCodes, kc => kc.ToString());

        public static int DisableRecoveryToggle;
        public static float SelfDestructTimer;
        public static KeyCode SelfDestructKey;

        public override void OnRegistered(string ModLocation)
        {
            _configPath = Path.Combine(ModLocation, "config.ini");

            RegisterSettings();

            NgSystemEvents.OnConfigRead += OnConfigRead;
            NgSystemEvents.OnConfigWrite += OnConfigWrite;
        }

        private void RegisterSettings()
        {
            string ModID = "No Anti-Skip Ever";

            string SelectorCategory0 = "Recovery Settings (WARNING: EXPERIMENTAL)";

            ModOptions.RegisterOption<NgBoxSelector>(false, ModID, SelectorCategory0, "SelfDestructKey_ID",
                selector =>
                {
                    selector.Configure("Self-Destruct Keybind", "Custom binding for self-destructing and forcing a respawn when you get stuck or fall out of bounds. Set to 'None' to leave this unbound/disabled.",
                        SelfDestructKey);
                    selector.SetOptions(Array.IndexOf(AllKeyCodes, SelfDestructKey), AllKeyCodeNames);
                }, selector =>
                {
                    SelfDestructKey = AllKeyCodes[selector.Value];
                });

            ModOptions.RegisterOption<NgBoxSelector>(false, ModID, SelectorCategory0, "DisableRecoveryToggle_ID",
                selector =>
                {
                    selector.Configure("Recovery Toggle", "Whether to enable or disable out-of-track-bounds recovery. Only for the most extreme skippers.\n\n" +
                        "NOTE: This also disables hoverpoint correction, making landing back onto the track from out of bounds significantly harder.\n\n" +
                        "WARNING: If you mess up a skip, the recovery drone will be unable to save you. Hold the Self-Destruct key to respawn on the track surface.",
                        DisableRecoveryToggle, null, "Recovery Enabled", "Recovery Disabled");
                },
                selector =>
                {
                    DisableRecoveryToggle = selector.Value;
                });            

            ModOptions.RegisterOption<NgBoxSlider>(false, ModID, SelectorCategory0, "SelfDestructTimer_ID",
                slider =>
                {
                    slider.Configure("Self Destruct Timer", "How many consecutive seconds you have to hold the self-destruct button down for in order to trigger a self-destruct. Setting this to 0 will allow you to self-destruct immediately when you press the self-destruct binding.\n\nNOTE: The self-destruct binding is the same as \"Recenter VR\". By default, \"Recenter VR\" is bound to 'R' on keyboards and the right stick button on gamepads. You may want to rebind this to your preference",
                        " Seconds", SelfDestructTimer, 0.00f, 3.00f, 0.01f);
                }, slider =>
                {
                    SelfDestructTimer = slider.Value;
                });

        }

        private void OnConfigRead()
        {
            INIParser ini = new INIParser();

            ini.Open(_configPath);

            SelfDestructKey = (KeyCode)ini.ReadValue("Settings", "SelfDestructKey_ID", (int)SelfDestructKey);
            DisableRecoveryToggle = ini.ReadValue("Settings", "DisableRecoveryToggle_ID", DisableRecoveryToggle);            
            SelfDestructTimer = (float)ini.ReadValue("Settings", "SelfDestructTimer_ID", SelfDestructTimer);

            ini.Close();
        }

        private void OnConfigWrite()
        {
            INIParser ini = new INIParser();

            ini.Open(_configPath);

            ini.WriteValue("Settings", "SelfDestructKey_ID", (int)SelfDestructKey);
            ini.WriteValue("Settings", "DisableRecoveryToggle_ID", DisableRecoveryToggle);            
            ini.WriteValue("Settings", "SelfDestructTimer_ID", SelfDestructTimer);

            ini.Close();
        }
    }
}
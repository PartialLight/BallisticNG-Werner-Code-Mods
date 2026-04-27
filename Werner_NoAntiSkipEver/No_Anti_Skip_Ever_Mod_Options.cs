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

        public static int DisableRecoveryToggle;
        public static int SelfDestructHeight;

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

            ModOptions.RegisterOption<NgBoxSelector>(false, ModID, SelectorCategory0, "DisableRecoveryToggle_ID",
                selector =>
                {
                    selector.Configure("Recovery Toggle", "Whether to enable or disable out-of-track-bounds recovery. Only for the most extreme skippers.\n\n" +
                        "NOTE: This also disables hoverpoint correction, making landing back onto the track from out of bounds significantly harder.\n\n" +
                        "WARNING: If you mess up a skip, the recovery drone will be unable to save you. PRESS OR HOLD WHATEVER KEY/BUTTON YOU HAVE BOUND TO \"Recenter VR\" TO SELF-DESTRUCT.",
                        DisableRecoveryToggle, null, "Recovery Enabled", "Recovery Disabled");
                },
                selector =>
                {
                    DisableRecoveryToggle = selector.Value;
                });

            ModOptions.RegisterOption<NgBoxSlider>(false, ModID, SelectorCategory0, "SelfDestructHeight_ID",
                slider =>
                {
                    slider.Configure("Self-Destruct Minimum Height", "How many units below the track your ship should be before song change will trigger a self-destruct.\n\n" +
                        "Recommended: >=10",
                        " Units", SelfDestructHeight, 0, 100, 1);
                }, slider =>
                {
                    SelfDestructHeight = (int) slider.Value;
                });

        }

        private void OnConfigRead()
        {
            INIParser ini = new INIParser();

            ini.Open(_configPath);

            DisableRecoveryToggle = ini.ReadValue("Settings", "DisableRecoveryToggle_ID", DisableRecoveryToggle);
            SelfDestructHeight = ini.ReadValue("Settings", "SelfDestructHeight_ID", SelfDestructHeight);

            ini.Close();
        }

        private void OnConfigWrite()
        {
            INIParser ini = new INIParser();

            ini.Open(_configPath);

            ini.WriteValue("Settings", "DisableRecoveryToggle_ID", DisableRecoveryToggle);
            ini.WriteValue("Settings", "SelfDestructHeight_ID", SelfDestructHeight);

            ini.Close();
        }
    }
}
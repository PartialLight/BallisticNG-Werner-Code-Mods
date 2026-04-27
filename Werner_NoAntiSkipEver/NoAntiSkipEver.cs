using NgGame;
using NgModding;
using NgShips;
using UnityEngine;

namespace NoAntiSkipEver //Original mod by Werner, Lord of the Skips
{
    public class NoAntiSkipEver : CodeMod
    {
        public NgShips.ShipController Current_Ship;

        public override void OnRegistered(string modPath)
        {
            NgEvents.NgRaceEvents.OnShipSpawned += DisableAntiSkip;
            NgEvents.NgRaceEvents.OnShipSpawned += DisableRecovery;
            NgEvents.NgRaceEvents.OnCountdownStart += SetupSelfDestructHook;

            DebugConsole.Log("[NoAntiSkipEver] i hate the antiskip!");
            DebugConsole.Log("[NoAntiSkipEver] i hate the antiskip!");
        }

        private void SetupSelfDestructHook()
        {
            GameObject NoAntiSkipEverMonoBehaviourHookObject = new GameObject("GlobalManager");
            NoAntiSkipEverMonoBehaviourHookObject.AddComponent<SelfDestructMonoBehaviour>();
        }

        private void DisableRecovery(ShipController ship)
        {
            //List Sections = new List<NgTrackData.Section> /*{ NgTrackData.TrackManager.Instance.data.sections }*/;
            //Sections = NgTrackData.TrackManager.Instance.data.sections;

            Current_Ship = ship;

            if (NoAntiSkipEverHUDOptions.ModMenuOptions.DisableRecoveryToggle == 1)
            {
                foreach (NgTrackData.Section section in NgTrackData.TrackManager.Instance.data.sections)
                {
                    section.AllowOutOfBounds = true;
                }
                //DebugConsole.Log("RECOVERY DISABLED");
            }            
        }

        private void DisableAntiSkip(ShipController ship)
        {
            // Pretend the ship entered a NoAntiSkip trigger.
            // However, it'll get turned back off if the ship
            // leaves a real NoAntiSkip trigger...
            ship.InNoAntiSkipTrigger = true;

            // Disable the antiskip through the race manager,
            // if there is an instance of such (should be the case
            // during races, right?)
            if ((bool)RaceManager.Instance)
            {
                RaceManager.Instance.AntiskipDisabled = true;
            }
            else
            {
                DebugConsole.Log("[NoAntiSkipEver] Attempted to disable anti skip through the RaceManager, but there is no instance of RaceManager");
            }
        }
    }

    public class SelfDestructMonoBehaviour : MonoBehaviour
    {
        void Update()
        {
            ResetShip();
        }

        void ResetShip()
        {
            if ((NgData.Ships.Loaded[NgMp.NgPeer.MySpawnIndex].CurrentSection.InverseTransformPoint(NgData.Ships.Loaded[NgMp.NgPeer.MySpawnIndex].T.position).y <= -NoAntiSkipEverHUDOptions.ModMenuOptions.SelfDestructHeight) && (NgIo.NgIn.GetButton("Recenter VR", 0) || NgIo.NgIn.GetButtonDown("Recenter VR", 0)))
            {
                NgData.Ships.Loaded[NgMp.NgPeer.MySpawnIndex].ShieldIntegrity = -1;
                //DebugConsole.Log("HEIGHT: " + Current_Ship.CurrentSection.InverseTransformPoint(Current_Ship.T.position).y.ToString());
                //DebugConsole.Log("LIMIT: -" + NoAntiSkipEverHUDOptions.ModMenuOptions.SelfDestructHeight.ToString());
            }
        }
    }
}

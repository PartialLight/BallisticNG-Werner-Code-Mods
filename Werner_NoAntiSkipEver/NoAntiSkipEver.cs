using NgGame;
using NgModding;
using NgShips;
using UnityEngine;

namespace NoAntiSkipEver //Original mod by Werner, Lord of the Skips
{
    public class NoAntiSkipEver : CodeMod
    {
        public override void OnRegistered(string modPath)
        {
            NgEvents.NgRaceEvents.OnShipSpawned += DisableAntiSkip;
            DebugConsole.Log("[NoAntiSkipEver] i hate the antiskip!");
            DebugConsole.Log("[NoAntiSkipEver] i hate the antiskip!");
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
}

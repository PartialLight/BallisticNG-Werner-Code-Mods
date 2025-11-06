using NgModding;
using UnityEngine;
using NgPickups;
using NgShips;
//using BallisticUnityTools.TrackTools;

namespace NoMines //Original mod by Werner, Lord of the Skips
{
    public class NoMines : CodeMod
    {
        public override void OnRegistered(string modPath)
        {
            NgEvents.NgRaceEvents.OnShipSpawned += BlockMinesFromWeightingTable;
        }

        private void BlockMinesFromWeightingTable(ShipController ship)
        {
            Pickup.PickupWeightingTable.RemoveAll(IsMines);
        }

        private bool IsMines(Pickup pickup)
        {
            return pickup.Name == "mines";
        }
    }
}

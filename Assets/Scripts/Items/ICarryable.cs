using UnityEngine;

namespace Items
{
    [AddComponentMenu("Scripts/Items/Items.ICarryable")]
    internal interface ICarryable
    {
        public void PickUp(Photon.Realtime.Player player);
        public void Drop();
    }
}

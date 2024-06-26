using UnityEngine;

namespace Items
{
    [AddComponentMenu("Scripts/Items/Items.IInteractable")]
    internal interface IInteractable
    {
        public void Interaction();
    }
}

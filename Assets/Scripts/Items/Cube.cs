using Photon.Pun;
using System;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(PhotonView), typeof(BoxCollider))]
    [AddComponentMenu("Scripts/Items/Items.Box")]
    internal class Cube : MonoBehaviour, ICarryable
    {
        internal enum CubeColor
        {
            White,
            Black
        }

        public CubeColor Color = CubeColor.White;

        public Action PickedUp;

        private new BoxCollider collider;
        private PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            collider = GetComponent<BoxCollider>();
        }

        public void PickUp(Photon.Realtime.Player player)
        {
            photonView.TransferOwnership(player);
            photonView.RPC("SetIsTrigger", RpcTarget.All);
        }

        [PunRPC]
        private void SetIsTrigger()
        {
            collider.isTrigger = true;
            PickedUp?.Invoke();
        }

        public void Drop()
        {
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
            photonView.RPC("UnsetIsTrigger", RpcTarget.All);
        }

        [PunRPC]
        private void UnsetIsTrigger()
        {
            collider.isTrigger = false;
        }
    }
}

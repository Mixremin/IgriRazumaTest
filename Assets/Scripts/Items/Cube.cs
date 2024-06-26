using Photon.Pun;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(PhotonView), typeof(BoxCollider))]
    [AddComponentMenu("Scripts/Items/Items.Box")]
    internal class Cube : MonoBehaviour, ICarryable
    {
        internal enum CubeColor
        {
            white,
            black
        }

        public CubeColor Color = CubeColor.white;

        private new BoxCollider collider;
        private PhotonView photonView;

        private void Start()
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

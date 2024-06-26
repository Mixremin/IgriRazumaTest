using Photon.Pun;
using UnityEngine;

namespace Items
{
    [AddComponentMenu("Scripts/Items/Items.Box")]
    internal class Box : MonoBehaviour, ICarryable
    {
        internal enum BoxColor
        {
            white,
            black
        }

        public BoxColor color = BoxColor.white;

        [SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private Rigidbody rb;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            rb = GetComponent<Rigidbody>();
        }

        public void PickUp(Photon.Realtime.Player player)
        {
            photonView.TransferOwnership(player);
            photonView.RPC("SetKinematic", RpcTarget.All);
        }

        [PunRPC]
        private void SetKinematic()
        {
            rb.isKinematic = true;
        }

        public void Drop()
        {
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
            photonView.RPC("UnsetKinematic", RpcTarget.All);
        }

        [PunRPC]
        private void UnsetKinematic()
        {
            rb.isKinematic = false;
        }
    }
}

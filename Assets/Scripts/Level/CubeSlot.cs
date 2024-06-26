using Items;
using Photon.Pun;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(PhotonView))]
    [AddComponentMenu("Scripts/Level/Level.CubeSlot")]
    internal class CubeSlot : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Transform cubePoint;

        private GameObject cube;
        private PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void Interaction(int cubeID)
        {
            photonView.RPC("SetCubeInSlotRPC", RpcTarget.All, cubeID);

        }

        [PunRPC]
        private void SetCubeInSlotRPC(int cubeID)
        {
            cube = PhotonView.Find(cubeID).gameObject;
            cube.transform.parent = cubePoint.transform;
            cube.transform.position = cubePoint.position;
            cube.transform.rotation = Quaternion.Euler(Vector3.zero);
            cube.transform.localScale = Vector3.one;
        }
    }
}

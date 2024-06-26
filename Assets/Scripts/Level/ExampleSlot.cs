using Photon.Pun;
using UnityEngine;

namespace Level
{
    [AddComponentMenu("Scripts/Level/Level.ExampleSlot")]
    internal class ExampleSlot : MonoBehaviour
    {

        public SlotState SlState
        {
            get => slState;
            set
            {
                slState = value;
                Debug.Log("New slot state:" + slState);
            }
        }

        private SlotState slState = SlotState.Empty;

        [SerializeField]
        private GameObject blackCube;

        [SerializeField]
        private GameObject whiteCube;

        [SerializeField]
        private Transform cubeSlot;

        private GameObject cubeInSlot;
        private PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void SetWhiteCube()
        {
            photonView.RPC("SetWhiteCubeRPC", RpcTarget.All);
        }

        [PunRPC]
        private void SetWhiteCubeRPC()
        {
            if (cubeInSlot != null)
            {
                PhotonNetwork.Destroy(cubeInSlot);
            }
            _ = PhotonNetwork.Instantiate(whiteCube.name, cubeSlot.position, Quaternion.identity);
            slState = SlotState.White;
        }

        public void SetBlackCube()
        {
            photonView.RPC("SetBlackCubeRPC", RpcTarget.All);
        }

        [PunRPC]
        private void SetBlackCubeRPC()
        {
            if (cubeInSlot != null)
            {
                PhotonNetwork.Destroy(cubeInSlot);
            }
            _ = PhotonNetwork.Instantiate(blackCube.name, cubeSlot.position, Quaternion.identity);
            slState = SlotState.Black;
        }
    }
}

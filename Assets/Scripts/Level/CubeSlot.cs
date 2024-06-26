using Items;
using Photon.Pun;
using UnityEngine;

namespace Level
{

    internal enum SlotState
    {
        Empty,
        White,
        Black
    }

    [RequireComponent(typeof(PhotonView))]
    [AddComponentMenu("Scripts/Level/Level.CubeSlot")]
    internal class CubeSlot : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Transform cubePoint;

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

        private GameObject cube;
        private Cube cubeComponent;

        private PhotonView photonView;

        private void Awake()
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
            cubeComponent = cube.GetComponent<Cube>();

            if (cubeComponent.Color == Cube.CubeColor.Black)
            {
                SlState = SlotState.Black;
            }
            else
            {
                SlState = SlotState.White;
            }

            cube.transform.parent = cubePoint.transform;
            cube.transform.position = cubePoint.position;
            cube.transform.rotation = Quaternion.Euler(Vector3.zero);
            cube.transform.localScale = Vector3.one;

            cubeComponent.PickedUp += ResetSlot;
        }

        private void ResetSlot()
        {
            photonView.RPC("ResetSlotRPC", RpcTarget.All);
        }

        [PunRPC]
        private void ResetSlotRPC()
        {
            if (cubeComponent != null)
            {
                cubeComponent.PickedUp -= ResetSlot;
                SlState = SlotState.Empty;
                cube = null;
                cubeComponent = null;
            }
        }
    }
}

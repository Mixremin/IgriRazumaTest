using Items;
using Photon.Pun;
using System;
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
        [Header("Cube Placement")]
        [SerializeField]
        private Transform cubePoint;


        public SlotState SlState
        {
            get => slState;
            set
            {
                slState = value;
                SlotStateChanged?.Invoke();
            }
        }

        private SlotState slState = SlotState.Empty;

        private GameObject cube;
        private Cube cubeComponent;

        private PhotonView photonView;

        public static Action SlotStateChanged;

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

            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                cubeComponent.PickedUp += ResetSlot;
            }
        }

        private void ResetSlot()
        {
            photonView.RPC("ResetSlotRPC", RpcTarget.All);
        }

        [PunRPC]
        private void ResetSlotRPC()
        {
            cubeComponent.PickedUp -= ResetSlot;
            SlState = SlotState.Empty;
            cube = null;
            cubeComponent = null;
        }
    }
}

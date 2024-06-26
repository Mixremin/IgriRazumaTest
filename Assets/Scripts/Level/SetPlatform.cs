using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [AddComponentMenu("Scripts/Level/Level.SetPlatform")]
    internal class SetPlatform : MonoBehaviour
    {
        [SerializeField]
        private List<CubeSlot> cubeSlots;

        [SerializeField]
        private PuzzleLogicManager puzzleLogicManager;

        private int[,] slots = new int[3, 3];
        private PhotonView photonView;


        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                photonView.RPC("InitializeSlotsRPC", RpcTarget.All);
                photonView.RPC("RenewSlotsInfoRPC", RpcTarget.All);
            }
            CubeSlot.SlotStateChanged += RenewSlotsInfo;
        }

        [PunRPC]
        private void InitializeSlotsRPC()
        {
            foreach (Transform child in transform)
            {
                cubeSlots.Add(child.GetComponent<CubeSlot>());
            }


        }

        private void RenewSlotsInfo()
        {
            Debug.Log("Renewing slots");
            photonView.RPC("RenewSlotsInfoRPC", RpcTarget.All);
        }

        [PunRPC]
        private void RenewSlotsInfoRPC()
        {
            int i = 0;
            int j = 0;
            foreach (CubeSlot slot in cubeSlots)
            {
                switch (slot.SlState)
                {
                    case SlotState.Empty:
                        slots[i, j] = 0;
                        break;
                    case SlotState.White:
                        slots[i, j] = 1;
                        break;
                    case SlotState.Black:
                        slots[i, j] = 2;
                        break;
                }

                if (j < 2)
                {
                    j++;
                }
                else if (i < 2)
                {
                    j = 0;
                    i++;
                }
            }

            puzzleLogicManager.CompareSlots();
        }

        public int[,] GetSlots()
        {
            return slots;
        }

        private void OnDisable()
        {
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                CubeSlot.SlotStateChanged -= RenewSlotsInfo;
            }
        }
    }
}

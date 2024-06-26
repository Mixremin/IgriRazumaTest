﻿using Photon.Pun;
using UnityEngine;

namespace Level
{
    [AddComponentMenu("Scripts/Level/Level.PuzzleLogicManager")]
    internal class PuzzleLogicManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField]
        private ComparissonPlatform compPlatformManager;

        [SerializeField]
        private SetPlatform setPlatformManager;

        [Header("Photon(For debug)")]
        [SerializeField]
        private PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void CompareSlots()
        {
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                photonView.RPC("CompareSlotsRPC", RpcTarget.All);
            }

        }

        [PunRPC]
        private void CompareSlotsRPC()
        {
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                int[,] setSlots = setPlatformManager.GetSlots();
                int[,] compsSlot = compPlatformManager.GetSlots();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (setSlots[i, j] != compsSlot[i, j])
                        {
                            return;
                        }
                    }
                }
                compPlatformManager.ResetPlatform(); //On win change pattern
            }
        }
    }
}

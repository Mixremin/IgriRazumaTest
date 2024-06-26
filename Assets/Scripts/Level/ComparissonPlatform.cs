
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [AddComponentMenu("Scripts/Level/Level.ComparissonPlatform")]
    internal class ComparissonPlatform : MonoBehaviour
    {
        [SerializeField]
        private List<ExampleSlot> exampleSlots;

        private int[,] slots = new int[3, 3];
        private PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                photonView.RPC("InitializeCubes", RpcTarget.All);
            }

        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                photonView.RPC("FillCubes", RpcTarget.All);
            }
        }

        [PunRPC]
        private void InitializeCubes()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    slots[i, j] = Random.Range(1, 3); // 1 - White cube, 2 - Black cube

                }
            }

            foreach (Transform child in transform)
            {
                exampleSlots.Add(child.GetComponent<ExampleSlot>());
            }
        }

        [PunRPC]
        private void FillCubes()
        {
            int i = 0;
            int j = 0;
            foreach (ExampleSlot slot in exampleSlots)
            {
                switch (slots[i, j])
                {
                    case 1:
                        slot.SetWhiteCube();
                        break;
                    case 2:
                        slot.SetBlackCube();
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
        }

        private void ResetPlatform()
        {

        }

        public int[,] GetSlots()
        {
            return slots;
        }
    }
}

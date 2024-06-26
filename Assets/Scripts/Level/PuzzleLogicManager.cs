using Photon.Pun;
using UnityEngine;

namespace Level
{
    [AddComponentMenu("Scripts/Level/Level.PuzzleLogicManager")]
    internal class PuzzleLogicManager : MonoBehaviour
    {
        [SerializeField]
        private ComparissonPlatform compPlatformManager;

        [SerializeField]
        private SetPlatform setPlatformManager;


        [SerializeField]
        private PhotonView photonView;

        private void Awake()
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

            int[,] setSlots = setPlatformManager.GetSlots();
            int[,] compsSlot = compPlatformManager.GetSlots();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (setSlots[i, j] != compsSlot[i, j])
                    {
                        Debug.Log("Incorrect puzzle");
                        return;
                    }
                }
            }
            Debug.Log("Puzzle Solved");
        }
    }
}

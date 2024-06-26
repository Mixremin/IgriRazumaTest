using Photon.Pun;
using Player;
using UnityEngine;

namespace Server
{
    internal class PlayerConnectionManager : MonoBehaviourPunCallbacks
    {
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log("Player connected");
            FindObjectOfType<FirstPersonMovement>().GetComponent<PhotonView>().TransferOwnership(newPlayer);
            FindObjectOfType<FirstPersonLook>().GetComponent<PhotonView>().TransferOwnership(newPlayer);

        }
    }
}

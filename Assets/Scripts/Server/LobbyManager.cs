using Photon.Pun;
using UnityEngine;
namespace server
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private string standartRoomName = "room_sample";

        public void HostLobby()
        {
            _ = PhotonNetwork.CreateRoom(standartRoomName);
        }

        public void JoinLobby()
        {
            _ = PhotonNetwork.JoinRoom(standartRoomName);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Level");
        }
    }
}

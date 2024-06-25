using Photon.Pun;
using UnityEngine.SceneManagement;

namespace server
{
    internal class ConnectOnStart : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            _ = PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            _ = PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}

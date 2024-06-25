using server;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyUI : MonoBehaviour
    {

        [SerializeField]
        private Button hostButton;

        [SerializeField]
        private Button joinButton;

        [SerializeField]
        private LobbyManager lobbyManager;

        private void Awake()
        {
            hostButton.onClick.AddListener(AskLobbyHost);
            joinButton.onClick.AddListener(AskLobbyJoin);
        }

        private void AskLobbyHost()
        {
            lobbyManager.HostLobby();
        }

        private void AskLobbyJoin()
        {
            lobbyManager.JoinLobby();
        }
    }
}


using Photon.Pun;
using UnityEngine;

namespace Level
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private Transform playerParent;

        [SerializeField]
        private Transform spawnPoint;

        private GameObject playerObject;

        private void Start()
        {
            if (playerObject == null)
            {
                playerObject = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
                playerObject.transform.parent = playerParent;
            }
        }
    }
}

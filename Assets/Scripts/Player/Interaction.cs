using Items;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    internal enum InteractionState
    {
        standart,
        carrying
    }
    internal class Interaction : MonoBehaviour
    {
        [SerializeField]
        private KeyCode interactionKey = KeyCode.E;

        [SerializeField]
        private float interactionRange = 10f;

        [SerializeField]
        private Transform carryingPoint;

        [SerializeField]
        private Camera playerCam;


        public InteractionState InteractionState = InteractionState.standart;

        private PhotonView photonView;
        private Vector3 rayOrigin;
        private GameObject carryingItem;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                return;
            }
            else if (photonView.IsMine)
            {
                rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                if (Input.GetKeyUp(interactionKey))
                {
                    switch (InteractionState)
                    {
                        case InteractionState.standart:
                            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out RaycastHit hit, interactionRange))
                            {
                                if (hit.transform.TryGetComponent(out ICarryable carryable))
                                {
                                    int objectID = hit.transform.GetComponent<PhotonView>().ViewID;
                                    carryable.PickUp(photonView.Owner);
                                    photonView.RPC("PickUpItem", RpcTarget.All, objectID);
                                }
                            }
                            break;
                        case InteractionState.carrying:

                            photonView.RPC("DropItem", RpcTarget.All);
                            break;
                    }
                }
            }
        }

        [PunRPC]
        private void PickUpItem(int objectID)
        {
            InteractionState = InteractionState.carrying;
            StandartInteraction(PhotonView.Find(objectID).gameObject);
        }

        private void StandartInteraction(GameObject item)
        {
            carryingItem = item;
            item.transform.parent = carryingPoint.transform;
            item.transform.rotation = Quaternion.Euler(Vector3.zero);
            item.transform.localPosition = Vector3.zero;
        }

        [PunRPC]
        private void DropItem()
        {
            InteractionState = InteractionState.standart;
            CarryingInteraction();
        }

        private void CarryingInteraction()
        {
            carryingItem.GetComponent<Box>().Drop();
            carryingItem.transform.parent = null;
            carryingItem = null;
        }
    }
}

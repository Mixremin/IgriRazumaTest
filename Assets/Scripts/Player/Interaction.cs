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


        private InteractionState interactionState = InteractionState.standart;
        private PhotonView photonView;
        private Vector3 rayOrigin;
        private GameObject carryingItem;
        private int carryingObjectID;

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
                    switch (interactionState)
                    {
                        case InteractionState.standart:
                            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out RaycastHit hit, interactionRange))
                            {
                                if (hit.transform.TryGetComponent(out ICarryable carryable))
                                {
                                    carryingObjectID = hit.transform.GetComponent<PhotonView>().ViewID;
                                    carryable.PickUp(photonView.Owner);
                                    photonView.RPC("PickUpItem", RpcTarget.All, carryingObjectID);
                                }
                            }
                            break;
                        case InteractionState.carrying:
                            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, interactionRange))
                            {
                                if (hit.transform.TryGetComponent(out IInteractable interactable))
                                {
                                    carryingItem.GetComponent<Cube>().Drop();
                                    interactable.Interaction(carryingObjectID);
                                    photonView.RPC("UncarryItem", RpcTarget.All);
                                }
                            }
                            photonView.RPC("DropItem", RpcTarget.All);
                            break;
                    }
                }
            }
        }

        [PunRPC]
        private void PickUpItem(int objectID)
        {
            interactionState = InteractionState.carrying;
            PickingUp(PhotonView.Find(objectID).gameObject);
        }

        private void PickingUp(GameObject item)
        {
            carryingItem = item;
            item.transform.parent = carryingPoint.transform;
            item.transform.localRotation = Quaternion.Euler(Vector3.zero);
            item.transform.localPosition = Vector3.zero;
        }

        [PunRPC]
        private void DropItem()
        {
            interactionState = InteractionState.standart;
            if (carryingItem != null)
            {
                Dropping();
            }

        }

        private void Dropping()
        {
            carryingItem.GetComponent<Cube>().Drop();
            carryingItem.transform.parent = null;
            carryingItem = null;
            carryingObjectID = -1;
        }

        [PunRPC]
        private void UncarryItem()
        {
            carryingItem = null;
            carryingObjectID = -1;
        }
    }
}

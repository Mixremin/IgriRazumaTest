using Photon.Pun;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PhotonView))]
    internal class FirstPersonLook : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Transform character;

        [SerializeField]
        private float sensitivity = 2;

        [SerializeField]
        private float smoothing = 1.5f;

        [SerializeField]
        private PhotonView photonView;

        private Vector2 velocity;
        private Vector2 frameVelocity;


        private void Reset()
        {
            character = GetComponentInParent<FirstPersonMovement>().transform;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                return;
            }
            else if (photonView.IsMine)
            {
                Vector2 mouseDelta = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
                frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
                velocity += frameVelocity;
                velocity.y = Mathf.Clamp(velocity.y, -90, 90);

                transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
                character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
            }
        }
    }
}

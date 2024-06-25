using Photon.Pun;
using UnityEngine;


namespace Player
{
    internal class FirstPersonMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5;

        [Header("Running")]
        [SerializeField]
        private bool canRun = true;

        [SerializeField]
        private float runSpeed = 9;

        [SerializeField]
        private KeyCode runningKey = KeyCode.LeftShift;

        private new Rigidbody rigidbody;
        private PhotonView photonView;
        public bool IsRunning { get; private set; }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            photonView = GetComponent<PhotonView>();
        }

        private void FixedUpdate()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                return;
            }
            else if (photonView.IsMine)
            {
                IsRunning = canRun && Input.GetKey(runningKey);

                float targetMovingSpeed = IsRunning ? runSpeed : speed;

                Vector2 targetVelocity = new(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
                rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
            }
        }
    }
}
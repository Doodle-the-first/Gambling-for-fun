using UnityEngine;

// Very small player controller for prototype testing

namespace VoxelEngine {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {
        public float speed = 5f;
        public float gravity = -9.81f;
        CharacterController cc;
        float verticalVel = 0f;

        void Start(){ cc = GetComponent<CharacterController>(); }
        void Update(){
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = (transform.forward * v + transform.right * h).normalized;
            if (cc.isGrounded && verticalVel < 0) verticalVel = -1f;
            verticalVel += gravity * Time.deltaTime;
            Vector3 vel = dir * speed + Vector3.up * verticalVel;
            cc.Move(vel * Time.deltaTime);
            // Simple mouse look
            float mx = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mx * 3f);
        }
    }
}

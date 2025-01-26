using UnityEngine;

namespace Core.Gameplay
{
    public class CameraControl : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private float rotationSpeed = 5f;

        private void LateUpdate()
        {
            if (target == null)
            {
                Debug.LogWarning("Camera target is not assigned!");
                return;
            }

            Vector3 desiredPosition = target.position + target.TransformDirection(offset);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
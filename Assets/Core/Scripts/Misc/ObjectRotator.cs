using UnityEngine;

namespace Core.Misc
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;
        [SerializeField] private Transform rotatePoint;

        public void Update()
        {
            if (!rotatePoint)
            {
                transform.Rotate(transform.up, rotateSpeed);
            } else
            {
                transform.Rotate(rotatePoint.up, rotateSpeed);
            }
        }
    }
}
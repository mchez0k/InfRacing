using DG.Tweening;
using UnityEngine;

namespace Core.Misc
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 30f;

        public void Update()
        {
            transform.Rotate(transform.up, rotateSpeed * Time.deltaTime);
        }
    }
}
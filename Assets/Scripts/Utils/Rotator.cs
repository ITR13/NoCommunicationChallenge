using UnityEngine;

namespace Utils
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed;
        
        private void Update()
        {
            transform.RotateAround(Vector3.forward, Time.deltaTime);
        }
    }
}
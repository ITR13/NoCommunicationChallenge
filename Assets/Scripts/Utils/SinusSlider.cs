using System.Collections;
using UnityEngine;

namespace Utils
{
    public class SinusSlider : MonoBehaviour
    {

        //Public coroutine called Slideloop


        private Vector3 _startPos;
        public Vector3 _targetPos;

        public float speed = 1f;
        public float offset = 0f;


        public void Start()
        {
            StartCoroutine(SlideLoop());
        }

        public IEnumerator SlideLoop()
        {
            _startPos = transform.position;
            //Start the coroutine
            while (true)
            {
              //Using a sinus wave lerp the object from its oriningal position to a target position
            float t = Mathf.Sin((Time.time + offset) * speed);
            //Normalize the value to be between 0 and 1
            t = (t + 1f) / 2f;
            transform.position = Vector3.Lerp(_startPos, _startPos + _targetPos, t);
            yield return null;            
            }
        }


        //On selected draw a gizmo at target pos
        private void OnDrawGizmosSelected()
        {
            //Draw a sphere at the relative position of the target
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position + _targetPos, 0.1f);
            Gizmos.DrawLine(transform.position, transform.position + _targetPos);
        }


    }
}
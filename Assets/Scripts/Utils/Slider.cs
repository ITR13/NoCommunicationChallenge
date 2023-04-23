using UnityEngine;

namespace Utils
{
    public class Slider : MonoBehaviour
    {
        [SerializeField]
        private Vector3 range;

        [SerializeField]
        private float speed = 0.1f;

        [SerializeField]
        private float delay = 0f;

        private bool goBack = false;
        private Vector3 startPos;
        
        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            //IF time since start is not greater than delay, return
            if (Time.timeSinceLevelLoad < delay) return;
            
            var pos = transform.position;
            var target = startPos + range;

            if (goBack)
            {
                target = startPos;
            }
            

            

            pos.x = Mathf.MoveTowards(pos.x, target.x, speed * Time.deltaTime);
            pos.y = Mathf.MoveTowards(pos.y, target.y, speed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Approximately(transform.position.x, target.x) &&
                Mathf.Approximately(transform.position.y, target.y))
            {
                goBack = true;
            }

            if (Mathf.Approximately(transform.position.x, startPos.x) &&
                Mathf.Approximately(transform.position.y, startPos.y))
            {
                goBack = false;
            }
        }
    }
}
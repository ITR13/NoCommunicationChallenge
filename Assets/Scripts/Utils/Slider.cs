using UnityEngine;

namespace Utils
{
    public class Slider : MonoBehaviour
    {
        [SerializeField]
        private Vector3 range;

        [SerializeField]
        private float speed = 0.1f;

        private bool goBack = false;
        private Vector3 startPos;
        
        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
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
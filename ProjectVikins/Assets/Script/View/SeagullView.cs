using UnityEngine;

namespace Assets.Script.View
{
    public class SeagullView : MonoBehaviour
    {
        private SpriteRenderer _SeagullSpriteRenderer;
        private SpriteRenderer SeagullSpriteRenderer { get { return _SeagullSpriteRenderer ?? (_SeagullSpriteRenderer = GetComponent<SpriteRenderer>()); } }
        
        float random;
        float positionX;

        private void Awake()
        {
            random = Random.Range(0, 2);
            if (random == 0)
            {
                transform.position = new Vector3(16, transform.position.y, transform.position.z);
                SeagullSpriteRenderer.flipX = true;
            }
            else
                transform.position = new Vector3(-16, transform.position.y, transform.position.z);

            positionX = transform.position.x;
        }

        private void Update()
        {
            if (random == 0)
                positionX -= 2 * Time.deltaTime;
            else
                positionX += 2 * Time.deltaTime;

            transform.position = new Vector3(positionX, transform.position.y, transform.position.z);

            if (transform.position.x <= -24 || transform.position.x >= 24)
                Destroy(this.gameObject);
        }
    }
}

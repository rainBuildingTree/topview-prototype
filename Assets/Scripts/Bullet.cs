using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector2 direction = Vector2.right;
    public bool isTriggered = false;

    private float lifeTime = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            transform.position += speed * Time.deltaTime * (Vector3)direction;
        }
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0f)
        {
            Destroy(gameObject);
        }
    }

    // destroy on collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Destroy(gameObject);
    }
}

using UnityEngine;

public class MermanProjectile : MonoBehaviour
{
    public int direction;
    public Vector3 velocity;
    private Collider2D hitbox;
    private float lifetime = 5;
    [SerializeField] private float speed;


    void Update()
    {
        velocity.x = direction * speed;
        transform.position = transform.position + velocity * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hitbox = GetComponent<Collider2D>();
            collision.GetComponent<Stats>().TakeDamage(12, false, hitbox);
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}


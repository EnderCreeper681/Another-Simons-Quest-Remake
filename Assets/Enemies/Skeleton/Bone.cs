using UnityEngine;

public class Bone : MonoBehaviour
{
    public int direction;
    public Vector3 velocity = new Vector3(0, 16, 0);
    private float gravity = 0.6f;
    private float rotation;
    private Collider2D hitbox;
    private float lifetime = 5;


    void Update()
    {
        velocity.x = direction * 5f;
        velocity.y -= gravity * Time.deltaTime * 60;
        transform.position = transform.position + velocity * Time.deltaTime; 
        rotation += 3 * 60 * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        lifetime -= Time.deltaTime;
        if(lifetime <= 0) {Destroy(gameObject);}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hitbox = GetComponent<Collider2D>();
            collision.GetComponent<Stats>().TakeDamage(5, false, hitbox);
        }
    }
}

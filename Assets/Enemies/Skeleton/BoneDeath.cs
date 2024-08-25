using UnityEngine;

public class BoneDeath : MonoBehaviour
{
    public float direction;
    public Vector2 speed;
    public Rigidbody2D rb;
    private float rotation;

    private void Start()
    {
        speed.x = direction * Random.Range(1f, 10f);
        speed.y = Random.Range(1f, 2f);
    }

    void FixedUpdate()
    {
        rb.velocity = speed;
        speed.y -= 10f * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        rotation += 2;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class ZombieAi : Enemy
{
    private int direction = 1;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    private float lifetime = 10;
    [SerializeField] private Animator anim;
    private bool active;
    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] private GameObject fireEffect;

    void Start()
    {
        if (stats.transform.position.x - transform.position.x > 0) { direction = 1; }
        else if (stats.transform.position.x - transform.position.x < 0) { direction = -1; }
    }

    new void Update()
    {
        base.Update();
        if (Physics2D.OverlapCircle(wallCheck.position, 0.15f, groundLayer))
        {
            direction *= -1;
        }
        transform.localScale = new(-direction, 1, 1);
        if(health <= 0) { Die(); }
        lifetime -= Time.deltaTime;
        if(lifetime < 10 && lifetime >= 9 || lifetime < 1.2f) 
        { 
            active = false;
            rb.gravityScale = 0;
        }
        else 
        {
            active = true;
            rb.gravityScale = 1.4f; 
        }
        if(lifetime < 1.2f) { anim.SetTrigger("Despawn"); rb.velocity = Vector2.zero; }
        if(lifetime <= 0) { Destroy(gameObject); }
    }

    private void FixedUpdate()
    {
        if (active) 
        { 
            rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
            contactDamage = true;
        }
        else { contactDamage = false; }
    }

    private void Die()
    {
        Drop(Random.Range(1, 21));
        for (int i = 0; i <= 5; i++)
        {
            Instantiate(fireEffect, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.8f, 1f), 0), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    
    private void EnableHitbox()
    {
        hitbox.enabled = true;
        contactDamage = true;
    }
    private void DisableHitbox()
    {
        hitbox.enabled = false;
        contactDamage = false;
    }
}

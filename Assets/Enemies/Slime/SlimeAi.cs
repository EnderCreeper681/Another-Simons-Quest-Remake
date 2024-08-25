using UnityEngine;

public class SlimeAi : Enemy
{
    [SerializeField] private float gravity = 0.6f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck1;
    [SerializeField] private Transform groundCheck2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ParticleSystem ps;
    private int jumpCounter;
    [SerializeField] private int direction;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float jumpSpeed = 5;
    private float aggroDistance = 17;
    private bool aggro;
    [SerializeField] private float jumpTimer = 2;
    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] private Animator anim;
    [SerializeField] private float jumpHeight = 20;
    private int rotation;

 

    void Start()
    {
        health = 40;
        damage = 12;
    }

    new private void Update()
    {
        base.Update();
        if (Mathf.Abs(stats.transform.position.x - transform.position.x) <= aggroDistance) { aggro = true; }
        isGrounded = Physics2D.OverlapArea(groundCheck1.position, groundCheck2.position, groundLayer);

        if(isGrounded && gravity == 0.6f && rb.velocity.y <= 0.01f || isGrounded && gravity == -0.6f && rb.velocity.y >= -0.01f) 
        { 
            velocity.x = 0;
            if (aggro) { jumpTimer -= Time.deltaTime; }
            anim.SetBool("isJumping", false); 
            anim.SetBool("isFlipping", false);
        }
        else 
        {
            //velocity.y -= gravity * Time.deltaTime * 60;
            velocity.x = jumpSpeed * direction;
        }

        if(jumpTimer <= 0 && jumpCounter < 3) { Jump(); }
        else if(jumpTimer <= 0 && jumpCounter == 3) { GravityFlip(); }
        if(health <= 0) { Die(); }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity.x, rb.velocity.y);
    }

    private void Jump()
    {
        //velocity.y = 20 * gravity;
        rb.AddForce(Vector2.up * gravity * jumpHeight, ForceMode2D.Impulse);
        if (stats.transform.position.x - transform.position.x > 0) { direction = 1; }
        else if (stats.transform.position.x - transform.position.x < 0) { direction = -1; }
        jumpCounter++;
        jumpTimer = 2;
        anim.SetBool("isJumping", true);
    }

    private void GravityFlip()
    {
        gravity *= -1;
        rb.gravityScale *= -1;
        rotation += 180;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        jumpCounter = 0;
        jumpTimer = 2;
        direction = 0;
        anim.SetBool("isFlipping", true);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Stats>().TakeDamage(damage, false, hitbox);
        }
    }

    private void Die()
    {
        hitbox.enabled = false;
        jumpTimer = 10;
        direction = 0;
        anim.SetTrigger("Death");
    }

    public void Destroy()
    {
        Destroy(gameObject);
        stats.currentExperience += exp;
    }
}


//nz parol 122328ub
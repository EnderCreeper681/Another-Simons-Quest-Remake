using UnityEngine;

public class MermanAi : Enemy
{
    public int direction = 1;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;
    private bool active = true;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private Collider2D terrainHitbox;
    private Stats player;
    private Vector3 playerDistance;
    private bool isSwimming = true;
    [SerializeField] private GameObject proj;
    private float attackTimer = 2f;
    private bool isJumping;
    [SerializeField] private SpriteRenderer sr;
    private bool aggro;
    [SerializeField] private ParticleSystem ps;

    void Start()
    {
        //base.Awake();
        player = FindObjectOfType<Stats>();
        playerDistance = player.transform.position - transform.position;
        if (playerDistance.x > 0) { direction = 1; }
        else if (playerDistance.x <= 0) { direction = -1; }
        
    }

    new void Update()
    {
        base.Update();
        playerDistance = player.transform.position - transform.position;
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer) && groundCheck.gameObject.activeSelf){direction *= -1;}
        if(Mathf.Abs(playerDistance.x) < 4 && isSwimming)
        {
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            moveSpeed = 0;
            anim.SetTrigger("Jump");
            isSwimming = false;
            rb.gravityScale = 2f;
            isJumping = true;
            sr.sortingLayerName = default;
            ps.Emit(20);
        }

        transform.localScale = new(-direction, 1, 1);
        if (health <= 0 && active) { Die(); }
        if (groundCheck.gameObject.activeSelf && aggro || groundCheck.gameObject.activeSelf && attackTimer <= 0) { attackTimer -= Time.deltaTime; }
        if(attackTimer <= 0) 
        { 
            anim.SetTrigger("Shoot"); 
            if(attackTimer <= -0.5f) { anim.ResetTrigger("Shoot"); attackTimer = 2; }
        }
        if(isJumping && Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer) && rb.velocity.y <= 0) { Invoke("JumpLanding", 0.5f); isJumping = false; }
    }

    private void FixedUpdate()
    {
        if (attackTimer > 0) { rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y); } 
        else { rb.velocity = new Vector2(0, rb.velocity.y); }
    }

    private void Die()
    {
        Drop(Random.Range(1, 21));
        if (playerDistance.x > 0) { direction = 1; moveSpeed = -13; }
        else if (playerDistance.x <= 0) { direction = -1; moveSpeed = -13; }
        terrainHitbox.enabled = false;
        anim.SetBool("isDead", true);
        hitbox.enabled = false;
        active = false;
        groundCheck.gameObject.SetActive(false);
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("Delete", 5);
    }
    private void Delete()
    {
        Destroy(gameObject);
    }
    private void JumpReset()
    {
        hitbox.enabled = true;
        terrainHitbox.enabled = true;
    }
    private void JumpLanding()
    {
        if (active) {
            anim.SetBool("stopJumping", true);
            moveSpeed = 3;
            if (playerDistance.x > 0) { direction = 1; }
            else if (playerDistance.x <= 0) { direction = -1; }
            groundCheck.gameObject.SetActive(true);
        }     
    }
    private void ChangeDirection()
    {
        if (playerDistance.x > 0) { direction = 1; }
        else if (playerDistance.x <= 0) { direction = -1; }
    }
    private void Shoot()
    {
        GameObject projClone = Instantiate(proj, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        projClone.GetComponent<MermanProjectile>().direction = direction;
        projClone.transform.localScale = new(direction, 1, 1);      
    }
    private void OnBecameVisible()
    {
        aggro = true;
    }
    private void OnBecameInvisible()
    {
        aggro = false;
    }
}

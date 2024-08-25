using UnityEngine;

public class WhipSkeletonAi : Enemy
{
    public Vector3 playerDistance;
    private GameObject player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float attackTimer = 2;
    private int direction;
    [SerializeField] private Animator anim;
    public GameObject bone1;
    public GameObject bone2;
    public GameObject skull;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float moveTimer;
    [SerializeField] private bool aggro;
    private float aggroDistance = 17;
    [SerializeField] BoxCollider2D whipCollider;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck1;
    [SerializeField] private Transform groundCheck2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform jumpCheckPos;
    [SerializeField] private Transform jumpCheckPos2;
    private float gravity = 0.3f;
    private float maxDist = 15;
    private float minDist = 5;
    private float minSpeed = 3.5f;
    private float maxSpeed = 6;

    private void Start()
    {
        player = GameObject.Find("Simon");
    }
    new void Update()
    {
        base.Update();
        playerDistance = player.transform.position - transform.position;
        float dist = Mathf.Abs(playerDistance.x);

        isGrounded = Physics2D.OverlapArea(groundCheck1.position, groundCheck2.position, groundLayer);
        



        if (dist > maxDist)
        {
            moveSpeed = maxSpeed;
        }
        else if (dist < minDist)
        {
            moveSpeed = minSpeed;
        }
        else
        {
            var distRatio = (dist - minDist) / (maxDist - minDist);


            var diffSpeed = maxSpeed - minSpeed;
 
            moveSpeed = (distRatio * diffSpeed) + minSpeed;
        }



        if (Mathf.Abs(playerDistance.x) <= aggroDistance) { aggro = true;}
        if (attackTimer <= 0)
        {
            anim.SetTrigger("Attack");
            if (attackTimer <= -0.61f)
            {
                attackTimer = 2f;
                anim.ResetTrigger("Attack");
            }
        }
        if (moveTimer >= 0 && aggro && isGrounded) { moveTimer -= Time.deltaTime; }
        if (playerDistance.x > 0 && attackTimer > 0)
        {
            direction = 1;
        }
        else if (playerDistance.x < 0 && attackTimer > 0)
        {
            direction = -1;
        }

        transform.localScale = new Vector3(direction, 1, 1);
        if (!aggro || attackTimer <= 0)
        {
            velocity.x = 0;
        }
        else if (Mathf.Abs(playerDistance.x) >= 4 && moveTimer <= 0)
        {
            velocity.x = moveSpeed * direction;
            moveTimer = 0.2f;
        }
        else if (Mathf.Abs(playerDistance.x) < 4 && moveTimer <= 0)
        {
            velocity.x = -moveSpeed * direction;
            moveTimer = 0.2f;
        }

        if (aggro && isGrounded) { attackTimer -= Time.deltaTime; }
        
        if (isGrounded){ velocity.y = 0; }
        else { velocity.y -= gravity * Time.deltaTime * 60; }

        if(isGrounded && !Physics2D.OverlapCircle(jumpCheckPos.position, 0.1f, groundLayer) && (velocity.x / direction) > 0 || 
           isGrounded && !Physics2D.OverlapCircle(jumpCheckPos2.position, 0.1f, groundLayer) && (velocity.x / direction) < 0) { Jump(); }

        if (health <= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;// * Time.deltaTime * 60;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Stats>().TakeDamage(damage, false, collider);
        }
    }

    public void ActivateCollider()
    {
        whipCollider.enabled = true;
        
    }

    public void DeactivateCollider()
    {
        whipCollider.enabled = false;
    }

    private void Jump()
    {
        velocity.y = 10;
        //rb.AddForce(Vector2.up, ForceMode2D.Impulse);
    }
    private void Die()
    {
        Destroy(gameObject);
        for (int i = 0; i <= 1; i++)
        {
            GameObject bone1Clone = Instantiate(bone1, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f), 0), transform.rotation);
            GameObject bone2Clone = Instantiate(bone2, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f), 0), transform.rotation);
            bone1Clone.GetComponent<BoneDeath>().direction = -direction;
            bone2Clone.GetComponent<BoneDeath>().direction = -direction;
        }
        GameObject skullClone = Instantiate(skull, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 1, 0), transform.rotation);
        skullClone.GetComponent<BoneDeath>().direction = -direction;
        Drop(Random.Range(1, 21));
    }
}
using UnityEngine;

public class SkeletonAI : Enemy
{
    public Vector3 playerDistance;
    private GameObject player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField]  private float boneTimer = 2;
    private int direction;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject boner;
    public GameObject bone1;
    public GameObject bone2;
    public GameObject skull;
    [SerializeField] private float velocity;
    [SerializeField] private float moveTimer;
    private bool aggro;

    private void Start()
    {
        player = GameObject.Find("Simon");
    }

    new private void Update()
    {
        base.Update();

        if (boneTimer <= 0)
        {
            anim.Play("Base Layer.skeleton_throw");
            if (boneTimer <= -0.41f)
            {
                boneTimer = 2f;             
            }
        }
        if (moveTimer >= 0 && aggro) { moveTimer -= Time.deltaTime; }
        if (playerDistance.x > 0 && boneTimer > 0)
        {
            direction = 1;
        }
        else if (playerDistance.x < 0 && boneTimer > 0)
        {
            direction = -1;
        }
        playerDistance = player.transform.position - transform.position;
        transform.localScale = new(direction, 1, 1); 
        if (Mathf.Abs(playerDistance.x) > 15 || boneTimer <= 0)
        {
            velocity = 0;
        }
        else if (Mathf.Abs(playerDistance.x) >= 5 && Mathf.Abs(playerDistance.x) <= 15 && moveTimer <= 0)
        {
            velocity = moveSpeed * direction;
            moveTimer = 0.4f;
        }
        else if (Mathf.Abs(playerDistance.x) < 5 && moveTimer <= 0)
        {
            velocity = -moveSpeed * direction;
            moveTimer = 0.4f;
        }

        if (aggro || boneTimer <= 0) { boneTimer -= Time.deltaTime; }
        anim.SetFloat("Speed", Mathf.Abs(velocity));
        if (health <= 0)
        {
            Die(); 
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity, rb.velocity.y);
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

    private void ThrowBone()
    {
        GameObject bonerClone = Instantiate(boner, transform.position, transform.rotation);
        bonerClone.GetComponent<Bone>().direction = direction;       
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
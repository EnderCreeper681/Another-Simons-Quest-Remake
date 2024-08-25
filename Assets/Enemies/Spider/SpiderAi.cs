using UnityEngine;

public class SpiderAi : Enemy
{
    public Vector3 playerDistance;
    private GameObject player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float attackTimer = 2.5f;
    [SerializeField] private Vector2 velocity = new(2, 8);
    private Vector2 direction;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject projectile;
    private bool aggro;
    [SerializeField] private GameObject fireEffect;

    private void Start()
    {       
        player = GameObject.Find("Simon");
        playerDistance = player.transform.position - transform.position;
        if (playerDistance.x > 0)
        {
            direction.x = 1;
        }
        else if (playerDistance.x < 0)
        {
            direction.x = -1;
        }
        
        if (Mathf.Abs(playerDistance.x) > 15) { aggro = false; }
        else { aggro = true; }
    }

    new private void Update()
    {
        base.Update();
        playerDistance = player.transform.position - transform.position;
        if (aggro || direction.x == 0) { transform.position += new Vector3(velocity.x * direction.x, velocity.y * direction.y, 0) * Time.deltaTime; }
        if (attackTimer <= 0)
        {
            if (attackTimer > -1f)
            {
                anim.SetTrigger("Attack");
                direction.y = -1;
                direction.x = 0;
            }

            if (attackTimer <= -2)
            {
                attackTimer = 2.5f;
                anim.ResetTrigger("Attack");
                direction.y = 0;
                if (playerDistance.x > 0)
                {
                    direction.x = 1;
                }
                else if (playerDistance.x < 0)
                {
                    direction.x = -1;
                }
            }
        }

        if (Mathf.Abs(playerDistance.x) > 15){ aggro = false; }
        else { aggro = true; }


        if (aggro || direction.x == 0) { attackTimer -= Time.deltaTime; }
        if (health <= 0)
        {
            Die();
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Stats>().TakeDamage(damage, false, collider);
        }
    }*/

    private void Die()
    {
        Destroy(gameObject);
        Drop(Random.Range(1, 21));
        for (int i = 0; i <= 3; i++)
        {
            Instantiate(fireEffect, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);
        }
        for (int i = 0; i <= 10; i++)
        {
            Instantiate(fireEffect, transform.position + new Vector3(0, i, 0), Quaternion.identity);
        }
    }

    private void Attack()
    {
        GameObject projectileClone = Instantiate(projectile, transform.position, transform.rotation);
        projectileClone.GetComponent<Cobweb>().direction = playerDistance.normalized;
        direction.y *= -1;
    }
}

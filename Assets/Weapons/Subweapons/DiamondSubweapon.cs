using UnityEngine;

public class DiamondSubweapon : MonoBehaviour
{
    private float lifetime = 5;
    public Vector3 velocity = new(15, -7, 0);
    [SerializeField] private Attacking attacking;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int baseDamage = 10;
    private Stats stats;

    private void Start()
    {
        attacking = FindObjectOfType<Attacking>();
        stats = FindObjectOfType<Stats>();
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

        if(Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer)) 
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            velocity.y *= -1;
        }
        if (Physics2D.OverlapCircle(wallCheck.position, 0.1f, groundLayer))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            velocity.x *= -1;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        Enemy hitEnemy = collider.GetComponent<Enemy>();
        if (hitEnemy != null && hitEnemy.iFrames <= 0)
        {
            hitEnemy.TakeDamage(baseDamage + Mathf.RoundToInt(stats.intelligence));
            hitEnemy.iFrames = 0.5f;
        }
    }
    private void OnDestroy()
    {
        attacking.diamondCount--;
    }
}

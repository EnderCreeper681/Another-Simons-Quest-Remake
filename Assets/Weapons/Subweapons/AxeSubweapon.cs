using UnityEngine;

public class AxeSubweapon : MonoBehaviour
{
    private float lifetime = 1.7f;
    [SerializeField] private float moveSpeed = 6;
    public int direction;
    public Vector3 velocity = new(0, 20, 0);
    private float gravity = 0.6f;
    private float rotation;
    private Attacking attacking;
    public int baseDamage = 7;
    private Stats stats;

    private void Start()
    {
        attacking = FindObjectOfType<Attacking>(); 
        stats = FindObjectOfType<Stats>();
    }

    void Update()
    {
        velocity.x = direction * moveSpeed;
        velocity.y -= gravity * Time.deltaTime * 60;
        transform.position = transform.position + velocity * Time.deltaTime;
        rotation += 5 * Time.deltaTime * 60; 
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);          
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
        attacking.axeCount--;
    }
}
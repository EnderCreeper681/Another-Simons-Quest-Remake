using System.Collections;
using UnityEngine;

public class CrossSubweapon : MonoBehaviour
{
    private float lifetime = 3;
    [SerializeField] private float moveSpeed = 10;
    public int direction;
    public Vector3 velocity;
    private int rotation;
    [SerializeField] private Attacking attacking;
    public int baseDamage = 7;
    public bool isReturning;
    [SerializeField] private GameObject trail;
    private float afterimageTimer = 0.2f;
    [SerializeField] private Stats stats;
    private Pausing pausing;

    private void Start()
    {
        velocity.x = direction * moveSpeed;
        attacking = FindObjectOfType<Attacking>();  
        stats = FindObjectOfType<Stats>();
        pausing = FindObjectOfType<Pausing>();
    }

    private void Update()
    {
        velocity.x -= direction * Time.deltaTime * 15;
        transform.position = transform.position + velocity * Time.deltaTime;
        rotation += 7;
        if(direction == 1 && velocity.x < 2 && velocity.x > -2 || direction == -1 && velocity.x > -2 && velocity.x < 2) { rotation += 14; }
        if (!pausing.isPaused) { transform.rotation = Quaternion.Euler(0, 0, rotation); }
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);           
        }
        if(direction == 1 && velocity.x < 1 || direction == -1 && velocity.x > -1) { isReturning = true; }
        afterimageTimer -= Time.deltaTime;
        if(afterimageTimer <= 0) 
        { 
            Instantiate(trail, transform.position, transform.rotation);
            afterimageTimer = 0.05f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && isReturning){ Destroy(gameObject);  }
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
        attacking.crossCount--;
    }
}
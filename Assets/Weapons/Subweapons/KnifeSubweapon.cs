using UnityEngine;

public class KnifeSubweapon : MonoBehaviour
{
    private float lifetime = 10;
    [SerializeField] private float moveSpeed = 10;
    public int direction;
    public Vector3 velocity;

    public int baseDamage = 2;
    public bool isReturning;
    [SerializeField] private GameObject trail;

    [SerializeField] private Stats stats;


    private void Start()
    {
        velocity.x = direction * moveSpeed;
        transform.localScale = new Vector3(-direction, 1, 1) * 0.8f;
        stats = FindObjectOfType<Stats>();
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
              
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy hitEnemy = collider.GetComponent<Enemy>();
        if (hitEnemy != null && (collider.isTrigger || hitEnemy.gameObject.name.Contains("Bone Pillar")))
        {
            hitEnemy.TakeDamage(baseDamage + Mathf.RoundToInt(stats.intelligence));
            Destroy(gameObject);
        }
        if (collider.CompareTag("Terrain")) { Destroy(gameObject); }
    }
}

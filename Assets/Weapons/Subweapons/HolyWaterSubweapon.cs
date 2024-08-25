using UnityEngine;

public class HolyWaterSubweapon : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 8;
    public int direction;
    public Vector3 velocity = new(0, -1, 0);
    private float gravity = 0.6f;
    [SerializeField] private GameObject holyWaterFlame;
    [SerializeField] private Animator anim;
    private Attacking attacking;
    private bool isDamaging;
    private Stats stats;
    private int baseDamage = 2;
    [SerializeField] private Collider2D projCollider;
    [SerializeField] private Collider2D fireCollider;
    private float lifetime;

    private void Start()
    {
        attacking = FindObjectOfType<Attacking>();
        stats = FindObjectOfType<Stats>();
    }

    void Update()
    {
        if (!isDamaging) { velocity.x = direction * moveSpeed; }
        if (!isDamaging) { velocity.y -= gravity * Time.deltaTime * 60; }
        transform.position = transform.position + velocity * Time.deltaTime;
        lifetime += Time.deltaTime;
        if(lifetime >= 3) { Delete(); }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Terrain"))
        {
            anim.SetTrigger("Fire");
            isDamaging = true;
            velocity = Vector2.zero;
            projCollider.enabled = false;
            fireCollider.enabled = true;
            lifetime -= 10;
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        Enemy hitEnemy = collider.GetComponent<Enemy>();
        if (hitEnemy != null && hitEnemy.iFrames <= 0 && isDamaging)
        {
            hitEnemy.TakeDamage(baseDamage + Mathf.RoundToInt(stats.intelligence * 0.5f));
            hitEnemy.iFrames = 0.5f;
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        attacking.holyWaterCount--;
    }
}

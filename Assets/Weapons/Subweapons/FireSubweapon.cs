using UnityEngine;

public class FireSubweapon : MonoBehaviour
{
    public Vector3 velocity = new(10, 0);
    private int baseDamage = 2;
    private Stats stats;
    private Attacking attacking;

    void Start()
    {
        velocity.y = Random.Range(-1f, 1f);
        stats = FindObjectOfType<Stats>();
        attacking = FindObjectOfType<Attacking>();
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void Delete()
    {
        Destroy(gameObject);
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
}

using UnityEngine;

public class WerewolfAi : Enemy
{
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private GameObject projectile;
    [SerializeField] private WerewolfWalk werewolfWalk;
    [SerializeField] private Collider2D chargeHitbox;
    [SerializeField] private Collider2D idleHitbox;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject flameGeyser;
    [SerializeField] private Collider2D ballHitbox;
    private Saving saving;
    public int direction = 1;
    public int chargeDirection;
    [SerializeField] private AudioClip bossTheme;


    void Start()
    {
        saving = FindObjectOfType<Saving>();
        if (saving.defeatedBosses.Contains(gameObject.name)) { Destroy(gameObject); }
        else { AudioManager.instance.PlayMusic(bossTheme, 1f); }        
    }
    
    new private void Update()
    {
        base.Update();
        if(health <= 0) { Die(); }
    }
    

    private void Throw()
    {
        GameObject projectileClone = Instantiate(projectile, transform.position, transform.rotation);
        projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * 20, -3);
    }

    private void Charge()
    {
        chargeDirection = direction;
        chargeHitbox.enabled = true;
        idleHitbox.enabled = false;
    }

    private void StopCharging()
    {
        chargeDirection = 0;
        chargeHitbox.enabled = false;
        idleHitbox.enabled = true;
    }

    private void Jump()
    {
        rb.gravityScale = 0;
        hitbox.enabled = false;
        rb.velocity = new Vector2(0, 20);
    }
    private void Dive()
    {
        transform.position = new Vector3(stats.transform.position.x, transform.position.y, transform.position.z);
        rb.velocity = new Vector2(0, -20);
    }

    private void DiveLanding()
    {
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;
        for (int i = -6; i <= 6; i++)
        {
            Instantiate(flameGeyser, transform.position + new Vector3(i, -1.2f, 0), Quaternion.identity);
        }
    }

    private void EnableCollider()
    {
        hitbox.enabled = true;
    }

    private void BallinHitboxes()
    {
        ballHitbox.enabled = true;
        hitbox.enabled = false;
        idleHitbox.enabled = false;
    }

    private void BalloutHitboxes()
    {
        ballHitbox.enabled = false;
        hitbox.enabled = true;
        idleHitbox.enabled = true;
    }

    private void Die()
    {
        saving.defeatedBosses.Add(gameObject.name); 
        Drop(Random.Range(1, 21));
        Destroy(gameObject);
    }
}

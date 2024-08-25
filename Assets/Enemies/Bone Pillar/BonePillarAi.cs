using UnityEngine;

public class BonePillarAi : Enemy
{
    private int direction = 1;
    [SerializeField] private GameObject proj;
    [SerializeField] private GameObject fireWave;
    [SerializeField] private GameObject fireEffect;
    private float turnTimer;
    private float attackTimer = 5;
    [SerializeField] private Animator anim;
    private bool onScreen = false;
    private bool close;
    

    void Start()
    {
        if (stats.transform.position.x - transform.position.x > 0) { direction = 1; }
        else if (stats.transform.position.x - transform.position.x < 0) { direction = -1; }
        turnTimer = Random.Range(-3, 2);
        attackTimer = Random.Range(2f, 4f);
    }

    new void Update()
    {
        base.Update();
        turnTimer += Time.deltaTime;

        if(turnTimer >= 0)
        {
            if (stats.transform.position.x - transform.position.x > 0) { direction = 1; }
            else if (stats.transform.position.x - transform.position.x < 0) { direction = -1; }
            turnTimer = Random.Range(-2f, 0f);
        }
        transform.localScale = new Vector3(-direction, 1, 1);

        if (onScreen && Mathf.Abs(stats.transform.position.y - transform.position.y) <= 3 ) { attackTimer -= Time.deltaTime; }

        anim.SetFloat("attackTimer", attackTimer);

        if (health <= 0) { Die(); }
    }


    private void Die()
    {
        Drop(Random.Range(1, 21));
        for (int i = 0; i <= 5; i++)
        {
            Instantiate(fireEffect, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void Shoot()
    {
        if (!close)
        {
            GameObject projClone = Instantiate(proj, transform.position, Quaternion.identity);
            projClone.GetComponent<MermanProjectile>().direction = direction;
            projClone.transform.localScale = new(direction, 1, 1);
            attackTimer += 2;
        }
    }

    private void OnBecameVisible()
    {
        onScreen = true;
    }
    private void OnBecameInvisible()
    {
        onScreen = false;
    }

    private void Fire()
    {
        if (close) 
        {
            GameObject projClone = Instantiate(fireWave, transform.position + new Vector3(direction * 0.6f, 0, 0), Quaternion.identity);
            projClone.GetComponent<MermanProjectile>().direction = direction;
            projClone.transform.localScale = new(-direction, 1, 1);
            attackTimer += 4;
        }
    }
    private void CheckDistance()
    {
        if(Mathf.Abs(stats.transform.position.x - transform.position.x) <= 4) { close = true; }
        else { close = false; }
    }
}

using UnityEngine;

public class PeepingEyeAi : Enemy
{
    [SerializeField] private Vector3 playerDistance;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private Transform eye;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float maxSpeed = 4;
    private float rotationSpeed = 180;
    private int offset = 180;
    private bool aggro;
    private float aggroDistance = 15;
    private int healthCompared = 30;
    
    


    new void Update()
    {
        base.Update();
        if(Mathf.Abs(stats.transform.position.x - transform.position.x) <= aggroDistance && Mathf.Abs(stats.transform.position.y - transform.position.y) <= aggroDistance) { aggro = true; }
        if(health <= 0) { Die(); }

        playerDistance = stats.transform.position - transform.position;
        playerDistance.Normalize();
        playerDistance *= maxSpeed;
        Debug.DrawLine(transform.position + playerDistance, transform.position);
        if (aggro) 
        {
            velocity = Vector3.Lerp(velocity, playerDistance, acceleration * Time.deltaTime * 60);
        }

        float angle = Mathf.Atan2(playerDistance.y, playerDistance.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        eye.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        if(health < healthCompared) { Knockback(); healthCompared = health; }
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity * Time.fixedDeltaTime * 60;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Stats>().TakeDamage(damage, false, hitbox);
        }
    }

    private void Die()
    {
        stats.currentExperience += exp;
        for (int i = 0; i <= 5; i++)
        {
            Instantiate(fireEffect, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void Knockback()
    {
        velocity = playerDistance * -1.5f;
    }
}

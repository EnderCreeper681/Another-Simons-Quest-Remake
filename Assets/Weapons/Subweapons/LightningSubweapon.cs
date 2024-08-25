using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LightningSubweapon : MonoBehaviour
{
    private Stats stats;
    private Attacking attacking;
    [SerializeField] private Vector2 targetRange = new(30, 30);
    [SerializeField] private List<Collider2D> targetEnemies;
    [SerializeField] private List<Collider2D> hitEnemies;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Vector3 minimumDistance = new(10000, 10000, 0);
    [SerializeField] private Vector3 velocity;
    private int baseDamage = 3;
    private float lifetime = 3;
    public Transform targetEnemy;

    void Start()
    {
        attacking = FindObjectOfType<Attacking>();
        stats = FindObjectOfType<Stats>();
        FindTarget();
        StartCoroutine(Arc());
    }


    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0) { Destroy(gameObject); }
        velocity = (targetEnemy.position - transform.position).normalized;
        transform.position += velocity * Time.deltaTime * 50;
    }

    private void FindTarget()
    {
        targetEnemies = Physics2D.OverlapBoxAll(transform.position, targetRange, 0, enemyLayer).ToList();
        for (int i = targetEnemies.Count - 1; i >= 0; i--)
        {
            if (targetEnemies[i].GetComponent<Enemy>().wasHitByLightning) { targetEnemies.RemoveAt(i); }
        }
        if (targetEnemies.Count == 0) { Destroy(gameObject); }
        else foreach(Collider2D enemy in targetEnemies)
        {
            if ((enemy.transform.position - transform.position).magnitude < minimumDistance.magnitude)
            { 
                    minimumDistance = enemy.transform.position - transform.position; 
                    targetEnemy = enemy.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() == null) return;
        lifetime = 3;
        minimumDistance = new(10000, 10000);
        collider.GetComponent<Enemy>().wasHitByLightning = true;
        hitEnemies.Add(collider);
        FindTarget();
        collider.GetComponent<Enemy>().TakeDamage(baseDamage + Mathf.RoundToInt(stats.intelligence));            
    }

    private void OnDestroy()
    {
        attacking.lightningCount--;
        foreach(Collider2D hit in hitEnemies) { if (hit != null) { hit.GetComponent<Enemy>().wasHitByLightning = false; } }
    }

    IEnumerator Arc()
    {
        while (true) 
        {
            transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            yield return new WaitForSeconds(0.15f);
        }       
    }
}
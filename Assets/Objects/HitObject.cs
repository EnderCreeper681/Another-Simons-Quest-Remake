using UnityEngine;

public class HitObject : Enemy
{
    [SerializeField] private GameObject smallHeart;
    [SerializeField] private GameObject largeHeart;
    [SerializeField] private GameObject smallCoin;
    [SerializeField] private GameObject largeCoin;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private bool isProjectile;

    void Start()
    {
        health = 1;
        contactDamage = false;
    }

    new private void Update()
    {
        if(health <= 0){ Die(Random.Range(1, 6)); }
    }

    private void Die(int dropRng)
    {
        if (!isProjectile)
        {
            if (stats.currentHearts == stats.maxHearts && dropRng == 1) { Instantiate(largeCoin, transform.position, Quaternion.identity); }
            else if (stats.currentHearts == stats.maxHearts) { Instantiate(smallCoin, transform.position, Quaternion.identity); }
            if (stats.currentHearts != stats.maxHearts && dropRng == 1) { Instantiate(largeHeart, transform.position, Quaternion.identity); }
            else if (stats.currentHearts != stats.maxHearts) { Instantiate(smallHeart, transform.position, Quaternion.identity); }
            Instantiate(fireEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
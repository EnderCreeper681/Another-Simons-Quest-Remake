using UnityEngine;

public class WerewolfProjectile : MonoBehaviour
{
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private int damage = 15;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Stats>().TakeDamage(damage, false, hitbox);
        }
        if (collider.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}

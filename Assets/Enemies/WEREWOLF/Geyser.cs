using UnityEngine;

public class Geyser : MonoBehaviour
{
    [SerializeField] private int damage = 15;
    [SerializeField] private Collider2D hitbox;
    void EnableCollider()
    {
        hitbox.enabled = true;
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Stats>().TakeDamage(damage, false, hitbox);
        }
    }
}

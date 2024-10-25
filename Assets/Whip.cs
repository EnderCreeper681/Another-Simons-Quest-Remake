using UnityEngine;

public class Whip : MonoBehaviour
{
    [SerializeField] private Attacking attacking;
    [SerializeField] private Stats stats;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy hitEnemy = collider.GetComponent<Enemy>();
        if (hitEnemy != null && hitEnemy.whipIframes <= 0)
        {
            hitEnemy.TakeDamage(attacking.whipBaseDamage + Mathf.RoundToInt(stats.strength));
            hitEnemy.whipIframes = 0.1f;
        }
    }
}
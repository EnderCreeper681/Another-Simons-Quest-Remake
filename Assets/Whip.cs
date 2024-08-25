using UnityEngine;

public class Whip : MonoBehaviour
{
    [SerializeField] private Attacking attacking;
    [SerializeField] private Stats stats;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy hitEnemy = collider.GetComponent<Enemy>();
        if (hitEnemy != null && hitEnemy.iFrames <= 0)
        {
            hitEnemy.TakeDamage(attacking.whipBaseDamage + Mathf.RoundToInt(stats.strength));
            hitEnemy.iFrames = 0.1f;
        }
    }
}

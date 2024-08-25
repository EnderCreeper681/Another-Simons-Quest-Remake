using UnityEngine;

public class DiveKick : MonoBehaviour
{
    [SerializeField] private Movement movement;
    private Stats stats;
 

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy hitEnemy = collider.GetComponent<Enemy>();
        if (hitEnemy != null && movement.isDiveKicking)
        {
            stats = FindObjectOfType<Stats>();
            hitEnemy.TakeDamage(Mathf.RoundToInt(stats.strength * 0.75f));
            movement.isDiveKicking = false;
            movement.velocity.y = 11;
            movement.doubleJumpTimer = 0;
        }
    } 
}

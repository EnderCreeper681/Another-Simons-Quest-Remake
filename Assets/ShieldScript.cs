using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Attacking attacking;
    [SerializeField] private Movement movement;

    private void Update()
    {
        if (Input.GetButtonDown("Shield") && !attacking.isAttacking)
        {
            col.enabled = true;
            spriteRenderer.color = new Color(1, 1, 1, 1);
            movement.isBackdashing = false;
            movement.backdashDelay = 0;
            movement.backdashTimer = 0;
        }
        else if(Input.GetButtonUp("Shield") || attacking.isAttacking) 
        {
            col.enabled = false;
            spriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            Destroy(collider.gameObject);
        }
    }
}

using UnityEngine;

public class WerewolfBallin : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Vector3 velocity = new(10, 17, 0);
    private int directionX = 1;
    private int directionY = 1;
    private Transform transform;
    [SerializeField] private LayerMask groundLayer;
    private float rotation;
    public RaycastHit hit;
    private float afterimageTimer = 0.2f;
    [SerializeField] private GameObject trail;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        transform = animator.GetComponent<Transform>();
        rb.gravityScale = 0;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform.position += velocity * Time.deltaTime;
        if(Physics2D.Raycast(transform.position, Vector2.up * directionY, 1f, groundLayer)) { directionY *= -1; velocity.y *= -1; }
        if (Physics2D.Raycast(transform.position, Vector2.right * directionX, 1f, groundLayer)) { directionX *= -1; velocity.x *= -1; }
        rotation += 10 * (Time.deltaTime * 60);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        afterimageTimer -= Time.deltaTime;
        if (afterimageTimer <= 0)
        {
            Instantiate(trail, transform.position, transform.rotation);
            afterimageTimer = 0.1f;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;
    }
}

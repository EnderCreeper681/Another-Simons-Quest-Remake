using UnityEngine;
public class WerewolfWalk : StateMachineBehaviour
{
    public Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform transform;
    public Vector3 playerDistance;
    public int rotation;
    public int direction;
    public int moveDirection;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float idleTimer;
    [SerializeField] private float idleTimerMax = 1.5f;
    [SerializeField] private int attackPattern;
    [SerializeField] private int previousAttackPattern;
    private float moveTimer;
    [SerializeField] private float moveTimerMax = 0.3f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        player = GameObject.Find("Simon").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        transform = animator.GetComponent<Transform>();
        playerDistance = player.position - transform.position;
        if (playerDistance.x > 0)
        {
            direction = 1;
        }
        else if (playerDistance.x < 0)
        {
            direction = -1;
        }
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        if(Mathf.Abs(playerDistance.x) < 4)
        {
            moveDirection = -direction;
        }
        else if (Mathf.Abs(playerDistance.x) >= 4)
        {
            moveDirection = direction;
        }
        idleTimer = 0;
        moveTimer = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(moveSpeed * moveDirection * Time.fixedDeltaTime * 60, rb.velocity.y);
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTimerMax)
        {
            attackPattern = Random.Range(1, 5);
            if (attackPattern != previousAttackPattern) 
            {
                animator.SetInteger("attackPattern", attackPattern);
            }
            else { attackPattern = Random.Range(1, 5);}
        }

        moveTimer += Time.deltaTime;
        playerDistance = player.position - transform.position;
        if (playerDistance.x > 0)
        {
            direction = 1;
        }
        else if (playerDistance.x < 0)
        {
            direction = -1;
        }
        transform.localScale = new Vector3(direction, 1, 1);
        if (Mathf.Abs(playerDistance.x) < 4 && moveTimer >= moveTimerMax)
        {
            moveDirection = -direction;
            moveTimer = 0;
        }
        else if (Mathf.Abs(playerDistance.x) >= 4 && moveTimer >= moveTimerMax)
        {
            moveDirection = direction;
            moveTimer = 0;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        previousAttackPattern = attackPattern;
        animator.SetInteger("attackPattern", 0);
        rb.velocity = Vector2.zero;
    }
}

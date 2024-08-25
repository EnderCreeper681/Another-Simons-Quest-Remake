using UnityEngine;

public class WerewolfCharge : StateMachineBehaviour
{
    public Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform transform;
    public Vector3 playerDistance;
    public int rotation;
    public int direction;
    public int moveDirection;
    private float moveSpeed = 4f;
    [SerializeField] private WerewolfAi werewolfAi;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        werewolfAi = animator.GetComponent<WerewolfAi>();
        player = GameObject.Find("Simon").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        transform = animator.GetComponent<Transform>();
        playerDistance = player.position - transform.position;
        if (playerDistance.x > 0)
        {
            werewolfAi.direction = 1;
        }
        else if (playerDistance.x < 0)
        {
            werewolfAi.direction = -1;
        }
        transform.localScale = new Vector3(werewolfAi.direction, 1, 1);
        moveSpeed = 5f;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(moveSpeed * werewolfAi.chargeDirection * Time.fixedDeltaTime * 500, rb.velocity.y);
        moveSpeed -= 0.06f * Time.deltaTime * 60;
    }
}

using UnityEngine;

public class WerewolfThrow : StateMachineBehaviour
{
    [SerializeField] private float jumpHeight = 4;
    public Transform player;
    [SerializeField] private Transform transform;
    public Vector3 playerDistance;
    public int rotation;
    public int direction;
    [SerializeField] private WerewolfAi werewolfAi;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        player = GameObject.Find("Simon").transform;
        transform = animator.GetComponent<Transform>();
        playerDistance = player.position - transform.position;
        werewolfAi = animator.GetComponent<WerewolfAi>();
        if (playerDistance.x > 0)
        {
            werewolfAi.direction = 1;
        }
        else if (playerDistance.x < 0)
        {
            werewolfAi.direction = -1;
        }
        transform.localScale = new Vector3(werewolfAi.direction, 1, 1);
        animator.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }
}

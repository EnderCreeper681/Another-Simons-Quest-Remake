using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 velocity;
    public bool isCrouching;
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed = 50;
    public float rotation;
    [SerializeField] private float jumpHeight = 10;
    public bool isGrounded;
    [SerializeField] private float gravity = 0.6f;
    [SerializeField] private BoxCollider2D idleHitbox;
    [SerializeField] private BoxCollider2D crouchHitbox;
    [SerializeField] private ParticleSystem ps;
    public Rigidbody2D rb;
    [SerializeField] private float terminalVelocity = -10f;
    [SerializeField] private Transform GroundCheck1;
    [SerializeField] private Transform GroundCheck2;
    [SerializeField] private Transform ceilingCheck1;
    [SerializeField] private Transform ceilingCheck2;
    public LayerMask groundLayer; 
    public float highJumpTimer;
    public float doubleJumpTimer;
    [SerializeField] private float highJumpTimerMax;
    [SerializeField] private float doubleJumpTimerMax;
    private float pitRespawnTimer;
    public bool hasDoubleJumped;
    public bool isDiveKicking;

    private Vector3 pitRespawnPos;
    public bool isSubmerged = false;
    public bool isBackdashing;
    public float backdashDelay;
    public float backdashTimer;
    public int direction = 1;
    [SerializeField] private Stats stats;
    public float stunDuration;
    [SerializeField] private Attacking attacking;
    public GameObject currentOneWayPlatform;
    public Collider2D[] groundColliders;
    public Collider2D[] ceilingColliders;
    public bool ceiling;
    private float afterimageTimer;
    [SerializeField] private GameObject afterimage;
    public Vector2 adjustedVelocity;

    [SerializeField] private Transform RaycastPos1;
    [SerializeField] private Transform RaycastPos2;
    [SerializeField] private Transform RaycastPos3;

    public bool onSlope;





    private void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        if(Input.GetButton("Crouch") && Input.GetButtonDown("Jump") && currentOneWayPlatform != null) { StartCoroutine(DisableCollision()); }
        if(Input.GetButton("Crouch") && Input.GetButtonDown("Jump") && doubleJumpTimer == doubleJumpTimerMax) { isDiveKicking = true; }
        velocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed; 
        if (velocity.x > 0 && !isBackdashing && stunDuration <= 0 && !attacking.isAttacking && !isDiveKicking)
        {
            direction = 1;
            transform.localScale = new Vector3(direction, 1, 1);
        }
        else if (velocity.x < 0 && !isBackdashing && stunDuration <= 0 && !attacking.isAttacking && !isDiveKicking)
        {
            direction = -1;
            transform.localScale = new Vector3(direction, 1, 1);
        }
        anim.SetFloat("velocityX", Mathf.Abs(velocity.x));



        if(Input.GetButton("Crouch") && isGrounded && stunDuration <= 0 && !attacking.isAttacking)
        {
            isCrouching = true;
            anim.SetBool("isCrouching", true);
        }
        else if(!attacking.isAttacking)
        {
            isCrouching = false;
            anim.SetBool("isCrouching", false); 
        }

        GroundAndCeilingCheck();



        RaycastHit2D hit1 = Physics2D.Raycast(RaycastPos1.position, Vector2.down, 1.5f, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(RaycastPos2.position, Vector2.down, 1.5f, groundLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(RaycastPos3.position, Vector2.down, 1.5f, groundLayer);




        if (hit3)
        {
            if (Mathf.Abs(Vector2.Perpendicular(hit3.normal).y) > 0.1f) { onSlope = true; }
            else { onSlope = false; }

            adjustedVelocity = Vector2.Perpendicular(hit3.normal) * 5;
            adjustedVelocity *= -direction;
            adjustedVelocity.x = velocity.x;
            if(velocity.x == 0) { adjustedVelocity.y = 0; }
            if (adjustedVelocity.y < -1) { adjustedVelocity.y -= 0.4f; }
            adjustedVelocity.Normalize();
            adjustedVelocity *= moveSpeed;
            Debug.DrawLine(transform.position, new(transform.position.x + adjustedVelocity.x, transform.position.y + adjustedVelocity.y, 0), Color.red);            
        }


        anim.SetBool("isGrounded", isGrounded);

        if(doubleJumpTimer == doubleJumpTimerMax) { hasDoubleJumped = true; }
        
        if (ceiling && velocity.y > 0) { velocity.y = 0; highJumpTimer = 0; if (doubleJumpTimer > 0) { doubleJumpTimer = doubleJumpTimerMax; } }
        else if (Input.GetButtonDown("Jump") && isGrounded && stunDuration <= 0 || highJumpTimer > 0 && Input.GetButton("Jump") && highJumpTimer < highJumpTimerMax && !ceiling && stunDuration <= 0)
        {
             velocity.y = jumpHeight;
             highJumpTimer += Time.deltaTime;
        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && stunDuration <= 0 && stats.relics.Contains("whiteCrystal") && doubleJumpTimer < doubleJumpTimerMax && !Input.GetButton("Crouch") || 
            doubleJumpTimer > 0 && Input.GetButton("Jump") && doubleJumpTimer < doubleJumpTimerMax && !ceiling && stunDuration <= 0 && stats.relics.Contains("whiteCrystal"))
        {
            velocity.y = jumpHeight;
            doubleJumpTimer += Time.deltaTime;
            anim.SetTrigger("doubleJump");
            //anim.Play("Base Layer.simon_double");
        }

        else if (isGrounded)
        {
            highJumpTimer = 0;
            doubleJumpTimer = 0;
            pitRespawnTimer += Time.deltaTime;
            isDiveKicking = false;

            //if (adjustedVelocity.y != 0 && !hit) { velocity.y = 0; }
            //if (!hit) { velocity.y = 0; }
            velocity.y = 0;

            if(pitRespawnTimer >= 3)
            {
                pitRespawnPos = transform.position;
                pitRespawnTimer = 0;
            }                       
        }
        else if (velocity.y >= terminalVelocity)
        {
            velocity.y -= gravity * Time.deltaTime * 60;
        }


        if (Input.GetButtonUp("Jump"))
        {
            highJumpTimer = 0;
            if(doubleJumpTimer > 0) { doubleJumpTimer = doubleJumpTimerMax; }
            
        }  
        if(doubleJumpTimer > 0.05f) { anim.ResetTrigger("doubleJump"); }

        if(Input.GetButtonDown("Backdash") && backdashDelay <= 0 && isGrounded && stats.relics.Contains("greenCrystal") && stunDuration <= 0)
        {
            Backdash();
        }

        if(backdashDelay > 0)
        {
            backdashDelay -= Time.deltaTime;
        }
        if (backdashTimer > 0)
        {
            backdashTimer -= Time.deltaTime;
        }
        if (backdashTimer <= 0 || !isGrounded)
        {
            isBackdashing = false;
            anim.SetBool("isBackdashing", false);
        }
        if (stunDuration > 0)
        {
            stunDuration -= Time.deltaTime;
        }
        anim.SetFloat("stunDuration", stunDuration);

        anim.SetBool("isDiveKicking", isDiveKicking);
    }

    void FixedUpdate()
    {
        if (stunDuration > 0 && isGrounded || attacking.isAttacking && isGrounded)
        {
            rb.velocity = Vector3.zero;
        }
        else if (stunDuration > 0 && !isGrounded)
        {
            rb.velocity = new Vector2(moveSpeed * direction * -1.5f, velocity.y) * Time.deltaTime * 60;           
        }
        else if (isDiveKicking)
        {
            rb.velocity = new Vector2(direction * 15f, -15) * Time.deltaTime * 60;
        }
        else if (isCrouching)
        {
            rb.velocity = Vector3.zero;
            idleHitbox.enabled = false;
            crouchHitbox.enabled = true;
        }
        else if(isBackdashing)
        {
            rb.velocity = new Vector2(backdashTimer * 50 * -direction, velocity.y) * Time.fixedDeltaTime * 60;
            afterimageTimer -= Time.deltaTime;
            if (afterimageTimer <= 0)
            { 
                GameObject afterimageClone = Instantiate(afterimage, transform.position, transform.rotation); 
                if(direction == -1) { afterimageClone.transform.rotation = Quaternion.Euler(0, 180, 0); }
                afterimageTimer = 0.05f; 
            }

        }
        else if(!isBackdashing && backdashTimer > 0)
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, velocity.x, 1f * Time.deltaTime * 60), velocity.y);
        }
        else 
        {
            /*rayPos = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.down, 1.5f, groundLayer);*/
            if (isGrounded && !Input.GetButtonDown("Jump") && highJumpTimer == 0 && onSlope) { rb.velocity = adjustedVelocity * Time.deltaTime * 60; }
            else { rb.velocity = 60 * Time.deltaTime * velocity; }        
        }
        if (!isCrouching)
        {
            idleHitbox.enabled = true;
            crouchHitbox.enabled = false;
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Water"))
        {
            if(!stats.relics.Contains("blueCrystal"))
            {
                transform.position = pitRespawnPos;
                rb.velocity = Vector2.zero;
                stats.TakeDamage(Mathf.RoundToInt(stats.maxHealth / 5), true, null);
            }
            else
            {
                isSubmerged = true;
                gravity = 0.4f;
                moveSpeed = 4;
                highJumpTimerMax = 0.4f;
                doubleJumpTimerMax = 0.2f;
                if(doubleJumpTimer == 1) { doubleJumpTimer = 2; }
                highJumpTimer = 0;
                jumpHeight = 8;
            }         
        }
        if (collider.CompareTag("Pit"))
        {
            transform.position = pitRespawnPos;
            rb.velocity = Vector2.zero;
            stats.TakeDamage(Mathf.RoundToInt(stats.maxHealth / 5), true, null);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Water"))
        {
            if (stats.relics.Contains("blueCrystal"))
            {
                isSubmerged = false;
                gravity = 0.6f;
                moveSpeed = 5;
                highJumpTimerMax = 0.217f;
                doubleJumpTimerMax = 0.1f;
                jumpHeight = 10;
            }
        }
    }

    private void Backdash()
    {
        isBackdashing = true;
        anim.SetBool("isBackdashing", true);
        backdashTimer = 0.5f;
        backdashDelay = 1f;
    }

    private void GroundAndCeilingCheck()
    {
        groundColliders = Physics2D.OverlapAreaAll(GroundCheck1.position, GroundCheck2.position, groundLayer);

        foreach (Collider2D collider in groundColliders)
        {
            if (collider.CompareTag("Platform") && rb.velocity.y <= 0.3f)
            {
                isGrounded = true;
                currentOneWayPlatform = collider.gameObject;
            }
            else if (!collider.CompareTag("Platform") && rb.velocity.y <= 0.3f)
            {
                isGrounded = true;
                currentOneWayPlatform = null;
            }
            else if (rb.velocity.y > 0.3f && !onSlope)
            {
                isGrounded = false;
                currentOneWayPlatform = null;
            }          
        }

        if (onSlope) { isGrounded = true; }
        if (groundColliders.Length == 0) { isGrounded = false; currentOneWayPlatform = null; }


        ceilingColliders = Physics2D.OverlapAreaAll(ceilingCheck1.position, ceilingCheck2.position, groundLayer);
        foreach(Collider2D collider in ceilingColliders)
        {
            if (!collider.CompareTag("Platform")) { ceiling = true; }
        }
        if (ceilingColliders.Length == 0) { ceiling = false; }
    }


    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(idleHitbox, platformCollider);
        Physics2D.IgnoreCollision(crouchHitbox, platformCollider);
        isGrounded = false;
        groundLayer &= ~(1 << LayerMask.NameToLayer("Terrain"));
        yield return new WaitForSeconds(0.25f);

        Physics2D.IgnoreCollision(crouchHitbox, platformCollider, false);
        Physics2D.IgnoreCollision(idleHitbox, platformCollider, false);
        groundLayer |= (1 << LayerMask.NameToLayer("Terrain"));
    }

    private void DoubleJumpEffect()
    {
        ps.Emit(20);
    }
}
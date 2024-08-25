using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attacking : MonoBehaviour
{
    [SerializeField] private int whipLevel = 1;
    [SerializeField] private float attackTimer;
    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] private BoxCollider2D hitboxSmall;
    [SerializeField] private BoxCollider2D hitboxSmallCrouch;
    [SerializeField] private Transform subPos;
    [SerializeField] private Transform subPosCrouch;
    public bool isAttacking;
    [SerializeField] private Movement movement;
    [SerializeField] private Animator anim;
    public int whipBaseDamage = 5;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject diamond;
    [SerializeField] private GameObject fireBreath;
    [SerializeField] private Stats stats;
    //[SerializeField] private Animator subAnim;
    [SerializeField] private GameObject subweaponIcon;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject cross;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject holyWater;

    public int axeCount;
    public int diamondCount;
    public int lightningCount;
    public int crossCount;
    public int holyWaterCount;
    public int i = 10;
    public float fireColliderTimer;
    public List<string> subweapons;
    public Subweapon currentSubweapon;
    public int subweaponNumber;   
    public bool fireCoroutineActive;
    [SerializeField] private Pausing pausing;
    private Vector3 pos;

    [SerializeField] AudioClip whip;

    public List<Subweapon> subweaponsNew;
    public Subweapon subweaponDefault;

    void Start() 
    {
        subweaponDefault = new Subweapon();
        subweaponDefault.title = "a";
        subweaponDefault.description = "nothing";
        subweaponDefault.icon = null;
    }

    void Update()
    {
        if (subweaponsNew.Count == 0) 
        { 
            subweaponIcon.GetComponent<Image>().sprite = null;
            currentSubweapon = subweaponDefault;
        }
        fireColliderTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Attack") && !isAttacking && !movement.isBackdashing && movement.stunDuration <= 0 && !pausing.isPaused && !movement.isDiveKicking)
        {
            Attack();
        }
        if(attackTimer > 0) 
        { 
            attackTimer -= Time.deltaTime;  
            isAttacking = true;
        }
        else
        {
            hitbox.enabled = false;
            hitboxSmall.enabled = false;
            hitboxSmallCrouch.enabled = false;
            isAttacking = false;
            anim.ResetTrigger("isAttacking");
        }
        if (Input.GetButtonDown("Subweapon") && !isAttacking && !movement.isBackdashing && movement.stunDuration <= 0 && currentSubweapon.title != "Fire Book" && !pausing.isPaused)
        {
            UseSubweapon();
        }
        if (currentSubweapon.title == "Fire Book" && stats.currentHearts >= 1 && !isAttacking && !movement.isBackdashing && movement.stunDuration <= 0 && Input.GetButtonDown("Subweapon") && !fireCoroutineActive)  
        { StartCoroutine(FireBreath()); fireCoroutineActive = true; anim.SetBool("CastingFire", true); }
        if (currentSubweapon.title == "Fire Book" && Input.GetButtonUp("Subweapon") && fireCoroutineActive || stats.currentHearts <= 1 && fireCoroutineActive || movement.stunDuration > 0 && fireCoroutineActive) 
        { StopAllCoroutines(); fireCoroutineActive = false; anim.SetBool("CastingFire", false); }
        if (Input.GetKeyDown(KeyCode.Z) && subweaponsNew.Count != 0) { SwitchSubweapon(-1); }
        if (Input.GetKeyDown(KeyCode.X) && subweaponsNew.Count != 0) { SwitchSubweapon(1); }
    }

    private void Attack()
    {
        anim.SetInteger("whipType", whipLevel);
        anim.SetTrigger("isAttacking");
        AudioManager.instance.PlaySound(whip, transform, 1f);
        attackTimer = 0.484f;
    }

    private void SwitchSubweapon(int amount)
    {
        subweaponNumber += amount;
        if (subweaponNumber > subweaponsNew.Count - 1) { subweaponNumber = 0; }
        if (subweaponNumber < 0) { subweaponNumber = subweaponsNew.Count - 1; }
        currentSubweapon = subweaponsNew[subweaponNumber];
            
 
        subweaponIcon.GetComponent<Image>().sprite = currentSubweapon.icon;
       
    }

    private void UseSubweapon()
    {
         if (currentSubweapon.title == "Axe" && stats.currentHearts >= 2 && axeCount < 2 ||
            currentSubweapon.title == "Diamond" && stats.currentHearts >= 3 && diamondCount < 3 ||
            currentSubweapon.title == "Thunder Book" && stats.currentHearts >= 6 && lightningCount == 0 ||
            currentSubweapon.title == "Cross" && stats.currentHearts >= 6 && crossCount == 0 ||
            currentSubweapon.title == "Knife" && stats.currentHearts >= 1 ||
            currentSubweapon.title == "Holy Water" && stats.currentHearts >= 2 && holyWaterCount == 0) 
         { anim.SetTrigger("Subweapon"); attackTimer = 0.48f; }
    }

    public void InstantiateSubweapon()
    {
        if (!movement.isCrouching) { pos = subPos.position; }
        else { pos = subPosCrouch.position;}
        if (currentSubweapon.title == "Axe")
        {
            GameObject axeClone = Instantiate(axe, pos, transform.rotation);
            axeClone.GetComponent<AxeSubweapon>().direction = movement.direction;
            stats.currentHearts -= 2;
            axeCount++;
        }
        if(currentSubweapon.title == "Diamond")
        {
            GameObject diamondClone = Instantiate(diamond, pos, Quaternion.identity); 
            diamondClone.GetComponent<DiamondSubweapon>().velocity.x *= movement.direction; 
            diamondClone.transform.localScale = new Vector3(movement.direction, 1, 1); 
            stats.currentHearts -= 3;
            diamondCount++;
        }
        if (currentSubweapon.title == "Thunder Book")
        {
            Instantiate(lightning, pos, Quaternion.identity);
            stats.currentHearts -= 6;
            lightningCount++;
        }
        if (currentSubweapon.title == "Cross")
        {
            GameObject crossClone = Instantiate(cross, pos, Quaternion.identity);
            crossClone.GetComponent<CrossSubweapon>().direction = movement.direction;
            stats.currentHearts -= 6;
            crossCount++;
        }
        if (currentSubweapon.title == "Knife")
        {
            GameObject knifeClone = Instantiate(knife, pos, Quaternion.identity);
            knifeClone.GetComponent<KnifeSubweapon>().direction = movement.direction;
            stats.currentHearts -= 1;
        }
        if (currentSubweapon.title == "Holy Water")
        {
            GameObject holyWaterClone = Instantiate(holyWater, pos, transform.rotation);
            holyWaterClone.GetComponent<HolyWaterSubweapon>().direction = movement.direction;
            stats.currentHearts -= 2;
            holyWaterCount++;
        }
    }

    public void WhipCollider()
    {
        hitbox.enabled = true;
    }

    public void WhipColliderSmall()
    {
        hitboxSmall.enabled = true;
    }

    public void WhipColliderSmallCrouch()
    {
        hitboxSmallCrouch.enabled = true;
    }

    IEnumerator FireBreath()
    {
        while (true)
        { 
            GameObject fireClone = Instantiate(fireBreath, firePoint.position, Quaternion.identity);
            fireClone.GetComponent<FireSubweapon>().velocity.x *= movement.direction;
            fireClone.GetComponent<FireSubweapon>().velocity.x += movement.rb.velocity.x;
            attackTimer = 0.1f;
            i++;
            if (i >= 10) { stats.currentHearts--; i = 0; }
            yield return new WaitForSeconds(0.05f);
        }         
    }
}

using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int exp;
    public int damage;
    public Stats stats;
    public Attacking attacking;
    [SerializeField] private GameObject damageNumber;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Material defaultMaterial;
    
    public bool wasHitByLightning = false;
    public new Collider2D collider;
    public float iFrames;
    public float whipIframes;
    public bool contactDamage = true;

    [SerializeField] private int dropClass;
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private GameObject coin1;
    [SerializeField] private GameObject coin2;
    [SerializeField] private GameObject coin3;
    [SerializeField] private GameObject coin4;


    public void Awake()
    {
        attacking = FindObjectOfType<Attacking>();
        stats = FindObjectOfType<Stats>();
        collider = GetComponent<Collider2D>();
    }

    public void Update()
    {
        if (iFrames > 0) { iFrames -= Time.deltaTime; }
        if (whipIframes > 0) { whipIframes -= Time.deltaTime; }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        AudioManager.instance.PlaySound(hitSound, transform, 1f);
        if (!CompareTag("Hit Object"))
        {
            GameObject numberClone = Instantiate(damageNumber, transform.position, Quaternion.identity);
            if (numberClone != null)
            {
                numberClone.GetComponent<TextMeshPro>().text = amount.ToString();
                numberClone.GetComponent<TextMeshPro>().color = Color.white;
            }
        }
        GetComponent<SpriteRenderer>().material = flashMaterial;
        Invoke("FlashReset", 0.15f);
    }

    private void FlashReset()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && contactDamage)
        {
            collision.GetComponent<Stats>().TakeDamage(damage, false, collider);
        }
    }

    public void Drop(int dropRng) 
    {
        stats.currentExperience += exp;
        if(dropRng <= 4)
        {
            if (dropClass == 1) { Instantiate(coin1, transform.position, Quaternion.identity); }
            if (dropClass == 2) { Instantiate(coin2, transform.position, Quaternion.identity); }
            if (dropClass == 3) { Instantiate(coin3, transform.position, Quaternion.identity); }
        }
        else if(dropRng == 5)
        {
            if (dropClass == 1) { Instantiate(coin2, transform.position, Quaternion.identity); }
            if (dropClass == 2) { Instantiate(coin3, transform.position, Quaternion.identity); }
            if (dropClass == 3) { Instantiate(coin4, transform.position, Quaternion.identity); }
        }
    }
}
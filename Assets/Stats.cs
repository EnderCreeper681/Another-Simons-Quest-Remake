using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class Stats : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100;
    public int currentHearts;
    public int maxHearts = 50;
    public bool greenCrystal;
    public bool blueCrystal;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text heartCounter;
    [SerializeField] private GameObject ui;
    [SerializeField] private Vector3 respawnPos;
    [SerializeField] private Movement movement;
    [SerializeField] private Material hurtMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material iframesMaterial;
    [SerializeField] private GameObject eventText;
    [SerializeField] private Saving saving;
    public float iFrames;
    [SerializeField] private Animator saveAnim;
    public float currentExperience;
    public float nextExperience;
    public int level = 1;
    public int gold;
    public float strength = 1;
    public int constitution = 1;
    public float intelligence = 1;
    public int luck = 1;

    public string currentScene;

    public List<string> relics;
    public List<string> pickups;
    public List<string> items;
    public List<string> itemCounts;
    [SerializeField] private GameObject damageNumber;
    


    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, currentHealth / maxHealth, 0.01f);
        heartCounter.text = currentHearts.ToString();

        if(currentExperience >= nextExperience) { LevelUp(); }
        currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "Title Screen" || currentScene == "Game Over Screen") { ui.SetActive(false);}
        else { ui.SetActive(true);}

        if(iFrames > 0)
        {
            iFrames -= Time.deltaTime;
            //StartCoroutine(BlinkEffect());
        }
        else
        {
            
        }
    }

    public void TakeDamage(int amount, bool wasUnderwater, Collider2D collider)
    {
        if(iFrames <= 0 && !wasUnderwater)
        {
            currentHealth -= amount;
            iFrames = 1.5f;
            movement.stunDuration = 0.5f;
            movement.isDiveKicking = false;
            GetComponent<SpriteRenderer>().material = hurtMaterial;
            Invoke("ResetMaterial", 0.15f);
            if (!movement.isGrounded) 
            { 
                movement.velocity.y = 8; 
                movement.highJumpTimer = 0; 
                if(collider.transform.position.x - transform.position.x > 0) { movement.direction = 1; }
                else if (collider.transform.position.x - transform.position.x <= 0) { movement.direction = -1; }
            }

            GameObject numberClone = Instantiate(damageNumber, transform.position, Quaternion.identity);
            if (numberClone != null)
            {
                numberClone.GetComponent<TextMeshPro>().text = amount.ToString();
                numberClone.GetComponent<TextMeshPro>().color = Color.red;
            }
        }
        if (wasUnderwater)
        {
            currentHealth -= amount;
            iFrames = 1;
            movement.stunDuration = 0.5f;
            movement.isDiveKicking = false;
        }
    }

    private void ResetMaterial()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
        StartCoroutine(BlinkEffect());
    }


    

    private void LevelUp()
    {
        level++;
        strength++;
        constitution++;
        intelligence++;
        luck++;
        maxHealth += 7;
        currentHealth += 7;
        currentExperience -= nextExperience;
        nextExperience = (level - 1) * 115;
        GameObject levelupText = Instantiate(eventText, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        levelupText.GetComponent<TextMeshPro>().color = Color.yellow;
        levelupText.GetComponent<TextMeshPro>().text = "LEVEL UP";
    }


    IEnumerator BlinkEffect()
    {
        if(iFrames > 0)
        {
            GetComponent<SpriteRenderer>().material = iframesMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = defaultMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = iframesMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = defaultMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = iframesMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = defaultMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = iframesMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = defaultMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = iframesMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = defaultMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = iframesMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = defaultMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = iframesMaterial;
            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().material = defaultMaterial;
            yield return new WaitForSeconds(0.1f);
        }      
    }
}
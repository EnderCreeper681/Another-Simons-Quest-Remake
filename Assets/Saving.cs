using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Saving : MonoBehaviour
{
    [SerializeField] private GameObject eventText;
    [SerializeField] private Stats stats;
    [SerializeField] private Movement movement;
    [SerializeField] private Attacking attacking;
    public string savedScene;
    public Vector3 respawnPos;
    [SerializeField] private Animator anim;
    public List<string> defeatedBosses = new List<string>();


    [SerializeField] private AudioClip bloodyTears;


    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    

    private void Update()
    {
        if(stats.currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Save") && Input.GetButtonDown("Interact"))
        {
            Save(collision);
        }
    }

    private void GameOver()
    {
        stats.iFrames += 5f;
        if (stats.currentHealth > -1000 && movement.isGrounded) 
        { 
            anim.SetTrigger("Death"); 
            Invoke("GameOverScreen", 3f);
            stats.currentHealth -= 1001; 
            movement.stunDuration += 15f;
        }
        if(!movement.isGrounded) { movement.stunDuration = 2f; }
    }

    public void Save(Collider2D collider)
    {
        SaveSystem.SavePlayer(GetComponent<Stats>(), GetComponent<Attacking>(), GetComponent<Saving>());

        stats.currentHealth = stats.maxHealth;
        stats.currentHearts = stats.maxHearts;
        collider.GetComponent<Animator>().SetTrigger("Save");
        GameObject saveText = Instantiate(eventText, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        saveText.GetComponent<TextMeshPro>().color = Color.blue;
        saveText.GetComponent<TextMeshPro>().text = "GAME SAVED";
    }

    public void GameOverScreen()
    {
        SceneManager.LoadScene("Game Over Screen");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/SavedData.json";
        if (!File.Exists(path))
        {
            LoadDefault();
        }
        else
        {
            PlayerData data = SaveSystem.LoadPlayer();
            SceneManager.LoadScene(data.currentScene);
            AudioManager.instance.CheckArea(SceneManager.GetActiveScene().ToString());

            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            stats.level = data.level;
            stats.nextExperience = data.nextExperience;
            stats.currentExperience = data.currentExperience;
            stats.gold = data.gold;
            stats.maxHealth = data.maxHealth;
            stats.maxHearts = data.maxHearts;
            stats.strength = data.strength;
            stats.constitution = data.constitution;
            stats.intelligence = data.intelligence;
            stats.relics = data.relics.ToList();
            attacking.subweapons = data.subweapons.ToList();
            stats.pickups = data.pickups.ToList();
            defeatedBosses = data.defeatedBosses.ToList();


            stats.currentHealth = data.maxHealth;
            stats.currentHearts = data.maxHearts;
            anim.ResetTrigger("Death");
            movement.stunDuration = 0;
            stats.iFrames = 1;

 
        }       
    }

    public void LoadDefault()
    {
        SceneManager.LoadScene("Virtus Village");
        AudioManager.instance.CheckArea(SceneManager.GetActiveScene().ToString());

        transform.position = new Vector3(-6, 15, 0);
        stats.level = 1;
        stats.nextExperience = 50;
        stats.currentExperience = 0;
        stats.gold = 0;
        stats.maxHealth = 100;
        stats.maxHearts = 20;
        stats.strength = 1;
        stats.constitution = 1;
        stats.intelligence = 1;
        stats.currentHearts = 0;
        stats.relics = new List<string>();
        attacking.subweapons = new List<string>();
        stats.pickups = new List<string>();
        defeatedBosses = new List<string>();


        stats.currentHealth = stats.maxHealth;
        anim.ResetTrigger("Death");
        movement.stunDuration = 0;
        stats.iFrames = 1;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        AudioManager.instance.CheckArea(scene.name);
    }
}

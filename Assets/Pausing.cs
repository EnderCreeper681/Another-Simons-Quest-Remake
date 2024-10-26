using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Controls;

public class Pausing : MonoBehaviour
{
    public bool isPaused;
    public bool isTextPaused;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Stats stats;
    [SerializeField] private Image expBar;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text heartText;
    [SerializeField] private GameObject currentWindow;


    void Update()
    {
        if (Input.GetButtonDown("Pause") && !isPaused && !isTextPaused)
        {
            Pause();
        }

        else if (Input.GetButtonDown("Pause") && isPaused)
        {
            Unpause();
        }

        if (Input.anyKeyDown && currentWindow != null) 
        { 
            currentWindow.GetComponent<Animator>().SetTrigger("Close");
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        expBar.fillAmount = stats.currentExperience / stats.nextExperience;
        levelText.text = "SIMON LVL " + stats.level;
        expText.text = "EXP " + stats.currentExperience + "/" + stats.nextExperience;
        goldText.text = "GOLD: " + stats.gold;
        hpText.text = "HP " + stats.currentHealth + "/" + stats.maxHealth;
        heartText.text = "SP " + stats.currentHearts + "/" + stats.maxHearts;
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void TextPause()
    {
        isTextPaused = true;
        Time.timeScale = 0;
        currentWindow = GameObject.FindGameObjectWithTag("Window");
    }

    public void TextUnpause()
    {
        isTextPaused = false;
        Time.timeScale = 1;
    }
}

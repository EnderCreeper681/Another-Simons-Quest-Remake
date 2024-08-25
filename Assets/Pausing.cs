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

        if (Input.anyKeyDown && isTextPaused) { TextUnpause(); }
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
    }

    private void TextUnpause()
    {
        isTextPaused = false;
        Time.timeScale = 1;
        GameObject[] textWindows = GameObject.FindGameObjectsWithTag("Window");
        foreach (GameObject obj in textWindows) { Destroy(obj); }
    }
}

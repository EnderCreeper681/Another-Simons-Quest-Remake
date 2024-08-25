using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    private Saving player;
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        string path = Application.persistentDataPath + "/SavedData.json";
        if (!File.Exists(path))
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        player = FindObjectOfType<Saving>();
        player.Load();
    }

    public void NewGame()
    {
        player = FindObjectOfType<Saving>();
        player.LoadDefault();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] MenuScript menuScript;
    [SerializeField] SoundController sounds;

    public void PlayGame()
    {
        menuScript.PlayGame();
    }

    public void RestartLevel()
    {
        menuScript.PlayGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        menuScript.SelectLevel(0);
    }

    public void MusicChange()
    {
        sounds.PauseMenuMusic();
    }
}

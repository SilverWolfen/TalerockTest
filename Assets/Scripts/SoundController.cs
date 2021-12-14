using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private string fileName = "input.settings";
    [SerializeField] private AudioSource sceneMainSound;
    [SerializeField] private AudioSource menuSound;
    [SerializeField] private Slider slider;
    private static float masterVolume = 1f;

    private void Awake()
    {
        menuSound.Play();
        LoadSettings();
    }

    private void Start()
    {
        menuSound.Pause();
        Debug.Log(Application.dataPath);
    }

    public void ChangeValue()
    {
        sceneMainSound.volume = slider.value;
        menuSound.volume = slider.value;
        masterVolume = slider.value;
    }

    private string Path()
    {
        return Application.dataPath + "/" + fileName;
    }

    public void LoadSettings()
    {
        if (!File.Exists(Path()))
        {
            sceneMainSound.volume = masterVolume;
            menuSound.volume = masterVolume;
            return;
        }

        StreamReader reader = new StreamReader(Path());

        while (!reader.EndOfStream)
        {
            SetValue(reader.ReadLine());
        }
        reader.Close();
    }

    private void SetValue(string value)
    {
        string[] result = value.Split(new char[] { '=' });
        if (result[0] == "MusicBar")
        {
            masterVolume = float.Parse(result[1]);
            if (slider != null)
            {
                slider.value = masterVolume;
            }
        }
    }

    public void PlayAllMusic()
    {
        sceneMainSound.Play();
        menuSound.Play();
    }

    public void PauseAllMusic()
    {
        sceneMainSound.Pause();
        menuSound.Pause();
    }

    public void PauseMenuMusic()
    {
        menuSound.Pause();
        sceneMainSound.UnPause();
    }

    public void PlayMenuMusic()
    { 
        sceneMainSound.Pause();
        menuSound.UnPause();
    }
}


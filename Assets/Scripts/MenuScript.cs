using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private ResultsWriter resultsWriter;
    [SerializeField] private IntegerVariable score;
    [SerializeField] private int topCount = 10;
    [SerializeField] private Text topText;

    private bool paused = false;
    private string[] botsName = new string[] {"Byxou salaT", "Ахаха я пошутил", "какао", "ВсеХ НагНу","Укуси меня пчола", "Хлеб", "I am Batmen",
                                                "Злой зелёный таракан", "тапок судьбы", "шаловливая пиранья", "CHYBO_KRABRA", "Оближи_меня", "Холодный чай",
                                                "ZloiZayac", "Игрок 1","Игрок 2", "Игрок 3", "Игрок 4", "Игрок 5", "Игрок 6", "Игрок 7",
                                                "Сливки", "Крачибачок", "Hugo_borz", "KOT_MAMINOI_PODRUGI", "Бекон", "nerg-molotok", "бОЕВОЕмясо!"};

    public void PauseGame()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void PlayGame()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
        }
    }

    public void SelectLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }

    public void AddPlaerName()
    {
        string inputName = inputField.text;
        resultsWriter.AddLine(inputName, score.GetValue());
        WriteTopList();
    }

    private void WriteTopList()
    {
        Dictionary<string, int> topList = resultsWriter.GetDictionary();
        int topNumber = 1;
        string resultString = "";
        if (topList.Count < topCount)
        {
            AddBots(topNumber);
        }
        List<KeyValuePair<string, int>> sortedList = topList.ToList();
        MyComparer myComparer = new MyComparer();

        sortedList.Sort(0, topCount, myComparer);
        foreach (var value in sortedList)
        {
            resultString += string.Format("{0}. {1} - {2} \n", topNumber, value.Key, value.Value);
            topNumber++;
        }
        topText.text = resultString;
    }

    private void AddBots(int incom)
    {
        for (int i = incom; i < topCount; i++)
        {
            System.Random rnd = new System.Random();
            int randScore = rnd.Next(1, 1000);
            int botIndex = rnd.Next(0, botsName.Length);
            string botName = botsName[botIndex];
            if (!resultsWriter.AddLine(botName, randScore))
                i--;
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    }

public class MyComparer : IComparer<KeyValuePair<string, int>>
{
    public int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
    {
        int retval = y.Value.CompareTo(x.Value);

        if (y.Value != 0)
        {
            return retval;
        }
        else
        {
            return y.Value.CompareTo(x.Value);
        }
    }
}

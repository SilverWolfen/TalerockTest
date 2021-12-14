using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ResultsWriter", menuName = "Variables/ResultsWriter", order = 50)]
public class ResultsWriter : ScriptableObject
{
	public delegate void OnResultWriteEvent(Dictionary<string, int> valueList);
	private event OnResultWriteEvent listeners;

	public event OnResultWriteEvent Listeners
    {
        add
        {
			listeners -= value;
			listeners += value;
        }
		remove => listeners -= value;
    }
	private ResultsWriter()
    {
		playerList = new Dictionary<string, int>();
	}

	[SerializeField] private Dictionary<string, int> playerList;

	public Dictionary<string, int> GetDictionary()
    {
		return playerList;
    }

	public void SetDictionary(Dictionary<string, int> value)
    {
		playerList = value;
		Raise();
    }

	public bool AddLine(string name, int score)
	{
		bool result = true;
		if (playerList.ContainsKey(name))
		{
			playerList[name] = score;
			result = false;
		}
		else
		{
			playerList.Add(name, score);
			result = true;
		}
		Raise();
		return result;
    }

	public void ResetValue()
    {
		playerList.Clear();
		Raise();
	}

	private void Raise()
	{
		listeners?.Invoke(playerList);
	}
}

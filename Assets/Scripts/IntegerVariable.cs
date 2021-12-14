using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntegerVariable", menuName = "Variables/IntegerVariable", order = 50)]
public class IntegerVariable : ScriptableObject
{
    public delegate void OnVariableChangeEvent(int value);
    private event OnVariableChangeEvent listeners;

    public event OnVariableChangeEvent Listeners
    {
        add
        {
            listeners -= value;
            listeners += value;
        }
        remove => listeners -= value;
    }

    [SerializeField] private int totalValue;

    public int GetValue()
    {
        return totalValue;
    }

    public void SetValue(int value)
    {
        totalValue = value;
        Raise();
    }

    public void ApplyChange(int value)
    {
        totalValue += value;
        Raise();
    }

    public void ResetValue()
    {
        totalValue = 0;
        Raise();
    }

    private void Raise()
    {
        listeners?.Invoke(totalValue);
    }
}

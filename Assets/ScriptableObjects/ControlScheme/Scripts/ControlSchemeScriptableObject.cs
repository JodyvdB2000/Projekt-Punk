using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "InputScheme", menuName = "ScriptableObjects/ControlScheme", order = 1)]
public class ControlSchemeScriptableObject : ScriptableObject
{
    public Inputs scheme;
}

[Serializable]
public class Inputs
{
    public string[] actions;
    public KeyCode[] keyCodes;

    public KeyCode GetKeyCode(string key)
    {
        KeyCode defaultKey = KeyCode.None;
        int counter = 0;

        foreach (string action in actions)
        {
            if (action.Equals(key))
            {
                return keyCodes[counter];
            }

            counter++;
        }

        Debug.Log("Couldn't find key");
        return defaultKey;
    }

    public void SetKeyCode(string key, KeyCode newKey)
    {
        int counter = 0;

        foreach (string action in actions) 
        {
            if (action.Equals(key))
            {
                keyCodes[counter] = newKey;
            }

            counter++;
            return;
        }

        Debug.Log("Couldn't find key");
        return;
    }
}
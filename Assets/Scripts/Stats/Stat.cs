using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is the blueprint for every Stat variable.
[System.Serializable]
public class Stat
    {
    public int BaseValue; //This is the BaseValue of the stat, assigned in the inspector.
    //The GetValue method will return the value of the stat's BaseValue.
    public int GetValue()
    {
        return BaseValue; //This will return the BaseValue of the Stat.
    }
}


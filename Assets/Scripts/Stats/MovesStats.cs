using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovesStats : MonoBehaviour
{
    public string m_name; //This variable will hold the Move GameObject's name, assigned in the inspector.
    public int m_damagemultiplier; //This variable will hold the Move GameObject's damage multiplier this will be multiplied to the Character GameObject's p_atk when the move is selected, assigned in the inspector.
    public int m_ppconsumed; //This variable will hold the Move GameObject's ppconsumed, the amount of pp that is minus from the Character GameObject's p_pp when the move is selected, assigned in the inspector.
}

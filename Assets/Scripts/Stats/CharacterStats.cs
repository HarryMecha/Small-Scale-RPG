using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterStats : MonoBehaviour
{
    public string p_name; //This variable will hold the Character GameObject's name, assigned in the inspector.
    public int p_currenthp { get; private set; } //This variable will hold the Character GameObject's current hp, it will start as the same as p_hp however will change when used in scripts e.g. TakeDamage or RecoverHealth.
    public int p_hp; //This variable will hold the Character GameObject's maximum hp, assigned in the inspector.
    public int p_pp; //This variable will hold the Character GameObject's maximum pp, assigned in the inspector.
    public int p_currentpp { get; private set; } //This variable will hold the Character GameObject's current pp, it will start as the same as p_pp however will change when used in scripts.
    public Stat p_def; //This variable will hold the Character GameObject's defence, assigned in the inspector.
    public Stat p_atk; //This variable will hold the Character GameObject's attack, assigned in the inspector.
    public Stat p_spd; //This variable will hold the Character GameObject's speed, assigned in the inspector.
    private void Start()
    {
        DatabaseManager.StatLoader(p_hp, p_pp, p_def, p_atk, p_spd); //This will run the script StatLoader found in the script DatabaseManager, replacing all of the stats with those found in the corresponding record in the Stats Table.
        if (Character.BattleCounter < 1) //This if statement will only run if the Character GameObject's BattleCounter is less than 1 implying the user has not battled an enemy yet.
        {
            p_currenthp = p_hp; //This will set p_currenthp to be equal to p_hp.
        }
    }
    //The TakeDamage method is used to minus the int damage that is in the EnemyStats component attached to the Enemy GameObject from the p_currenthp of the Character GameObject.
    public void TakeDamage(int damage) //This script requires an int damage that is in the EnemyStats component attached to the Enemy GameObject. 
    {
        p_currenthp -= damage; //This will minus the Enemy's damage from the Character GameObject's p_currenthp
        damage = Mathf.Clamp(damage, 0, int.MaxValue); //This to make sure that damage is never less than 0
    }
    //The RecoverHealth method is used to update the p_currenthp in the script to the update value of currenthp.
    public void RecoverHealth(int currenthp, int basehp) //This script requires the currenthp and the base hp.
    {
        p_currenthp = currenthp; //This assigns the value of p_currenthp to the update value of currenthp.
    }
}

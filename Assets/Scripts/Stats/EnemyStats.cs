using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStats : MonoBehaviour {
    public string e_name; //This variable will hold the Enemy GameObject's name, assigned in the inspector.
    public float currentHealth { get; private set; } //This variable will hold the Enemy GameObject's current hp, it will start as the same as e_hp however will change when used in scripts e.g. TakeDamage.
    public Stat e_hp ;//This variable will hold the Enemy GameObject's maximum hp, assigned in the inspector.
    public Stat e_def; //This variable will hold the Enemy GameObject's defence, assigned in the inspector.
    public Stat e_atk; //This variable will hold the Enemy GameObject's attack, assigned in the inspector.
    public Stat e_spd; //This variable will hold the Enemy GameObject's speed, assigned in the inspector.
    public GameObject drop_item; //This variable will hold the Enemy GameObject's drop Item GameObject that is added to the Character GameObject's inventory if the user wins the battle, assigned in the inspector.
    public Image Healthbar; //This variable will hold the Enemy GameObject's Healthbar Image that will change in accordance to the currentHealth to give a visual representation of the amount of health the Enemy has left.
    private void Awake()
    {
        currentHealth = e_hp.BaseValue; //This will set current health to be equal to the base value of the e_hp Stat.
    }
    //The TakeDamage method is used to minus the int damage that is in the Character GameObject's p_atk component from the currentHealth of the Enemy GameObject.
    public void TakeDamage(float damage) //This script requires an float damage that is in the Character GameObject's p_atk component.
    {
        currentHealth -= damage; //This will minus the Character GameObject's p_atk from the Enemy GameObject's currentHealth
        Healthbar.fillAmount = (currentHealth / e_hp.BaseValue); //This will update the Healthbar to represent the new value of currentHealth to visually show the user they have done damage.
    }
}

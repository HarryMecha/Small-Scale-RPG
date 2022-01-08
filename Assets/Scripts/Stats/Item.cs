using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item:MonoBehaviour {
    public int ID; //This variable will hold the Item GameObject's ID, assigned in the inspector.
    public string Type; //This variable will hold the Item GameObject's Type, assigned in the inspector.
    public string Description; //This variable will hold the Item GameObject's Description, assigned in the inspector.
    public Sprite Icon; //This variable will hold the Item GameObject's Icon, assigned in the inspector.
    public bool IsHealing; //This variable will hold the boolean that will determine whether it is a healing item or not that will change it's effect in the UseItem script found in InventoryManager, assigned in the inspector.
    public int HealAmount; //This variable will hold the Item GameObject's HealAmount, assigned in the inspector.
    public int BuyAmount; //This variable will hold the Item GameObject's BuyAmount, assigned in the inspector.
    public int SellAmount; //This variable will hold the Item GameObject's SellAmount, assigned in the inspector.
}

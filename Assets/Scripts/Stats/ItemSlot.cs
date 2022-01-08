using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    public GameObject item; //This variable will hold the ItemGameObject, assigned in the inspector.
    public int ID; //This variable will hold the ItemSlot GameObject's ID, assigned in the inspector.
    public string type; //This variable will hold the ItemSlot GameObject's Type, assigned in the inspector.
    public string description; //This variable will hold the ItemSlot GameObject's Description, assigned in the inspector.
    public bool empty; //This variable will hold a boolean that will determine is the ItemSlot is empty, contains a placeholder, assigned in the inspector.
    public Sprite icon; //This variable will hold the ItemSlot GameObject's Icon, assigned in the inspector.
    public int BuyAmount; //This variable will hold the ItemSlot GameObject's BuyAmount, assigned in the inspector.
    public int SellAmount; //This variable will hold the ItemSlot GameObject's SellAmount, assigned in the inspector.
}

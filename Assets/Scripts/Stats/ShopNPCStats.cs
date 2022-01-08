using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopNPCStats : MonoBehaviour {
    public string ShopNpc_name; //This variable will hold the ShopNPC GameObject's name that will displayed when interacting with the NPC GameObject, assigned in the inspector.
    public string ShopNpc_text; //This variable will hold the ShopNPC GameObject's text that will displayed when interacting with the NPC GameObject, assigned in the inspector.
    public GameObject ShopItem1; //This variable will hold the ShopNPC GameObject's ShopItem GameObject that the user will be select from to buy, assigned in the inspector.
    public GameObject ShopItem2; //This variable will hold the ShopNPC GameObject's ShopItem GameObject that the user will be select from to buy, assigned in the inspector.
    public GameObject ShopItem3; //This variable will hold the ShopNPC GameObject's ShopItem GameObject that the user will be select from to buy, assigned in the inspector.
    public GameObject ShopItem4; //This variable will hold the ShopNPC GameObject's ShopItem GameObject that the user will be select from to buy, assigned in the inspector.
    public GameObject ShopItem5; //This variable will hold the ShopNPC GameObject's ShopItem GameObject that the user will be select from to buy, assigned in the inspector.
    public GameObject ShopItem6; //This variable will hold the ShopNPC GameObject's ShopItem GameObject that the user will be select from to buy, assigned in the inspector.
    public GameObject[] ItemArray; //This variable will hold the ShopNPC GameObject's GameObject Array that will be populated by the ShopItem GameObject's attached to the ShopNPC.
    private void Start()
    {
        ItemArray = new GameObject[6]; //This will set the ItemArray size to 6 enough to hold every ShopItem GameObject.
        ItemArray[0] = ShopItem1; //This will set the GameObject in position 0 of the array to the GameObject ShopItem1.
        ItemArray[1] = ShopItem2; //This will set the GameObject in position 1 of the array to the GameObject ShopItem2.
        ItemArray[2] = ShopItem3; //This will set the GameObject in position 2 of the array to the GameObject ShopItem3.
        ItemArray[3] = ShopItem4; //This will set the GameObject in position 3 of the array to the GameObject ShopItem4.
        ItemArray[4] = ShopItem5; //This will set the GameObject in position 4 of the array to the GameObject ShopItem5.
        ItemArray[5] = ShopItem6; //This will set the GameObject in position 5 of the array to the GameObject ShopItem6.
    }
}

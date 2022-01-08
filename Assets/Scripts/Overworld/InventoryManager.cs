using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public static int totalslots; //This variable holds the total number of slots that items can be added to.
    public GameObject SlotPanel; //This variable holds the GameObject ItemSlotPanel where the ItemSlots are held.
    public GameObject PlaceHolderPrefab; //This variable holds the prefab for the Placeholder Item, assigned in the inspector.
    public static Item Placeholder; //This variable holds the Item component attached to the PlaceHolderPrefab.
    public static bool ItemRecieved; //This variable is a boolean that determines whether the item has been recieved and has been put into the user's inventory.
    public static GameObject Icon; //This variable will contain the ItemSlot GameObject.
    public static Sprite IconSprite; //This variable will contain the sprite component attached to the Icon GameObject.
    public static GameObject[] slots; //This variable is an array of GameObjects that will temporarily hold the Items that will go into the user's inventory.
    public static Image IconImage; //This variable will contain the Image component attached to the Icon GameObject.
    private void Start()
    {
        SlotPanel = GameObject.Find("ItemSlotPanel"); //This assigns the SlotPanel to the GameObject ItemSlotPanel.
        totalslots = 18; //This assigns the total number of slots that items can be added to.
        Placeholder = PlaceHolderPrefab.GetComponent<Item>(); //This variable holds the Item component attached to the PlaceHolderPrefab.
        ItemRecieved = false; //The sets the boolean to false as the item is yet to be recieved.
        slots = new GameObject[totalslots]; //Sets the GameObject Array to have a size of the same size totalslots.
        InventorySetup(); //This calls the method InventorySetup which sets assigns slot of the inventory to hold a placeholder.
        DatabaseManager.InventoryLoader(Character.slot); //This will call the InventoryLoader method in DatabaseManager using the users inventory held in the Character script.
    }
    //The AddItem method is used to add an Item object attached to an Enemy GameObject into the user's inventory.
    public static void AddItem(Item obtainedItem) //This method requires the Item Object that is attached to the Enemy GameObject.
    {
        Debug.Log("AddItem");
        for (int i = 0; i < totalslots; i++) //This for loop goes from 0 to the total inventory slots to check which one is a placeholder.
        {
            if ((Character.slot[i].ID == 0) && (ItemRecieved == false)) //This if statement is used to check is the Item in the slot in the user's inventory is a placeholder and if they have already recieved the item.
            {
                Character.slot[i] = obtainedItem; //This replaces the PlaceHolder Item with the Item attached to the Enemy GameObject.
                ItemRecieved = true; //This sets the boolean to true showing that the item has been retrieved and is in the user's inventory.
            }
        }
        ItemRecieved = false; //This sets the boolean to false outside the for loop allowing other Items to be added.
    }
    //The DeleteItem method is used to delete an Item object from the user's inventory and then reorder the inventory so the items that arent placeholders are at the beginning.
    public static void DeleteItem(Item currentItem) //This method requires the Item Object that is atatched to the current inventory slot.
    {
        Debug.Log("DeleteItem");
        Character.slot[OptionsConfig.currentslot] = Placeholder; //This replaces the Item with a Placeholder Item.
        for (int i = 1; i < totalslots; i++) //This for loop goes from 0 to the total inventory slots to check which one is a placeholder so it can reorder the inventory.
        {
            if (Character.slot[i - 1].ID == 0) //This if statment only runs if the Item in the slot is a placeholder.
            {
                Character.slot[i - 1] = Character.slot[i]; //This sets the item that is the slot previous to the current slot.
                Character.slot[i] = Placeholder; //This sets the item in the current slot to a placeholder.
            }
        }
        IconSetup(Character.slot); //This calls the IconSetup method to make sure all the icons the user is seeing match the GameObjects in the slots.
    }
    //The UseItem method is used in conjunction with the DeleteItem Method allowing the user to use an item if it's IsHealing boolean in it's item class is true.
    public static void UseItem(Item ItemInUse) //This method requires the Item Object that is atatched to the current inventory slot.
    {
        Debug.Log("UseItem");
        if (ItemInUse.IsHealing == true) //This if statement only runs if it's IsHealing boolean in it's item class is true.
        {
            int CharacterCurrentHP = GameObject.Find("CharacterOverworld").GetComponent<CharacterStats>().p_currenthp; //This creates a new int that holds the user's currenthp that is held in the Character class.
            int CharacterBaseHP = GameObject.Find("CharacterOverworld").GetComponent<CharacterStats>().p_hp; //This creates a new int that holds the user's hp that is held in the Character class.
            CharacterCurrentHP = CharacterCurrentHP + ItemInUse.HealAmount; //This adds the Item's healing amount onto the Character GameObject's currenthp.
            if (CharacterCurrentHP > CharacterBaseHP) //This if statement only runs if the currenthp is greater than the basehp.
            {
                CharacterCurrentHP = CharacterBaseHP; //This sets the currenthp to be the same as the basehp
            }
            GameObject.Find("CharacterOverworld").GetComponent<CharacterStats>().RecoverHealth(CharacterCurrentHP, CharacterBaseHP); //This then calls the recoverhealth methods with the two hp variables.
        }
    }
    //The method InventorySetup is used at the beginning of the game when a user logins in to set the inventory full of placeholder Items to either be kept or replaced.
    public static void InventorySetup()
    {
        Debug.Log("InventorySetup");
        for (int i = 0; i < totalslots; i++) //This for loop goes from 0 to the total slots in order to set each one as a Placeholder Item
        {
            if (Character.InventoryASetup == false) //This if statement only runs if the Inventory has not already been setup defined by the InvetoryASetup boolean.
            {
                Character.slot[i] = Placeholder; //This sets the current inventory slot's item component to a Placeholder Item.
            }
        }
        Character.InventoryASetup = true; //Thi sets the boolean to true to ensure it isn't setup more than once which would replace all items the user may have obtained.
    }
    //This sets each of the ItemSlot components variables attached to the IconSlot to the icon in their Item component.
    public static void IconSetup(Item[] items) //The method requires the characters inventory which is stored in an array of items.
    {
        for (int i = 0; i < totalslots; i++) //This for loop goes through the inventory from 0 to the total slots chaning each ItemSlot's variables.
        {
            Icon = GameObject.Find("ItemSlot" + i); //Icon is set to the ItemSlot with location i.
            Icon.GetComponent<ItemSlot>().ID = items[i].ID; //This sets the ItemSlots ID to that of the Items.
            Icon.GetComponent<ItemSlot>().type = items[i].Type; //This sets the ItemSlots Type to that of the Items.
            Icon.GetComponent<ItemSlot>().description = items[i].Description; //This sets the ItemSlots Description to that of the Items.
            Icon.GetComponent<ItemSlot>().icon = items[i].Icon; //This sets the ItemSlots Icon to that of the Items.
            IconImage = Icon.GetComponent<Image>(); //This gets the image component of the Icon.
            IconImage = Icon.transform.GetChild(0).GetComponent<Image>(); //This gets the image component of the Icon.
            IconImage.sprite = Icon.GetComponent<ItemSlot>().icon; //This changes the sprite to the Icon to the IconImage
            slots[i] = Icon.gameObject; //This sets the InventorySlots Item to the GameObject the Item is a component of.
        }
    }
}

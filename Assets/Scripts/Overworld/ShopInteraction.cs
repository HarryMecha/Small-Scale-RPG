using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShopInteraction : MonoBehaviour
{
    public Camera MainCamera;//This variable holds the Character's camera that is a component in it's children and is turned on and off depending on whether the user is interacting with an NPC or not.
    public static bool ItemRecieved; //This variable is a boolean that determines whether the item has been recieved and has been put into the user's inventory.
    public GameObject PlaceHolderPrefab; //This variable holds the prefab for the Placeholder Item, assigned in the inspector.
    public static Item Placeholder; //This variable holds the Item component attached to the PlaceHolderPrefab.
    public static int EmptySlotCounter; //This variable holds the amount of slots in the ShopNPC's ItemSlots that contain the Placeholder Item.
    void Start()
    {
        MainCamera.GetComponent<Camera>().enabled = true; //This sets the user's perspective to the main camera by it being the only active camera.
        Placeholder = PlaceHolderPrefab.GetComponent<Item>(); //This variable holds the Item component attached to the PlaceHolderPrefab.
    }
    void OnTriggerStay(Collider other)//This method only actives when a GameObject stays in the Character GameObjects collider.
    {
        if (other.gameObject.tag == ("Shop")) //This if statement only runs if the GameObject within the Character's collider has the tag Shop.
        {
            if (Input.GetKeyDown(KeyCode.E)) //This if statement only runs if while there is a GameObject with tag Shop the E key is pressed.
            {
                MainCamera.GetComponent<Camera>().enabled = false; //This turns off the MainCamera in order for the user's perspective to be placed in front of the NPC by switching the cameras.
                other.GetComponentInChildren<Camera>().enabled = true; //This turns on the ShopNPC GameObject's Camera component in order for the user's perspective to be placed in front of the ShopNPC by switching the cameras.
                ShopConfig.currentPhase = ShopConfig.MenuPhase.MENU; //This will change currentPhases value in the ShopConfig script to MenuPhase.MENU and the switch will compensate for such changing the case it is running to MenuPhase.MENU.
                ShopConfig.ShopNameText.SetText(other.GetComponent<ShopNPCStats>().ShopNpc_name); //This will set the ShopNameText in the ShopConfig script to the ShopNpc_name string, found in the ShopNPC that is in the Character GameObjects collider's ShopNPCStats component.
                ShopConfig.ShopText.SetText(other.GetComponent<ShopNPCStats>().ShopNpc_text); //This will set the ShopText in the ShopConfig script to the ShopNpc_text string, found in the ShopNPC that is in the Character GameObjects collider's ShopNPCStats component.
                ItemMenuSetup(other.gameObject); //This will run the ItemMenuSetup script that will place the Item's in the ShopNPC's ItemSlot component into the ItemSlot components of the ShopItemList. 
                ShopItemListCursorSetup(); //This will run the CursorSetup script that will set up the ability to move the cursor along the ShopItemList.
            }
            if (ShopConfig.currentPhase == ShopConfig.MenuPhase.CLOSED) //This if statement only runs when the currentPhase in the ShopConfig script is set to MenuPhase.CLOSED, this is to ensure the camera returns to normal once the user closes the menu.
            {
                MainCamera.GetComponent<Camera>().enabled = true; //This sets the user's perspective to the main camera by it being the only active camera.
                other.GetComponentInChildren<Camera>().enabled = false; //This turns off the ShopNPC GameObject's Camera component in order for the user's perspective back behind the Character GameObject.
            }
        }
    }
    void OnTriggerExit(Collider other) //This method only actives when a GameObject exits in the Character GameObjects collider.
    {
        if (other.gameObject.tag == ("Shop")) //This if statement only runs if the GameObject within the Character's collider has the tag Shop.
        {
            MainCamera.GetComponent<Camera>().enabled = true; //This sets the user's perspective to the main camera by it being the only active camera.
            other.GetComponentInChildren<Camera>().enabled = false; //This turns off the ShopNPC GameObject's Camera component in order for the user's perspective back behind the Character GameObject.
            ShopConfig.currentPhase = ShopConfig.MenuPhase.CLOSED; //This will change currentPhases value in the ShopConfig script to MenuPhase.CLOSED and the switch will compensate for such changing the case it is running to MenuPhase.CLOSED.
        }
    }
    //The BuyItem script takes the Item that is currently in the selected in the ShopItemList and adds it to the CharacterGameObjects slot Item Array and deducts the Character GameObjects money by the Items BuyAmount.
    public static void BuyItem(Item obtainedItem) //This method requires the Item Object that is attached to the selected ShopItemSlot GameObject.
    {
        if (obtainedItem.BuyAmount < Character.money) //This if statement only runs when the BuyAmount of the selected ShopItemSlot GameObject's Item component is greater than the Character GameObject's money, this is to prevent items from being bought without having a sufficient amount of money.
        {
            for (int i = 0; i < InventoryManager.totalslots; i++)  //This for loop goes from 0 to the total inventory slots to check which one is a placeholder.
            {
                if ((Character.slot[i].ID == 0) && (ItemRecieved == false)) //This if statement is used to check is the Item in the slot in the user's inventory is a placeholder and if they have already recieved the item.
                {
                    Character.slot[i] = obtainedItem; //This replaces the PlaceHolder Item with the Item attached to the ShopItemSlot GameObject.
                    ItemRecieved = true; //This sets the boolean to true showing that the item has been retrieved and is in the user's inventory.
                    Character.money = Character.money - obtainedItem.BuyAmount; //This minuses the BuyAmount of the selected ShopItemSlot GameObject's Item component from the Character GameObject's money.
                    ShopConfig.ShopText.SetText("Thank you for your purchase"); //This sets the ShopText to a message visually showing the user they have purchased the item and it has successfully been added to the Character GameObjects Inventory.
                    ShopConfig.ShopCharacterMoney.gameObject.transform.Find("ShopCharacterMoneyValue").GetComponent<TextMeshProUGUI>().SetText(Character.money.ToString("R")); //This updates the ShopCharacterMoneyValue GameObject to the new amount of money after the purchase.
                }
            }
            ItemRecieved = false; //This sets the boolean to false outside the for loop allowing other Items to be added.
        }
        else
        {
            ShopConfig.ShopText.SetText("You don't have sufficient money for this purchase"); //This sets the ShopText to a message visually showing the user they cannot purchase the item as the Character GameObjects money is less than the Items BuyAmount.
        }
    }
    //The ItemMenuSetup script is used to set up all of ItemSlot components of the ShopItemSlot.
    public static void ItemMenuSetup(GameObject ShopNPC) //The method requires the GameObject ShopNPC so it can access the Items in the ShopNPCStats component attached to it. 
    {
        ShopConfig.ShopCharacterMoney.gameObject.transform.Find("ShopCharacterMoneyValue").GetComponent<TextMeshProUGUI>().SetText(Character.money.ToString("R"));  //This sets the ShopCharacterMoneyValue GameObject's text component to the Character GameObject's money.
        GameObject[] ShopItems = ShopNPC.GetComponent<ShopNPCStats>().ItemArray; //This creates a GameObject array and assigns it's value to ItemArray held in the ShopNPC's ShopNPCStats component.
        for (int i = 0; i < 6; i++) //This for loop goes through the inventory from 0 to 6 chaning each ShopItemSlot's variables.
        {
            GameObject ShopItemSlot = ShopConfig.ShopItemList.gameObject.transform.Find("ShopItemSlot" + (i + 1)).gameObject; //This assigns the value of the ShopItemSlot GameObject to the be ShopItemSlot that reflects the current number the for loop is on,i.
            ShopItemSlot.GetComponent<ItemSlot>().item = ShopItems[i];
            ShopItemSlot.GetComponent<ItemSlot>().ID = ShopItems[i].GetComponent<Item>().ID;
            if (ShopItemSlot.GetComponent<ItemSlot>().ID != 0) //This if statement only runs if the ID of the added Item is 0 meaning it's a placeholder and cannot be selected in the ShopListMenu.
            {
                EmptySlotCounter = EmptySlotCounter++; //This adds 1 to the EmptySlotCounter indicating another slot the user cannot select to buy.
            }
            ShopItemSlot.GetComponent<ItemSlot>().type = ShopItems[i].GetComponent<Item>().Type; //This sets the ShopItemSlots Type to that of the ShopNPC's Items.
            ShopItemSlot.GetComponent<ItemSlot>().description = ShopItems[i].GetComponent<Item>().Description; //This sets the ShopItemSlots Description to that of the ShopNPC's Items.
            ShopItemSlot.GetComponent<ItemSlot>().icon = ShopItems[i].GetComponent<Item>().Icon; //This sets the ShopItemSlots Icon to that of the ShopNPC's Items.
            ShopItemSlot.GetComponent<TextMeshProUGUI>().SetText(ShopItemSlot.GetComponent<ItemSlot>().type); //This sets the text on the ShopListPanel to the ShopNpc'S Item Type.
            ShopItemSlot.GetComponent<ItemSlot>().BuyAmount = ShopItems[i].GetComponent<Item>().BuyAmount; //This sets the ShopItemSlots BuyAmount to that of the ShopNPC's Items.
            ShopItemSlot.GetComponent<ItemSlot>().SellAmount = ShopItems[i].GetComponent<Item>().SellAmount; //This sets the ShopItemSlots SellAmount to that of the ShopNPC's Items.
        }
    }
    //The ShopItemCursorSetup class sets up how the user will be able to select through the ShopItemSlots.
    public static void ShopItemListCursorSetup()
    {
        for (int i = 0; i < 7; i++) //This is for loop is between 0 and 7, the available ShopItemSlots.
        {
            GameObject ShopItemSlot = ShopConfig.ShopItemList.gameObject.transform.Find("ShopItemSlot" + (i + 1)).gameObject; //This creates and assigns the GameObject ShopItemSlot to the ShopItemSlot that reflects the i value.
            ShopItemSlot.SetActive(true); //This will set the ShopItemSlot GameObject to active so it will be visible to the user and scripts attached to it will run.
            GameObject ShopItemSlotOutline = ShopItemSlot.gameObject.transform.Find("ShopItemSlot" + (i + 1) + "Outline").gameObject; //This creates and assigns the GameObject ShopItemSlotOutline to the ShopItemSlotOutline that reflects the i value.
            ShopItemSlotOutline.SetActive(true); //This will set the ShopItemSlotOutline GameObject to active so it will be visible to the user and scripts attached to it will run.
            if (ShopConfig.ItemListCursorPosition != i) //This if statement only runs if the CursorPosition is not equal to the, it will deactive any ShopItemSlotOutline other than i.
            {
                ShopItemSlotOutline.SetActive(false); //This will set the ShopItemSlotOutline GameObject to inactive so it will be not visible to the user and no scripts attached to it will run.
            }
        }
    }
    //The IconPreviewSetup class set up the IconPreviewPanel, allowing the user to see the items information before buying.
    public static void ItemPreviewSetup()
    {
        GameObject ItemPreviewImage = ShopConfig.ItemPreviewPanel.gameObject.transform.Find("ItemPreviewImage").gameObject; //This creates and assigns a new GameObject, ItemPreviewImage, to the ItemPreviewImage GameObject in the ItemPreviewPanel.
        Image ItemSlotImage = ItemPreviewImage.gameObject.transform.Find("ItemSlotImage").gameObject.GetComponent<Image>(); //This creates and assigns a new Item variable, ItemSlotImage, to the ItemSlotImage GameObject in the ItemPreviewImage.
        GameObject ShopItemSlot = ShopConfig.ShopItemList.gameObject.transform.Find("ShopItemSlot" + (ShopConfig.ItemListCursorPosition + 1)).gameObject; //This creates and assigns a new GameObject, ShopItemSlot, to the currently selected ShopItemSlot GameObject that is determined by the position of the ItemListCursor.
        GameObject ItemPreviewPrice = ShopConfig.ItemPreviewPanel.gameObject.transform.Find("ItemPreviewPrice").gameObject; //This creates and assigns a new GameObject, ItemPreviewPrice, to the ItemPreviewPrice GameObject in the ItemPreviewPanel.
        ShopConfig.ItemPreviewPanel.gameObject.transform.Find("ItemPreviewName").gameObject.GetComponent<TextMeshProUGUI>().SetText(ShopItemSlot.GetComponent<ItemSlot>().type); //This sets the text of the ItemPreviewName GameObject in the ItemPreviewPanel to the Type variable of the ItemSlot component attached to the currently selected ShopItemSlot.
        ShopConfig.ItemPreviewPanel.gameObject.transform.Find("ItemPreviewDescription").gameObject.GetComponent<TextMeshProUGUI>().SetText(ShopItemSlot.GetComponent<ItemSlot>().description);  //This sets the text of the ItemPreviewDescription GameObject in the ItemPreviewPanel to the description variable of the ItemSlot component attached to the currently selected ShopItemSlot.
        ItemPreviewPrice.gameObject.transform.Find("ItemMoneyValue").gameObject.GetComponent<TextMeshProUGUI>().SetText(ShopItemSlot.GetComponent<ItemSlot>().BuyAmount.ToString()); //This sets the text of the ItemMoneyValue GameObject in the ItemPreviewPrice to the BuyAmount variable of the ItemSlot component attached to the currently selected ShopItemSlot.
        ItemSlotImage.sprite = ShopItemSlot.GetComponent<ItemSlot>().icon; //This sets the sprite component of the ItemSlotImage to the icon variable of the ItemSlot component attached to the currently selecte    }
    }
}

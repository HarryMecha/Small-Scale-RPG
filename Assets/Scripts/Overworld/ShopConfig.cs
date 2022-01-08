using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ShopConfig : MonoBehaviour
{
    [SerializeField]
    public enum MenuPhase //This is an enumerator list that contains named constants used for the menuphases allowing for transitions between these phases.
    {
        CLOSED,
        MENU,
        BUY,
        SELL,
    }
    public static MenuPhase currentPhase; //This MenuPhase variable is used to change the switch which inturn changes which item on the enumerator list is active and which case to perform depending on the value of the switch.
    int CursorPosition; //This is a variable that holds which position the Cursor is on relative the Shop menu.
    public static int ItemListCursorPosition; //This is a variable that holds which position the ItemListCursor is on relative the ItemList menu.
    private GameObject ShopMenu; //This variable holds the GameObject ShopMenu that will open when the user presses the interact key, E, when a GameObject with the tag Shop is in the Character GameObjects collider.
    public static GameObject ShopItemList; //This variable holds the GameObject ShopItemList that contains children GameObjects with ItemSlot components that will be set by the ShopNPCStats component on the ShopNPC.
    public static GameObject ItemPreviewPanel; //This variable holds the GameObject ItemPreviewPanel which visibly shows the users the Item's held in ShopItemList's Types, Descriptions, Icons and Prices.
    private GameObject ShopPanelCursor; //This is a variable that is assigned to the OptionsPanelCursor GameObject.
    public static GameObject ShopTextPanel; //This is a variable that is assigned to the ShopTextPanel GameObject.
    public static TextMeshProUGUI ShopText; //This is a variable that is assigned to the ShopTextPanel GameObject's TextMeshProUGUI component.
    public static GameObject ShopNameTextPanel; //This is a variable that is assigned to the ShopNameTextPanel GameObject.
    public static TextMeshProUGUI ShopNameText; //This is a variable that is assigned to the ShopNameTextPanel GameObject's TextMeshProUGUI component.
    public static GameObject ShopItemPanel; //This is a variable that is assigned to the ShopItemPanel GameObject.
    public static GameObject ShopCharacterMoney; //This is a variable that is assigned to the ShopCharacterPanel GameObject.
    public static bool IsInteracting; //This variable is a boolean that determines whether the user has pressed the Interact key, E, whilst in an GameObject with the tag Shop is in the Character GameObjects collider.
    public static int currentslot; //This is an int that will be assigned to the current slot number the user has selected when traversing the shop menu.
    void Start()
    {
        ShopItemList = GameObject.Find("ShopItemList"); //This assigns the variable to the GameObject ShopItemList.
        ShopItemList.SetActive(true); //This will set the ShopItemList GameObject to active so it will be visible to the user and scripts attached to it will run.
        ItemPreviewPanel = GameObject.Find("ItemPreviewPanel"); //This assigns the variable to the GameObject ItemPreviewPanel.
        ItemPreviewPanel.SetActive(true); //This will set the ItemPreviewPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        ShopPanelCursor = GameObject.Find("ShopPanelCursor"); //This assigns the variable to the GameObject ShopItemList.
        ShopPanelCursor.SetActive(true); //This will set the ShopPanelCursor GameObject to active so it will be visible to the user and scripts attached to it will run.
        ShopMenu = GameObject.Find("ShopMenu"); //This assigns the variable to the GameObject ShopMenu.
        ShopMenu.SetActive(false); //This will set the ShopMenu GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        ShopCharacterMoney = ShopMenu.gameObject.transform.Find("ShopCharacterMoney").gameObject; //This assigns the variable to the GameObject ShopCharacterMoney.
        ShopCharacterMoney.SetActive(true); //This will set the ShopCharacterMoney GameObject to active so it will be visible to the user and scripts attached to it will run.
        ShopTextPanel = ShopMenu.gameObject.transform.Find("ShopMenuPanel").gameObject; //This assigns the variable to the GameObject ShopMenuPanel.
        ShopText = ShopTextPanel.GetComponentInChildren<TextMeshProUGUI>(); //This will assign the variable to the TextMeshProUGUI component of the ShopTextPanel.
        ShopNameTextPanel = ShopMenu.gameObject.transform.Find("ShopNamePanel").gameObject; //This assigns the variable to the GameObject ShopNamePanel.
        ShopNameText = ShopNameTextPanel.GetComponentInChildren<TextMeshProUGUI>(); //This will assign the variable to the TextMeshProUGUI component of the ShopNameTextPanel.
        currentslot = 0; //Assigns the int to 0 as no selection has been made.
        currentPhase = MenuPhase.CLOSED; //This will set the starting switch to MenuPhase.CLOSED and run the associated case.
    }
    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //This retrieves the name of the scene.
        string SceneName = currentScene.name; //This assigns the SceneName string the name of the current scene.
        switch (currentPhase) //This controls the switch that will run the case that is equal to the value of currentPhase.
        {
            case (MenuPhase.CLOSED): //This when run when currentPhase is equal to MenuPhase.CLOSED.
                for (int i = 0; i < (CursorPosition); i++) //This is a for loop that goes from 0 to the CursorPosition and is used to return the ShopPanelCursor to it's original position.
                {
                    ShopPanelCursor.transform.Translate(60f, 0.0f, 0.0f); //This will move the ShopPanelCursor up 60 in the x axis to put it back to it's original position.
                }
                CursorPosition = 0; //Sets the CursorPosition back to 0 which is the first selectable option.
                IsInteracting = false; //This will set IsInteracting to false as the user is no longer pressing the Interact Key, E, whilst an ShopNPC is in the Character GameObject's collider.
                break;
            case (MenuPhase.MENU): //This will change currentPhases value to MenuPhase.MENU and the switch will compensate for such changing the case it is running to MenuPhase.MENU.
                ShopMenu.SetActive(true); //This will set the ShopMenu GameObject to active so it will be visible to the user and scripts attached to it will run.
                ShopItemList.SetActive(false); //This will set the ShopItemList GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
                ItemPreviewPanel.SetActive(false); //This will set the ItemPreviewPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
                IsInteracting = true; //This will set IsInteracting to true as the user is pressing the Interact Key, E, whilst an ShopNPC is in the Character GameObject's collider.
                if (Input.GetKeyDown(KeyCode.S)) //This if statement will only run if the user presses the S key showing they want to move the cursor down.
                {
                    CursorPosition = CursorPosition + 1; //This will add 1 to the CursorPosition showing it has moved down a position and to correspond to script need to be run when the user presses the return key.
                    ShopPanelCursor.transform.Translate(-60f, 0.0f, 0.0f); //This will move the OptionsPanelCursor down 60 in the x axis to visually show to the user the options they can select.
                    if (CursorPosition > 2) //This if statement will only run if the CursorPosition is over 2 which is out of bounds and so it will need to be moved back to an option on the menu.
                    {
                        ShopPanelCursor.transform.Translate(60f, 0.0f, 0.0f); //This will move the OptionsPanelCursor up 60 in the x axis to put it back onto the list of options that visually show the user the options they can select.
                        CursorPosition = 2; //Sets the CursorPosition back to 2 which is the last selectable option.
                    }
                }
                if (Input.GetKeyDown(KeyCode.W)) //This if statement will only run if the user presses the W key showing they want to move the cursor up.
                {
                    CursorPosition = CursorPosition - 1; //This will minus 1 to the CursorPosition showing it has moved up a position and to correspond to script need to be run when the user presses the return key.
                    ShopPanelCursor.transform.Translate(60f, 0.0f, 0.0f); //This will move the OptionsPanelCursor up 60 in the x axis to visually show to the user the options they can select.
                    if (CursorPosition < 0) //This if statement will only run if the CursorPosition is less than 0 which is out of bounds and so it will need to be moved back to an option on the menu.
                    {
                        ShopPanelCursor.transform.Translate(-60f, 0.0f, 0.0f); //This will move the OptionsPanelCursor down 60 in the x axis to put it back onto the list of options that visually show the user the options they can select.
                        CursorPosition = 0; //Sets the CursorPosition back to 0 which is the first selectable option.
                    }
                }
                if (Input.GetKeyDown(KeyCode.Return)) //This if statement will only run when return is pressed showing the user wants to select and option which will depend on where the cursor position is.
                {
                    if (CursorPosition == 0 || CursorPosition == -1) //This if statement will only run when the CursorPosition is either 0 or an underflow value of -1 which is used to prevent errors, this showing the user wants to select Buy and access the ShopItemList and ItemPreviewPanel.
                    {
                        currentslot = 0; //This sets the currentslot back ot it's original position.
                        currentPhase = MenuPhase.BUY; //This will change currentPhases value to MenuPhase.BUY and the switch will compensate for such changing the case it is running to MenuPhase.BUY.
                    }
                    if (CursorPosition == 2 || CursorPosition == 3) //This if statement will only run when the CursorPosition is either 2 or an overflow value of 3 which is used to prevent errors, this showing the user wants to select Exit and close the ShopMenuPanel.
                    {
                        currentslot = 0; //This sets the currentslot back ot it's original position.
                        currentPhase = MenuPhase.CLOSED; //This will change currentPhases value to MenuPhase.CLOSE and the switch will compensate for such changing the case it is running to MenuPhase.CLOSE.
                        ShopMenu.SetActive(false); //This will set the ShopMenu GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
                    }
                }
                break; //This is used to terminate the current case so it can be switched.
            case (MenuPhase.BUY): //This when run when currentPhase is equal to MenuPhase.BUY.
                ShopInteraction.ItemPreviewSetup(); //This runs the ItemPreviewSetup method in the ShopInteraction script, this will apply all the Icons, Types and Descriptions from the ItemSlot Item components to the GameObjects in the ItemPreviewPanel and ShopItemList.
                ShopItemList.SetActive(true); //This will set the ShopItemList GameObject to active so it will be visible to the user and scripts attached to it will run.
                ItemPreviewPanel.SetActive(true); //This will set the ItemPreviewPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
                if (Input.GetKeyDown(KeyCode.S)) //This if statement will only run if the user presses the S key showing they want to move their item selection down.
                {
                    if (ItemListCursorPosition < ShopInteraction.EmptySlotCounter) { } //This if statement only runs if the ItemListCursorPosition is less than the EmptySlotCounter located in the ShopInteraction script, this is to prevent the user from trying to access an item below that doesn't exist.
                    else
                    { 
                        ItemListCursorPosition++; //This will add 1 to the ItemListCursorPosition showing it has moved down a position and to correspond to Item that the user want to buy.
                        ShopInteraction.ItemPreviewSetup(); //This runs the ItemPreviewSetup method in the ShopInteraction script, this is used to update the Icons, Types and Descriptions of the ItemPreviewList GameObject to reflect the new selected Item.
                        ShopInteraction.ShopItemListCursorSetup(); //This runs the CursorSetup method in the ShopInteraction script, this is used to update the ShopPanelCursor to reflect the new selected Item.
                    }
                }
                if (Input.GetKeyDown(KeyCode.W)) //This if statement will only run if the user presses the W key showing they want to move their item selection up.
                {
                    if (ItemListCursorPosition < 1) { }//This if statement only runs if the ItemListCursorPosition is less than the 1, this is to prevent the user from trying to access an item above that doesn't exist.
                    else
                    { 
                        ItemListCursorPosition--; //This will minus 1 to the ItemListCursorPosition showing it has moved up a position and to correspond to Item that the user want to buy.
                        ShopInteraction.ItemPreviewSetup(); //This runs the ItemPreviewSetup method in the ShopInteraction script, this is used to update the Icons, Types and Descriptions of the ItemPreviewList GameObject to reflect the new selected Item.
                        ShopInteraction.ShopItemListCursorSetup(); //This runs the CursorSetup method in the ShopInteraction script, this is used to update the ShopPanelCursor to reflect the new selected Item.
                    }
                }
                if (Input.GetKeyDown(KeyCode.Return)) //This if statement will only run when the user has pressed return showing that they want to buy the item at the ItemListCursorPosition.
                {
                    ShopInteraction.BuyItem(GameObject.Find(("ShopItemSlot") + (ItemListCursorPosition + 1)).GetComponent<ItemSlot>().item.GetComponent<Item>()); //This will run the BuyItem script in ShopInteraction using the ShopItemSlot at the ItemListCursorPosition's Item in it's ItemSlot component.
                }
                if (Input.GetKeyDown(KeyCode.Escape)) //This if statement will only run when the user has pressed escape showing that they want to exit the buying menu.
                {
                    ShopItemList.SetActive(false); //This will set the ShopItemList GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
                    ItemPreviewPanel.SetActive(false); //This will set the ItemPreviewPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
                    currentPhase = MenuPhase.MENU; //This will change currentPhases value to MenuPhase.MENU and the switch will compensate for such changing the case it is running to MenuPhase.MENU.
                }
                break; //This is used to terminate the current case so it can be switched.
        }
    }
}

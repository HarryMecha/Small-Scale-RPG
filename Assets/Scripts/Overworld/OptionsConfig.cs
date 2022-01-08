using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class OptionsConfig : MonoBehaviour
{
    public enum MenuPhase //This is an enumerator list that contains named constants used for the menuphases allowing for transitions between these phases.
    {
        CLOSED,
        MENU,
        INVENTORY,
        SAVE
    }
    public MenuPhase currentPhase; //This MenuPhase variable is used to change the switch which inturn changes which item on the enumerator list is active and which case to perform depending on the value of the switch.
    int CursorPosition; //This is a variable that holds which position the Cursor is on relative the Options menu.
    private static GameObject OptionsPanelCursor; //This is a variable that is assigned to the OptionsPanelCursor GameObject.
    public static GameObject PauseMenu; //This a variable that is assigned to the PauseMenu that can be turned on and off to show or hide it from the user.
    public static GameObject InventoryMenu; //This a variable that is assigned to the InventoryMenu that can be turned on and off to show or hide it from the user.
    public static GameObject DarkerBackground; //This a variable that is assigned to the DarkerBackground GameObject, which makes the bakcground reflect the menu being open by darkening the background bringing the users focus to the menu.
    public static bool PauseMenuOpen; //This is a boolean that is determined by whether the PauseMenu is open or closed.
    public Sprite IconSprite; //This variable will contain the sprite component attached to the Icon GameObject.
    public Image IconImage; //This variable will contain the Image component attached to the Icon GameObject.
    public GameObject Icon; //This variable will contain the ItemSlot GameObject.;
    private Item[] slot; //This is an Item array that will be assign as a path to the Item array that represents the inventory held in the Character class.
    public static int currentslot; //This is an int that will be assigned to the current slot number the user has selected when traversing the inventory menu.
    public GameObject ItemNameText; //This variable will contain the GameObject ItemNameText found in the Inventory Panel.
    void Start()
    {
        OptionsPanelCursor = GameObject.Find("OptionsPanelCursor"); //This assigns the OptionsPanelCursor to the GameObject OptionsPanelCursor found in Inventory Menu.
        InventoryMenu = GameObject.Find("InventoryMenu"); //This assigns the InventoryMenu to the InventoryMenu GameObject.
        PauseMenu = GameObject.Find("PauseMenu");  //This assigns the PauseMenu to the PauseMenu GameObject.
        DarkerBackground = GameObject.Find("DarkerBackground"); //This assigns the DarkerBackground to the DarkerBackground GameObject.
        PauseMenu.SetActive(false); //This will set the PauseMenu GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        InventoryMenu.SetActive(false); //This will set the InventoryMenu GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        PauseMenuOpen = false; //This will set the boolean to false showing the PauseMenu is closed on start.
        DarkerBackground.SetActive(false); //This will set the DarkerBackground GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        ItemNameText = GameObject.Find("ItemNameText"); //This assigns the ItemNameText to the GameObject ItemNameText found in Inventory Menu.
        currentPhase = MenuPhase.CLOSED; //This will set the starting switch to MenuPhase.CLOSED and run the associated case.
        slot = Character.slot; //This will assign the Item array to the Item Array slot found in the Character Script.
        currentslot = 0; //Assigns the int to 0 as no selection has been made.
    }
    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //This retrieves the name of the scene.
        string SceneName = currentScene.name; //This assigns the SceneName string the name of the current scene.
        switch (currentPhase) //This controls the switch that will run the case that is equal to the value of currentPhase.
        {
            case (MenuPhase.CLOSED):
                if (ShopConfig.IsInteracting == false) //This if statement will only run when the boolean IsInteracting found in ShopConfig is false, implying the user has not pressed the Interact Key, E, whilst a GameObject with the tag NPC is in the Character GameObjects collider.
                {
                    if (Input.GetKeyDown(KeyCode.Escape)) //This if statment will only run when the user presses the escape key meaning they want to open the menu.
                    {
                        PauseMenu.SetActive(true); //This will set the PauseMenu GameObject to active so it will be visible to the user and scripts attached to it will run.
                        DarkerBackground.SetActive(true); //This will set the DarkerBackground GameObject to active so it will be visible to the user and scripts attached to it will run.
                        PauseMenuOpen = true; //This will set the boolean to true showing the PauseMenu is open.
                        currentPhase = MenuPhase.MENU; //This will change currentPhases value to MenuPhase.MENU and the switch will compensate for such changing the case it is running to MenuPhase.MENU.
                    }
                }
                break; //This is used to terminate the current case so it can be switched.
            case (MenuPhase.MENU): //This will run when currentPhase is equal to MenuPhase.MENU.
                if ((Input.GetKeyDown(KeyCode.Escape)) && (SceneName != ("Battle"))) //This if statement will only run when the user pressed the escape key and the scene is not in Battle, this is to prevent errors occuring if the user presses escape while in the battle scene. 
                {
                    if (PauseMenuOpen == true) //This if statement only runs when the boolean is true showing that the PauseMenu is open.
                    {
                        PauseMenu.SetActive(false); //This will set the PauseMenu GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
                        DarkerBackground.SetActive(false); //This will set the DarkerBackground GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
                        PauseMenuOpen = false; //This will set the boolean to false showing the PauseMenu is closed on start.
                        currentPhase = MenuPhase.CLOSED; //This will change currentPhases value to MenuPhase.CLOSED and the switch will compensate for such changing the case it is running to MenuPhase.CLOSED.
                    }
                }
                GameObject.Find("LocationName").GetComponent<TextMeshProUGUI>().SetText(currentScene.name); //This will find the GameObject LocationName located in the PauseMenu and change it's TextMeshProUGUI component to the name of the currentscene.
                GameObject.Find("MoneyValue").GetComponent<TextMeshProUGUI>().SetText(Character.money.ToString("R")); //This will find the GameObject MoneyValue located in the PauseMenu and change it's TextMeshProUGUI component to the value of money in the Character script.
                if (Input.GetKeyDown(KeyCode.S)) //This if statement will only run if the user presses the S key showing they want to move the cursor down.
                {
                    CursorPosition = CursorPosition + 1; //This will add 1 to the CursorPosition showing it has moved down a position and to correspond to script need to be run when the user presses the return key.
                    OptionsPanelCursor.transform.Translate(-110f, 0.0f, 0.0f); //This will move the OptionsPanelCursor down 110 in the x axis to visually show to the user the options they can select.
                    if (CursorPosition > 3) //This if statement will only run if the CursorPosition is over 3 which is out of bounds and so it will need to be moved back to an option on the menu.
                    {
                        OptionsPanelCursor.transform.Translate(110f, 0.0f, 0.0f); //This will move the OptionsPanelCursor up 110 in the x axis to put it back onto the list of options that visually show the user the options they can select.
                        CursorPosition = 3; //Sets the CursorPosition back to 3 which is the last selectable option.
                    }
                }
                if (Input.GetKeyDown(KeyCode.W)) //This if statement will only run if the user presses the W key showing they want to move the cursor up.
                {
                    CursorPosition = CursorPosition - 1; //This will minus 1 to the CursorPosition showing it has moved up a position and to correspond to script need to be run when the user presses the return key.
                    OptionsPanelCursor.transform.Translate(110f, 0.0f, 0.0f); //This will move the OptionsPanelCursor up 110 in the x axis to visually show to the user the options they can select.
                    if (CursorPosition < 0) //This if statement will only run if the CursorPosition is less than 0 which is out of bounds and so it will need to be moved back to an option on the menu.
                    {
                        OptionsPanelCursor.transform.Translate(-110f, 0.0f, 0.0f); //This will move the OptionsPanelCursor down 110 in the x axis to put it back onto the list of options that visually show the user the options they can select.
                        CursorPosition = 0; //Sets the CursorPosition back to 0 which is the first selectable option.
                    }
                }
                if (Input.GetKeyDown(KeyCode.Return)) //This if statement will only run when return is pressed showing the user wants to select and option which will depend on where the cursor position is.
                {
                    if (CursorPosition == 0 || CursorPosition == -1) //This if statement will only run when the CursorPosition is either 0 or an underflow value of -1 which is used to prevent errors, this showing the user wants to select Inventory and access the inventory menu.
                    {
                        currentslot = 0; //This sets the currentslot back ot it's original position.
                        currentPhase = MenuPhase.INVENTORY; //This will change currentPhases value to MenuPhase.INVENTORY and the switch will compensate for such changing the case it is running to MenuPhase.INVENTORY.
                    }
                    if (CursorPosition == 2) //This if statement will only run when the CursorPosition is 2 this showing the user wants to select Save and save the game.
                    {
                        currentslot = 0; //This sets the currentslot back ot it's original position.
                        currentPhase = MenuPhase.SAVE; //This will change currentPhases value to MenuPhase.SAVE and the switch will compensate for such changing the case it is running to MenuPhase.SAVE.
                    }
                    if (CursorPosition == 3) //This if statement will only run when the CursorPosition is 3 this showing the user wants to select Quit and quit the game.
                    {
                        Application.Quit(); //This will close the application as the user has chosen to quit the game.
                    }
                }
                break; //This is used to terminate the current case so it can be switched.
            case (MenuPhase.INVENTORY): //This will run when currentPhase is equal to MenuPhase.MENU.
                InventoryMenu.SetActive(true); //This will set the InventoryMenu GameObject to active so it will be visible to the user and scripts attached to it will run.
                PauseMenu.SetActive(false); //This will set the PauseMenu GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
                InventoryManager.IconSetup(Character.slot); //This will run the IconSetup method in the InventoryManager script so that the user has a visible representation of the items in their inventory.
                InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(176, 176, 176, 255); //This changes the colour of the currently selected inventory slot's border to be grey, giving a visual representation of which slot is currently selected to the user.
                GameObject.Find("ItemNameText").GetComponent<TextMeshProUGUI>().SetText(Character.slot[currentslot].Type); //This will find the GameObject ItemNameText and change it's TextMeshProUGUI component's text to the Type variable of the item that is held in the current inventory slot.
                GameObject.Find("ItemDescriptionText").GetComponent<TextMeshProUGUI>().SetText(Character.slot[currentslot].Description); //This will find the GameObject ItemDescriptionText and change it's TextMeshProUGUI component's text to the Description variable of the item that is held in the current inventory slot.
                if (Input.GetKeyDown(KeyCode.Escape)) //This if statement will only run when the user pressed the escape key showing they want to close the inventory. 
                {
                    InventoryMenu.SetActive(false); //This will set the InventoryMenu GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
                    PauseMenu.SetActive(true); //This will set the PauseMenu GameObject to active so it will be visible to the user and scripts attached to it will run.
                    currentPhase = MenuPhase.MENU; //This will change currentPhases value to MenuPhase.MENU and the switch will compensate for such changing the case it is running to MenuPhase.MENU.
                }
                if (Input.GetKeyDown(KeyCode.W)) //This if statement will only run if the user presses the W key showing they want to move to the inventory slot above.
                {
                    if (0 <= currentslot && currentslot <= 17) //This if statment only runs if the currentslot is between 0 and 17 showing that it is in the range of accessible inventory slots.
                    {
                        if (currentslot > 8) //This if statement only runs if the currentslot is greater than 8, to prevent the user from trying to access a slot above that doesn't exist. 
                        {
                            InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(255, 255, 255, 255); //This changes the colour of the currently selected inventory slot's border to be white, showing the user that this slot isn't selected.
                            currentslot = currentslot - 9; //This will minus 9 from the value of currentslot setting it back to a value from the top row.
                            if (currentslot < 0) //This if statement will only run if currentslot is less than 0 implying the W key has been pressed on the top row, this used to prevent an error of there being no slot there by preventing the user from selecting a slot above while on the top row.
                            {
                                currentslot = currentslot + 9; //This will add 9 to the value of currentslot setting it back to the value before W was pressed.
                            }
                            InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(176, 176, 176, 255); //This changes the colour of the currently selected inventory slot's border to be grey, giving a visual representation of which slot is currently selected to the user.
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.A)) //This if statement will only run if the user presses the A key showing they want to move to the inventory slot to the left.
                {
                    if (0 <= currentslot && currentslot <= 17) //This if statment only runs if the currentslot is between 0 and 17 showing that it is in the range of accessible inventory slots.
                    {
                        if (currentslot != 0 || currentslot != 9) //This if statement only runs if the current slot is not 0 or 9, this preventing the user from moving to another slot if the selected slot is at the beginning of a row.
                        {
                            InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(255, 255, 255, 255); //This changes the colour of the currently selected inventory slot's border to be white, showing the user that this slot isn't selected.
                            currentslot = currentslot - 1; //This minuses 1 from the currentslot showing the new selected slot is to the left. 
                            if (currentslot < 0) //This if statement will only run if currentslot is less than 0 implying the A key has been pressed at the beginning of the top row, this used to prevent an error of there being no slot to move to by preventing the user from pressing the A key while at the beginning of the top row.
                            {
                                currentslot = currentslot + 1; //This will add 1 to the value of currentslot setting it back to the value before A was pressed.
                            }
                            InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(176, 176, 176, 255); //This changes the colour of the currently selected inventory slot's border to be grey, giving a visual representation of which slot is currently selected to the user.
                        }
                    }
                }
                if ((Character.slot[currentslot + 1].ID != 0)) //This if statement prevents the user from moving to the right when there is a Placeholder Item in the adjacent inventory slot's InventorySlot component.
                {
                    if (Input.GetKeyDown(KeyCode.D)) //This if statement will only run if the user presses the D key showing they want to move to the inventory slot to the right.
                    {
                        if (0 <= currentslot && currentslot <= 17) //This if statment only runs if the currentslot is between 0 and 17 showing that it is in the range of accessible inventory slots.
                        {
                            InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(255, 255, 255, 255); //This changes the colour of the currently selected inventory slot's border to be white, showing the user that this slot isn't selected.
                            currentslot = currentslot + 1; //This adds 1 from the currentslot showing the new selected slot is to the right. 
                            if (currentslot > 17) //This if statement will only run if currentslot greater than 17 implying the D key has been pressed at the end of the bottom row, this used to prevent an error of there being no slot to move to by preventing the user from pressing the D key while at the end of the bottom row.
                            {
                                currentslot = currentslot - 1; //This will minus 1 to the value of currentslot setting it back to the value before D was pressed.
                            }
                            InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(176, 176, 176, 255); //This changes the colour of the currently selected inventory slot's border to be grey, giving a visual representation of which slot is currently selected to the user.
                        }
                    }
                }
                if (0 <= currentslot && currentslot <= 17) //This if statment only runs if the currentslot is between 0 and 17 showing that it is in the range of accessible inventory slots.
                {
                    if (Character.slot[currentslot + 9].ID != 0) //This if statement prevents the user from moving down when there is a Placeholder Item in the below inventory slot's InventorySlot component.
                    {
                        if (Input.GetKeyDown(KeyCode.S))  //This if statement will only run if the user presses the S key showing they want to move to the inventory slot to the down.
                        {
                            if (currentslot < 9) //This if statement only runs if the currentslot is less than 9, to prevent the user from trying to access a slot below that doesn't exist. 
                            {
                                InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(255, 255, 255, 255); //This changes the colour of the currently selected inventory slot's border to be white, showing the user that this slot isn't selected.
                                currentslot = currentslot + 9; //This will add 9 from the value of currentslot setting it back to a value from the bottom row.
                                if (currentslot > 17) //This if statement will only run if currentslot greater than 17 implying the S key has been pressed at the end of the bottom row, this used to prevent an error of there being no slot to move to by preventing the user from pressing the S key while at the end of the bottom row.
                                {
                                    currentslot = currentslot - 9; //This will minus 9 to the value of currentslot setting it back to the value before W was pressed.
                                }
                                InventoryManager.slots[currentslot].GetComponent<Image>().color = new Color32(176, 176, 176, 255); //This changes the colour of the currently selected inventory slot's border to be grey, giving a visual representation of which slot is currently selected to the user.
                            }
                        }
                    }
                }
                if (0 <= currentslot && currentslot <= 17) //This if statment only runs if the currentslot is between 0 and 17 showing that it is in the range of accessible inventory slots.
                {
                    if (Input.GetKeyDown(KeyCode.Return)) //This if statement only runs if the user presses return showing they want to use an item in their inventory.
                    {
                        InventoryManager.UseItem(slot[currentslot]); //This runs the UseItem method in the InventoryManager script with the Item in the currentslot.
                        InventoryManager.DeleteItem(slot[currentslot]); //This runs the DeleteItem method in the InventoryManager script with the Item in the currentslot.
                    }
                }
                break; //This is used to terminate the current case so it can be switched.
            case (MenuPhase.SAVE): //This will change currentPhases value to MenuPhase.SAVE and the switch will compensate for such changing the case it is running to MenuPhase.SAVE.
                GameObject CharacterGo = GameObject.Find("CharacterOverworld"); //This creates a GameObject variable and assigns that to Character GameObject so that values from it's CharacterStats component can be accessed.
                string p_name = CharacterGo.GetComponent<CharacterStats>().p_name; //This creates a string p_name and assigns it to the p_name variable in the CharacterStats component attached to the Character GameObject.
                int p_hp = CharacterGo.GetComponent<CharacterStats>().p_hp; //This creates a int p_hp and assigns it to the p_hp variable in the CharacterStats component attached to the Character GameObject.
                int p_pp = CharacterGo.GetComponent<CharacterStats>().p_pp; //This creates a int p_pp and assigns it to the p_pp variable in the CharacterStats component attached to the Character GameObject.
                Stat p_def_stat = CharacterGo.GetComponent<CharacterStats>().p_def; //This creates a Stat p_def_stat and assigns it to the p_def variable in the CharacterStats component attached to the Character GameObject.
                int p_def = Int32.Parse(p_def_stat.BaseValue.ToString()); //This creates a int p_def and assigns it to the BaseValue of the p_def variable.
                Stat p_atk_stat = CharacterGo.GetComponent<CharacterStats>().p_atk; //This creates a Stat p_atk_stat and assigns it to the p_atk variable in the CharacterStats component attached to the Character GameObject.
                int p_atk = Int32.Parse(p_atk_stat.BaseValue.ToString()); //This creates a int p_atk and assigns it to the BaseValue of the p_atk variable.
                Stat p_spd_stat = CharacterGo.GetComponent<CharacterStats>().p_spd; //This creates a Stat p_spd_stat and assigns it to the p_spd variable in the CharacterStats component attached to the Character GameObject.
                int p_spd = Int32.Parse(p_spd_stat.BaseValue.ToString()); //This creates a int p_spd and assigns it to the BaseValue of the p_spd variable.
                DatabaseManager.StatUpdater(p_name, p_hp, p_pp, p_def, p_atk, p_spd); //This runs the StatUpdater method in the Inventory manager with the variables created in this case to update the already existing reocrd in the Stats table of the database.
                DatabaseManager.InventoryUpdater(Character.slot); //This runs the StatUpdater method in the Inventory manager with the Character GameObjects slot component to add the inventory items to the record in the Inventory table of the database.
                currentPhase = MenuPhase.MENU; //This will change currentPhases value to MenuPhase.MENU and the switch will compensate for such changing the case it is running to MenuPhase.MENU.
                break; //This is used to terminate the current case so it can be switched.
        }
    }
}
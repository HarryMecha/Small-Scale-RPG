using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class CharacterPanelConfig : MonoBehaviour
{
    public enum BattlePhase//This is an enumerator list that contains named constants used for the BattlePhases allowing for transitions between these phases.
    {
        START,
        PLAYERTURN,
        COMBO,
        PLAYERATTACK,
        ANIMATIONWAIT,
        ENEMYTURN,
        LOSE,
        WIN
    }
    public BattlePhase currentPhase;
    private static GameObject[,] CharactersArray; //Array containing Characters the i values being a validation variable 1 is Name, 2 is HP, 3 is PP and j value is for each character.
    private GameObject[,] EnemiesArray; //Array containing enemies properties e.g. Names, Stats, HP , PP, Amount.
    private int ActiveCharacters = 1; //The number of Currently active Characters in the party.
    private int ActiveEnemies = 1; //The number of Currently active Enemies in the party : This is determined by the class.
    protected static bool CharacterSelectActive; //This is a boolean that determines that the Character Select Panel is opened and the user has control.
    protected static bool MoveSelectActive; //This is a boolean that determines that the Move Select Panel is opened and the user has control.
    protected static bool ComboSelectActive; //This is a boolean that determines that the Combo Select Panel is opened and the user has control.
    protected static bool EnemySelectActive; //This is a boolean that determines that the Enemy Select Panel is opened.
    protected static bool ComboClose; //This is a boolean that determines if the ComboPanel is closed.
    private static GameObject MovesPanel; //This is the GameObject MovesPanel that's children are the content of the panel.
    private static GameObject MovesPanelCursor; //This is the GameObject of the Cursor that is moved by CursorControl().
    private static GameObject ComboPanel; //This is the GameObject ComboPanel that's children are the content of the panel.
    private static GameObject EnemyPanel; //This is the GameObject EnemyPanel that's children are the content of the panel.
    public static GameObject CharacterPanel; //This is the GameObject CharacterPanel that's children are the content of the panel.
    public static GameObject AttackDisplay; //This is the GameObject AttackDisplay that's children are the content of the panel.
    public static GameObject OutcomePanel; //This is the GameObject OutcomeDisplay that's children are the content of the panel.
    public static bool AnimationFinished; //This is a boolean that is determined by whether the Enemy GameObject's Animation has finished playing.
    int CursorPosition; //This is an integer that keeps track of the cursor in terms of the menu, it is also use to determine which method to execute in CursorControl().
    public static bool CharacterWin; //This a boolean that is determined by whether the EnemyGameObjects currentHealth is 0 before the Character GameObject's p_currenthp is 0.
    public Image EnemyHealth; //This variable contains the Image for the EnemyHealthbar.
    public float Damage; //This variable holds the amount of damage the Character GameObject or Enemy GameObject will have minuses from their p_currenthp and currentHealth respectively.
    public static int ComboCount; //This variable will hold the amount of times the user has successfully pressed the corresponding Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
    private static bool moneyrecievedbool; //This boolean will be determined by whether the random amount of money has been added to the Character GameObject's money variable.
    private void Start() //The start Method is just setting up all the variables beginning state.
    {
        MovesPanel = GameObject.Find("MovesPanel"); //This assigns the variable to the GameObject MovesPanel.
        MovesPanelCursor = GameObject.Find("MovesPanelCursor"); //This assigns the variable to the GameObject MovesPanelCursor.
        ComboPanel = GameObject.Find("ComboPanel"); //This assigns the variable to the GameObject ComboPanel.
        CharacterPanel = GameObject.Find("CharacterPanel"); //This assigns the variable to the GameObject CharacterPanel.
        EnemyPanel = GameObject.Find("EnemyPanel"); //This assigns the variable to the GameObject EnemyPanel.
        AttackDisplay = GameObject.Find("AttackDisplay"); //This assigns the variable to the GameObject AttackDisplay.
        OutcomePanel = GameObject.Find("OutcomePanel"); //This assigns the variable to the GameObject OutcomePanel.
        CharacterArraySetup(); //This will create an array that contains all the GameObjects in the CharacterPanel so their text components can be assigned.
        EnemyArraySetup(); //This will create an array that contains all the GameObjects in the EnemyPanel so their text components can be assigned.
        CharacterSetup(); //This will run the CharacterSetup script which will deactivate any of the unused columns set up by CharacterArraySetup.
        CharacterStatSetup(); //This will run the CharacterArraySetup which will apply the variables held in the Character GameObjects CharacterStats component to the text components of the GameObjects held in the CharacterPanel.
        EnemySetup(); //This will run the EnemySetup script which will deactivate any of the unused columns set up by EnemyArraySetup.
        EnemyStatSetup(); //This will run the EnemyArraySetup which will apply the variables held in the Enemy GameObjects EnemyStats component to the text components of the GameObjects held in the EnemyPanel.
        CharacterSelectActive = false; //This will set the boolean to false at start as no user input has been made to proceed to the CharacterSelect.
        ComboClose = false; //This will set the boolean to false at start as the ComboPanel has not been opened and doesn't need to be closed.
        ComboSelectActive = false; //This will set the boolean to false at start as no user input has been made to proceed to the ComboSelect.
        EnemySelectActive = true; //This will set the boolean to true on start for the user to select an enemy to attack.
        MovesPanel.SetActive(false); //This will set the MovesPanel GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        ComboPanel.SetActive(false); //This will set the ComboPanel GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        AttackDisplay.SetActive(false); //This will set the AttackDisplay GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        OutcomePanel.SetActive(false); //This will set the OutcomePanel GameObject to inactive so it will not be visible to the user and no scripts attached to it will run.
        currentPhase = BattlePhase.START;  //This will set the starting switch to BattlePhase.START and run the associated case.
        AnimationFinished = false; //This will set the boolean to false as no Animation has been played.
        CharacterWin = false; //This will set the boolean to false as the user has not won the battle on start.
        moneyrecievedbool = false; //This will set the boolean to false as money has yet to be added to the Charcter GameObject's money variable.
    }
    void Update()
    {
        switch (currentPhase) //This controls the switch that will run the case that is equal to the value of currentPhase.
        {
            case (BattlePhase.START): //This will run when currentPhase is equal to BattlePhase.START.
                AnimationFinished = false; //This will set the boolean to false as the Animation has finished playing.
                ComboClose = false; //This will set the boolean to false as the ComboPanel is no longer open therefore doesn't need to be closed.
                if (Input.GetKeyDown(KeyCode.Return)) //This if statement will only run if the user presses the Return key showing that they have chosen the Enemy GameObject they want to attack.
                {
                    for (int i = 0; i < (CursorPosition); i++) //This is a for loop that runs from 0 to the CursorPosition and is used to the set the Cursor position back to the first option.
                    {
                        MovesPanelCursor.transform.Translate(42f, 0.0f, 0.0f); //This moves the MovesPanelCursor back to it's original position.
                    }
                    CursorPosition = 0; //This CursorPosition is set back to 0 as it has returned to it's original position.
                    Input.ResetInputAxes(); //This resets the users input so that it doesn't loop through the if statement.
                    MovePanelOpen(); //This runs the MovesPanelOpen method which sets the MovesPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
                    currentPhase = BattlePhase.PLAYERTURN; //This will change currentPhases value to BattlePhase.PLAYERTURN and the switch will compensate for such changing the case it is running to BattlePhase.PLAYERTURN.
                }
                break; //This is used to terminate the current case so it can be switched.
            case (BattlePhase.PLAYERTURN): //This will run when currentPhase is equal to BattlePhase.PLAYERTURN.
                if (Input.GetKeyDown(KeyCode.Escape)) //This if statement will only run if the user presses the Escape key showing that they want to close the MovesPanel and select a different enemy.
                {
                    for (int i = 0; i < (CursorPosition); i++) //This is a for loop that runs from 0 to the CursorPosition and is used to the set the Cursor position back to the first option.
                    {
                        MovesPanelCursor.transform.Translate(42f, 0.0f, 0.0f); //This moves the MovesPanelCursor back to it's original position.
                    }
                    CursorPosition = 0; //This CursorPosition is set back to 0 as it has returned to it's original position.
                    MovesPanel.SetActive(false); //This sets the MovesPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
                    currentPhase = BattlePhase.START; //This will change currentPhases value to BattlePhase.START and the switch will compensate for such changing the case it is running to BattlePhase.START.
                }
                if (Input.GetKeyDown(KeyCode.S)) //This if statement will only run if the user presses the S key showing they want to move the cursor down.
                {
                    CursorPosition = CursorPosition + 1; //This will add 1 to the CursorPosition showing it has moved down a position and to correspond to script need to be run when the user presses the return key.
                    MovesPanelCursor.transform.Translate(-42f, 0.0f, 0.0f); //This will move the MovesPanelCursor down 42 in the x axis to visually show to the user the options they can select.
                    if (CursorPosition > 3) //This if statement will only run if the CursorPosition is over 3 which is out of bounds and so it will need to be moved back to an option on the menu.
                    {
                        MovesPanelCursor.transform.Translate(42f, 0.0f, 0.0f); //This will move the MovesPanelCursor up 42 in the x axis to put it back onto the list of options that visually show the user the options they can select.
                        CursorPosition = 3; //Sets the CursorPosition back to 3 which is the last selectable option.
                    }
                }
                if (Input.GetKeyDown(KeyCode.W)) //This if statement will only run if the user presses the W key showing they want to move the cursor up.
                {
                    CursorPosition = CursorPosition - 1; //This will minus 1 to the CursorPosition showing it has moved up a position and to correspond to script need to be run when the user presses the return key.
                    MovesPanelCursor.transform.Translate(42f, 0.0f, 0.0f); //This will move the MovesPanelCursor up 42 in the x axis to visually show to the user the options they can select.
                    if (CursorPosition < 0) //This if statement will only run if the CursorPosition is less than 0 which is out of bounds and so it will need to be moved back to an option on the menu.
                    {
                        MovesPanelCursor.transform.Translate(-42f, 0.0f, 0.0f); //This will move the MovesPanelCursor down 42 in the x axis to put it back onto the list of options that visually show the user the options they can select.
                        CursorPosition = 0; //Sets the CursorPosition back to 0 which is the first selectable option.
                    }
                }
                if (Input.GetKeyDown(KeyCode.Return)) //This if statement will only run when return is pressed showing the user wants to select and option which will depend on where the cursor position is.
                {
                    if (CursorPosition == 0 || CursorPosition == -1) //This if statement will only run when the CursorPosition is either 0 or a underflow value of -1 which is used to prevent errors, this showing the user wants to select Attack and run the Attack method.
                    {
                        Attack(); //This will run the Attack method that will minus the damage int from the Enemy's currentHealth variable in their EnemyStats component.
                    }
                    if (CursorPosition == 1) //This if statement will only run when the CursorPosition is 1 this showing the user wants to select Special to run the Special method.
                    {
                        Special(); //This will run the Special method opening the ComboPanel and allowing user to increase the amount of damage that will be minus from the Enemy's currentHealth variable in their EnemyStats component.
                    }
                    if (CursorPosition == 3 || CursorPosition == 4) //This if statement will only run when the CursorPosition is either 3 or a over value of 4 which is used to prevent errors, this showing the user wants to select Run and run the Run method.
                    {
                        Run(); //This will run the Run method that will imply the user no longer wants to battle and change the scene back to City.
                    }
                }
                break; //This is used to terminate the current case so it can be switched.
            case (BattlePhase.COMBO): //This will run when currentPhase is equal to BattlePhase.COMBO.
                if (ComboClose == false) //This if statement will only run if the ComboClose if false implying the ComboPanel is not open.
                {
                    ComboPanelOpen(); //This will run the ComboPanelOpen methods that will set the ComboPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
                }
                else
                {
                    AnimationFinished = false; //This will set the boolean to false as the Animation is going to play and has not yet finished.
                    currentPhase = BattlePhase.ANIMATIONWAIT; //This will change currentPhases value to BattlePhase.ANIMATIONWAIT and the switch will compensate for such changing the case it is running to BattlePhase.ANIMATIONWAIT.
                }
                break; //This is used to terminate the current case so it can be switched.
            case (BattlePhase.PLAYERATTACK): //This will run when currentPhase is equal to BattlePhase.PLAYERATTACK.
                Damage = (GameObject.Find("CharacterOverworld").GetComponent<CharacterStats>().p_atk.BaseValue) + (ComboCount * 5); //This will assign the damage variable to the the p_atk's basevalue that can be found in the CharacterStats component of the Character GameObject plus 5 times ComboCount.
                GameObject.Find("Enemy1").GetComponent<EnemyStats>().TakeDamage(Damage); //This will run the TakeDamage method in the EnemyStats script and minus the damage variable from the Enemy GameObject's currentHealth.
                ComboCount = 0; //This sets the ComboCount back to 0 so if Special is run again the number will not keep increasing.
                if (GameObject.Find("Enemy1").GetComponent<EnemyStats>().currentHealth <= 0) //This if statement will only run if the EnemyGameObject's currentHealth found in the EnemyStats component is equal or less than 0.
                {
                    currentPhase = BattlePhase.WIN; //This will change currentPhases value to BattlePhase.WIN and the switch will compensate for such changing the case it is running to BattlePhase.WIN.
                }
                else
                {
                    currentPhase = BattlePhase.ENEMYTURN; //This will change currentPhases value to BattlePhase.ENEMYTURN and the switch will compensate for such changing the case it is running to BattlePhase.ENEMYTURN.
                }
                break; //This is used to terminate the current case so it can be switched.
            case (BattlePhase.ANIMATIONWAIT): //This will run when currentPhase is equal to BattlePhase.ANIMATIONWAIT.
                if (AnimationFinished == true) //This if statement will only run when AnimationFinished is true showing that the Animation is done and resuming the Battle.
                {
                    AttackDisplayClose(); //This will run the AttackDisplayClose method that will set the AttackDisplayPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
                    currentPhase = BattlePhase.PLAYERATTACK; //This will change currentPhases value to BattlePhase.PLAYERATTACK and the switch will compensate for such changing the case it is running to BattlePhase.PLAYERATTACK.
                }
                if (AnimationFinished == false) //This if statement will only run when AnimationFinished is false showing that the Animation is still running and stops the Battle.
                {
                    AttackDisplayOpen(); //This will run the AttackDisplayClose method that will set the AttackDisplayPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
                    GameObject.Find("AttackText").GetComponent<TextMeshProUGUI>().SetText("Attack"); //This finds the GameObject AttackText and changes it's TextMeshProUGUI component to be Attack.
                    GameObject.Find("AttackText2").GetComponent<TextMeshProUGUI>().SetText(GameObject.Find(("CharacterOverworld")).GetComponent<CharacterStats>().p_name + ("'s Attack:")); //This finds the GameObject AttackText2 and changes it's TextMeshProUGUI component to be 's Attack:.
                    StartCoroutine(AnimationWait()); //This starts a cooroutine that will only turn AnimationFinished true when the time for the Animation attached to the Enemy GameObject has played once.
                }
                break; //This is used to terminate the current case so it can be switched.
            case (BattlePhase.ENEMYTURN): //This will run when currentPhase is equal to BattlePhase.ENEMYTURN.
                GameObject.Find("CharacterOverworld").GetComponent<CharacterStats>().TakeDamage(GameObject.Find("Enemy1").GetComponent<EnemyStats>().e_atk.BaseValue);  //This will run the TakeDamage method in the CharacterStats script and minus the damage variable,the e_atk's basevalue that can be found in the EnemyStats component of the Enemy GameObject, from the Character GameObject's p_currenthp.
                CharacterStatSetup(); //This will run the CharacterStatSetup to update the values that are dispayed on the CharacterPanel GameObject.
                MovePanelClose(); //This will run the MovePanelClose method that sets the MovesPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
                currentPhase = BattlePhase.START; //This will change currentPhases value to BattlePhase.START and the switch will compensate for such changing the case it is running to BattlePhase.START.
                break; //This is used to terminate the current case so it can be switched.
            case (BattlePhase.LOSE): //This will run when currentPhase is equal to BattlePhase.LOSE.
                Character.BattleCounter = Character.BattleCounter + 1; //This will add 1 to the Character GameObject's BattleCounter showing they have fought another battle.
                break; //This is used to terminate the current case so it can be switched.
            case (BattlePhase.WIN): //This will run when currentPhase is equal to BattlePhase.WIN.
                OutcomePanelOpen(); //This will run the OutcomePanelOpen method that sets the OutcomePanel GameObject to active so it will be visible to the user and scripts attached to it will run.
                if (Input.GetKeyDown(KeyCode.Return)) //This if statement only runs if the user presses the Return key showing they have finished reading the OutcomePanel and wish to exit the battle.
                {
                    CharacterWin = true; //This sets the CharacterWin boolean to true showing that the user has beaten the Enemy GameObject. 
                    SceneManager.LoadScene("City"); //This will change the scene to City.
                    if (CharacterWin == true) //This if statement will only run whilst CharacterWin is true.
                    {
                        GameObject DroppedItem = GameObject.Find("Enemy1").GetComponent<EnemyStats>().drop_item; //This creates and sets the DroppedItem GameObject to the Enemy GameObjects drop_item variable in the EnemyStats component.
                        Item ItemObtained = DroppedItem.GetComponent<Item>(); //This creates and sets the ItemObatined Item to the DroppedItem's Item component.
                        InventoryManager.AddItem(ItemObtained); //This will run the AddItem script found in the InventoryManager script to add the ItemObtained into the player's inventory.
                    }
                }
                Character.BattleCounter = Character.BattleCounter + 1; //This will add 1 to the Character GameObject's BattleCounter showing they have fought another battle.
                break; //This is used to terminate the current case so it can be switched.
        }
    }
    //The CharacterSetup method deactivates any of the unused columns set up by CharacterArraySetup().
    void CharacterSetup() 
    {
        for (int i = 0; i < 3; i++) //This is a for loop that goes from 0 to 3 and represents the columns in the CharacterPanel.
        {
            for (int j = ActiveCharacters; j < 4; j++) //This is a for loop that goes from the amount of Active characters to 4 and represents all of the current vancant columns in the CharacterPanel.
            {
                CharactersArray[i, j].SetActive(false); //This sets the GameObjects held at the position CharacterArray [i,j] to inactive so it will be invisible to the user and no scripts attached to it will run.
            }
        }
    }
    //The CharacterArraySetup method sets up all the active characters using a 2D Array with the i variable being the row and j being the column.
    void CharacterArraySetup()
    {
        CharactersArray = new GameObject[3, 4]; //This sets the size of the 2D array to be 3,4 which will hold the columns of the CharacterPanel in the first value and the Character GameObject's values in the second.
        for (int i = 0; i < 3; i++) //This is a for loop that goes from 0 to 3 and represents the columns in the CharacterPanel.
        {
            for (int j = 0; j < 4; j++) //This is a for loop that goes from 0 to 4 and represents the rows of the CharacterPanel.
            {
                if (i == 0) //This if statment only runs if i is 0, the Character Name column.
                {
                    CharactersArray[i, j] = GameObject.Find("Character" + (j + 1) + "Name"); //This assigns the value of CharactersArray at position i,j to be the CharacterName variable of the CharacterStats component of the Character GameObject that corresponds the the j value.
                }
                if (i == 1) //This if statment only runs if i is 1, the Character HP column.
                {
                    CharactersArray[i, j] = GameObject.Find("Character" + (j + 1) + "HP"); //This assigns the value of CharactersArray at position i,j to be the CharacterHP GameObject that corresponds the the j value.
                }
                if (i == 2) //This if statment only runs if i is 2, the Character PP column.
                {
                    CharactersArray[i, j] = GameObject.Find("Character" + (j + 1) + "PP"); //This assigns the value of CharactersArray at position i,j to be the CharacterPP GameObject that corresponds the the j value.
                }
            }
        }
    }
    //The CharacterStatSetup method applies the variables held in the Character GameObjects CharacterStats component to the text components of the GameObjects held in the CharacterPanel.
    void CharacterStatSetup()
    {
        for (int i = 0; i < 3; i++) //This is a for loop that goes from 0 to 3 and represents the columns in the CharacterPanel.
        {
            for (int j = 0; j < ActiveCharacters; j++) //This is a for loop that goes from 0 to the amount of ActiveCharacters only setting up the rows with Character GameObject that are active.
            {
                if (i == 0) //This if statment only runs if i is 0, the Character Name column.
                {
                    CharactersArray[i, j].GetComponent<TextMeshProUGUI>().SetText(GameObject.Find("CharacterOverworld" /*+ (j + 1)*/).GetComponent<CharacterStats>().p_name); //This sets the TextMeshProUGUI compoent of the GameObject at position i,j to be the CharacterName variable of the CharacterStats component of the Character GameObject that corresponds the the j value. 
                }
                if (i == 1) //This if statment only runs if i is 1, the Character HP column.
                {
                    CharactersArray[i, j].GetComponent<TextMeshProUGUI>().SetText((GameObject.Find("CharacterOverworld" /*+ (j + 1)*/).GetComponent<CharacterStats>().p_currenthp.ToString()) + "/" + (GameObject.Find("CharacterOverworld" /*+ (j + 1)*/).GetComponent<CharacterStats>().p_hp.ToString())); //This sets the TextMeshProUGUI compoent of the GameObject at position i,j to be the CharacterHP variable of the CharacterStats component of the Character GameObject that corresponds the the j value. 
                }
                if (i == 2) //This if statment only runs if i is 2, the Character PP column.
                {
                    CharactersArray[i, j].GetComponent<TextMeshProUGUI>().SetText(GameObject.Find("CharacterOverworld" /*+ (j + 1)*/).GetComponent<CharacterStats>().p_pp.ToString()); //This sets the TextMeshProUGUI compoent of the GameObject at position i,j to be the CharacterPP variable of the CharacterStats component of the Character GameObject that corresponds the the j value. 
                }
            }
        }
    }
    void EnemyArraySetup() //This sets up all the active enemies using a 2D Array with the i being a placeholder j being the column
    {
        EnemiesArray = new GameObject[1, 4]; //This sets the size of the 2D array to be 1,4 which will hold the columns of the EnemyPanel in the first value and the Enemy GameObject's values in the second.
        for (int i = 0; i < 1; i++) //This is a for loop that goes from 0 to 1 and represents the columns in the EnemyPanel.
        {
            for (int j = 0; j < 4; j++) //This is a for loop that goes from 0 to 4 and represents the rows of the EnemyPanel.
            {
                if (i == 0) //This if statment only runs if i is 0, the Enemy Name column.
                {
                    EnemiesArray[i, j] = GameObject.Find("Enemy" + (j + 1) + "Name"); //This assigns the value of EnemiessArray at position i,j to be the EnemyName variable of the EnemyStats component of the Enemy GameObject that corresponds the the j value.
                }
            }
        }
    }
    void EnemySetup() //This deactivates any of the unused columns set up by CharacterArraySetup()
    {
        for (int i = 0; i < 1; i++) //This is a for loop that goes from 0 to 3 and represents the columns in the EnemyPanel.
        {
            for (int j = ActiveEnemies; j < 4; j++) //This is a for loop that goes from the amount of Active enemies to 4 and represents all of the current vancant columns in the EnemyPanel.
            {
                EnemiesArray[i, j].SetActive(false); //This sets the GameObjects held at the position EnemyArray [i,j] to inactive so it will be invisible to the user and no scripts attached to it will run.
            }
        }
    }
    //The EnemyStatSetup method applies the variables held in the Enemy GameObjects EnemyStats component to the text components of the GameObjects held in the EnemyPanel.
    void EnemyStatSetup()
    {
        for (int i = 0; i < 1; i++) //This is a for loop that goes from 0 to 1 and represents the columns in the EnemyPanel.
        {
            for (int j = 0; j < ActiveEnemies; j++) //This is a for loop that goes from 0 to the amount of ActiveEnemies only setting up the rows with Enemy GameObjects that are active.
            {
                if (i == 0) //This if statment only runs if i is 0, the Enemy Name column.
                {
                    EnemiesArray[i, j].GetComponent<TextMeshProUGUI>().SetText(GameObject.Find("Enemy" + (j + 1)).GetComponent<EnemyStats>().e_name); //This sets the TextMeshProUGUI compoent of the GameObject at position i,j to be the EnemyName variable of the EnemyStats component of the Enemy GameObject that corresponds the the j value. 
                }
            }
        }
    }
    //The MovePanelOpen script is used to open up the MovesPanel.
    public static void MovePanelOpen()
    {
        CharacterPanel.SetActive(true); //This sets the CharacterPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        EnemyPanel.SetActive(true); //This sets the EnemyPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        MovesPanel.SetActive(true); //This sets the MovesPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
    }
    //The MovePanelClose script is used to close the MovesPanel.
    public static void MovePanelClose()
    {
        CharacterPanel.SetActive(true); //This sets the CharacterPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        EnemyPanel.SetActive(true); //This sets the EnemyPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        MovesPanel.SetActive(false); //This sets the MovesPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
    }
    //The ComboPanelOpen script is used to open the ComboPanel.
    public static void ComboPanelOpen()
    {
        MovesPanel.SetActive(false); //This sets the MovesPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        CharacterPanel.SetActive(false); //This sets the CharacterPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        EnemyPanel.SetActive(false); //This sets the EnemyPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        ComboPanel.SetActive(true); //This sets the ComboPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        ComboSelectActive = true; //This sets the boolean ComboSelectActive to true representing that the ComboSelectPanel is open.
    }
    //The ComboPanelClose script is used to close the ComboPanel.
    public void ComboPanelClose()
    {
        ArrowMovement.ArrowCount = 0;
        MovesPanel.SetActive(false); //This sets the MovesPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        CharacterPanel.SetActive(true); //This sets the CharacterPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        ComboPanel.SetActive(false); //This sets the ComboPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        EnemyPanel.SetActive(true);  //This sets the EnemyPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        MoveSelectActive = (false); //This sets the boolean MovesSelectActive to false implying the MoveSelectPanel is closed.
        CharacterSelectActive = true; //This sets the boolean CharacterSelectActive to true implying the CharacterSelectPanel is open.
        currentPhase = BattlePhase.PLAYERATTACK; //This will change currentPhases value to BattlePhase.PLAYERATTACK and the switch will compensate for such changing the case it is running to BattlePhase.PLAYERATTACK.
    }
    //The AttackDisplayOpen script is used to open the AttackDisplay Panel.
    public static void AttackDisplayOpen()
    {
        AttackDisplay.SetActive(true); //This sets the AttackDisplay GameObject to active so it will be visible to the user and scripts attached to it will run.
        CharacterPanel.SetActive(false); //This sets the CharacterPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        EnemyPanel.SetActive(false); //This sets the EnemyPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
    }
    //The AttackDisplayClose script is used to close the AttackDisplay Panel.
    public static void AttackDisplayClose()
    {
        AttackDisplay.SetActive(false); //This sets the AttackDisplay GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        CharacterPanel.SetActive(true); //This sets the CharacterPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        EnemyPanel.SetActive(true); //This sets the EnemyPanel GameObject to active so it will be visible to the user and scripts attached to it will run.
    }
    //The OutcomePanelOpen script is used to open the Outcome Panel.
    public static void OutcomePanelOpen()
    {
        OutcomePanel.SetActive(true); //This sets the OutcomePanel GameObject to active so it will be visible to the user and scripts attached to it will run.
        AttackDisplay.SetActive(false); //This sets the AttackDisplay GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        ComboPanel.SetActive(false); //This sets the ComboPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        CharacterPanel.SetActive(false); //This sets the CharacterPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        EnemyPanel.SetActive(false); //This sets the EnemyPanel GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        if (moneyrecievedbool == false) //This if statement only runs if the moneyrecievedbool is false implying that the money has not been added the Character GameObject's money variable.
        {
            float moneyrecieved = Random.Range(25, 35); //This creates and assigns the float moneyrecieved to a random float in the range of 25 and 35.
            Character.money = Character.money + moneyrecieved; //This adds on the moneyrecieved float to the Character GameObject's money variable.
            GameObject.Find("OutcomeText").GetComponent<TextMeshProUGUI>().SetText("YOU WIN! " + "\n" + "You recieved a " + (GameObject.Find("Enemy1").GetComponent<EnemyStats>().drop_item.GetComponent<Item>().Type) + " and " + moneyrecieved + " gold."); //This sets the value of the GameObject OutcomeText's TextMeshProUGUI component.
            moneyrecievedbool = true; //This sets the moneyrecievedbool to true to show that the money has been added the Character GameObject's money variable and to stop the if statement from looping.
        }
    }
    //The Attack method is used to assign the damage variable and change the currentPhase to BattlePhase.PLAYERATTACK.
    public void Attack()
    {
        Damage = GameObject.Find("CharacterOverworld").GetComponent<CharacterStats>().p_atk.BaseValue; //This assigns the Damage int to the p_atk variables BaseValue that can be found the CharacterGameObjects CharacterStats component.
        currentPhase = BattlePhase.PLAYERATTACK; //This will change currentPhases value to BattlePhase.PLAYERATTACK and the switch will compensate for such changing the case it is running to BattlePhase.PLAYERATTACK.
    }
    //The Special method is used to change the currentPhase to BattlePhase.COMBO.
    public void Special()
    {
        currentPhase = BattlePhase.COMBO; //This will change currentPhases value to BattlePhase.COMBO and the switch will compensate for such changing the case it is running to BattlePhase.COMBO.
    }
    //The Run method is used when the user no longer wants to battle and change the scene back to City.
    public void Run()
    {
        SceneManager.LoadScene("City"); //This will change the scene to City.
    }
    //The AnimationWait method is used to wait until the duration of the Animation component attached to the Enemy GameObject has played out.
    IEnumerator AnimationWait()
    {
        AttackAnimationsEnemy.animation.Play("EnemyTestAnimation"); //This will play the animation found in the AttackAnimationEnemy script.
        yield return new WaitForSeconds(AttackAnimationsEnemy.animation["EnemyTestAnimation"].length); //This will wait the duration of the animation before continuing the script.
        AnimationFinished = true; //This will set the value of AnimationFinished to true showing that the Animation has finished playing.
        StopCoroutine(AnimationWait()); //This will stop the AnimationWait coroutine.
    }
}
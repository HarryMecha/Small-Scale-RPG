using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ArrowMovement : CharacterPanelConfig
{
    public Transform ArrowSpawn; //It's the position of an empty GameObject that will the Arrow will spawn onto.
    public Transform TextSpawn; //This is the position of the an empty GameObject that text will be instantiated to.
    public GameObject ArrowPrefab; //This is the ArrowsPrefab that is instantaited onto the position of ArrowSpawn.
    public GameObject Arrow; //This is the Arrow as a whole used to instantiate the arrow.
    public GameObject TextPrefab; //This is a prefab for the text so it can be instantiated at poisition, TextSpawn.
    public bool ArrowActive; //This is a boolean that determines if there is an arrow currently on the screen so that a new one can be instantiated or the Combo will end.
    public string[] Directions; //This is an array that contains strings that are used to determine which spawn the arrow will come from.
    public int Arrows; //The amount of arrows that need to be spawn per special move.
    public static int ArrowCount = 0; //The amount of arrows that have been spawned in that combo used as a check with ArrowActive.
    public static Collider2D CurrentIconCollider; //The bottom row that is a sensor that detects if an arrow is in that area, used as a check to see if the user has hit the corresponding arrow key at the right time.
    public int RndmPosition; //This is a random int variable so that the arrows spawn at different positions everytime.
    public Vector3 ColliderMax, ColliderMin; //This is the Maximum and Minimum positions of the bottom row.
    public void Start()
    {
        Directions = new string[4]; //This sets the Directions array to have a size of 4 to hold all 4 directions.
        Directions[0] = ("Left"); //This will set the string in position 0 of the array to Left.
        Directions[1] = ("Down"); //This will set the string in position 1 of the array to Down.
        Directions[2] = ("Up"); //This will set the string in position 2 of the array to Up.
        Directions[3] = ("Right"); //This will set the string in position 3 of the array to Right.
        ArrowActive = false; //This will turn the ArrowActive boolean to false showing that no Arrow is present on the ComboPanel.
        ArrowCount = 0; //This sets the ArrowCount to 0 as o arrow has been instantiated at start.
        ComboCount = 0; //This sets ComboCount to 0 as no arrow has been instatiated at start so there is no prompt for the user to press a directional key.
    }
    public void Update()
    {
        InputDetection(); //This will run the InputDetection script every frame to make sure when the Arrow GameObject is instantiated and it's position overlaps with the ArrowSensor GameObject and an input is detected by the user it will register it.
        if (ComboSelectActive == true) //This if statement only runs when the ComboPanel is opened showing that the user has control.
        {
            for (int i = 0; i < 4; i++) //This for loop runs from 0 to 4 and makes sure that 4 arrows have been instantiated and moved before changing the currentPhase to BattlePhase.PLAYERTURN.
            {
                MovementManager(); //This will run the MovementManager script that will make the instaiated Arrows move down the ComboPanel.
            }
        }
        else
        {
            currentPhase = BattlePhase.PLAYERTURN; //This will change currentPhases value to BattlePhase.PLAYERTURN in CharacterPanelConfig and the switch will compensate for such changing the case it is running to BattlePhase.PLAYERTURN.
        }
    }
    //The ArrowCreate script is used to instantiate an arrow at the top of one of the tracks which is randomly decided upon.
    public void ArrowCreate()
    {
        RndmPosition = Random.Range(0, 4);//This creates a random number that will be put into the array in order to obtain a direction.
        ArrowSpawn = GameObject.Find(Directions[RndmPosition] + "ArrowSpawn").transform; //This sets up the Arrows spawn position based on the random array position.
        Arrow = Instantiate(ArrowPrefab, ArrowSpawn.position, ArrowSpawn.rotation); //Instantiates the arrow.
        GameObject ComboMask = GameObject.Find("ComboMask"); //This finds the Mask around the combo box.
        Arrow.transform.SetParent(ComboMask.transform); //This sets the Arrow as a child of the mask making it invisible when it's not in the panel.
        ArrowActive = true; //Boolean to make sure that arrows don't keep instantaited.
    }
    //The MovementManager script is used to move the instantiated arrows down the track and destroy them wither when the user presses the correct input when the arrow's position overlaps the Arrow Sensor or when their positions are below the ComboPanel.
    public void MovementManager()
    {
        if (ArrowActive == false)//This is the condition that keeps it recuring.
        {
            ArrowCreate(); //External script that instantiates arrows.
        }
        if (Arrow.transform.position.y >= -125) //A check that the should move when it is above -125 that being it's final destination.
        {
            Arrow.GetComponent<Rigidbody>().velocity = Arrow.transform.up * 500; //This moves the arrow downwards by speed 500.
        }
        else
        {
            Destroy(Arrow); //This destroys the arrow when it's below -125.
            ArrowActive = false; //This allows another arrow to be instantiated.
            ArrowCount = ArrowCount + 1;//Adds one to the arrow counter that would be compared with the max amount of arrows.
        }
        if (ArrowCount >= 4) //This if statement only runs if the ArrowCount is greater than or equal to 4, it closes the ComboPanel and stops the Arrows from instantiating.
        {
            ComboClose = true; //The boolean is set to true showing that the Combo is done and the ComboPanel needs to be closed.
            ComboSelectActive = false; //This boolean is set to false showing that the Combo is done and it will stop the if statement in Update from running and starting the else statement.
            ComboPanelClose(); //The ComboPanelClose script located in the CharacterPanelConfig is run that the set the ComboPanel to inactive and other panels to active.
        }
    }
    //Thw InputDetection script is used to show the system what to do when handling the inputs the user gives whilst the arrow's position overlaps the Arrow Sensor.
    public void InputDetection()
    {
        if (ArrowActive == true) //This if statement only runs whilst ArrowActive is true implying an arrow has been instantiated on the ComboPanel. 
        {
            CurrentIconCollider = GameObject.Find("Arrow Sensor").GetComponent<Collider2D>(); //This assigns the CurrentIconCollider to the Arrow Sensor GameObject's Collider2D component.
            ColliderMin = CurrentIconCollider.bounds.min; //Assigns the minimum point of the collider.
            ColliderMax = CurrentIconCollider.bounds.max; //Assigns the maximum point of the collider.
            if ((Arrow.transform.position.y > ColliderMin.y) && (Arrow.transform.position.y < ColliderMax.y)) //This if statement only runs when the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
            {
                if (RndmPosition == 0) //This if statement only runs while the RndmPosition is 0 showing it's on the left track and only register inputs from the LeftArrow Key.
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) //This if statement will only run when the user presses the LeftArrow Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    {
                        Destroy(Arrow); //This destroys the Arrow GameObject as it registers the user input.
                        ArrowActive = false; //This will set the ArrowActive boolean to false as there in no current Arrow GameObject on the ComboPanel.
                        ArrowCount = ArrowCount + 1; //This will add 1 to the ArrowCount showing that an Arrow GameObject has made one pass through the ComboPanel.
                        ComboCount = ComboCount + 1; //This will add 1 to the ComboCount showing the user has successfully pressed the corresponding Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    }
                }
                if (RndmPosition == 1) //This if statement only runs while the RndmPosition is 1 showing it's on the down track and only register inputs from the DownArrow Key.
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow)) //This if statement will only run when the user presses the DownArrow Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    {
                        Destroy(Arrow); //This destroys the Arrow GameObject as it registers the user input.
                        ArrowActive = false; //This will set the ArrowActive boolean to false as there in no current Arrow GameObject on the ComboPanel.
                        ArrowCount = ArrowCount + 1; //This will add 1 to the ArrowCount showing that an Arrow GameObject has made one pass through the ComboPanel.
                        ComboCount = ComboCount + 1; //This will add 1 to the ComboCount showing the user has successfully pressed the corresponding Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    }
                }
                if (RndmPosition == 2) //This if statement only runs while the RndmPosition is 2 showing it's on the up track and only register inputs from the UpArrow Key.
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow)) //This if statement will only run when the user presses the UpArrow Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    {
                        Destroy(Arrow); //This destroys the Arrow GameObject as it registers the user input.
                        ArrowActive = false; //This will set the ArrowActive boolean to false as there in no current Arrow GameObject on the ComboPanel.
                        ArrowCount = ArrowCount + 1; //This will add 1 to the ArrowCount showing that an Arrow GameObject has made one pass through the ComboPanel.
                        ComboCount = ComboCount + 1; //This will add 1 to the ComboCount showing the user has successfully pressed the corresponding Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    }
                }
                if (RndmPosition == 3) //This if statement only runs while the RndmPosition is 3 showing it's on the right track and only register inputs from the RightArrow Key.
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow)) //This if statement will only run when the user presses the RightArrow Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    {
                        Destroy(Arrow); //This destroys the Arrow GameObject as it registers the user input.
                        ArrowActive = false; //This will set the ArrowActive boolean to false as there in no current Arrow GameObject on the ComboPanel.
                        ArrowCount = ArrowCount + 1; //This will add 1 to the ArrowCount showing that an Arrow GameObject has made one pass through the ComboPanel.
                        ComboCount = ComboCount + 1; //This will add 1 to the ComboCount showing the user has successfully pressed the corresponding Key whilst the Arrow GameObject's position overlaps with the Arrow Sensor's collider component.
                    }
                }
            }
        }
    }
}

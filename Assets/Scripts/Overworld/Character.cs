using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Character : MonoBehaviour
{
    public Camera camPrefab; //This is the cameras spawn point when following the character
    public float Characterrunspeed = 16.0f; //Dictates speed when running  
    public float Characterspeed = 8.0f; //Dictates speed
    public float Characterrotate = 8.0f; //Dictates how fast the character rotates
    public float CharacterjumpSpeed = 8.0f; //Dictates how high the character jumps
    public float Charactergravity = 20.0f; //Dictates how heavy gravity is on the characters
    public bool Charactermovement = true; //A boolean to check is the character is moving or not
    [SerializeField]
    private Vector3 CharacterDirection = Vector3.zero; //This is the direction the character is facing
    [SerializeField]
    public static Vector3 CharacterPosition; //This is a vector three that 
    private CharacterController controller; //This is the CharacterController attached to the GameObject
    public static bool InventoryASetup; //This is a boolean that is detremined by whether the inventory has been setup to avoid the possibility of it performing the InventorySetup method in Inventory Manager from running twice and deleting any items that may be in the user's inventory.
    public static Item[] slot; //This is a array of Item GameObjects that acts as the users inventory.
    public static float money; //This is a float that stores the user's moneys which is used in the shops with the BuyItem method and after an enemy defeated.
    public static int BattleCounter; //This is an int for the amount of battles a user has fought, used in the CharacterPanelConfig after the BattlePhase turns to WIN or LOSE.
    void Start()
    {
        BattleCounter = 0; //This assigns the int to 0 when you first load into the game.
        DontDestroyOnLoad(this); //This makes sure that the Character GameObject is not destroyed when the scene is changed.
        if (FindObjectsOfType(GetType()).Length > 1) //This portion of code identifies if there are any duplicate CharacterGameObjects and destroys them
        {
            Destroy(gameObject);
        }
        camPrefab = Camera.main; //This sets the camPrefab to the main camera following the character.
        controller = GetComponent<CharacterController>(); //This assigns the controller variable to the CharacterController attached to the CharacterController used for movement.
        if (FindObjectsOfType(GetType()).Length == 1) //This if statement checks that there are no duplicates of the Character as to make sure that another inventory is created which would cause errors.
        {
            slot = new Item[18]; //This sets the slot[] array to have a size of 18, the size of the user's inventory.
        }
        //var mainCam = Instantiate (camPrefab, camSpawn.position, camSpawn.rotation);
    }
    void Update()
    {
        //Debug.Log(slot[0]);
        if (Input.GetKeyDown(KeyCode.T)) //This if statement only run if T is pressed
        {
            CharacterPosition = this.transform.position; //This assigns CharacterPosition to the Character GameObjects current position.
            PositionWriter(); //This calls the PositionWriter method.
        }
        Scene currentScene = SceneManager.GetActiveScene(); //This retrieves the name of the scene.
        string SceneName = currentScene.name; //This assigns the SceneName string the name of the current scene.
        if (SceneName != ("Battle") || (OptionsConfig.PauseMenuOpen = false) || (NPCInteraction.IsInteracting = false)) //This if statment is used to restrict movement to make sure that the character doesn't move if these conditions are not met.
        {
            this.GetComponent<CharacterController>().enabled = true; //This enables the CharacterController allowing the user to move the character.
            this.GetComponent<Rigidbody>().isKinematic = true; //This enables the RigidBodies isKinematic allowing the Character GameObject to keep momentumn and interact with colliders.
            this.GetComponent<Rigidbody>().useGravity = true; //This enables the RigidBodies useGravity allowing the Character GameObject to experience the effect of gravity.
            Charactergravity = 20; //This assigns the gravity value which effects the characters jumping and allowing it to stay on the ground.
        }
        else
        {
            this.GetComponent<CharacterController>().enabled = false; //This disables the CharacterController preventing the user to move the character.
            this.GetComponent<Rigidbody>().isKinematic = false; //This disables the RigidBodies isKinematic preventing the Character GameObject to keep momentumn and interact with colliders.
            this.GetComponent<Rigidbody>().useGravity = false; //This disables the RigidBodies useGravity preventing the Character GameObject to experience the effect of gravity.
            Charactergravity = 0; //This assigns the gravity value to 0 suspending the Character's z coordinate.
        }
        if (OptionsConfig.PauseMenuOpen == false) //This if statment is used to restrict movement to make sure that the character doesn't move if these conditions are not met.
        {
            this.GetComponent<CharacterController>().enabled = true; //This enables the CharacterController allowing the user to move the character.
        }
        else
        {
            this.GetComponent<CharacterController>().enabled = false; //This disables the CharacterController preventing the user to move the character.
        }
        if (controller.isGrounded) //This if statement is used to restrict the user's movement if the Character GameObject is not standing on the ground.
        {
            CharacterDirection = new Vector3(0, 0, Input.GetAxis("Vertical")); //This assigns to an empty Vector3 with the Z axis being set between 1 and -1 depending on the user's input.
            CharacterDirection = transform.TransformDirection(CharacterDirection); //This assigns the Vector3 to be Character GameObjects position.
            CharacterDirection *= Characterspeed; //This moves the Character GameObejcts position by the Characterspeed.
            if (Input.GetButtonDown("Run")) //This if statment is used to only run when Shift is pressed.
            {
                CharacterDirection = new Vector3(0, 0, Input.GetAxis("Vertical")); //This assigns to an empty Vector3 with the Z axis being set between 1 and -1 depending on the user's input.
                CharacterDirection = transform.TransformDirection(CharacterDirection); //This assigns the Vector3 to be Character GameObjects position.
                CharacterDirection *= Characterrunspeed; //This moves the Character GameObjects position by the Characterrunspeed.
            }
            if (Input.GetButtonDown("Jump")) //This if statment is used to jump when SpaceBar is pressed. 
            {
                CharacterDirection.y = CharacterjumpSpeed; //This will move the character up in the y axis with speed of CharacterjumpSpeed
            }
        }
        else //This else statement is used to define any movement that can be made when the character is in the air e.g. jumping.
        {
            CharacterDirection = new Vector3(0, CharacterDirection.y, Input.GetAxis("Vertical")); //This assigns to an empty Vector3 with the Z axis being set between 1 and -1 depending on the user's input.
            CharacterDirection = transform.TransformDirection(CharacterDirection); //This assigns the Vector3 to be Character GameObjects position.
            CharacterDirection.x *= Characterrotate; //This assigns the vector3's x rotation when a is pressed.
            CharacterDirection.z *= Characterrotate; //This assigns the vector3's x rotation when d is pressed.
        }
        if ((OptionsConfig.PauseMenuOpen == false) && (SceneName != ("Battle"))) //This if statment is used to restrict movement to make sure that the character doesn't move if these conditions are not met.
        {
            transform.Rotate(0, Input.GetAxis("Horizontal"), 0); //This assigns to an empty Vector3 with the Y axis being set between 1 and -1 depending on the user's input.
            CharacterDirection.y -= Charactergravity * Time.deltaTime; //This is used whenever the character is above the grounf allowing them to fall, the speed of which is dictated by the CharacterGravity.
            controller.Move(CharacterDirection * Time.deltaTime); //This states that whenever the controller is moving the Character it will do it by the normal passing of time.
            if (Input.GetKey("left shift")) //This if statment is used to only run when Shift is pressed.
            {
                Characterspeed = Characterrunspeed; //This assigns the CharacterSpeed to the Characterrunspeed.
                CharacterjumpSpeed = 12.0f; //This assigns and increases the CharacterjumpSpeed.
            }
            else
            {
                Characterspeed = 8.0f; //This assigns the regular CharacterSpeed.
                CharacterjumpSpeed = 8.0f; //This assigns the regular CharacterjumpSpeed.
            }
        }
    }
    //The PositionWriter method obtains that characters position and then exports it into a text file.
    private void PositionWriter()
    {
        string path = Application.dataPath + "/UserPosition.text"; //Sets the destination for the text file
        if (!File.Exists(path)) //Checks that a text document doesn't already exist
        {
            File.WriteAllText(path, CharacterPosition.ToString()); //Writes the users position into a text file
        }
    }
}

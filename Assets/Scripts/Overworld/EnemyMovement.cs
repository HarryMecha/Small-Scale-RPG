using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))] //This states that NavMeshAgent is required in this script.
public class EnemyMovement : MonoBehaviour
{
    private bool Entered; //This is a boolean the dictates whether a Character GameObject has colldied with an Enemy GameObject and entered a battle.
    private Rigidbody rb; //This is the RigidBody that is attached to the Enemy GameObject.
    public void Awake()
    {
        DontDestroyOnLoad(this); //This makes sure that the Character GameObject is not destroyed when the scene is changed.
        rb = GetComponent<Rigidbody>(); //This assigns the rigidbody to the current rigidody component attached to the Enemy GameObject.
    }
    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //This retrieves the name of the scene.
        string SceneName = currentScene.name; //This assigns the SceneName string the name of the current scene.
        if (SceneName != ("Battle")) //This if statment is used to restrict the Enemy GameObject's movement to make sure that the Enemy doesn't move if these conditions are not met.
        {
            if ((OptionsConfig.PauseMenuOpen == false))  //This if statment is used to allow the Enemy GameObject's movement if the PauseMenu is closed
            {
                this.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true; //This turns on the NavMeshAgent connected to the EnemyGameObject allowing it to move towards the Character GameObject.
            }
            else //This else statment is used to restrict the Enemy GameObject's movement if the PauseMenu is open
            {
                this.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false; //This turns off the NavMeshAgent connected to the EnemyGameObject preventing it from moving towards the Character GameObject.
                rb.velocity = Vector3.zero; //This assigns the Enemy GameObjects velocity to 0 stopping it's movement.
                rb.angularVelocity = Vector3.zero; //This assigns the Enemy GameObjects rotational velocity to 0 stopping it's rotation.
            }
        }
        else //This else statement prevents the EnemyGameObjects movement if the scene is battle to stop is moving as it does not destroyed.
        {
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;  //This turns off the NavMeshAgent connected to the EnemyGameObject preventing it from moving towards the Character GameObject.
            rb.velocity = Vector3.zero; //This assigns the Enemy GameObjects velocity to 0 stopping it's movement.
            rb.angularVelocity = Vector3.zero; //This assigns the Enemy GameObjects rotational velocity to 0 stopping it's rotation.
        }
    }
    void OnTriggerStay(Collider other)
    { //This method only actives when a GameObject stays in the EnemyGameObjects collider.
        if (other.tag == "Player")
        { //This if statement only runs if the GameObject that entered it's collider has the tag Player.
            if ((OptionsConfig.PauseMenuOpen == false)) //This if statement only runs when the pause menu is closed.
            {
                Transform player; //This creates a new transform variable.
                player = GameObject.FindGameObjectWithTag("Player").transform; //This assigns the transform variable to the Character GameObjects position.
                UnityEngine.AI.NavMeshAgent nav; //This creates a new NavMesh variable.
                nav = GetComponent<UnityEngine.AI.NavMeshAgent>(); //This assigns the NavMesh variable to the NavMesh component atatched to the Enemy GameObject.
                nav.SetDestination(player.position); //This sets the Enemy GameObjects path to be towards the Character GameObject.
            }
        }
    }
    private void OnCollisionEnter(Collision coll) //This method only actives when a GameObject collides with the EnemyGameObjects collider.
    {
        Scene currentScene = SceneManager.GetActiveScene(); //This retrieves the name of the scene.
        string SceneName = currentScene.name; //This assigns the SceneName string the name of the current scene.
        if (coll.collider.tag == ("Player"))
        {  //This if statement only runs if the GameObject that collided with the Enemy GameObject has the tag Player.
            if (CharacterPanelConfig.CharacterWin == true) //This if statement only runs when the Character wins a battle.
            {
                Destroy(this.gameObject); //This destroys the EnemyGameObject.
                CharacterPanelConfig.CharacterWin = false; //This assigns the CharacterWin boolean to false so the if statement doesn't repeat.
            }
            if ((SceneName != ("Battle")) && !Entered) //This if statement only runs if the scene is not battle and the user has not entered the battle.
            {
                Entered = true; //This assigns the boolean to true indidctaing the Character GameObject has entered the battle.
                SceneManager.LoadScene("Battle"); //This loads in the Battle scene.
                Destroy(this.gameObject); //This destroys the EnemyGameObject.
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class NPCInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera; //This variable holds the Character's camera that is a component in it's children and is turned on and off depending on whether the user is interacting with an NPC or not.
    private GameObject NPCInteractionBox; //This variable holds the NPCInteractionBox that the user sees when it interacts with the NPC GameObject.
    public static bool IsInteracting; //This variable is a boolean that determines whether the user has pressed the Interact key, E, whilst in an GameObject with the tag NPC is in the Character GameObjects collider.
    void Start()
    {
        NPCInteractionBox = GameObject.Find("NPCInteraction"); //This assigns the NPCInteractionBox to the GameoObject NPCInteraction found in the InteractionCanvas.
        NPCInteractionBox.SetActive(false); //This sets the NPCInteractionPanel to inactive so it will not show up on start or when the InteractionCanvas is active for other menus.
        MainCamera.GetComponent<Camera>().enabled = true; //This sets the user's perspective to the main camera by it being the only active camera.
        IsInteracting = false; //This sets the boolean IsInteracting to false as the Character GameObject on start is neither whithin the NPC's collider or pressing the Interact Key,E.
    }
    void OnTriggerStay(Collider other) //This method only actives when a GameObject stays in the Character GameObjects collider.
    { 
        if (other.gameObject.tag == ("NPC")) //This if statement only runs if the GameObject within the Character's collider has the tag NPC.
        {
            if (Input.GetButton("Interact")) //This if statement only runs if while there is a GameObject with tag NPC the Interact Key, E, is pressed.
            {
                GameObject NPCNameText = GameObject.Find("NPCNameText"); //This assigns the NPCNameText GameObject to the NPCNameText attached to the NPCInteraction so that the TextMeshProUGUI component's text can be set to the npc_name in the NPCStat class.
                GameObject NPCTextText = GameObject.Find("NPCTextText"); //This assigns the NPCTextText GameObject to the NPCTextText attached to the NPCInteraction so that the TextMeshProUGUI component's text can be set to the text in the NPCStat class.
                MainCamera.GetComponent<Camera>().enabled = false; //This turns off the MainCamera in order for the user's perspective to be placed in front of the NPC by switching the cameras.
                other.GetComponentInChildren<Camera>().enabled = true; //This turns on the NPC GameObject's Camera compoenent in order for the user's perspective to be placed in front of the NPC by switching the cameras.
                NPCInteractionBox.SetActive(true); //This will turn the NPCInteractionPanel on for the user to see.
                NPCNameText.GetComponent<TextMeshProUGUI>().SetText(other.GetComponent<NPCStats>().npc_name); //This will change the TextMeshProUGUI component of the NPCNameText's text to the npc_name in the NPCStats of the NPC GameObject in the Character GameObject's collider. 
                NPCTextText.GetComponent<TextMeshProUGUI>().SetText(other.GetComponent<NPCStats>().npc_text); //This will change the TextMeshProUGUI component of the NPCTextText's text to the npc_text in the NPCStats of the NPC GameObject in the Character GameObject's collider. 
                IsInteracting = true; //This will set IsInteracting to true as the user is pressing the Interact Key, E, whilst an NPC is in the Character GameObject's collider.
            }
            if (Input.GetKeyDown(KeyCode.Return)) //This if statement will only run if the user presses return, signalling they are finished with the npc interaction.
            {
                MainCamera.GetComponent<Camera>().enabled = true; //This turns on the MainCamera in order for the user's perspective to be placed behind the Characte GameObject by switching the cameras.
                other.GetComponentInChildren<Camera>().enabled = false; //This turns off the NPC GameObject's Camera compoenent in order for the user's perspective to be placed behind the Characte GameObject by switching the cameras.
                NPCInteractionBox.SetActive(false); //This will turn off the NPCInteractionPanel so it cannot be seen by the user.
                GameObject.Find("CharacterOverworld").GetComponent<CharacterController>().enabled = true; //This will turn controls for the Character GameObject on so the user will be able to move.
                IsInteracting = false; //This will set IsInteracting to false as the user is no longer pressing the Interact Key, E, whilst an NPC is in the Character GameObject's collider.
            }
        }
    }
    void OnTriggerExit(Collider other) //This method only actives when a GameObject exits in the Character GameObjects collider.
    {
        if (other.gameObject.tag == ("NPC")) //This if statement only runs if the GameObject within the Character's collider has the tag NPC.
        {
            MainCamera.GetComponent<Camera>().enabled = true; //This turns on the NPC GameObject's Camera compoenent in order for the user's perspective to be placed in front of the NPC by switching the cameras.
            other.GetComponentInChildren<Camera>().enabled = false; //This turns off the NPC GameObject's Camera compoenent in order for the user's perspective to be placed behind the Characte GameObject by switching the cameras.
            NPCInteractionBox.SetActive(false); //This will turn off the NPCInteractionPanel so it cannot be seen by the user.
            GameObject.Find("CharacterOverworld").GetComponent<CharacterController>().enabled = true; //This will turn controls for the Character GameObject on so the user will be able to move.
            IsInteracting = false;  //This will set IsInteracting to false as the user is no longer pressing the Interact Key, E, whilst an NPC is in the Character GameObject's collider.
        }
    }
}

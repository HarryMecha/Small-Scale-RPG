using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IVH : MonoBehaviour
{
    public static int enemyspawned; //This variable holds the amount of enemies that have been instantiated in the Enemy Spawn Area it is held here so that the number can be kept whilst changning scenes.
    void Start()
    {
        DontDestroyOnLoad(this); //This makes sure that this GameObject is not destroyed when the scene is changed.
        if (FindObjectsOfType(GetType()).Length > 1) //This portion of code identifies if there are any duplicate of this GameObject and destroys them.
        {
            Destroy(gameObject);
        }
    }
}
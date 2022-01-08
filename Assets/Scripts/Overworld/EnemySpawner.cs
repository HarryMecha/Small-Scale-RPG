using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public int spawnlimit; //This is the limit of enemies that are spawned within the object
    public Rigidbody enemyPrefab; //This is the enemy prefab that will be instantiated
    Collider Collider; //This is the collider attached to the empty GameObject that makes up the area for spawning 
    Vector3 ColliderMax, ColliderMin; //These are the minimum and maximum points of the area
    private void OnTriggerEnter(Collider other)  //This method only actives when a GameObject enters the EnemySpawnArea.
    {
        Rigidbody enemyInstance; //This creates a new Rigibody variable.
        if (IVH.enemyspawned <= (spawnlimit)) //If statement that only runs whist the amount of Enemy GameObjects instantiated in the area is less than or equal the spawn limit assigned in the inspector.
        {
            if (other.gameObject.tag == ("Player")) //This if statement only runs if the GameObject that entered it's collider has the tag Player. 
                if (IVH.enemyspawned < spawnlimit) //If statement that only runs whist the amount of Enemy GameObjects instantiated in the area is less than the spawn limit assigned in the inspector.
                {
                    for (int i = 0; i < spawnlimit; i++) //This for loop instantiates an Enemy GameObject until it has reached the spawnlimit.
                    {
                        Vector3 position = new Vector3(Random.Range(ColliderMin.x, ColliderMax.x), 0, Random.Range(ColliderMin.z, ColliderMax.z)); //This sets the position of the EnemyGameObject to random coordinates that lie within the EnemySpawnArea.
                        enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity) as Rigidbody; //This instantiates the enemy prefab assigned in the inspector with a psoition defined by the Vector3 position.
                        IVH.enemyspawned++; //Adds one to the enemyspawned count.
                    }
                }
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) //If statement that only runs when all Enemy GameObjects have been destroyed. 
        {
            IVH.enemyspawned = 0; //Assigns the enemyspawned count to 0 is order to Instantiate new Enemy GameObjects.
        }
    }
    void Start()
    {
        Collider = GetComponent<Collider>(); //Gets the Collider component attached to the EnemySpawnArea GameObject.
        ColliderMin = Collider.bounds.min; //Assigns the minimum point of the collider.
        ColliderMax = Collider.bounds.max; //Assigns the maximum point of the collider.
    }
}

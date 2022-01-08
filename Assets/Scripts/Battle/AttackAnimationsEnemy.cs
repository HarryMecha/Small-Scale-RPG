using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackAnimationsEnemy : MonoBehaviour {
    public static Animator animator; //This variable will hold the Animator component attached to the EnemyGameObject.
    public static Animation animation; //This variable will hold the Animation component attached to the EnemyGameObject.
    public AnimationClip EnemyTestAnimation; //This variable will hold the AnimationClip component attached to the EnemyGameObject.
    void Start()
    {
        animator = GetComponent<Animator>(); //This will assign the variable to the Animator component attached to the EnemyGameObject.
        animation = GetComponent<Animation>();//This will assign the variable to the Animation component attached to the EnemyGameObject.
    }
}
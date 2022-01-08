using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CursorSpin : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 2f, 0); //This will rotate the GameObject this is attached to by 2 in the y direction every frame.
    }
}
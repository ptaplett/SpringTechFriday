using UnityEngine;
using System.Collections;
public class ColorChangeDevice : MonoBehaviour
{
    public void Operate() // Same name as door function
    {
        Color random = new Color(Random.Range(0f, 1f),
        Random.Range(0f, 1f), Random.Range(0f, 1f));        // Random number between 0,1
        GetComponent<Renderer>().material.color = random;   // Sets random color to objects material
    }
}

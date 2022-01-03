using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe SafetyAreaEvent
// Classe g�rant la destruction des victimes une fois mise en suret�.
// TODO Am�lioration : Compteur de victimes sauv�s ? 
public class SafetyAreaEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "recovered target")
        {
            Destroy(other.gameObject);
        }

        // On incr�mente/d�cr�mente un compteur ?
    }
}

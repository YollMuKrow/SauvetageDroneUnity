using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe SafetyAreaEvent
// Classe gérant la destruction des victimes une fois mise en sureté.
// TODO Amélioration : Compteur de victimes sauvés ? 
public class SafetyAreaEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "recovered target")
        {
            Destroy(other.gameObject);
        }

        // On incrémente/décrémente un compteur ?
    }
}

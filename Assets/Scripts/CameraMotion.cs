using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe CameraMotion
// Classe permettant le controle de la cam�ra pour se balader sur la map
// D�placement lat�ral : Q (gauche) et D (droite)
// D�placement Avant/Arri�re : Z (Avant) et S (Arri�re)
// D�placment Haut/Bas : Espace (Haut) et Controle Gauche (Bas)
// Rotation Droite/Gauche (Axe Y) : A (Gauche) et E (Droite)

public class CameraMotion : MonoBehaviour
{
    Camera cam;
    private float speed = 1f;

    private void Start()
    {
        // On r�cup�re l'objet cam�ra
         cam = GetComponent<Camera>();
    }

    // On change la postion et la rotation en fonction des touches appuy�es � chaque appel de update
    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal") * speed;
        float zAxisValue = Input.GetAxis("Vertical") * speed;
        float yAxisValue = Input.GetAxis("Jump") * speed/2;
        float yRotateValue = Input.GetAxis("Rotate Left/Right") * speed;
        transform.position = transform.position + Camera.main.transform.forward * zAxisValue + Camera.main.transform.right * xAxisValue;

        cam.transform.position = new Vector3(transform.position.x, transform.position.y + yAxisValue, transform.position.z);

        cam.transform.Rotate(Vector3.down, 20f * yRotateValue * Time.deltaTime);
    }
}

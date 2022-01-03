using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe GroundVehicleEntity
// Classe permettant la gestion d'un robot terrestre
// Chaque véhicule peut avoir trois états
//  WAITING : Le robot est en attente de personne à secourir
//  GOTO_TARGET : Le robot se déplace vers la cible qui lui a était attribué
//  GOTO_SAFETYAREA : Après avoir récupéré la cible, le robot rentre à la zone de sauvetage poru déposer la victime
public class GroundVehicleEntity : MonoBehaviour
{
    // Start is called before the first frame update
    public enum VehicleMode
    {
        WAITING,
        GOTO_TARGET,
        GOTO_SAFETYAREA
    }

    // Variable
    private Vector3 targetDestination;
    private Vector3 rotationDestination;
    private Vector3 safetyAreaPosition;
    public VehicleMode vehicleMode = VehicleMode.WAITING;

    //constant
    private readonly float rotationSpeed = 3f;
    public float speed = 2f;
    public float gravity = 9.81f;

    void Start()
    {
        Vector3 forward_world = transform.TransformDirection(Vector3.forward);
        targetDestination = new Vector3();
    }

    private void FixedUpdate()
    {
        setSafetyAreaPosition();
        switch (vehicleMode)
        {
            case VehicleMode.WAITING:
                return;

            case VehicleMode.GOTO_TARGET:
                SetRotationDestination(targetDestination);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationDestination), Time.deltaTime * rotationSpeed);
                
                Vector3 forward_world_to_target = transform.TransformDirection(Vector3.forward);
                transform.position += forward_world_to_target * speed * Time.deltaTime;
                ThereIsTargetOnPosition();
                return;

            case VehicleMode.GOTO_SAFETYAREA:
                if (getDistance(this.transform.position, safetyAreaPosition) < 4f)
                {
                    vehicleMode = VehicleMode.WAITING;
                }
                SetRotationDestination(safetyAreaPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationDestination), Time.deltaTime * rotationSpeed);

                Vector3 forward_world_to_area = transform.TransformDirection(Vector3.forward);
                transform.position += forward_world_to_area * speed * Time.deltaTime;
                return;
        }
    }

    // On donne au robot l'orientation qu'il doit prendre pour aller vers son objectif
    private void SetRotationDestination(Vector3 destinationToLookAt)
    {
        rotationDestination = (destinationToLookAt - this.transform.position);
        rotationDestination.y = 0;
    }

    private void setSafetyAreaPosition()
    {
        safetyAreaPosition = GameObject.Find("Safety_Area").transform.position;
    }

    // TODO : Un jour
    // Permet de récupérer physiquement la victime pour la déplacer
    private void RecoveredTargetByGroundVehicle(GameObject target)
    {

    }
    private float getDistance(Vector3 from, Vector3 destination)
    {
        float xa = from.x;
        float za = from.z;
        float xb = destination.x;
        float zb = destination.z;
        float distance = Mathf.Sqrt(Mathf.Pow((xb - xa), 2) + Mathf.Pow((zb - za), 2));
        
        return distance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "target found")
        {
            collision.gameObject.tag = "recovered target";
            vehicleMode = VehicleMode.GOTO_SAFETYAREA;
            // TODO : Un jour aussi
            // Remplacer par l'appel à RecoveredTargetByGroundVehicle pour déplacer la victime
            // L'objet sera ensuite détruit par la zone
            Destroy(collision.gameObject);
        }
    }

    // Correction d'un bug ou la cible n'était plus à la position 
    private void ThereIsTargetOnPosition()
    {
        if (getDistance(this.transform.position, targetDestination) < 0.5f)
        {
            vehicleMode = VehicleMode.WAITING;
        }
    }

    // public function 
    public void setTargetDestination(Vector3 destination)
    {
        targetDestination = destination;
        vehicleMode = VehicleMode.GOTO_TARGET;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}

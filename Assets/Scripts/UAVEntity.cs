using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe UAVEntity
// Classe permettant le controle, le déplacement et la récupération des victimes
// touchées par le raycast de chaque drones volant
public class UAVEntity : MonoBehaviour
{
    // Variables 
    private Rigidbody uav_rigidBody;
    private Vector3 position;
    private float altitude;

    // random
    private System.Random randomGenerator;

    // Variables modifié au fur et à mesure par les gestionnaire de drone (SET) 
    private Vector3 positionDestination;
    private Vector3 rotationDestination;

    // Variables modifié localement
    private float upDownForce = 0f;
    private float forwardForce = 0f;
    private bool destinationReached = false;
    private float maxSpeed = 2f;
    private int destinationStep;

    private List<Vector3> checkPoints;
    public List<GameObject> listOfTargetFound;

    // Constantes
    private readonly float maxUpDownForce = 20f;
    private readonly float maxForwardForce = 40f;
    private readonly float rotationSpeed = 2f;
    private readonly float stationnaryForce = 98.1f;
    private readonly Vector3 sizeMap = new(200, 10, 200);

    private void Awake()
    {
        randomGenerator = new System.Random();
        uav_rigidBody = GetComponent<Rigidbody>();

        position = uav_rigidBody.transform.position;
        this.altitude = 20f;
        
        checkPoints = new List<Vector3>();
        destinationStep = 0;

        generateCheckPoints();
        setRotationDestination(positionDestination);
        setPositionDestination(positionDestination);
    }


    private void FixedUpdate()
    {
        if (destinationReached == true && destinationStep < checkPoints.Count-1)
        {
            destinationStep++;
            setPositionDestination(checkPoints[destinationStep]);
        }
        if (destinationStep == checkPoints.Count-1)
        {
            destinationStep = 0;
            checkPoints.Clear(); 
            generateCheckPoints();

        }
        // on récupère la position du drone
        position = uav_rigidBody.transform.position;

        //On actualise l'angle de rotation et la position en cas de changement 
        // exemple : dans le menu ou par changement de cible
        setRotationDestination(positionDestination);
        setPositionDestination(positionDestination);

        // On calcul la force nécessaire pour avancer et se tenir à la bonne altitude
        MovementForwardBackward(position, positionDestination);
        MovementUpDown(position);

        //On fait tourner le drone
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationDestination), Time.deltaTime * rotationSpeed);
        
        // on ajoute les forces calculées précédemment
        uav_rigidBody.AddRelativeForce(Vector3.up * (stationnaryForce + upDownForce)); 
        uav_rigidBody.AddRelativeForce(Vector3.forward * forwardForce);

        // On normalise la vitesse finale
        normalyzeSpeedValues();
    }

    private void Update()
    {
        updateRaycastTarget();
    }

    
    //---------------------- private functions - movement ------------------------
    private void MovementUpDown(Vector3 from)
    {

        if (from.y < altitude)
        {
            upDownForce = maxUpDownForce;
        }
        else if (from.y > altitude)
        {
            upDownForce = -maxUpDownForce;
        }
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
    private void MovementForwardBackward(Vector3 from, Vector3 destination)
    {
        float distance = getDistance(destination, from);
        if (distance > 5f && !destinationReached)
        {
            forwardForce = maxForwardForce;
        }
        else if (distance < 5f)
        {
            destinationReached = true;
            forwardForce = -maxForwardForce;
        }
    }

    // On normalise la vitesse du drone pour que les accélération et décélération soit plus douces
    private void normalyzeSpeedValues()
    {
        uav_rigidBody.velocity = Vector3.ClampMagnitude(uav_rigidBody.velocity ,Mathf.Lerp(uav_rigidBody.velocity.magnitude, maxSpeed, Time.deltaTime * 5f));
    }

    //---------------------- private functions - raycast ------------------------
    private void updateRaycastTarget()
    {
        List<GameObject> tempTargetList = this.gameObject.GetComponentInChildren<RaycastCamera>().getTargetsFound();
        if(tempTargetList.Count > 0)
        {
            foreach(GameObject target in tempTargetList)
            {
                if (!listOfTargetFound.Contains(target))
                {
                    listOfTargetFound.Add(target);
                }
            }
            this.gameObject.GetComponentInChildren<RaycastCamera>().resetTargetsFound();
        }
    }


    //---------------------- private function - Generate position --------------
    private Vector3 getRandomVector3(int rangeX, int rangeY, int rangeZ)
    {
        return new Vector3(((float)randomGenerator.NextDouble()) * rangeX,
                           ((float)randomGenerator.NextDouble()) * rangeY,
                           ((float)randomGenerator.NextDouble()) * rangeZ);
    }

    private void generateCheckPoints()
    {
        int x_part = (int)sizeMap.x / 3;
        int z_part = (int)sizeMap.z / 3;
        for (int x = 0; x < 3; x++)
            for (int z = 0; z < 3; z++)
                checkPoints.Add(getRandomVector3((x_part + x_part * x)-((int)sizeMap.x/2), 10, (z_part + z_part * z) - ((int)sizeMap.z / 2)));
        setPositionDestination(checkPoints[destinationStep]);
    }

    //---------------------- public functions ---------------------------
    public void setPositionDestination(Vector3 destination)
    {
        positionDestination = destination;
        destinationReached = false;
    }

    public void setRotationDestination(Vector3 destinationToLookAt)
    {
        rotationDestination = (destinationToLookAt - this.transform.position);
        rotationDestination.y = 0;
    }

    public void setAltitude(float altitude)
    {
        this.altitude = altitude;
    }

    public void setSpeed(float speed)
    {
        this.maxSpeed = speed;
    }

    public void setRaycast(bool isActivate)
    {
        this.gameObject.GetComponentInChildren<RaycastCamera>().setRaycast(isActivate);
    }


    public List<GameObject> getUAVlocalListOfTarget()
    {
        if (listOfTargetFound.Count == 0)
            return new List<GameObject>();
        else
            return listOfTargetFound;
    }

    public void resetListOfTarget()
    {
        listOfTargetFound.Clear();
    }

}
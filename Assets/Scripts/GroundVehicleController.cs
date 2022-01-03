using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe GroundVehicleController
// Classe permettant de gérer l'équipe de robot terrestre en leur attribuant des cibles 
public class GroundVehicleController : MonoBehaviour
{
    // Start is called before the first frame update

    private List<GameObject> targetsFound;
    private List<GameObject> targetsAssigned;

    private List<GameObject> listOfUAV;
    private List<GameObject> listOfGroundVehicle;

    GameObject uavSpawnerEntity;
    GameObject groundVehicleSpawnerEntity;
    // Update is called once per frame
    private void Start()
    {
        uavSpawnerEntity = GameObject.Find("Spawn_Drone");
        listOfUAV = uavSpawnerEntity.GetComponent<UAVSpawner>().getListOfUAV();

        groundVehicleSpawnerEntity = GameObject.Find("Spawn_Vehicule");
        listOfGroundVehicle = groundVehicleSpawnerEntity.GetComponent<GroundVehicleSpawner>().getListOfGroundVehicle();
        
        targetsFound = new List<GameObject>();
        targetsAssigned = new List<GameObject>();
    }

    void FixedUpdate()
    {
        listOfUAV = uavSpawnerEntity.GetComponent<UAVSpawner>().getListOfUAV();
        listOfGroundVehicle = groundVehicleSpawnerEntity.GetComponent<GroundVehicleSpawner>().getListOfGroundVehicle();

        updateTargetList();
        AssignTargetToVehicle();
    }

    //private fonctions
    private void updateTargetList()
    {
        foreach (GameObject uav in listOfUAV)
        {
            List<GameObject> temporaryTarget = uav.GetComponent<UAVEntity>().getUAVlocalListOfTarget();
            if (temporaryTarget.Count > 0)
                foreach (GameObject target in temporaryTarget)
                    if (!targetsFound.Contains(target) /**&& !targetsAssigned.Contains(target)**/)
                    {
                        targetsFound.Add(target);
                    }
            uav.GetComponent<UAVEntity>().resetListOfTarget();
        }
    }

    private GameObject getFirstOperationalVehicle()
    {
        GameObject tmp = null;
        bool notFound = true;
        int i = 0;
        while(notFound && i < listOfGroundVehicle.Count)
        {
            GroundVehicleEntity component = listOfGroundVehicle[i].GetComponent<GroundVehicleEntity>();
            if (component.vehicleMode == GroundVehicleEntity.VehicleMode.WAITING)
            {
                notFound = false;
                tmp = listOfGroundVehicle[i];
            }
            i++;
        }

        return tmp;
    }
    private void AssignTargetToVehicle()
    {
        if (targetsFound.Count > 0) {
            GameObject targetToAssigned = targetsFound[0];
            GameObject vehicle = getFirstOperationalVehicle();

            if (vehicle != null && targetsFound.Count > 0)
            {
                if (!targetsAssigned.Contains(targetsFound[0]))
                {
                    targetsAssigned.Add(targetsFound[0]);
                    GroundVehicleEntity component = vehicle.GetComponent<GroundVehicleEntity>();
                    component.setTargetDestination(targetToAssigned.transform.position);
                    targetsFound.RemoveAt(0);
                    return;
                }
                return;
            }
        }
    }
}
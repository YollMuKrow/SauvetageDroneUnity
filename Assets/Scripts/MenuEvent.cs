using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe MenuEvent
// Classe gérant tout les events depuis le menu et envoie ces données aux classes respectives 
public class MenuEvent : MonoBehaviour
{
    private UAVSpawner uAVSpawner;
    private TargetSpawner targetSpawner;
    private GroundVehicleSpawner groundVehicleSpawner;
    private GroundVehicleController groundVehicleController;

    // Start is called before the first frame update
    void Start()
    {
        uAVSpawner = GameObject.Find("Spawn_Drone").GetComponent<UAVSpawner>();
        targetSpawner = GameObject.Find("Spawn_Target").GetComponent<TargetSpawner>();
        groundVehicleSpawner = GameObject.Find("Spawn_Vehicule").GetComponent<GroundVehicleSpawner>();
        groundVehicleController = GameObject.Find("Antenna tower").GetComponent<GroundVehicleController>();
    }

    public void altitudeUpdate(float value)
    {
        foreach(GameObject drone in uAVSpawner.getListOfUAV())
        {
            drone.GetComponent<UAVEntity>().setAltitude(value);
        }
    }

    public void droneSpeedUpdate(float value)
    {
        foreach (GameObject drone in uAVSpawner.getListOfUAV())
        {
            drone.GetComponent<UAVEntity>().setSpeed(value);
        }
    }

    public void droneSizeUpdate(float value)
    {
        uAVSpawner.setUAVSize((int)value);
    }

    public void droneRaycastUpdate(bool value)
    {
        foreach (GameObject drone in uAVSpawner.getListOfUAV())
        {
            drone.GetComponent<UAVEntity>().setRaycast(value);
        }
    }

    public void vehicleSpeedUpdate(float value)
    {
        foreach (GameObject vehicle in groundVehicleSpawner.getListOfGroundVehicle())
        {
            vehicle.GetComponent<GroundVehicleEntity>().setSpeed(value);
        }
    }

    public void vehicleSizeUpdate(float value)
    {
        groundVehicleSpawner.setGroundVehicleSize((int)value);
    }

    public void targetSizeUpdate(float value)
    {
        targetSpawner.setTargetSize((int)value);
    }


}

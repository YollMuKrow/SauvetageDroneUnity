using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe GroundVehicleSpawner
// Classe permettant l'instanciation de chaque véhicules/robot terrestre
public class GroundVehicleSpawner : MonoBehaviour
{
    public static GroundVehicleSpawner instance = null;

    // List of GameObject
    private List<GameObject> listOfGroundVehicle;

    // Initial set up
    private int GroundVehicleSize = 2;

    // Prefab GameObjects
    public GameObject groundVehicleEntityPrefab;

    // random
    private int randomSeed = 0;
    private System.Random randomGenerator;

    // positions and rotation
    private Vector3[] groundVehiclePositions;
    private Vector3[] groundVehicleRotations;

    // Spawner range and GameObject
    public GameObject groundVehicleEntitySpawn;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        listOfGroundVehicle = new List<GameObject>();
        groundVehicleEntitySpawn = GameObject.Find("Spawn_Vehicule");
        generateGroundVehiclePositions(GroundVehicleSize);
        generateGroundVehicleEntities();
    }

    //--------------------- GETTER AND SETTER -----------------------
    // Get GameObjects
    public List<GameObject> getListOfGroundVehicle()
    {
        return listOfGroundVehicle;
    }

    // Get size
    public int getGroundVehicleSize()
    {
        return this.GroundVehicleSize;
    }

    // Set size
    public void setGroundVehicleSize(int newGroundVehicleSize)
    {
        if (newGroundVehicleSize > GroundVehicleSize)
        {
            increaseVehicle(newGroundVehicleSize - GroundVehicleSize);
        }
        if (newGroundVehicleSize < GroundVehicleSize)
        {
            decreaseVehicle(GroundVehicleSize - newGroundVehicleSize);
        }
        this.GroundVehicleSize = newGroundVehicleSize;
    }


    //--------------------- private functions ----------------

    private Vector3 getRandomVector3(int rangeX, int rangeY, int rangeZ)
    {
        return new Vector3(((float)randomGenerator.NextDouble() - .5f) * rangeX,
                           ((float)randomGenerator.NextDouble() - .5f) * rangeY,
                           ((float)randomGenerator.NextDouble() - .5f) * rangeZ);
    }

    // Generate random positions and rotations for each entities
    private void generateGroundVehiclePositions(int groundVehicleSize)
    {
        randomGenerator = new System.Random(randomSeed);

        groundVehiclePositions = new Vector3[groundVehicleSize];
        groundVehicleRotations = new Vector3[groundVehicleSize];

        for (int i = 0; i < groundVehicleSize; i++)
        {
            groundVehiclePositions[i] = getRandomVector3(10, 0, 10);
            groundVehicleRotations[i] = getRandomVector3(1, 0, 0);
        }
    }

    // Entities generation method
    private void generateGroundVehicleEntities()
    {
        listOfGroundVehicle = new List<GameObject>();

        for (int i = 0; i < GroundVehicleSize; i++)
        {
            GameObject newEntity = Instantiate(groundVehicleEntityPrefab,
                                               Vector3.zero,
                                               Quaternion.identity,
                                               groundVehicleEntitySpawn.transform);

            newEntity.transform.position = groundVehicleEntitySpawn.transform.position + Vector3.Scale(groundVehicleEntitySpawn.transform.localScale, groundVehiclePositions[i]);
            newEntity.transform.rotation = Quaternion.Euler(groundVehicleRotations[i]);
            listOfGroundVehicle.Add(newEntity);
        }
    }
    private void increaseVehicle(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 newPos = getRandomVector3(20, 0, 20);
            Vector3 newRot = getRandomVector3(1, 0, 0);

            GameObject newEntity = Instantiate(groundVehicleEntityPrefab,
                                               Vector3.zero,
                                               Quaternion.identity,
                                               groundVehicleEntitySpawn.transform);

            newEntity.transform.position = groundVehicleEntitySpawn.transform.position + Vector3.Scale(groundVehicleEntitySpawn.transform.localScale, newPos);
            newEntity.transform.rotation = Quaternion.Euler(newRot);

            listOfGroundVehicle.Add(newEntity);
        }
    }
    private void decreaseVehicle(int number)
    {
        for (int i = 0; i < number; i++)
        {
            int random_value = randomGenerator.Next(listOfGroundVehicle.Count);
            GameObject drone = listOfGroundVehicle[random_value];
            listOfGroundVehicle.RemoveAt(random_value);
            Destroy(drone);
        }
    }
}

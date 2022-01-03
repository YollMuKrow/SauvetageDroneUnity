using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe UAVSpawner
// Classe permettant l'instanciation de chaque drones volant (UAV)
public class UAVSpawner : MonoBehaviour
{
    public static UAVSpawner instance = null;

    // List of GameObject
    private List<GameObject> listOfUAV;

    // Initial set up
    private int UAVSize = 5;

    // positions and rotation
    private Vector3[] uavPositions;
    private Vector3[] uavRotations;

    // Spawner range and GameObject
    public GameObject uAVEntitySpawn;

    // Prefab GameObjects
    public GameObject uAVEntityPrefab;

    // random
    private int randomSeed = 0;
    private System.Random randomGenerator;

    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        listOfUAV = new List<GameObject>();
        uAVEntitySpawn = GameObject.Find("Spawn_Drone");
        generateUAVPositions(UAVSize);
        generateUAVEntities();
    }

    //--------------------- GETTER AND SETTER -----------------------
    // Get GameObjects
    public List<GameObject> getListOfUAV()
    {
        return this.listOfUAV;
    }

    // Get size
    public int getUAVSize()
    {
        return this.UAVSize;
    }

    // Set size
    public void setUAVSize(int newUAVSize)
    {
        if (newUAVSize > UAVSize)
        {
            increaseUAV(newUAVSize - UAVSize);
        }
        if (newUAVSize < UAVSize)
        {
            decreaseUAV(UAVSize - newUAVSize);
        }
        this.UAVSize = newUAVSize;
    }


    //--------------------- private functions ----------------

    private Vector3 getRandomVector3(int rangeX, int rangeY, int rangeZ)
    {
        return new Vector3(((float)randomGenerator.NextDouble() - .5f) * rangeX,
                           ((float)randomGenerator.NextDouble() - .5f) * rangeY,
                           ((float)randomGenerator.NextDouble() - .5f) * rangeZ);
    }

    // Generate random positions and rotations for each entities
    private void generateUAVPositions(int uavSize)
    {
        randomGenerator = new System.Random(randomSeed);

        uavPositions = new Vector3[uavSize];
        uavRotations = new Vector3[uavSize];

        for (int i = 0; i < uavSize; i++)
        {
            uavPositions[i] = getRandomVector3(20, 5, 20);
            uavRotations[i] = getRandomVector3(1, 0, 0);
        }
    }

    // Entities generation method
    private void generateUAVEntities()
    {
        listOfUAV = new List<GameObject>();
        for (int i = 0; i < UAVSize; i++)
        {
            GameObject newEntity = Instantiate(uAVEntityPrefab,
                                               Vector3.zero,
                                               Quaternion.identity,
                                               uAVEntitySpawn.transform);

            newEntity.transform.position = uAVEntitySpawn.transform.position + Vector3.Scale(uAVEntitySpawn.transform.localScale, uavPositions[i]);
            newEntity.transform.rotation = Quaternion.Euler(uavRotations[i]);

            listOfUAV.Add(newEntity);
        }
    }

    private void increaseUAV(int number)
    {
        for(int i = 0; i < number; i++)
        {
            Vector3 newPos = getRandomVector3(20, 5, 20);
            Vector3 newRot = getRandomVector3(1, 0, 0);

            GameObject newEntity = Instantiate(uAVEntityPrefab,
                                               Vector3.zero,
                                               Quaternion.identity,
                                               uAVEntitySpawn.transform);
            newEntity.transform.position = uAVEntitySpawn.transform.position + Vector3.Scale(uAVEntitySpawn.transform.localScale, newPos);
            newEntity.transform.rotation = Quaternion.Euler(newRot);

            listOfUAV.Add(newEntity);
        }
    }
    private void decreaseUAV(int number)
    {
        for (int i = 0; i < number; i++)
        {
            int random_value = randomGenerator.Next(listOfUAV.Count);
            GameObject drone = listOfUAV[random_value];
            listOfUAV.RemoveAt(random_value);
            Destroy(drone);
        }
    }
}

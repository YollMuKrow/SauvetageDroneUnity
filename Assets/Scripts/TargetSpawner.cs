using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code by Alexis Lheritier
// Classe TargetSpawner
// Classe permettant l'instanciation de chaque victimes dans une zone aléatoire
public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner instance = null;
    private List<GameObject> listOfTarget;
    private int TargetSize = 10;

    // positions and rotation
    private Vector3[] targetPositions;
    private Vector3[] targetRotations;
    public GameObject targetEntitySpawn;
    public GameObject targetEntityPrefab;

    // random
    private int randomSeed = 1;
    private System.Random randomGenerator;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        targetEntitySpawn = GameObject.Find("Spawn_Target");
        generateTargetPositions(TargetSize);
        generateTargetEntities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //--------------------- GETTER AND SETTER -----------------------

    public List<GameObject> getListOfTarget()
    {
        return this.listOfTarget;
    }

    public int getTargetSize()
    {
        return this.TargetSize;
    }


    public void setTargetSize(int newTargetSize)
    {
        if (newTargetSize > TargetSize)
        {
            increaseTarget(newTargetSize - TargetSize);
        }
        if (newTargetSize < TargetSize)
        {
            decreaseTarget(TargetSize - newTargetSize);
        }
        this.TargetSize = newTargetSize;
    }

    //--------------------- private functions ----------------
    private Vector3 getRandomVector3(int rangeX, int rangeY, int rangeZ)
    {
        return new Vector3(((float)randomGenerator.NextDouble() - .5f) * rangeX,
                           ((float)randomGenerator.NextDouble() - .5f) * rangeY,
                           ((float)randomGenerator.NextDouble() - .5f) * rangeZ);
    }

    private void generateTargetPositions(int targetSize)
    {
        randomGenerator = new System.Random(randomSeed);

        targetPositions = new Vector3[targetSize];
        targetRotations = new Vector3[targetSize];

        for (int i = 0; i < targetSize; i++)
        {
            targetPositions[i] = getRandomVector3(180, 0, 180);
            targetRotations[i] = getRandomVector3(1, 1, 1);
        }
    }

    private void generateTargetEntities()
    {
        listOfTarget = new List<GameObject>();

        for (int i = 0; i < TargetSize; i++)
        {
            GameObject newEntity = Instantiate(targetEntityPrefab,
                                               Vector3.zero,
                                               Quaternion.identity,
                                               targetEntitySpawn.transform);

            newEntity.transform.position = targetEntitySpawn.transform.position + Vector3.Scale(targetEntitySpawn.transform.localScale, targetPositions[i]);
            newEntity.transform.rotation = Quaternion.Euler(targetRotations[i]);
            newEntity.tag = "target to save";

            listOfTarget.Add(newEntity);
        }
    }
    private void increaseTarget(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 newPos = getRandomVector3(180, 0, 180);
            Vector3 newRot = getRandomVector3(1, 1, 1);

            GameObject newEntity = Instantiate(targetEntityPrefab,
                                               Vector3.zero,
                                               Quaternion.identity,
                                               targetEntitySpawn.transform);

            newEntity.transform.position = targetEntitySpawn.transform.position + Vector3.Scale(targetEntitySpawn.transform.localScale, newPos);
            newEntity.transform.rotation = Quaternion.Euler(newRot);

            listOfTarget.Add(newEntity);
        }
    }
    private void decreaseTarget(int number)
    {
        for (int i = 0; i < number; i++)
        {
            int random_value = randomGenerator.Next(listOfTarget.Count);
            GameObject drone = listOfTarget[random_value];
            listOfTarget.RemoveAt(random_value);
            Destroy(drone);
        }
    }

}

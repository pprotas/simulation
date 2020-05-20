using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Class representing a lane in the simulation
/// </summary>
[Serializable]
class Lane : MonoBehaviour
{
    // The ID is equal to the agreed upon lane id.
    [SerializeField]
    public string id;

    public List<Light> TrafficLights;

    [SerializeField]
    public int maxCars = 20;

    [SerializeField]
    public int spawnDelay = 5;

    [SerializeField]
    public bool HasCollision = true;

    [SerializeField]
    public GameObject carPrefab;

    [SerializeField]
    public List<GameObject> cars;

    // A list of waitpoints that represent the route a car has to follow
    [SerializeField]
    public List<WayPoint> wayPoints;

    // Busy lanes don't spawn cars
    private bool IsBusy
    {
        get
        {
            if (cars?.Count > maxCars - 1)
            {
                return true;
            }
            return false;
        }
    }

    // Used for spawning cars. Prevents cars from being spawned too often
    float timeSinceLastCall;

    void Start()
    {
        cars = new List<GameObject>();
        wayPoints = new List<WayPoint>(gameObject.GetComponentsInChildren<WayPoint>());
        TrafficLights = gameObject.GetComponentsInChildren<Light>().ToList();
    }

    void Update()
    {
        timeSinceLastCall += Time.deltaTime;
    }

    // Forces the simulation to spawn a car at the specified location
    private void SpawnCar(GameObject carPrefab, Vector3 cd)
    {
        GameObject car = Instantiate(carPrefab, cd, new Quaternion());
        if (car)
        {
            car.AddComponent<TrafficUser>();
            car.GetComponent<TrafficUser>().lane = gameObject;
            car.GetComponent<TrafficUser>().HasCollision = HasCollision;
            cars.Add(car);
            transform.parent.GetComponent<State>().IsUpdated = true;
        }
    }

    // Spawns a car only if specific conditions are met 
    public void TrySpawnCar()
    {
        if (IsBusy)
        {
            return;
        }
        if (wayPoints.Count <= 0)
        {
            return;
        }
        if (timeSinceLastCall <= spawnDelay)
        {
            return;
        }
        timeSinceLastCall = 0;
        SpawnCar(carPrefab, wayPoints[0].transform.position);
    }

    public override string ToString()
    {
        return $"{id}";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[Serializable]
class Lane : MonoBehaviour
{
    [SerializeField]
    public string id;

    [SerializeField]
    public Light trafficLight;

    [SerializeField]
    public int maxCars = 3;

    [SerializeField]
    public GameObject carPrefab;

    [SerializeField]
    public List<GameObject> cars;

    [SerializeField]
    public List<WayPoint> wayPoints;

    private bool IsBusy
    {
        get
        {
            if (cars?.Count > maxCars)
            {
                return true;
            }
            return false;
        }
    }

    float timeSinceLastCall;

    void Start()
    {
        cars = new List<GameObject>();
        wayPoints = new List<WayPoint>(gameObject.GetComponentsInChildren<WayPoint>());
        trafficLight = trafficLight ?? gameObject.GetComponentInChildren<Light>();
    }

    void Update()
    {
        timeSinceLastCall += Time.deltaTime;
    }

    private void SpawnCar(GameObject carPrefab, Vector3 cd)
    {
        GameObject car = Instantiate(carPrefab, cd, new Quaternion());
        if (car)
        {
            car.AddComponent<TrafficUser>();
            car.GetComponent<TrafficUser>().lane = gameObject;
            cars.Add(car);
        }
    }

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
        if (timeSinceLastCall <= 3)
        {
            return;
        }
        timeSinceLastCall = 0;
        SpawnCar(carPrefab, wayPoints[0].transform.position);
    }

    private void DespawnCar(GameObject car)
    {
        cars.Remove(car);
    }

    public override string ToString()
    {
        return $"{id}";
    }
}

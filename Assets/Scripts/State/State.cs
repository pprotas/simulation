using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.Collections.Specialized;

internal class State : MonoBehaviour
{
    [SerializeField]
    ObservableCollection<Lane> lanes;

    private bool _isUpdated;
    public bool IsUpdated
    {
        get
        {
            var tmp = _isUpdated;
            _isUpdated = false; // State is not updated anymore if someone accesses this property
            return tmp;
        }
        set
        {
            _isUpdated = value;
        }
    }

    void Start()
    {
        lanes = new ObservableCollection<Lane>(gameObject.GetComponentsInChildren<Lane>());
        lanes.CollectionChanged += Lanes_Changed;
    }

    void Update()
    {
        SpawnCarAtRandom();
    }

    private void Lanes_Changed(object sender, NotifyCollectionChangedEventArgs e)
    {
        IsUpdated = true;
    }

    Lane GetLaneById(string id)
    {
        Lane result;

        if (!string.IsNullOrEmpty(id))
        {
            result = lanes.SingleOrDefault(lane => lane.id == id);
        }
        else
        {
            print($"Empty id for Lane specified");
            return null;
        }

        if (result)
        {
            return result;
        }
        print($"No lane found with the id of {id}");
        return null;
    }

    private void SetLightForId(string laneId, LightColor color)
    {
        GetLaneById(laneId).trafficLight.color = color;
    }

    public void SetAllLights(LightColor color)
    {
        foreach(Lane lane in lanes)
        {
            lane.trafficLight.color = color;
        }
    }

    public void SetAllLights(ControllerData data)
    {
        foreach (FieldInfo field in data.GetType().GetFields(BindingFlags.Instance |
                       BindingFlags.Static |
                       BindingFlags.NonPublic |
                       BindingFlags.Public))
        {
            LightColor color = GetLightColor(data, field);
            string id = field.Name;
            GetLaneById(id).trafficLight.color = color;
            SetLightForId(id, color);
        }
    }

    private LightColor GetLightColor(ControllerData data, FieldInfo field)
    {
        return (LightColor)field.GetValue(data);
    }

    private void SpawnCarAtRandom()
    {
        lanes[Random.Range(lanes.IndexOf(lanes.First()), lanes.Count)].TrySpawnCar();
    }
}
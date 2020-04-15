using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.Collections.Specialized;
using Boo.Lang;

internal class State : MonoBehaviour
{
    [SerializeField]
    public ObservableCollection<Lane> lanes;
    private Light[] Lights;

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

        Lights = gameObject.GetComponentsInChildren<Light>();
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

    Light GetLightById(string id)
    {
        Light result;

        if (!string.IsNullOrEmpty(id))
        {
            result = Lights.SingleOrDefault(light => light.id == id);
        }
        else
        {
            print($"Empty id for light specified");
            return null;
        }

        if (result)
        {
            return result;
        }
        print($"No light found with the id of {id}");
        return null;
    }

    private void SetLightForId(string laneId, LightColor color)
    {
        Light lane = GetLightById(laneId);
        if (lane != null)
        {
            lane.color = color;
        }
    }

    public void SetAllLights(LightColor color)
    {
        foreach (Light light in Lights)
        {
            light.color = color;
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

    public string ToJson()
    {
        string result = "{\n";
        foreach(Lane lane in lanes)
        {
            result += $"\"{lane.id}\": {lane.cars.Count},\n";
        }
        result = result.Substring(0, result.Length - 2);
        result += "\n}";
        return result;
    }
}
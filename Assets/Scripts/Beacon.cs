using UnityEngine;

public class Beacon
{
    private float lat;
    private float lng;
    public string fullName;
    public string shortName;
    public BeaconType type;
    public BeaconImpl impl;
    private GameObject instance;
    public float distance = 0f;
    public NDTarget minimapTarget;

    public float Lat
    {
        get => lat;
        set
        {
            if (value > MapHelper.RIGHT_BOTTOM_LAT && value < MapHelper.LEFT_TOP_LAT)
            {
                lat = value;
            }
        }
    }

    public float Lng
    {
        get => lng;
        set
        {
            if (value > MapHelper.LEFT_TOP_LONG && value < MapHelper.RIGHT_BOTTOM_LONG)
            {
                lng = value;
            }
        }
    }

    public GameObject Instance
    {
        get => instance;
        set
        {
            if (value != null)
            {
                instance = value;
            }
        }
    }

    public Beacon(float lat, float lng, string fullName, string shortName, BeaconType type, BeaconImpl impl)
    {
        Lat = lat;
        Lng = lng;
        this.fullName = fullName;
        this.shortName = shortName;
        this.type = type;
        this.impl = impl;
    }
}
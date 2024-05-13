using UnityEngine;

public class Beacon
{
	public string fullName;
	public string shortName;

	public float distance = 0f;

	public NDTarget ndTarget;

	public BeaconType type;
	public BeaconImpl impl;
	
	private float lat;
	public float Lat
	{
		get => lat;
		set
		{
			if(value > MapHelper.RIGHT_BOTTOM_LAT &&
				value < MapHelper.LEFT_TOP_LAT)
			{
				lat = value;
			}
		}
	}

	private float lng;
	public float Lng
	{
		get => lng;
		set
		{
			if(value > MapHelper.LEFT_TOP_LONG &&
				value < MapHelper.RIGHT_BOTTOM_LONG)
			{
				lng = value;
			}
		}
	}

	private GameObject go;
	public GameObject GO
	{
		get => go;
		set
		{
			if(value != null)
			{
				go = value;
				go.name = fullName;
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
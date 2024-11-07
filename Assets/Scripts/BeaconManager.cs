using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BeaconManager : MonoBehaviour
{
	public static BeaconManager Instance;

	[SerializeField]
	private GameObject defaultBeaconPrefab;

	[SerializeField]
	private GameObject customBeaconPrefab;

	[SerializeField]
	private Sprite vorBeaconIcon;

	[SerializeField]
	private Sprite dmeBeaconIcon;

	[SerializeField]
	private Sprite vordmeBeaconIcon;

	// Хардкод элементов, т.к. так пока что хочет клиент
	public List<Beacon> beacons = new()
	{
		new(52.283920052645854f, 104.28749346364783f, "Иркутск", "UIII", BeaconType.VORDME, BeaconImpl.DEFAULT),
		new(51.83649802605531f, 107.58217090901114f, "Улан-Удэ", "UUD", BeaconType.VORDME, BeaconImpl.DEFAULT),
		new(52.05368214721413f, 113.46639143197622f, "Чита", "HTA", BeaconType.VORDME, BeaconImpl.DEFAULT),
		new(56.29262613720093f, 101.71032953908764f, "Братск", "BTK", BeaconType.VORDME, BeaconImpl.DEFAULT),

		new(53.39154577756098f, 109.00635888385294f, "Усть-Баргузин", "UIUI", BeaconType.DME, BeaconImpl.DEFAULT),
		new(52.51695723137947f, 111.53375523122716f, "Сосново-Озерское", "UIUS", BeaconType.DME, BeaconImpl.CUSTOM),
		new(55.78296050231323f, 109.54890162529887f, "Нижнеангарск", "NZG", BeaconType.DME, BeaconImpl.CUSTOM)
	};

	public void Initialize()
	{
		Instance = this;
		SpawnBeacons();
		GetBeaconsDistancesAndSort();
	}

	private void Update()
	{
		if (Aircraft.Instance.isMoving)
		{
			GetBeaconsDistancesAndSort();
		}
	}

	private void SpawnBeacons()
	{
		for (int i = 0; i < beacons.Count; i++)
		{
			beacons[i].GO = Instantiate(beacons[i].impl switch
			{
				BeaconImpl.DEFAULT => defaultBeaconPrefab,
				BeaconImpl.CUSTOM => customBeaconPrefab,
				_ => throw new NotImplementedException(),
			},
			Bootstrap.Instance.mapRect);

			Vector2 position = MapHelper.Instance.LatLongToXY(beacons[i].Lat, beacons[i].Lng);
			beacons[i].GO.GetComponent<RectTransform>().anchoredPosition = position;
			beacons[i].AnchoredPos = position;

			NDTarget target = beacons[i].GO.AddComponent<NDTarget>();
			target.label = beacons[i].shortName;
			target.iconSprite = beacons[i].type switch
			{
				BeaconType.VORDME => vordmeBeaconIcon,
				BeaconType.DME => dmeBeaconIcon,
				_ => throw new NotImplementedException(),
			};

			if (beacons[i].impl == BeaconImpl.CUSTOM)
			{
				beacons[i].GO.GetComponent<Text>().text = beacons[i].fullName;
			}
		}
	}

	public void GetBeaconsDistancesAndSort()
	{
		(float, float) aircraftPoint = MapHelper.Instance.XYToLatLong(Bootstrap.Instance.aircraftRect.anchoredPosition);
		for (int i = 0; i < beacons.Count; i++)
		{

			// beacons[i].distance = Vector2.Distance(Bootstrap.Instance.aircraftRect.position,
			// 											beacons[i].GO.transform.position);
			(float, float) beaconPoint = MapHelper.Instance.XYToLatLong(beacons[i].AnchoredPos);
			beacons[i].distance = MapHelper.Instance.DistanceLatLngMiles(aircraftPoint, beaconPoint);
		}

		beacons = beacons.OrderBy(b => b.distance).ToList();
	}
}

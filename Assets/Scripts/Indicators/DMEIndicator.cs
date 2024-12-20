using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DMEIndicator : MonoBehaviour
{
	[SerializeField]
	private List<Beacon> closestBeacons;

	public event BeaconsDataHandler ClosestBeaconsChangedEvent;

	public void Initialize()
	{
		GetDMEBeaconsAndDistances();
	}

	private void Update()
	{
		if(Aircraft.Instance.isMoving)
		{
			GetDMEBeaconsAndDistances();
			OnClosestBeaconsChange();
		}
	}

	private void GetDMEBeaconsAndDistances()
	{
		closestBeacons = BeaconManager.Instance.beacons.Where(beacon =>
			beacon.type == BeaconType.DME || beacon.type == BeaconType.VORDME
		).Take(2).ToList();
	}

	public void OnClosestBeaconsChange()
	{
		ClosestBeaconsChangedEvent?.Invoke(
			(closestBeacons[0].fullName, closestBeacons[1].fullName),
			(closestBeacons[0].distance, closestBeacons[1].distance)
		);
	}
}
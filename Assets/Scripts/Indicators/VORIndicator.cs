using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VORIndicator : MonoBehaviour
{
	[SerializeField]
	private RectTransform shortRangeArrow;

	[SerializeField]
	private RectTransform midRangeArrow;

	private const float ARROWS_ROTATION_SPEED = 1000f;

	[SerializeField]
	private RectTransform aircraftRect;

	[SerializeField]
	private List<Beacon> closestBeacons;

	private Vector3 shortDir, midDir;
	private float shortEulerAngle, midEulerAngle;
	private Quaternion shortTargetRot, midTargetRot;
	private Quaternion shortAngleClamped, midAngleClamped;

	public event BeaconsDataHandler ClosestBeaconsChangedEvent;

	public void Initialize()
	{
		GetVORBeaconsAndRotations();

		shortRangeArrow.rotation = shortTargetRot;
		midRangeArrow.rotation = midTargetRot;

		aircraftRect.rotation = Bootstrap.Instance.aircraftRect.rotation;
	}

	private void Update()
	{
		if(Bootstrap.Instance.aircraft.isMoving)
		{
			GetVORBeaconsAndRotations();
			OnClosestBeaconsChange();
			UpdateArrowIndicator();
		}
	}

	private void UpdateArrowIndicator()
	{
		shortRangeArrow.rotation = Quaternion.RotateTowards(shortRangeArrow.rotation, shortTargetRot, ARROWS_ROTATION_SPEED * Time.deltaTime);
		midRangeArrow.rotation = Quaternion.RotateTowards(midRangeArrow.rotation, midTargetRot, ARROWS_ROTATION_SPEED * Time.deltaTime);
		aircraftRect.rotation = Bootstrap.Instance.aircraftRect.rotation;
	}

	private void GetVORBeaconsAndRotations()
	{
		closestBeacons = BeaconManager.Instance.beacons.Where(beacon =>
		{
			return beacon.type == BeaconType.VOR || beacon.type == BeaconType.VORDME;
		}).Take(2).ToList();

		shortDir = closestBeacons[0].GO.transform.position - Bootstrap.Instance.aircraftRect.position;
		midDir = closestBeacons[1].GO.transform.position - Bootstrap.Instance.aircraftRect.position;

		shortEulerAngle = Mathf.Atan2(shortDir.normalized.y, shortDir.normalized.x) * Mathf.Rad2Deg - 90f;
		midEulerAngle = Mathf.Atan2(midDir.normalized.y, midDir.normalized.x) * Mathf.Rad2Deg - 90f;

		shortTargetRot = Quaternion.Euler(new Vector3(0f, 0f, shortEulerAngle));
		midTargetRot = Quaternion.Euler(new Vector3(0f, 0f, midEulerAngle));
	}

	public void OnClosestBeaconsChange()
	{
		shortAngleClamped = Quaternion.Euler(0f, 0f, shortEulerAngle);
		shortAngleClamped.w *= -1f;

		midAngleClamped = Quaternion.Euler(0f, 0f, midEulerAngle);
		midAngleClamped.w *= -1f;

		ClosestBeaconsChangedEvent?.Invoke(
			(closestBeacons[0].fullName, closestBeacons[1].fullName),
			(shortAngleClamped.eulerAngles.z, midAngleClamped.eulerAngles.z)
		);
	}
}


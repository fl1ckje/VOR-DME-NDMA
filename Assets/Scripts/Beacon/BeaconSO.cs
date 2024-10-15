using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "BeaconsScriptableObjects", order = 1)]
public class BeaconSO : ScriptableObject
{
    public List<Beacon> beacons;
}
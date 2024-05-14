using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	public static Bootstrap Instance;
	private MapHelper mapUtils;

	[Header("Editor-linked refs")]
	public RectTransform mapRect;

	public WayDrawer wayDrawer;
	public BeaconManager beaconManager;

	public VORIndicator vorIndicator;
	public DMEIndicator dmeIndicator;

	[SerializeField]
	private UIUpdater uiUpdater;

	[SerializeField]
	private GameObject aircraftPrefab;

	[Header("Runtime-linked refs")]
	public RectTransform aircraftRect;
	public Aircraft aircraft;

	private void Awake()
	{
		Instance = this;

		mapUtils = new MapHelper();
		mapUtils.Initialize();

		wayDrawer.Initialize();

		beaconManager.Initialize();

		aircraftRect = Instantiate(aircraftPrefab, mapRect).GetComponent<RectTransform>();
		aircraft = aircraftRect.GetComponent<Aircraft>();
		aircraft.Initialize();

		vorIndicator.Initialize();
		dmeIndicator.Initialize();

		uiUpdater.Initialize();
	}
}

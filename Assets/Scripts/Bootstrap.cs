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
	private UIController uiController;

	[SerializeField]
	private GameObject aircraftPrefab;

	[Header("Runtime-linked refs")]
	public RectTransform aircraftRect;

	private void Awake()
	{
		Instance = this;

		mapUtils = new MapHelper();
		mapUtils.Initialize();

		wayDrawer.Initialize();

		aircraftRect = Instantiate(aircraftPrefab, mapRect).GetComponent<RectTransform>();
		aircraftRect.GetComponent<Aircraft>().Initialize();

		beaconManager.Initialize();
		vorIndicator.Initialize();
		dmeIndicator.Initialize();

		uiController.Initialize();
	}
}

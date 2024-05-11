using UnityEngine;

public class NDTarget : MonoBehaviour
{
	public Sprite iconSprite;
	public Color iconColor = Color.magenta;
	private GameObject blip;

	private void Start()
	{
		blip = NavigationDisplay.Instance.AddObject(GetComponent<RectTransform>(), iconSprite, iconColor);
	}

	private void OnDisable()
	{
		if (blip) blip.SetActive(false);
	}

	private void OnEnable()
	{
		if (blip) blip.SetActive(true);
	}
}

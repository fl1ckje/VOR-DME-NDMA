using UnityEngine;

public class NDTarget : MonoBehaviour
{
	public string label;
	public Sprite iconSprite;
	private Color iconColor = Color.magenta;
	private GameObject blip;

	private void Start()
	{
		Vector2 position = GetComponent<RectTransform>().anchoredPosition;
		blip = NavigationDisplay.Instance.AddObject(position, iconSprite, iconColor, label);
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

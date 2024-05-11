using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]

public class NavigationDisplay : MonoBehaviour
{
	[SerializeField]
	private RectTransform maskRect;

	[SerializeField]
	private NDBlip[] pool;

	private RectTransform aircraft;
	public static NavigationDisplay Instance;

	private void Awake() => Instance = this;

	private void Start() => aircraft = Bootstrap.Instance.aircraftTransform;

	public Vector2 TransformPosition(Vector2 position) => position - aircraft.anchoredPosition;

	public GameObject AddObject(RectTransform rect, Sprite icon, Color color, string name)
	{
		for (int i = 0; i < pool.Length; i++)
		{
			if (!pool[i].isActiveAndEnabled)
			{
				pool[i].rect = rect;

				pool[i].image.rectTransform.SetParent(maskRect);
				pool[i].image.sprite = icon;
				pool[i].image.color = color;

				TMP_Text label = pool[i].GetComponentInChildren<TMP_Text>();
				label.color = color;
				label.text = name;

				pool[i].gameObject.SetActive(true);
				
				return pool[i].gameObject;
			}
		}

		return null;
	}

	private void LateUpdate()
	{
		for (int i = 0; i < pool.Length; i++)
		{
			if (pool[i].isActiveAndEnabled) pool[i].UpdatePosition();
		}
	}
}

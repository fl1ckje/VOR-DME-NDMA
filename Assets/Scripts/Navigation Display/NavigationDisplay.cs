using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]

public class NavigationDisplay : MonoBehaviour
{
	public static NavigationDisplay Instance;

	[SerializeField]
	private RectTransform maskRect;

	[SerializeField]
	private NDBlip[] pool;

	private RectTransform aircraftRect;

	private void Awake() => Instance = this;

	private void Start()
	{
		aircraftRect = Bootstrap.Instance.aircraftRect;
	}

	public Vector2 TransformPosition(Vector2 position)
	{
		return position - aircraftRect.anchoredPosition;
	}

	public GameObject AddObject(Vector2 position, Sprite icon, Color color, string name)
	{
		for(int i = 0; i < pool.Length; i++)
		{
			if(!pool[i].isActiveAndEnabled)
			{
				pool[i].MapPosition = position;

				pool[i].Image.rectTransform.SetParent(maskRect);
				pool[i].Image.sprite = icon;
				pool[i].Image.color = color;

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
		for(int i = 0; i < pool.Length; i++)
		{
			if(pool[i].isActiveAndEnabled)
			{
				pool[i].UpdatePosition();
			}
		}
	}
}

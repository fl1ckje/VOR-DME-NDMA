using UnityEngine;
using UnityEngine.UI;

public class NDBlip : MonoBehaviour
{
	public RectTransform rect;
	public Image image;

	public void UpdatePosition()
	{
		image.rectTransform.localPosition = NavigationDisplay.Instance.TransformPosition(rect.anchoredPosition);
	}
}

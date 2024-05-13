using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Метка маяка на нав. дисплее
/// </summary>
public class NDBlip : MonoBehaviour
{
	/// <summary>
	/// Позиция на карте
	/// </summary>
	public Vector2 MapPosition;

	/// <summary>
	/// Изображение метки
	/// </summary>
	public Image Image;

	public void UpdatePosition()
	{
		Image.rectTransform.localPosition = NavigationDisplay.Instance.TransformPosition(MapPosition);
	}
}

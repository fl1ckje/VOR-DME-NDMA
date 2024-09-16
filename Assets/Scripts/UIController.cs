using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
	[Header("Short-range beacon DME fields")]
	[SerializeField]
	private InputField shortRangeDMENameTextField;

	[SerializeField]
	private InputField shortRangeDMEDistanceTextField;

	[Header("Mid-range DME beacon fields")]
	[SerializeField]
	private InputField midRangeDMENameTextField;

	[SerializeField]
	private InputField midRangeDMEDistanceTextField;

	public void Initialize()
	{
		Bootstrap.Instance.aircraft.OnPositionChange();
		Bootstrap.Instance.wayDrawer.OnMousePositionChange();
		Bootstrap.Instance.vorIndicator.OnClosestBeaconsChangeUpdate();

		Bootstrap.Instance.dmeIndicator.ClosestBeaconsChangedEvent += UpdateDMETextFields;
		Bootstrap.Instance.dmeIndicator.OnClosestBeaconsChange();
	}

	private string FormatFloat(float value) =>
		Math.Round(value, 3).ToString();

	private void UpdateDMETextFields((string, string) names, (float, float) distances)
	{
		shortRangeDMENameTextField.text = names.Item1;
		midRangeDMENameTextField.text = names.Item2;

		shortRangeDMEDistanceTextField.text = FormatFloat(distances.Item1);
		midRangeDMEDistanceTextField.text = FormatFloat(distances.Item2);
	}
}

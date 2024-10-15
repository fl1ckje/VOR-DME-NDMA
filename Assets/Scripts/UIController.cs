using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
	[Header("Short-range DME beacon")]
	[SerializeField]
	private Toggle shortRangeDMEToggle;

	[SerializeField]
	private GameObject shortRangeDMEInfoContainer;

	[SerializeField]
	private InputField shortRangeDMENameTextField;

	[SerializeField]
	private InputField shortRangeDMEDistanceTextField;

	[Header("Mid-range DME beacon")]
	[SerializeField]
	private Toggle midRangeDMEToggle;

	[SerializeField]
	private GameObject midRangeDMEInfoContainer;

	[SerializeField]
	private InputField midRangeDMENameTextField;

	[SerializeField]
	private InputField midRangeDMEDistanceTextField;

	[Header("Short-range VOR beacon")]
	[SerializeField]
	private Toggle shortRangeVORToggle;

	[SerializeField]
	private GameObject shortRangeVORInfoContainer;

	[SerializeField]
	private InputField shortRangeVORNameTextField;

	[SerializeField]
	private InputField shortRangeVORAzimuthTextField;

	[Header("Mid-range VOR beacon")]
	[SerializeField]
	private Toggle midRangeVORToggle;

	[SerializeField]
	private GameObject midRangeVORInfoContainer;

	[SerializeField]
	private InputField midRangeVORNameTextField;

	[SerializeField]
	private InputField midRangeVORAzimuthTextField;

	[Header("Aircraft Params Controls")]
	[SerializeField]
	private Slider aircraftSpeedSlider;

	[SerializeField]
	private Text aircraftSpeedText;

	public void Initialize()
	{
		Bootstrap.Instance.aircraft.OnPositionChange();
		Bootstrap.Instance.wayDrawer.OnMousePositionChange();

		Bootstrap.Instance.dmeIndicator.ClosestBeaconsChangedEvent += UpdateDMETextFields;
		Bootstrap.Instance.dmeIndicator.OnClosestBeaconsChange();

		Bootstrap.Instance.vorIndicator.ClosestBeaconsChangedEvent += UpdateVORTextFields;
		Bootstrap.Instance.vorIndicator.OnClosestBeaconsChange();

		shortRangeDMEToggle.onValueChanged.AddListener(
			(isOn) => shortRangeDMEInfoContainer.SetActive(isOn)
		);

		midRangeDMEToggle.onValueChanged.AddListener(
			(isOn) => midRangeDMEInfoContainer.SetActive(isOn)
		);

		shortRangeVORToggle.onValueChanged.AddListener(
			(isOn) => shortRangeVORInfoContainer.SetActive(isOn)
		);

		midRangeVORToggle.onValueChanged.AddListener(
			(isOn) => midRangeVORInfoContainer.SetActive(isOn)
		);
	}

	private string FormatFloat(float value) =>
		Math.Round(value, 3).ToString();

	private void UpdateDMETextFields((string, string) names, (float, float) values)
	{
		shortRangeDMENameTextField.text = names.Item1;
		shortRangeDMEDistanceTextField.text = FormatFloat(values.Item1);

		midRangeDMENameTextField.text = names.Item2;
		midRangeDMEDistanceTextField.text = FormatFloat(values.Item2);
	}

	private void UpdateVORTextFields((string, string) names, (float, float) values)
	{
		shortRangeVORNameTextField.text = names.Item1;
		shortRangeVORAzimuthTextField.text = FormatFloat(values.Item1);

		midRangeVORNameTextField.text = names.Item2;
		midRangeVORAzimuthTextField.text = FormatFloat(values.Item2);
	}
}

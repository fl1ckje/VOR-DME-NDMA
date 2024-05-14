using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WayDrawer : MonoBehaviour
{
	public static WayDrawer Instance;

	public LineRenderer Line;

	private const float LINE_WIDTH = 0.015f;
	private const float WAYPOINTS_DELTA = 0.06f;

	private Vector3 previousPosition;

	private RectTransform mapRect;
	private readonly Vector3[] mapBounds = new Vector3[4];

	private Vector3 mouseWorldPos;
	private Vector2 mouseAnchoredPos;
	private Vector2 mouseAnchoredOffset;

	public event Position2DHandler MousePositionChangedEvent;

	public void Initialize()
	{
		Instance = this;

		Line = GetComponent<LineRenderer>();
		Line.positionCount = 1;
		Line.startWidth = Line.endWidth = LINE_WIDTH;

		previousPosition = transform.position;

		mapRect = Bootstrap.Instance.mapRect;
		mapRect.GetWorldCorners(mapBounds);

		mouseAnchoredOffset = new(MapHelper.Instance.MapSize.x / 2f, -MapHelper.Instance.MapSize.y / 2f);
	}

	public void OnMousePositionChange()
	{
		mouseAnchoredPos = Bootstrap.Instance.mapRect.InverseTransformPoint(mouseWorldPos);
		mouseAnchoredPos += mouseAnchoredOffset;

		MousePositionChangedEvent?.Invoke(MapHelper.Instance.XYToLatLong(mouseAnchoredPos));
	}

	private void Update()
	{
		if(Input.GetAxisRaw("Mouse X") != 0f || Input.GetAxisRaw("Mouse Y") != 0f)
		{
			GetMousePosition();

			if(MousePosInMapBounds())
			{
				OnMousePositionChange();
			}
		}
	}

	public bool MousePosInMapBounds()
	{
		return mouseWorldPos.x > mapBounds[1].x && mouseWorldPos.x < mapBounds[2].x &&
				mouseWorldPos.y > mapBounds[0].y && mouseWorldPos.y < mapBounds[1].y;
	}

	private void GetMousePosition()
	{
		RectTransformUtility.ScreenPointToWorldPointInRectangle(Bootstrap.Instance.mapRect,
									Input.mousePosition, Camera.main, out mouseWorldPos);

		mouseWorldPos.x = Mathf.Clamp(mouseWorldPos.x, mapBounds[1].x, mapBounds[2].x);
		mouseWorldPos.y = Mathf.Clamp(mouseWorldPos.y, mapBounds[0].y, mapBounds[1].y);
	}

	public void CreateSingleWaypoint()
	{
		if(!MousePosInMapBounds())
			return;

		Line.positionCount = 1;
		Line.SetPosition(0, mouseWorldPos);
	}

	public void CreateMultipleWaypoints()
	{
		if(!MousePosInMapBounds()) return;

		if(Vector3.Distance(mouseWorldPos, previousPosition) > WAYPOINTS_DELTA)
		{
			Line.positionCount++;

			if(previousPosition == transform.position)
			{
				Line.SetPosition(0, mouseWorldPos);
			}
			else
			{
				Line.SetPosition(Line.positionCount - 1, mouseWorldPos);
			}

			previousPosition = mouseWorldPos;
		}
	}
}

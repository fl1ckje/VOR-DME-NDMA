using UnityEngine;

/// <summary>
/// Сущность ЛА
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class Aircraft : MonoBehaviour
{
	private RectTransform rect;

	/// <summary>
	/// Отрисовщик маршрута
	/// </summary>
	private WayDrawer wayDrawer;

	/// <summary>
	/// Точки маршрута
	/// </summary>
	private Vector3[] positions;

	/// <summary>
	/// Индекс текущей точки назначения
	/// </summary>
	private int wayPointIndex = 0;

	/// <summary>
	/// Состояние движения ЛА
	/// </summary>
	public bool isMoving;

	/// <summary>
	/// Скорость движения
	/// </summary>
	private const float MOVE_SPEED = 1.5f;

	/// <summary>
	/// Угол поворота при движении до текущей точки
	/// </summary>
	private float targetAngle;

	private Vector2 targetPosition;
	private Vector2 targetDirection;

	private const float START_LAT = 52.29775689091742f;
	private const float START_LONG = 104.2721954266937f;

	public event Position2DHandler PositionChangedEvent;

	/// <summary>
	/// Инициализирует ЛА на карте
	/// </summary>
	public void Initialize()
	{
		wayDrawer = WayDrawer.Instance;
		rect = GetComponent<RectTransform>();
		SetPosition(START_LAT, START_LONG);
	}

	/// <summary>
	/// Срабатывает при смене позиции
	/// </summary>
	public void OnPositionChange()
	{
		PositionChangedEvent?.Invoke(MapHelper.Instance.XYToLatLong(rect.anchoredPosition));
	}

	private void Update()
	{
		if(Input.GetMouseButton(0))
		{
			wayDrawer.CreateMultipleWaypoints();
		}

		if(Input.GetMouseButtonDown(0))
		{
			wayDrawer.CreateSingleWaypoint();
		}

		if(Input.GetMouseButtonUp(0) && wayDrawer.MousePosInMapBounds())
		{
			GetWaypoints();
		}

		Move();
	}

	/// <summary>
	/// Устанавливает позицию ЛА по широте и долготе
	/// </summary>
	/// <param name="lat">Широта</param>
	/// <param name="lng">Долгота</param>
	public void SetPosition(float lat, float lng)
	{
		lng = Mathf.Clamp(lng, MapHelper.LEFT_TOP_LONG, MapHelper.RIGHT_BOTTOM_LONG);
		lat = Mathf.Clamp(lat, MapHelper.RIGHT_BOTTOM_LAT, MapHelper.LEFT_TOP_LAT);

		rect.anchoredPosition = MapHelper.Instance.LatLongToXY(lat, lng);
	}

	/// <summary>
	/// Устанавливает позицию ЛА по пикселю
	/// </summary>
	/// <param name="pixel"></param>
	//public void SetPosition(Vector2 pixel)
	//{
	//    pixel.x = Mathf.Clamp(pixel.x, 0, MapHelper.Instance.MapSize.x);
	//    pixel.y = Mathf.Clamp(pixel.y, 0, -MapHelper.Instance.MapSize.y);

	//    rectTransform.anchoredPosition = pixel;
	//}

	/// <summary>
	/// Получает путевые точки, нанесённые пользователем на карте
	/// </summary>
	private void GetWaypoints()
	{
		positions = new Vector3[wayDrawer.line.positionCount];
		wayDrawer.line.GetPositions(positions);

		// позже надо сделать переключатель для этой штуки,
		// чтобы в тестах не ждать ВС к подходу слишком долго
		// transform.position = positions[0];

		isMoving = true;
		wayPointIndex = 0;
	}

	/// <summary>
	/// Двигает ЛА по карте
	/// </summary>
	private void Move()
	{
		if(isMoving && positions.Length > 0)
		{
			targetPosition = positions[wayPointIndex];
			transform.position = Vector2.MoveTowards(transform.position, targetPosition, MOVE_SPEED * Time.deltaTime);

			targetDirection = targetPosition - (Vector2)transform.position;
			targetAngle = Mathf.Atan2(targetDirection.normalized.y, targetDirection.normalized.x) * Mathf.Rad2Deg + 90f;

			if(targetAngle != 90f)
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), 0.3f);

			if(Vector2.Distance(transform.position, targetPosition) < 0.0005f)
			{
				wayPointIndex++;
			}

			if(wayPointIndex > positions.Length - 1)
			{
				isMoving = false;
			}

			OnPositionChange();
		}
	}
}
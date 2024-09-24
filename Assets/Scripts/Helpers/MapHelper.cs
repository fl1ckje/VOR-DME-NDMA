using UnityEngine;

public class MapHelper
{
	public static MapHelper Instance;

	/// <summary>
	/// Размер изображения карты (вместе с ним и GO с компонентом Image)
	/// </summary>
	public readonly Vector2Int MapSize = new(1370, 770);

	/// <summary>
	/// Широта левого верхнего пикселя
	/// </summary>
	public const float LEFT_TOP_LAT = 58.237404433709145f;

	/// <summary>
	/// Долгота левого верхнего пикселя
	/// </summary>
	public const float LEFT_TOP_LNG = 92.4907995336642f;

	/// <summary>
	/// Широта правого нижнего пикселя
	/// </summary>
	public const float RIGHT_BOTTOM_LAT = 50.39817431586244f;

	/// <summary>
	/// Долгота правого нижнего пикселя
	/// </summary>
	public const float RIGHT_BOTTOM_LNG = 116.5069128147453f;

	/// <summary>
	/// Шаг в 1 пиксель по широте
	/// </summary>
	private float latStep;

	/// <summary>
	/// Шаг в 1 пиксель по долготе
	/// </summary>
	private float lngStep;

	/// <summary>
	/// Коэффициент компенсации ошибки по широте
	/// </summary>
	private const float latErrCompensation = 13.7f;

	/// <summary>
	/// Коэффициент компенсации ошибки по долготе
	/// </summary>
	private const float lngErrCompensation = 1.43f;

	/// <summary>
	/// Радиус Земли в метрах
	/// </summary>
	private const float EARTH_RADIUS = 6371000f;

	/// <summary>
	/// Приведение метров к морским милям
	/// </summary>
	private const float MILES_PER_METER = 0.00053995680345572f;

	public void Initialize()
	{
		Instance = this;

		latStep = (LEFT_TOP_LAT - RIGHT_BOTTOM_LAT) / MapSize.y;
		lngStep = (RIGHT_BOTTOM_LNG - LEFT_TOP_LNG) / MapSize.x;
	}

	public Vector2 LatLongToXY(float lat, float lng)
	{
		lng = Mathf.Clamp(lng, LEFT_TOP_LNG, RIGHT_BOTTOM_LNG);
		lat = Mathf.Clamp(lat, RIGHT_BOTTOM_LAT, LEFT_TOP_LAT);

		return new Vector2((lng - LEFT_TOP_LNG) / lngStep - lngErrCompensation,
							-(LEFT_TOP_LAT - lat) / latStep - latErrCompensation);
	}

	public (float, float) XYToLatLong(Vector2 pixel)
	{
		pixel.x = Mathf.Clamp(pixel.x, 0, MapSize.x);
		pixel.y = Mathf.Clamp(pixel.y, -MapSize.y, 0);

		return (pixel.y * latStep + LEFT_TOP_LAT, pixel.x * lngStep + LEFT_TOP_LNG);
	}

	/// <summary>
	/// Считает расстояние между точками в метрах по формуле Хаверсина
	/// </summary>
	/// <param name="p1">Широта и долгота точки 1</param>
	/// <param name="p2">Широта и долгота точки 2</param>
	/// <returns></returns>
	public float DistanceLatLngMeters((float, float) p1, (float, float) p2)
	{
		// Преобразование составляющих координат в радианы
		float lat1Rad = ToRadians(p1.Item1);
		float lon1Rad = ToRadians(p1.Item2);
		float lat2Rad = ToRadians(p2.Item1);
		float lon2Rad = ToRadians(p2.Item2);

		// Вычисление разницы в широте и долготе между точками
		float dLat = lat2Rad - lat1Rad;
		float dLon = lon2Rad - lon1Rad;

		// Вычисление основных угловых функций
		float a = Mathf.Sin(dLat / 2f) * Mathf.Sin(dLat / 2f) +
				Mathf.Cos(lat1Rad) * Mathf.Cos(lat2Rad) *
				Mathf.Sin(dLon / 2f) * Mathf.Sin(dLon / 2f);

		// Расчет расстояния
		return EARTH_RADIUS * 2f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1f - a));
	}

	/// <summary>
	/// Считает расстояние между точками в морских милях по формуле Хаверсина
	/// </summary>
	/// <param name="p1">Широта и долгота точки 1</param>
	/// <param name="p2">Широта и долгота точки 2</param>
	/// <returns></returns>
	public float DistanceLatLngMiles((float, float) p1, (float, float) p2) =>
		DistanceLatLngMeters(p1, p2) * MILES_PER_METER;

	private float ToRadians(float deg) => deg * Mathf.Deg2Rad;
}

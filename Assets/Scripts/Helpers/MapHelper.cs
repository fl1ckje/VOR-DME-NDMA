using UnityEngine;

public class MapHelper
{
	public static MapHelper Instance;

	public readonly Vector2Int MapSize = new(1370, 770);

	public const float LEFT_TOP_LAT = 58.237404433709145f;
	public const float LEFT_TOP_LNG = 92.4907995336642f;

	public const float RIGHT_BOTTOM_LAT = 50.39817431586244f;
	public const float RIGHT_BOTTOM_LNG = 116.5069128147453f;

	private float latStep;
	private float lngStep;

	private const float latErrCompensation = 13.7f;
	private const float lngErrCompensation = 1.43f;

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
}

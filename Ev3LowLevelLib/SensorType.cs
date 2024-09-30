namespace Ev3LowLevelLib
{
	public enum SensorType
	{
		None = 0,
		MediumMotor = 1,
		LargeMotor = 2,
		TouchSensor = 3,
		UsSensor = 4,
		IrSensor = 5,
		GyroSensor = 6,
		ColorSensor = 7,
	}

	public class SensorData
	{
		public static readonly Dictionary<SensorType, SensorData> AllSensorData = new Dictionary<SensorType, SensorData>()
		{
			{ SensorType.None, new SensorData() { SensorType = SensorType.None, Dcm = 0, Conn = 126 } },
			{ SensorType.MediumMotor, new SensorData() { SensorType = SensorType.MediumMotor, Dcm = 8, Conn = 125 } },
			{ SensorType.LargeMotor, new SensorData() { SensorType = SensorType.LargeMotor, Dcm = 7, Conn = 125 } },
			{ SensorType.TouchSensor, new SensorData() { SensorType = SensorType.TouchSensor, Dcm = 16, Conn = 121 } },
			{ SensorType.UsSensor, new SensorData() { SensorType = SensorType.TouchSensor, Dcm = 30, Conn = 122 } },
			{ SensorType.IrSensor, new SensorData() { SensorType = SensorType.TouchSensor, Dcm = 33, Conn = 122 } },
			{ SensorType.GyroSensor, new SensorData() { SensorType = SensorType.TouchSensor, Dcm = 32, Conn = 122 } },
			{ SensorType.ColorSensor, new SensorData() { SensorType = SensorType.TouchSensor, Dcm = 29, Conn = 122 } },
		};

		public SensorType SensorType { get; set; }
		public int Dcm { get; set; } // Type in typedata
		public int Conn { get; set; }
	}
}

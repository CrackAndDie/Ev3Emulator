using Ev3EmulatorCore.Lms.Cui;

namespace Ev3EmulatorCore.Lms.Lms2012
{
	public partial class LmsInstance
	{
		private LmsInstance() { }

		private static LmsInstance _instance;
		public static LmsInstance Inst
		{
			get
			{
				_instance ??= new LmsInstance();
				return _instance;
			}
			private set { _instance = value; }
		}

		public UiFileHandler UiFileHandler = new UiFileHandler();
		public CuiClass.UI_GLOBALS UiInstance = new CuiClass.UI_GLOBALS();
		public CuiClass CuiClass = new CuiClass();
		public ClcdClass ClcdClass = new ClcdClass();
		public DterminalClass DterminalClass = new DterminalClass();
	}
}

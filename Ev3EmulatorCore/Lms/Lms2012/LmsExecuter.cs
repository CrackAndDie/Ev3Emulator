using EV3DecompilerLib.Decompile;

namespace Ev3EmulatorCore.Lms.Lms2012
{
	public partial class LmsInstance
	{
		/// <summary>
		/// Executing the bytecode
		/// </summary>
		/// <param name="bytecode">Bytecode</param>
		/// <param name="globals">Globals</param>
		/// <param name="locals">Locals</param>
		/// <returns>Stat</returns>
		public lms2012.DSPSTAT ExecuteBytecode(byte[] bytecode, byte[] globals, byte[] locals)
		{
			// TODO
			return lms2012.DSPSTAT.NOBREAK;
		}
	}
}

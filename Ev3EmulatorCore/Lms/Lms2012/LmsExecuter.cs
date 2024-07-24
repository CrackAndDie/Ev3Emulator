using EV3DecompilerLib.Decompile;
using static EV3DecompilerLib.Decompile.lms2012;

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
		public unsafe DSPSTAT ExecuteByteCode(IP bytecode, GP globals, LP locals)
		{
			// TODO
			return DSPSTAT.NOBREAK;
		}
	}
}

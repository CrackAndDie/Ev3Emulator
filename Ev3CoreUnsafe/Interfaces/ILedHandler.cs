using static Ev3CoreUnsafe.Defines;
using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Interfaces
{
	public interface ILedHandler
	{
		/// <summary>
		/// Sets the state of a led
		/// </summary>
		/// <param name="buff">First one is <see cref="LEDPATTERN"/> and the second one is a state></param>
		public void SetLed(sbyte[] buff);
	}
}

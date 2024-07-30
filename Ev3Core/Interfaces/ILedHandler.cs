using static Ev3Core.Defines;
using Ev3Core.Enums;

namespace Ev3Core.Interfaces
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

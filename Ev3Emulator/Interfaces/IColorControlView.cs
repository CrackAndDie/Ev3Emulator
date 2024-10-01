using Hypocrite.Core.Interfaces.Presentation;
using System;

namespace Ev3Emulator.Interfaces
{
	public interface IColorControlView : IView
	{
		// reflect and ambient. color is controlled directly
		event Action<byte, byte> UpdateSensor;
	}
}

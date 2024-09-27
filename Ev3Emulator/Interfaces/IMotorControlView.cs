using Hypocrite.Core.Interfaces.Presentation;
using System;

namespace Ev3Emulator.Interfaces
{
	public interface IMotorControlView : IView
	{
		event Action<int> UpdateSpeed;
	}
}

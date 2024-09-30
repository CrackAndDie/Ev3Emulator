using Hypocrite.Core.Interfaces.Presentation;
using System;

namespace Ev3Emulator.Interfaces
{
	public interface ITouchControlView : IView
	{
		event Action TouchPressed;
		event Action TouchReleased;
	}
}

using Hypocrite.Core.Interfaces.Presentation;
using System;

namespace Ev3Emulator.Interfaces
{
	public interface IGyroControlView : IView
	{
		event Action<float> UpdateAngle;
	}
}

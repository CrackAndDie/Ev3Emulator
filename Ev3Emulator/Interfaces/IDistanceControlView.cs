using Hypocrite.Core.Interfaces.Presentation;
using System;

namespace Ev3Emulator.Interfaces
{
	public interface IDistanceControlView : IView
	{
		event Action<float> UpdateDistance;
	}
}

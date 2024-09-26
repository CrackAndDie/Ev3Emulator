using Hypocrite.Core.Interfaces.Presentation;
using System;

namespace Ev3Emulator.Interfaces
{
    public interface IMainView : IView
	{
        event Action CenterButtonPressed;
        event Action CenterButtonReleased;
		byte[] GetButtons();
    }
}

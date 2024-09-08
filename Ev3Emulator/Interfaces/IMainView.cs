using System;

namespace Ev3Emulator.Interfaces
{
    public interface IMainView
    {
        event Action CenterButtonPressed;
        event Action CenterButtonReleased;
		byte[] GetButtons();
    }
}

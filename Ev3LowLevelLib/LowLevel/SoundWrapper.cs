using System;
using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class SoundWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_sound_playTone(reg_w_sound_playToneAction playTone);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_sound_playToneAction(short freq, ushort duration);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_sound_isSoundPlaying(reg_w_sound_isSoundPlayingAction isSoundPlaying);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int reg_w_sound_isSoundPlayingAction();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_sound_playSound(reg_w_sound_playSoundAction playSound);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_sound_playSoundAction(string name, int size, int rate);

		public static void Init(Action<short, ushort> playTone, Func<int> isSoundPlaying, Action<string, int, int> playSound)
		{
			_playTone = playTone;
			_isSoundPlaying = isSoundPlaying;
			_playSound = playSound;

			reg_w_sound_playTone(PlayTone);
			reg_w_sound_isSoundPlaying(IsSoundPlaying);
			reg_w_sound_playSound(PlaySound);
		}

		private static void PlayTone(short freq, ushort duration)
		{
			_playTone?.Invoke(freq, duration);
		}

		private static int IsSoundPlaying()
		{
			return _isSoundPlaying?.Invoke() ?? 0;
		}

		private static void PlaySound(string name, int size, int rate)
		{
			_playSound?.Invoke(name, size, rate);
		}

		private static Action<short, ushort> _playTone;
		private static Func<int> _isSoundPlaying;
		private static Action<string, int, int> _playSound;
	}
}

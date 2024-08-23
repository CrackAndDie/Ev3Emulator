using System;
using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class SoundWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_sound_playTone(reg_w_sound_playToneAction playTone);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_sound_playToneAction(short freq);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_sound_isTonePlaying(reg_w_sound_isTonePlayingAction isTonePlaying);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int reg_w_sound_isTonePlayingAction();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_sound_playChunk(reg_w_sound_playChunkAction playChunk);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_sound_playChunkAction(IntPtr data, int len);

		public static void Init(reg_w_sound_playToneAction playTone, reg_w_sound_isTonePlayingAction isTonePlaying, reg_w_sound_playChunkAction playChunk)
		{
			reg_w_sound_playTone(playTone);
			reg_w_sound_isTonePlaying(isTonePlaying);
			reg_w_sound_playChunk(playChunk);
		}
	}
}

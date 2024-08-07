namespace Ev3CoreUnsafe.Lms2012.Interfaces
{
	public interface ITimer
	{
		ULONG cTimerGetuS();
		ULONG cTimerGetmS();

		void cTimerWait();

		void cTimerReady();

		void cTimerRead();

		void cTimerReaduS();
	}
}

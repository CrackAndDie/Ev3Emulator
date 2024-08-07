namespace Ev3CoreUnsafe.Lms2012.Interfaces
{
	public interface IMove
	{
		void cMove8to8();

		void cMove8to16();

		void cMove8to32();

		void cMove8toF();

		void cMove16to8();

		void cMove16to16();

		void cMove16to32();

		void cMove16toF();

		void cMove32to8();

		void cMove32to16();

		void cMove32to32();

		void cMove32toF();

		void cMoveFto8();

		void cMoveFto16();

		void cMoveFto32();

		void cMoveFtoF();

		void cMoveInitBytes();

		void cMoveRead8();

		void cMoveRead16();

		void cMoveRead32();

		void cMoveReadF();

		void cMoveWrite8();

		void cMoveWrite16();

		void cMoveWrite32();

		void cMoveWriteF();
	}
}

namespace Ev3Core.Lms2012.Interfaces
{
    public interface ILms
    {
        void Error();
        void Nop();
        void ObjectStop();
        void ObjectStart();
        void ObjectTrig();
        void ObjectWait();
        void ObjectCall();
        void ObjectReturn();
        void ObjectEnd();
        void ProgramStart();
        void ProgramStop();
        void Sleep();
        void ProgramInfo();
        void DefLabel();
        void Do();
        void Probe();
        void BreakPoint();
        void BreakSet();
        void Random();
        void Info();
        void Strings();
        void MemoryWrite();
        void MemoryRead();
        void cBranchJr();
        void PortCnvOutput();
        void PortCnvInput();
        void NoteToFreq();
        void System();
        void Monitor();

        void TstClose();
        void Tst();
    }
}

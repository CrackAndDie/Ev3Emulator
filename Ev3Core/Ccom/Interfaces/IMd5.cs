namespace Ev3Core.Ccom.Interfaces
{
	public interface IMd5
	{
		int md5_file(ArrayPointer<UBYTE> filename, int binary, ArrayPointer<UBYTE> md5_result);
	}
}

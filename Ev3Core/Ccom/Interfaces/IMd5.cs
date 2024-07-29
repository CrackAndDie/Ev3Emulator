namespace Ev3Core.Ccom.Interfaces
{
	public interface IMd5
	{
		int md5_file(char[] filename, int binary, byte[] md5_result);
	}
}

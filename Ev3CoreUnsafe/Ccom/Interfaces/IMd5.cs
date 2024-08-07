namespace Ev3CoreUnsafe.Ccom.Interfaces
{
	public unsafe interface IMd5
	{
		int md5_file(DATA8* filename, int binary, byte* md5_result);
	}
}

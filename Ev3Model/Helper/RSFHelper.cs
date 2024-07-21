using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3ModelLib.Helper
{
    /// <summary>
    /// RSF Sound class
    /// </summary>
    public class RSFSound
    {
        public RSFSound(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ReadFromStream(stream);
            }
        }

        public RSFSound(Stream stream, long size = 0)
        {
            ReadFromStream(stream, size);
        }

        public RSFSound(byte[] data)
        {
            ReadFromData(data);
        }

        /// <summary>
        /// Read RSF from memory stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        private void ReadFromStream(Stream stream, long size = 0)
        {
            if (size == 0) size = stream.Length;
            byte[] data = new byte[size];
            stream.Read(data, 0, data.Length);
            ReadFromData(data);
        }

        /// <summary>
        /// Converted WAV data
        /// </summary>
        public byte[] SoundDataRawWav { get; private set; }

        /// <summary>
        /// Read RSF from byte array
        /// </summary>
        /// <param name="data_in"></param>
        private void ReadFromData(byte[] data_in)
        {
            ReadRSFFile(data_in);
            SoundDataRawWav = ReadRSFFile(data_in);
        }

        /// <summary>
        /// ReadFrom RSF Rudolph Sound file
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        private static byte[] ReadRSFFile(byte[] sound)
        {
            int num1 = ((int)sound[4] << 8) + (int)sound[5];
            int length = sound.Length - 8 + num1;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter((Stream)memoryStream, Encoding.UTF8))
                {
                    byte[] buffer = new byte[length];
                    Array.Copy((Array)sound, 8, (Array)buffer, 0, sound.Length - 8);
                    binaryWriter.Write("RIFF".ToCharArray());
                    binaryWriter.Write((int)(36 + length));
                    binaryWriter.Write("WAVEfmt ".ToCharArray());
                    binaryWriter.Write((int)16);
                    binaryWriter.Write((short)1);
                    binaryWriter.Write((short)1);
                    binaryWriter.Write((int)num1);
                    binaryWriter.Write((int)num1);
                    binaryWriter.Write((short)1);
                    binaryWriter.Write((short)8);
                    binaryWriter.Write("data".ToCharArray());
                    binaryWriter.Write((int)length);
                    binaryWriter.Write(buffer);
                    //TODO: padding with 0 instead of buffer -- binaryWriter.Write(sound, 8, sound.Length - 8);
                    memoryStream.Position = 0L;
                    return memoryStream.ToArray();
                }
            }
        }
    }
}

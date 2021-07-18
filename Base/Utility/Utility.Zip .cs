/******************************************************************
** Utility.Zip.cs
** @Author       : BanMing 
** @Date         : 2020/9/28 下午2:43:07
** @Description  : 
*******************************************************************/

using ICSharpCode.SharpZipLib.Zip.Compression;
using System.IO;

namespace BMBaseCore
{
    public static class Zip
    {
        public static byte[] Compress(byte[] content)
        {
            Deflater compressor = new Deflater();
            compressor.SetLevel(Deflater.BEST_COMPRESSION);

            compressor.SetInput(content);
            compressor.Finish();

            using (MemoryStream ms = new MemoryStream(content.Length))
            {
                byte[] buf = new byte[1024];
                while (!compressor.IsFinished)
                {
                    int n = compressor.Deflate(buf);
                    ms.Write(buf, 0, n);
                }
                return ms.ToArray();
            }
        }

        public static byte[] Decompress(byte[] content)
        {
            return Decompress(content, 0, content.Length);
        }

        public static byte[] Decompress(byte[] content, int offset, int count)
        {
            Inflater decompressor = new Inflater();
            decompressor.SetInput(content, offset, count);

            using (MemoryStream ms = new MemoryStream(content.Length))
            {
                byte[] buf = new byte[1024];
                while (!decompressor.IsFinished)
                {
                    int n = decompressor.Inflate(buf);
                    ms.Write(buf, 0, n);
                }
                return ms.ToArray();
            }

        }

    }
}
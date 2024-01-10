// Part of MapDataBuilder V1.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ReadNullTerminator
{
    public static class NullTerminator
    {
        public static string ReadNullTerminatedString(BinaryReader reader)
        {
            List<byte> bytes = new List<byte>();
            byte b;
            while ((b = reader.ReadByte()) != 0)
            {
                bytes.Add(b);
            }
            return Encoding.ASCII.GetString(bytes.ToArray()).ToLower();

        }
    }

}

namespace ExtractBytes
{
    public static class FileHelper
    {
        public static byte[] ExtractBytesFromBox(string filePath, uint offset, uint size)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fs.Seek(offset, SeekOrigin.Begin);

                    byte[] fileBytes = new byte[size];
                    int bytesRead = fs.Read(fileBytes, 0, (int)size);

                    if (bytesRead == size)
                    {
                        return fileBytes;
                    }
                    else
                    {
                        return null; // If read != success, then return with value null...
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}

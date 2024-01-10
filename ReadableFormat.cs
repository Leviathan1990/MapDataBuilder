// Part of MapDataBuilder V1.0


using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ScriptEditor
{
    public class EditorFunctions
    {
        public static bool IsReadableFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return extension == ".oms" || extension == ".cfg" || extension == ".ai" || extension == ".txt";
        }
    }
}


namespace ConfigFileReader
{

    public static class FileHelper
    {
        public static string ConvertBytesToString(Byte[] bytes, bool includeBytes = false)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in bytes)
            {
                if (IsPrintableAscii(b))
                {
                    if (includeBytes)
                    {
                        stringBuilder.AppendFormat("{0:X2} ", b);
                    }
                    stringBuilder.Append((char)b);
                }
                else
                {
                    stringBuilder.AppendFormat("0x{0:X2} ", b);
                }
            }

            return stringBuilder.ToString();
        }


        public static bool IsPrintableAscii(byte b)
        {
            return b >= 32 && b <= 126;
        }

        // Scripter extras
    }

}

namespace Commandlists
{
    public static class ListBoxInitializer
    {
            //      For information.oms file!
        public static void InitializeListBox(ListBox listBox)
        {
            listBox.Items.Add("si_Map_Description(\"\");");
            listBox.Items.Add("si_Map_Author(\"char name max 8chars\");");
            listBox.Items.Add("si_Map_Players(int num of players);");
            listBox.Items.Add("si_Map_Width(int width);");
            listBox.Items.Add("si_Map_Height(int height);");
        }
    }

    public static class ListBoxInitializer2
    {
        public static void InitializeListBox2(ListBox listBoxx)
        {
            listBoxx.Items.Add("scene_bgcolor_red(64);");
            listBoxx.Items.Add("scene_bgcolor_green(0);");
            listBoxx.Items.Add("scene_bgcolor_blue(8);");
            listBoxx.Items.Add("scene_sun_blue(300);");
            listBoxx.Items.Add("scene_sun_green(300);");
            listBoxx.Items.Add("scene_sun_red(256);");
            listBoxx.Items.Add("scene_sun_pitch(0.4);");
            listBoxx.Items.Add("scene_sun_heading(0.2);");
            listBoxx.Items.Add("scene_ambinet_green(25);");
            listBoxx.Items.Add("scene_ambinet_blue(25);");
            listBoxx.Items.Add("scene_ambinet_red(0);");
       
        }
    }

}

/*
 * Mapdata.box archive validation checker
 * Part of MapDataBuilder tool V1.0 or above
 * By: Krisztian Kispeti
 */

// For button1_Click

using System;

namespace MapDataBoxValidation
{
    public static class ValidationHelper
    {
        public static bool CheckDescription(string content)
        {
            int startIndex = content.IndexOf("si_Map_Description (");

            if (startIndex != -1)
            {
                startIndex += "si_Map_Description (".Length;
                int endIndex = content.IndexOf(");", startIndex);

                if (endIndex != -1)
                {
                    string description = content.Substring(startIndex, endIndex - startIndex).Trim('\'', '\"', ' ', '\t', '\n', '\r');

                    if (description.Length <= 12)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool CheckAuthor(string content)
        {
            int startIndex = content.IndexOf("si_Map_Author(\"");

            if (startIndex != -1)
            {
                startIndex += "si_Map_Author(\"".Length;
                int endIndex = content.IndexOf("\"", startIndex);

                if (endIndex != -1)
                {
                    string author = content.Substring(startIndex, endIndex - startIndex).Trim('\'', '\"', '\t', '\n', '\r', ' ');

                    return author.Length <= 8 && author.EndsWith("\";");
                }
            }

            return false;
        }

        public static bool CheckPlayers(string content)
        {
            int startIndex = content.IndexOf("si_Map_Players(");

            if (startIndex != -1)
            {
                startIndex += "si_Map_Players(".Length;
                int endIndex = content.IndexOf(");", startIndex);

                if (endIndex != -1)
                {
                    string playersStr = content.Substring(startIndex, endIndex - startIndex).Trim();
                    return int.TryParse(playersStr, out int players) && players >= 2 && players <= 8;
                }
            }

            return false;
        }

        public static bool CheckWidth(string content)
        {
            int startIndex = content.IndexOf("si_Map_Width(");

            if (startIndex != -1)
            {
                startIndex += "si_Map_Width(".Length;
                int endIndex = content.IndexOf(");", startIndex);

                if (endIndex != -1)
                {
                    string widthStr = content.Substring(startIndex, endIndex - startIndex).Trim();
                    return int.TryParse(widthStr, out int width) && width >= 25 && width <= 156;
                }
            }

            return false;
        }

        public static bool CheckHeight(string content)
        {
            int startIndex = content.IndexOf("si_Map_Height(");

            if (startIndex != -1)
            {
                startIndex += "si_Map_Height(".Length;
                int endIndex = content.IndexOf(");", startIndex);

                if (endIndex != -1)
                {
                    string heightStr = content.Substring(startIndex, endIndex - startIndex).Trim();
                    return int.TryParse(heightStr, out int height) && height >= 25 && height <= 156;
                }
            }

            return false;
        }
    }
}

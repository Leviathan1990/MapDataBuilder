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
            // Map description can be 12 characters long max. "Custom", "Deathmatch" etc..

            int startIndex = content.IndexOf("si_map_description(\"");
            if (startIndex != -1)
            {
                startIndex += "si_map_description(\"".Length;
                int endIndex = content.IndexOf("\"", startIndex);
                if (endIndex != -1)
                {
                    string description = content.Substring(startIndex, endIndex - startIndex);
                    return description.Length <= 12;
                }
            }
            return false;
        }

        public static bool CheckAuthor(string content)
        {
            //  Author name can be only 8 characters long

            int startIndex = content.IndexOf("si_map_author(\"");
            if (startIndex != -1)
            {
                startIndex += "si_map_author(\"".Length;
                int endIndex = content.IndexOf("\"", startIndex);
                if (endIndex != -1)
                {
                    string author = content.Substring(startIndex, endIndex - startIndex);
                    return author.Length <= 8;
                }
            }
            return false;
        }

        public static bool CheckPlayers(string content)
        {
            //  Minimum players on any map is 2, maximum is 8

            int startIndex = content.IndexOf("si_map_players(");
            if (startIndex != -1)
            {
                startIndex += "si_map_players(".Length;
                int endIndex = content.IndexOf(")", startIndex);
                if (endIndex != -1)
                {
                    string playersStr = content.Substring(startIndex, endIndex - startIndex).Trim();
                    if (int.TryParse(playersStr, out int players))
                    {
                        return players >= 2 && players <= 8;
                    }
                }
            }
            return false;
        }

        public static bool CheckWidth(string content)
        {
            // The minimum map width is 25, maximum is 156

            int startIndex = content.IndexOf("si_map_width(");
            if (startIndex != -1)
            {
                startIndex += "si_map_width(".Length;
                int endIndex = content.IndexOf(")", startIndex);
                if (endIndex != -1)
                {
                    string widthStr = content.Substring(startIndex, endIndex - startIndex).Trim();
                    if (int.TryParse(widthStr, out int width))
                    {
                        return width >= 25 && width <= 156;
                    }
                }
            }
            return false;
        }

        public static bool CheckHeight(string content)
        {
            // The minimum map height is 25, maximum is 156

            int startIndex = content.IndexOf("si_map_height(");
            if (startIndex != -1)
            {
                startIndex += "si_map_height(".Length;
                int endIndex = content.IndexOf(")", startIndex);
                if (endIndex != -1)
                {
                    string heightStr = content.Substring(startIndex, endIndex - startIndex).Trim();
                    if (int.TryParse(heightStr, out int height))
                    {
                        return height >= 25 && height <= 156;
                    }
                }
            }
            return false;
        }
    }
}

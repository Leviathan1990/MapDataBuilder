// Part of MapDataBuilder V1.0

using System;
using System.Collections.Generic;
using System.IO;
using MapDataBuilder;
using System.Text;
using ReadNullTerminator;
using System.Windows.Forms;

namespace MapDataBuilder
{
    public struct BoxItem
    {
        public string Filename;
        public uint Offset;
        public uint Size;
        //public bool IsImage;
        public bool IsReadable;
        public string FullPath { get; set; }

    }
}


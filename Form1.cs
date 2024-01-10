/*
*        MapDataBuilder tool V1.0
*        Designed for the game "The Outforce".
*        Features: Open/Modify/Save the MapData.box archive
*        By: K. Krisztian.
*/

//      Usage in a nutshell:
//      The contents of MapData.box archive will be loaded in the listView1 component
//      If the selected file is a readable format file 'See more in the ReadableFormat.cs file'
//      Then it will be loaded in the richTextBox1 component...


//      TODO: Help needed: Implement saving method.

using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapDataBuilder;                         //  custom .cs file
using ReadNullTerminator;                     //  Null terminator
using ScriptEditor;                           //  For richtextbox component
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;
using SaveBox;

namespace MapDataBuilder
{

    using static Commandlists.ListBoxInitializer;

    public partial class Form1 : Form
    {
        private List<BoxItem> boxItems = new List<BoxItem>();
        private string directoryPath = "";
        private string boxFilePath = "";
        private ImageList customImageList = new ImageList();

        public Form1()
        {
            InitializeComponent();
            directoryPath = Path.GetDirectoryName(Application.ExecutablePath);
            customImageList.ImageSize = new Size(40, 40);                                                           // Icon size in listView1
            InitializeCustomImageList();
            listView1.SmallImageList = customImageList;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Outforce - MapDataBuilder tool V1.0.\n\nPart of DevTools\nVersion: V1.0\nProgramming by: Krisztian Kispeti.\nIconset by: Icons8 com", "Product information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void InitializeCustomImageList()                                                                    //  Add icons to Imagelist...
        {
            customImageList.Images.Add("init.oms", Properties.Resources.icons8_settings_48);                        //  init.oms icon
            customImageList.Images.Add("information.oms", Properties.Resources.icons8_code_48);                     //  information.oms icon
            customImageList.Images.Add("radar.bik", Properties.Resources.icons8_export_file_32);                    //  radar.bik icon
            customImageList.Images.Add("background.bik", Properties.Resources.icons8_export_file_32);               //  background.bik icon
            customImageList.Images.Add("visual.bin", Properties.Resources.icons8_v_32);                             //  visual.bin icon
            customImageList.Images.Add("defaultIcon", Properties.Resources.icons8_file_48);                         //  Default icon
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "MapData Box Archives (*.box)|MapData.box";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    boxFilePath = openFileDialog.FileName;

                    directoryPath = Path.GetDirectoryName(boxFilePath);

                    try
                    {
                        boxItems.Clear();

                        using (BinaryReader reader = new BinaryReader(File.OpenRead(boxFilePath), Encoding.ASCII))
                        {
                            reader.BaseStream.Seek(-4, SeekOrigin.End);
                            uint directoryOffset = reader.ReadUInt32();
                            reader.BaseStream.Seek(directoryOffset, SeekOrigin.Begin);
                            uint numFiles = reader.ReadUInt32();

                            for (int i = 0; i < numFiles; i++)
                            {
                                BoxItem item = new BoxItem();
                                item.Filename = NullTerminator.ReadNullTerminatedString(reader);
                                item.Offset = reader.ReadUInt32();
                                item.Size = reader.ReadUInt32();
                                item.IsReadable = EditorFunctions.IsReadableFile(item.Filename);

                                // Full path to MapData.box
                                item.FullPath = Path.Combine(directoryPath, item.Filename);
                                Console.WriteLine($"Trying to read file: {item.FullPath}");
                                boxItems.Add(item);
                            }
                        }

                        listView1.Items.Clear();

                        foreach (var item in boxItems)
                        {
                            ListViewItem listViewItem = new ListViewItem(item.Filename);
                            string iconKey = Path.GetFileName(item.Filename);
                            iconKey = iconKey.ToLower();

                            if (customImageList.Images.ContainsKey(iconKey))
                            {
                                listViewItem.ImageKey = iconKey;
                            }
                            else
                            {
                                listViewItem.ImageKey = "defaultIcon";
                            }

                            listViewItem.Name = item.Filename;

                            listViewItem.Tag = listViewItem.Name;

                            listView1.Items.Add(listViewItem);
                        }

                        double archiveSizeKB = new FileInfo(boxFilePath).Length / 1024.0;
                        textBox8.Text = $"{archiveSizeKB:F2} KB";

                        MessageBox.Show("*.box archive contents loaded successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading *.box archive contents: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedListItem = listView1.SelectedItems[0];
                string selectedFileName = selectedListItem.Text.ToLower();

                int selectedIndex = selectedListItem.Index;
                if (selectedIndex >= 0 && selectedIndex < boxItems.Count)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(selectedFileName).ToLower();

                    switch (fileNameWithoutExtension)
                    {
                        case "init":
                            toolStripStatusLabel2.Text = "Your map initialisation file";          //  For init.oms file
                            break;
                        case "information":
                            toolStripStatusLabel2.Text = "Your map configuration file";          //  For information.oms file
                            break;
                        case "radar":
                            toolStripStatusLabel2.Text = "Background image for Radar";           //  For radar.bik file
                            break;
                        case "background":
                            toolStripStatusLabel2.Text = "Background image for your map";        //  For background.bik file
                            break;
                        default:
                            toolStripStatusLabel2.Text = $"Your map file: {fileNameWithoutExtension}";
                            break;
                    }

                    uint fileSize = boxItems[selectedIndex].Size;
                    uint fileOffset = boxItems[selectedIndex].Offset;

                    textBox9.Text = fileOffset.ToString();

                    double fileSizeKB = fileSize / 1024.0;
                    textBox10.Text = $"{fileSizeKB:F2} KB";

                    if (EditorFunctions.IsReadableFile(selectedFileName))
                    {
                        byte[] fileContent = ExtractBytes.FileHelper.ExtractBytesFromBox(boxFilePath, fileOffset, fileSize);

                        if (fileContent != null)
                        {
                            string fileContentString = System.Text.Encoding.ASCII.GetString(fileContent);
                            richTextBox1.Text = fileContentString;

                            textBox11.Text = Path.GetFileName(selectedFileName);
                        }
                        else
                        {
                            richTextBox1.Text = string.Empty;
                            MessageBox.Show($"Error reading file content: file not found: {selectedFileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        richTextBox1.Text = "not readable format file";
                    }
                }
            }
            else
            {
                textBox9.Text = string.Empty;
                textBox10.Text = string.Empty;
                richTextBox1.Text = string.Empty;
                toolStripStatusLabel2.Text = string.Empty;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void restartProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        private void button2_Click(object sender, EventArgs e)
        {
          //   Implementation needed.
          //   Saving method - logic. Save (update) the opened MapData.box archive
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                listView1.View = View.List;
            }

            else
            {
                listView1.View = View.SmallIcon;
            }
        }


    }
}

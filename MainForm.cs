﻿using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Ionic.Zlib;

namespace PackImage
{
    public partial class MainForm : Form
    {
        [Serializable]
        public class PackData
        {
            public List<MemoryStream> img;
        }

        private byte[] PACK_SPACE = new byte[] { 0xFF, 0xAA, 0x00, 0xCC };
        public MainForm()
        {
            InitializeComponent();
        }


        public byte[] zipData(byte[] data)
        {
            return GZipStream.CompressBuffer(data);
            
        }

        public byte[] unzipData(byte[] zdata)
        {
            return GZipStream.UncompressBuffer(zdata);
        }

        protected byte[] AuthGetFileData(string fileUrl)
        {
            using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
            {
                byte[] buf = new byte[fs.Length];
                fs.Read(buf, 0, buf.Length);
                return buf;
            }
        }


        public static byte[] GetSingleBitmap(string file)
        {
            Bitmap pimage = new Bitmap(file);
            Bitmap source;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert
            if (pimage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(pimage.Width, pimage.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(pimage.HorizontalResolution, pimage.VerticalResolution);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(pimage, 0, 0);
                }
            }
            else
            {
                source = pimage;
            }

            // Lock source bitmap in memory
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy image data to binary array
            int imageSize = sourceData.Stride * sourceData.Height;
            byte[] sourceBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

            // Unlock source bitmap
            source.UnlockBits(sourceData);

            // Create destination bitmap
            System.Drawing.Bitmap destination =
                new System.Drawing.Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);

            // Lock destination bitmap in memory
            BitmapData destinationData =
                destination.LockBits(new Rectangle(0, 0, destination.Width, destination.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            // Create destination buffer
            imageSize = destinationData.Stride * destinationData.Height;
            byte[] destinationBuffer = new byte[imageSize];
            int height = source.Height;
            int width = source.Width;
            int threshold = 500;

            // Iterate lines
            for (int y = 0; y < height; y++)
            {
                int sourceIndex = y * sourceData.Stride;
                int destinationIndex = y * destinationData.Stride;
                byte destinationValue = 0;
                int pixelValue = 128;

                // Iterate pixels
                for (int x = 0; x < width; x++)
                {
                    // Compute pixel brightness (i.e. total of Red, Green, and Blue values)
                    int pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] +
                                     sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }

                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }

                    sourceIndex += 4;
                }

                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }

            // Copy binary image data to destination bitmap
            Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

            // Unlock destination bitmap
            destination.UnlockBits(destinationData);

            // Dispose of source if not originally supplied bitmap
            if (source != pimage)
            {
                source.Dispose();
            }

            MemoryStream ms = new MemoryStream();
            destination.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            };
            fbd.ShowDialog();
            textBox1.Text = fbd.SelectedPath;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
           
            if (textBox1.Text != string.Empty)
            {
                string fname;
                SaveFileDialog sd = new SaveFileDialog();
                sd.Filter = "PackFile(*.dat)|*.dat";
                sd.Title = "请输入保存文件名";

                if (sd.ShowDialog(this) == DialogResult.OK)
                {
                    fname = sd.FileName;
                }
                else
                {
                    return;
                }


                PackData pack = new PackData();
                List<MemoryStream> ls = new List<MemoryStream>();

                foreach (var file in Directory.GetFiles(textBox1.Text))
                {
                    byte[] tmp = AuthGetFileData(file);
                    MemoryStream ms = new MemoryStream();
                    ms.Write(tmp, 0, tmp.Length);
                    ls.Add(ms);
                }

                pack.img = ls;
                MemoryStream mm = new MemoryStream();
                var formatter = new BinaryFormatter();
                formatter.Serialize(mm, pack);

                var zdata = zipData(mm.ToArray());



                if (File.Exists(fname))
                    File.Delete(fname);


               
                FileStream fs = new FileStream(fname, FileMode.Create);
                fs.Write(zdata, 0, zdata.Length);
                fs.Dispose();
                Process.Start("Explorer.exe", "/select," + fname);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "/pack.dat", FileMode.Open))
            {
                MemoryStream ms = new MemoryStream();
                fs.CopyTo(ms);
                var data = unzipData(ms.ToArray());
                ms = new MemoryStream();
                ms.Write(data, 0, data.Length);

                ms.Seek(0, SeekOrigin.Begin);
                var formatter = new BinaryFormatter();
                PackData pack = (PackData)formatter.Deserialize(ms);

                foreach(var m in pack.img)
                {
                   Image img = Image.FromStream(m);
                   picbox.Image = img;
                   MessageBox.Show("ca");
                }




            }
        }




    }
}

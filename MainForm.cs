using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Ionic.Zlib;
using ImageMagick;

namespace PackImage
{
    public partial class MainForm : Form
    {
        [Serializable]
        public class PackData
        {
            public List<MemoryStream> img;
        }

        private SynchronizationContext Sync = null;
        private Task procTask = null;
        public MainForm()
        {
            InitializeComponent();
            this.Sync = SynchronizationContext.Current;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            nbxThread.Value = Environment.ProcessorCount;
        }

        private void setPbar(object o)
        {
            this.pbar.Value = (int)o;
        }

        public byte[] zipData(byte[] data)
        {
            return GZipStream.CompressBuffer(data);

        }

        public byte[] unzipData(byte[] zdata)
        {
            return GZipStream.UncompressBuffer(zdata);
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            pnMain.Enabled = false;
            btnStart.Enabled = false;
            if (textBox1.Text != string.Empty)
            {
                string fname;
                SaveFileDialog sd = new SaveFileDialog
                {
                    Filter = "PackFile(*.dat)|*.dat",
                    Title = "请输入保存文件名"
                };

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
                var files = Directory.GetFiles(textBox1.Text);
                decimal count = 1;

                procTask = new Task(() =>
                {


                    foreach (var file in files)
                    {
                        var td = (count / files.Length) * 100;
                        var percent = decimal.ToInt32(td);
                        Sync.Send(setPbar, percent);

                        var buf = GetSingleBitmap(file);
                        MagickImage img = new MagickImage(buf) { Format = MagickFormat.Xbm };
                        var width = Convert.ToInt32(nbxWidth.Value);
                        var height = Convert.ToInt32(nbxHeight.Value);
                        img.Resize(new MagickGeometry($"{width}x{height}!"));
                        buf = img.ToByteArray();
                        MemoryStream ms = new MemoryStream();
                        ms.Write(buf, 0, buf.Length);
                        ls.Add(ms);
                        count++;
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

                    pnMain.Enabled = true;
                    btnStart.Enabled = true;

                });


                procTask.Start();

            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            /*
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

                foreach (var m in pack.img)
                {
                    Image img = Image.FromStream(m);
                    picbox.Image = img;
                    MessageBox.Show("ca");
                }

            }

          */

        }

        private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var files = e.Argument as string[];
            PackData pack = new PackData();
            List<MemoryStream> ls = new List<MemoryStream>();
            decimal count = 1;

            foreach (var f in files)
            {
                var td = (count / files.Length) * 100;
                var percent = decimal.ToInt32(td);
                Sync.Send(setPbar, percent);

                var buf = GetSingleBitmap(f);
                MagickImage img = new MagickImage(buf) { Format = MagickFormat.Xbm };
                var width = Convert.ToInt32(nbxWidth.Value);
                var height = Convert.ToInt32(nbxHeight.Value);
                img.Resize(new MagickGeometry($"{width}x{height}!"));
                buf = img.ToByteArray();
                MemoryStream ms = new MemoryStream();
                ms.Write(buf, 0, buf.Length);
                ls.Add(ms);
                count++;
            }
        }

        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer
            };
            fbd.ShowDialog();
            textBox1.Text = fbd.SelectedPath;
        }
    }
}

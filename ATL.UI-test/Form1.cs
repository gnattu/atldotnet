﻿using System;
using System.IO;
using System.Windows.Forms;

namespace ATL.UI_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void displayProgress(float progress)
        {
            String str = (progress * 100).ToString() + "%";
            Console.WriteLine(str);
            ProgressLbl.Text = str;
        }

        private void GoBtn_Click(object sender, EventArgs e)
        {
            string filePath = @"D:\temp\m4a-mp4\160\2tracks_TestFromABC-Orig.m4a";
            string testFileLocation = TestUtils.GenerateTempTestFile(filePath);
            Settings.FileBufferSize = 30;
            try
            {
                IProgress<float> progress = new Progress<float>(displayProgress);
                Track theFile = new Track(testFileLocation, progress);

                double tDuration = theFile.DurationMs;
                long lDataOffset = theFile.TechnicalInformation.AudioDataOffset;
                long lDataAudioSize = theFile.TechnicalInformation.AudioDataSize;
                theFile.Chapters[0].Title += 'x';
                theFile.Chapters[0].Picture = PictureInfo.fromBinaryData(System.IO.File.ReadAllBytes(@"C:\Users\zeugm\source\repos\Zeugma440\atldotnet\ATL.test\Resources\_Images\pic1.jpeg"));
                //theFile.Chapters[0].Picture = PictureInfo.fromBinaryData(System.IO.File.ReadAllBytes("M:\\Temp\\Audio\\avatarOfSorts.png"));
                theFile.Chapters[0].Picture.ComputePicHash();
                theFile.Chapters[1].Title += 'x';
                theFile.Chapters[1].Picture = PictureInfo.fromBinaryData(System.IO.File.ReadAllBytes(@"C:\Users\zeugm\source\repos\Zeugma440\atldotnet\ATL.test\Resources\_Images\pic2.jpeg"));
                //theFile.Chapters[1].Picture = PictureInfo.fromBinaryData(System.IO.File.ReadAllBytes("M:\\Temp\\Audio\\themeOfTheTrack.jpg"));
                theFile.Chapters[1].Picture.ComputePicHash();

                theFile.Save();
                theFile = new Track(testFileLocation);

                theFile.Chapters[0].Picture.ComputePicHash();
                Console.WriteLine(theFile.Chapters[0].Picture.PictureHash);
                theFile.Chapters[1].Picture.ComputePicHash();
                Console.WriteLine(theFile.Chapters[1].Picture.PictureHash);

                theFile.Chapters[1].Picture = null;

                theFile.Save();

                ProgressLbl.Text = "";
                ProgressLbl.Visible = true;
            }
            finally
            {
                File.Delete(testFileLocation);
            }
        }
    }
}

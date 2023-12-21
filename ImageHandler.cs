﻿using IronSoftware.Drawing;
using Color = IronSoftware.Drawing.Color;

namespace IsoTiloSlicer
{
    internal class ImageHandler
    {
        public ImageHandler(string imagePath = "", int tileWidth = 44, int tileHeight = 44, int offset = 1, string outputDirectory = "out") 
        {
            ImagePath = imagePath;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Offset = offset;
            OutputDirectory = outputDirectory;
        }

        public Color BackgroundColor { get; set; } = Color.Black;
        public string ImagePath { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int Offset { get; set; }
        public string OutputDirectory { get; set; }
        public AnyBitmap OriginalImage { get; private set; }
        public List<AnyBitmap> Slices { get; private set; } = new List<AnyBitmap>();
        public string LastErrorMessage { get; private set; } = string.Empty;
        public string FileNameFormat { get; set; } = "{0}";
        public int StartingFileNumber { get; set; } = 0;

        private int xSlices = 0, ySlices = 0;

        public bool Process()
        {
            if(File.Exists(ImagePath))
            {
                OriginalImage = AnyBitmap.FromFile(ImagePath);

                xSlices = (int)Math.Ceiling((double)OriginalImage.Width / (double)TileWidth) + 2;
                ySlices = (int)Math.Ceiling((double)OriginalImage.Height / (double)TileHeight) + 2;

                SplitImage();
                SaveImages();
                Console.WriteLine("Sliced succesfully.");
                return true;
            } 
            else
            {
                LastErrorMessage = "Image path could not be found";
                Console.WriteLine(LastErrorMessage);
            }
            return false;
        }

        private void SplitImage()
        {
            int xOffset = -(TileWidth / 2), yOffset = -(TileHeight / 2);

            for (int row = 0; row < xSlices; row++)
            {
                for (int col = 0; col < ySlices; col++)
                {
                    int startX = (col * TileWidth) + xOffset;
                    int startY = (row * TileHeight) + yOffset;

                    AnyBitmap slice = new AnyBitmap(TileWidth, TileHeight, BackgroundColor);

                    int offset = Offset;
                    bool reverse = false;

                    for (int yPixel = 0; yPixel < TileHeight; yPixel++)
                    {
                        //0, 0 -> 0, 1 -> 0, 44

                        int grabX = (TileWidth / 2) - offset; // = 21
                        int grabQty = offset * 2; //2

                        for (int i = 0; i < grabQty; i++)
                        {
                            //slice.SetPixel(grabX + i, yPixel, Color.White); Test to see shape

                            if (grabX + i + startX < 0 || grabX + i + startX >= OriginalImage.Width || yPixel + startY < 0 || yPixel + startY >= OriginalImage.Height)
                            {
                                continue;
                            }
                            slice.SetPixel(grabX + i, yPixel, OriginalImage.GetPixel(grabX + i + startX, yPixel + startY));
                        }

                        if (!reverse)
                        {
                            offset++;
                            if ((TileWidth / 2) - offset < 0)
                            {
                                offset--; //Keep offset the same for this run
                                reverse = true;
                            }
                        } 
                        else
                        {
                            offset--;
                        }
                        //20, 4
                    }
                    Slices.Add(slice);
                }
            }
        }

        public void SaveImages()
        {
            int startingNumber = StartingFileNumber;
            foreach (var image in Slices)
            {
                if (!Directory.Exists(OutputDirectory))
                {
                    Directory.CreateDirectory(OutputDirectory);
                }

                image.SaveAs(Path.Combine(OutputDirectory, string.Format(FileNameFormat + ".bmp", startingNumber)), AnyBitmap.ImageFormat.Bmp);
                startingNumber++;
            }
        }
    }
}
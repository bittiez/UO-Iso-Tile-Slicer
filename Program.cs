using System;

namespace IsoTiloSlicer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ImageHandler image = new ImageHandler();

            if(args.Length == 0 )
            {
                Console.WriteLine("Usage: IsoTiloSlicer.exe -image path");
                Console.WriteLine("Optional args:");
                Console.WriteLine("-tilesize 44  <- Tile size.");
                Console.WriteLine("-offset 1  <- 0 if tile size is an odd number, 1 if it is an even number. Other numbers have unknown results.");
                Console.WriteLine("-output out  <- Where to save sliced images to.");
                Console.WriteLine("-filename {0} <- {0} is the image number, you can use tile{0} or file{0} for example.");
                Console.WriteLine("-startingnumber 0 <- The file number to start with.");
            }

            foreach (string arg in args)
            {
                switch(arg)
                {
                    case "image":
                        image.ImagePath = arg;
                        break;
                    case "tilesize":
                        if (int.TryParse(arg, out int h))
                        {
                            image.TileHeight = h;
                            image.TileWidth = h;
                        }
                        break;
                    case "offset":
                        if (int.TryParse(arg, out int o))
                        {
                            image.Offset = o;
                        }
                        break;
                    case "output":
                        image.OutputDirectory = arg;
                        break;
                    case "filename":
                        image.FileNameFormat = arg;
                        break;
                    case "startingnumber":
                        if (int.TryParse(arg, out int s))
                        {
                            image.StartingFileNumber = s;
                        }
                        break;
                }
            }

            image.Process();
        }
    }
}
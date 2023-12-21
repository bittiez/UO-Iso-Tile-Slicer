using CommandLine;

namespace IsoTiloSlicer
{
    internal class Program
    {
        public class Options
        {
            [Option("image", Required = true)]
            public string ImagePath { get; set; }

            [Option("tilesize", Required = false)]
            public int TileSize { get; set; } = 0;

            [Option("offset", Required = false)]
            public int Offset { get; set; } = 0;

            [Option("filename", Required = false)]
            public string FileName { get; set; }

            [Option("output", Required = false)]
            public string Output { get; set; }

            [Option("startingnumber", Required = false)]
            public int StartingNumber { get; set; } = 0;
        }

        public static void Main(string[] args)
        {
            ImageHandler image = new ImageHandler();

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: IsoTiloSlicer.exe --image path");
                Console.WriteLine("Optional args:");
                Console.WriteLine("--tilesize 44  <- Tile size.");
                Console.WriteLine("--offset 1  <- 0 if tile size is an odd number, 1 if it is an even number. Other numbers have unknown results.");
                Console.WriteLine("--output out  <- Where to save sliced images to.");
                Console.WriteLine("--filename {0} <- {0} is the image number, you can use tile{0} or file{0} for example.");
                Console.WriteLine("--startingnumber 0 <- The file number to start with.");
                return;
            }

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => 
            {
                if (!string.IsNullOrEmpty(o.ImagePath))
                {
                    image.ImagePath = o.ImagePath;
                }

                if(o.TileSize > 0)
                {
                    image.TileHeight = o.TileSize;
                    image.TileWidth = o.TileSize;
                }

                if(o.Offset > 0)
                {
                    image.Offset = o.Offset;
                }

                if(!string.IsNullOrEmpty(o.Output))
                {
                    image.OutputDirectory = o.Output;
                }

                if (!string.IsNullOrEmpty(o.FileName))
                {
                    image.FileNameFormat = o.FileName;
                }

                if (o.StartingNumber != 0)
                {
                    image.StartingFileNumber = o.StartingNumber;
                }
            });

            image.Process();
        }
    }
}
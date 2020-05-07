using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Foto
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "";

            using (Window window = new Window("capture"))
            {

                for(int i=0; i<271; i++)
                {
                    if (i < 10)
                    {
                        path = @"C:\Users\UO258767\Desktop\Roza\roza_00"+i.ToString()+".jpg";
                        Console.WriteLine(path);
                    }
                    if(i>9 && i < 100)
                    {
                        path = @"C:\Users\UO258767\Desktop\Roza\roza_0" + i.ToString() + ".jpg";
                    }
                    if(i>99)
                    {
                        path = @"C:\Users\UO258767\Desktop\Roza\roza_" + i.ToString() + ".jpg";
                    }

                    Console.WriteLine("Show");

                    using (Mat image = new Mat(path))
                    {
                        window.ShowImage(image);
                        Cv2.WaitKey(10);
                        Console.WriteLine("Video ejecutado");
                    }
                        
                }

            }
        }
    }
}

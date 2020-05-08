using System;
using OpenCvSharp;

namespace Video
{
    class Program
    {
        static void Main(string[] args)
        {
            // Opens MP4 file (ffmpeg is probably needed)
            VideoCapture capture = new VideoCapture(1);

            int sleepTime = (int)Math.Round(500 / capture.Fps);  //Captura cada xtiempo

            using (Window window = new Window("capture"))
            using (Mat image = new Mat()) // Frame image buffer
            {
                // When the movie playback reaches end, Mat.data becomes NULL.
                while (true)
                {
                    capture.Read(image); // same as cvQueryFrame
                    if (image.Empty())
                        break;
                    var grayImage = new Mat();
                    Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGRA2GRAY);
                    Cv2.EqualizeHist(grayImage, grayImage);

                    var cascade = new CascadeClassifier(@"..\..\..\Data\haarcascade_frontalface_alt.xml");
                    var nestedCascade = new CascadeClassifier(@"..\..\..\Data\haarcascade_eye_tree_eyeglasses.xml");

                    var faces = cascade.DetectMultiScale(
                        image: grayImage,
                        scaleFactor: 1.1,
                        minNeighbors: 2,
                        flags: HaarDetectionType.DoRoughSearch | HaarDetectionType.ScaleImage,
                        minSize: new Size(30, 30)
                        );

                    Console.WriteLine("Detected faces: {0}", faces.Length);

                    var rnd = new Random();
                    var count = 1;
                    foreach (var faceRect in faces)
                    {
                        var detectedFaceImage = new Mat(image, faceRect);
                        //Cv2.ImShow(string.Format("Face {0}", count), detectedFaceImage);
                        //Cv2.WaitKey(1); // do events

                        var color = Scalar.FromRgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                        Cv2.Rectangle(image, faceRect, color, 3);

                        count++;
                    }
                    
                    //srcImage.Dispose();

                    window.ShowImage(image);
                    Cv2.WaitKey(sleepTime);
                    Console.WriteLine("Video ejecutado");
                }
            } 
        }
    }
}

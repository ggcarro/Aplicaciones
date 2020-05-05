using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iVocabulary;
using OpenCvSharp;

namespace ImageWorker
{
    public class FaceDetec
    {

        public FaceDetec(){


        }

        public Image Detec(Image image)
        {
            string type = (image.Filename).Split(".")[1];
            File.WriteAllBytes("temp." + type, image.Data);


            var srcImage = new Mat("temp."+type);

            var grayImage = new Mat();
            Cv2.CvtColor(srcImage, grayImage, ColorConversionCodes.BGRA2GRAY);
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
                var detectedFaceImage = new Mat(srcImage, faceRect);
                //Cv2.ImShow(string.Format("Face {0}", count), detectedFaceImage);
                //Cv2.WaitKey(1); // do events

                var color = Scalar.FromRgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                Cv2.Rectangle(srcImage, faceRect, color, 3);
                
                count++;
            }

            Cv2.ImWrite("FaceDetec." + type, srcImage);

            srcImage.Dispose();
            image.Data = File.ReadAllBytes("FaceDetec." + type);
            image.Filename = "FA_" + image.Filename;
            return image;
        }
    }
}

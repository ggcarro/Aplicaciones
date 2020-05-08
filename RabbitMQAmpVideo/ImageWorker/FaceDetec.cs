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

        public Mat Detec(Mat image)
        {

            var srcImage = image;

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
                var color = Scalar.FromRgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                Cv2.Rectangle(srcImage, faceRect, color, 3);
                
                count++;
            }

            return srcImage;
        }
    }
}

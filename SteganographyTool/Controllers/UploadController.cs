using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace FileUpload.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                string _path = "";
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";

                Bitmap newImage = Encode(_path);

                SaveImage(newImage);

                return View();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

        public void SaveImage(Bitmap img)
        {
            String _path = "";
            string _FileName = Path.GetFileName("ImageWithEncodedText.png");
            _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
            img.Save(_path, ImageFormat.Png);


            //img.Save("imgWithText.png", ImageFormat.Png);
        }


        public Bitmap Encode(string location)
        {
            Bitmap img = new Bitmap(location);

            Bitmap imgWithText = embedText("Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   Hallo ich bin ein Testtext   ", img);

            string test = extractText(imgWithText);

            return imgWithText;

        }


        public static Bitmap embedText(string text, Bitmap bmp)
        {

            bool markEnd = false;

            int charIndex = 0;

            int charValue = 0;

            long pixelElementIndex = 0;

            // Zähler für Nullen zur Abschlussmarkierung
            int zeros = 0;

            //RGB pro Pixel
            int R = 0, G = 0, B = 0;

            //Doppel-Schleife für X & Y - Koordinaten von jedem Pixel
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    //Pixelauswahl
                    Color pixel = bmp.GetPixel(x, y);

                    //LSB leeren
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    //Loop durch RGB-Kanäle je Pixel
                    for (int n = 0; n < 3; n++)
                    {
                        //überprüft ob Char fertig ist
                        if (pixelElementIndex % 8 == 0)
                        {
                            
                            if (markEnd && zeros == 8)
                            {
                                //letzter Pixel
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    bmp.SetPixel(x, y, Color.FromArgb(R, G, B));
                                }

                                //fertiges Bitmap returnen
                                return bmp;
                            }

                            //prüfen ob komplett fertig
                            if (charIndex >= text.Length)
                            {
                                //Mode umschalten
                                markEnd = true;
                            }
                            else
                            {
                                // move to the next character and process again
                                charValue = text[charIndex++];
                            }
                        }

                        //abwechselnd r g  & b kanal auswählen um lsb zu ändern
                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if (!markEnd)
                                    {
                                        
                                        R += charValue % 2;

                                        
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (!markEnd)
                                    {
                                        G += charValue % 2;

                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (!markEnd)
                                    {
                                        B += charValue % 2;

                                        charValue /= 2;
                                    }

                                    bmp.SetPixel(x, y, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;

                        if (markEnd)
                        {
                            //Nullen zählen
                            zeros++;
                        }
                    }
                }
            }

            return bmp;
        }

        public static string extractText(Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;

            string extractedText = String.Empty;

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                   
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }

                        colorUnitIndex++;

                       
                        if (colorUnitIndex % 8 == 0)
                        {
                           
                            charValue = reverseBits(charValue);

                          
                            if (charValue == 0)
                            {
                                return extractedText;
                            }

                            
                            char c = (char)charValue;

                            
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }

        public static int reverseBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }
    }
}

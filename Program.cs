using System;
using Gadgeteer.Modules.GHIElectronics;
using GHI.Premium.System;
using Microsoft.SPOT;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Camera
{
    public partial class Program
    {
        void ProgramStarted()
        {
            button.ButtonPressed += new Button.ButtonEventHandler(button_ButtonPressed);
            camera.PictureCaptured += new GTM.GHIElectronics.Camera.PictureCapturedEventHandler(camera_PictureCaptured);
        }

        void camera_PictureCaptured(GTM.GHIElectronics.Camera sender, GT.Picture picture)
        {
            GT.StorageDevice storage = sdCard.GetStorageDevice();
            var imageNumber = (uint)storage.ListFiles(@"\Me").Length;

            var bitmap = picture.MakeBitmap();
            var bmpData = bitmap.GetBitmap();

            byte[] bmpFile = new byte[bitmap.Width * bitmap.Height * 3 + 54];
            Util.BitmapToBMPFile(bmpData, bitmap.Width, bitmap.Height, bmpFile);

            string pathFileName = @"\Me\Image-" + (imageNumber++).ToString() + ".bmp";
            display.SimpleGraphics.DisplayImage(bitmap, 0, 0);

            try
            {
                storage.WriteFile(pathFileName, bmpFile);
            }
            catch (Exception ex)
            {
                Debug.Print("Message: " + ex.Message + "  Inner Exception: " + ex.InnerException);
            }

        }

        void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            camera.TakePicture();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace wallUpdate
{
    class WallSetter
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        private static readonly UInt32 SPI_SETDESKWALLPAPER  = 0x14;

        private static readonly UInt32 SPIF_UPDATEINIFILE    = 0x01;

        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

 

        public void SetWallpaper(String path)

        {

            if(path.EndsWith(".png")){
                System.Drawing.Image Dummy = Image.FromFile(path);
                path = path.Replace(".png", ".jpeg");
                Dummy.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }


            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path,

                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

        }

    }
}

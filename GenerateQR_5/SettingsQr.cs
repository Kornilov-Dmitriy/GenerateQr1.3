using QRCoder;

namespace GenerateQr
{
    class SettingsQr
    {
        public static QRCodeGenerator.ECCLevel levelError; // Level of restored information
        private static int pixelsPerModule = 20;           // quality generated image
        private static bool drawQuietZones;                // white border for better recognizing
        private static string disableIcon;                 // set image into center qr code
        private static string pathIcon;                    // path for icon

        public static int PixelsPerModule
        {
            get
            {
                return pixelsPerModule;
            }
            set
            {
                pixelsPerModule = value;
            }
        }

        public static bool DrawQuietZones
        {
            get
            {
                return drawQuietZones;
            }
            set
            {
                drawQuietZones = value;
            }
        }

        public static string PathIcon
        {
            get
            {
                return pathIcon;
            }
            set
            {
                pathIcon = value;
            }
        }

        public static string DisableIcon
        {
            get
            {
                return disableIcon;
            }
            set
            {
                disableIcon = value;
            }
        }
    }
}

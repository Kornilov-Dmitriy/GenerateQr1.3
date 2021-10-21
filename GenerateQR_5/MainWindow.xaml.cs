using Microsoft.Win32;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static QRCoder.PayloadGenerator;

namespace GenerateQr
{
    public partial class MainWindow : Window
    {
       private Bitmap ImageLogo;      // Bitmap object for logo in the middle
       private Bitmap qrCodeAsBitmap; // Bitmap object for qr code GUI
       string payload = "";           // variable for reading data from textboxes

        public MainWindow()
        {
            InitializeComponent();
           
        }

        void LevelErrors()
        {
            if (RadioButtonL.IsChecked == true)
            {
                SettingsQr.levelError = QRCodeGenerator.ECCLevel.L;
            }
            else if (RadioButtonM.IsChecked == true)
            {
                SettingsQr.levelError = QRCodeGenerator.ECCLevel.M;
            }
            else if (RadioButtonQ.IsChecked == true)
            {
                SettingsQr.levelError = QRCodeGenerator.ECCLevel.Q;
            }
            else if (RadioButtonH.IsChecked == true)
            {
                SettingsQr.levelError = QRCodeGenerator.ECCLevel.H;
            }
        }

        void CheckIconTest()
        {

            if (SettingsQr.PathIcon == null || disable_icon.IsChecked == true)
            {
                ImageLogo = null;
            }
            else
            {
                ImageLogo = new Bitmap(SettingsQr.PathIcon);
            }
        }

        void Generate()
        {
            CheckIconTest();
            LevelErrors();

            // Check data for Wifi 

            if (ExpanderWifi.IsExpanded == true)
            {
                WiFi.Authentication CoderWifi = WiFi.Authentication.nopass;
                if (None.IsChecked == true)
                {
                    CoderWifi = WiFi.Authentication.nopass;
                }
                else if (Wep.IsChecked == true)
                {
                    CoderWifi = WiFi.Authentication.WEP;
                }
                else if (Wpa.IsChecked == true)
                {
                    CoderWifi = WiFi.Authentication.WPA;
                }

                WiFi generator = new WiFi(InputSsid.Text, WifiPassword.Password, CoderWifi, (bool)CheckBoxHidden.IsChecked);
                payload = generator.ToString();
            }

            // Check data for Url

            else if (ExpanderUrl.IsExpanded == true)
            {
                Url generator = new Url(TextBoxExpanderUrl.Text);
                payload = generator.ToString();
            }

            // Check data for Sms
            else if (ExpanderSms.IsExpanded == true)
            {
                SMS.SMSEncoding sMSEncoding = SMS.SMSEncoding.SMS;
                if (RadioSms.IsChecked == true)
                {
                    sMSEncoding = SMS.SMSEncoding.SMS;
                }
                else if (RadioSmsTo.IsChecked == true)
                {
                    sMSEncoding = SMS.SMSEncoding.SMSTO;
                }
                else if (RadioSmsIos.IsChecked == true)
                {
                    sMSEncoding = SMS.SMSEncoding.SMS_iOS;
                }

                SMS generator = new SMS(TextBoxNumber.Text, TextboxText.Text, sMSEncoding);
                payload = generator.ToString();
            }

            // Check data for Check data for WhatsApp
            else if (ExpanderWhatsApp.IsExpanded == true) {
                WhatsAppMessage generator = new WhatsAppMessage(TextBoxNumberWhatsApp.Text, TextboxTextWhatsApp.Text);
                payload = generator.ToString();
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, SettingsQr.levelError);
            QRCode qrCode = new QRCode(qrCodeData);
            qrCodeAsBitmap = qrCode.GetGraphic((int)SliderPixel.Value, System.Drawing.Color.Black, System.Drawing.Color.White, ImageLogo, (int)IconSize.Value, 50, (bool)WhiteBorderQr.IsChecked);

            //  Convert Bitmap for GUI

            BitmapImage BitmapToImageSource(Bitmap imageConvert)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    imageConvert.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                    memory.Position = 0;
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                    return bitmapimage;
                }
            }

            // Create image for GUI

            qrImage.Source = BitmapToImageSource(qrCodeAsBitmap);
        }

        private void ButtonIcon_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                SettingsQr.PathIcon = dlg.FileName;
            }
        }

        private void ButtonSaveQR_Click(object sender, RoutedEventArgs e)
        {
            // Save file with SaveFileDialog

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPG Files (*.jpg)|*.jpg";        
            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                qrCodeAsBitmap.Save(saveFileDialog.FileName);
            }
            else
            {
               // MessageBox.Show("Wrong path");
            }
        }

        private void ExpanderWifi_Click(object sender, RoutedEventArgs e)
        {
            ExpanderUrl.IsExpanded = false;
            ExpanderSms.IsExpanded = false;
            ExpanderWhatsApp.IsExpanded = false;        
        }

        private void ExpanderUrl_Click(object sender, RoutedEventArgs e)
        {
            ExpanderWifi.IsExpanded = false;
            ExpanderSms.IsExpanded = false;
            ExpanderWhatsApp.IsExpanded = false;
        }

        private void ExpanderSms_Click(object sender, RoutedEventArgs e)
        {
            ExpanderUrl.IsExpanded = false;
            ExpanderWifi.IsExpanded = false;
            ExpanderWhatsApp.IsExpanded = false;
        }

        private void ExpanderWhatsApp_Click(object sender, RoutedEventArgs e)
        {
            ExpanderUrl.IsExpanded = false;
            ExpanderWifi.IsExpanded = false;
            ExpanderSms.IsExpanded = false;
        }

        private void WifiPassword_PasswordChanged(object sender, RoutedEventArgs e) => Generate();

        private void TextboxText_TextChanged(object sender, TextChangedEventArgs e) => Generate();

        private void Common_Event_GenerateQR(object sender, RoutedEventArgs e) => Generate();

        private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e) => Generate(); 
    }
}

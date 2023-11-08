using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Reflection;
using System.Net.Security;
using System.Windows.Threading;

namespace Cookie_Clicker_Murat_Varen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MediaPlayer tapSoundPlayer = new MediaPlayer();
        private DispatcherTimer klok = new DispatcherTimer();
        private decimal cookieCounter = 0;
        private int pointerCounter = 0;
        private const decimal pointerBasePrice = 15;
        private decimal pointerPrice = 15;
        public MainWindow()
        {
            InitializeComponent();
            klok.Tick += new EventHandler(KlokAfgelopen);
            klok.Interval = new TimeSpan(0, 0, 0, 0, 10);
            klok.Start();
        }
        private void KlokAfgelopen(object sender, EventArgs e)
        {
            PassiveIncome(pointerCounter, 0.001m);
            LblCookieCount.Content = $"{cookieCounter.ToString("00.00")} cookies";
            LblPassiveIncome.Content = $"+{pointerCounter * 0.01m}";
            ButtonEnabler();
        }

        private void ImgCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                To = new Thickness(30),
                Duration = TimeSpan.FromMilliseconds(100),
                FillBehavior = FillBehavior.Stop,
            };
            ImgCookie.BeginAnimation(Image.MarginProperty, animation);
            RandomTapSound();
            cookieCounter++;
            LblCookieCount.Content = $"{cookieCounter.ToString("00.00")} cookies";
            this.Title = $"{cookieCounter}";
        }

        public void ButtonEnabler()
        {
            if(cookieCounter >= pointerPrice)
            {
                BtnPointer.IsEnabled = true;
            }
            else
            {
                BtnPointer.IsEnabled = false;
            }
        }
        public void PassiveIncome(int counter,decimal ammount)
        {
            cookieCounter += counter * ammount;

        }

        public void RandomTapSound()
        {
            Random random = new Random();
            int number = random.Next(1, 3);
            tapSoundPlayer.Open(new Uri($"Assets/Audio/tap-{number}.wav", UriKind.RelativeOrAbsolute));
            tapSoundPlayer.Stop();
            tapSoundPlayer.Play();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if(button != null)
            {
                string btnContent = button.Name.ToString().Replace("Btn","");
                switch(btnContent)
                {
                    case "Pointer":
                        if(cookieCounter >= pointerPrice)
                        {
                            cookieCounter -= pointerPrice;
                            pointerCounter++;
                            LblPointerCounter.Content = pointerCounter;
                            pointerPrice = Math.Round(pointerCounter * 1.15m * pointerBasePrice);
                            LblPointerPrice.Content = pointerPrice;
                        }
                        break;
                    case "Granny":
                        break;
                    case "Farm":
                        break;
                    case "Mine":
                        break;
                    case "Factory":
                        break;
                }
            }
        }
    }
}

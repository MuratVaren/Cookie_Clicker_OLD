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
        private readonly DispatcherTimer klok = new DispatcherTimer();
        private decimal cookieCounter = 0;

        private int pointerCounter = 0;
        private const decimal pointerBasePrice = 15;
        private decimal pointerPrice = 15;

        private int grannyCounter = 0;
        private const decimal grannyBasePrice = 100;
        private decimal grannyPrice = 100;

        private int farmCounter = 0;
        private const decimal farmBasePrice = 1100;
        private decimal farmPrice = 1100;


        private int mineCounter = 0;
        private const decimal mineBasePrice = 12000;
        private decimal minePrice = 12000;


        private int FactoryCounter = 0;
        private const decimal FactoryBasePrice = 130000;
        private decimal FactoryPrice = 130000;

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
            PassiveIncome(grannyCounter, 0.01m);
            PassiveIncome(farmCounter, 0.08m);
            PassiveIncome(mineCounter, 0.47m);
            PassiveIncome(FactoryCounter, 2.60m);


            LblCookieCount.Content = $"{cookieCounter.ToString("00.00")} cookies";
            LblPassiveIncome.Content = $"+{(pointerCounter * 0.1m) + (grannyCounter * 1m) + (farmCounter * 8m) + (mineCounter * 47m) + (FactoryCounter * 260m)} ";
            this.Title = $"{cookieCounter.ToString("00")} cookies";
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

            if (cookieCounter >= grannyPrice)
            {
                BtnGranny.IsEnabled = true;
            }
            else
            {
                BtnGranny.IsEnabled = false;
            }

            if (cookieCounter >= farmPrice)
            {
                BtnFarm.IsEnabled = true;
            }
            else
            {
                BtnFarm.IsEnabled = false;
            }

            if (cookieCounter >= minePrice)
            {
                BtnMine.IsEnabled = true;
            }
            else
            {
                BtnMine.IsEnabled = false;
            }

            if (cookieCounter >= FactoryPrice)
            {
                BtnFactory.IsEnabled = true;
            }
            else
            {
                BtnFactory.IsEnabled = false;
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
                        if (cookieCounter >= grannyPrice)
                        {
                            cookieCounter -= grannyPrice;
                            grannyCounter++;
                            LblGrannyCounter.Content = grannyCounter;
                            grannyPrice = Math.Round(grannyCounter * 1.15m * grannyBasePrice);
                            LblGrannyPrice.Content = grannyPrice;
                        }
                        break;

                    case "Farm":
                        if (cookieCounter >= farmPrice)
                        {
                            cookieCounter -= farmPrice;
                            farmCounter++;
                            LblFarmCounter.Content = farmCounter;
                            farmPrice = Math.Round(farmCounter * 1.15m * farmBasePrice);
                            LblFarmPrice.Content = farmPrice;
                        }
                        break;

                    case "Mine":
                        if (cookieCounter >= minePrice)
                        {
                            cookieCounter -= minePrice;
                            mineCounter++;
                            LblMineCounter.Content = mineCounter;
                            minePrice = Math.Round(mineCounter * 1.15m * mineBasePrice);
                            LblMinePrice.Content = minePrice;
                        }
                        break;

                    case "Factory":
                        if (cookieCounter >= FactoryPrice)
                        {
                            cookieCounter -= FactoryPrice;
                            FactoryCounter++;
                            LblFactoryCounter.Content = FactoryCounter;
                            FactoryPrice = Math.Round(FactoryCounter * 1.15m * FactoryBasePrice);
                            LblFactoryPrice.Content = FactoryPrice;
                        }
                        break;
                }
            }
        }
    }
}

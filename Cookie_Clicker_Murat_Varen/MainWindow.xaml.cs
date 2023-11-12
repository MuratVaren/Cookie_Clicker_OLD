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

        private double cookieCounter = 0;

        private int pointerCounter = 0;
        private const double pointerBasePrice = 15;
        private double pointerPrice = 15;

        private int grannyCounter = 0;
        private const double grannyBasePrice = 100;
        private double grannyPrice = 100;

        private int farmCounter = 0;
        private const double farmBasePrice = 1100;
        private double farmPrice = 1100;


        private int mineCounter = 0;
        private const double mineBasePrice = 12000;
        private double minePrice = 12000;


        private int factoryCounter = 0;
        private const double factoryBasePrice = 130000;
        private double factoryPrice = 130000;

        

        public MainWindow()
        {
            InitializeComponent();
            klok.Tick += new EventHandler(KlokAfgelopen);
            klok.Interval = new TimeSpan(0, 0, 0, 0, 10);
            klok.Start();
            IdleAnimation();
            ColorAndSizeChanger();
        }
        private void KlokAfgelopen(object sender, EventArgs e)
        {
            // passief inkomen updaten
            PassiveIncome(pointerCounter, 0.001);
            PassiveIncome(grannyCounter, 0.01);
            PassiveIncome(farmCounter, 0.08);
            PassiveIncome(mineCounter, 0.47);
            PassiveIncome(factoryCounter, 2.60);

            // labels en titel updaten
            LblCookieCount.Content = $"{cookieCounter.ToString("00.00")} cookies";
            LblPassiveIncome.Content = $"+{(pointerCounter * 0.1) + (grannyCounter * 1) + (farmCounter * 8) + (mineCounter * 47) + (factoryCounter * 260)} ";
            this.Title = $"{cookieCounter.ToString("00")} cookies";

            ButtonEnabler();
        }
        public void IdleAnimation()
        {
            // new rotatie object 
            RotateTransform rotateTransform = new RotateTransform();
            ImgCookie.RenderTransform = rotateTransform;

            // rotatie via center van afbeelding
            ImgCookie.RenderTransformOrigin = new Point(0.5, 0.5);

            // simple animatie
            DoubleAnimation rotateAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                RepeatBehavior = RepeatBehavior.Forever
            };

            // animatie spelen op de rotatie
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

        }
        private void ImgCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // afbeelding aanpassen bij klik
            BitmapImage bitmapImage =
            new BitmapImage(new Uri("assets/Images/kookie2.png", UriKind.RelativeOrAbsolute));
            ImgCookie.Source = bitmapImage;

            // simple animatie dat thickness kan aanpassen
            ThicknessAnimation animation = new ThicknessAnimation
            {
                To = new Thickness(15),
                Duration = TimeSpan.FromMilliseconds(110),
                FillBehavior = FillBehavior.Stop,
            };
            // animatie einde == terug naar originele afbeelding
            animation.Completed += Animation_Completed;

            // animatie spelen op margin
            ImgCookie.BeginAnimation(Image.MarginProperty, animation);

            RandomTapSound();
            cookieCounter++;
            LblCookieCount.Content = $"{cookieCounter.ToString("00.00")} cookies";
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            // originele afbeelding
            BitmapImage newImage = new BitmapImage(new Uri("assets/Images/cookietest.png", UriKind.RelativeOrAbsolute));
            ImgCookie.Source = newImage;
        }

        public void ColorAndSizeChanger()
        {
            ColorAnimationUsingKeyFrames colorAnimation = new ColorAnimationUsingKeyFrames()
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = TimeSpan.FromMilliseconds(6000)
            };
            colorAnimation.KeyFrames.Add(new LinearColorKeyFrame(Colors.Red, KeyTime.FromPercent(0.0)));
            colorAnimation.KeyFrames.Add(new LinearColorKeyFrame(Colors.Blue, KeyTime.FromPercent(0.5)));
            colorAnimation.KeyFrames.Add(new LinearColorKeyFrame(Colors.Green, KeyTime.FromPercent(1.0)));

            LblCookieCount.Foreground = new SolidColorBrush(Colors.Red);
            DoubleAnimation fontsizeAnimation = new DoubleAnimation()
            {
                From = 24,
                To = 30,
                Duration = TimeSpan.FromMilliseconds(400),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
            };
            LblCookieCount.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            LblCookieCount.BeginAnimation(Label.FontSizeProperty, fontsizeAnimation);
        }
        public void ButtonEnabler()
        {
            // buttons disablen/enablen als je genoeg cookies hebt
            BtnPointer.IsEnabled = cookieCounter >= pointerPrice;
            BtnGranny.IsEnabled = cookieCounter >= grannyPrice;
            BtnFarm.IsEnabled = cookieCounter >= farmPrice;
            BtnMine.IsEnabled = cookieCounter >= minePrice;
            BtnFactory.IsEnabled = cookieCounter >= factoryPrice;
        }
        public void PassiveIncome(int counter,double ammount)
        {
            // simple methode dat cookie counter update
            cookieCounter += counter * ammount;
        }

        public void RandomTapSound()
        {
            // geluid randomizer
            Random random = new Random();
            int number = random.Next(1, 3);
            tapSoundPlayer.Open(new Uri($"Assets/Audio/tap-{number}.wav", UriKind.RelativeOrAbsolute));

            // geluid stoppen als het nog aan het spelen was
            tapSoundPlayer.Stop();
            tapSoundPlayer.Play();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            // een methode voor alle knoppen via sender
            // cookiecounter updaten, labels updaten, upgrades++ en prijs updaten
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
                            pointerPrice = Math.Round(pointerBasePrice * Math.Pow(1.15,pointerCounter));
                            LblPointerPrice.Content = pointerPrice;
                        }
                        break;

                    case "Granny":
                        if (cookieCounter >= grannyPrice)
                        {
                            cookieCounter -= grannyPrice;
                            grannyCounter++;
                            LblGrannyCounter.Content = grannyCounter;
                            grannyPrice = Math.Round(grannyBasePrice * Math.Pow(1.15, grannyCounter));
                            LblGrannyPrice.Content = grannyPrice;
                        }
                        break;

                    case "Farm":
                        if (cookieCounter >= farmPrice)
                        {
                            cookieCounter -= farmPrice;
                            farmCounter++;
                            LblFarmCounter.Content = farmCounter;
                            farmPrice = Math.Round(farmBasePrice * Math.Pow(1.15, farmCounter));
                            LblFarmPrice.Content = farmPrice;
                        }
                        break;

                    case "Mine":
                        if (cookieCounter >= minePrice)
                        {
                            cookieCounter -= minePrice;
                            mineCounter++;
                            LblMineCounter.Content = mineCounter;
                            minePrice = Math.Round(mineBasePrice * Math.Pow(1.15, mineCounter));
                            LblMinePrice.Content = minePrice;
                        }
                        break;

                    case "Factory":
                        if (cookieCounter >= factoryPrice)
                        {
                            cookieCounter -= factoryPrice;
                            factoryCounter++;
                            LblFactoryCounter.Content = factoryCounter;
                            factoryPrice = Math.Round(factoryBasePrice * Math.Pow(1.15, factoryCounter));
                            LblFactoryPrice.Content = factoryPrice;
                        }
                        break;
                }
            }
        }
    }
}

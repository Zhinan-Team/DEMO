using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WznGwent
{
    /// <summary>
    /// PlayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlayWindow : Window
    {
        public PlayWindow()
        {
            InitializeComponent();
            LoadCardSet();
            CardFace leaderCard = myCardSet.LeaderCard;
            List<CardFace> otherCards = myCardSet.Deckcards;
            currentCards.Add(otherCards[1]);
            currentCards.Add(otherCards[3]);
            currentCards.Add(otherCards[5]);
            currentCards.Add(otherCards[2]);
            myCardSetList.ItemsSource = currentCards;
            thrownCardList1.ItemsSource = thrownCards1;
        }
        private CardsModel myCardSet = new CardsModel();
        private ObservableCollection<CardFace> currentCards = new ObservableCollection<CardFace>();
        private ObservableCollection<CardFace> thrownCards1 = new ObservableCollection<CardFace>();
        private ObservableCollection<CardFace> thrownCards2 = new ObservableCollection<CardFace>();
        private ObservableCollection<CardFace> thrownCards3 = new ObservableCollection<CardFace>();
        private CardFace tmpCard;
        private void LoadCardSet()
        {
            if(File.Exists("config.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CardsModel));
                using (FileStream file = new FileStream("config.xml", FileMode.Open, FileAccess.Read))
                {
                    myCardSet = (CardsModel)serializer.Deserialize(file);
                }
            }
        }
        private void DrawCards(List<CardFace> cards)
        {
            //随机抽牌
            //TODO
        }
        private void myCardSetClick(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            tmpCard = (CardFace)border.DataContext;
            int currentIndex = currentCards.IndexOf(tmpCard);
            currentCards.Remove(currentCards[currentIndex]);
            pickedCard.DataContext = tmpCard;
            pickedCard.Visibility = Visibility.Visible;
            //Point point = pickedCard.TranslatePoint(new Point(0, 0),(UIElement)myCardSetList.Items[0]);
            //TranslateTransform tt = new TranslateTransform();
            //pickedCard.RenderTransform = tt;
            //DoubleAnimation daX = new DoubleAnimation();
            //daX.Duration = new Duration(TimeSpan.FromMilliseconds(2000));
            //daX.From = point.X;
            //daX.To = 0;
            //DoubleAnimation daY = new DoubleAnimation();
            //daY.Duration = new Duration(TimeSpan.FromMilliseconds(2000));
            //daY.From = point.Y;
            //daY.To = 0;
            //tt.BeginAnimation(TranslateTransform.XProperty, daX);
            //tt.BeginAnimation(TranslateTransform.YProperty, daY);
        }
        private void cardPickedToSet1(object sender, MouseButtonEventArgs e)
        {
            thrownCards1.Add(tmpCard);
            pickedCard.Visibility = Visibility.Hidden;
        }
    }
}

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
using System.ComponentModel;


namespace WznGwent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.handCradsZone.ItemsSource = tt;
            //DoubleAnimation animScaleX = new DoubleAnimation(1, 50, TimeSpan.FromMilliseconds(500));
            //Storyboard.SetTargetName(animScaleX, myRectangle.Name);
            //Storyboard.SetTargetProperty(animScaleX, new PropertyPath(ScaleTransform.ScaleXProperty));
            //DoubleAnimation animScaleY = new DoubleAnimation(1, 70, TimeSpan.FromMilliseconds(500));
            //Storyboard.SetTargetName(animScaleY, myRectangle.Name);
            //Storyboard.SetTargetProperty(animScaleY, new PropertyPath(ScaleTransform.ScaleYProperty));
            //DoubleAnimation animTrans = new DoubleAnimation(12, 0, TimeSpan.FromMilliseconds(1000));
            //Storyboard.SetTargetName(animTrans, myRectangle.Name);
            //Storyboard.SetTargetProperty(animTrans, new PropertyPath(TranslateTransform.XProperty));
            //Storyboard loadStory = new Storyboard();
            //loadStory.Children.Add(animScaleX);
            //loadStory.Children.Add(animScaleY);
            //loadStory.Children.Add(animTrans);

            //BeginStoryboard myBeginStoryboard = new BeginStoryboard();
            //myBeginStoryboard.Storyboard = loadStory;
            //EventTrigger myBtnClickTrigger = new EventTrigger();
            //myBtnClickTrigger.RoutedEvent = Button.ClickEvent;
            //myBtnClickTrigger.Actions.Add(myBeginStoryboard);
            //testBtn.Triggers.Add(myBtnClickTrigger);
        }
        List<string> tt = new List<string> ();
        //cardFace newCard = new cardFace();

        private void releaseCard(object sender, RoutedEventArgs e)
        {
            // generate a new card obj
            var myRectangle = new Rectangle();
            myRectangle.Width = 1;
            myRectangle.Height = 1;
            // Create an ImageBrush  
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/images/test.jpg", UriKind.RelativeOrAbsolute));
            myRectangle.Fill = imgBrush;
         
            myRectangle.StrokeThickness = 0.02;
            myRectangle.Stroke = Brushes.White;
            //myRectangle.DataContext = newCard;

            //myBinding.Source = ViewModel.SomeString;
            //myBinding.Mode = BindingMode.TwoWay;
            //myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //BindingOperations.SetBinding(txtText, TextBox.TextProperty, myBinding);
            Binding myBinding = new Binding();
            //myBinding.Source = newCard;
            myBinding.Path = new PropertyPath("TestValue");
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(myRectangle, WidthProperty, myBinding);
            //myRectangle.SetBinding(TranslateTransform.XProperty, new Binding("Testvalue"));
            //myRectangle.Width = "{Binding Path=TestValue}";
            //List<string> tt = new List<string> { "222","333"};
            //this.handCradsZone.ItemsSource = tt;

            tt.Add("EEE" );
            this.handCradsZone.Items.Refresh();
            //// define all transforms(scale and translate)
            //ScaleTransform transScale = new ScaleTransform();
            //TranslateTransform transTrans = new TranslateTransform(100, 0);

            ////DoubleAnimation animScaleX = new DoubleAnimation(1, 50, TimeSpan.FromMilliseconds(500));
            ////Storyboard.SetTargetName(animScaleX, myRectangle.Name);
            ////Storyboard.SetTargetProperty(animScaleX, new PropertyPath(ScaleTransform.ScaleXProperty));
            ////DoubleAnimation animScaleY = new DoubleAnimation(1, 70, TimeSpan.FromMilliseconds(500));
            ////Storyboard.SetTargetName(animScaleY, myRectangle.Name);
            ////Storyboard.SetTargetProperty(animScaleY, new PropertyPath(ScaleTransform.ScaleYProperty));
            ////DoubleAnimation animTrans = new DoubleAnimation(12, 0, TimeSpan.FromMilliseconds(1000));
            ////Storyboard.SetTargetName(animTrans, myRectangle.Name);
            ////Storyboard.SetTargetProperty(animTrans, new PropertyPath(TranslateTransform.XProperty));
            ////Storyboard loadStory = new Storyboard();
            ////loadStory.Children.Add(animScaleX);
            ////loadStory.Children.Add(animScaleY);
            ////loadStory.Children.Add(animTrans);

            ////BeginStoryboard myBeginStoryboard = new BeginStoryboard();
            ////myBeginStoryboard.Storyboard = loadStory;
            ////EventTrigger myBtnClickTrigger = new EventTrigger();
            ////myBtnClickTrigger.RoutedEvent = Button.ClickEvent;
            ////myBtnClickTrigger.Actions.Add(myBeginStoryboard);
            ////testBtn.Triggers.Add(myBtnClickTrigger);

            //// add transforms to a transform group
            //TransformGroup transGroup = new TransformGroup();
            //transGroup.Children.Add(transTrans);
            //transGroup.Children.Add(transScale);

            //// add transform grou  p to the card rectangle
            //myRectangle.RenderTransform = transGroup;

            // Use the Loaded event to start the animation
            myRectangle.Loaded += new RoutedEventHandler(myRectangleLoaded);
            //myRectangle.Loaded += delegate (object ssender, RoutedEventArgs ee)
            //{
            //    DoubleAnimation animScaleX = new DoubleAnimation(1, 50, TimeSpan.FromMilliseconds(500));
            //    DoubleAnimation animScaleY = new DoubleAnimation(1, 70, TimeSpan.FromMilliseconds(500));
            //    DoubleAnimation animTrans = new DoubleAnimation(12, 0, TimeSpan.FromMilliseconds(1000));
            //    transScale.BeginAnimation(ScaleTransform.ScaleXProperty, animScaleX);
            //    transScale.BeginAnimation(ScaleTransform.ScaleYProperty, animScaleY);
            //    transTrans.BeginAnimation(TranslateTransform.XProperty, animTrans);
            //};
            Canvas.SetLeft(myRectangle, 760);
            Canvas.SetTop(myRectangle, 4);

            // load the card into hand card zone
            myHandCardsZone.Children.Add(myRectangle);

            //Canvas.SetTop(myRectangle, 0);
            //myCanvas.Children.Add(myRectangle);
            //Canvas.SetLeft(myRectangle, 50);


            //this.Content = myPanel;
            //mainGrid.Children.Add(myCanvas);

        }
        double tempStep = 0.5;
        private void myRectangleLoaded(object sender, RoutedEventArgs e)
        {
            ScaleTransform transScale = new ScaleTransform();
            TranslateTransform transTrans = new TranslateTransform(100, 0);

            TransformGroup transGroup = new TransformGroup();
            transGroup.Children.Add(transTrans);
            transGroup.Children.Add(transScale);

            ((Rectangle)sender).RenderTransform = transGroup;
            DoubleAnimation animScaleX = new DoubleAnimation(1, 50, TimeSpan.FromMilliseconds(500));
            DoubleAnimation animScaleY = new DoubleAnimation(1, 70, TimeSpan.FromMilliseconds(500));
            //DoubleAnimation animTrans2 = new DoubleAnimation(0, 12, TimeSpan.FromMilliseconds(1));

            DoubleAnimation animTrans = new DoubleAnimation(0, -15 + tempStep, TimeSpan.FromMilliseconds(1000));
            transScale.BeginAnimation(ScaleTransform.ScaleXProperty, animScaleX);
            transScale.BeginAnimation(ScaleTransform.ScaleYProperty, animScaleY);
            //transTrans.BeginAnimation(TranslateTransform.XProperty, animTrans2);

            transTrans.BeginAnimation(TranslateTransform.XProperty, animTrans);
            tempStep += 0.5;
        }

        //enum CardFaceAbilities { Berserker, Horn, Hero, Medic, Morale, Muster, Scorch, Spy, Bond, WeatherRain, WeatherFog };
        //enum CardFaceRanges { CloseCombat, Ranged, Siege, Agile };
        //enum CardFaceFactions { NorthernRealms, Nilfgaard };
        //private class cardFace : INotifyPropertyChanged
        //{
        //    public int points;
        //    public CardFaceFactions faction;
        //    public CardFaceRanges range;
        //    public CardFaceAbilities ability;

        //    public event PropertyChangedEventHandler PropertyChanged;
        //    private double testValue;
        //    public double TestValue 
        //    {
        //        get { return testValue; }
        //        set {
        //            if (this.testValue == value) { return; }
        //            testValue = value;
        //            Notify("TestValue");
        //        }
        //    }
        //    protected void Notify(string propName) 
        //    {
        //        if (this.PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        //        }
        //    }
        //}

        private void testBtnClick2(object sender, RoutedEventArgs e)
        {
            //newCard.TestValue += 1;
            //MessageBox.Show(newCard.TestValue.ToString());
        }
    }
}

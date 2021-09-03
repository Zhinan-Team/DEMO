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
using System.Windows.Controls.Primitives;

/** 
 * 虽然说是CustomControl，但其实知识定义了自定义的FlipView和FlipViewPanel,
 * 参考https://www.codeproject.com/Articles/741026/WPF-FlipView
 * 自定义控件需要Style配合，所以在使用其的应用XAML中要加Style，这样
 * 控件就不够封装了。所以更好的做法是将控件封装在另外一个项目中编
 * 译成dll给使用者引用
 */
namespace WznGwent
{
    public class FlipView : Selector
    {
        /**
         * FlipView继承自Selector，那里面的每个ContentControl都是迭代，
         * 之所以定义五个，是打算显示3个时，最外面两个一起加载，以为加载时是
         * 闪现的，这样5个多话，左 滚动时产生一种所有item都已经加载好的无限效果，
         * 将所有的item放在一个UIElement中称为Root，将包含FlipView的容器称为Container
         */
        #region Private Fields
        private ContentControl PART_CurrentItem;
        private ContentControl PART_PrePreviousItem;
        private ContentControl PART_PreviousItem;
        private ContentControl PART_NextItem;
        private ContentControl PART_NexNextItem;

        private FrameworkElement PART_Root;
        private FrameworkElement PART_Container;
        private double fromValue = 0.0;
        private double elasticFactor = 1.0;
        /** 
         * 添加依赖属性，默认情况下FlipView只能看到当前item，但在空速表、角色选择等
         * 组件中往往可以看到前后的item，所以默认情况下，一个item的大小会充满Root，两个
         * item的UIElement不重叠，即左右偏移item元素宽度的倍数，通过itemGap可以控制偏移
         * 的距离，单位为一个item宽度
         */
        public double itemGap
        {
            get { return (double)GetValue(itemGapProperty); }
            set { SetValue(itemGapProperty, value); }
        }
        public static readonly DependencyProperty itemGapProperty = DependencyProperty.Register(
            "itemGap", typeof(double), typeof(FlipView), new PropertyMetadata(1.0));
        #endregion

        #region Constructor
        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
            SelectedIndexProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(-1, OnSelectedIndexChanged));
        }

        //?#1
        public FlipView()
        {
            this.CommandBindings.Add(new CommandBinding(NextCommand, this.OnNextExecuted, this.OnNextCanExecute));
            this.CommandBindings.Add(new CommandBinding(PreviousCommand, this.OnPreviousExecuted, this.OnPreviousCanExecute));
        }
        #endregion

        #region Private methods
        // 响应滚轮
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                this.SelectedIndex += 1;
            if (e.Delta > 0 && this.SelectedIndex > 0)
                this.SelectedIndex -= 1;
        }
        // 触摸手势滑动
        private void OnRootManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            this.fromValue = e.TotalManipulation.Translation.X;
            if (this.fromValue > 0)
            {
                if (this.SelectedIndex > 0)
                {
                    this.SelectedIndex -= 1;
                }
            }
            else
            {
                if (this.SelectedIndex < this.Items.Count - 1)
                {
                    this.SelectedIndex += 1;
                }
            }

            if (this.elasticFactor < 1)
            {
                this.RunSlideAnimation(0, ((MatrixTransform)this.PART_Root.RenderTransform).Matrix.OffsetX);
            }
            this.elasticFactor = 1.0;
        }

        private void OnRootManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (!(this.PART_Root.RenderTransform is MatrixTransform))
            {
                this.PART_Root.RenderTransform = new MatrixTransform();
            }

            Matrix matrix = ((MatrixTransform)this.PART_Root.RenderTransform).Matrix;
            var delta = e.DeltaManipulation;

            if ((this.SelectedIndex == 0 && delta.Translation.X > 0 && this.elasticFactor > 0)
                || (this.SelectedIndex == this.Items.Count - 1 && delta.Translation.X < 0 && this.elasticFactor > 0))
            {
                this.elasticFactor -= 0.05;
            }

            matrix.Translate(delta.Translation.X * elasticFactor, 0);
            this.PART_Root.RenderTransform = new MatrixTransform(matrix);

            e.Handled = true;
        }

        private void OnRootManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this.PART_Container;
            e.Handled = true;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RefreshViewPort(this.SelectedIndex);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.SelectedIndex > -1)
            {
                this.RefreshViewPort(this.SelectedIndex);
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FlipView;

            control.OnSelectedIndexChanged(e);
        }

        private void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!this.EnsureTemplateParts())
            {
                return;
            }

            if ((int)e.NewValue >= 0 && (int)e.NewValue < this.Items.Count)
            {
                double toValue = (int)e.OldValue < (int)e.NewValue ? -this.ActualWidth * this.itemGap : this.ActualWidth * this.itemGap;
                this.RunSlideAnimation(toValue, fromValue);
            }
        }

        private void RefreshViewPort(int selectedIndex)
        {
            if (!this.EnsureTemplateParts())
            {
                return;
            }

            //Console.WriteLine(PART_Root.ActualWidth);
            Canvas.SetLeft(this.PART_PrePreviousItem, -this.ActualWidth * this.itemGap * 2);
            Canvas.SetLeft(this.PART_PreviousItem, -this.ActualWidth * this.itemGap);
            //改位置
            Canvas.SetLeft(this.PART_NextItem, this.ActualWidth * this.itemGap);
            Canvas.SetLeft(this.PART_NexNextItem, this.ActualWidth * this.itemGap * 2);
            this.PART_Root.RenderTransform = new TranslateTransform();

            var currentItem = this.GetItemAt(selectedIndex);
            var nexnextItem = this.GetItemAt(selectedIndex + 2);
            var nextItem = this.GetItemAt(selectedIndex + 1);
            var previousItem = this.GetItemAt(selectedIndex - 1);
            var prepreviousItem = this.GetItemAt(selectedIndex - 2);

            this.PART_CurrentItem.Content = currentItem;
            this.PART_NexNextItem.Content = nexnextItem;
            this.PART_NextItem.Content = nextItem;
            this.PART_PreviousItem.Content = previousItem;
            this.PART_PrePreviousItem.Content = prepreviousItem;

            if (previousItem == null)
                this.PART_PreviousItem.Visibility = Visibility.Hidden;
            else
                this.PART_PreviousItem.Visibility = Visibility.Visible;
            if (nextItem == null)
                this.PART_NextItem.Visibility = Visibility.Hidden;
            else
                this.PART_NextItem.Visibility = Visibility.Visible;
        }

        public void RunSlideAnimation(double toValue, double fromValue = 0)
        {
            if (!(this.PART_Root.RenderTransform is TranslateTransform))
            {
                this.PART_Root.RenderTransform = new TranslateTransform();
            }

            ScaleTransform scaleTransform = new();
            if (PART_CurrentItem.RenderTransform is ScaleTransform scaleTransformTemp)
                scaleTransform = scaleTransformTemp;
            else PART_CurrentItem.RenderTransform = scaleTransform;
            

            var story = AnimationFactory.Instance.GetAnimation(this.PART_Root, toValue, fromValue);
            story.Completed += (s, e) =>
            {
                this.RefreshViewPort(this.SelectedIndex);
            };
            
            DoubleAnimation doubleAnimation = new();
            doubleAnimation.To = 1.5;
            doubleAnimation.Duration = story.Duration;
            doubleAnimation.BeginAnimation(ScaleTransform.ScaleXProperty,doubleAnimation);
            doubleAnimation.BeginAnimation(ScaleTransform.ScaleYProperty,doubleAnimation);
            story.Begin();
        }

        private object GetItemAt(int index)
        {
            if (index < 0 || index >= this.Items.Count)
            {
                return null;
            }

            return this.Items[index];
        }

        private bool EnsureTemplateParts()
        {
            return this.PART_CurrentItem != null &&
                this.PART_NexNextItem != null &&
                this.PART_NextItem != null &&
                this.PART_PreviousItem != null &&
                this.PART_PrePreviousItem != null &&
                this.PART_Root != null;
        }

        private void OnPreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex > 0;
        }

        private void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SelectedIndex -= 1;
        }

        private void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex < (this.Items.Count - 1);
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SelectedIndex += 1;
        }
        #endregion

        #region Commands
        public static RoutedUICommand NextCommand = new RoutedUICommand("Next", "Next", typeof(FlipView));
        public static RoutedUICommand PreviousCommand = new RoutedUICommand("Previous", "Previous", typeof(FlipView));
        #endregion

        #region Override methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_PrePreviousItem = this.GetTemplateChild("PART_PrePreviousItem") as ContentControl;
            this.PART_PreviousItem = this.GetTemplateChild("PART_PreviousItem") as ContentControl;
            this.PART_NextItem = this.GetTemplateChild("PART_NextItem") as ContentControl;
            this.PART_NexNextItem = this.GetTemplateChild("PART_NexNextItem") as ContentControl;
            this.PART_CurrentItem = this.GetTemplateChild("PART_CurrentItem") as ContentControl;
            this.PART_Root = this.GetTemplateChild("PART_Root") as FrameworkElement;
            this.PART_Container = this.GetTemplateChild("PART_Container") as FrameworkElement;

            PART_CurrentItem.RenderTransformOrigin = new Point(0.5, 0.5);
            PART_CurrentItem.RenderTransform = new ScaleTransform(1.5, 1.5);
            
            this.Loaded += this.OnLoaded;
            this.SizeChanged += this.OnSizeChanged;
            this.PART_Root.ManipulationStarting += this.OnRootManipulationStarting;
            this.PART_Root.ManipulationDelta += this.OnRootManipulationDelta;
            this.PART_Root.ManipulationCompleted += this.OnRootManipulationCompleted;
            this.PART_Root.PreviewMouseWheel += this.OnPreviewMouseWheel;
        }
        #endregion

    }

    /**
     * 没有动画的话，上下滚动会是闪现效果（如PPT播放）而不是平滑过渡
     */
    public class AnimationFactory
    {
        public static AnimationFactory Instance
        {
            get
            {
                return new AnimationFactory();
            }
        }

        public Storyboard GetAnimation(DependencyObject target, double to, double from)
        {
            Storyboard story = new Storyboard();
            Storyboard.SetTargetProperty(story, new PropertyPath("(TextBlock.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTarget(story, target);

            var doubleAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame fromFrame = new EasingDoubleKeyFrame(from);
            fromFrame.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };
            fromFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0));

            EasingDoubleKeyFrame toFrame = new EasingDoubleKeyFrame(to);
            toFrame.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };
            toFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));

            doubleAnimation.KeyFrames.Add(fromFrame);
            doubleAnimation.KeyFrames.Add(toFrame);

            story.Children.Add(doubleAnimation);



            //var doubleAnimation2 = new DoubleAnimationUsingKeyFrames();

            //Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(ToggleButton.WidthProperty));
            ////Storyboard.SetTargetName(doubleAnimation2, "PART_CurrentItem");

            //DoubleKeyFrame fromFrame2 = new LinearDoubleKeyFrame(0);

            ////fromFrame2.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };
            //fromFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0));

            //LinearDoubleKeyFrame toFrame2 = new LinearDoubleKeyFrame(300);
            ////toFrame2.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };
            //toFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000));

            //doubleAnimation2.KeyFrames.Add(fromFrame2);
            //doubleAnimation2.KeyFrames.Add(toFrame2);

            ////实例化一个DoubleAnimation类。
            //DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            //Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(ToggleButton.WidthProperty));

            ////设置From属性。
            //doubleAnimation2.From =100;
            ////设置To属性。
            //doubleAnimation2.To = 250;
            ////设置Duration属性。
            //doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(5));

            //story.Children.Add(doubleAnimation2);

            return story;
        }
    }

    /**
     * 为Root定义FlipViewPanel
     */
    public class FlipViewPanel : Panel
    {
        protected override Size MeasureOverride(System.Windows.Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                if (child == null) { continue; }
                child.Measure(availableSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                double top = Canvas.GetTop(child);
                double left = Canvas.GetLeft(child);

                left = Double.IsNaN(left) ? 0.0 : left;
                top = Double.IsNaN(top) ? 0.0 : top;

                child.Arrange(new Rect(left, top, finalSize.Width, finalSize.Height));
            }
            return finalSize;
        }
    }

}

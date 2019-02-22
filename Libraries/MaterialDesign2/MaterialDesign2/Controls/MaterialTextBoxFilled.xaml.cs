using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialDesign2.Controls
{
    public partial class MaterialTextBoxFilled : TextBox
    {
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public ImageSource GetIcon() => Icon;
        public void SetIcon(ImageSource value) => Icon = value;
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(ImageSource), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public string GetLabel() => Label;
        public void SetLabel(string value) => Label = value;
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.RegisterAttached("Label", typeof(string), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        public SolidColorBrush InactiveLabelBrush
        {
            get { return (SolidColorBrush)GetValue(InactiveLabelBrushProperty); }
            set { SetValue(InactiveLabelBrushProperty, value); }
        }
        public SolidColorBrush GetInactiveLabelColor() => InactiveLabelBrush;
        public void SetInactiveLabelColor(SolidColorBrush value) => InactiveLabelBrush = value;
        public static readonly DependencyProperty InactiveLabelBrushProperty =
            DependencyProperty.RegisterAttached("InactiveLabelColor", typeof(SolidColorBrush), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.Inherits));

        public SolidColorBrush ActiveLabelBrush
        {
            get { return (SolidColorBrush)GetValue(ActiveLabelBrushProperty); }
            set { SetValue(ActiveLabelBrushProperty, value); }
        }
        public SolidColorBrush GetActiveLabelColor() => ActiveLabelBrush;
        public void SetActiveLabelColor(SolidColorBrush value) => ActiveLabelBrush = value;
        public static readonly DependencyProperty ActiveLabelBrushProperty =
            DependencyProperty.RegisterAttached("ActiveLabelBrush", typeof(SolidColorBrush), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7A7FEE")), FrameworkPropertyMetadataOptions.Inherits));

        #region Label Font
        public FontFamily LabelFontFamily
        {
            get { return (FontFamily)GetValue(LabelFontFamilyProperty); }
            set { SetValue(LabelFontFamilyProperty, value); }
        }
        public FontFamily GetLabelFontFamily() => LabelFontFamily;
        public void SetLabelFontFamily(FontFamily value) => LabelFontFamily = value;
        public static readonly DependencyProperty LabelFontFamilyProperty =
            DependencyProperty.RegisterAttached("LabelFontFamily", typeof(FontFamily), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(new FontFamily("/MaterialDesign2;component/Fonts/Roboto/#Roboto"), FrameworkPropertyMetadataOptions.Inherits));

        public FontStyle LabelFontStyle
        {
            get { return (FontStyle)GetValue(LabelFontStyleProperty); }
            set { SetValue(LabelFontStyleProperty, value); }
        }
        public FontStyle GetLabelFontStyle() => LabelFontStyle;
        public void SetLabelFontStyle(FontStyle value) => LabelFontStyle = value;
        public static readonly DependencyProperty LabelFontStyleProperty =
            DependencyProperty.RegisterAttached("LabelFontStyle", typeof(FontStyle), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(FontStyles.Normal, FrameworkPropertyMetadataOptions.Inherits));

        public FontWeight LabelFontWeight
        {
            get { return (FontWeight)GetValue(LabelFontWeightProperty); }
            set { SetValue(LabelFontWeightProperty, value); }
        }
        public FontWeight GetLabelFontWeight() => LabelFontWeight;
        public void SetLabelFontWeight(FontWeight value) => LabelFontWeight = value;
        public static readonly DependencyProperty LabelFontWeightProperty =
            DependencyProperty.RegisterAttached("LabelFontWeight", typeof(FontWeight), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(FontWeights.Light, FrameworkPropertyMetadataOptions.Inherits));

        public double LabelFontSize
        {
            get { return (double)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }
        public double GetLabelFontSize() => LabelFontSize;
        public void SetLabelFontSize(double value) => LabelFontSize = value;
        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.RegisterAttached("LabelFontSize", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(30d, FrameworkPropertyMetadataOptions.Inherits));

        public double ActiveLabelFontSize
        {
            get { return (double)GetValue(ActiveLabelFontSizeProperty); }
            set { SetValue(ActiveLabelFontSizeProperty, value); }
        }
        public double GetActiveLabelFontSize() => ActiveLabelFontSize;
        public void SetActiveLabelFontSize(double value) => ActiveLabelFontSize = value;
        public static readonly DependencyProperty ActiveLabelFontSizeProperty =
            DependencyProperty.RegisterAttached("ActiveLabelFontSize", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(15d, FrameworkPropertyMetadataOptions.Inherits));

        public TextAlignment LabelTextAlignment
        {
            get { return (TextAlignment)GetValue(LabelTextAlignmentProperty); }
            set { SetValue(LabelTextAlignmentProperty, value); }
        }
        public TextAlignment GetLabelTextAlignment() => LabelTextAlignment;
        public void SetLabelTextAlignment(TextAlignment value) => LabelTextAlignment = value;
        public static readonly DependencyProperty LabelTextAlignmentProperty =
            DependencyProperty.RegisterAttached("LabelTextAlignment", typeof(TextAlignment), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(TextAlignment.Left, FrameworkPropertyMetadataOptions.Inherits));
        #endregion
        
        #region Columns
        public GridLength LeftWidth
        {
            get { return (GridLength)GetValue(LeftWidthProperty); }
            set { SetValue(LeftWidthProperty, value); }
        }
        public GridLength GetLeftWidth() => LeftWidth;
        public void SetLeftWidth(GridLength value) => LeftWidth = value;
        public static readonly DependencyProperty LeftWidthProperty =
            DependencyProperty.RegisterAttached("LeftWidth", typeof(GridLength), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(new GridLength(0, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits));

        public double LeftMinWidth
        {
            get { return (double)GetValue(LeftMinWidthProperty); }
            set { SetValue(LeftMinWidthProperty, value); }
        }
        public double GetLeftMinWidth() => LeftMinWidth;
        public void SetLeftMinWidth(double value) => LeftMinWidth = value;
        public static readonly DependencyProperty LeftMinWidthProperty =
            DependencyProperty.RegisterAttached("LeftMinWidth", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.Inherits));

        public double LeftMaxWidth
        {
            get { return (double)GetValue(LeftMaxWidthProperty); }
            set { SetValue(LeftMaxWidthProperty, value); }
        }
        public double GetLeftMaxWidth() => LeftMaxWidth;
        public void SetLeftMaxWidth(double value) => LeftMaxWidth = value;
        public static readonly DependencyProperty LeftMaxWidthProperty =
            DependencyProperty.RegisterAttached("LeftMaxWidth", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));


        public GridLength RightWidth
        {
            get { return (GridLength)GetValue(RightWidthProperty); }
            set { SetValue(RightWidthProperty, value); }
        }
        public GridLength GetRightWidth() => RightWidth;
        public void SetRightWidth(GridLength value) => RightWidth = value;
        public static readonly DependencyProperty RightWidthProperty =
            DependencyProperty.RegisterAttached("RightWidth", typeof(GridLength), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(new GridLength(0, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits));

        public double RightMinWidth
        {
            get { return (double)GetValue(RightMinWidthProperty); }
            set { SetValue(RightMinWidthProperty, value); }
        }
        public double GetRightMinWidth() => RightMinWidth;
        public void SetRightMinWidth(double value) => RightMinWidth = value;
        public static readonly DependencyProperty RightMinWidthProperty =
            DependencyProperty.RegisterAttached("RightMinWidth", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.Inherits));

        public double RightMaxWidth
        {
            get { return (double)GetValue(RightMaxWidthProperty); }
            set { SetValue(RightMaxWidthProperty, value); }
        }
        public double GetRightMaxWidth() => RightMaxWidth;
        public void SetRightMaxWidth(double value) => RightMaxWidth = value;
        public static readonly DependencyProperty RightMaxWidthProperty =
            DependencyProperty.RegisterAttached("RightMaxWidth", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));
        #endregion

        public double LabelMoveDuration
        {
            get { return (double)GetValue(LabelMoveDurationProperty); }
            set { SetValue(LabelMoveDurationProperty, value); }
        }
        public double GetLabelMoveDuration() => LabelMoveDuration;
        public void SetLabelMoveDuration(double value) => LabelMoveDuration = value;
        public static readonly DependencyProperty LabelMoveDurationProperty =
            DependencyProperty.RegisterAttached("LabelMoveDuration", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(0.2d, FrameworkPropertyMetadataOptions.Inherits));

        public double IndicatorThickness
        {
            get { return (double)GetValue(IndicatorThicknessProperty); }
            set { SetValue(IndicatorThicknessProperty, value); }
        }
        public double GetIndicatorThickness() => IndicatorThickness;
        public void SetIndicatorThickness(double value) => IndicatorThickness = value;
        public static readonly DependencyProperty IndicatorThicknessProperty =
            DependencyProperty.RegisterAttached("IndicatorThickness", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.Inherits));
        
        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        public IEasingFunction GetEasingFunction() => EasingFunction;
        public void SetEasingFunction(IEasingFunction value) => EasingFunction = value;
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.RegisterAttached("EasingFunction", typeof(IEasingFunction), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(new PowerEase(), FrameworkPropertyMetadataOptions.Inherits));

        public NumberPaneMode NumberPane
        {
            get { return (NumberPaneMode)GetValue(NumberPaneProperty); }
            set { SetValue(NumberPaneProperty, value); }
        }
        public NumberPaneMode GetNumberPane() => NumberPane;
        public void SetNumberPane(NumberPaneMode value) => NumberPane = value;
        public static readonly DependencyProperty NumberPaneProperty =
            DependencyProperty.RegisterAttached("NumberPane", typeof(NumberPaneMode), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(NumberPaneMode.Off, FrameworkPropertyMetadataOptions.Inherits));

        public enum NumberPaneMode
        {
            Off,
            RealPlusMinus,
            RealPlus,
            WholePlusMinus,
            WholePlus,
        }

        public double NumberValue
        {
            get { return (double)GetValue(NumberValueProperty); }
            set { SetValue(NumberValueProperty, value); }
        }
        public double GetNumberValue() => NumberValue;
        public void SetNumberValue(double value) => NumberValue = value;
        public static readonly DependencyProperty NumberValueProperty =
            DependencyProperty.RegisterAttached("NumberValue", typeof(double), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Inherits));

        public bool FractionAllowed
        {
            get { return (bool)GetValue(FractionAllowedProperty); }
            set { SetValue(FractionAllowedProperty, value); }
        }
        public bool GetFractionAllowed() => FractionAllowed;
        public void SetFractionAllowed(bool value) => FractionAllowed = value;
        public static readonly DependencyProperty FractionAllowedProperty =
            DependencyProperty.RegisterAttached("FractionAllowed", typeof(bool), typeof(MaterialTextBoxFilled), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
        
        public string Heading { get; set; }

        #region Rows
        public GridLength RowsActiveLabelRowHeight
        {
            get { return (GridLength)GetValue(RowsActiveLabelRowHeightProperty); }
            set { SetValue(RowsActiveLabelRowHeightProperty, value); }
        }
        public GridLength GetRowsActiveLabelRowHeight() => RowsActiveLabelRowHeight;
        public void SetRowsActiveLabelRowHeight(GridLength value) => RowsActiveLabelRowHeight = value;
        public static readonly DependencyProperty RowsActiveLabelRowHeightProperty =
            DependencyProperty.RegisterAttached("RowsActiveLabelRowHeight", typeof(GridLength), typeof(MaterialButton), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits));


        public double RowsActiveLabelRowMinHeight
        {
            get { return (double)GetValue(RowsActiveLabelRowMinHeightProperty); }
            set { SetValue(RowsActiveLabelRowMinHeightProperty, value); }
        }
        public double GetRowsActiveLabelRowMinHeight() => RowsActiveLabelRowMinHeight;
        public void SetRowsActiveLabelRowMinHeight(double value) => RowsActiveLabelRowMinHeight = value;
        public static readonly DependencyProperty RowsActiveLabelRowMinHeightProperty =
            DependencyProperty.RegisterAttached("RowsActiveLabelRowMinHeight", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Inherits));

        public double RowsActiveLabelRowMaxHeight
        {
            get { return (double)GetValue(RowsActiveLabelRowMaxHeightProperty); }
            set { SetValue(RowsActiveLabelRowMaxHeightProperty, value); }
        }
        public double GetRowsActiveLabelRowMaxHeight() => RowsActiveLabelRowMaxHeight;
        public void SetRowsActiveLabelRowMaxHeight(double value) => RowsActiveLabelRowMaxHeight = value;
        public static readonly DependencyProperty RowsActiveLabelRowMaxHeightProperty =
            DependencyProperty.RegisterAttached("RowsActiveLabelRowMaxHeight", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));
        



        public GridLength RowsTextRowHeight
        {
            get { return (GridLength)GetValue(RowsTextRowHeightProperty); }
            set { SetValue(RowsTextRowHeightProperty, value); }
        }
        public GridLength GetRowsTextRowHeight() => RowsTextRowHeight;
        public void SetRowsTextRowHeight(GridLength value) => RowsTextRowHeight = value;
        public static readonly DependencyProperty RowsTextRowHeightProperty =
            DependencyProperty.RegisterAttached("RowsTextRowHeight", typeof(GridLength), typeof(MaterialButton), new FrameworkPropertyMetadata(new GridLength(1.5, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits));


        public double RowsTextRowMinHeight
        {
            get { return (double)GetValue(RowsTextRowMinHeightProperty); }
            set { SetValue(RowsTextRowMinHeightProperty, value); }
        }
        public double GetRowsTextRowMinHeight() => RowsTextRowMinHeight;
        public void SetRowsTextRowMinHeight(double value) => RowsTextRowMinHeight = value;
        public static readonly DependencyProperty RowsTextRowMinHeightProperty =
            DependencyProperty.RegisterAttached("RowsTextRowMinHeight", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Inherits));

        public double RowsTextRowMaxHeight
        {
            get { return (double)GetValue(RowsTextRowMaxHeightProperty); }
            set { SetValue(RowsTextRowMaxHeightProperty, value); }
        }
        public double GetRowsTextRowMaxHeight() => RowsTextRowMaxHeight;
        public void SetRowsTextRowMaxHeight(double value) => RowsTextRowMaxHeight = value;
        public static readonly DependencyProperty RowsTextRowMaxHeightProperty =
            DependencyProperty.RegisterAttached("RowsTextRowMaxHeight", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));




        public GridLength RowsPaddingBottomHeight
        {
            get { return (GridLength)GetValue(RowsPaddingBottomHeightProperty); }
            set { SetValue(RowsPaddingBottomHeightProperty, value); }
        }
        public GridLength GetRowsPaddingBottomHeight() => RowsPaddingBottomHeight;
        public void SetRowsPaddingBottomHeight(GridLength value) => RowsPaddingBottomHeight = value;
        public static readonly DependencyProperty RowsPaddingBottomHeightProperty =
            DependencyProperty.RegisterAttached("RowsPaddingBottomHeight", typeof(GridLength), typeof(MaterialButton), new FrameworkPropertyMetadata(new GridLength(0.2, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits));


        public double RowsPaddingBottomMinHeight
        {
            get { return (double)GetValue(RowsPaddingBottomMinHeightProperty); }
            set { SetValue(RowsPaddingBottomMinHeightProperty, value); }
        }
        public double GetRowsPaddingBottomMinHeight() => RowsPaddingBottomMinHeight;
        public void SetRowsPaddingBottomMinHeight(double value) => RowsPaddingBottomMinHeight = value;
        public static readonly DependencyProperty RowsPaddingBottomMinHeightProperty =
            DependencyProperty.RegisterAttached("RowsPaddingBottomMinHeight", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Inherits));

        public double RowsPaddingBottomMaxHeight
        {
            get { return (double)GetValue(RowsPaddingBottomMaxHeightProperty); }
            set { SetValue(RowsPaddingBottomMaxHeightProperty, value); }
        }
        public double GetRowsPaddingBottomMaxHeight() => RowsPaddingBottomMaxHeight;
        public void SetRowsPaddingBottomMaxHeight(double value) => RowsPaddingBottomMaxHeight = value;
        public static readonly DependencyProperty RowsPaddingBottomMaxHeightProperty =
            DependencyProperty.RegisterAttached("RowsPaddingBottomMaxHeight", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));
        #endregion

        public MaterialTextBoxFilled()
        {
            InitializeComponent();

            GotFocus += MaterialTextBox_GotFocus;
            LostFocus += MaterialTextBox_LostFocus;
            MouseDown += (s, e) => { if (!IsMouseOver) MaterialTextBox_LostFocus(null, null); };
            
            Loaded += (s, e) =>
            {
                try
                {
                    MaterialTextBox_GotFocus(null, null);
                    MaterialTextBox_LostFocus(null, null);
                    Grid_SizeChanged(null, null);
                }
                catch { }
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void MaterialTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TimeSpan duration = TimeSpan.FromSeconds(LabelMoveDuration);


            var Indicator = (Rectangle)Template.FindName("Indicator", this);


            DoubleAnimation indicatorHeightAnimation = new DoubleAnimation()
            { From = Indicator.ActualHeight, To = IndicatorThickness, Duration = TimeSpan.FromSeconds(LabelMoveDuration / 5d), EasingFunction = EasingFunction, };
            Indicator.BeginAnimation(HeightProperty, indicatorHeightAnimation);

            DoubleAnimation indicatorWidthAnimation = new DoubleAnimation()
            { From = Indicator.ActualWidth, To = ActualWidth, Duration = duration, EasingFunction = EasingFunction, };
            Indicator.BeginAnimation(WidthProperty, indicatorWidthAnimation);
            

            var LabelContainer = (Border)Template.FindName("LabelContainer", this);
            var ActiveLabelRowMarker = (Border)Template.FindName("ActiveLabelRowMarker", this);
            var Rows = (Grid)Template.FindName("Rows", this);
            Point markerPos = ActiveLabelRowMarker.TranslatePoint(new Point(), Rows);

            ThicknessAnimation labelAnimation = new ThicknessAnimation()
            { From = LabelContainer.Margin, To = ThicknessFromY(markerPos.Y, Rows.ActualHeight, ActiveLabelFontSize), Duration = duration, EasingFunction = EasingFunction, };

            LabelContainer.BeginAnimation(MarginProperty, labelAnimation);


            var LabelBlock = (TextBlock)Template.FindName("LabelBlock", this);


            DoubleAnimation sizeAnimation = new DoubleAnimation()
            { From = LabelBlock.FontSize, To = ActiveLabelFontSize, Duration = duration, EasingFunction = EasingFunction, };
            LabelBlock.BeginAnimation(FontSizeProperty, sizeAnimation);

            SolidColorBrushAnimation colorAnimation = new SolidColorBrushAnimation()
            { From = InactiveLabelBrush, To = ActiveLabelBrush, Duration = duration, EasingFunction = EasingFunction, };
            LabelBlock.BeginAnimation(TextBlock.ForegroundProperty, colorAnimation);
        }

        private void MaterialTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TimeSpan duration = TimeSpan.FromSeconds(LabelMoveDuration);


            var Indicator = (Rectangle)Template.FindName("Indicator", this);


            DoubleAnimation indicatorHeightAnimation = new DoubleAnimation()
            { From = Indicator.ActualHeight, To = 0, Duration = TimeSpan.FromSeconds(LabelMoveDuration * 5d), EasingFunction = EasingFunction, };
            Indicator.BeginAnimation(HeightProperty, indicatorHeightAnimation);

            DoubleAnimation indicatorWidthAnimation = new DoubleAnimation()
            { From = Indicator.ActualWidth, To = 0, Duration = duration, EasingFunction = EasingFunction, };
            Indicator.BeginAnimation(WidthProperty, indicatorWidthAnimation);


            if (!string.IsNullOrWhiteSpace(ThisTextBox.Text)) return;

            
            var LabelContainer = (Border)Template.FindName("LabelContainer", this);
            var TextRowMarker = (Border)Template.FindName("TextRowMarker", this);
            var Rows = (Grid)Template.FindName("Rows", this);
            Point markerPos = TextRowMarker.TranslatePoint(new Point(), Rows);

            ThicknessAnimation labelAnimation = new ThicknessAnimation()
            { From = LabelContainer.Margin, To = ThicknessFromY(markerPos.Y, Rows.ActualHeight, FontSize), Duration = duration, EasingFunction = EasingFunction, };

            LabelContainer.BeginAnimation(MarginProperty, labelAnimation);



            var LabelBlock = (TextBlock)Template.FindName("LabelBlock", this);


            DoubleAnimation sizeAnimation = new DoubleAnimation()
            { From = LabelBlock.FontSize, To = LabelFontSize, Duration = duration, EasingFunction = EasingFunction, };
            LabelBlock.BeginAnimation(FontSizeProperty, sizeAnimation);

            SolidColorBrushAnimation colorAnimation = new SolidColorBrushAnimation()
            { From = ActiveLabelBrush, To = InactiveLabelBrush, Duration = duration, EasingFunction = EasingFunction, };
            LabelBlock.BeginAnimation(TextBlock.ForegroundProperty, colorAnimation);

            if (NumberPane != NumberPaneMode.Off) ParseText();
        }

        private Thickness ThicknessFromY(double y, double height, double size) => new Thickness(0, 0, 0, height-y);

        private void ParseText()
        {
            if (double.TryParse(Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value)) NumberValue = value;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var ActiveLabelRowMarker = (Border)Template.FindName("ActiveLabelRowMarker", this);
            var TextRowMarker = (Border)Template.FindName("TextRowMarker", this);
            var Rows = (Grid)Template.FindName("Rows", this);
            
            var TextContainer = (Border)Template.FindName("TextContainer", this);
            Point markerPos = TextRowMarker.TranslatePoint(new Point(), Rows);

            TextContainer.Margin = ThicknessFromY(markerPos.Y, Rows.ActualHeight, FontSize);

            var LabelContainer = (Border)Template.FindName("LabelContainer", this);
            bool focused = IsFocused || (!IsFocused && Text != "");
            markerPos = (focused ? ActiveLabelRowMarker : TextRowMarker).TranslatePoint(new Point(), Rows);

            LabelContainer.Margin = ThicknessFromY(markerPos.Y, Rows.ActualHeight, 0);
        }

        private void Text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter) ThisTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            else e.Handled = !IsTextAllowed(Text + "0");
        }

        private bool IsTextAllowed(string text)
        {
            double valueDouble;
            int valueInt;
            switch (NumberPane)
            {
                case NumberPaneMode.Off: return true;
                case NumberPaneMode.RealPlus: return double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out valueDouble) && (NumberValue = valueDouble) >= 0d;
                case NumberPaneMode.RealPlusMinus: return double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out valueDouble) && (NumberValue = valueDouble) >= double.MinValue;
                case NumberPaneMode.WholePlus: return int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out valueInt) && (NumberValue = valueInt) >= 0d;
                case NumberPaneMode.WholePlusMinus: return int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out valueInt) && (NumberValue = valueInt) >= int.MinValue;
                default: return true;
            }
        }
            
        private void Text_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));

                if (!IsTextAllowed(text)) e.CancelCommand(); 
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
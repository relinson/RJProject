using System.Windows.Controls;
using System.Windows;

namespace PRO_ReceiptsInvMgr.Client.UserControls
{
    public static class DatePickerHelper
    {
        public static readonly DependencyProperty WatermarkProperty =
        DependencyProperty.RegisterAttached("Watermark", typeof(object), typeof(DatePickerHelper),
        new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(OnWatermarkChanged)));
        public static object GetWatermark(DatePicker d)
        {
            return d.GetValue(WatermarkProperty);
        }
        public static void SetWatermark(DatePicker d, object value)
        {
            d.SetValue(WatermarkProperty, value);
        }
        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = d as DatePicker;
            if (datePicker == null)
            {
                return;
            }
            if (datePicker.IsLoaded)
            {
                SetWatermarkInternal(datePicker, e.NewValue);
            }
            else
            {
                RoutedEventHandler loadedHandler = null;
                loadedHandler = delegate
                {
                    datePicker.Loaded -= loadedHandler;
                    SetWatermarkInternal(datePicker, e.NewValue);
                };
                datePicker.Loaded += loadedHandler;
            }
        }
        private static void SetWatermarkInternal(DatePicker d, object value)
        {
            var textBox = d.Template.FindName("PART_TextBox", d) as Control;
            if (textBox != null)
            {
                var watermarkControl = textBox.Template.FindName("PART_Watermark", textBox) as ContentControl;
                if (watermarkControl != null)
                {
                    watermarkControl.Content = value;
                }
            }
        }
    }

}

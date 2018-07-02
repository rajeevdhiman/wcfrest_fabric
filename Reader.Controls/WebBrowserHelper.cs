using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Reader.Controls
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty SourceUriProperty = DependencyProperty.RegisterAttached("SourceUri",
            typeof(Uri), typeof(WebBrowserHelper), new FrameworkPropertyMetadata(null, 
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, SourceUriChanged));

        public static Uri GetSourceUri(DependencyObject obj) => (Uri)obj.GetValue(SourceUriProperty);
        public static void SetSourceUri(DependencyObject obj, Uri value) => obj.SetValue(SourceUriProperty, value);

        private static void SourceUriChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is WebBrowser && e.NewValue is Uri)
            {
                var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes((e.NewValue as Uri).AbsoluteUri));
                var serviceUri = $"http://codezilla.westus2.cloudapp.azure.com/News/Data?args={base64}";
                //var serviceUri = $"http://localhost:21880/News/Data?args={base64}";
                (sender as WebBrowser).Navigate(serviceUri);
            }
        }
    }
}

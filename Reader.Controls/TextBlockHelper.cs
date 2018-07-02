using FabricWCF.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Reader.Controls
{
    public static class TextBlockHelper 
    {
        public static readonly DependencyProperty FormattedImageProperty = DependencyProperty.RegisterAttached("FormattedImage",
            typeof(string), typeof(TextBlockHelper), new FrameworkPropertyMetadata("",
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, FormattedImageChanged));

        public static readonly DependencyProperty FormattedTextProperty = DependencyProperty.RegisterAttached("FormattedText",
            typeof(string), typeof(TextBlockHelper), new FrameworkPropertyMetadata("",
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, FormattedTextChanged));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", 
                typeof(ICommand), typeof(TextBlockHelper), new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.Inherits, CommandChanged));

        public static Regex attribPattern = new Regex(@"(?<name>\w+)=['""]?(?<value>[^'""]+)", RegexOptions.Compiled);

        public static Regex tokenPattern = new Regex(@"<(?<token>(?<tokenName>\w+)[^>]*)>(?<inlines>.*?)(?:</\2>)|<(?<token>(?<tokenName>\w+)[^>]*)>", RegexOptions.Compiled);

        public static string GetFormattedImage(DependencyObject obj) => (string)obj.GetValue(FormattedImageProperty);
        public static string GetFormattedText(DependencyObject obj) => (string)obj.GetValue(FormattedTextProperty);
        public static ICommand GetCommand(DependencyObject obj) => (ICommand)obj.GetValue(CommandProperty);
        public static void SetFormattedImage(DependencyObject obj, string value) => obj.SetValue(FormattedImageProperty, value);
        public static void SetFormattedText(DependencyObject obj, string value) => obj.SetValue(FormattedTextProperty, value);
        public static void SetCommand(DependencyObject obj, ICommand value) => obj.SetValue(CommandProperty, value);

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(CommandProperty, e.NewValue);
            var textValue = d.GetValue(FormattedTextProperty);
            d.SetValue(FormattedTextProperty, FormattedTextProperty.DefaultMetadata.DefaultValue);
            d.SetValue(FormattedTextProperty, textValue);
        }

        private static void FormattedImageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            string value = e.NewValue as string;
            TextBlock textBlock = sender as TextBlock;

            var unformattedText = value;
            while (tokenPattern.IsMatch(unformattedText))
            {
                unformattedText = tokenPattern.Replace(unformattedText, "$2");
            }
            textBlock.Text = unformattedText;

            if (textBlock != null)
            {
                textBlock.Inlines.Clear();
                textBlock.Inlines.AddRange(LocateImage(value));
            }
        }

        private static void FormattedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            string value = e.NewValue as string;
            TextBlock textBlock = sender as TextBlock;

            var unformattedText = value;
            while (value != null && tokenPattern.IsMatch(unformattedText))
            {
                unformattedText = tokenPattern.Replace(unformattedText, "$2");
            }
            textBlock.Text = unformattedText;

            if (value != null && textBlock != null)
            {
                textBlock.Inlines.Clear();
                textBlock.Inlines.Add(Traverse(sender, value));
            }
        }

        /// <summary>
        /// Examines the passed string and find the first token, where it begins and where it ends.
        /// </summary>
        /// <param name="value">The string to examine.</param>
        /// <param name="token">The found token.</param>
        /// <returns>True if a token was found.</returns>
        private static bool GetTokenInfo(string value, out Token token)
        {
            token = null;
            if (tokenPattern.IsMatch(value) == false) return false;

            var tokenInfo = tokenPattern.Match(value); ;
            token = new Token
            {
                Name = tokenInfo.Groups["tokenName"].Value,
                Index = tokenInfo.Index,
                Inline = tokenInfo.Groups["inlines"].Value,
                Value = tokenInfo.Groups["token"].Value,
                Length = tokenInfo.Length
            };

            if (attribPattern.IsMatch(token.Value))
            {
                foreach (Match item in attribPattern.Matches(token.Value))
                {
                    token.Attributes.Add(item.Groups["name"].Value, item.Groups["value"].Value);
                }
            }

            return true;
        }

        private static Inline LocateImage(Token token)
        {
            // Get the content to further examination
            string content = token.Inline;
            if (token.Name.ToUpper() == "IMG")
            {
                var span = new Span()
                {
                    BaselineAlignment = BaselineAlignment.Bottom
                };
                var src = token.Attributes["src"];
                if (Uri.IsWellFormedUriString(src, UriKind.Absolute))
                {
                    Image finalImage = new Image() { Margin = new Thickness(4), Stretch = Stretch.Uniform, MaxHeight = 80 };
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(src);
                    logo.EndInit();
                    finalImage.Source = logo;
                    span.Inlines.Add(finalImage);
                }
                return span;
            }
            else
            {
                Token childToken;
                if (GetTokenInfo(token.Inline, out childToken))
                    return LocateImage(childToken);
                else
                    return new Run();
            }
        }

        private static IEnumerable<Inline> LocateImage(string value)
        {
            string[] sections = SplitIntoSections(value);
            foreach (string section in sections)
            {
                Token token;
                if (GetTokenInfo(section, out token))
                    yield return LocateImage(token);
            }
        }

        private static string[] SplitIntoSections(string value)
        {
            List<string> sections = new List<string>();

            while (!string.IsNullOrEmpty(value))
            {
                Token token;
                if (GetTokenInfo(value, out token))
                {
                    if (token.Index > 0)
                        sections.Add(value.Substring(0, token.Index));

                    sections.Add(value.Substring(token.Index, token.Length));
                    value = value.Substring(token.EndIndex); // Trim away
                }
                else // No tokens, just add the text
                {
                    if (!string.IsNullOrWhiteSpace(value))
                        sections.Add(value);
                    value = null;
                }
            }

            return sections.ToArray();
        }

        private static Inline Traverse(DependencyObject sender, string value, bool loadImages = false)
        {
            string[] sections = SplitIntoSections(value);

            if (sections.Length.Equals(1))
            {
                string section = sections[0];
                Token token;
                if (GetTokenInfo(section, out token))
                {
                    // Get the content to further examination
                    string content = token.Inline;
                    switch (token.Name.ToUpper())
                    {
                        case "B":
                        case "STRONG": return new Bold(Traverse(sender, content));
                        case "I": return new Italic(Traverse(sender, content));
                        case "U": return new Underline(Traverse(sender, content));
                        case "BR": return new LineBreak();
                        case "A":
                            var href = token.Attributes["href"];
                            var link = new Hyperlink(Traverse(sender, content));
                            link.Command = GetCommand(sender);
                            if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
                            {
                                link.CommandParameter = href;
                                link.NavigateUri = new Uri(href);
                            }
                            return link;

                        case "FONT":
                            var foreColor = ColorConverter.ConvertFromString(token.Attributes["color"]);
                            var font = Traverse(sender, content);
                            if (foreColor != null) font.Foreground = new SolidColorBrush((Color)foreColor);
                            return font;

                        case "IMG":
                            var imgSpan = new Span()
                            {
                                BaselineAlignment = BaselineAlignment.Bottom
                            };
                            if (loadImages)
                            {
                                var src = token.Attributes["src"];
                                Uri resource;

                                if (Uri.TryCreate(src, UriKind.Absolute, out resource))
                                {
                                    Image finalImage = new Image() { Margin = new Thickness(4), Stretch = Stretch.Uniform, MaxHeight = 80 };
                                    BitmapImage logo = new BitmapImage();
                                    logo.BeginInit();
                                    if (resource.Scheme == "data")
                                    {
                                        var base64 = resource.DecodeDataUri();
                                        logo.StreamSource = new MemoryStream(base64);
                                    }
                                    else
                                    {
                                        logo.UriSource = new Uri(src);
                                    }
                                    logo.EndInit();
                                    finalImage.Source = logo;
                                    imgSpan.Inlines.Add(finalImage);
                                }
                            }
                            return imgSpan;

                        case "LI":
                            var li = new Span();
                            li.Inlines.Add(Traverse(sender, content));
                            li.Inlines.Add(new LineBreak());
                            return li;

                        default:
                            return new Span(Traverse(sender, content));
                    }
                }
                else return new Run(section.HtmlDecode());
            }
            else
            {
                Span span = new Span();
                foreach (string section in sections)
                    span.Inlines.Add(Traverse(sender, section));
                return span;
            }
        }

        private class Token
        {
            public Token()
            {
                Attributes = new NameValueCollection();
            }

            public NameValueCollection Attributes { get; set; }
            public int EndIndex { get { return Index + Length; } }
            public int Index { get; set; }
            public String Inline { get; set; }
            public int Length { get; set; }
            public String Name { get; set; }
            public String Value { get; set; }
        }
    }
}
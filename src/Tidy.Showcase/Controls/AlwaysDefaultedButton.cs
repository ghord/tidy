using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tidy.Showcase.Controls
{
    public class AlwaysDefaultedButton : Button
    {
        private static readonly DependencyPropertyKey IsDefaultedPropertyKey;
        private static readonly object True = true;
        static AlwaysDefaultedButton()
        {

            IsDefaultedPropertyKey = (DependencyPropertyKey)typeof(Button)
                .GetField(nameof(IsDefaultedPropertyKey), BindingFlags.Static | BindingFlags.NonPublic)!
                .GetValue(null)!;

            IsDefaultedPropertyKey.OverrideMetadata(typeof(AlwaysDefaultedButton), new FrameworkPropertyMetadata(True, FrameworkPropertyMetadataOptions.None, null, OnCoerce));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(AlwaysDefaultedButton), new FrameworkPropertyMetadata(typeof(Button)));
        }

        private static object OnCoerce(DependencyObject d, object baseValue)
        {
            return true;
        }
    }
}

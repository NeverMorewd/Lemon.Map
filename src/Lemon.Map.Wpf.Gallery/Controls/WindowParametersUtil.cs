﻿using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

namespace Lemon.Map.Wpf.Gallery.Controls
{
    public static class WindowParametersUtil
    {
        private static Thickness? _paddedBorderThickness;

        private static double? _ribbonContextualTabGroupHeight;
        public enum SM
        {
            /// <summary>
            /// The amount of border padding for captioned windows, in pixels.
            /// Returns the amount of extra border padding around captioned windows
            /// Windows XP/2000:  This value is not supported.
            /// </summary>
            CXPADDEDBORDER = 92,
        }
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(SM nIndex);
        /// <summary>
        /// returns the border thickness padding around captioned windows,in pixels. Windows XP/2000:  This value is not supported.
        /// </summary>
        public static Thickness PaddedBorderThickness
        {
            [SecurityCritical]
            get
            {
                if (_paddedBorderThickness == null)
                {
                    var paddedBorder = GetSystemMetrics(SM.CXPADDEDBORDER);
                    var dpi = GetDpi();
                    Size frameSize = new(paddedBorder, paddedBorder);
                    Size frameSizeInDips = DpiUtil.DeviceSizeToLogical(frameSize, dpi / 96.0, dpi / 96.0);
                    _paddedBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height, frameSizeInDips.Width, frameSizeInDips.Height);
                }

                return _paddedBorderThickness.Value;
            }
        }

        public static double RibbonContextualTabGroupHeight
        {

            get
            {
                _ribbonContextualTabGroupHeight ??= SystemParameters.WindowNonClientFrameThickness.Top + (1d * GetDpi() / 96.0);

                return _ribbonContextualTabGroupHeight.Value;
            }
        }


        /// <summary>
        /// Get Dpi
        /// </summary>
        /// <returns>Return 96,144/returns>
        public static double GetDpi()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException();
            var dpiX = (int)dpiXProperty!.GetValue(null, null)!;
            return dpiX;
        }
    }
}

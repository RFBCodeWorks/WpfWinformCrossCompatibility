using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace System.WPFWinformCompatibility
{
    /// <inheritdoc cref="System.Windows.Forms.IWin32Window.Handle"/>
    /// <see href="https://stackoverflow.com/questions/7822164/winform-dialog-with-wpf-window-as-parent"/>
    /// <remarks>
    /// Supports implicit conversion from both <see cref="Window"/> and <see cref="Form"/>.
    /// </remarks>
    public class Wpf32Window : System.Windows.Forms.IWin32Window, System.Windows.Interop.IWin32Window
    {
        /// <inheritdoc cref="System.Windows.Forms.IWin32Window.Handle"/>
        public IntPtr Handle { get; private set; }

        /// <summary>
        ///  Get the handle from a WPF window
        /// </summary>
        public Wpf32Window(Window wpfWindow)
        {
            Handle = new WindowInteropHelper(wpfWindow).Handle;
        }

        /// <summary>
        ///  Get the handle from a class that implements <see cref="System.Windows.Forms.IWin32Window"/>
        /// </summary>
        public Wpf32Window(System.Windows.Forms.IWin32Window window)
        {
            Handle = window.Handle;
        }

        /// <summary>
        ///  Get the handle from a class that implements <see cref="System.Windows.Interop.IWin32Window"/>
        /// </summary>
        public Wpf32Window(System.Windows.Interop.IWin32Window window)
        {
            Handle = window.Handle;
        }

        /// <inheritdoc cref="Wpf32Window.Wpf32Window(Window)"/>
        public static implicit operator Wpf32Window(Window window) => new Wpf32Window(window);

        /// <inheritdoc cref="Wpf32Window.Wpf32Window(System.Windows.Forms.IWin32Window)"/>
        public static implicit operator Wpf32Window(System.Windows.Forms.Form form) => new Wpf32Window(form);

    }
}

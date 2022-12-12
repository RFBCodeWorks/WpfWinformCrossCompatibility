using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using IWin32Window = System.Windows.Forms.IWin32Window;
using WPFControl = System.Windows.Controls.Control;

namespace System.WPFWinformCompatibility
{
    /// <summary>
    /// Basic interface for a control to Show/Hide it.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Invokes the <paramref name="action"/> against the <paramref name="cntrl"/>. <br/>
        /// Works for both Winforms and WPF.
        /// </summary>
        /// <param name="cntrl"></param>
        /// <param name="action"></param>
        public static void InvokeIfRequired(this IControl cntrl, Action action)
        {
            if (cntrl is System.Windows.Forms.Control WinFrm)
            {
                if (WinFrm.InvokeRequired)
                {
                    WinFrm.Invoke(action);
                }
                else
                    action();
                return;
            }
            else if (cntrl is System.Windows.Controls.Control wpf)
            {
                wpf.Dispatcher.Invoke(action);
            }
        }

        /// <summary>
        /// Show the control
        /// </summary>
        /// <param name="cntrl"></param>
        public static void Show(this IControl cntrl)
        {
            if (cntrl.IsWinform())
                ((Control)cntrl).Show();
            else if (cntrl.IsWPF())
                ((WPFControl)cntrl).Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hide the control
        /// </summary>
        /// <param name="cntrl"></param>
        public static void Hide(this IControl cntrl)
        {
            if (cntrl.IsWinform())
                ((Control)cntrl).Hide();
            else if (cntrl.IsWPF())
                ((WPFControl)cntrl).Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Reference a WPF window as an <see cref="IWin32Window"/> to set it as the parent of a child winform 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        /// <see href="https://stackoverflow.com/questions/7822164/winform-dialog-with-wpf-window-as-parent"/>
        public static IWin32Window GetWin32Window(this Window parent)
        {
            return new Wpf32Window(parent);
        }

        /// <summary>
        /// Evaluate if the provided object is a WPF Control -- <see cref="System.Windows.Controls.Control"/>
        /// </summary>
        /// <param name="cntrl"></param>
        /// <returns></returns>
        public static bool IsWPF(this IControl cntrl) => cntrl is System.Windows.Controls.Control;
        
        /// <summary>
        /// Evaluate if the provided object is a Winforms Control -- <see cref="System.Windows.Forms.Control"/>
        /// </summary>
        /// <param name="cntrl"></param>
        /// <returns></returns>
        public static bool IsWinform(this IControl cntrl) => cntrl is System.Windows.Forms.Control;

    }

}

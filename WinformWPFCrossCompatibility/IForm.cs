using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;  //WPF
using System.Windows.Forms; //Winforms
using System.Windows.Interop;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace System.WPFWinformCompatibility
{
    /// <summary>
    /// Basic interface for all forms
    /// </summary>
    public interface IForm : IDisposable, IWin32Window
    {
        /// <summary>
        /// Determine if the form is visible to the user
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Gets/Sets the title of the userform
        /// </summary>
        string Title { get; set; }

        /// <inheritdoc cref="System.Windows.Forms.Control.Capture"/>
        bool IsMouseCaptured { get; }

        /// <inheritdoc cref="System.Windows.Forms.Control.CanFocus"/>
        bool CanFocus { get; }

        /// <summary>
        /// Displays the form to the user
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the form from the user
        /// </summary>
        void Hide();

        /// <summary>
        /// Closes the splash screen form
        /// </summary>
        void Close();

        /// <summary>
        /// <inheritdoc cref="Form.Activate"/>
        /// </summary>
        void Activate();

        /// <inheritdoc cref="System.Windows.Forms.Control.Invoke(Delegate)"/>
        void Invoke(Delegate method);

        /// <inheritdoc cref="System.Windows.Forms.Control.Invoke(Delegate, object[])"/>
        void Invoke(Delegate method, params object[] args);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace System.WPFWinformCompatibility
{
    /// <summary>
    /// Basic interface for a control to Show/Hide it.
    /// </summary>
    public interface IControl
    {
        /// <summary>
        /// <inheritdoc cref="Control.Show"/>
        /// </summary>
        void Show();

        /// <summary>
        /// <inheritdoc cref="Control.Hide"/>
        /// </summary>
        void Hide();
    }
}

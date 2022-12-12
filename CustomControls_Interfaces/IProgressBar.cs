using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.WPFWinformCompatibility;
using System.Windows.Forms;

namespace CustomControls.Interfaces
{
    /// <summary>
    /// Interface for interacting with a Progress Bar + Label
    /// </summary>
    public interface IProgressBar : IControl
    {
        /// <summary>
        /// Toggle visibility for the progress bar
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Toggle visibility for the display text
        /// </summary>
        bool Label_Visible { get; set; }

        /// <summary>
        /// Display text for the associated progress bar
        /// </summary>
        string Label_Text { get; set; }

        /// <summary>
        /// <inheritdoc cref="ProgressBar.Value"/>
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// <inheritdoc cref="ProgressBar.Minimum"/>
        /// </summary>
        int Minimum { get; set; }

        /// <summary>
        /// <inheritdoc cref="ProgressBar.Maximum"/>
        /// </summary>
        int Maximum { get; set; }
    }
}

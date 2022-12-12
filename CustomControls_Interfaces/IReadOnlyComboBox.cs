using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.WPFWinformCompatibility;

namespace CustomControls.Interfaces
{
    /// <summary>
    /// Interface for interaction with a ReadOnly ComboBox
    /// </summary>
    public interface IReadOnlyComboBox : IControl
    {
        /// <summary>
        /// Get/Set the ReadOnly property of the control
        /// </summary>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// Get/Set the selected value of the combobox
        /// </summary>
        object SelectedValue { get; set; }

        /// <summary>
        /// Get/Set the selected Item of the combobox
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// Get/Set the selected index of the combobox
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Get/Set the selected string value displayed by the combox
        /// </summary>
        string Text { get; }
    }
}

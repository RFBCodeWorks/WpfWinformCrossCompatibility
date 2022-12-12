using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.WPFWinformCompatibility;

namespace CustomControls.Interfaces
{
    /// <summary> Interface to allow binding between custom Number-Only controls and NumericParameter objects </summary>
    public interface INumericTextBox : IControl
    {
        /// <summary> The maximum value of the control to accept. </summary>
        int Maximum { get; set; }
        /// <summary> The minimum value of the control to accept. </summary>
        int Minimum { get; set; }
        /// <summary> Integer Value of the control. </summary>
        int Value { get; set; }
        /// <summary> Set the Default value of the control (used to determine if the value has been modified) </summary>
        int DefaultValue { get; set; }
        /// <summary> Set a custom tooltip for the control (Does not specify the Min/Max values) </summary>
        string CustomToolTip { get; set; }

        /// <summary> Occurs when the <see cref="Value"/> proeprty is updated </summary>
        event ValueChangedEventArgs.ValueChangedEventHandler ValueChanged;

        /// <summary>
        /// ToolTip to show how to display the MinMax<br/>
        /// </summary>
        /// <remarks>
        /// Format IDs:
        /// <br/> - 0 -- <see cref="CustomToolTip"/>
        /// <br/> - 1 -- <see cref="Minimum"/>
        /// <br/> - 2 -- <see cref="Maximum"/>
        /// <br/> - 3 -- <see cref="DefaultValue"/>
        /// </remarks>
        string ToolTipFormat { get; set; }

    }


    /// <summary>
    /// Static class that provides extension methods for the various interfaces provided by the CustomControls_Interfaces dll
    /// </summary>
    public static partial class CustomInterfaceExtensions
    {
        /// <summary>
        /// Set the range and the value of the <see cref="INumericTextBox"/>
        /// </summary>
        /// <param name="numericTextBox"></param>
        /// <param name="minimum"></param>
        /// <param name="maximuim"></param>
        /// <param name="Value"></param>
        public static void SetValues(this INumericTextBox numericTextBox, int minimum, int maximuim, int Value)
        {
            if (minimum > Value) throw new ArgumentException("Minimum value is greater than the provided Value");
            if (minimum > maximuim) throw new ArgumentException("Minimum value is greater than the provided Maximum");
            if (Value > maximuim) throw new ArgumentException("Value is greater than the provided Maximum");
            numericTextBox.InvokeIfRequired(() =>
            {
                numericTextBox.Minimum = minimum;
                numericTextBox.Maximum = maximuim;
                numericTextBox.Value = Value;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControls.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>  </summary>
        public delegate void ValueChangedEventHandler(INumericTextBox obj, ValueChangedEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public ValueChangedEventArgs(int oldValue, int newValue) : base()
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public int OldValue { get; }

        /// <summary>
        /// 
        /// </summary>
        public int NewValue { get; }
    }
}

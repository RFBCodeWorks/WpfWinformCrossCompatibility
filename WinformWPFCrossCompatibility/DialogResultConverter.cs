using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.WPFWinformCompatibility
{
    /// <summary>
    /// Convert between Winform and WPF Dialogue Results Implicitly
    /// </summary>
    public class DialogResultConverter
    {
        private DialogResultConverter(System.Windows.Forms.DialogResult e1, System.Windows.MessageBoxResult e2, int value)
        {
            WinForms = e1;
            Wpf = e2;
            Value = value;
        }

        /// <summary>
        /// The WinForm enum value
        /// </summary>
        public System.Windows.Forms.DialogResult WinForms { get; }

        /// <summary>
        /// The closest WPF Enum value
        /// </summary>
        public System.Windows.MessageBoxResult Wpf { get; }

        /// <summary>
        /// WinForm Enum Value
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Winforms &amp; WPF : None
        /// </summary>
        public static DialogResultConverter None { get; } = new DialogResultConverter(Windows.Forms.DialogResult.None, Windows.MessageBoxResult.None, 0);

        /// <summary>
        /// Winforms &amp; WPF : OK
        /// </summary>
        public static DialogResultConverter OK { get; } = new DialogResultConverter(Windows.Forms.DialogResult.OK, Windows.MessageBoxResult.OK, 1);

        /// <summary>
        /// Winforms &amp; WPF : Cancel
        /// </summary>
        public static DialogResultConverter Cancel { get; } = new DialogResultConverter(Windows.Forms.DialogResult.Cancel, Windows.MessageBoxResult.Cancel, 2);

        /// <summary>
        /// Winforms: Abort <br/> WPF: Cancel
        /// </summary>
        public static DialogResultConverter Abort { get; } = new DialogResultConverter(Windows.Forms.DialogResult.Abort, Windows.MessageBoxResult.Cancel, 3);

        /// <summary>
        /// Winforms: Retry <br/> WPF: OK
        /// </summary>
        public static DialogResultConverter Retry { get; } = new DialogResultConverter(Windows.Forms.DialogResult.Retry, Windows.MessageBoxResult.OK, 4);

        /// <summary>
        /// Winforms: Abort <br/> WPF: Cancel
        /// </summary>
        public static DialogResultConverter Ignore { get; } = new DialogResultConverter(Windows.Forms.DialogResult.Ignore, Windows.MessageBoxResult.No, 5);

        /// <summary>
        /// Winforms &amp; WPF : Yes
        /// </summary>
        public static DialogResultConverter Yes { get; } = new DialogResultConverter(Windows.Forms.DialogResult.Yes, Windows.MessageBoxResult.Yes, 6);

        /// <summary>
        /// Winforms &amp; WPF : No
        /// </summary>
        public static DialogResultConverter No { get; } = new DialogResultConverter(Windows.Forms.DialogResult.No, Windows.MessageBoxResult.No, 7);


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static implicit operator System.Windows.Forms.DialogResult(DialogResultConverter obj) => obj.WinForms;
        public static implicit operator System.Windows.MessageBoxResult(DialogResultConverter obj) => obj.Wpf;

        public static implicit operator DialogResultConverter(System.Windows.Forms.MessageBoxButtons obj) => FindByValue(obj);
        public static implicit operator DialogResultConverter(Windows.MessageBoxButton obj) => FindByValue(obj);

        public static DialogResultConverter FindByValue(System.Windows.Forms.MessageBoxButtons v) => FindByValue((int)v);
        public static DialogResultConverter FindByValue(System.Windows.MessageBoxButton v) => FindByValue((int)v);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        private static DialogResultConverter FindByValue(int value)
        {
            switch (value)
            {
                case 0: return None;
                case 1: return OK;
                case 2: return Cancel;
                //case 3: return Abort;
                //case 4: return Retry;
                //case 5: return Ignore;
                case 6: return Yes;
                case 7: return No;
                default: throw new NotImplementedException("No Implicit Conversion Exists");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.WPFWinformCompatibility
{
    /// <summary>
    /// Convert between Winform and WPF MessageBoxButtons Implicitly
    /// </summary>
    public class MessageBoxButtonConverter
    {
        private MessageBoxButtonConverter(System.Windows.Forms.MessageBoxButtons e1, System.Windows.MessageBoxButton e2, int value)
        {
            WinForms = e1;
            Wpf = e2;
            Value = value;
        }

        /// <summary>
        /// The WinForm enum value
        /// </summary>
        public System.Windows.Forms.MessageBoxButtons WinForms { get; }
        
        /// <summary>
        /// The closest WPF Enum value
        /// </summary>
        public System.Windows.MessageBoxButton Wpf { get; }
        
        /// <summary>
        /// WinForm Enum Value
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Winforms &amp; WPF : OK
        /// </summary>
        public static MessageBoxButtonConverter OK { get; } = new MessageBoxButtonConverter(Windows.Forms.MessageBoxButtons.OK, Windows.MessageBoxButton.OK, 0);

        /// <summary>
        /// Winforms &amp; WPF : OKCancel
        /// </summary>
        public static MessageBoxButtonConverter OKCancel { get; } = new MessageBoxButtonConverter(Windows.Forms.MessageBoxButtons.OKCancel, Windows.MessageBoxButton.OKCancel, 1);

        /// <summary>
        /// Winforms: AbortRetryIgnore <br/> WPF: YesNoCancel
        /// </summary>
        public static MessageBoxButtonConverter AbortRetryIgnore { get; } = new MessageBoxButtonConverter(Windows.Forms.MessageBoxButtons.AbortRetryIgnore, Windows.MessageBoxButton.YesNoCancel, 2);
        
        /// <summary>
        /// Winforms &amp; WPF : YesNoCancel
        /// </summary>
        public static MessageBoxButtonConverter YesNoCancel { get; } = new MessageBoxButtonConverter(Windows.Forms.MessageBoxButtons.YesNoCancel, Windows.MessageBoxButton.YesNoCancel, 3);

        /// <summary>
        /// Winforms &amp; WPF : YesNo
        /// </summary>
        public static MessageBoxButtonConverter YesNo { get; } = new MessageBoxButtonConverter(Windows.Forms.MessageBoxButtons.YesNo, Windows.MessageBoxButton.YesNo, 4);
        
        /// <summary>
        /// Winforms: RetryCancel <br/> WPF: OKCancel
        /// </summary>
        public static MessageBoxButtonConverter RetryCancel { get; } = new MessageBoxButtonConverter(Windows.Forms.MessageBoxButtons.RetryCancel, Windows.MessageBoxButton.OKCancel, 5);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static implicit operator System.Windows.Forms.MessageBoxButtons(MessageBoxButtonConverter obj) => obj.WinForms;
        public static implicit operator System.Windows.MessageBoxButton(MessageBoxButtonConverter obj) => obj.Wpf;

        public static implicit operator MessageBoxButtonConverter(System.Windows.Forms.MessageBoxButtons obj) => FindByValue(obj);
        public static implicit operator MessageBoxButtonConverter(Windows.MessageBoxButton obj) => FindByValue(obj);

        public static MessageBoxButtonConverter FindByValue(System.Windows.Forms.MessageBoxButtons v) => FindByValue((int)v);
        public static MessageBoxButtonConverter FindByValue(System.Windows.MessageBoxButton v) => FindByValue((int)v);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        private static MessageBoxButtonConverter FindByValue(int value)
        {
            switch(value)
            {
                case 0: return OK;
                case 1: return OKCancel;
                //case 2: return AbortRetryIgnore;
                case 3: return YesNoCancel;
                case 4: return YesNo;
                //case 5: return RetryCancel;
                default: throw new NotImplementedException("No implicit Conversion Exists");
            }
        }
    }
}

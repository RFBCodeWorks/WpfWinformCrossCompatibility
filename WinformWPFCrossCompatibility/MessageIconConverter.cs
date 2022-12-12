using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace System.WPFWinformCompatibility
{
    /// <summary>
    /// Convert between Winform and WPF MessageBoxButtons Implicitly
    /// </summary>
    public class MessageIconConverter
    {
        private MessageIconConverter(System.Windows.Forms.MessageBoxIcon e1, System.Windows.MessageBoxImage e2, int value)
        {
            WinForms = e1;
            Wpf = e2;
            Value = value;
        }

        /// <summary>
        /// The WinForm enum value
        /// </summary>
        public System.Windows.Forms.MessageBoxIcon WinForms { get; }

        /// <summary>
        /// The closest WPF Enum value
        /// </summary>
        public System.Windows.MessageBoxImage Wpf { get; }

        /// <summary>
        /// WinForm Enum Value
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Winforms &amp; WPF : None
        /// </summary>
        public static MessageIconConverter None { get; } = new MessageIconConverter(MessageBoxIcon.None, MessageBoxImage.None, 0);

        /// <summary>
        /// Winforms &amp; WPF : Error
        /// </summary>
        public static MessageIconConverter Error { get; } = new MessageIconConverter(MessageBoxIcon.Error, MessageBoxImage.Error, 16);

        /// <summary>
        /// Winforms: AbortRetryIgnore <br/> WPF: Question
        /// </summary>
        public static MessageIconConverter Question { get; } = new MessageIconConverter(MessageBoxIcon.Question, MessageBoxImage.Question, 32);

        /// <summary>
        /// Winforms &amp; WPF : Warning
        /// </summary>
        public static MessageIconConverter Warning { get; } = new MessageIconConverter(MessageBoxIcon.Warning, MessageBoxImage.Warning, 48);

        /// <summary>
        /// Winforms &amp; WPF : Information
        /// </summary>
        public static MessageIconConverter Information { get; } = new MessageIconConverter(MessageBoxIcon.Information, MessageBoxImage.Information, 64);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static implicit operator MessageBoxIcon(MessageIconConverter obj) => obj.WinForms;
        public static implicit operator MessageBoxImage(MessageIconConverter obj) => obj.Wpf;

        public static implicit operator MessageIconConverter(MessageBoxIcon obj) => FindByValue(obj);
        public static implicit operator MessageIconConverter(MessageBoxImage obj) => FindByValue(obj);

        public static MessageIconConverter FindByValue(MessageBoxIcon v) => FindByValue((int)v);
        public static MessageIconConverter FindByValue(MessageBoxImage v) => FindByValue((int)v);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        private static MessageIconConverter FindByValue(int value)
        {
            switch (value)
            {
                case 0: return None;
                case 16: return Error;
                case 32: return Question;
                case 48: return Warning;
                case 64: return Information;
                default: throw new NotImplementedException("No implicit Conversion Exists");
            }
        }
    }
}

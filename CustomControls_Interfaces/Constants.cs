using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomControls
{
    /// <summary>
    /// Defines various constants that can be used by the consumers of the library
    /// </summary>
    public static partial class Constants
    {

        /// <summary>
        /// Regex Validation for an IPV4 string
        /// </summary>
        public static readonly Regex IPV4Regex = new Regex(@"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}", RegexOptions.Compiled);

        /// <summary>
        /// Represents a single group within the IPV4 string
        /// </summary>
        public const string IPV4Group = "###";

        /// <summary>
        /// Format a string as an IP address
        /// </summary>
        public const string IPV4Format = "###.###.###.###";

        /// <summary>
        /// For use when specifying 4 different strings that combine to create an IPV4 address
        /// </summary>
        public const string IPV4Format_Expanded = "{0:###}.{1:###}.{2:###}.{3:###}";

        /// <summary>
        /// The array of all Number Keys
        /// </summary>
        public static Key[] NumberKeys { get; } = new Key[] { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0, Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9 };

        /// <summary>
        /// Contains all Number Keys and the period key
        /// </summary>
        public static Key[] DecimalKeys { get; }

        /// <summary>
        /// Arracy of all alphabet keys
        /// </summary>
        public static Key[] AlphabetKeys { get; } = new Key[] { Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z };

        /// <summary>
        /// Contains the standard punction keys + Tab and Brackets
        /// </summary>
        public static Key[] Punctionation { get; } = new Key[] { Key.OemPeriod, Key.OemSemicolon, Key.OemComma, Key.OemQuestion, Key.OemCloseBrackets, Key.OemOpenBrackets, Key.OemQuotes, Key.OemTilde, Key.Tab };

        /// <summary>
        /// Array of Alphabet and Numeric keys
        /// </summary>
        public static Key[] AlphaNumeric { get; }

        
        static Constants()
        {
            //AlphaNumeric
            List<Key> aN = new List<Key>();
            aN.AddRange(AlphabetKeys); aN.AddRange(NumberKeys);
            AlphaNumeric = aN.ToArray();

            //Decimal Keys
            aN = new List<Key>();
            aN.AddRange(NumberKeys);
            aN.Add(Key.OemPeriod);
            DecimalKeys = aN.ToArray();
        }

        /// <summary>
        /// Determine if either SHIFT key is held down
        /// </summary>
        /// <returns> Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) </returns>
        public static bool IsShiftHeld() => Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

        /// <summary>
        /// Determine if the CTRL key is held down
        /// </summary>
        /// <returns> Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl); </returns>
        public static bool IsCtrlHeld() => Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

        /// <summary>
        /// Determine if the ALT key is held down
        /// </summary>
        /// <returns> Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt); </returns>
        public static bool IsAltHeld() => Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);


        /// <summary>
        /// Determine if the key is one of the following special keys: <br/>
        /// - <see cref="Key.Back"/>
        /// - <see cref="Key.Capital"/>
        /// - <see cref="Key.Clear"/>
        /// - <see cref="Key.CrSel"/>
        /// - <see cref="Key.Delete"/>
        /// - <see cref="Key.End"/>
        /// - <see cref="Key.Enter"/>
        /// - <see cref="Key.Escape"/>
        /// - <see cref="Key.Help"/>
        /// - <see cref="Key.Home"/>
        /// - <see cref="Key.Insert"/>
        /// - <see cref="Key.RWin"/>
        /// - <see cref="Key.LWin"/>
        /// - <see cref="Key.OemBackslash"/>
        /// - <see cref="Key.PageDown"/>
        /// - <see cref="Key.PageUp"/>
        /// - <see cref="Key.Print"/>
        /// - <see cref="Key.PrintScreen"/>
        /// - <see cref="Key.Return"/>
        /// - <see cref="Key.System"/>
        /// </summary>
        /// <returns> True if the key is typically a special function, otherwise false </returns>
        public static bool IsSpecialSystemKey(this Key key)
        {
            switch (key)
            {
                case Key.Back:
                case Key.Capital:
                case Key.Clear:
                case Key.OemClear:
                case Key.CrSel:
                case Key.Delete:
                case Key.End:
                case Key.Enter:
                case Key.Escape:
                case Key.Help:
                case Key.Home:
                case Key.Insert:
                case Key.RWin:
                case Key.LWin:
                case Key.OemBackslash:
                case Key.PageDown:
                case Key.PageUp:
                case Key.Print:
                case Key.PrintScreen:
                case Key.NumLock:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if the pressed key is any of the F# keys
        /// </summary>
        /// <param name="key"></param>
        /// <returns>TRUE if it is a F# key (such as <see cref="Key.F1"/>), otherwise false.</returns>
        public static bool IsFunctionKey(this Key key)
        {
            switch (key)
            {
                case Key.F1:
                case Key.F2:
                case Key.F3:
                case Key.F4:
                case Key.F5:
                case Key.F6:
                case Key.F7:
                case Key.F8:
                case Key.F9:
                case Key.F10:
                case Key.F11:
                case Key.F12:
                case Key.F13:
                case Key.F14:
                case Key.F15:
                case Key.F16:
                case Key.F17:
                case Key.F18:
                case Key.F19:
                case Key.F20:
                case Key.F21:
                case Key.F22:
                case Key.F23:
                case Key.F24:
                    return true;
                default:
                    return false;
            }
        }

    }
}

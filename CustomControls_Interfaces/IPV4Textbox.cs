using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.WPFWinformCompatibility;

namespace CustomControls.Interfaces
{
    /// <summary> Interface to allow binding between custom Number-Only controls and NumericParameter objects </summary>
    public interface IPV4Textbox : IControl
    {
        /// <summary>
        /// Get/Set the IP Address value of the textbox
        /// </summary>
        string Address { get; set; }
    }
    

    /// <summary>
    /// Static class that provides extension methods for the various interfaces provided by the CustomControls_Interfaces dll
    /// </summary>
    public static partial class CustomInterfaceExtensions
    {
        /// <summary>
        /// Set the value of the <see cref="IPV4Textbox"/>
        /// </summary>
        public static void TrySetIP(this IPV4Textbox IPTextBox, string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out var IP))
            {
                IPTextBox.Address = IP.ToString();
            }
            else
            {
                //IPTextBox.Text = string.Empty;
            }
        }
    }
}

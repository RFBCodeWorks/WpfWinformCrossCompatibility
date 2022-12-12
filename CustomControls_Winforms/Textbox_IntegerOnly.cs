using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomControls.Interfaces;

namespace CustomControls.WinForms
{

    

    /// <summary>
    /// Implement the NumericUpDown textbox while hiding the arrows.
    /// </summary>
    //[System.ComponentModel.DesignerCategory("Default")]
    public class TextBoxIntegerOnly : NumericUpDown, INumericTextBox
    {
        //https://stackoverflow.com/questions/29450844/how-to-hide-arrows-on-numericupdown-control-in-win-forms

        /// <summary>
        /// <inheritdoc cref="NumericUpDown_NoButtons"/>
        /// </summary>
        public TextBoxIntegerOnly() : base() 
        {

        }

        public event ValueChangedEventArgs.ValueChangedEventHandler ValueChanged;

        protected override void OnResize(EventArgs e)
        {
            CompensateForHiddenButtons();
            base.OnResize(e);
        }

        new public int Value { get => (int)base.Value; set => base.Value = value; }
        new public int Maximum { get => (int)base.Maximum; set => base.Maximum = value; }
        new public int Minimum { get => (int)base.Minimum; set => base.Minimum = value; }

        private bool ArrowsHiddenField = false;
        public bool ArrowsHidden { 
            get => ArrowsHiddenField; 
            set
            {
                ArrowsHiddenField = value;
                ShowHideButtons();
            } 
        }

        public int DefaultValue { get; set; }
        public string CustomToolTip { get; set; }
        public string ToolTipFormat { get; set; }

        protected void CompensateForHiddenButtons()
        {
            if (Controls[0].Visibility())
            {
                //Visible buttons
                Controls[1].Width = Width - Controls[0].Width;
            }
            else
            {
                //Hidden buttons
                Controls[1].Width = Width - (this.BorderStyle == BorderStyle.None ? -4 : 4);
            }
        }

        private void ShowHideButtons()
        {
            if (ArrowsHidden)
            {
                Controls[0].Hide(); //Hide the buttons
            }
            else
            {
                Controls[0].Show(); //Hide the buttons
            }
            CompensateForHiddenButtons();
        }
    }
}

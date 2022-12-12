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
    /// This is a textbox that intercepts the OnKeyPress event to only accept numbers. This can be configured to accept decimal points as well. 
    /// </summary>
    [System.ComponentModel.DesignerCategory("Default")]
    public class Textbox_NumericOnly : Textbox_BaseValidation, INumericTextBox
    {
        public Textbox_NumericOnly() : base() { }

        #region < Control Value Properties >

        protected int MinValueField = 0;
        protected int MaxValueField = 100;

        /// <summary> The minimum value of the control to accept </summary>
        public virtual int Minimum
        {
            get => MinValueField;
            set
            {
                if (MinValueField != value)
                {
                    MinValueChange?.Invoke(this, new ValueChangedEventArgs((int)MinValueField, value));
                    MinValueField = value;
                    GenerateToolTip();
                }
            }
        }

        /// <summary> The maximum value of the control to accept </summary>
        public virtual int Maximum
        {
            get => MaxValueField;
            set
            {
                if (MaxValueField != value)
                {
                    MaxValueChange?.Invoke(this, new ValueChangedEventArgs((int)MaxValueField, value));
                    MaxValueField = value;
                    GenerateToolTip();
                }
            }
        }

        /// <summary> Since the Value property changes with the textbox, we need to store the old value when the control gets focus.
        /// When the control loses focus, compare this to the textbox value to determine if the ValueChanged event should be invoked. </summary>
        private Decimal OldValue = 0;
        
        /// <summary> Get the Decimal value of the control </summary>
        public virtual decimal Value
        {
            get
            {
                if (this.InputBox.Text.ToCharArray().Any((char c) => (Char.IsDigit(c))))
                    return Convert.ToDecimal(this.InputBox.Text);
                else
                    return 0;
            }
            set 
            {
                if (value != this.Value)
                {
                    var v = this.Value;
                    this.FormatDisplayText(value.ToString()); //Enters the value into the textbox
                    ValueChanged?.Invoke(this, new ValueChangedEventArgs((int)v, (int)value));
                }
                else if(String.IsNullOrWhiteSpace(this.Text))
                {
                    this.FormatDisplayText(this.Value.ToString());
                }
            }
        }


        /// <summary> 
        /// When <paramref name="true"/>, do not autocorrect values to either the Min or Max values whenuser submits a value outside the range. <br/>
        /// When <paramref name="false"/>, if a value above <paramref name="MaximumValue"/> is entered, change it to the max value instead. If below <paramref name="MinimumValue"/>, change it to Minimum Value.
        /// </summary>
        public virtual bool AllowEnteringValuesOutsideMinMax { get; set; } = false;

        #endregion < /Control Value Properties >

        #region < GetValue Methods >

        /// <inheritdoc cref="Single"/>
        public Single GetValueAsSingle() => GetValueAsFloat();
        /// <inheritdoc cref="float"/>
        public float GetValueAsFloat() => (float)Value;
        /// <inheritdoc cref="int32"/>
        public int GetValueAsInt32() => (Int32)Value;
        /// <inheritdoc cref="long"/>
        public long GetValueAsLong() => (long)Value;
        /// <inheritdoc cref="double"/>
        public double GetValueAsDouble() => (double)Value;

        /// <summary> Find the index of the decimal point. </summary>
        /// <param name="input"> String to eval </param>
        /// <returns>
        /// If there is a decimal, returns the index. <br/>
        /// If no decimal and the textbox has a some string (including a negative symbol), the decimal is at the end of the string. <br/>
        /// If there is no string in the textbox, returns -1.
        /// </returns>
        private int IndexOfDecimal(string input) => !this.Text.Contains(DecimalSeperator) ? this.Text.Length : this.Text.IndexOf(DecimalSeperator);

        ///<inheritdoc cref="IndexOfDecimal(string)"/>
        public int IndexOfDecimal() => IndexOfDecimal(this.Text);

        #endregion <  /GetSetValue Methods >

        #region < TextBox Properties >

        /// <returns> The decimal seperator for the display text. </returns>
        protected string DecimalSeperator => System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

        /// <returns> The thousands seperator for the display text. </returns>
        protected string ThousandsSeperator => System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator;

        public int DefaultValue { get; set; }

        /// <summary> Check if the value entered into the textbox is negative </summary>
        public bool IsNegative => this.Text.Length > 0 && this.Text[0] == '-';

        /// <summary> True if the Minimum value is less than 0 </summary>
        /// <returns></returns>
        public virtual bool AllowNegativeValues => Minimum < 0;

        /// <summary> 
        /// Determines how many Digits of Precision to allow (Decimal Precision) <br/>
        /// Default is 0 (integers only).
        /// </summary>
        public virtual int DecimalPlaces 
        {
            get => DecimalPlacesField;
            set
            {
                if (value != DecimalPlacesField && value >=0 )
                    DecimalPlacesField = value;
            }
        } 

        private int DecimalPlacesField = 0;

        /// <summary> Get the text format for how the number should be displayed in the textbox. </summary>
        protected virtual string TextFormat => GetTextFormat();
        private string GetTextFormat()
        {
            string format = "###";
            switch (true) 
            {
                //Prevent showing the thousands symbol
                case true when !InputBox.Focused & this.DisplayThousandSeperator == false: break; //No Focus & Display = false
                case true when InputBox.Focused & this.AllowThousandSeperator == false: break;  //Don't show if user is prevented from inputting them
                // Show the thousands symbol
                case true when 
                !InputBox.Focused & this.DisplayThousandSeperator == true |        //No Focus & Display = true
                !InputBox.Focused & InputBox.Text.Contains(ThousandsSeperator) |   //No Focus & Display = null & a comma exists
                InputBox.Focused & this.AllowThousandSeperator == true:            //Has focus & Thousands Seperator is allowed for user entry
                    format = "###," + format; break; 
            }
            if (AllowDecimalPoint && this.Text.Contains(DecimalSeperator))
            {
                format += ".";
                for (int i = 1; i <= DecimalPlaces; i++)
                    format += "#";
            }
            return format;
        }

        /// <summary> Return the string value of the control </summary>
        new public string Text => this.InputBox.Text;

        /// <summary>
        /// Set this to <paramref name="true"/> if you wish to include a single decimal point in the textbox. If more than 1 decimal point is submitted, throw up an Error Provider symbol until another button is pressed. 
        /// <br/> Default = <paramref name="false"/>
        /// </summary>
        /// <returns><paramref name="true"/> if a decimal point is an accepted input. Otherwise <paramref name="false"/>. </returns>
        public virtual bool AllowDecimalPoint => this.DecimalPlaces > 0;

        /// <summary>
        /// Set to <paramref name="true"/> if you wish to allow the user entering in a thousands seperator into the textbox. <br/>
        /// If <paramref name="true"/>, you can use the GetValueAs methods to return a normalized value. <br/>
        /// Default = <paramref name="false"/>
        /// </summary>
        /// <returns></returns>
        public virtual bool AllowThousandSeperator { get; set; } = true;

        /// <summary>
        /// If <paramref name="true"/>, detect when the control loses focus and add in the commas as the thousands seperators. <br/>
        /// If <paramref name="false"/>, when the control loses focus remove any commas in the textbox. <br/>
        /// If <paramref name="null"/> (default), leave the text as the user entered it.
        /// </summary>
        /// <returns></returns>
        public virtual bool? DisplayThousandSeperator
        {
            get
            {
                if (AllowThousandSeperator == true && DisplayThousandSeperatorField == null)
                    return true;
                else
                    return DisplayThousandSeperatorField;
            }
            set => DisplayThousandSeperatorField = value;
        }

        private bool? DisplayThousandSeperatorField = null;

        /// <summary> If True, add the trailing decimal zeroes until the <paramref name="DecimalPlaces" /> amount is shown. <br/>
        /// ( if true and DecimalPlaces = 3, then the value shown could be 123,456.000 ) <br/>
        /// ( if false, then value shown would be 123,456 )</summary>
        public virtual bool DisplayTrailingZeroes { get; set; } = false;

        #endregion < /TextBox Properties >

        /// <summary> Show the Min / Max values as part of the tooltip. </summary>
        public virtual bool ToolTip_ShowMinMax { get; set; } = true;

        protected override void GenerateToolTip()
        {
            this.ToolTip_SetToolTip(
                (this.ToolTip_CustomCaption != "") ? 
                    ToolTip_CustomCaption + GetMinMaxToolTip(true) 
                    : GetMinMaxToolTip(false)
                );
        }
        protected virtual string GetMinMaxToolTip(bool newline) => (ToolTip_ShowMinMax == false) ? "" : $"{(newline? Environment.NewLine : "" )} Min: {MinValueField}  \\\\ Max: {MaxValueField}";

        #region < ErrorProvider Properties >

        protected override void ErrorProvider_SetRejectKeyError(char c)
        {
            base.ErrorProvider_SetRejectKeyError(c);
            switch (true)
            {
                case true when (!this.AllowDecimalPoint && c == '.'):
                    this.ErrorProvider_SetErrorMessage($"Decimal Points are not permitted.");
                    return;
                case true when (!this.AllowNegativeValues && c == '-'):
                    this.ErrorProvider_SetErrorMessage($"Negative are not permitted.");
                    return;
                default:
                    base.ErrorProvider_SetRejectKeyError(c);
                    return;
            }
        }

        protected override bool PerformValidation()
        {
            if (Value < Minimum || Value > Maximum)
            {
                this.ErrorProvider_SetErrorMessage($"Value is outside the Min/Max Range. {Environment.NewLine} Min: {MinValueField} {Environment.NewLine} Max: {MaxValueField}");
                return false;
            } else
            {
                this.ErrorProvider_ClearError();
                return true;
            }
        }


        #endregion < ErrorProvider Properties >

        #region < Events >

        /// <summary> Occurs when the <see cref="Minimum"/> of the control changes. </summary>
        public event ValueChangedEventArgs.ValueChangedEventHandler MinValueChange;

        /// <summary> Occurs when the <see cref="Maximum"/> of the control changes. </summary>
        public event ValueChangedEventArgs.ValueChangedEventHandler MaxValueChange;

        /// <summary> Occurs when the <see cref="Value"/> of the control changes. </summary>
        public event ValueChangedEventArgs.ValueChangedEventHandler ValueChanged;


        #endregion < / Events >

        #region < KeyPress Evaluation and Reactions >

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            int index = InputBox.SelectionStart;
            bool HasLength = this.Text.Length >= 1;
            int indexOfDecimal = IndexOfDecimal();
            switch (true)
            {
                case true when Char.IsControl(c): return;   //this is a control character -> no reaction to the input.
                case true when Char.IsDigit(c):             //user entered a number -> accept if able
                    switch (true)
                    {
                        case true when (!IsNegative): OnKeyAccepted(e); return; //Accept the input
                        case true when (IsNegative && index > 0): OnKeyAccepted(e); return; //Accept the input -> not to left of negative symbol
                        case true when (IsNegative && index == 0 && this.InputBox.SelectionLength > 0): OnKeyAccepted(e); return; //Accept the input -> not to left of negative symbol
                        default: OnKeyRejected(e); return;  //Reject the input
                    }
                case true when this.AllowNegativeValues && c == '-' && !IsNegative && index == 0: OnKeyAccepted(e); return; //Accept the input, since this is setting the value to negative.
                case true when this.AllowNegativeValues && c == '-' && IsNegative  && index == 0 && InputBox.SelectionLength >=1 : OnKeyAccepted(e); return; //User Overwriting the negative symbol
                case true when c.ToString() == ThousandsSeperator && this.AllowThousandSeperator:    //Process a thousands seperator character
                    switch (true)
                    {
                        case true when index != 0 | index != 1 && IsNegative: OnKeyAccepted(e); return; //Don't allow as first character
                        case true when indexOfDecimal >= 0 && index > indexOfDecimal: OnKeyRejected(e); return;  //Reject the input -> can't have seperator after decimal point
                        case true when indexOfDecimal >= 3 && index == indexOfDecimal - 3: OnKeyAccepted(e); return; //Accept input since this is valid location X,XXX.YYY
                        case true when this.Text.IndexOf(ThousandsSeperator) >= 3 && index == indexOfDecimal - 3: OnKeyAccepted(e); return; //Accept input since this is valid location X,XXX,XXX
                        default: OnKeyRejected(e); return; //Unknown scenario or invalid location: Reject the input.
                    }
                case true when c.ToString() == DecimalSeperator && this.AllowDecimalPoint:
                    switch (true)
                    {
                        case true when !this.Text.Contains(DecimalSeperator): OnKeyAccepted(e); return; //Accept the decimal point, since it is the first one user has entered.
                        case true when this.Text.Contains(DecimalSeperator) && indexOfDecimal >= index && indexOfDecimal <= InputBox.SelectionLength + index: OnKeyAccepted(e); return; //User Overwriting the decimal place
                        default: OnKeyRejected(e); return; //Unknown scenario or invalid location: Reject the input.
                    }
                default: OnKeyRejected(e); return; //Unknown scenario, invalid character: Reject the input.
            }
        }

        ///<summary>Occurs when the user Enters the InputBox control </summary>
        protected override void OnEnter(EventArgs e)
        {
            if (this.IsDesignMode()) return;
            this.OldValue = Value;
            FormatDisplayText(InputBox.Text);
            base.OnEnter(e);
        }

        /// <summary>
        /// Occurs when the user Leaves the InputBox control <br/>
        /// Compares the OldValue to the NewValue, and raises the ValueChanged event if needed.
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            if (this.IsDesignMode()) return;
            FormatDisplayText(InputBox.Text);
            if (OldValue != Value)
                ValueChanged?.Invoke(this, new ValueChangedEventArgs((int)OldValue, (int)Value));
            OldValue = Value;
            base.OnLeave(e);
        }

        /// <summary> Update the textbox with a properly formatted string </summary>
        private void FormatDisplayText(string ValueToDisplay)
        {
            if (this.IsDesignMode()) return;
            if (PerformingTexboxChange) return;
            PerformingTexboxChange = true;
            int cursorIndex = InputBox.SelectionStart;
            string txt = (ValueToDisplay.IsNotEmpty() && ValueToDisplay.IsNumeric()) ? ValueToDisplay : "0";
            bool hasNeg = txt[0] == '-';
            bool hasDec = txt.Contains(DecimalSeperator);
            Decimal value = Convert.ToDecimal(ValueToDisplay);
            switch (true)
            {
                case true when !AllowEnteringValuesOutsideMinMax && value > this.Maximum:
                    txt = this.Maximum.ToString(TextFormat);
                    cursorIndex = txt.Length - 1;
                    break;
                case true when !AllowEnteringValuesOutsideMinMax && value< this.Minimum:
                    txt = this.Minimum.ToString(TextFormat);
                    cursorIndex = txt.Length - 1;
                    break;
                case true when this.Value == value:
                    break;
                default:
                    txt = value.ToString(TextFormat);
                    break;
            }
            if (InputBox.Focused)
            {
                if (hasNeg & txt[0] != '-') txt = '-' + txt;    //Re-Add the negative symbol if it was removed
                if (hasDec & !txt.Contains(DecimalSeperator)) txt += DecimalSeperator; //Re-Add the decimal symbol if it was removed
                if (InputBox.Text.Contains(DecimalSeperator))
                { //Add any missing trailing digits
                    string InputTrailingDigits = InputBox.Text.Substring(InputBox.Text.IndexOf(DecimalSeperator) +1);
                    string txtTrailingDigits = txt.Substring(InputBox.Text.IndexOf(DecimalSeperator));
                    txt += InputTrailingDigits.Substring(txtTrailingDigits.Length);
                }
            }
            if (txt.IsNullOrEmpty()) txt = "0"; //If the value is 0, ensure thatthe 0 exists in the string
            if (this.AllowDecimalPoint & this.DisplayTrailingZeroes)    //Add trailing zeroes
            {
                if (!txt.Contains(DecimalSeperator)) txt += DecimalSeperator;
                int indexOfDecimal = txt.IndexOf(DecimalSeperator);
                while ((txt.Length - indexOfDecimal) < DecimalPlaces + 1)
                    txt += '0';
            }
            if (!InputBox.Focused) txt = Math.Round(Convert.ToDecimal(txt), DecimalPlaces).ToString(TextFormat); //If doesn't have focus, round to the nearest allowed decimal
            if (txt.Length >= 2 && txt.Left(2) == "-.") txt = txt.Insert(1, "0");  //Ensure that the negative symbol always has a digit between it and the decimal 
            InputBox.Text = txt.IsNullOrEmpty() ? "0" : txt; //Insert the resulting string to the textbox
            if (InputBox.Focused && cursorIndex <= InputBox.TextLength -1) this.InputBox.SelectionStart = cursorIndex; //Restore the cursor position if has focus
            PerformingTexboxChange = false;
        }

        protected override void OnTextBoxChanged_WithFocus(EventArgs e)
        {
            if (this.IsDesignMode()) return;
            FormatDisplayText(this.InputBox.Text);
            //base.OnTextBoxChanged_WithFocus(e);
        }

        #endregion < KeyPress Evaluation and Reactions >

        #region < BindingInterface >

        int INumericTextBox.Value { get => (int)this.Value; set => this.Value = value; }
        public string CustomToolTip { get; set; }
        public string ToolTipFormat { get; set; }

        #endregion

    }
}

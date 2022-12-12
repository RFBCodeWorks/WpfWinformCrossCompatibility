using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace CustomControls.WinForms
{
    /// <summary>
    /// This is a base form of a custom user control that implements a TextBox, a tooltip, and an ErrorProvider.<br/>
    /// This class is not meant to be used as a control on a form, the derived classes are what should be used. (Cannot abstract a control) <br/>
    /// The idea is to derive some other controls out of this base, and implement custom validation within those derived controls. 
    /// <para/> TextBox Properties and Events: <br/>
    ///  - Instead of overriding OnValidating to Validate the textbox, <see cref="PerformValidation"/> should be overriddden. <br/>
    ///  ---> When Validation occurred can be customized according to the new <see cref="AutoValidate"/> property for this control. <br/>
    ///  - OnKeyPress (Occurs via <see cref="InputBox"/>.KeyPress event) => Raise this control's KeyPress event. <br/> 
    ///  ---> If e.Handled is false, the <see cref="OnKeyPress(KeyPressEventArgs)"/> method fires.  <br/>
    ///  ---> The base (overridable) OnKeyPress method accepts all keys. <br/>
    ///  ------> Overrides should determine if keys are rejected, then trigger either <paramref name="OnKeyAccepted"/> or <paramref name="OnKeyRejected"/> methods to raise the respective events. <br/>
    ///  - This.Text => Default allows getting and setting directly with the Textbox text. This may want to be overriden or hidden if not using as a string-based control. (Such as a numeric control)     
    /// <para/> ToolTip: --->
    /// If adding additional info into the tooltip, the <see cref="GenerateToolTip"/> method should also be overridden.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Default")]
    public class Textbox_BaseValidation : UserControl
    {
        public Textbox_BaseValidation() : base()
        {
            InitializeComponent();
            ErrorProvider.SetError(this, "");               //Base error has no value.
            InputBox.KeyDown += InputBox_KeyDown;           //React to the KeyDown event
            InputBox.KeyPress += InputBox_OnKeyPress;           //Eval the key press event
            InputBox.TextChanged += InputBox_OnTextBoxChanged;  //Eval when the textbox is updated
        }

        /// <summary>
        /// Use with the Design Window -> If the DesignerCategory [] is disabled, then this will adjust itself automatically.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(0, 0);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(100, 20);
            this.InputBox.TabIndex = 0;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // Textbox_NumericOnly
            // 
            this.Controls.Add(InputBox);
            this.Name = "Textbox_NumericOnly";
            this.Size = new System.Drawing.Size(75, 20);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
        }

        protected TextBox InputBox;
        private System.ComponentModel.IContainer components;
        private ErrorProvider ErrorProvider;
        private ToolTip ToolTip;

        /// <summary> 
        /// Get => Return the string value of the textbox text.<br/>
        /// Set => Set the value of the textbox.
        /// </summary>
        public override string Text
        {
            get => this.InputBox.Text;
            set
            {
                if (this.InputBox.Text != value)
                    this.InputBox.Text = value;
            }
        }

        /// <summary> True if the textbox entry is is determined to be valid. Set after validation occurs or ValidateInput() method is called.  </summary>
        public virtual bool IsValid { get; private set; } = false;

        /// <inheritdoc cref="TextBox.ReadOnly"/>
        public bool ReadOnly
        {
            get => InputBox.ReadOnly;
            set => InputBox.ReadOnly = value;
        }

        /// <inheritdoc cref="TextBox.TextAlign"/>
        public HorizontalAlignment TextAlign
        {
            get => InputBox.TextAlign;
            set => InputBox.TextAlign = value;
        }

        public enum WhenToValidate {
            /// <summary> Validation is not automatically performed. </summary>
            None,
            /// <summary> Validate the textbox only when focus is lost. (During the OnValidating event )</summary>
            OnLeave,
            /// <summary> Validate every time the textbox is changed.</summary>
            OnTextBoxChanged,
            /// <summary> Validate when the texbox is changed and when focus is lost (During the OnValidating event)</summary>
            OnLeaveAndTextBoxChanged,
        }

        /// <summary> If TRUE: Run the Validation method against the textbox when focus is lost. </summary>
        new public virtual WhenToValidate AutoValidate { get; set; } = WhenToValidate.OnTextBoxChanged;

        /// <summary> 
        /// Raise the <see cref="Control.Validating"/> event. <br/>
        /// Then, if <see cref="AutoValidate"/> is set to occur AND <see cref="CancelEventArgs.Cancel"/> == false : <br/>
        /// Run the overridable <see cref="PerformValidation"/> method to evaluate the text in the textbox, and return a result. <br/>
        /// Store the result in the <paramref name="IsValid"/> property. <br/>
        /// PerformValidation() should be overridden to store the custom validation logic.
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (this.AutoValidate == WhenToValidate.None | this.AutoValidate == WhenToValidate.OnTextBoxChanged | e.Cancel) return;
            IsValid = PerformValidation();
        }

        /// <summary> 
        /// Perform the Validation method against the textbox and store the result into the <paramref name="IsValid"/> property. <br/>
        /// Raises the OnValidating event, stores the validation result, Raises the OnValidated event, then returns the result.
        /// </summary>
        /// <returns>The <paramref name="IsValid"/> property will be set to the result of the protected virtual PerformValidation method. The value of the <paramref name="IsValid"/> property will then be returned to the caller. </returns>
        public bool ValidateNow() 
        {
            base.OnValidating(new CancelEventArgs());
            IsValid = PerformValidation();
            base.OnValidated(new EventArgs());
            return IsValid; 
        }

        ///<summary> This method is where the validation logic should be housed. This is called by the public ValidateInput method to store the result in the IsValid property. </summary>
        ///<returns> 
        ///Base method returns true, then exits. <br/>
        ///Overridden method should evaluate the input, and set the ErrorProvider accordingly using <see cref="ErrorProvider_SetErrorMessage(string,bool)"/>
        ///</returns>
        protected virtual bool PerformValidation() { return true; }

        #region < Tooltip Properties >

        /// <summary> Enable or Disable the tooltip for mouse hovering </summary>
        [Category("Misc_ToolTip")]
        public virtual bool ToolTip_Enabled { get; set; } = true;

        /// <summary> Custom text for the tooltip to display. <br/> Setting this value runs the protected GenerateToolTip method. </summary>
        [Category("Misc_ToolTip")]
        [Description("Custom text for the tooltip to display. <br/> Setting this value runs the protected GenerateToolTip method.")]
        public virtual string ToolTip_CustomCaption
        {
            get => CustomToolTipCaption;
            set
            {
                if (value != CustomToolTipCaption)
                {
                    CustomToolTipCaption = value ?? "";
                    GenerateToolTip();
                }
            }
        }
        private string CustomToolTipCaption = "";

        /// <summary> 
        /// Overridable method to generate the tooltip string. <br/>
        /// When overridden, this should call <see cref="ToolTip_SetToolTip"/> instead of Base.GenerateToolTip.
        /// </summary>
        protected virtual void GenerateToolTip() => ToolTip_SetToolTip(CustomToolTipCaption);

        /// <summary> Use this method to set the ToolTip for this control. Called at the end of GenerateToolTip method. </summary>
        /// <returns> If the tooltip text is changed, raise the ToolTipChanged event.</returns>
        protected void ToolTip_SetToolTip(string ToolTipMessage)
        {
            string txt = this.ToolTip.GetToolTip(InputBox);
            if (ToolTip_Enabled)
                this.ToolTip.SetToolTip(InputBox, CustomToolTipCaption ?? "");
            else
                this.ToolTip.RemoveAll();
            //Raise Event
            if (txt != ToolTip.GetToolTip(InputBox))
                ToolTipChanged?.Invoke(this, new EventArgs());
        }

        #endregion < /Tooltip Properties >

        #region < ErrorProvider Properties >

        /// <summary> True if the currently shown error is the RejectKey error. </summary>
        protected bool RejectKeyErrorShown = false;

        /// <summary> Display an ErrorProvider icon with a tooltip if the user performs certain actions.</summary>
        [Category("Misc_ErrorProvider")]
        public virtual bool ErrorProvider_Enabled { get; set; } = false;

        ///  <summary> 
        ///  The textbox evaluates the entered character on every KeyPress event to determine whether or not to accept the character.
        ///  This will determine if the ErrorProvider will display an error if the user types in a rejected key.
        ///  </summary>
        [Category("Misc_ErrorProvider")]
        public virtual bool ErrorProvider_ShowOnRejectedKeys { get; set; } = false;

        /// <inheritdoc cref="ErrorProvider.BlinkRate"/>
        [Category("Misc_ErrorProvider")]
        public virtual int ErrorProvider_BlinkRate { 
            get => ErrorProvider.BlinkRate; 
            set => ErrorProvider.BlinkRate = value; 
        }

        /// <inheritdoc cref="ErrorProvider.BlinkStyle"/>
        [Category("Misc_ErrorProvider")]
        public virtual ErrorBlinkStyle ErrorProvider_BlinkStyle { 
            get => ErrorProvider.BlinkStyle; 
            set => ErrorProvider.BlinkStyle = value; 
        }

        ///  <summary> Gets \ <inheritdoc cref="ErrorProvider.SetIconAlignment(Control, ErrorIconAlignment)"/> </summary>
        [Category("Misc_ErrorProvider")]
        public virtual ErrorIconAlignment ErrorProvider_IconAlignment
        {
            get => ErrorProvider.GetIconAlignment(this);
            set => ErrorProvider.SetIconAlignment(this, value);
        }

        ///  <summary> Gets \ <inheritdoc cref="ErrorProvider.SetIconPadding(Control, int)"/> </summary>
        [Category("Misc_ErrorProvider")]
        public virtual int ErrorProvider_IconPadding { 
            get => ErrorProvider.GetIconPadding(this);
            set => ErrorProvider.SetIconPadding(this, value); 
        }

        /// <summary>
        /// Setup according to this control's <paramref name="ErrorProvider_*"/> properties. Does not reset displayed error messages.
        /// </summary>
        private void SetupErrorProvider() 
        {
            ErrorProvider.ContainerControl = (this.Parent is ContainerControl) ? (ContainerControl)this.Parent : this.ParentForm ;
            ErrorProvider.SetIconAlignment(this, ErrorProvider_IconAlignment);
            ErrorProvider.SetIconPadding(this, ErrorProvider_IconPadding);
        }

        ///<returns>The error description for this control. If no error is present, return String.Empty.</returns>
        /// <inheritdoc cref="ErrorProvider.GetError(Control)"/>
        public string ErrorProvider_GetError() => ErrorProvider.GetError(this) ?? String.Empty;

        /// <summary> Set the error message for this control using the supplied <paramref name="Message"/>. </summary>
        /// <param name="Message">Error Text to display</param>
        /// <param name="IsRejectKeyError"> Set this to TRUE if this message was sent due to a RejectKey </param>
        protected void ErrorProvider_SetErrorMessage(string Message, bool IsRejectKeyError = false)
        {
            ErrorProvider.SetError(this, Message);
            this.RejectKeyErrorShown = IsRejectKeyError && !Message.IsNullOrEmpty();
        }

        /// <summary>
        /// This method is called by the <see cref="OnRejectKey"/> method if <paramref name="ErrorProvider_ShowOnRejectedKeys"/> is set to true. <br/>
        /// Default functionality is to display the ErrorProvider with message of: <br/>
        /// "Character '<paramref name="c"/>' is not allowed."
        /// <para/>
        /// If other functionality or messages are desired, override this method.
        /// </summary>
        /// <param name="c"></param>
        protected virtual void ErrorProvider_SetRejectKeyError(char c) 
        {
            ErrorProvider.SetError(this, $"Character '{c}' is not allowed.");
            RejectKeyErrorShown = true;
        }

        /// <summary> Clear the error associated with this control. </summary>
        protected void ErrorProvider_ClearError()
        {
            ErrorProvider.SetError(this, "");
            RejectKeyErrorShown = false;
        }

        #endregion < ErrorProvider Properties >

        #region < Events >

            /// <summary> Occurs after the underlying textbox's text is updated by the user. </summary>
        public event EventHandler TextBoxChanged;

        /// <summary> Occurs when the <paramref name="ToolTip"/> of the control changes. </summary>
        public event EventHandler ToolTipChanged;

        /// <summary>
        /// <inheritdoc cref="Control.KeyPress"/> <br/>
        /// Occurs prior to running the control's KeyPress event, which should evaluate the key 
        /// and determine to run either KeyAccepted or KeyRejected events. <br/>
        /// If e.<paramref name="Handled"/> is is true after invoking this event, then the textbox will not evaluate to accept/reject the key. (voiding the input)
        ///</summary>
        new public event KeyPressEventHandler KeyPress;

        /// <summary> 
        /// This event fires when a key input is accepted. Does not trigger on Control keys. <br/>
        /// This event will supply the base <see cref = "KeyPressEventArgs" /> object, so setting <seealso cref = "KeyPressEventArgs.Handled"/> to true will cause it to avoid writing the value into the textbox. <br/>
        /// </summary>
        public event KeyPressEventHandler KeyAccepted;

        /// <summary> 
        /// This event fires when a key input is rejected.  Does not trigger on Control keys. <br/>
        /// This event will supply a new <see cref="KeyPressEventArgs"/> object, while the initial one is handled to avoid submitting into the textbox.<br/>
        /// </summary>
        public event KeyPressEventHandler KeyRejected;

        #endregion < / Events >

        #region < Control Focus Events >

        /// <summary> <inheritdoc cref="Control.OnParentChanged(EventArgs)"/> Then call the SetupErrorProvider method. </summary>
        protected override void OnParentChanged(EventArgs e) { base.OnParentChanged(e); SetupErrorProvider(); }

        /// <summary> Call the SetupErrorProvider method, then raises the <see cref="UserControl.OnLoad"/> event. </summary>
        protected override void OnLoad(EventArgs e) { SetupErrorProvider(); base.OnLoad(e); }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.InputBox.Location = new System.Drawing.Point(0, 0);
            this.InputBox.Size = this.Size;
            this.Refresh();
        }

        ///<summary>Occurs when the user Enters the InputBox control </summary>
        protected override void OnEnter(EventArgs e) { base.OnEnter(e); }

        /// <summary>
        /// Occurs when the user Leaves the InputBox control <br/>
        /// Runs the SetErrorProvider routine.
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            if (RejectKeyErrorShown) this.ErrorProvider_ClearError();
            base.OnLeave(e);
        }

        #endregion </ Control Focus Events >

        #region < TextBox Changed Event >

        /// <summary> 
        /// This is set TRUE prior to running the OnTextBoxChanged_WithFocus \ *WithoutFocus events. <br/>
        /// After the Events have completed, this will get set back to FALSE. <br/>
        /// This is meant to avoid triggering the events repeatedly when reacting to the textbox changed event. (such as during validation).
        /// </summary>
        protected bool PerformingTexboxChange = false;

        /// <summary> 
        /// React to when the user changes the textbox. <br/>
        /// </summary>
        private void InputBox_OnTextBoxChanged(object obj, EventArgs e)
        {
            if (PerformingTexboxChange) return;
            try {
                PerformingTexboxChange = true;
                if (this.AutoValidate == WhenToValidate.OnTextBoxChanged | AutoValidate == WhenToValidate.OnLeaveAndTextBoxChanged)
                {
                    var cancelValidation = new CancelEventArgs();
                    this.OnValidating(cancelValidation);
                    if (cancelValidation.Cancel) return;

                    if (InputBox.Focused)
                        OnTextBoxChanged_WithFocus(e);
                    else
                        OnTextBoxChanged_WithoutFocus(e);
                }
            } finally {
                PerformingTexboxChange = false;
            }
            return;
        }

        /// <summary> This method fires when the textbox value changes when InputBox.Focused = false <br/>
        /// Base method only fires the <paramref name="TextBoxChanged"/> event for this control. <br/>
        /// This is meant to be overridden if other functionality is required. </summary>
        /// <param name="e"> EventArgs passed in from the InputBox.TextBoxChanged event </param>
        protected virtual void OnTextBoxChanged_WithoutFocus(EventArgs e) { this.TextBoxChanged?.Invoke(this, e); }

        /// <summary> This method fires when the textbox value changes when InputBox.Focused = true <br/>
        /// Base method only fires the <paramref name="TextBoxChanged"/> event for this control. <br/>
        /// This is meant to be overridden if other functionality is required. </summary>
        /// <param name="e"> EventArgs passed in from the InputBox.TextBoxChanged event </param>
        protected virtual void OnTextBoxChanged_WithFocus(EventArgs e) { this.TextBoxChanged?.Invoke(this, e); }

        #endregion </ TextBox Changed Event >

        #region < KeyPress Events >

        /// <summary> 
        /// Raise the <paramref name="KeyPress"/> event for this control in response to the textbox KeyPress event. <br/>
        /// </summary>
        private void InputBox_OnKeyPress(object obj, KeyPressEventArgs e)
        {
            this.KeyPress?.Invoke(this, e);
            if (!e.Handled)
                this.OnKeyPress(e);
        }

        /// <summary> 
        /// Raise the <paramref name="KeyDown"/> event for this control in response to the textbox KeyDown event. <br/>
        /// If the event is not handled, call the overridable <see cref="OnKeyDown(KeyEventArgs)"/> method.
        /// </summary>
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        /// <summary> 
        /// Overridable method that triggers when the user presses a key while the InputBox has focus. <br/>
        /// <inheritdoc cref="Control.OnKeyDown(KeyEventArgs)"/>
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled) base.OnKeyDown(e);
        }

        /// <summary> 
        /// Evaluate the input character and process it. Reject disallowed characters. <br/>
        /// If not overridden, accept all characters (fire KeyAccepted event). <br/>
        /// Note: The OnKeyAccepted base method runs the UserControl.OnKeyPress() method that will pass the event to the textbox control.
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            OnKeyAccepted(e);
        }

        /// <summary> 
        /// Raise the <paramref name="KeyAccepted"/> event. <br/>
        /// If e.<paramref name="Handled"/> is false, pass the KeyPress event  to the underlying textbox to accept the character entry. via (base)UserControl.OnKeyPress(e);
        /// </summary>
        protected virtual void OnKeyAccepted(KeyPressEventArgs e)
        {
            if (RejectKeyErrorShown) this.ErrorProvider_ClearError();
            this.KeyAccepted?.Invoke(this, e);
            if (!e.Handled) base.OnKeyPress(e);
        }

        /// <summary> 
        /// Raise the <paramref name="KeyRejected"/> event. A new <see cref="KeyPressEventArgs"/> object is created to pass to the reacting routine. <br/>
        /// If <paramref name="ErrorProvider_ShowOnRejectedKeys"/> is TRUE and no other error currently is displayed, run the <see cref="ErrorProvider_SetRejectKeyError(char)"/> method.
        /// Finally, set e.<paramref name="Handled"/> to TRUE to avoid writing into the textbox.
        /// </summary>
        protected virtual void OnKeyRejected(KeyPressEventArgs e)
        {
            KeyRejected?.Invoke(this, new KeyPressEventArgs(e.KeyChar));
            if (ErrorProvider_ShowOnRejectedKeys && ErrorProvider_GetError() == String.Empty) this.ErrorProvider_SetRejectKeyError(e.KeyChar);
            e.Handled = true;
        }
        
        #endregion </ KeyPress Events >

    }
}

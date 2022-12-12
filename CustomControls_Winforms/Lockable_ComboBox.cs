using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomControls.Interfaces;

namespace CustomControls.WinForms
{

    ///<summary> Create a combobox that has the 'ReadOnly' / 'Locked' functionality that VBA provided that for some reason C# does not. 
    ///<para> ReadOnly and IsLocked provide identical functionality. Both provided for ease of use. (ReadOnly points to IsLocked) </para>
    ///<see href="https://www.codeproject.com/Articles/9952/ComboBox-with-read-only-behavior"/></summary>
    [System.ComponentModel.DesignerCategory("Default")]
    public sealed class Lockable_ComboBox : ComboBox, IReadOnlyComboBox
    {
        public Lockable_ComboBox() : base()
        {
            _textbox = new TextBox();
            _locked = false;
            _visible = true;
        }

        private bool _locked;
        private bool _visible;
        private TextBox _textbox;

        ///<summary> When true: Display a Read-Only Textbox. When False, display a combo-box. </summary>
        public bool ReadOnly { get => IsLocked; set => IsLocked = value; }

        ///<summary> Prevent use making changes by displaying a Read-Only textbox instead of the combobox. </summary>
        public void Lock() => IsLocked = true;

        ///<summary> Allow user to use the Drop-Down menu. </summary>
        public void UnLock() => IsLocked = false;

        ///<summary> When true: Display a Read-Only Textbox. When False, display a combo-box. </summary>
        public bool IsLocked
        {
            get { return _locked; }
            set
            {
                if (value != _locked)
                {
                    _locked = value;
                    ShowControl();
                }
            }
        }

        ///<summary> Toggle the visibilty of the control </summary>
        public new bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                ShowControl();
            }
        }

        bool IReadOnlyComboBox.IsReadOnly { get => IsLocked; set => IsLocked = value; }

        ///<summary> Show the control </summary>
        public new void Show() => this.Visible = true;
        ///<summary> Hide the control </summary>
        public new void Hide() => this.Visible = false;

        ///<summary> Decide if showing the textbox or the combobox or neither </summary>
        private void ShowControl()
        {
            if (_locked)
            {
                _textbox.Visible = _visible && this.Enabled;
                base.Visible = _visible && !this.Enabled;
                _textbox.Text = this.Text;
            }
            else
            {
                _textbox.Visible = false;
                base.Visible = _visible;
            }
        }

        ///<summary>Create the textbox in place of the combobox </summary>
        private void AddTextbox()
        {
            _textbox.ReadOnly = true;
            _textbox.Location = this.Location;
            _textbox.Size = this.Size;
            _textbox.Dock = this.Dock;
            _textbox.Anchor = this.Anchor;
            _textbox.Enabled = this.Enabled;
            _textbox.Visible = this.Visible;
            _textbox.RightToLeft = this.RightToLeft;
            _textbox.Font = this.Font;
            _textbox.Text = this.Text;
            _textbox.TabStop = this.TabStop;
            _textbox.TabIndex = this.TabIndex;
        }

        ///<summary> If this control moves, move the textbox too </summary>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent != null)
                AddTextbox();
            _textbox.Parent = this.Parent;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (this.SelectedItem == null)
                _textbox.Clear();
            else
                _textbox.Text = this.SelectedItem.ToString();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            ShowControl();
        }

        protected override void OnDropDownStyleChanged(EventArgs e)
        {
            base.OnDropDownStyleChanged(e);
            _textbox.Text = this.Text;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _textbox.Size = this.Size;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            _textbox.Location = this.Location;
        }


    }

}

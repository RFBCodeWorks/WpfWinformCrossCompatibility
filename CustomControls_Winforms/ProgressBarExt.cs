using System;
using System.Drawing;
using System.Windows.Forms;
using CustomControls.Interfaces;
using System.Windows.Forms.Design;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControls.WinForms
{
    ///<summary> Progress Bar class that has an event when the value changes. This also introduces a label that corresponds to this progressbar.  </summary>
    [System.ComponentModel.DesignerCategory("Default")]
    public sealed class ProgressBarExt : ProgressBar, IProgressBar
    {
        public ProgressBarExt() : base() 
        {
            _label = new Label { Text = "", Visible = true };
            Initialize(ProgressDisplayLocation.Manual, LabelDisplayLocation.Manual);
        }

        /// <summary> </summary>
        /// <param name="progressDisplayLocation"></param>
        public ProgressBarExt(ProgressDisplayLocation progressDisplayLocation) : base()
        {
            _label = new Label { Text = "", Visible = true };
            Initialize(progressDisplayLocation, LabelDisplayLocation.CenterTop);
        }

        /// <summary> </summary>
        /// <param name="labelDisplayLocation"></param>
        /// <param name="progressDisplayLocation"></param>
        public ProgressBarExt(ProgressDisplayLocation progressDisplayLocation, LabelDisplayLocation labelDisplayLocation = LabelDisplayLocation.CenterTop) : base()
        {
            _label = new Label { Text = "", Visible = true };
            Initialize(progressDisplayLocation, labelDisplayLocation);
        }

        private void Initialize(ProgressDisplayLocation progressDisplayLocation, LabelDisplayLocation labelDisplayLocation)
        {
            this.Label_AutoLocation = labelDisplayLocation;
            this.AutoLocation = progressDisplayLocation;
            this.AutoLocateProgressBar();
            _label.SizeChanged += _label_SizeChanged;
        }

        //Custom Event Setup
        public delegate void VoidEvent();

        /// <summary> This event fires whenever the Progress Bar's value is updated. </summary>
        public event VoidEvent OnProgressBarValueChange;


        private Label _label;
        private bool _labelVisible = false;
        private void UpdateLabelVisibility() => _label.Visible = _labelVisible && this.Visible && _label.Text != "";
        private void _label_SizeChanged(object sender, EventArgs e) => AutoLocateLabel();

        public bool Label_Visible 
        { 
            get => _label.Visible;
            set 
            {
                _labelVisible = value;
                UpdateLabelVisibility();
            } 
        }
        public System.Drawing.Size Label_Size
        {
            get => _label.Size;
            set => _label.Size = value;
        }
        public bool Label_AutoSize
        {
            get => _label.AutoSize;
            set => _label.AutoSize = value;
        }

        public string  Label_Text
        {
            get => _label.Text;
            set => _label.Text = value;
        }

        public System.Drawing.Point Label_Location
        {
            get => _label.Location;
            set => _label.Location = value;
        }

        public enum LabelDisplayLocation 
        {
            ///<summary> Manually Adjust the Label Location </summary>
            Manual,
            ///<summary>Center label up between left of form and start of label</summary>
            Left,
            ///<summary>Center label above the progress bar </summary>
            CenterTop,
            ///<summary>Put the label immediately to the right of the progress bar </summary>
            Right
        }
        public LabelDisplayLocation Label_AutoLocation { get; set; } = LabelDisplayLocation.CenterTop;
        
        private void AutoLocateLabel()
        {
            if (!this.Visible | !this.Label_Visible | this.DesignMode) return;
            _label.BringToFront();
            if (this.Label_AutoLocation == LabelDisplayLocation.Manual) return;
            if (this.Label_AutoLocation == LabelDisplayLocation.CenterTop)
            {
                _label.CenterControl_Width(this);
                _label.Top = this.Top - _label.Height - 3;
            }
            else
            {
                _label.CenterControl_Height(this);
                _label.AutoSize = true;
                if (this.Label_AutoLocation == LabelDisplayLocation.Left)
                    _label.Left = this.Left / 2 - _label.Width / 2;
                if (this.Label_AutoLocation == LabelDisplayLocation.Right)
                    _label.Left = this.Left + this.Width + 5;
            }
        }


        ///<inheritdoc cref="System.Windows.Forms.ProgressBar.Value" path="*"/>
        new public int Value 
        { 
            get => base.Value;
            set 
            { 
                if (value != base.Value &&  0 <= value && value <= 100 )
                {
                    base.Value = value;
                    OnProgressBarValueChange?.Invoke();
                }
            }
        }
        
        ///<inheritdoc cref="System.Windows.Forms.Control.Visible" path="*"/>
        new public bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;
                UpdateLabelVisibility();
            }
        }


        public enum ProgressDisplayLocation
        {
            ///<summary> Manually Adjust the Label Location </summary>
            Manual,
            ///<summary> Label moves to bottom of the form. Left and Width not adjusted. </summary>
            Bottom,
            ///<summary> Label aligns to bottom of form. Adjust size to the width of the parent control / form. </summary>
            Bottom_FullWidth,
            ///<summary> Center the progress bar up in the form. If Label.Visible = TRUE: Label will be center point instead and Label.AutoLocate will use the CenterTop position</summary>
            CenterMid,
                ///<summary> Center the progress bar up in the form. If Label.Visible = TRUE: Label will be center point instead and Label.AutoLocate will use the CenterTop position</summary>
            CenterMid_FullWidth
        }

        public ProgressDisplayLocation AutoLocation { get; set; }
        
        public void AutoLocateProgressBar()
        {
            if (!this.Visible) return;
            if (this.AutoLocation == ProgressDisplayLocation.Manual) return;

            //Adjust Width
            if (this.AutoLocation == ProgressDisplayLocation.CenterMid_FullWidth | this.AutoLocation == ProgressDisplayLocation.Bottom_FullWidth)
                this.Width = this.Parent.Size.Width - 22;
            //Center Mid
            if (this.AutoLocation == ProgressDisplayLocation.CenterMid | this.AutoLocation == ProgressDisplayLocation.CenterMid_FullWidth)
            {
                this.CenterControl(); //Center up height and width
                if (_labelVisible)
                {
                    this.Top = this.Top + this.Height / 2;
                    this.Label_AutoLocation = LabelDisplayLocation.CenterTop;
                    this.AutoLocateLabel();
                }
            }
            //Center Bottom
            if (this.AutoLocation == ProgressDisplayLocation.Bottom_FullWidth)
                this.CenterControl_Width();
            //Bottom Heights
            if (this.AutoLocation == ProgressDisplayLocation.Bottom_FullWidth | this.AutoLocation == ProgressDisplayLocation.Bottom)
                this.Top = this.Parent.Size.Height - this.Height - 41; //41 = border offset
        }


        public void ResetProgressBar()
        {
            this.Label_Text = "";
            this.Value = 0;
            this.Visible = false;
        }

        ///<summary> If this control moves, move the textbox too </summary>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
                _label.Parent = this.Parent;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AutoLocateLabel();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            AutoLocateLabel();
        }
    }

}



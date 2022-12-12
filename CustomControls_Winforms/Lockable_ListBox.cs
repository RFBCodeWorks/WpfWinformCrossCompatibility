using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControls.WinForms
{
    ///<summary> 
    ///Listbox that adds and enforced the ReadOnly capability <br/>
    ///This Listbox also allows text wrapping.
    ///</summary>
    [System.ComponentModel.DesignerCategory("Default")]
    public sealed class Lockable_ListBox : ListBox
    {
        public Lockable_ListBox() : base()
        {
            _ReadOnly = false;
            _SelectedIndexCollection = new List<int>();
            base.DrawMode = DrawMode.OwnerDrawVariable;
            base.MeasureItem += MeasureItemEvent;
            base.DrawItem += DrawItemEvent;
            base.SelectedIndexChanged += UserSelectedItem;
        }

        private bool _ReadOnly;
        private readonly List<int> _SelectedIndexCollection;

        ///<summary> Enable / Disable if user can select items in the listbox or not </summary>
        public bool ReadOnly { 
            get => _ReadOnly;
            set
            {
                _ReadOnly = value;
            }
        }

        ///<summary> prevent user from changing selection of items in the list </summary>
        public void Lock() => ReadOnly = true;

        ///<summary> Allow user to select items in the list </summary>
        public void UnLock() => ReadOnly = false;

        new public bool MultiColumn => false;

        private void UserSelectedItem(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                if (ReadOnly)
                    ResetSelectedItems();
                else
                    LogSelectedItems();
            }
        }

        private void LogSelectedItems()
        {
            _SelectedIndexCollection.Clear();
            foreach (int i in base.SelectedIndices)
                _SelectedIndexCollection.Add(i);
        }

        private void ResetSelectedItems()
        {
            base.SelectedIndices.Clear();
            foreach (int i in _SelectedIndexCollection)
                base.SelectedIndices.Add(i);
        }


        // Text Wrapping 
        //https://stackoverflow.com/questions/17613613/winforms-dotnet-listbox-items-to-word-wrap-if-content-string-width-is-bigger-tha

        private void MeasureItemEvent(object sender, MeasureItemEventArgs e)
        {
            if (!DesignMode)
                e.ItemHeight = (int)e.Graphics.MeasureString(base.Items[e.Index].ToString(), base.Font, base.Width).Height;
        }

        private void DrawItemEvent(object sender, DrawItemEventArgs e)
        {
            if (!DesignMode)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
                e.Graphics.DrawString(base.Items[e.Index].ToString(), e.Font, new System.Drawing.SolidBrush(e.ForeColor), e.Bounds);
            }
        }
    }

}

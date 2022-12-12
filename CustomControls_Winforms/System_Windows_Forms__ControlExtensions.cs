using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ControlExtensions
    {
        /// <summary>
        /// Invoke an action against some control if invoking is required. If Invoking is not required, run the action.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void InvokeIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
                action();
        }

        /// <summary>
        /// Check if this control or its parent(s) are in Design Mode.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static bool IsDesignMode(this Control control)
        {
            var site = control as ISite;
            return (site?.DesignMode ?? false) | ( control?.Parent?.IsDesignMode() ?? false);
        }
    }
}

namespace System.Windows.Forms
{
    public static partial class ControlExtensions
    {

        /// <summary>Retrieve an IEnumerable list of all controls and their children.</summary>
        /// <param name="control"></param>
        /// <returns>IEnumerable list of Controls</returns>
        /// <remarks>https://stackoverflow.com/questions/15186828/loop-through-all-controls-of-a-form-even-those-in-groupboxes</remarks>
        public static IEnumerable<Control> GetAllChildControls(this Control control)
            => Enumerable.Repeat(control, 1)
            .Union(control.Controls.OfType<Control>()
                .SelectMany(GetAllChildControls)
                );

        /// <summary>
        /// Method to get the actual VISIBLE state of the control, instead of the base one. This is because
        /// <see cref="Control.Visible"/> will return false any parent control is not visible. <br/>
        /// For example, if a control is on a TabPage within a TabControl, and a different TabPage is selected, Control.Visible will return false. <br/>
        /// But this can return true, as long as when that TabPage is selected this will be visible again.
        /// </summary>
        /// <param name="control"></param>
        /// <returns>TRUE if the control's public Visible setting is TRUE. Otherwise FALSE.</returns>
        public static bool Visibility(this Control control)
        {
            //https://stackoverflow.com/questions/27596529/how-to-identify-whether-visibility-of-the-control-is-changed-by-user-or-not

            //Avoid reflection if control is visible
            if (control.Visible)
                return true;

            //Find non-public GetState method of control using reflection
            System.Reflection.MethodInfo GetStateMethod = control.GetType().GetMethod("GetState", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            //return control's visibility if GetState method not found
            if (GetStateMethod != null)
                //return visibility from the state maintained for control
                return (bool)(GetStateMethod.Invoke(control, new object[] { 2 }));
            return false;
        }

        #region < Lock / Unlock Methods >

        // NOTE: Locking isn't really easy to do without creating a new class for each control type and overriding the OnPaint event. 
        // So instead of doing that, use the built-in functions and normalize the method name using an extension method.

        ///<summary>Turns ReadOnly ON</summary>
        public static void Lock(this TextBox cntrl) => cntrl.ReadOnly = true;
        ///<summary>Turns ReadOnly OFF</summary>
        public static void UnLock(this TextBox cntrl) => cntrl.ReadOnly = false;
        
        /// <summary>
        /// Check if the TextBox is ReadOnly
        /// </summary>
        /// <param name="cntrl"></param>
        /// <returns></returns>
        public static bool IsLocked(this TextBox cntrl) => cntrl.ReadOnly;

        ///<summary>Sets SelectionMode = NONE</summary>
        public static void Lock(this ListBox cntrl) => cntrl.SelectionMode = SelectionMode.None;
        ///<summary>Specify a Selection Mode</summary>
        public static void UnLock(this ListBox cntrl, SelectionMode mode) => cntrl.SelectionMode = mode;
        
        /// <summary>
        /// Check if the ListBox Selection Mode == <see cref="SelectionMode.None"/>
        /// </summary>
        /// <param name="cntrl"></param>
        /// <returns></returns>
        public static bool IsLocked(this ListBox cntrl) => cntrl.SelectionMode == SelectionMode.None;

        #endregion

        #region < CenterControl Methods >

        ///<summary> Centers this control within its container - adjusts TOP and LEFT properties </summary> 
        public static void CenterControl(this Control control) { control.CenterControl_Width(); control.CenterControl_Height(); }
        ///<summary> Centers this control within its container - adjusts TOP property </summary> 
        public static int CenterControl_Height(this Control control) => control.Top = (control.Parent.Height / 2) - (control.Height / 2);
        ///<summary> Centers this control within its container - adjusts LEFT property </summary> 
        public static int CenterControl_Width(this Control control) => control.Left = (control.Parent.Width / 2) - (control.Width / 2);
        ///<summary> Aligns this control to the center of another control </summary> 
        public static Drawing.Point CenterControl(this Control control, Control ControlToAlignTo) => control.Location = new Drawing.Point(control.CenterControl_Width(ControlToAlignTo, 0), control.CenterControl_Height(ControlToAlignTo, 0));
        ///<summary> Aligns the Height-Center of this control to be in line with the center of another control </summary> 
        public static int CenterControl_Height(this Control control, Control ControlToAlignTo) => control.CenterControl_Height(ControlToAlignTo, 0);
        ///<summary> Aligns the Height-Center of this control to be in line with the center of another control, then adjust the TOP value by the supplied offset. </summary> 
        public static int CenterControl_Height(this Control control, Control ControlToAlignTo, int TopOffset) => control.Top = ControlToAlignTo.Top + (ControlToAlignTo.Height / 2) - (control.Height / 2) + TopOffset;
        ///<summary> Aligns the Width-Center of this control to be in line with the center of another control </summary> 
        public static int CenterControl_Width(this Control control, Control ControlToAlignTo) => control.CenterControl_Width(ControlToAlignTo, 0);
        ///<summary> Aligns the Width-Center of this control to be in line with the center of another control, then adjust the LEFT value by the supplied offset. </summary> 
        public static int CenterControl_Width(this Control control, Control ControlToAlignTo, int LeftOffset) => control.Left = ControlToAlignTo.Left + (ControlToAlignTo.Width / 2) - (control.Width / 2) + LeftOffset;

        ///<summary> Adjust the Left location of the control relative to another control, then set the Y location of the control</summary> 
        /// <param name="control"></param>
        /// <param name="ControlToAlignTo">Takes this control's Left value as the base for the calculation</param>
        /// <param name="LeftOffset">Offset this control this distance compared to the <paramref name="control"/>'s Left value</param>
        /// <param name="Y">Y Position for this control</param>
        /// <returns></returns>
        public static Drawing.Point CenterControl_Width(this Control control, Control ControlToAlignTo, int LeftOffset, int Y) => control.Location = new Drawing.Point(CenterControl_Width(control, ControlToAlignTo, LeftOffset), Y);

        #endregion

        #region < Buffer Spacing >

        /// <summary>
        /// offsets the the LEFT of control 1 compared to the left of Control 2
        /// </summary>
        /// <param name="Cnt_1"></param>
        /// <param name="Cnt_2"></param>
        /// <param name="Offset"></param>
        /// <param name="UseRightOffsetInstead">Use the right offset instead of the left</param>
        /// <returns></returns>
        public static void Offset_Left(this Control Cnt_1, Control Cnt_2, int Offset, bool UseRightOffsetInstead)
        {
            int C2 = UseRightOffsetInstead ? Cnt_2.Right : Cnt_2.Left;
            Cnt_1.Location = new Drawing.Point(C2 + Offset, Cnt_1.Location.Y);
        }

        /// <summary>
        /// Offsets the control some distance from the bottom of the form
        /// </summary>
        /// <param name="Cnt"></param>
        /// <param name="DistanceFromBottom"></param>
        public static void Offset_Top(this Control Cnt, int DistanceFromBottom) => Cnt.Top = Cnt.Parent.Height - Cnt.Height - DistanceFromBottom;

        /// <summary>
        /// offsets the bottom of Control 1 a specific distance away from the top of Control 2
        /// </summary>
        /// <param name="Cnt_1"></param>
        /// <param name="Cnt_2"></param>
        /// <param name="DistanceAboveOtherControl"></param>
        /// <returns></returns>
        public static void Offset_Top(this Control Cnt_1, Control Cnt_2, int DistanceAboveOtherControl) => Cnt_1.Top = Cnt_2.Top - Cnt_1.Height - DistanceAboveOtherControl;

        #endregion

        #region < Data Binding Methods >

        /// <summary> Loops through all bindings of the supplied control and removes the binding from the requested Property Name</summary>
        /// <param name="Cntrl"></param>
        /// <param name="ControlPropertyName"></param>
        public static void RemoveBinding<T>(this T Cntrl, string ControlPropertyName) where T : Control
        {
            foreach (Binding B in (Cntrl).DataBindings)
            {
                if (B.PropertyName == ControlPropertyName)
                {
                    Cntrl.DataBindings.Remove(B);
                    return;
                }
            }
        }

        private static string PropertyPrefix(string BindingProperty) => (BindingProperty.Substring(0, 1) == ".") ? BindingProperty : String.Join(".", BindingProperty);

        #region < Simple Binding (Checkboxes / TextBoxes ) >

        /// <summary>Binds a some object property to a control's property. Removes the binding against this property first if one exists. </summary>
        /// <typeparam name="T">Object type of the object to bind to</typeparam>
        /// <param name="cntrl"> This is the control to bind to. </param>
        /// <param name="ControlProperty">Name of the property of the control that will be bound to the object.</param>
        /// <param name="Obj"> This is the object for the control to bind to. </param>
        /// <param name="PropertyToBind">Name of the <paramref name="Obj"/> property to be bound to. Should be formatted as ' . {PropertyName} '</param>
        public static void Bind<T>(this Control cntrl, string ControlProperty, T Obj, string PropertyToBind)
        {
            RemoveBinding(cntrl, ControlProperty);
            if (Obj != null) cntrl.DataBindings.Add(new Binding(ControlProperty, Obj, PropertyPrefix(PropertyToBind)));
        }

        /// <summary>Binds a Boolean Property to a checkbox's CHECKED property</summary>
        /// <param name="cntrl"></param>
        /// <param name="Obj">Object to bind to</param>
        /// <param name="BooleanProperty">Boolean property of the object to bind to. </param>
        /// <inheritdoc cref="Bind{T}(Control, string, T, string)"/>
        public static void Bind<T>(this CheckBox cntrl, T Obj, string BooleanProperty) => cntrl.Bind("Checked", Obj, BooleanProperty);

        /// <summary>Binds a Boolean Property to a RadioButton's CHECKED property</summary>
        /// <inheritdoc cref="Bind{T}(CheckBox, T, string)"/>
        public static void Bind<T>(this RadioButton cntrl, T Obj, string BooleanProperty) => cntrl.Bind("Checked", Obj, BooleanProperty);

        /// <summary>Binds a string Property to a Textbox's TEXT property</summary>
        /// <inheritdoc cref="Bind{T}(Control, string, T, string)"/>
        public static void Bind<T>(this TextBoxBase cntrl, T Obj, string PropertyToBind) => cntrl.Bind("Text", Obj, PropertyToBind);

        #endregion //Simple Binding 

        /// <summary> Binds a combobox to the supplied Dictionary</summary>
        /// <typeparam name="key">This is the KEY object type (typically string or int )</typeparam>
        /// <typeparam name="val">This is the VALUE object type in the dictionary</typeparam>
        /// <param name="listbox">Listbox to bind</param>
        /// <param name="Dict">{int,string} or {string,string} dictionary</param>
        /// /// <param name="DisplayMemberIsKey"> Default function assumes the KEY will be displayed. If set to false, it will display the VALUE member instead </param>
        public static void Bind<key, val>(this ListBox listbox, Dictionary<key, val> Dict, bool DisplayMemberIsKey = true)
        {//https://stackoverflow.com/questions/6412739/binding-combobox-using-dictionary-as-the-datasource
            if (Dict.Count > 0)
            {
                listbox.DataSource = new BindingSource(Dict, null);
                listbox.DisplayMember = (DisplayMemberIsKey) ? "Key" : "Value";
                listbox.ValueMember = (DisplayMemberIsKey) ? "Value" : "Key";
                listbox.Enabled = true;
            }
            else
            {
                RemoveBinding<ListBox>(listbox, "DataSource");
                listbox.DataSource = null;
                listbox.DisplayMember = "";
                listbox.ValueMember = "";
                listbox.Enabled = false;
                listbox.Items.Clear();
                listbox.Items.Add("No Items to Display");
            }
        }

        /// <inheritdoc cref="Bind{ListType}(ListBox, IEnumerable{ListType}, string, string)"/>
        public static void Bind<ListType>(this ListBox listbox, IEnumerable<ListType> list) => listbox.Bind<ListType>(list, "", "");
        /// <inheritdoc cref="Bind{ListType}(ListBox, IEnumerable{ListType}, string, string)"/>
        public static void Bind<ListType>(this ListBox listbox, IEnumerable<ListType> list, string displayMember) => listbox.Bind<ListType>(list, displayMember, "");

        /// <summary>
        /// Bind the Listbox to a list.
        /// </summary>
        /// <typeparam name="ListType">Type of list to bind to</typeparam>
        /// <param name="listbox"></param>
        /// <param name="list">The list to bind to</param>
        /// <param name="displayMember"><inheritdoc cref="ListControl.DisplayMember"/></param>
        /// <param name="valueMember"><inheritdoc cref="ListControl.ValueMember"/></param>
        public static void Bind<ListType>(this ListBox listbox, IEnumerable<ListType> list, string displayMember, string valueMember)
        {//https://stackoverflow.com/questions/6412739/binding-combobox-using-dictionary-as-the-datasource

                if (list.Count() > 0)
                {
                    // NOTE: Ignore the Null Reference Exception here, as it is a problem with CheckListBox.RefreshItems(), which is a protected method.
                    // This is a problem with MS source code, and must be resolved by them. The CheckedListBox itself works properly otherwise.
                    // Note: Even though the exception occurs in the debugger, its not actually being thrown, so it can safely be ignored.
                    listbox.DataSource = new BindingSource(list, null);
                    listbox.DisplayMember = displayMember;
                    listbox.ValueMember = valueMember;
                    listbox.Enabled = true;
                }
                else
                {
                    RemoveBinding<ListBox>(listbox, "DataSource");
                    listbox.DisplayMember = "";
                    listbox.ValueMember = "";
                    listbox.Enabled = false;
                    listbox.DataSource = null;
                    listbox.Items.Clear();
                    listbox.Items.Add("No Items to Display");
                }
        }

        #endregion < Data Binding >

        #region < ForEach() Style Methods >

        /// <summary>
        /// Loop through all items in the listbox and check if the item is selected
        /// </summary>
        /// <param name="listbox"></param>
        /// <returns>new ListBox.ObjectCollection of all items that are not selected</returns>
        public static ListBox.ObjectCollection GetUnselectedItems(this ListBox listbox)
        {
            var ret = new ListBox.ObjectCollection(listbox);
            for (int i = 0; i < listbox.Items.Count; i++)
            {
                if (!listbox.GetSelected(i))
                    ret.Add(listbox.Items[i]);
            }
            return ret;
        }

        /// <summary> Runs <see cref="ListBox.SetSelected(int, bool)"/>  on all items in the list</summary>
        public static void SelectAll(this ListBox listbox)
        {
            for (int i = 0; i < listbox.Items.Count; i++)
                listbox.SetSelected(i, true);
        }

        /// <summary> Runs <see cref="CheckedListBox.SetItemCheckState(int, CheckState)"/> = <see cref="CheckState.Checked"/> on all items in the list.</summary>
        public static void CheckAllItems(this CheckedListBox chkListBox)
        {
            for (int i = 0; i < chkListBox.Items.Count; i++)
                chkListBox.SetItemCheckState(i, CheckState.Checked);
        }

        /// <summary> Runs <see cref="CheckedListBox.SetItemCheckState(int, CheckState)"/> on all items in the list.</summary>
        /// <param name="state">State to assign all items in the list.</param>
        /// <param name="chkListBox"></param>
        public static void SetAllItems(this CheckedListBox chkListBox, CheckState state)
        {
            for (int i = 0; i < chkListBox.Items.Count; i++)
                chkListBox.SetItemCheckState(i, state);
        }

        /// <inheritdoc cref="ForEach{T}(Collections.ICollection, Action{T})"/>
        public static void ForEach(this ListBox.ObjectCollection collection, Action<object> action)
            => ForEach<object>(collection, action);

        /// <inheritdoc cref="ForEach{T}(Collections.ICollection, Predicate{object})"/>
        public static object ForEach(this ListBox.ObjectCollection collection, Predicate<object> predicate)
            => ForEach<object>(collection, predicate);

        /// <inheritdoc cref="ForEach{T}(Collections.ICollection, Action{T})"/>
        public static void ForEach(this ListBox.SelectedObjectCollection collection, Action<object> action)
            => ForEach<object>(collection, action);

        /// <inheritdoc cref="ForEach{T}(Collections.ICollection, Predicate{object})"/>
        public static object ForEach(this ListBox.SelectedObjectCollection collection, Predicate<object> predicate)
            => ForEach<object>(collection, predicate);

        /// <summary>
        /// Run some action against all items in the collection
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        private static void ForEach<T>(Collections.ICollection collection, Action<T> action)
        {
            foreach (T o in collection)
                action.Invoke(o);
        }

        /// <summary>
        /// Run some action against all items in the collection
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="predicate"><inheritdoc cref="Predicate{T}"/></param>
        /// <returns>
        /// If the <paramref name="predicate"/> returned True, returns the object that broke the loop. <br/>
        /// If no object returned true, this returns 'default'.
        /// </returns>
        private static T ForEach<T>(Collections.ICollection collection, Predicate<object> predicate)
        {
            foreach (T o in collection)
                if (predicate.Invoke(o)) return o;
            return default;
        }

        #endregion

        ///// <summary>
        ///// Take the control and move up through its IContainer stack until a tooltip is found.
        ///// </summary>
        ///// <param name="control"></param>
        ///// <returns>Returns the first tooltip found in a control's IContainer Stack. If no tooltip is found, returns null.</returns>
        //public static ToolTip FindFirstToolTip(this Control control)
        //{
        //    var obj = GetComponentCollection(control);
        //    if (obj is null) return null;
        //    var tTips = obj.OfType<ToolTip>();
        //    if (tTips.Count() > 0) return tTips.First();
        //    if (control.Parent is Control)
        //        return FindFirstToolTip(control.Parent);
        //    else return null;
            
        //}

        ///// <summary>
        ///// Get the private Component Collection from some control
        ///// </summary>
        ///// <param name="control"></param>
        ///// <returns></returns>
        //public static ComponentCollection GetComponentCollection(this Control control)
        //{
        //    Type parentType = control.Parent.GetType();
        //    FieldInfo fieldInfo = parentType.GetField("components", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
        //    var Obj = fieldInfo.GetValue(control.Parent) as IContainer;
        //    return Obj?.Components;
        //}
    }
}

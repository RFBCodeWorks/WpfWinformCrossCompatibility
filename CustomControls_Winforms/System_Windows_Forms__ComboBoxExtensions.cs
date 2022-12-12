using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace System.Windows.Forms
{
    /// <summary>
    /// Contains extension methods for standard control types
    /// </summary>
    public static partial class ControlExtensions
    {

        #region < ComboBox Validation 

        private static bool ComboxValidationRunning = false;

        /// <inheritdoc cref="Validate(ComboBox,bool)"/>
        public static bool Validate(this ComboBox Cmb) => Cmb.Validate(true);

        /// <summary>Provides a method to evaluate if a match exists on the Combobox since MatchRequired property doesn't exist.  Shows the default error message if invalid.</summary>
        /// <inheritdoc cref="Validate(ComboBox,bool,string,string)"/>
        public static bool Validate(this ComboBox Cmb, bool NotifyUser) => Cmb.Validate(NotifyUser, "Please select an item from the list!", "Invalid Selection!");

        /// <inheritdoc cref="Validate(ComboBox,bool,string,string)"/>
        public static bool Validate(this ComboBox Cmb, string ErrorMessageToDisplay) => Cmb.Validate(ErrorMessageToDisplay, "Invalid Selection!");

        /// <inheritdoc cref="Validate(ComboBox,bool,string,string)"/>
        public static bool Validate(this ComboBox Cmb, string ErrorMessageToDisplay, string ErrorMessageWindowTitle) => Cmb.Validate(true, ErrorMessageToDisplay, ErrorMessageWindowTitle);

        /// <summary>Provides a method to evaluate if a match exists on the Combobox since MatchRequired property doesn't exist.</summary>
        /// <param name="Cmb">Compare the selected item and the entered text of this combobox against the items in the list.</param>
        /// <param name="NotifyUser">Declare wether to display an error message to the user, or simply validate the results</param>
        /// <param name="ErrorMessageToDisplay">Message to display to user if the a match is not found.</param>
        /// <param name="ErrorMessageWindowTitle">Title of the Error Message Window if it pops up</param>
        /// <returns></returns>
        public static bool Validate(this ComboBox Cmb, bool NotifyUser, string ErrorMessageToDisplay, string ErrorMessageWindowTitle)
        {
            if (ComboxValidationRunning) return true; //This flag avoids an infinite loop of events running against this routine if the combox isn't validated
            ComboxValidationRunning = true;
            bool Invalid = false;
            switch (true)
            {
                case true when (!(Cmb.SelectedIndex >= 0 && Cmb.SelectedIndex < Cmb.Items.Count)): Invalid = true; break; // somehow doesn't match any items
                case true when (!Cmb.Items.Contains(Cmb.Text)): Invalid = true; break; // Typed Value does not match !
            }
            if (Invalid && (NotifyUser))
            {
                Cmb.Text = "";
                MessageBox.Show(ErrorMessageToDisplay, ErrorMessageWindowTitle, MessageBoxButtons.OK);
                Cmb.Focus();
                Cmb.DroppedDown = true;
            }
            ComboxValidationRunning = false;
            return !Invalid;
        }

        #endregion


        #region < Combo Box Binding >

        ///<summary> Unbinds the DataSource, DisplayMember, ValueMember and SelectedValue</summary>
        public static void UnBind_All(this ComboBox cmb)
        {
            cmb.UnBind_DataSource();
            cmb.UnBind_DisplayMember();
            cmb.UnBind_ValueMember();
            cmb.UnBind_SelectedValue();
        }

        /// <summary> Unbind the DataSource property from the combobox </summary>
        public static void UnBind_DataSource(this ComboBox cmb)
        {
            RemoveBinding<ComboBox>(cmb, "DataSource");
            cmb.DataSource = null;
        }

        /// <summary> Unbind the DisplayMember property from the combobox </summary>
        public static void UnBind_DisplayMember(this ComboBox cmb) => RemoveBinding<ComboBox>(cmb, "DisplayMember");
        /// <summary> Unbind the SelectedValue property from the combobox </summary>
        public static void UnBind_SelectedValue(this ComboBox cmb) => RemoveBinding<ComboBox>(cmb, "SelectedValue");
        /// <summary> Unbind the ValueMember property from the combobox </summary>
        public static void UnBind_ValueMember(this ComboBox cmb) => RemoveBinding<ComboBox>(cmb, "ValueMember");

        /// <summary> Binds a combobox to the supplied Dictionary</summary>
        /// <typeparam name="key">This is the KEY object type (typically string or int )</typeparam>
        /// <typeparam name="val">This is the VALUE object type in the dictionary</typeparam>
        /// <param name="Cmb">combox to bind</param>
        /// <param name="Dict">{int,string} or {string,string} dictionary</param>
        /// <param name="AllowDrop"><inheritdoc cref="Control.AllowDrop" path="*"/></param>
        /// <param name="DisplayMemberIsKey"> Default function assumes the KEY will be displayed. If set to false, it will display the VALUE member instead </param>
        public static void Bind<key, val>(this ComboBox Cmb, Dictionary<key, val> Dict, bool DisplayMemberIsKey = true, bool AllowDrop = false)
        {//https://stackoverflow.com/questions/6412739/binding-combobox-using-dictionary-as-the-datasource
            if (Dict.Count > 0)
            {
                Cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                Cmb.DataSource = null;
                Cmb.DisplayMember = (DisplayMemberIsKey) ? "Key" : "Value";
                Cmb.ValueMember = (DisplayMemberIsKey) ? "Value" : "Key";
                Cmb.DataSource = new BindingSource(Dict, null);
                Cmb.AllowDrop = AllowDrop;
                Cmb.Enabled = true;
            }
            else
            {
                Cmb.UnBind_All();
                Cmb.Enabled = false;
                Cmb.AllowDrop = false;
                Cmb.Text = "No Items to Display";
            }
        }

        /// <summary> Binds a combobox to the supplied Dictionary </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="Cmb">combox to bind</param>
        /// <param name="Dict">Bind the dropdown to the values of this {int,string} or {string,string} dictionary</param>
        /// <param name="Obj"> Object whose property is updated as the combobox selection changes</param>
        /// <param name="BindToProperty">Bind the'SELECTED VALUE' combox property to this property of the object.</param>
        /// <param name="AllowDrop"><inheritdoc cref="Control.AllowDrop"/></param>
        public static void Bind<T, O>(this ComboBox Cmb, Dictionary<T, string> Dict, O Obj, string BindToProperty, bool AllowDrop = false)
        {//https://stackoverflow.com/questions/6412739/binding-combobox-using-dictionary-as-the-datasource

            Cmb.UnBind_All();
            Cmb.Bind<T, string>(Dict, false, AllowDrop);
            RemoveBinding(Cmb, "SelectedValue");
            Cmb.DataBindings.Add("SelectedValue", Obj, PropertyPrefix(BindToProperty));
        }

        ///<inheritdoc cref="Bind{key, val}(ComboBox, Dictionary{key, val}, bool, bool)"/>
        public static void Bind<key, val>(this ComboBox Cmb, IReadOnlyDictionary<key, val> Dict, bool DisplayMemberIsKey = true, bool AllowDrop = false)
        {//https://stackoverflow.com/questions/6412739/binding-combobox-using-dictionary-as-the-datasource

            if ((Dict?.Count ?? 0) > 0)
            {
                Cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                Cmb.DataSource = new BindingSource(Dict, null);
                Cmb.DisplayMember = (DisplayMemberIsKey) ? "Key" : "Value";
                Cmb.ValueMember = (DisplayMemberIsKey) ? "Value" : "Key";
                Cmb.Enabled = true;
                Cmb.AllowDrop = AllowDrop;
            }
            else
            {
                Cmb.UnBind_All();
                Cmb.Enabled = false;
                Cmb.Text = "No Items to Display";
            }
        }

        /// <inheritdoc cref="Bind{T}(ComboBox, DataTable, string, string, T, string)"/>
        public static void Bind(this ComboBox Cmb, DataTable Tbl, string DisplayColName, string ValueColName)
        {//https://stackoverflow.com/questions/6412739/binding-combobox-using-dictionary-as-the-datasource
            Cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            Cmb.DataSource = new BindingSource(Tbl, null);
            Cmb.DisplayMember = DisplayColName;
            Cmb.ValueMember = ValueColName;
        }

        /// <summary> Binds a combobox to the supplied DataTable </summary>
        /// <param name="Cmb">combox to bind</param>
        /// <param name="Tbl">DataTable to bind to</param>
        /// <param name="DisplayColName">Display all values in this column</param>
        /// <param name="ValueColName">Return the value in this column for the selected item</param>
        /// <param name="Obj">Robot Object to bind to</param>
        /// <param name="BindToProperty">Bind the'SELECTED VALUE' combox property to this robot property</param>
        public static void Bind<T>(this ComboBox Cmb, DataTable Tbl, string DisplayColName, string ValueColName, T Obj, string BindToProperty)
        {

            Bind(Cmb, Tbl, DisplayColName, ValueColName);
            RemoveBinding(Cmb, "SelectedValue");
            Cmb.DataBindings.Add("SelectedValue", Obj, PropertyPrefix(BindToProperty));
        }

        /// <summary>Doesn't actually BIND to the enum, but rather adds the Enum names to the list </summary>
        /// <param name="Cmb">ComboBox to numerate</param>
        /// <param name="EnumType">Selected Enum Type example: { typeof(UserSettings.UserTypes) } </param>
        public static void Bind(this ComboBox Cmb, Type EnumType)
        {
            Cmb.Items.Clear();
            foreach (string Item in Enum.GetNames(EnumType))
            {
                Cmb.Items.Add(Item);
            }
        }

        /// <summary>Binds the combobox to either a List{String} or a String[] </summary>
        /// <param name="Cmb">ComboBox to populate </param>
        /// <param name="List">List{String} or a String[]</param>
        public static void Bind<ListType>(this ComboBox Cmb, IEnumerable<ListType> List) => Bind(Cmb, List, "");

        /// <summary>Binds the combobox to either a List{String} or a String[] </summary>
        /// <param name="Cmb">ComboBox to populate </param>
        /// <param name="List">List{String} or a String[]</param>
        /// <param name="displayMember"><inheritdoc cref="ListControl.DisplayMember"/></param>
        /// <param name="allowDrop"><inheritdoc cref="Control.AllowDrop" path="*"/></param>
        public static void Bind<ListType>(this ComboBox Cmb, IEnumerable<ListType> List, string displayMember, bool allowDrop = false)
        {//https://stackoverflow.com/questions/6412739/binding-combobox-using-dictionary-as-the-datasource
            if (List.Count() > 0)
            {
                Cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                Cmb.DataSource = new BindingSource(List, null);
                Cmb.DisplayMember = displayMember;
                Cmb.ValueMember = "";
                Cmb.Enabled = true;
                Cmb.AllowDrop = allowDrop;
            }
            else
            {
                Cmb.UnBind_All();
                Cmb.Enabled = false;
                Cmb.Text = "No Items to Display";
            }
        }
        //public static void ComboBox_Populate(ComboBox Cmb, IList<string> List) { foreach (string Item in List) { Cmb.Items.Add(Item); } }

        #endregion // Combo Box Binding 
    }
}

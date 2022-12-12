using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace RobotSoftwareTool.UserForms.Shared
{
    /// <summary>
    /// How to use this form:
    /// <br/> - Create a new object of this form type, but keep it hidden. The path can be changed after the fact, but the boolean properties must be submitted on construction.
    /// <br/> - Once the form is shown, it will use the  <b><see cref="DirectoryToOpen"/></b> property to navigate to some directory. 
    /// <br/> - Based on the  <b><see cref="DisplayFolders"/></b>, <b><see cref="DisplayFiles"/></b> and <b><see cref="SelectionFilter"/></b> properties, it will determine what to display.
    /// <br/> - NOTE: if both DisplayFolders and DisplayFiles are true, only files will be shown. 
    /// <br/> - NOTE: This form should be shown as a DIALOG 
    /// </summary>
    /// <returns>
    ///  - When the 'Accept' button is clicked, the form will hide (not close). 
    ///  <br/> - This will allow the caller to access the <b><see cref="SelectedFiles"/></b> or  <b><see cref="SelectedFolders"/></b> list (caller must choose correct item)
    ///  <br/> - If the user selects the '<b>Cancel</b>' button, then both lists remain empty and  <b><see cref="UserCancelled"/></b> is set to true.
    ///  <br/> - Once complete, dispose the form.
    /// </returns>
    public partial class MultiFileSelector : Form
    {
        public MultiFileSelector()
        {
            InitializeComponent();
            DisplayFiles = true;
            SelectionFilter = "*";
        }

        public MultiFileSelector(bool displayFiles, bool displayFolders)
        {
            InitializeComponent();
            DisplayFolders = displayFolders;
            DisplayFiles = displayFiles;
            SelectionFilter = "*";
        }

        public MultiFileSelector(bool displayFiles, bool displayFolders, DirectoryInfo directoryToOpen)
        {
            InitializeComponent();
            DisplayFolders = displayFolders;
            DisplayFiles = displayFiles;
            DirectoryToOpen = directoryToOpen;
            SelectionFilter = "*";
        }

        public MultiFileSelector(bool displayFiles, bool displayFolders, DirectoryInfo directoryToOpen, string selectionFilter)
        {
            InitializeComponent();
            DisplayFolders = displayFolders;
            DisplayFiles = displayFiles;
            DirectoryToOpen = directoryToOpen;
            SelectionFilter = selectionFilter;
        }

        public DirectoryInfo DirectoryToOpen { get; set; }
        public string SelectionFilter { get; set; }
        public string LabelText { get => this.label1.Text; set => this.label1.Text = value; }

        public bool DisplayFolders{ get; private set; }
        public bool DisplayFiles { get; private set; }
        public bool UserCancelled { get; private set; }
        
        public List<DirectoryInfo> SelectedFolders { get; private set; }
        public List<FileInfo> SelectedFiles { get; private set; }

        private Dictionary<string, DirectoryInfo> DirDict;
        private Dictionary<string, FileInfo> FileDict;

        private void OnShownEvent(object sender, EventArgs e) { this.label1.AutoSize = true; this.label1.CenterControl_Width(); }

        private void MultiFileSelector_Load(object sender, EventArgs e)
        {
            RefreshDisplayedItems();
        }


        /// <returns> If user clicks the 'Accept' button, return <see cref="DialogResult.OK"/>. Otherwise return <see cref="DialogResult.Cancel"/></returns>
        /// <inheritdoc cref="Form.ShowDialog(IWin32Window)"/>
        new public DialogResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
            return this.UserCancelled ? DialogResult.Cancel : DialogResult.OK;
        }

        /// <returns> If user clicks the 'Accept' button, return <see cref="DialogResult.OK"/>. Otherwise return <see cref="DialogResult.Cancel"/></returns>
        /// <inheritdoc cref="Form.ShowDialog()"/>
        new public DialogResult ShowDialog()
        {
            base.ShowDialog();
            return this.UserCancelled ? DialogResult.Cancel : DialogResult.OK;
        }

        private void RefreshDisplayedItems()
        {
            ResetExitLists();
            if (DisplayFiles)
            {
                FileDict = new Dictionary<string, FileInfo>();
                ShowFiles();
            }
            else if (DisplayFolders)
            {
                DirDict = new Dictionary<string, DirectoryInfo>();
                ShowFolders();
            }
        }

        private void ResetExitLists()
        {
            SelectedFolders = new List<DirectoryInfo>();
            SelectedFiles = new List<FileInfo>();
        }

        private void ShowFolders() 
        {
            if (!(this.DirectoryToOpen?.Exists ?? false))
                return;
            FileDict = null;
            DirectoryInfo[] dirs = DirectoryToOpen.GetDirectories(SelectionFilter);
            foreach (DirectoryInfo d in dirs)
            {
                DirDict.Add(d.Name, d);
                this.checkedListBox1.Items.Add(d.Name);
            }
        }

        private void ShowFiles() 
        {
            if (!(this.DirectoryToOpen?.Exists ?? false))
                return;
            DirDict = null;
            FileInfo[] dirs = DirectoryToOpen.GetFiles(SelectionFilter);
            foreach (FileInfo d in dirs)
            { 
                FileDict.Add(d.Name, d);
                this.checkedListBox1.Items.Add(d.Name);
            }
        }

        private void Btn_Accept_Click(object sender, EventArgs e)
        {
            this.ResetExitLists();

            foreach (var item in this.checkedListBox1.CheckedItems)
            {
                if (this.DisplayFiles && FileDict.TryGetValue(item.ToString(), out FileInfo f))
                    SelectedFiles.Add(f);

                else if (this.DisplayFolders && DirDict.TryGetValue(item.ToString(), out DirectoryInfo d))
                    SelectedFolders.Add(d);
            }
            UserCancelled = false;
            this.Hide();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            ResetExitLists();
            UserCancelled = true;
            this.Hide();
        }
    }
}


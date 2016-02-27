using System;
using System.Windows.Forms;

namespace SSGui
{
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
        public string WindowName
        {
            set { this.Text = value; }
        }

        public string SelectedCellName
        {
            get { return nameBox.Text; }

            set { nameBox.Text = value; }
        }

        public string SelectedCellValue
        {
            set { valueBox.Text = value; }
        }

        public string SelectedCellContents
        {
            get { return contentsBox.Text; }

            set { this.contentsBox.Text = value; }
        }

        public SpreadsheetWindow()
        {
            InitializeComponent();

            spreadsheetPanel.SelectionChanged += CellSelectionHandler;
        }

        private void CellSelectionHandler(SpreadsheetPanel ssPanel)
        {
            int col, row;
            ssPanel.GetSelection(out col, out row);

            if (CellSelectionChangedEvent != null)
            {
                CellSelectionChangedEvent(col, row);
            }
        }

        public event Action NewFileEvent;
        public event Action<string> FileChosenEvent;
        public event Action<string> SaveFileEvent;
        public event Action CloseEvent;
        public event Action HelpContentsEvent;
        public event Action<int, int> SetContentsEvent;
        public event Action<int, int> CellSelectionChangedEvent;

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewFileEvent != null)
            {
                NewFileEvent();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(openFileDialog.FileName);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveAsFileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (SaveFileEvent != null)
                {
                    SaveFileEvent(saveAsFileDialog.FileName);
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HelpContentsEvent != null)
            {
                HelpContentsEvent();
            }
        }

        public void SetCellValue(int col, int row, string value)
        {
            spreadsheetPanel.SetValue(col, row, value);
        }

        private void contentsBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return) // ENTER key is pressed
            {
                int col, row;
                spreadsheetPanel.GetSelection(out col, out row);

                if (SetContentsEvent != null)
                {
                    SetContentsEvent(col, row);
                }
            }
        }
    }
}

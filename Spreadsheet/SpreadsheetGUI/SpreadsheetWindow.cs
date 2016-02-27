using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSGUI
{
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
        public string WindowName
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedCellName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedCellValue
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedCellContents
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SpreadsheetWindow()
        {
            InitializeComponent();
        }

        public event Action NewFileEvent;
        public event Action<string> FileChosenEvent;
        public event Action<string> SaveFileEvent;
        public event Action CloseEvent;
        public event Action HelpContentsEvent;
        public event Action<string> SetContentsEvent;
        public event Action<string> CellSelectionChangedEvent;

        private void formulaBox_TextChanged(object sender, EventArgs e)
        {
            // TODO if 'Enter' is hit, do something
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void SetCellValue(int col, int row, string value)
        {
            throw new NotImplementedException();
        }
    }
}

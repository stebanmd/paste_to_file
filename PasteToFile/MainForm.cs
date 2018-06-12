using System;
using System.Windows.Forms;

namespace PasteToFile
{
    public partial class MainForm : Form
    {
        public MainForm(string fileName)
        {
            InitializeComponent();
            txtNome.Text = fileName;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNome.Text))
                {
                    ContextUtil.RemoveContext();
                }
                else
                {
                    ContextUtil.CreateContext(txtNome.Text);
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtNome.SelectAll();
            txtNome.Focus();
        }
    }
}
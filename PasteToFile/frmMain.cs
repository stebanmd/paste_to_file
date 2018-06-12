using System;
using System.Windows.Forms;

namespace PasteToFile
{
  public partial class frmMain : Form
  {
    public frmMain(string fileName)
    {
      InitializeComponent();
      txtNome.Text = fileName;      
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtNome.Text))
        {
          Utils.RemoveContexto();

        }
        else
        {
          Utils.CriaContexto(txtNome.Text);
        }

        Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
      txtNome.SelectAll();
      txtNome.Focus();
    }
  }
}
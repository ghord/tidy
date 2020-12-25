using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tidy.Showcase.Forms
{
    public partial class Buttons : UserControl
    {
        public Buttons()
        {
            InitializeComponent();
            //EnableAeroAnimations();
            SetDefaultButton();
        }

        private void SetDefaultButton()
        {
            defaultButton.NotifyDefault(true);
        }

        private void EnableAeroAnimations()
        {
            foreach (Control control in Controls)
            {
                if (control is ButtonBase buttonControl)
                {
                    buttonControl.FlatStyle = FlatStyle.System;
                }
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolMgt.UI.Controls
{
    public partial class PDFControl: UserControl
    {
        public PDFControl(string filename)
        {
            InitializeComponent();
            this.axAcroPDF1.LoadFile(filename);
            axAcroPDF1.setShowScrollbars(false);
        }
    }
}

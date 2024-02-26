using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.FormDisplayManager
{
    public partial class FormLogout : Form
    {
        public string name;
        public FormLogout()
        {
            InitializeComponent();
            Console.WriteLine(name);
        }
        
    }
}

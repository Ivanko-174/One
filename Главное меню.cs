using CIPHER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CIPHER.Вход;
using CIPHER;
using CIPHER_WF;

namespace CIPHER_WF
{
    public partial class Главное_меню : Form
    {
        public Главное_меню()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Шифратор Шиф = new Шифратор();
            Шиф.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Дешифратор ДеШиф = new Дешифратор();
            ДеШиф.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Description Desc = new Description();
            Desc.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ШифраторФайлов SFa = new ШифраторФайлов();
            SFa.Show();
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PursaklarBelediyesiSporSalonu
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }

        private void FormGetir(Form frm)
        {
            panel2.Controls.Clear();
            frm.MdiParent = this;
            frm.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(frm);
            frm.Show();
        }

        private void btnUyeEkle_Click(object sender, EventArgs e)
        {
            UyeEkle uyeekle = new UyeEkle();
            FormGetir(uyeekle);
        }

        private void btnUyeSilGuncelle_Click(object sender, EventArgs e)
        {
            UyeSilGuncelle uye = new UyeSilGuncelle();
            FormGetir(uye);
        }

        private void btnPaketler_Click(object sender, EventArgs e)
        {
            Paketler paketler = new Paketler();
            FormGetir(paketler);
        }

        private void btnHocalar_Click(object sender, EventArgs e)
        {
            Hocalar hocalar = new Hocalar();
            FormGetir(hocalar);
        }

        private void btnDersler_Click(object sender, EventArgs e)
        {
            Dersler dersler = new Dersler();
            FormGetir(dersler);
        }

        private void btnOdemeler_Click(object sender, EventArgs e)
        {
            Odemeler odemeler = new Odemeler();
            FormGetir(odemeler);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Çıkış yapılacak.
        }
    }
}

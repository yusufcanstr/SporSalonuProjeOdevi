using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PursaklarBelediyesiSporSalonu
{
    public partial class Paketler : Form
    {
        public Paketler()
        {
            InitializeComponent();
        }

        SqlConnection bag = new SqlConnection("Data Source=DESKTOP-HC8OO1J\\MSSQLSERVER1;Initial Catalog=PursaklarSporSalonuDB;Integrated Security=True");

        private void Paketler_Load(object sender, EventArgs e)
        {
            loadPaketlerDG();
        }

        private void loadPaketlerDG()
        {
            string sql = "SELECT * FROM PAKET_TBL";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (bag.State == ConnectionState.Closed)
            {
                bag.Open();
                SqlCommand veriEkle = new SqlCommand("INSERT INTO PAKET_TBL (ADI,FIYAT,AY) VALUES (@adi, @fiyat, @ay)", bag);
                veriEkle.Parameters.AddWithValue("@adi", tbPaketAdi.Text);
                veriEkle.Parameters.AddWithValue("@fiyat", int.Parse(tbFiyat.Text));
                veriEkle.Parameters.AddWithValue("@ay", int.Parse(tbAy.Text));
                veriEkle.ExecuteNonQuery();
                bag.Close();
                MessageBox.Show("Veri Eklendi !");
                loadPaketlerDG();
            }
            else
            {
                MessageBox.Show("Veri Ekleme Başarısız !");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            //Silme İşlemi 
            for(int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                bag.Open();
                string sql = "DELETE FROM PAKET_TBL WHERE PAKET_ID = '" + dataGridView1.SelectedRows[i].Cells[0].Value.ToString() + "'";
                SqlCommand komut = new SqlCommand(sql,bag);
                komut.ExecuteNonQuery();
                bag.Close();
            }
            MessageBox.Show("Paket silindi.");
            loadPaketlerDG();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

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
    public partial class UyeEkle : Form
    {
        public UyeEkle()
        {
            InitializeComponent();
        }

        SqlConnection bag = new SqlConnection("Data Source=DESKTOP-HC8OO1J\\MSSQLSERVER1;Initial Catalog=PursaklarSporSalonuDB;Integrated Security=True");

        private void UyeEkle_Load(object sender, EventArgs e)
        {
            loadUyelerDG();
            hocaIsimleriCbYukle();
            paketIsimleriCbYukle();
            textBoxlariTemizle();
        }

        private void hocaIsimleriCbYukle()
        {
            DataTable tablo = new DataTable();
            string sql = "SELECT * FROM HOCALAR_TBL";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            da.Fill(tablo);
            cbHoca.ValueMember = "ID_HOCA";
            cbHoca.DisplayMember = "H_ADI";
            cbHoca.DataSource = tablo;
        }

        private void paketIsimleriCbYukle()
        {
            DataTable tablo = new DataTable();
            string sql = "SELECT * FROM PAKET_TBL";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            da.Fill(tablo);
            cbPaket.ValueMember = "PAKET_ID";
            cbPaket.DisplayMember = "ADI";
            cbPaket.DataSource = tablo;
            
        }

        public string AyiYazdir(int aySayisi)
        {
            if (aySayisi >= 1 && aySayisi <= 12)
            {
                DateTime tarih = DateTime.Now;
                DateTime hedefTarih = tarih.AddMonths(aySayisi - 1);
                string ayAdi = hedefTarih.ToString("MMMM");
                return ayAdi;
            }
            else
            {
                return "Geçersiz ay sayısı";
            }
        }


        private void loadUyelerDG()
        {
            string sql = "SELECT U.U_ID, U.U_AD, U.U_SOYAD, U.DOG_TAR, P.ADI , H.H_ADI, U.BAS_TAR, U.BIT_TAR FROM UYE_TBL U, PAKET_TBL P, HOCALAR_TBL H WHERE U.ID_PAKET = P.PAKET_ID AND U.ID_HOCA = H.ID_HOCA; ";
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
                SqlCommand veriEkle = new SqlCommand("INSERT INTO UYE_TBL (U_AD,U_SOYAD,DOG_TAR,ID_PAKET,ID_HOCA,BAS_TAR,BIT_TAR) VALUES (@adi,@soyadi,@dogTar,@idPaket,@idHoca,@basTar,@bitTar)", bag);
                veriEkle.Parameters.AddWithValue("@adi", tbAd.Text);
                veriEkle.Parameters.AddWithValue("@soyadi", tbSoyad.Text);
                veriEkle.Parameters.AddWithValue("@dogTar", dtpDogumTar.Text.ToString());
                veriEkle.Parameters.AddWithValue("@idPaket", cbPaket.SelectedValue);
                veriEkle.Parameters.AddWithValue("@idHoca", cbHoca.SelectedValue);
                veriEkle.Parameters.AddWithValue("@basTar", dtpBaslangicTar.Text.ToString());
                veriEkle.Parameters.AddWithValue("@bitTar", dtpBitisTar.Text.ToString());
                veriEkle.ExecuteNonQuery();
                bag.Close();
                MessageBox.Show("Veri Eklendi !");
                textBoxlariTemizle();
                loadUyelerDG();
            }
            else
            {
                MessageBox.Show("Veri Ekleme Başarısız !");
            }
        }

        private void textBoxlariTemizle()
        {
            tbAd.Text = "";
            tbSoyad.Text = "";
            dtpDogumTar.Text = "";
            dtpBaslangicTar.Text = "";
            dtpBitisTar.Text = "";
            cbHoca.Text = "";
            cbPaket.Text = "";
        }

        private void dtpBitisTar_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbPaket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPaket.SelectedIndex >= 0)
            {
                int aySayisi = Convert.ToInt32(cbPaket.SelectedValue);
                string ayAdi = AyiYazdir(aySayisi);

                // dateTimePicker'ı güncelle
                dtpBitisTar.Value = DateTime.Now.AddMonths(aySayisi);
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            if (tbAramaBari.Text == "")
            {
                MessageBox.Show("Üye İsmini giriniz ve daha sonra arama yapınız !");
            }
            else
            {
                try
                {
                    bag.Open();

                    string sql = "SELECT U.U_ID, U.U_AD, U.U_SOYAD, P.ADI, H.H_ADI, U.BAS_TAR, U.BIT_TAR " +
                                 "FROM UYE_TBL U, PAKET_TBL P, HOCALAR_TBL H " +
                                 "WHERE U.ID_PAKET = P.PAKET_ID AND U.ID_HOCA = H.ID_HOCA AND U.U_AD = @UyeName;";

                    SqlCommand command = new SqlCommand(sql, bag);
                    command.Parameters.AddWithValue("@UyeName", tbAramaBari.Text);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    bag.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            loadUyelerDG();
        }
    }
}

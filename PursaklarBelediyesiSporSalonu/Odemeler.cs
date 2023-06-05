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
    public partial class Odemeler : Form
    {
        public Odemeler()
        {
            InitializeComponent();
        }

        SqlConnection bag = new SqlConnection("Data Source=DESKTOP-HC8OO1J\\MSSQLSERVER1;Initial Catalog=PursaklarSporSalonuDB;Integrated Security=True");
        private void Odemeler_Load(object sender, EventArgs e)
        {
            loadOdemeBilgileriDGV();
            loadUyeleriGetirDGV();
        }

        private void loadUyeleriGetirDGV()
        {
            string sql = "SELECT U.U_ID, U.U_AD, U.U_SOYAD, P.FIYAT FROM UYE_TBL U, PAKET_TBL P WHERE U.ID_PAKET = P.PAKET_ID";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }

        private void loadOdemeBilgileriDGV()
        {
            string sql = "SELECT O.ID_ODEME, U.U_AD, U.U_SOYAD, O.FIYAT, O.ODEME_TAR FROM ODEME_TBL O, UYE_TBL U WHERE O.UYE_ID = U.U_ID";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView2.DataSource = tablo;
        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            if (tbUyeNo.Text == "")
            {
                MessageBox.Show("Üye Numarasını giriniz ve daha sonra arama yapınız !");
            }
            else
            {
                try
                {
                    bag.Open();
                    string query = "SELECT U.U_ID, U.U_AD, U.U_SOYAD, P.FIYAT FROM UYE_TBL U, PAKET_TBL P WHERE U.ID_PAKET = P.PAKET_ID AND U_ID = @UYEID;";

                    SqlCommand command = new SqlCommand(query, bag);
                    command.Parameters.AddWithValue("@UYEID", tbUyeNo.Text);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        tbAd.Text = reader["U_AD"].ToString();
                        tbSoyad.Text = reader["U_SOYAD"].ToString();
                        lblToplamFiyat.Text = reader["FIYAT"].ToString(); 
                    }
                    else
                    {
                        MessageBox.Show("Üye bulunamadı.");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    bag.Close();
                }
            }
        }

        private void btnOde_Click(object sender, EventArgs e)
        {
            //Ödeme işlemi burda olacak.(Odeme Tablosuna veri girişi)

            if (tbUyeNo.Text == "" || tbAd.Text == "" || tbSoyad.Text == "" || dtpOdemeTar.Text == "" || lblToplamFiyat.Text == "00TL")
            {
                MessageBox.Show("Bilgileri eksiksiz doldur !");
            }
            else
            {
                try
                {
                    //Odeme Ekle
                    if (bag.State == ConnectionState.Closed)
                    {
                        bag.Open();
                        SqlCommand veriEkle = new SqlCommand("INSERT INTO ODEME_TBL (UYE_ID, FIYAT, ODEME_TAR, O_AD,O_SOYAD) VALUES (@prm1, @prm2,@prm3,@prm4,@prm5)", bag);
                        veriEkle.Parameters.AddWithValue("@prm1", int.Parse(tbUyeNo.Text.ToString()));
                        veriEkle.Parameters.AddWithValue("@prm2", int.Parse(lblToplamFiyat.Text.ToString()));
                        veriEkle.Parameters.AddWithValue("@prm3", dtpOdemeTar.Text.ToString());
                        veriEkle.Parameters.AddWithValue("@prm4", tbAd.Text.ToString());
                        veriEkle.Parameters.AddWithValue("@prm5", tbSoyad.Text.ToString());
                        veriEkle.ExecuteNonQuery();
                        bag.Close();
                        MessageBox.Show("Veri Eklendi !");
                        tbAd.Text = "";
                        tbSoyad.Text = "";
                        lblToplamFiyat.Text = "00 TL";
                        tbUyeNo.Text = "";
                        loadOdemeBilgileriDGV();
                    }
                    else
                    {
                        MessageBox.Show("Veri Ekleme Başarısız !");
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }


        }

        private void btnUyeAra_Click(object sender, EventArgs e)
        {

            if (tbUyeAraBar.Text == "")
            {
                MessageBox.Show("Üye İsmini giriniz ve daha sonra arama yapınız !");
            }
            else
            {
                try
                {
                    bag.Open();

                    string query = "SELECT U.U_ID, U.U_AD, U.U_SOYAD, P.FIYAT FROM UYE_TBL U, PAKET_TBL P WHERE U.ID_PAKET = P.PAKET_ID AND U_AD = @UyeName;";

                    SqlCommand command = new SqlCommand(query, bag);
                    command.Parameters.AddWithValue("@UyeName", tbUyeAraBar.Text);

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

        private void btnOdemeAra_Click(object sender, EventArgs e)
        {
            if (tbOdemeAraBar.Text == "")
            {
                MessageBox.Show("Üye İsmini giriniz ve daha sonra arama yapınız !");
            }
            else
            {
                try
                {
                    bag.Open();

                    string query = "SELECT O.ID_ODEME, U.U_AD, U.U_SOYAD, O.FIYAT, O.ODEME_TAR FROM ODEME_TBL O, UYE_TBL U WHERE O.UYE_ID = U.U_ID AND U.U_AD = @UyeName;";

                    SqlCommand command = new SqlCommand(query, bag);
                    command.Parameters.AddWithValue("@UyeName", tbOdemeAraBar.Text);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView2.DataSource = dataTable;

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
            loadUyeleriGetirDGV();
            loadOdemeBilgileriDGV();
        }
    }
}

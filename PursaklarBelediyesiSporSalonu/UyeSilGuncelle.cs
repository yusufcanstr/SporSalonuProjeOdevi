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
    public partial class UyeSilGuncelle : Form
    {
        public UyeSilGuncelle()
        {
            InitializeComponent();
        }

        SqlConnection bag = new SqlConnection("Data Source=DESKTOP-HC8OO1J\\MSSQLSERVER1;Initial Catalog=PursaklarSporSalonuDB;Integrated Security=True");


        private void UyeSilGuncelle_Load(object sender, EventArgs e)
        {
            loadUyelerDG();
            hocaIsimleriCbYukle();
            paketIsimleriCbYukle();
        }

        private void loadUyelerDG()
        {
            string sql = "SELECT U.U_ID, U.U_AD, U.U_SOYAD,U.DOG_TAR, P.ADI , H.H_ADI, U.BAS_TAR, U.BIT_TAR FROM UYE_TBL U, PAKET_TBL P, HOCALAR_TBL H WHERE U.ID_PAKET = P.PAKET_ID AND U.ID_HOCA = H.ID_HOCA; ";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
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

        int key = 0;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if(tbAd.Text == "" || tbSoyad.Text == "" || dtpDogumTar.Text == "" || cbPaket.Text == "" || cbHoca.Text == "" || dtpBaslangicTar.Text == "" || dtpBitisTar.Text == "")
            {
                MessageBox.Show("Güncellenecek üyeyi seçiniz !");
            }
            else
            {
                try
                {
                    bag.Open();
                    string sql = "UPDATE UYE_TBL SET U_AD=@prm1,U_SOYAD=@prm2,DOG_TAR= @prm3,ID_PAKET= @prm4,ID_HOCA=@prm5,BAS_TAR=@prm6, BIT_TAR= @prm7 WHERE U_ID=" + tbUyeNo.Text + ";";
                    SqlCommand komut = new SqlCommand(sql, bag);
                    komut.Parameters.AddWithValue("@prm1", tbAd.Text);
                    komut.Parameters.AddWithValue("@prm2", tbSoyad.Text);
                    komut.Parameters.AddWithValue("@prm3", dtpDogumTar.Text);
                    komut.Parameters.AddWithValue("@prm4", cbPaket.SelectedValue);
                    komut.Parameters.AddWithValue("@prm5", cbHoca.SelectedValue);
                    komut.Parameters.AddWithValue("@prm6", dtpBaslangicTar.Text);
                    komut.Parameters.AddWithValue("@prm7", dtpBitisTar.Text);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Üye Başarıyla Güncellendi.");
                    bag.Close();
                    textBoxlariTemizle();
                    loadUyelerDG();
                    
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message.ToString());
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

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
            tbUyeNo.Text = "";
        }

        private void btnIdBul_Click(object sender, EventArgs e)
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
                    string query = "SELECT U.U_ID, U.U_AD, U.U_SOYAD, U.DOG_TAR, P.ADI, H.H_ADI, U.BAS_TAR, U.BIT_TAR " +
                                   "FROM UYE_TBL U, PAKET_TBL P, HOCALAR_TBL H " +
                                   "WHERE U.ID_PAKET = P.PAKET_ID AND U.ID_HOCA = H.ID_HOCA AND U.U_ID = @UYEID";

                    SqlCommand command = new SqlCommand(query, bag);
                    command.Parameters.AddWithValue("@UYEID", tbUyeNo.Text);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        tbAd.Text = reader["U_AD"].ToString();
                        tbSoyad.Text = reader["U_SOYAD"].ToString();
                        cbPaket.Text = reader["ADI"].ToString();
                        cbHoca.Text = reader["H_ADI"].ToString();
                        dtpBaslangicTar.Value = Convert.ToDateTime(reader["BAS_TAR"]);
                        dtpBitisTar.Value = Convert.ToDateTime(reader["BIT_TAR"]);
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


        private void btnSil_Click(object sender, EventArgs e)
        {
            if(tbUyeNo.Text == "")
            {
                MessageBox.Show("Üye numarasını giriniz !");
            }else
            {
                try
                {
                    bag.Open();

                    string sql = "DELETE FROM UYE_TBL WHERE U_ID = @UyeID;";

                    SqlCommand command = new SqlCommand(sql, bag);
                    command.Parameters.AddWithValue("@UyeID", tbUyeNo.Text);

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Üye başarılı bir şekilde silindi !");
                        loadUyelerDG();
                        textBoxlariTemizle();
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi başarısız oldu !");
                    }
                    bag.Close();

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
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

                    string query = "SELECT U.U_ID, U.U_AD, U.U_SOYAD, U.DOG_TAR, P.ADI, H.H_ADI, U.BAS_TAR, U.BIT_TAR " +
                                   "FROM UYE_TBL U, PAKET_TBL P, HOCALAR_TBL H " +
                                   "WHERE U.ID_PAKET = P.PAKET_ID AND U.ID_HOCA = H.ID_HOCA AND U.U_AD = @UyeName";

                    SqlCommand command = new SqlCommand(query, bag);
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

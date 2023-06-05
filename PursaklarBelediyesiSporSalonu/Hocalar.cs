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
    public partial class Hocalar : Form
    {
        public Hocalar()
        {
            InitializeComponent();
        }

        SqlConnection bag = new SqlConnection("Data Source=DESKTOP-HC8OO1J\\MSSQLSERVER1;Initial Catalog=PursaklarSporSalonuDB;Integrated Security=True");
        
        private void Hocalar_Load(object sender, EventArgs e)
        {
            loadHocalar();
            alanIsimleriCbYukle();
        }

        private void loadHocalar()
        {
            string sql = "SELECT H.ID_HOCA, H.H_ADI, H.H_SOYADI, A.ALAN_ADI, H.TEL_NO, H.H_EMAIL, H.H_MAAS FROM ALAN_TBL A, HOCALAR_TBL H WHERE H.ID_ALAN = A.ALAN_ID;";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }

        private void alanIsimleriCbYukle()
        {
            DataTable tablo = new DataTable();
            string sql = "SELECT * FROM ALAN_TBL";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            da.Fill(tablo);
            cbAlan.ValueMember = "ALAN_ID";
            cbAlan.DisplayMember = "ALAN_ADI";
            cbAlan.DataSource = tablo;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            //Silme İşlemi hata var düzelteceğim
            if(tbUyeNo.Text == "")
            {
                MessageBox.Show("Hocanın id'sini giriniz !");
            }else
            {
                try
                {
                    bag.Open();
                    string sql = "DELETE FROM HOCALAR_TBL WHERE ID_HOCA = '" + tbUyeNo.Text + "'";
                    SqlCommand komut = new SqlCommand(sql, bag);
                    komut.ExecuteNonQuery();
                    bag.Close();
                    MessageBox.Show("Hoca Silindi !");
                    loadHocalar();
                    textBoxlariTemizle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bilinmedik bir hata");
                }
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (bag.State == ConnectionState.Closed)
            {
                bag.Open();
                SqlCommand veriEkle = new SqlCommand("INSERT INTO HOCALAR_TBL (H_ADI, H_SOYADI, ID_ALAN, TEL_NO, H_EMAIL, H_MAAS) VALUES (@adi, @soyadi, @idAlani, @telNo, @email, @maas)", bag);
                veriEkle.Parameters.AddWithValue("@adi", tbAd.Text);
                veriEkle.Parameters.AddWithValue("@soyadi", tbSoyad.Text);
                veriEkle.Parameters.AddWithValue("@idAlani", cbAlan.SelectedValue); 
                veriEkle.Parameters.AddWithValue("@telNo", tbTelNo.Text);
                veriEkle.Parameters.AddWithValue("@email", tbEmail.Text);
                veriEkle.Parameters.AddWithValue("@maas", int.Parse(tbMaas.Text));
                veriEkle.ExecuteNonQuery();
                bag.Close();
                MessageBox.Show("Veri Eklendi !");
                textBoxlariTemizle();
                loadHocalar();
            }
            else
            {
                MessageBox.Show("Veri Ekleme Başarısız !");
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tbAd.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            tbSoyad.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            tbTelNo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            tbEmail.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void textBoxlariTemizle()
        {
            tbAd.Text = "";
            tbSoyad.Text = "";
            tbEmail.Text = "";
            tbMaas.Text = "";
            tbTelNo.Text = "";
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if(tbUyeNo.Text == "")
            {
                MessageBox.Show("Hocanın id'sini giriniz !");
            }else
            {
                if(tbAd.Text == "" || tbSoyad.Text == "" || tbEmail.Text == "" || tbMaas.Text == "" || tbEmail.Text == "" || tbTelNo.Text == "")
                {
                    MessageBox.Show("Hoca bilgileri eksik onları doldurunuz !");
                }else
                {

                    //Güncelleme İŞlemi Yapılacak
                    try
                    {
                        bag.Open();
                        string sql = "UPDATE HOCALAR_TBL SET H_ADI=@prm1,H_SOYADI=@prm2, ID_ALAN= @prm3,TEL_NO= @prm4,H_EMAIL=@prm5,H_MAAS=@prm6 WHERE ID_HOCA=" + tbUyeNo.Text + ";";
                        SqlCommand komut = new SqlCommand(sql, bag);
                        komut.Parameters.AddWithValue("@prm1", tbAd.Text);
                        komut.Parameters.AddWithValue("@prm2", tbSoyad.Text);
                        komut.Parameters.AddWithValue("@prm3", cbAlan.SelectedValue);
                        komut.Parameters.AddWithValue("@prm4", tbTelNo.Text);
                        komut.Parameters.AddWithValue("@prm5", tbEmail.Text);
                        komut.Parameters.AddWithValue("@prm6", tbMaas.Text);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Üye Başarıyla Güncellendi.");
                        bag.Close();
                        textBoxlariTemizle();
                        loadHocalar();

                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message.ToString());
                    }


                }
            }
        }

        private void btnIdBul_Click(object sender, EventArgs e)
        {
            if (tbUyeNo.Text == "")
            {
                MessageBox.Show("Hoca idsini giriniz ve daha sonra arama yapınız !");
            }
            else
            {
                try
                {
                    bag.Open();
                    string query = "SELECT H.ID_HOCA, H.H_ADI, H.H_SOYADI, A.ALAN_ADI, H.TEL_NO, H.H_EMAIL, H.H_MAAS FROM ALAN_TBL A, HOCALAR_TBL H WHERE H.ID_ALAN = A.ALAN_ID AND ID_HOCA=@PRMUYEID;";

                    SqlCommand command = new SqlCommand(query, bag);
                    command.Parameters.AddWithValue("@PRMUYEID", tbUyeNo.Text);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        tbAd.Text = reader["H_ADI"].ToString();
                        tbSoyad.Text = reader["H_SOYADI"].ToString();
                        cbAlan.Text = reader["ALAN_ADI"].ToString();
                        tbTelNo.Text = reader["TEL_NO"].ToString();
                        tbEmail.Text = reader["H_EMAIL"].ToString();
                        tbMaas.Text = reader["H_MAAS"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Hoca bulunamadı.");
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

        private void btnListele_Click(object sender, EventArgs e)
        {
            loadHocalar();
        }
    }
}

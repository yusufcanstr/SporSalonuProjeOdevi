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
    public partial class Dersler : Form
    {
        public Dersler()
        {
            InitializeComponent();
        }

        SqlConnection bag = new SqlConnection("Data Source=DESKTOP-HC8OO1J\\MSSQLSERVER1;Initial Catalog=PursaklarSporSalonuDB;Integrated Security=True");

        private void Dersler_Load(object sender, EventArgs e)
        {
            loadDersler();
            loadAlanlar();
            loadComboBoxHocalar();
        }

        private void loadDersler()
        {
            string sql = "SELECT D.ID_DERS, D.DERS_ADI, H.H_ADI FROM DERS_TBL D, HOCALAR_TBL H WHERE D.ID_HOCA = H.ID_HOCA";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }

        private void loadAlanlar()
        {
            string sql = "SELECT * FROM ALAN_TBL";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView2.DataSource = tablo;
        }

        private void loadComboBoxHocalar()
        {
            DataTable tablo = new DataTable();
            string sql = "SELECT * FROM HOCALAR_TBL";
            SqlDataAdapter da = new SqlDataAdapter(sql, bag);
            da.Fill(tablo);
            cbHoca.ValueMember = "ID_HOCA";
            cbHoca.DisplayMember = "H_ADI";
            cbHoca.DataSource = tablo;
        }

        private void textBoxlariTemizle()
        {
            tbDersAdi.Text = "";
            tbAlanAdi.Text = "";
            tbDersId.Text = "";
            tbAlanId.Text = "";
        }

        private void btnDersEkle_Click(object sender, EventArgs e)
        {
            if(tbDersAdi.Text == "" || cbHoca.Text == "")
            {
                MessageBox.Show("Üye ID ve dersin adını giriniz !");
            }else
            {
                try
                {
                    //Ders Ekleme
                    if (bag.State == ConnectionState.Closed)
                    {
                        bag.Open();
                        SqlCommand veriEkle = new SqlCommand("INSERT INTO DERS_TBL (DERS_ADI, ID_HOCA) VALUES (@prm1, @prm2)", bag);
                        veriEkle.Parameters.AddWithValue("@prm1", tbDersAdi.Text);
                        veriEkle.Parameters.AddWithValue("@prm2", cbHoca.SelectedValue);
                        veriEkle.ExecuteNonQuery();
                        bag.Close();
                        MessageBox.Show("Veri Eklendi !");
                        textBoxlariTemizle();
                        loadDersler();
                    }
                    else
                    {
                        MessageBox.Show("Veri Ekleme Başarısız !");
                    }

                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void btnAlanEkle_Click(object sender, EventArgs e)
        {
            if (tbAlanAdi.Text == "")
            {
                MessageBox.Show("Alan adını giriniz !");
            }
            else
            {
                try
                {
                    //Alan Ekleme
                    if (bag.State == ConnectionState.Closed)
                    {
                        bag.Open();
                        SqlCommand veriEkle = new SqlCommand("INSERT INTO ALAN_TBL (ALAN_ADI) VALUES (@prm1)", bag);
                        veriEkle.Parameters.AddWithValue("@prm1", tbAlanAdi.Text);
                        veriEkle.ExecuteNonQuery();
                        bag.Close();
                        MessageBox.Show("Veri Eklendi !");
                        textBoxlariTemizle();
                        loadAlanlar();
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

        private void btnDersListele_Click(object sender, EventArgs e)
        {
            loadDersler();
        }

        private void btnAlanListele_Click(object sender, EventArgs e)
        {
            loadAlanlar();
        }

        private void btnDersSil_Click(object sender, EventArgs e)
        {
            if (tbDersId.Text == "")
            {
                MessageBox.Show("Silinecek dersin id sini giriniz !");
            }else
            {
                try
                {
                    bag.Open();
                    string sql = "DELETE FROM DERS_TBL WHERE ID_DERS = '" + tbDersId.Text + "'";
                    SqlCommand komut = new SqlCommand(sql, bag);
                    komut.ExecuteNonQuery();
                    bag.Close();
                    MessageBox.Show("Ders Silindi !");
                    loadDersler();
                    textBoxlariTemizle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bilinmedik bir hata");
                }
            }
        }

        private void btnAlanSil_Click(object sender, EventArgs e)
        {
            if (tbAlanAdi.Text == "")
            {
                MessageBox.Show("Silinecek alanın ismini giriniz !");
            }
            else
            {
                try
                {
                    bag.Open();
                    string sql = "DELETE FROM ALAN_TBL WHERE ALAN_ADI = '" + tbAlanAdi.Text + "'";
                    SqlCommand komut = new SqlCommand(sql, bag);
                    komut.ExecuteNonQuery();
                    bag.Close();
                    MessageBox.Show("Bu Alan Silindi !");
                    loadDersler();
                    textBoxlariTemizle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bilinmedik bir hata");
                }
            }
        }

        private void btnDersGuncelle_Click(object sender, EventArgs e)
        {
            //Dersi Güncelleme İşlemleri
            if (tbDersId.Text == "")
            {
                MessageBox.Show("Dersin id'sini giriniz !");
            }
            else
            {
                if (tbDersAdi.Text == "" || cbHoca.Text == "" )
                {
                    MessageBox.Show("Ders bilgileri eksik olanları doldurunuz !");
                }
                else
                {

                    //Güncelleme İşlemi Yapılacak
                    try
                    {
                        bag.Open();
                        string sql = "UPDATE DERS_TBL SET DERS_ADI=@prm1, ID_HOCA=@prm2k WHERE ID_DERS=" + tbDersId.Text + ";";
                        SqlCommand komut = new SqlCommand(sql, bag);
                        komut.Parameters.AddWithValue("@prm1", tbDersAdi.Text);
                        komut.Parameters.AddWithValue("@prm2", cbHoca.SelectedValue);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Ders Başarıyla Güncellendi.");
                        bag.Close();
                        textBoxlariTemizle();
                        loadDersler();

                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message.ToString());
                    }


                }
            }
        }

        private void btnAlanGuncelle_Click(object sender, EventArgs e)
        {
            // Alan ismini güncelleme işlemleri
            if (tbAlanId.Text == "")
            {
                MessageBox.Show("Alan id'sini giriniz !");
            }
            else
            {
                if (tbAlanAdi.Text == "")
                {
                    MessageBox.Show("Alan adını gir !");
                }
                else
                {

                    //Güncelleme İşlemi Yapılacak
                    try
                    {
                        bag.Open();
                        string sql = "UPDATE ALAN_TBL SET ALAN_ADI=@prm1 WHERE ALAN_ID=" + tbAlanId.Text + ";";
                        SqlCommand komut = new SqlCommand(sql, bag);
                        komut.Parameters.AddWithValue("@prm1", tbAlanAdi.Text);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Alan ismi Başarıyla Güncellendi.");
                        bag.Close();
                        textBoxlariTemizle();
                        loadDersler();

                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message.ToString());
                    }


                }
            }
        }
    }
}

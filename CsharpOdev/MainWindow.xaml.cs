using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CsharpOdev
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> memleketler = new List<string>() {
                  "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Aksaray", "Amasya", "Ankara", "Antalya", "Ardahan",
    "Artvin", "Aydın", "Balıkesir", "Bartın", "Batman", "Bayburt", "Bilecik", "Bingöl", "Bitlis", "Bolu",
    "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Düzce", "Edirne", "Elazığ",
    "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkâri", "Hatay", "Iğdır", "Isparta",
    "İstanbul", "İzmir", "Kahramanmaraş", "Karabük", "Karaman", "Kars", "Kastamonu", "Kayseri", "Kırıkkale", "Kırklareli",
    "Kırşehir", "Kilis", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Mardin", "Mersin", "Muğla",
    "Muş", "Nevşehir", "Niğde", "Ordu", "Osmaniye", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop",
    "Sivas", "Şanlıurfa", "Şırnak", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak"
            };
            cmbMemleket.ItemsSource = memleketler;

            // Huy ComboBox'ı için liste oluşturma
            List<string> huy = new List<string>() { "Çalışkan", "Girişken", "Sakin" };
            cmbHuy.ItemsSource = huy;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=SELÇUK;Initial Catalog=KislerDb;Integrated Security=True");
            con.Open();
            int yas;
            // Ad alanı boş ise hata mesajı ver
            if (!Regex.IsMatch(tb1.Text, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Lütfen ad alanına sadece harf girin!");
                return;
            }

            // Yas alanına sadece sayı girildiğinden emin ol
            
           else   if (!int.TryParse(tb4.Text, out yas))
            {
                MessageBox.Show("Lütfen yaş alanına sadece sayı girin!");
                return;
            }
            else if (cmbHuy.SelectedItem == null || cmbMemleket.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            SqlCommand cmd = new SqlCommand("insert into Kisiler values(@Ad,@Huy,@Memleket,@Yas)", con);
            cmd.Parameters.AddWithValue("@Ad", tb1.Text);
            cmd.Parameters.AddWithValue("@Huy", cmbHuy.SelectedValue);
            cmd.Parameters.AddWithValue("@Memleket", cmbMemleket.SelectedValue);
            cmd.Parameters.AddWithValue("@Yas", yas);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Mükemmel bir ekleme!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb1.Text) || string.IsNullOrEmpty(tb4.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            SqlConnection con = new SqlConnection("Data Source=SELÇUK;Initial Catalog=KislerDb;Integrated Security=True");
            con.Open();
            int yas;
            // Ad alanı boş ise hata mesajı ver
            if (!Regex.IsMatch(tb1.Text, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Lütfen ad alanına sadece harf girin!");
                return;
            }

            // Yas alanına sadece sayı girildiğinden emin ol
            
            else if (!int.TryParse(tb4.Text, out yas))
            {
                MessageBox.Show("Lütfen yaş alanına sadece sayı girin!");
                return;
            }

            else if (cmbHuy.SelectedItem == null || cmbMemleket.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            SqlCommand cmd = new SqlCommand("update Kisiler set Huy=@Huy, Memleket=@Memleket, Yas=@Yas where Ad=@Ad", con);
            cmd.Parameters.AddWithValue("@Ad", tb1.Text);
            cmd.Parameters.AddWithValue("@Yas", yas);
            cmd.Parameters.AddWithValue("@Huy", cmbHuy.SelectedValue);
            cmd.Parameters.AddWithValue("@Memleket", cmbMemleket.SelectedValue);
            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Kayıt başarıyla güncellendi!");
            }
            else
            {
                MessageBox.Show("Güncelleme işlemi başarısız oldu!");
            }
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=SELÇUK;Initial Catalog=KislerDb;Integrated Security=True");
            con.Open();

            string ad = tb1.Text.Trim(); // boşlukları temizle
            if (string.IsNullOrEmpty(ad))
            {
                MessageBox.Show("Lütfen silinecek kaydın adını girin.");
                return;
            }

            SqlCommand cmd = new SqlCommand("select * from Kisiler where Ad = @Ad", con);
            cmd.Parameters.AddWithValue("@Ad", ad);

            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                MessageBox.Show("Silinecek veri bulunamadı!");
                reader.Close();
                con.Close();
                return;
            }
            reader.Close();

            cmd = new SqlCommand("delete from Kisiler where Ad = @Ad", con);
            cmd.Parameters.AddWithValue("@Ad", ad);

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Kayıt başarıyla silindi!");
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız oldu!");
            }

        }

      private void Button_Click_3(object sender, RoutedEventArgs e)
{
            SqlConnection con = new SqlConnection("Data Source=SELÇUK;Initial Catalog=KislerDb;Integrated Security=True");
            con.Open();

            SqlCommand cmd = new SqlCommand("select * from Kisiler", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataG.ItemsSource = dt.DefaultView;

        }
        private void SearchBt_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=SELÇUK;Initial Catalog=KislerDb;Integrated Security=True");
            con.Open();

            string searchText = "";
            if (!string.IsNullOrEmpty(tb1.Text) && !string.IsNullOrEmpty(tb4.Text))
            {
                MessageBox.Show("Lütfen sadece bir arama terimi girin.");
                return;
            }
            else if (!string.IsNullOrEmpty(tb1.Text))
            {
                searchText = tb1.Text;
            }
            else if (!string.IsNullOrEmpty(tb4.Text))
            {
                searchText = tb4.Text;
            }
            else
            {
                MessageBox.Show("Lütfen bir arama terimi girin.");
                return;
            }

            SqlCommand cmd = new SqlCommand("select * from Kisiler where Ad like '%' + @searchText + '%' or Yas like '%' + @searchText + '%'", con);

            cmd.Parameters.AddWithValue("@searchText", searchText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Aradığınız kriterlere uygun kayıt bulunamadı.");
            }

            dataG.ItemsSource = dt.DefaultView;
        }


    }




}

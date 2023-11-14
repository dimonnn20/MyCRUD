using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

namespace MyCRUD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection _sqlConnection = new SqlConnection(@"Data Source=DESKTOP-NNO1SAJ\SQLEXPRESS;Initial Catalog=NewDB;Integrated Security=True");
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM people", _sqlConnection);
            DataTable dt = new DataTable();
            _sqlConnection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            _sqlConnection.Close();
            datagrid.ItemsSource = dt.DefaultView;

        }

        private void ClearData()
        {
            name_txt.Clear();
            age_txt.Clear();
            gender_txt.Clear();
            city_txt.Clear();

        }

        private bool IsValid()
        {
            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == string.Empty)
            {
                MessageBox.Show("Age is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == string.Empty)
            {
                MessageBox.Show("Gender is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (city_txt.Text == string.Empty)
            {
                MessageBox.Show("City is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO people (name,age,gender,city) VALUES (@Name,@Age,@Gender,@City);", _sqlConnection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("Age", age_txt.Text);
                    cmd.Parameters.AddWithValue("Gender", gender_txt.Text);
                    cmd.Parameters.AddWithValue("City", city_txt.Text);
                    _sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    _sqlConnection.Close();
                    LoadGrid();
                    ClearData();
                    MessageBox.Show("Inserted", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                    SqlCommand cmd = new SqlCommand("DELETE FROM people WHERE id = @Id;", _sqlConnection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("Id", id_txt.Text);
                    _sqlConnection.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res == 0) { throw new Exception("No such Id in DB"); }
                    LoadGrid();
                    MessageBox.Show($"Deleted {res} line(s)", "Done", MessageBoxButton.OK, MessageBoxImage.Information);

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _sqlConnection.Close();
                id_txt.Text = string.Empty;
            }
        }
    }
}
    

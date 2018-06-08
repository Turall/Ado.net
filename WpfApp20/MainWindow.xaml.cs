using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace WpfApp20
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Students std = new Students();
        Teachers tch = new Teachers();
        public MainWindow()
        {
            InitializeComponent();
            UsersBox.Items.Add("Students");
            UsersBox.Items.Add("Teachers");
        }
        public static void Connect(DataGrid data, string info)
        {
            string con = @"Data Source=DESKTOP-VUKJ5T5\TURAL;Initial Catalog=Library;Integrated Security=True; ";
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from " + info, connection);
                SqlDataReader read = cmd.ExecuteReader();
                DataTable table = Data(read);
                ShowData(table, data);
            }
        }
        public static DataTable Data(SqlDataReader reader)
        {
            DataTable table = new DataTable("Info");
            table.Load(reader);
            return table;
        }

        public static void ShowData(DataTable data, DataGrid grid)
        {
            grid.ItemsSource = data.DefaultView;
        }



        private void UsersBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = UsersBox.SelectedItem as string;
            Connect(data, str);

        }


        private void data_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            var selected = e.Column;

            DataRowView rowView = data.SelectedItem as DataRowView;
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                int index = data.CurrentCell.Column.DisplayIndex;
                string CellValue = rowView.Row.ItemArray[index].ToString();
                CellValue = ((TextBox)e.EditingElement).Text as string;
                if (rowView.Row.ItemArray[0] is int)
                {
                    int ID = (int)rowView.Row.ItemArray[0];
                    string con = @"Data Source = DESKTOP-VUKJ5T5\TURAL;Initial Catalog= Library; Integrated Security = true; ";

                    using (SqlConnection connection = new SqlConnection(con))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("Update " + (string)UsersBox.SelectedItem + " Set " + selected.Header + "= '" +
                      CellValue + "' where id=" + ID, connection);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
        }


        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = data.SelectedItem as DataRowView;


            if (rowView.Row.ItemArray[0] is int)
            {
                int ID = (int)rowView.Row.ItemArray[0];
                string con = @"Data Source = DESKTOP-VUKJ5T5\TURAL;Initial Catalog= Library; Integrated Security = true; ";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Delete  From " + (string)UsersBox.SelectedItem +
                  " where id= " + ID, connection);
                    cmd.ExecuteNonQuery();
                    data.Items.Refresh();
                }
               

            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {


            DataRowView datarow = data.SelectedItem as DataRowView;
            string con = @"Data Source =DESKTOP-VUKJ5T5\TURAL; Initial Catalog = Library; Integrated security = true;";
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                if (UsersBox.SelectedItem.Equals("Students"))
                {
                    std.ID = (int)datarow[0];
                    std.FirstName = datarow[1] as string;
                    std.LastName = datarow[2] as string;
                    std.Id_Group = (int)datarow[3];
                    std.Term = (int)datarow[4];
                    SqlCommand cmd = new SqlCommand("Insert into Students(ID,Firstname,lastname,id_group,term) " +
                        "values(" + std.ID + ",'" + std.FirstName + "','" + std.LastName + "'," + std.Id_Group + "," + std.Term + ")", connection);
                    cmd.ExecuteNonQuery();
                    data.Items.Refresh();
                }
                else if (UsersBox.SelectedItem.Equals("Teachers"))
                {
                    tch.ID = (int)datarow[0];
                    tch.FirstName = datarow[1] as string;
                    tch.LastName = datarow[2] as string;
                    tch.Id_Dep = (int)datarow[3];
                    SqlCommand cmd = new SqlCommand("Insert into Teachers(ID,Firstname,lastname,id_dep) " +
                         "values(" + tch.ID + ",'" + tch.FirstName + "','" + tch.LastName + "'," + tch.Id_Dep  + ")", connection);
                    cmd.ExecuteNonQuery();
                    data.Items.Refresh();
                }

            }

        }
    }
}

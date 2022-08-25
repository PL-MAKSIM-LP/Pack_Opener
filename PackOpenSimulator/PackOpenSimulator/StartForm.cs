using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PackOpenSimulator
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlController sqlController = new SqlController();
          

            SqlDataReader sqlDataReader = sqlController.GetUser(textBox1.Text);

            try
            {               
                if (sqlDataReader.Read())
                {
                    PlayerData.playerLogin = Convert.ToString(sqlDataReader["Login"]);
                    PlayerData.playerCoins = Convert.ToInt32(sqlDataReader["Coin"]);
                    PlayerData.playerDust = Convert.ToInt32(sqlDataReader["Dust"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                    sqlDataReader.Close();
            }

            sqlController.CreateTable(textBox1.Text);

            MainMenuForm mainMenuform = new MainMenuForm();
            mainMenuform.Show();
        }
    }
}

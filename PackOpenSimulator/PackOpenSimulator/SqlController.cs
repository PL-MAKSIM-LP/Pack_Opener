using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PackOpenSimulator
{
    internal class SqlController
    {  
        public SqlDataReader ExecuteComand(string comand)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(comand, sqlConnection);
            return sqlCommand.ExecuteReader();
        }
        public SqlDataReader GetUser(string login)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            sqlConnection.Open();
            string comand = $"IF NOT EXISTS(SELECT 1 FROM Persons WHERE Login Like '{login}') " + @" BEGIN" + $" INSERT INTO Persons(Login, Dust, Coin) VALUES('{login}', 0, 0) END " + $"Select * from Persons where Login Like '{login}'";
            SqlCommand sqlCommand = new SqlCommand(comand, sqlConnection);
            return sqlCommand.ExecuteReader();
        }

        public void CreateTable(string login)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            sqlConnection.Open();
            string comand = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{login}_Cards' and xtype='U') CREATE TABLE {login}" + "_Cards( Card_Id int, Card_Count int)";
            SqlCommand sqlCommand = new SqlCommand(comand, sqlConnection);
            sqlCommand.ExecuteReader();
        }

        public void SetValue(string colums, int value)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            sqlConnection.Open();
            string comand = $"UPDATE Persons SET {colums} = {value} WHERE Login Like '{PlayerData.playerLogin}';";
            SqlCommand sqlCommand = new SqlCommand(comand, sqlConnection);
            sqlCommand.ExecuteReader();
        }

        public int GetCartCountValue(string playerLogin, int id)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            sqlConnection.Open();

            string comand = $"Select Card_Count from {playerLogin}_Cards WHERE Card_Id  = {id};";
            SqlCommand sqlCommand = new SqlCommand(comand, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            int count = 0; 
            try
            {
                while (sqlDataReader.Read())
                {
                    count = Convert.ToInt32(sqlDataReader["Card_Count"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                    sqlDataReader.Close();
            }

            return count;
        }

   
        public void AddCard(int id)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            sqlConnection.Open();

            string comand = $"if not exists(Select* from {PlayerData.playerLogin}_Cards where Card_Id = {id}) Insert Into {PlayerData.playerLogin}_Cards(Card_Id, Card_Count) Values({id}, 1) Else update {PlayerData.playerLogin}_Cards set Card_Count = ({PlayerData.playerLogin}_Cards.Card_Count + 1) Where Card_Id = {id}";

            SqlCommand sqlCommand = new SqlCommand(comand, sqlConnection);
            sqlCommand.ExecuteReader();
        }

        public SqlDataReader GetOneCard(string rarity, string packName)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
            sqlConnection.Open();

            string comand = $"SELECT TOP 1 * FROM AllCards where Rarity Like '{rarity}' and Card_Set Like '{packName}'  ORDER BY NEWID();";

            SqlCommand sqlCommand = new SqlCommand(comand, sqlConnection);
            return sqlCommand.ExecuteReader();
        }

    }
}

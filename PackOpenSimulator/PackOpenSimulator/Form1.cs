using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System;

namespace PackOpenSimulator
{
    public partial class Form1 : Form
    {  
        private int packPrice = 0;
        string packName = null;

        private Form currentForm = null;

        public Form1(int packPrice, string packName)
        {
            InitializeComponent();
            this.packName = packName;
            this.packPrice = packPrice;
        }

        private void DrawForm()
        {
            Label label = new Label();
            label.Location = new Point(160, 20);
            label.Text = "Coins: ";
            label.Width = TextRenderer.MeasureText(label.Text, label.Font).Width;
            currentForm.Controls.Add(label);

            Label coinsLabel = new Label();
            coinsLabel.Location = new Point(200, 20);
            coinsLabel.Text = PlayerData.playerCoins.ToString();
            currentForm.Controls.Add(coinsLabel);

            Button button = new Button();
            button.Location = new Point(0, 0);
            button.Size = new Size(50, 100);
            button.MouseUp += new MouseEventHandler(button2_Click);
            button.Text = "Open Pack";

            currentForm.Controls.Add(button);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            currentForm = Application.OpenForms["Form1"];
            DrawForm();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            DrawForm();       

            if (!PacksController.BuyPack(packPrice))            
                return;

            SqlController sqlController = new SqlController();
            sqlController.SetValue("Coin", PlayerData.playerCoins);

            Random rnd = new Random();
            String[] cardPath = new String[5];

            for (int j = 0; j < 5; j++)
            {
                SqlDataReader sqlDataReader = null;

                try
                {             
                    int i = 0;
                    i = rnd.Next(1, 101);

                    string rarity = null; ;
                    if (j != 4)
                    {
                        rarity = PacksController.GetRarityCard(i, "Common");
                    }
                    else
                    {
                        rarity = PacksController.GetRarityCard(i, "Rare");
                    }

                    sqlDataReader = sqlController.GetOneCard(rarity, packName);

                    while (sqlDataReader.Read())
                    {
                        cardPath[j] = Convert.ToString(sqlDataReader["Card_Image"]);
                        int cardCount = sqlController.GetCartCountValue(PlayerData.playerLogin, Convert.ToInt32(sqlDataReader["Id"]));
                        if (cardCount < 5)
                        {
                            sqlController.AddCard(Convert.ToInt32(sqlDataReader["Id"]));
                        }
                        else 
                        {
                            PlayerData.playerDust += PacksController.GetDust(Convert.ToString(sqlDataReader["Rarity"]));
                            sqlController.SetValue("Dust", PlayerData.playerDust);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if(sqlDataReader != null && !sqlDataReader.IsClosed)
                        sqlDataReader.Close();
                }
            }
            DrawCards(currentForm, cardPath);
        }

        private void DrawCards(Form form, string[] imagePath)
        {
            String[] images = imagePath;
            for (int j = 0; j < 5; j++)
            {
                ButtonControler buttonControler = new ButtonControler();
                Button button = new Button();
                button = new Button();
                button.Location = new Point(j * 130, 150);
                button.Size = new Size(128, 172);
                button.FlatAppearance.BorderSize = 0;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.BackgroundImage = new Bitmap(Constants.projectDir + images[j]);
                button.MouseEnter += buttonControler.FocusedButton;
                button.MouseLeave += buttonControler.NoFocusedButton;
                form.Controls.Add(button);
            }
        }       
    }
}

using System;
using System.Windows.Forms;

namespace PackOpenSimulator
{
    public partial class MainMenuForm : Form
    {       
        public MainMenuForm()
        {           
            InitializeComponent();
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = new Form1(PacksController.corePackPrice, PacksController.packName);
            form.Show();
        }

        void Init()
        {
            label4.Text = PlayerData.playerLogin.ToString();
            label5.Text = PlayerData.playerCoins.ToString();
            label6.Text = PlayerData.playerDust.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PlayerData.playerCoins += 1000;
            SqlController sqlController = new SqlController();
            sqlController.SetValue("Coin", PlayerData.playerCoins);
            label5.Text = PlayerData.playerCoins.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CollectionForm collectionForm = new CollectionForm();
            collectionForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CraftingForm craftingForm = new CraftingForm();
            craftingForm.Show();
        }
    }
}

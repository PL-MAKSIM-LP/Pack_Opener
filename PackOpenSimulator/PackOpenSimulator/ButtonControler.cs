using System.Windows.Forms;

namespace PackOpenSimulator
{
    internal class ButtonControler
    {
        CardForm cardform;

        public string rarity = null;
        public int cardId;
        public void FocusedButton(object sender, System.EventArgs e)
        {
            Button button = sender as Button;
            cardform = new CardForm(button.BackgroundImage);
            cardform.Show();
        }

        public void NoFocusedButton(object sender, System.EventArgs e)
        {
            cardform.Close();
        }

        public void DustCard(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DialogResult dialogResult = MessageBox.Show("Dust card", "Do you want dust card?",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Cancel)
                    return;
                
                SqlController sqlController = new SqlController();
                sqlController.SetValue("Dust", PlayerData.playerDust +PacksController.GetDust(rarity));

                string comand = $"UPDATE {PlayerData.playerLogin}_Cards SET Card_Count = Card_Count - 1 WHERE Card_Id = {cardId}; DELETE FROM {PlayerData.playerLogin}_Cards WHERE Card_Count = 0;";
                sqlController.ExecuteComand(comand);
            }       
        }

        public void CraftCard(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DialogResult dialogResult = MessageBox.Show("Craft card", "Do you want craft card?",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Cancel)
                    return;

                if(PlayerData.playerDust - PacksController.GetCraftDust(rarity) < 0)
                {
                    MessageBox.Show("Not enough dust");
                    return;
                }             

                SqlController sqlController = new SqlController();
                PlayerData.playerDust -= PacksController.GetCraftDust(rarity);
                sqlController.SetValue("Dust", PlayerData.playerDust);
                sqlController.AddCard(cardId);
            }
        }

    }
}

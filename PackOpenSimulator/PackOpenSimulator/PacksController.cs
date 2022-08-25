using System.Windows.Forms;

namespace PackOpenSimulator
{
    internal class PacksController
    {
        static public int corePackPrice = 10;
        static public string packName = "Core";

        static public string GetRarityCard(int i, string rarity)
        {
            if (i <= 90)
                return rarity;
            else if (i <= 96)
                return "Rare";
            else if (i <= 99)
                return "Epic";
            else return "Legendary";
        }

        static public int GetDust(string rarity)
        {
            switch (rarity)
            {
                case "Common": return 5;
                case "Rare": return 20;
                case "Epic": return 100;
                case "Legendary": return 400;
            }
            return -1;
        }

        static public int GetCraftDust(string rarity)
        {
            switch (rarity)
            {
                case "Common": return 40;
                case "Rare": return 100;
                case "Epic": return 400;
                case "Legendary": return 1600;
            }
            return -1;
        }

        static public bool BuyPack(int packPrice)
        {
            if (PlayerData.playerCoins - packPrice < 0)
            {
                MessageBox.Show("Not enough coins");
                return false;
            }
            else PlayerData.playerCoins -= packPrice;
            return true;
        }
    }
}

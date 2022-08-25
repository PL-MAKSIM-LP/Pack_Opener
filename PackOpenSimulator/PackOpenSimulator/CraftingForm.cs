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

namespace PackOpenSimulator
{
    public partial class CraftingForm : Form
    {
        TableLayoutPanel tableLayoutPanel;
        public CraftingForm()
        {
            InitTable();
            InitializeComponent();
        }

        void InitTable()
        {
            Controls.Clear();
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Width = 900;
            tableLayoutPanel.Height = 800;

            tableLayoutPanel.AutoScroll = true;

            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            Controls.Add(tableLayoutPanel);
            AddCardToPanel();
        }

        void AddCardToPanel()
        {
            string comand = $@"Select * From (SELECT AllCards.*, {PlayerData.playerLogin}_Cards.Card_Count FROM AllCards 
                        Left JOIN {PlayerData.playerLogin}_Cards ON AllCards.Id = {PlayerData.playerLogin}_Cards.Card_Id) as restable
                        where restable.Card_Count < 5 or  restable.Card_Count is NULL;";
            
            SqlController sqlController = new SqlController();
            SqlDataReader sqlDataReader = sqlController.ExecuteComand(comand);

            while (sqlDataReader.Read())
            {
                TableLayoutPanel tmpTableLayoutPanel = new TableLayoutPanel();

                tmpTableLayoutPanel.Width = 256;
                tmpTableLayoutPanel.Height = 400;

                ButtonControler buttonControler = new ButtonControler();
                buttonControler.rarity = Convert.ToString(sqlDataReader["Rarity"]);
                buttonControler.cardId = Convert.ToInt32(sqlDataReader["Id"]);

                Button button = new Button();
                button.Size = new Size(256, 344);

                button.FlatAppearance.BorderSize = 0;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.BackgroundImage = new Bitmap(Constants.projectDir + Convert.ToString(sqlDataReader["Card_Image"]));
                button.MouseEnter += buttonControler.FocusedButton;
                button.MouseLeave += buttonControler.NoFocusedButton;

                button.MouseUp += buttonControler.CraftCard;
                button.MouseUp += ButtonRightClick;

                tmpTableLayoutPanel.Controls.Add(button);

                Label label = new Label();
                label.AutoSize = true;
             
                if (sqlDataReader["Card_Count"].ToString() == "")                    
                    label.Text = "Count: 0";
                  else   
                    label.Text = "Count: " + Convert.ToInt32(sqlDataReader["Card_Count"]);

                tmpTableLayoutPanel.Controls.Add(label);

                tableLayoutPanel.Controls.Add(tmpTableLayoutPanel);
            }

            void ButtonRightClick(object sender, MouseEventArgs e)
            {
                tableLayoutPanel.Controls.Clear();
                AddCardToPanel();
            }
        }
    }
}

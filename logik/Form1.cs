using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace logik
{
    public partial class Form1 : Form
    {
        private readonly Game game;

        public List<TextBox[]> Hidden { get; }
        public List<ComboBox[]> Radky { get; }

        private readonly int size = 10;
        private int numB; private int numW;
        private int LocationX { get; set; }
        private int LocationY { get; set; }
        public Form1()
        {
            InitializeComponent();

            game = new Game(this);
            game.PrepareGenerator(); //vytvori list ke generovani cisel ve schovanem (Hidden) radku

            Hidden = new List<TextBox[]>();
            TextBox[] HiddenRadek = { textBoxHidden_1, textBoxHidden_2, textBoxHidden_3, textBoxHidden_4, textBoxHidden_5 }; Hidden.Add(HiddenRadek);

            Radky = new List<ComboBox[]>();
            ComboBox[] radek0 = { CB_0_1, CB_0_2, CB_0_3, CB_0_4, CB_0_5 }; Radky.Add(radek0);
            ComboBox[] radek1 = { CB_1_1, CB_1_2, CB_1_3, CB_1_4, CB_1_5 }; Radky.Add(radek1);
            ComboBox[] radek2 = { CB_2_1, CB_2_2, CB_2_3, CB_2_4, CB_2_5 }; Radky.Add(radek2);
            ComboBox[] radek3 = { CB_3_1, CB_3_2, CB_3_3, CB_3_4, CB_3_5 }; Radky.Add(radek3);
            ComboBox[] radek4 = { CB_4_1, CB_4_2, CB_4_3, CB_4_4, CB_4_5 }; Radky.Add(radek4);
            ComboBox[] radek5 = { CB_5_1, CB_5_2, CB_5_3, CB_5_4, CB_5_5 }; Radky.Add(radek5);
            ComboBox[] radek6 = { CB_6_1, CB_6_2, CB_6_3, CB_6_4, CB_6_5 }; Radky.Add(radek6);
            ComboBox[] radek7 = { CB_7_1, CB_7_2, CB_7_3, CB_7_4, CB_7_5 }; Radky.Add(radek7);
            ComboBox[] radek8 = { CB_8_1, CB_8_2, CB_8_3, CB_8_4, CB_8_5 }; Radky.Add(radek8);
            ComboBox[] radek9 = { CB_9_1, CB_9_2, CB_9_3, CB_9_4, CB_9_5 }; Radky.Add(radek9);


            var formNew = new Form
            {
                Text = ("Jak hrat logika"),
                Size = new Size(600, 150)
            };
            formNew.Show(this);

            Label label = new Label
            {
                Location = new Point(10, 10),
                AutoSize = true,
                Text = File.ReadAllText("../../resources/logiktext.txt")
            };
            formNew.Controls.Add(label);

            SetButtons(1); //spusteni programu, case 1
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            SetButtons(2); //zacetek hry, case 2
            SetHiddenTextBoxes();

            game.LineCount = 0; //nastaveni radku na prvni
        }

        private void ControlBtn_Click(object sender, EventArgs e)
        {
            game.Inicialization(); //nastavi primarni hodnoty
            game.ControlLines(game.LineCount); //vola na hlavni logiku hry
            game.CheckEnd();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            game.Inicialization();
            game.PrepareGenerator();
            SetButtons(1); //spusteni programu, case 1

            game.LineCount = 0; //reset radku na prvni

            for (int i = 0; i < 10; i++)
            {
                foreach (ComboBox text in Radky[i])
                {
                    text.BackColor = Color.White; //odbarvi Radky na puvodni barvy
                    text.ForeColor = Color.Black;
                    text.SelectedIndex = -1; //odstrani zvolena cisla
                }
            }
            foreach (TextBox text in Hidden[0])
            {
                text.Clear();
                text.ForeColor = Color.Black;
                text.BackColor = Color.White;
            }

            _whites.Clear(); _blacks.Clear();
            Invalidate(); //refresh na odstraneni pinu
        }

        private void SetButtons(int a)
        {
            switch (a)
            {
                case 1:
                    StartBtn.Enabled = true;
                    ControlBtn.Enabled = false;
                    ResetBtn.Enabled = false;
                    break;
                case 2:
                    StartBtn.Enabled = false;
                    ControlBtn.Enabled = true;
                    ResetBtn.Enabled = true;
                    break;
            }
        }
        public PictureBox PB { get; } = new PictureBox();
        private void SetHiddenTextBoxes()
        {
            PB.Image = Image.FromFile("../../resources/cover.png");
            PB.SizeMode = PictureBoxSizeMode.AutoSize;
            PB.Location = new Point(14, 315);
            PB.Visible = true;
            //pb.Visible = false;
            PB.BringToFront();
            Controls.Add(PB);

            foreach (TextBox text in Hidden[0])
            {
                text.Text = game.RandomGenerator(); //nastavi nahodne hodnoty do Hidden radku
                text.Visible = false;
            }
        }

        private SolidBrush BrushBlack { get; } = new SolidBrush(Color.Black);
        private SolidBrush BrushWhite { get; } = new SolidBrush(Color.White);
        private List<Rectangle> _whites { get; } = new List<Rectangle>();
        private List<Rectangle> _blacks { get; } = new List<Rectangle>();

        private void SetPinsLocation(int radek)
        {
            LocationX = 410;
            LocationY = 40 + radek*27;
            numB = 0; numW = 0;
        }
        
        public void DrawPins(int radek, int blackN, int whiteN)
        {
            SetPinsLocation(radek);
            if ((blackN + whiteN) <= 5)
            {
                while (numB < blackN)
                {
                    var blacks = new Rectangle(LocationX, LocationY, size, size); //size je 10
                    _blacks.Add(blacks);
                    numB++; LocationX += 20;
                }
                while (numW < whiteN)
                {
                    var whites = new Rectangle(LocationX, LocationY, size, size);
                    _whites.Add(whites);
                    numW++; LocationX += 20;
                }
            }
            else { MessageBox.Show("Nelze vykreslit, pinu je vice nez 5."); }
               
            Invalidate(); //znovu-zavolani na paint
        }

        private protected void Form1_Load(object sender, EventArgs e)
        {
            Paint += Form1_Paint; //propojeni Paintu z Formu s Form1_Paintem
        }
        
        private protected void Form1_Paint(object sender, PaintEventArgs e) //vykresluje automaticky, nutno Invalidate()
        {
            foreach (var rectangle in _blacks)
            {
                e.Graphics.FillEllipse(BrushBlack, rectangle);
            }
            foreach (var rectangle in _whites)
            {
                e.Graphics.FillEllipse(BrushWhite, rectangle);
            }
        }
    }
}
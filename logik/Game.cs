using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace logik
{
    public class Game
    {
        public Game(Form1 form) { this.form = form; }
        private readonly Form1 form;

        private int BlackCount {get; set;}
        private int WhiteCount { get; set; }
        private int CombinationCheck { get; set; }
        public int LineCount { get; set; }

        public void Inicialization() //nastavi primarni hodnoty
        {
            CombinationCheck = 0;
            BlackCount = 0;
            WhiteCount = 0;
        }

        private Random Rnd { get; } = new Random();
        private List<int> Numbers { get; set; }

        public void PrepareGenerator()
        {
            Numbers = new List<int>();
            Numbers.AddRange(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
        }

        public string RandomGenerator() //vybere nahodne cislo z listu a to vymaze
        {
            int index = Rnd.Next(0, Numbers.Count - 1);
            int nmbr = Numbers[index];
            Numbers.RemoveAt(index);

            return Convert.ToString(nmbr);
        }

        public void CheckEnd()
        {
            if (BlackCount == 5) { End(0); }
            if (LineCount == 10) { End(1); }
        }

        private void End(int i)
        {
            if (i == 0)
            {
                MessageBox.Show("KONEC HRY, GRATULUJI");
                EndMethod();
                LineCount -= 1; //zamezeni spatnemu konci
            }
            else if (i == 1)
            {
                MessageBox.Show("KONEC HRY, BOHUZEL");
                EndMethod();
            }
        }

        private void EndMethod() //odkryje obrazek a zobrazi hidden radek
        {
            form.PB.Visible = false;
            foreach (TextBox text in form.Hidden[0])
            {
                text.Visible = true;
            }
            form.ControlBtn.Enabled = false;
        }
        
        public void ControlLines(int a) //hlavni logika
        {
            ComboBox[] radek = form.Radky[a];
            ControlLinesMethod(radek[0].Text, radek[1].Text, radek[2].Text, radek[3].Text, radek[4].Text);
        }

        private void ControlLinesMethod(string s1, string s2, string s3, string s4, string s5)
        {
            combinationChecker(s1, s2, s3, s4, s5);
            
            if (CombinationCheck == 1) //kontrola, aby program nevypsal pocet cernych po combinationChecku
            {
                BlackChecker(s1, s2, s3, s4, s5); //nastavi blackCount
                WhiteChecker(s1, s2, s3, s4, s5); //nastavi whiteCount
                form.DrawPins(LineCount, BlackCount, WhiteCount); //vykresli piny
                Colour(LineCount); //obarveni daneho radku

                LineCount++;
            }
        }

        public void combinationChecker(string s1, string s2, string s3, string s4, string s5)
        {
            //pokud spravne, nastavi hodnotu combCheck 1
            //pokud spatne, vyhodi z loopu, aby uzivatel znovu zadal kombinaci
            int ErrorCounter = 0;

            if (s1 == "" || s2 == "" || s3 == "" || s4 == "" || s5 == "")
            {
                MessageBox.Show("Cislo nebylo zadano.");
                ErrorCounter++;
            }
            else if (s1 == s2 || s1 == s3 || s1 == s4 || s1 == s5 || s2 == s3 || s2 == s4 || s2 == s5 || s3 == s4 || s3 == s5 || s4 == s5)
            {
                MessageBox.Show("Cisla se opakuji.");
                ErrorCounter++;
            }
            if (ErrorCounter == 0) { CombinationCheck = 1; }
        }

        private void BlackChecker(string s1, string s2, string s3, string s4, string s5) //urci pocet cernych (spravne trefene barvy s pozici)
        {
            TextBox[] radek = form.Hidden[0];
            if (radek[0].Text == s1) { BlackCount++; }
            if (radek[1].Text == s2) { BlackCount++; }
            if (radek[2].Text == s3) { BlackCount++; }
            if (radek[3].Text == s4) { BlackCount++; }
            if (radek[4].Text == s5) { BlackCount++; }
        }

        private void WhiteChecker(string s1, string s2, string s3, string s4, string s5) //urci pocet bilych (musi se odecist cerne)
        {
            foreach (TextBox text in form.Hidden[0])
            {
                if (text.Text == s1) { WhiteCount++; }
                if (text.Text == s2) { WhiteCount++; }
                if (text.Text == s3) { WhiteCount++; }
                if (text.Text == s4) { WhiteCount++; }
                if (text.Text == s5) { WhiteCount++; }
            }
            WhiteCount -= BlackCount;
        }

        private void Colour(int radek)
        {
            foreach (ComboBox text in form.Radky[radek])
            {
                if (text.Text == "0") { text.BackColor = Color.White; }
                if (text.Text == "1") { text.BackColor = Color.LightGray; }
                if (text.Text == "2") { text.BackColor = Color.Black; text.ForeColor = Color.White;  }
                if (text.Text == "3") { text.BackColor = Color.Yellow; }
                if (text.Text == "4") { text.BackColor = Color.LightGreen; }
                if (text.Text == "5") { text.BackColor = Color.SkyBlue; }
                if (text.Text == "6") { text.BackColor = Color.Pink; }
                if (text.Text == "7") { text.BackColor = Color.Red; }
            }
            if (radek < 1)
            {
                foreach (TextBox text in form.Hidden[0])
                {
                    if (text.Text == "0") { text.BackColor = Color.White; }
                    if (text.Text == "1") { text.BackColor = Color.LightGray; }
                    if (text.Text == "2") { text.BackColor = Color.Black; text.ForeColor = Color.White; }
                    if (text.Text == "3") { text.BackColor = Color.Yellow; }
                    if (text.Text == "4") { text.BackColor = Color.LightGreen; }
                    if (text.Text == "5") { text.BackColor = Color.SkyBlue; }
                    if (text.Text == "6") { text.BackColor = Color.Pink; }
                    if (text.Text == "7") { text.BackColor = Color.Red; }
                }
            } 
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
// Author: Adel Sliman
//ID: 572643
//Date: 12/05/2015
//Purpose: the purpose for this application is so the user can enjoy a nice, classic game of Hangman.
namespace HangMan__Adel_Sliman
{
    public partial class HangMan : Form
    {
        
        
        public HangMan()
        {
            InitializeComponent();
        }
        // some variables to set the numbers missed and make the words be counted properly
        string word = "";
        List<Label> Labels = new List<Label>();
        int AmountMissed = 0;
        // makes the body parts of Hangman set as variables so they can appear as they are drawn.
        enum BodyParts
        {
            Head,
            Left_Eye,
            Right_Eye,
            Mouth,
            Body,
            Left_Arm,
            Right_Arm,
            Left_Leg,
            Right_Leg,
        }
        // draws the gallows everytime the the form is activated...@@@It Is Kinda Buggy@@@
        void DrawGallows()
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black, 10);
            g.DrawLine(p, new Point(130, 218), new Point(130, 5));
            g.DrawLine(p, new Point(135, 5), new Point(65, 5));
            g.DrawLine(p, new Point(60, 0), new Point(60, 50));

            g.Dispose(); 
            
        }
        // Draws the body parts based on the set coordinates.
        void DrawBodyParts(BodyParts Bp)
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.DarkViolet, 2);
            if (Bp == BodyParts.Head)
            {
                g.DrawEllipse(p, 40, 50, 40, 40);
            }
            else if (Bp == BodyParts.Left_Eye)
            {
                SolidBrush s = new SolidBrush(Color.Chartreuse);
                g.FillEllipse(s, 50, 60, 5, 5);
            }
            else if (Bp == BodyParts.Right_Eye)
            {
                SolidBrush s = new SolidBrush(Color.Chartreuse);
                g.FillEllipse(s, 63, 60, 5, 5);
            }
            else if (Bp == BodyParts.Mouth)
            {
                SolidBrush s = new SolidBrush(Color.DarkViolet);
                g.DrawArc(p, 50, 60, 20, 20, 45, 90);
            }
            else if (Bp == BodyParts.Body)
            {
                g.DrawLine(p, new Point(60, 90), new Point(60, 170));
            }
            else if (Bp == BodyParts.Left_Arm)
            {
                g.DrawLine(p, new Point(60, 100), new Point(30, 85));
            }
            else if (Bp == BodyParts.Right_Arm)
            {
                g.DrawLine(p, new Point(60, 100), new Point(90, 85));
            }
            else if (Bp == BodyParts.Left_Leg)
            {
                g.DrawLine(p, new Point(60, 170), new Point(30, 190));
            }
            else if (Bp == BodyParts.Right_Leg)
            {
                g.DrawLine(p, new Point(60, 170), new Point(90, 190));
            }

            g.Dispose();
        }
        // makes labels appear on the form as simple __ marks that hold the value of the random word.
        void MakeLabels()
        {
            word = RandomWord();
            char[] chars = word.ToArray();
            int space = 400 / chars.Length;
            for (int i = 0; i < chars.Length; i++)
            {
                Labels.Add(new Label());
                Labels[i].Location = new Point((i * space) + 10, 80);
                Labels[i].Text = "_";
                Labels[i].Parent = groupBox2;
                Labels[i].BringToFront();
                Labels[i].CreateControl();

            }
            lblWordLength.Text = "Word Length: " + (chars.Length).ToString();
        }
        // generates a random word everytime from the list of words from the site below vvvvvvv.
        String RandomWord()
        {
            WebClient wc = new WebClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string wordlist = wc.DownloadString("https://raw.githubusercontent.com/Tom25/Hangman/master/wordlist.txt");
            string[] words = wordlist.Split('\n');
            Random random = new Random();
            wc.Dispose();
            return words[random.Next(0, words.Length)];
            
        }
// makes the labels and gallows appear everytime the form is drawn.
        private void Form1_Shown(object sender, EventArgs e)
        {
            DrawGallows();
            MakeLabels();
        }
        // when the letter butten is clicked it checks to see if the letter is part of the word. if it is it shows on the form, and if its not the user is shown an added
        // body part also the missed letter shows up under the missed letters label. also this button displays an error if the users attempts to submit anything other
        // then a letter. if the user wins a messageBix is displayed saying they have won and the game restarts, and if the user loses then the game shows them the missed 
        // word and restarts.
        private void btnLetter_Click(object sender, EventArgs e)
        {
           try
            { char letter = txtLetter.Text.ToLower().ToCharArray()[0];
            
                if (!char.IsLetter(letter))
                {
                    MessageBox.Show("you can only submit letters!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (word.Contains(letter))
                {
                    char[] letters = word.ToCharArray();
                    for (int i = 0; i < letters.Length; i++)
                    {
                        if (letters[i] == letter)
                            Labels[i].Text = letter.ToString();
                    }
                    foreach (Label l in Labels)
                        if (l.Text == "_") return;
                    MessageBox.Show("You Have Won!!!", "Congrats");
                    reset();
                }
                else
                {
                    MessageBox.Show("The letter you guessed is not in the word.");
                    lblMissed.Text += " " + letter.ToString() + ",";
                    DrawBodyParts((BodyParts)AmountMissed);
                    AmountMissed++;
                    if (AmountMissed == 9)
                    {
                        MessageBox.Show("Sorry, you lose. The word Was " + word);
                        reset();
                    }
                }
            }
            catch
            {
                MessageBox.Show("you can only submit letters!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
// creats the value to reset the game.
        void reset()
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(panel1.BackColor);
            RandomWord();
            MakeLabels();
            DrawGallows();
            lblMissed.Text = "Missed: ";
            txtLetter.Text = "";
        }
// lets the user enter the whole word if they can and if they have all the letters it will display a messagebox to show them if they won or lost.
        private void btnWord_Click(object sender, EventArgs e)
        {
            if (txtWord.Text == word)
            {
                MessageBox.Show("You Have Won!!!", "Congrats");
                reset();
            }
            else
            {
                MessageBox.Show("The word you guessed is wrong, sorry!");
                DrawBodyParts((BodyParts)AmountMissed);
                AmountMissed++;
                if (AmountMissed == 9)
                {
                    MessageBox.Show("Sorry, you lose. The word Was " + word);
                    reset();
                    
                }
            }
        }
// i added a clear button that sets the focus back to the letter box.
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLetter.Text = "";
            txtWord.Text = "";
            txtLetter.Focus();
        }
// i added a reset button because sometimes the game is alittle buggy and certain features do not work right. ie. the gallows not being drawn. the player never losing. 
        // no body parts being shown. ect.
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            reset();
            
        }
// closes the form.
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Close();
        }

      
    }
}

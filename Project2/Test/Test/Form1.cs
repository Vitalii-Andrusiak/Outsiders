using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Test
{


    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();

        }

        

        // Класи Тести Запитання Відповіді
        public Tests tests = new Tests();
        public Tests testsWithAnswers = new Tests();
        public static List<CorrectAnswersToQuestions> CorrectList = new List<CorrectAnswersToQuestions>();
        int seconds = 60;
        int min;

     // static int min = tests.ExecutionTime;

        public string question;
        public double point;
        public string answer;



        // Читання з XML
        private Tests DeserializeXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Tests));
            using (FileStream fileStream = new FileStream($"{tests.Title}", FileMode.Open, FileAccess.Read))
            {
                return (Tests)serializer.Deserialize(fileStream);
            }
        }



        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileOk += OpenFileDialog_FileOk;
            openFileDialog.ShowDialog();
        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            splitContainer1.Visible = true;
            OpenFileDialog openFileDialog = sender as OpenFileDialog;
            this.tests.Title = openFileDialog.FileName;
            Tests tests = DeserializeXML();
            testsWithAnswers = tests;//////
            this.tests = tests;
            this.text_Title.Text = tests.Title;
            this.text_Author.Text = tests.Author;
            this.text_Count.Text = tests.list_Questions.Count.ToString();
            this.text_MaxPoint.Text = tests.MaxPoint.ToString();
            this.richText_Info.Text = tests.Info;
            this.richText_Description.Text = tests.Description;
            this.text_MinPass.Text = tests.MinPass.ToString();
            this.ExecutionTimetextBox.Text = tests.ExecutionTime.ToString();
            this.questionsBindingSource.DataSource = tests.list_Questions;
        }

        private void textBoxPicture_TextChanged(object sender, EventArgs e)
        {

            try
            {
                pictureBox1.Image = Bitmap.FromFile(textBoxPicture.Text);
            }
            catch (Exception)
            {


            }

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            min = tests.ExecutionTime;
            MessageBox.Show(min.ToString());
            mlabel.Text = "0" + min.ToString();
            MessageBox.Show(min.ToString());
            slabel.Text = "60";
            timer1.Start();
            min--;
            if (min < 10)
                mlabel.Text = "0" + min.ToString();
            else
                mlabel.Text = min.ToString();
            


            tableLayoutPanel2.Visible = true;
            buttonStart.Visible = false;
            buttonFinish.Visible = true;




            List<int> Amountlist = new List<int>();
            foreach (var item in tests.list_Questions)
            {
                question = item.Question_Text;
                point = item.point;
                foreach (var item1 in item.list_Answers)
                {
                    if (item1.IsRight == true)
                        answer = item1.Answer_Text;
                    item1.IsRight = false;
                }
                Amountlist.Add(item.list_Answers.Count());

                CorrectAnswersToQuestions correctAnswersToQuestions = new CorrectAnswersToQuestions(question, point, answer);
                CorrectList.Add(correctAnswersToQuestions);
                //MessageBox.Show(correctAnswersToQuestions.AnswerText.ToString());Ok

            }
            for (int i = 0; i < questionsDataGridView.Rows.Count; i++)
            {
                questionsDataGridView.Rows[i].Cells[2].Value = Amountlist[i].ToString();

            }
            questionsDataGridView.Refresh();
            //MessageBox.Show(CorrectList.Count.ToString());
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            FinishTest();
            timer1.Stop();




        }
        private void FinishTest()
        {

            List<string> uAnswers = new List<string>();
            double ScoredPoints = 0;


            foreach (var item in tests.list_Questions)
            {
                int k = 0;
                foreach (var item1 in item.list_Answers)
                {

                    if (item1.IsRight == true)
                    {
                        uAnswers.Add(item1.Answer_Text);
                        break;

                    }
                    else if (k == item.list_Answers.Count - 1)
                    {
                        uAnswers.Add("");
                    }

                    k++;
                }

            }
            //MessageBox.Show($"u{uAnswers.Count}cl{CorrectList.Count}");
            for (int i = 0; i < CorrectList.Count; i++)
            {

                //MessageBox.Show($"{uAnswers[i]} // {CorrectList[i].AnswerText}");
                if (uAnswers[i] == CorrectList[i].AnswerText)
                {
                    //MessageBox.Show($"!!!!{CorrectList[i].AnswerText}   {uAnswers[i]}");
                    ScoredPoints += CorrectList[i].QuestioPoint;
                }
            }

            //MessageBox.Show(ScoredPoints.ToString());
            splitContainer1.Visible = false;

            ResultForm resultForm = new ResultForm(tests, ScoredPoints);
            resultForm.ShowDialog();

            this.Close();


        }



         private void timer1_Tick(object sender, EventArgs e)
         {
            seconds--;
            if (seconds == 0 || seconds < 10)
            {
                slabel.Text = "0" + seconds.ToString();

            }

            else
                slabel.Text = seconds.ToString();
            if (seconds == 0 && min > 0)
            {
                min--;

                seconds = 60;
                slabel.Text = seconds.ToString();
                if (min < 10)
                    mlabel.Text = "0" + min.ToString();
                else
                    mlabel.Text = min.ToString();



            }
            if (seconds <= 0 && min <= 0)
            {

                timer1.Stop();
                slabel.Text = "0" + seconds.ToString();
                MessageBox.Show("all gg");
                FinishTest();
                
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
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
        public Form1(){
            
            InitializeComponent();
            
        }

        // Класи Тести Запитання Відповіді
        public Tests tests = new Tests();
        public Questions question = new Questions();
        public Answers answer = new Answers();
        // Змінна для перевірки чи файл відкритий для редагування
        public bool IsOpen=false;
        // List  Запитання Відповіді
        public static List<Questions> qList = new List<Questions>();
        public static List<Questions> tmpqList = new List<Questions>();
        public static List<Answers> aList = new List<Answers>();
        

        //  Функція для запису в XML
        private void SerializeXML()
        {
            tests.list_Questions = questionsBindingSource.DataSource as List<Questions>;
            XmlSerializer serializer = new XmlSerializer(typeof(Tests));
            using (FileStream fileStream = new FileStream($"{tests.Title}.xml", FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fileStream, tests);
            }
        }
        // Функція для читання з XML
        private Tests DeserializeXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Tests));
            using (FileStream fileStream = new FileStream($"{tests.Title}", FileMode.Open, FileAccess.Read))
            {
                return (Tests)serializer.Deserialize(fileStream);
            }
        }

        // Відриваємо форму для створення Тесту
        public void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Початкові дані
            splitContainer1.Visible = true;
            text_Author.Text = "admin";
            text_MaxPoint.Text = "100";
            text_MinPass.Text = "50";
            richText_Description.Text = "test";
            richText_Info.Text = "inf";
            text_Count.Text = "10";
            ExecutionTimetextBox.Text = "2";
        }
        //На основі даних створення тесту
        public void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            // Перевіряємо дані на валідність
            
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                tests.Title = text_Title.Text;
                tests.Author = text_Author.Text;
                tests.Description = richText_Description.Text;
                tests.Info = richText_Info.Text;
                tests.MaxPoint = double.Parse(text_MaxPoint.Text);
                tests.MinPass = double.Parse(text_MinPass.Text);

                // Для створення запитань переходимо на форму QuestionForm

                QuestionForm questionForm = new QuestionForm(tests,qList, aList, questionsBindingSource);
                questionForm.ShowDialog(this);
            }
            else
            {
                questionsBindingSource.RemoveCurrent();
            
               textBoxPicture.Text = "C:\\Users\\User\\Desktop\\Tests\\Project2\\Test\\Photo\\2.png";
                //MessageBox.Show(textBoxPicture.Text);
            }
        }

       

        // Зберігаємо тест у форматі XML
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            questionsDataGridView.EndEdit();
            answersDataGridView.EndEdit();
            bool IsOK = true;
            if (IsOpen)
            {
                double point = 0;
                double Mp = double.Parse(text_MaxPoint.Text);
                

                MessageBox.Show(tests.list_Questions.Count.ToString());
                foreach (var item in tests.list_Questions)
                {
                    point += item.point;
                }
                
                //double.TryParse(text_MaxPoint.Text,aList=0)
                if ( point>Mp)
                {
                    MessageBox.Show("point sum !!!");
                    IsOK = false;
                }


                foreach (var item in tests.list_Questions)
                {
                    int i=1;
                    int rightA = 0;
                    foreach (var item2 in item.list_Answers)
                    {
                        if (item2.IsRight == true)
                        {
                            rightA++;
                            i++;
                        }
                        if (rightA==0&& item.list_Answers.Count==i)
                        {
                            MessageBox.Show("Кожне запитання має мати відповідь");
                            IsOK = false;
                        }
                        if (rightA>1 )
                        {
                            MessageBox.Show("Одне питання одна відповідь");
                            IsOK = false;
                        }

                    }
                }

            }

          if(IsOK)
            {
                tests.Title = text_Title.Text;
                tests.Author = text_Author.Text;
                tests.Description = richText_Description.Text;
                tests.Info = richText_Info.Text;
                tests.MaxPoint = double.Parse(text_MaxPoint.Text);
                tests.MinPass = double.Parse(text_MinPass.Text);
                tests.ExecutionTime = int.Parse(ExecutionTimetextBox.Text);
                tests = new Tests(tests.Title, tests.Author, tests.Description, tests.Info, tests.MaxPoint, tests.MinPass, tests.ExecutionTime, qList);
                SerializeXML();
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
            IsOpen=true;
            splitContainer1.Visible = true;
            OpenFileDialog openFileDialog = sender as OpenFileDialog;
            this.tests.Title = openFileDialog.FileName;
            Tests tests = DeserializeXML();
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
            qList.AddRange(tests.list_Questions);
            questionsDataGridView.ReadOnly = false;
            answersDataGridView.ReadOnly = false;
           
        }

        private void textBoxPicture_TextChanged(object sender, EventArgs e)
        {
        
            try
            {

                pictureBox1.Image = Bitmap.FromFile(textBoxPicture.Text);
            }
            catch (Exception)
            {


                //textBoxPicture.Text = "C:\\Users\\Misha\\Desktop\\Tests\\Project1\\Test\\Photo\\2.png";
                //pictureBox1.Image = Bitmap.FromFile(textBoxPicture.Text);
            }

        }

        private void text_Title_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(text_Title.Text))
            {
                e.Cancel = true;
                text_Title.Focus();
                errorProvider1.SetError(text_Title, "Enter Title");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(text_Title, null);
            }
        }

        private void text_Author_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(text_Author.Text))
            {
                e.Cancel = true;
                text_Author.Focus();
                errorProvider1.SetError(text_Author, "Enter your name");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(text_Author, null);
            }
        }

        private void text_Count_Validating(object sender, CancelEventArgs e)
        {
            int a = int.Parse(text_Count.Text);
            if (a < 1 || a > 100)
            {
                e.Cancel = true;
                text_Count.Focus();
                errorProvider1.SetError(text_Count, "[1..100]");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(text_Count, null);
            }
        }

        

        private void text_MaxPoint_Validating(object sender, CancelEventArgs e)
        {
            int a = int.Parse(text_MaxPoint.Text);
            if (a < 1 || a > 100)
            {
                e.Cancel = true;
                text_MaxPoint.Focus();
                errorProvider1.SetError(text_MaxPoint, "[1..100]");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(text_MaxPoint, null);
            }

        }

        private void text_MinPass_Validating(object sender, CancelEventArgs e)
        {
            int a = int.Parse(text_MinPass.Text);
            if (a < 50 || a > 100)
            {
                e.Cancel = true;
                text_MinPass.Focus();
                errorProvider1.SetError(text_MinPass, "[50..100]");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(text_MinPass, null);
            }
        }

        private void ExecutionTimetextBox_Validating(object sender, CancelEventArgs e)
        {
            int a = int.Parse(ExecutionTimetextBox.Text);
            if (a < 1 || a > 91)
            {
                e.Cancel = true;
                ExecutionTimetextBox.Focus();
                errorProvider1.SetError(ExecutionTimetextBox, "[1..90]");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(ExecutionTimetextBox, null);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

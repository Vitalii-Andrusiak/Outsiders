using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Test
{
    public partial class AnswerForm : Form
    {
        public AnswerForm(BindingSource bindingSource)
        {
            InitializeComponent();
            this.bindingSource1= bindingSource;
            
        }

        int trueAnser = 0;
        //public AnswerForm(List<Answers> aList,List<Questions> qlist)
        //{
        //    InitializeComponent();

        //}
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text!=string.Empty)
            {
                
                Answers answers = new Answers();
                answers.Answer_Text = richTextBox1.Text;
                if (checkBox1.Checked)
                {
                    answers.IsRight = true;
                    trueAnser++;
                }
                this.bindingSource1.Add(answers);
                 Form1.aList.Add(answers);
                //if (trueAnser>=1)
                //{
                //    this.bindingSource1.Add(answers);
                //    Form1.aList.Add(answers);
                //}
                //if (trueAnser>1)
                //{
                //    MessageBox.Show("2 r ansvers");
                //}


            }
            this.Close();
            //ONUpdate(answers);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WordProcessorTrial
{
    public partial class Form1 : Form
    {
        String defaultText = "Новая коллекция";
        String newFileName;
        public Form1()
        {
            InitializeComponent();
           
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.Text = defaultText;
            
            label1.Text = "Лексема";
            label2.Text = "Количество контекстов";
            label3.Text = "Семантика";
            label4.Text = "Основание для анализа";
            label5.Text = "Анализ";
            label6.Text = "Итог";
            label7.Text = "Контекст";
            label8.Text = "Источник контекста";

            button1.Text = "Внести";
            button2.Text = "Внести";
            button3.Text = "Внести";
            button4.Text = "Внести";
            button5.Text = "Внести";
            button6.Text = "Внести в файл";
            button7.Text = "Внести";
            button8.Text = "Внести";

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;

            richTextBox5.ReadOnly = true;

           
    }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            richTextBox1.ReadOnly = true;
            String transferrable = "<b><i>" + label1.Text + "</i></b>: "; 
            transferrable += "<i>" + richTextBox1.Text + "</i>";
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            String transferrable = "<b><i>" + label2.Text + "</i></b>: ";
            richTextBox5.Text += transferrable;
            richTextBox5.Text += numericUpDown1.Value;
            richTextBox5.Text += ".<br>\n";
            button8.Enabled = true;
        }
        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            button3.Enabled = false;
            richTextBox2.ReadOnly = true;
            String transferrable = "<b><i>" + label3.Text + "</i></b>: ";
            transferrable += richTextBox2.Text;
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button7.Enabled = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            button4.Enabled = false;
            richTextBox3.ReadOnly = true;
            String transferrable = "<b><i>" + label4.Text + "</i></b>: ";
            transferrable += richTextBox3.Text;
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button5.Enabled = true;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {

            button5.Enabled = false;
            richTextBox4.ReadOnly = true;
            String transferrable = "<b><i>" + label5.Text + "</i></b>: ";
            transferrable += richTextBox4.Text;
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button6.Enabled = true;
        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            richTextBox6.ReadOnly = true;
            String transferrable = "<b><i>" + label7.Text + "</i></b>: ";
            transferrable += "<i>" + richTextBox6.Text + "</i> ";            
            transferrable += "[" + richTextBox7.Text + "]";
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;
            button4.Enabled = true;
        }

        
        
       private string fn = String.Empty;

       private void SaveDocument()
        {
            if ((fn == string.Empty) || (this.Text == newFileName)) 
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fn = saveFileDialog1.FileName;
                    this.Text = fn;
                    newFileName = this.Text;
                }
            }
             
        }
        
       

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.Text == defaultText)
            {
                this.SaveDocument();
                String fn1 = Path.GetFullPath(saveFileDialog1.FileName);
                System.IO.File.AppendAllText(fn1, richTextBox5.Text);
            }
            else
            {
                string message = "Файл уже существует. Продолжить?"; 
                string caption = "Предупреждение";                 
                MessageBoxButtons buttons = MessageBoxButtons.YesNo; 
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    String fn1 = Path.GetFullPath(saveFileDialog1.FileName);
                    System.IO.File.AppendAllText(fn1, richTextBox5.Text);
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    this.SaveDocument();
                    String fn1 = Path.GetFullPath(saveFileDialog1.FileName);
                    System.IO.File.AppendAllText(fn1, richTextBox5.Text);
                }
            }
            
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            richTextBox7.Clear();
            numericUpDown1.Value = 0;
            button6.Enabled = false;
            button1.Enabled = true;
            richTextBox1.ReadOnly = false;
            richTextBox2.ReadOnly = false;
            richTextBox3.ReadOnly = false;
            richTextBox4.ReadOnly = false;
            richTextBox6.ReadOnly = false;
            richTextBox7.ReadOnly = false;
            numericUpDown1.ReadOnly = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            richTextBox7.ReadOnly = true;
            button3.Enabled = true;

        }
    }
}

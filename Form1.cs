using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {   
        private CancellationTokenSource CancelTypingTokenSource;

        public Form1()
        {
            InitializeComponent();
            this.label1.Text = $"{this.trackBar1.Value} ms";
        }

        async Task TypeText(string textToType, int timeDelay, CancellationToken cancelToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(timeDelay), cancelToken);

            textToType = Regex.Replace(textToType, @"([\{\}\+\^%~\(\)\[\]])", "{$1}");
            textToType = Regex.Replace(textToType, @"\r\n?|\n", "{ENTER}");
            textToType = Regex.Replace(textToType, @"\t", "{TAB}");

            SendKeys.SendWait(textToType);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            this.CancelTypingTokenSource = new CancellationTokenSource();
            int timeDelay = this.trackBar1.Value;
            
            try
            {
                await this.TypeText(this.textBox1.Text, this.trackBar1.Value, this.CancelTypingTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation has been cancelled.");
            }
            catch (Exception)
            {
                MessageBox.Show("There was an exception.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = Clipboard.GetText();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.label1.Text = $"{this.trackBar1.Value} ms";
        }

        //Cancel button
        private void button3_Click(object sender, EventArgs e)
        {
            if (null != this.CancelTypingTokenSource)
            {
                this.CancelTypingTokenSource.Cancel();
            }
        }
    }
}

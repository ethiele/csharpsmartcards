using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GemCard;

namespace SmartTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool disc = false;
        APDUResponse apduResp;

        CardNative iCard;

        private void button1_Click(object sender, EventArgs e)
        {
            iCard = new CardNative();

            string[] readers = iCard.ListReaders();

            textBox1.Text = "Readers: " + Environment.NewLine;
            foreach (string s in readers)
            {
                textBox1.Text += "   " + s +Environment.NewLine;
            }
            textBox1.Text += "Connected to first reader";
            iCard.Connect(readers[0], SHARE.Shared, PROTOCOL.T0);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] b = { 0x09, 0xD4, 0x60, 0x01, 0x01, 0x20, 0x23, 0x11, 0x04 };
            APDUCommand cmd = new APDUCommand(0xFF, 0xC0, 0x00, 0x00, null, 0x13);
                apduResp = iCard.Transmit(cmd);
                this.Text = apduResp.Status.ToString();
                textBox1.Text += byteArrToString(apduResp.Data) + Environment.NewLine;
        }

        private string byteArrToString(byte[] b)
        {
            string st = "";
            if (b == null) return "";
            foreach (byte bi in b)
            {
                st += bi.ToString("x") + " ";
            }
            return st;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            iCard.Disconnect(DISCONNECT.Leave);
        }
    }
}

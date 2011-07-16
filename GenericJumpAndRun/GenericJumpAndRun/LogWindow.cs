using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericJumpAndRun
{
    public partial class LogWindow : Form
    {
        public LogWindow()
        {
            InitializeComponent();
            messageTimer.Interval = 10;
            messageTimer.Tick += new EventHandler(messageTimer_Tick);
            messageTimer.Start();
        }

        void messageTimer_Tick(object sender, EventArgs e)
        {
            if(messageQueue.Count > 0)
                richTextBox1.AppendText(messageQueue.Dequeue() + Environment.NewLine);
        }

        private Timer messageTimer = new Timer();
        private Queue<string> messageQueue = new Queue<string>();
        public void AddMessage(string message)
        {
            messageQueue.Enqueue(message);
        }
        
    }
}

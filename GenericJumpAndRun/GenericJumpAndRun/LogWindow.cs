using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GenericJumpAndRun
{
    public partial class LogWindow : Form
    {
        public LogWindow()
        {
            InitializeComponent();
            _messageTimer.Interval = 10;
            _messageTimer.Tick += MessageTimerTick;
            _messageTimer.Start();
        }

        void MessageTimerTick(object sender, EventArgs e)
        {
            if(_messageQueue.Count > 0)
                richTextBox1.AppendText(_messageQueue.Dequeue() + Environment.NewLine);
        }

        private readonly Timer _messageTimer = new Timer();
        private readonly Queue<string> _messageQueue = new Queue<string>();
        public void AddMessage(string message)
        {
            _messageQueue.Enqueue(message);
        }
        
    }
}

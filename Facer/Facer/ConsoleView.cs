﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Facer
{
    class ConsoleView : Editor, IWriteLine
    {

        public ConsoleView() : base()
        {
            this.IsEnabled = false;
            this.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
        }

        public void WriteLine(string msg)
        {
            this.Text += msg + "\n";
        }

        public void Clear()
        {
            this.Text = "";
        }
    }
}

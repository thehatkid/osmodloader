﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Threading;
using System.IO;

namespace OneShot_ModLoader
{
    public class LoadingBar
    {
        public Label text = new Label();

        public LoadingBar(Form form)
        {
            text.ForeColor = Color.MediumPurple;
            text.Location = new Point(0, 190);
            text.AutoSize = true;

            text.Font = Constants.GetTerminusFont(10);
            text.ForeColor = Color.MediumPurple;
            text.BackColor = Color.Transparent;

            form.Controls.Add(text);
        }

        public string GetLoadingBGM()
        {
            return "bgm_0" + new Random().Next(1, 6) + ".mp3";
        }

        public async Task SetLoadingStatus(string status)
        {
            try
            {
                string finalStatus = status;

                // replace the working directory or oneshot path with an empty string to shorten the status
                if (finalStatus.Contains(Directory.GetCurrentDirectory()))
                    finalStatus = finalStatus.Replace(Directory.GetCurrentDirectory(), string.Empty);
                else if (finalStatus.Contains(Form1.baseOneShotPath))
                    finalStatus = finalStatus.Replace(Form1.baseOneShotPath, string.Empty);

                // set the status
                text.Text = finalStatus;
            }
            catch (Exception ex)
            {
                text.Font = new Font(new FontFamily(GenericFontFamilies.Monospace), 10);
                string message = "exception encountered in loading bar: " + ex.Message;

                Console.WriteLine(message + "\n---\n" + ex.ToString());
            }
            
            await Task.Delay(1);
        }
    }
}

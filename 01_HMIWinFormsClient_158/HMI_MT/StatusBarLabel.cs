using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace HMI_MT
{
    public class StatusBarLabel
    {
        /// <summary>
        /// интервал смены строк в миллисек
        /// </summary>
        public double TimerInterval 
        {
            set 
            {
                timerInterval = value;
                if (timerRenewval != null)
                    timerRenewval.Interval = value;
            }
        }
        double timerInterval;

        Form parent;
        StatusStrip statusParent;
        ToolStripStatusLabel tsStatusLabel;
        SortedList<string, string> slStr4View;
        SortedList<string, System.Drawing.Color> slStrMesFontColor;
        System.Timers.Timer timerRenewval;
        string lastMes;
        System.Drawing.Font defaultFont;
        System.Drawing.Color defaultColor;

        public StatusBarLabel(Form prnt, StatusStrip statusparent, ToolStripStatusLabel tssl)
        {
            parent = prnt;
            statusParent = statusparent;
            tsStatusLabel = tssl;
            slStr4View = new SortedList<string, string>();
            slStrMesFontColor = new SortedList<string, System.Drawing.Color>();
            timerRenewval = new System.Timers.Timer();
            TimerInterval = 3000;   // таймер по умолчанию 3 сек
            timerRenewval.Stop();
            timerRenewval.Elapsed += new System.Timers.ElapsedEventHandler(timerRenewval_Elapsed);
            lastMes = String.Empty;
            defaultFont = new System.Drawing.Font("Courier New", 8, System.Drawing.FontStyle.Bold);
            defaultColor = System.Drawing.Color.Black;
        }

        void timerRenewval_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           timerRenewval.Stop();
            // выбор очередного сообщения
            int ind = GetIndexRec();

            if (ind == -1)
                SetText_toolStripStatusLabelClock(String.Empty, System.Drawing.Color.White);
            else
                SetText_toolStripStatusLabelClock(slStr4View.ElementAt(ind).Value, slStrMesFontColor.ElementAt(ind).Value);

            try
            {
               lastMes = slStr4View.ElementAt(ind).Key;
            }
            catch { }
            

            timerRenewval.Start();
        }
        
        private int GetIndexRec()
        {
            if (slStr4View.Count == 0)
                return -1;  // список пуст

            // попытаемся получить следующий элемент
            if (!slStr4View.ContainsKey(lastMes))
                return 0;

            int ind = slStr4View.IndexOfKey(lastMes) + 1;
            try
            {
               if (!String.IsNullOrEmpty(slStr4View.ElementAtOrDefault(ind).Key))
                  return ind;
            }
            catch (Exception ex)
            {
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 90, DateTime.Now.ToString() + " : StatusBarLabel.cs : GetIndexRec() : " + ex);               
            }
            return 0;   // первый элемент
        }

        /// <summary>
        /// добавление/изменение строки для отображения
        /// </summary>
        /// <param Name="strkey">строка-ключ</param>
        /// <param Name="strmes">строка для отображения</param>
        /// <param Name="clr">цвет (null - цвет по умолчанию)</param>
        public void ShowStr(string strkey, string strmes, System.Drawing.Color clr)
        {
            if (slStr4View.ContainsKey(strkey))
            {
                slStr4View[strkey] = strmes;
                return;
            }

            if (String.IsNullOrEmpty(strmes))
                return;


            if (!slStr4View.ContainsKey(strkey))
                slStr4View.Add(strkey, strmes);

            if (clr == System.Drawing.Color.Empty)
                slStrMesFontColor.Add(strkey, defaultColor);
            else
                slStrMesFontColor.Add(strkey, clr);
            
           // если в списке более одного элемента, то запустить таймер
            if (slStr4View.Count >= 1)
                timerRenewval.Start();
        }

        /// <summary>
        /// удалить строку из списка отображаемых
        /// </summary>
        /// <param Name="strkey">строка-ключ</param>
        public void RemoveStr4Show(string strkey) 
        {
            if (slStr4View.ContainsKey(strkey))
                slStr4View.Remove(strkey);
            if (slStrMesFontColor.ContainsKey(strkey))
                slStrMesFontColor.Remove(strkey);

            // если в списке не более одного элемента , то остановить таймер
            if (slStr4View.Count <= 1)
                timerRenewval.Stop();
        }

        private delegate void SetText_toolStripStatusLabelClock_Delegate(string text,System.Drawing.Color t);
        private void SetText_toolStripStatusLabelClock(string text, System.Drawing.Color t)
        {
           try
           {
              if (statusParent.InvokeRequired)
              {
                 SetText_toolStripStatusLabelClock_Delegate sett = new SetText_toolStripStatusLabelClock_Delegate(SetText_toolStripStatusLabelClock);
                 parent.Invoke(sett, new object[] { text, t });
              }
              else
              {
                 tsStatusLabel.Text = text;
                 tsStatusLabel.Font = defaultFont;
                 tsStatusLabel.ForeColor = t;
              }
           }
           catch (Exception ex)
           {
              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 149, "StatusBarLabel.cs : " + ex.Message);               
           }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
//using CRZADevices;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace HMI_MT
{
   public enum DTF { of_long, of_long100 };
   
   public class TreeViewPanel: TreeView
   {
      #region Свойства
      [DisplayName("Формат времени")]
      [Description("Формат времени в метке времени тега")]
      [Category("Формат")]
      public DTF DTFormat
      {
         get { return dTFormat; }
         set { dTFormat = value; }
      }
      private DTF dTFormat;
      #endregion

      #region private
      // список переменных по которым идет сбор трендов
      SortedList slTagForTrends = new SortedList(); // бывший slOPCForArm
      ListView linkListView = null;  // связанный ListView для заполнения
      XDocument xdocTrends = new XDocument();
      FileStream fs;
      StreamWriter sw;
      // для формирования строки тренда
      StringBuilder sbresstr = null;
      #endregion

      public TreeViewPanel() 
      {
         this.Dock = DockStyle.Fill;
         this.CheckBoxes = true;

         //xdocTrends = XDocument.Load(Application.StartupPath + Path.DirectorySeparatorChar + "TrendsRes.trnd");
      }

      #region работа с деревом тегов - иницализация/очистка
      /// <summary>
      /// инициализация дерева на базе конфигурации устройств
      /// </summary>
      /// <param Name="newKB_KB"></param>
      public void FillTreeView(ArrayList newKB_KB, Control parent)
      {
         //this.Parent = parent;

         //foreach (FC aFc in newKB_KB)
         //{
         //   TreeNode tn = new TreeNode("ФК - " + aFc.NumFC.ToString(), 0, 0);
         //   tn.Checked = aFc.isCollectDataForRemoteARM;
         //   tn.Tag = aFc;

         //   foreach (TCRZADirectDevice tdd in aFc)
         //   {
         //      TreeNode tnn = new TreeNode("[ № уст. - " + tdd.NumDev.ToString() + " ] " + tdd.ToString(), 1, 1);
         //      tnn.Checked = tdd.isCollectDataForRemoteARM;
         //      tnn.Tag = tdd;
         //      tn.Nodes.Add(tnn);

         //      // добавляем группы
         //      foreach (TCRZAGroup tcg in tdd)
         //      {
         //         // добавляем группы доступных для выбора тегом - с текущей информацией, коэф. трансф., причины вызова
         //         //if (tcg.i.Id == 3 || tcg.Id == 6 || tcg.Id == 5)
         //         if (tcg.IsTheGroupTrendMember)
         //         {
         //            TreeNode tg = new TreeNode(tcg.Name, 0, 0);//"Текущая информация"
         //            tg.Tag = tcg;

         //            tnn.Nodes.Add(tg);

         //            foreach (TCRZAVariable tcv in tcg)
         //            {
         //               TreeNode tv = new TreeNode(tcv.Name);
         //               tv.Tag = tcv;
         //               tg.Nodes.Add(tv);
         //            }
         //            tnn.Collapse(true);
         //            //break;
         //         }
         //      }
         //   }
         //   tn.Tag = aFc;
         //   //treeviewPrjConfig.Nodes.Add(tn);
         //   this.Nodes.Add(tn);
         //}
      }

      /// <summary>
      /// FillTrendTreeView() - настройка конфигурации в механизме трендов в соответствии с конфигурацией TreeView
      /// (аналог FillOPCTreeView в механизме OPC)
      /// </summary>
      public void FillTrendTreeView(bool makeCurrent /*сделать текущей данную конфигурацию тегов для мех трендов*/)
      {
//         StringBuilder sbKey = new StringBuilder();
//         StringBuilder tr = new StringBuilder();
//         XDocument xdoc = null;
//         XDocument xdocTMP = null;

//         if (makeCurrent)
//         {
//            xdocTMP = FormingXDOCbyTreeViewNodes();
//            // разорвем связь между xdocTMP и xdoc
//            xdocTMP.Save(Application.StartupPath + Path.DirectorySeparatorChar + "xdocTMP.tmp");
//            xdoc = XDocument.Load(Application.StartupPath + Path.DirectorySeparatorChar + "xdocTMP.tmp");
//         }

//         IEnumerable<XElement> tvxe = xdoc.Element("TrendsTagConfig").Elements();
//         foreach (XElement tfc in tvxe)
//         {
//            if (tfc.Name != "FC")
//            {
//               MessageBox.Show("frmOPCSender.FillOPCTreeView: Неправильный формат документа", "frmOPCSender", MessageBoxButtons.OK, MessageBoxIcon.Error);
//               return;
//            }
//            foreach (TreeNode tnfc in this.Nodes)
//            {
//               FC tagFC = (FC)tnfc.Tag;
//               if (tagFC.NumFC != Convert.ToInt32(tfc.Attribute("FC_number").Value))
//                  continue;

//               if (tfc.Attribute("Checked").Value == "True")
//               {
//                  tnfc.Checked = true;
//                  ChangeCheckedTreeViewNodes(tnfc, true);
//                  continue;  // если для всего фк стоит checked = true, то выделили все теги для всех устройств и перешли к следующему ФК
//               }
//               else
//               {
//                  tnfc.Checked = false;
//                  ChangeCheckedTreeViewNodes(tnfc, false);   // сбросили выделение для всех дочерних узлов
//               }

//               // смотрим устройства
//               IEnumerable<XElement> tvxedev = tfc.Elements();
//               foreach (XElement tdev in tvxedev)
//                  foreach (TreeNode tnd in tnfc.Nodes)
//                  {
//                     TCRZADirectDevice tcdd = (TCRZADirectDevice)tnd.Tag;
//                     if (tcdd.NumDev.ToString() != tdev.Attribute("Dev_number").Value)
//                        continue;

//                     if (tdev.Attribute("Checked").Value == "True")
//                     {
//                        tnd.Checked = true;
//                        ChangeCheckedTreeViewNodes(tnd, true);
//                        tnd.ExpandAll();
//                     }
//                     else
//                     {
//                        tnd.Checked = false;
//                        ChangeCheckedTreeViewNodes(tnd, false);   // сбросили выделение для всех дочерних узлов
//                        tnd.Collapse(false);
//                     }

//                     // смотрим группы
//                     IEnumerable<XElement> tvxedevgr = tdev.Elements();
//                     foreach (XElement tdevgr in tvxedevgr)
//                        foreach (TreeNode tndg in tnd.Nodes)
//                        {
//                           TCRZAGroup tcg = (TCRZAGroup)tndg.Tag;
//                           if (tcg.Id.ToString() != tdevgr.Attribute("Gr_number").Value)
//                              continue;

//                           if (tdevgr.Attribute("Checked").Value == "True")
//                           {
//                              tndg.Checked = true;
//                              ChangeCheckedTreeViewNodes(tndg, true);
//                              tndg.ExpandAll();
//                           }
//                           else
//                           {
//                              tndg.Checked = false;
//                              ChangeCheckedTreeViewNodes(tndg, false);   // сбросили выделение для всех дочерних узлов
//                              tndg.Collapse(false);
//                           }

//                           // смотрим отдельные теги
//                           IEnumerable<XElement> tvxedevgrvar = tdevgr.Elements();
//                           foreach (XElement tdevgrv in tvxedevgrvar)
//                              foreach (TreeNode tndgv in tndg.Nodes)
//                              {
//                                 TCRZAVariable tcgv = (TCRZAVariable)tndgv.Tag;
//                                 if (tcgv.RegInDev.ToString() != tdevgrv.Attribute("Var_RegInDev").Value)
//                                    continue;

//                                 if (tdevgrv.Attribute("Type").Value == "TBitFieldVariable" & (tcgv.ToString() == "TBitFieldVariable"))
//                                    if (((TBitFieldVariable)tcgv).bitMask != tdevgrv.Element("BitMask").Value)
//                                       continue;

//                                 if (tdevgrv.Attribute("Checked").Value == "True")
//                                 {
//                                    tndgv.Checked = true;
//                                    ChangeCheckedTreeViewNodes(tndgv, true);
//                                    tnd.ExpandAll();

//                                    if (makeCurrent)
//                                    {
//                                       // добавляем объект тренда из общего списка трендов системы
//                                       Trend trnd = new Trend();
//                                       trnd.FcLink = tagFC;
//                                       trnd.DeviceLink = tcdd;
//                                       trnd.GroupLink = tcg;
//                                       trnd.VariableLink = tcgv;
//                                       HMI_Settings.ListOfTrends.Add(trnd);

//                                       #region Страрый код для OPC-сервера
//                                       /*		                                       
//                                       tcdd.isCollectDataForRemoteARM = true;

//                                       // формируем элемент списка - его имя будет видеться в OPC-сервере
//                                       sbKey.Length = 0;

//                                       sbKey.Append("#");
//                                       sbKey.Append(tagFC.NumFC.ToString() + "." + tcdd.NumDev.ToString() + "." + tcg.Id.ToString() + "." + tcgv.RegInDev.ToString());
//                                       sbKey.Append("ФК № " + tagFC.NumFC.ToString());
//                                       sbKey.Append("#");
//                                       sbKey.Append(tcdd.ToString() + "уст № " + tcdd.NumDev.ToString());
//                                       sbKey.Append("#");
//                                       sbKey.Append(tcg.Name + "гр №" + tcg.Id.ToString() + "#рег №" + tcgv.RegInDev.ToString());
//                                       sbKey.Append("#");
//                                       sbKey.Append(tcgv.Name);

//                                       switch (tcgv.ToString())
//                                       {
//                                          case "TBitFieldVariable":
//                                             sbKey.Append("#");
//                                             sbKey.Append(((TBitFieldVariable)tcgv).bitMask);
//                                             break;
//                                          case "TIntVariable":
//                                             if ((tcgv.RegInDev < 33) || (tcgv.RegInDev > 50))
//                                                break;
//                                             // для аналоговых сигналов сформируем элемент в списке коэффициентов трансформации
//                                             float ktr = Convert.ToSingle(GetKtr(tcdd, tcgv.RegInDev - minNumRegForAnalog));

//                                             if (!slKtrForArm.ContainsKey(sbKey.ToString()))
//                                                slKtrForArm.Add(sbKey.ToString(), ktr);

//                                             break;
//                                          default:
//                                             break;
//                                       }

//                                       if (!slOPCForArm.ContainsKey(sbKey.ToString()))
//                                          slOPCForArm.Add(sbKey.ToString(), tcgv);
//                                    }
//*/
//                                       #endregion
//                                    }
//                                    else
//                                    {
//                                       tndgv.Checked = false;
//                                       ChangeCheckedTreeViewNodes(tndgv, false);   // сбросили выделение

//                                       if (makeCurrent)
//                                       {
//                                          // удаляем объект тренда из общего списка трендов системы
//                                          #region Старый код OPC-сервера
//                                          //tcdd.isCollectDataForRemoteARM = false;
//                                          #endregion
//                                       }
//                                    }
//                              }
//                        }
//                  }
//            }
//         }
//            }
      }

      /// <summary>
      /// рекурсивная функция обработки Checked для узлов
      /// </summary>
      /// <param Name="node"></param>
      /// <param Name="valcheck"></param>
      private void ChangeCheckedTreeViewNodes(TreeNode node, bool valcheck)
      {
         node.Checked = valcheck;

         foreach (TreeNode nd in node.Nodes)
            ChangeCheckedTreeViewNodes(nd, valcheck);
      }

      /// <summary>
      /// формирование xdoc по содержимому treeview
      /// </summary>
      /// <returns></returns>
      private XDocument FormingXDOCbyTreeViewNodes()
      {
         XDocument xdoc_cur = new XDocument();
         XElement newxe;
         xdoc_cur.Add(newxe = new XElement("TrendsTagConfig"));
         XAttribute xaver = new XAttribute("version", "0.1");   // версия формата файла
         newxe.Add(xaver);
         // обходим дерево и формируем xml-теги с признаком Checked
         foreach (TreeNode tn in this.Nodes)
         {
            //if ((tn.Tag as FC) == null)
            //{
            //   MessageBox.Show("Неправильный порядок описания устройств в конфигурации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //   return null;
            //}

            //XElement newxeFC = new XElement("FC");
            //xaver = new XAttribute("FC_number", ((FC)tn.Tag).NumFC.ToString());   // версия формата файла
            //newxeFC.Add(xaver);
            //xaver = new XAttribute("Checked", tn.Checked.ToString());   // признак выделения
            //newxeFC.Add(xaver);

            //newxe.Add(newxeFC);

            //// теперь добавляем устройства для данного фк
            //foreach (TreeNode tnd in tn.Nodes)
            //{
            //   if ((tnd.Tag as TCRZADirectDevice) == null)
            //   {
            //      MessageBox.Show("Неправильный порядок описания устройств в конфигурации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //      return null;
            //   }
            //   XElement newxeDev = new XElement("TCRZADirectDevice");
            //   xaver = new XAttribute("Dev_number", ((TCRZADirectDevice)tnd.Tag).NumDev.ToString());
            //   newxeDev.Add(xaver);
            //   xaver = new XAttribute("Checked", tnd.Checked.ToString());   // признак выделения
            //   newxeDev.Add(xaver);

            //   newxeFC.Add(newxeDev);

            //   // теперь добавляем группы для данного устройства
            //   foreach (TreeNode tng in tnd.Nodes)
            //   {
            //      if ((tng.Tag as TCRZAGroup) == null)
            //      {
            //         MessageBox.Show("Неправильный порядок описания устройств в конфигурации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //         return null;
            //      }
            //      XElement newxeGr = new XElement("TCRZAGroup");
            //      xaver = new XAttribute("Gr_number", ((TCRZAGroup)tng.Tag).Id.ToString());
            //      newxeGr.Add(xaver);
            //      xaver = new XAttribute("Checked", tng.Checked.ToString());   // признак выделения
            //      newxeGr.Add(xaver);

            //      newxeDev.Add(newxeGr);

            //      // теперь добавляем теги для данной группы
            //      foreach (TreeNode tnv in tng.Nodes)
            //      {
            //         if ((tnv.Tag as TCRZAVariable) == null)
            //         {
            //            MessageBox.Show("Неправильный порядок описания устройств в конфигурации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return null;
            //         }
            //         XElement newxeVar = new XElement("TCRZAVariable");
            //         xaver = new XAttribute("Var_RegInDev", ((TCRZAVariable)tnv.Tag).RegInDev.ToString());
            //         newxeVar.Add(xaver);
            //         xaver = new XAttribute("Checked", tnv.Checked.ToString());   // признак выделения
            //         newxeVar.Add(xaver);

            //         // переменная битовая?
            //         xaver = new XAttribute("Type", tnv.Tag.ToString());   // тип
            //         switch (tnv.Tag.ToString())
            //         {
            //            case "TBitFieldVariable":
            //               newxeVar.Add(new XElement("BitMask", ((TBitFieldVariable)tnv.Tag).bitMask));
            //               break;
            //            default:
            //               break;
            //         }
            //         newxeVar.Add(xaver);

            //         newxeGr.Add(newxeVar);
            //      }
            //   }
            //}
         }
         return xdoc_cur;
      }
      #endregion

      #region listview
      public void FillListView(ListView listview)
      {
         linkListView = listview;
         LinkSetLV(null, true);    // очищаем ListView для обновления  

         StringBuilder ts = new StringBuilder();
         StringBuilder sbidtag = new StringBuilder();

         //foreach( Trend trndLV in HMI_Settings.ListOfTrends )
         //{
         //   ListViewItem li = new ListViewItem();
         //   li.SubItems.Clear();

         //   sbidtag.Length = 0;
         //   // название тега для ListView
         //   //sbidtag.Append(slOPCForArm.GetKey(curRow));
         //   sbidtag.Append(trndLV.FcLink.NumFC.ToString());
         //   sbidtag.Append("#");
         //   sbidtag.Append(trndLV.DeviceLink.NumDev.ToString());
         //   sbidtag.Append("#");
         //   sbidtag.Append(trndLV.GroupLink.Id.ToString());
         //   sbidtag.Append("#");
         //   sbidtag.Append(trndLV.VariableLink.ToString());
         //   sbidtag.Append("#");
         //   sbidtag.Append(trndLV.VariableLink.Name.ToString());

         //   li.SubItems[0].Text = sbidtag.ToString();

         //   // поле для значения
         //   sbidtag.Length = 0;
         //   sbidtag.Append(" ");
         //   li.SubItems.Add(sbidtag.ToString());

         //   li.Tag = trndLV;  // Tag = экземпляру объекта Trend
         //   LinkSetLV(li, false);
         //}

         // раскращиваем ListView в зебру
         CommonUtils.CommonUtils.DrawAsZebra(linkListView);
      }

      #region Мониторинг
      public void InitMonitoring()
      {
         // проверим существование папки Trends
         if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Trends"))
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Trends");

         string pth = GetCurrentFileNameForTrends();

         if (!File.Exists(pth))
            fs = File.Create(pth);
         else
            fs = File.Open(pth, FileMode.Append);

         sw = new StreamWriter(fs);
         
         /* если файл новый, то первая строка в нем 
          * определяет формат последующих записей 
          */
         if (fs.Length == 0)
            sw.WriteLine(CreateHeaderForTrendsFile());

         sbresstr = new StringBuilder();
      }

      public void StopMonitoring()
      {
         if (sw == null)
            return;

         sw.Flush();
         sw.Close();
         sw = null;
      }

      /// <summary>
      /// обновить listview
      /// </summary>
      /// <param Name="lvw"></param>
      public void ReNewListView()
      {
         //// проверим существование файла для текущего времени суток
         //if (!File.Exists(GetCurrentFileNameForTrends()))
         //   CreateNewTrendsFile(GetCurrentFileNameForTrends());

         //for (int curRow = 0; curRow < linkListView.Items.Count; curRow++)
         //{
         //   ListViewItem li = linkListView.Items[curRow];
         //   Trend trn = li.Tag as Trend;
         //   TCRZAVariable tcv = trn.VariableLink;

         //   if (trn == null)
         //   {
         //      sw.WriteLine("Не представляется возможным формирование записи по тренду. Нулевая ссылка на тренд.");
         //      continue;
         //   }

         //   ListViewItem.ListViewSubItemCollection livisic = li.SubItems;

         //   livisic[1].Text = tcv.ExtractTagValueAsString();

         //   // формируем запись в файле xdocTrends
         //   sw.WriteLine(CreateRecForTrendsFile(trn));
         //   sw.Flush();
         //}

         //// раскращиваем ListView в зебру
         //CommonUtils.CommonUtils.DrawAsZebra(linkListView);
      }

      private string GetCurrentFileNameForTrends()
      {
         // проверим существование файла трендов за текущий день
         string curdate = DateTime.Now.ToShortDateString();
         curdate = Regex.Replace(curdate, @"\.", "_");

         return AppDomain.CurrentDomain.BaseDirectory
            + Path.DirectorySeparatorChar + "Trends"
            + Path.DirectorySeparatorChar + curdate + ".trnd";
      }

      /// <summary>
      /// создание нового файла тренда (при смене суток)
      /// </summary>
      /// <param Name="filename"></param>
      private void   CreateNewTrendsFile(string filename)
      {
         if (sw != null)
         {
            sw.Flush();
            sw.Close();
            sw = null;
         }
         if (fs != null)
         {
            fs.Close();
            fs = null;
         }

         fs = File.Create(filename);
         sw = new StreamWriter(fs);
         // первая строка файла с форматом последующих строк
         sw.WriteLine(CreateHeaderForTrendsFile());
      }

      private string CreateHeaderForTrendsFile()
      {
         return "Date&Time; FC_number;Device_number;TagGuid;TagValue;";
      }

      private string CreateRecForTrendsFile(Trend trn)
      {

         //TCRZAVariable tcv = trn.VariableLink;

         //sbresstr.Length = 0;
         //sbresstr.Append(tcv.GetTimeMarker(dTFormat.ToString())); // метка времени тега;
         //sbresstr.Append(";");
         //sbresstr.Append(trn.FcLink.NumFC.ToString());
         //sbresstr.Append(";");
         //sbresstr.Append(trn.DeviceLink.NumDev.ToString());
         //sbresstr.Append(";");
         //sbresstr.Append(tcv.TagGGUID.ToString()); // уник номер тега в устройстве
         //sbresstr.Append(";");
         //sbresstr.Append(trn.VariableLink.ExtractTagValueAsString());  // значение тега
         //sbresstr.Append(";");

         return sbresstr.ToString();
      }

      #endregion

      #region потокобезопасное обращение к ListView
      delegate void SetLVCallback(ListViewItem li, bool actDellstV);

      // actDellstV - действия с ListView : false - не трогать; true - очистить;
      private void LinkSetLV(object Value, bool actDellstV)
      {
         if (!(Value is ListViewItem) && !actDellstV)
            return;   // сгенерировать ошибку через исключение

         ListViewItem li = null;
         if (!actDellstV)
            li = (ListViewItem)Value;

         if (linkListView.InvokeRequired)
         {
            if (!actDellstV)
               SetLV(li, actDellstV);
            else
               SetLV(null, actDellstV);
         }
         else
         {
            if (!actDellstV)
               linkListView.Items.Add(li);
            else
               linkListView.Items.Clear();
         }
      }

      private void SetLV(ListViewItem li, bool actDellstV)
      {
         if (linkListView.InvokeRequired)
         {
            SetLVCallback d = new SetLVCallback(SetLV);
            this.Invoke(d, new object[] { li, actDellstV });
         }
         else
         {
            if (!actDellstV)
               linkListView.Items.Add(li);
            else
               linkListView.Items.Clear();
         }
      }
      #endregion
      #endregion
   }
}

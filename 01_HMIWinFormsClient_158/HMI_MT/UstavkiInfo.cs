using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data;

namespace HMI_MT
{
   /// <summary>
   /// Начало иерархии описания конфигурации уставок устройства
   /// Необходимость создания этой иерархии вызвана тем, что пока
   /// в конфигурации тегов верхнего уровня не учитывается конфигурация 
   /// подгрупп, вследствие чего нельзя сформировать пакеты для записи уставок 
   /// в Экры, т.к. там уставки пишуться кусками.
   /// </summary>
   public class UstavkiInfo
   {
      public UstavkiInfo()
      { 
      }
      public UstavkiInfo(string deviceCfgFileName)
      {
         PrepareClassTreeViewForUstGroup("Уставки");
      }

      private void PrepareClassTreeViewForUstGroup(string groupname)
      {
         // найдем описание группы в файле описания устройства
         XDocument xdoc = XDocument.Load(path2DeviceCFG);

         var xe_config = (from x in xdoc.Descendants("GroupInDev")
                          where x.Element("Name").Value == groupname
                          select x).DefaultIfEmpty().Single();

         if (xe_config == null)
         {
            MessageBox.Show("Нет данных для форимирования вкладки для группы " + groupname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         // создадим DataSet
         DataSet dsust = new DataSet();

         // пустая группа ?
         if (xe_config.Descendants("SubGroup").Count() == 0 && ((string)xe_config.Element("Tags") == null))
         {
            MessageBox.Show("Нет данных для отображения группы " + groupname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         // проверим есть ли у корневого элемента узел "Tags"
         if (!String.IsNullOrEmpty((string)xe_config.Element("Tags")))
            CreateTableForNodeTags(treeViewUstCFG.Nodes[0], xe_config, dsust, xe_config.Element("NumberGrInASU").Value);

         if (xe_config.Elements("SubGroup").Count() != 0)
         {
            CreateTableForSubGroupNodeTags(treeViewUstCFG.Nodes[0], xe_config, dsust, xe_config.Element("NumberGrInASU").Value);
         }

         treeViewUstCFG.ExpandAll();

         #region старый код
         // slnflps - сортированный список flp с именами и условными обозначениями
         /*SortedList*/
         //slnflps = CreateTPforGroup(xe_config, tabpage, tabpage.Text, pnlTP);

         // теперь новые flp нужно добавить в список
         //GetCCforFLP((ControlCollection)this.Controls);

         //arlist = CreateArrayList(tabpage.Text);//grname

         //PlaceVisElemOnForm(tabpage.Text, String.Empty, arlist); 
         #endregion 
      }

      private void CreateTableForSubGroupNodeTags(TreeNode tn, XElement xe, DataSet dsust, string numgr)
      {
         IEnumerable<XElement> xesgs = xe.Elements("SubGroup");

         foreach (XElement xesg in xesgs)
         {
            // создаем новый treenode для подгруппы
            TreeNode newtreenode = new TreeNode(xesg.Element("Name").Value);
            tn.Nodes.Add(newtreenode);

            if (!String.IsNullOrEmpty((string)xesg.Element("Tags")))
               CreateTableForNodeTags(newtreenode, xesg, dsust, numgr);

            CreateTableForSubGroupNodeTags(newtreenode, xesg, dsust, numgr);
         }
      }

      StringBuilder sb = new StringBuilder();

      private void CreateTableForNodeTags(TreeNode tn, XElement xe, DataSet dsust, string numgr)
      {
         DataTable dt = new DataTable();

         dt.Columns.Add("Имя тега", typeof(System.String));
         dt.Columns.Add("Текущее значение", typeof(System.String));
         dt.Columns.Add("Новое значение", typeof(System.String));
         dt.Columns.Add("Адрес тега", typeof(System.String));
         dt.Columns.Add("TagGUID", typeof(System.String));

         // инициализируем строки таблицы
         IEnumerable<XElement> xetags = xe.Element("Tags").Descendants("ASU_level_Describe");

         if (xetags.Count() == 0)
            return;
         // формируем таблицу и списки (заодно)
         foreach (XElement xetag in xetags)
         {
            sb.Length = 0;
            DataRow dtr = dt.NewRow();

            sb.Append(xetag.Element("AddressModbus").Value);

            if (!String.IsNullOrEmpty(xetag.Element("BitMask").Value.Trim()))
               sb.Append("_" + xetag.Element("BitMask").Value);

            dtr["Адрес тега"] = sb.ToString();
            dtr["Имя тега"] = xetag.Element("Description").Value;
            dtr["TagGUID"] = xetag.Element("TagGUID").Value;

            dt.Rows.Add(dtr);

            #region также формируем соответсвие между адресом тега и таблицей где он содержиться - чтобы сделать возможным динамическое обновление
            slLinkAdr2DataTable.Add(sb.ToString(), dt);
            #endregion

            #region одновременно формируем сортированный список - ключ - адрес тега
            slFromAdr2TCRZAVar.Add(sb.ToString(), GetLinkToCRZAVar(xetag.Element("TagGUID").Value));

            sb.Length = 0;
            // и подписываемся на обновление
            sb.Append("0(" + IFC.ToString() + "." + IIDDev.ToString() + "." + numgr + "." + xetag.Element("AddressModbus").Value + ".");

            if (!String.IsNullOrEmpty(xetag.Element("BitMask").Value.Trim()))
               sb.Append(xetag.Element("BitMask").Value);
            else
               sb.Append("0000");

            sb.Append(")");
            #endregion


            #region формируем FormulaEval - для обновления
            TypeOfTag tot = new TypeOfTag();

            switch (xetag.Element("AnalDiscretType").Value)
            {
               case "Analog":
                  tot = TypeOfTag.Analog;
                  break;
               case "Discret":
                  tot = TypeOfTag.Discret;
                  break;
               case "Combo":
                  tot = TypeOfTag.Combo;
                  break;
               default:
                  MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  break;
            }

            FormulaEval eval = new FormulaEval(parent.KB, sb.ToString(), "0", xetag.Element("Description").Value, xetag.Element("Dimention").Value, tot, xetag.Element("TypeFrm").Value);
            eval.OnChangeValForm += new FormulaEval.ChangeValForm(eval_OnChangeValForm);
            #endregion
         }

         dsust.Tables.Add(dt);
         tn.Tag = dt;
      }
   }

   /// <summary>
   /// базовый класс для описания уставок
   /// </summary>
   public class UstavkiBase
   { 
   }

   /// <summary>
   /// Постоянные(общие) уставки
   /// </summary>
   public class OverallUstGroup : UstavkiBase
   { 
   }

   /// <summary>
   /// Сменяемые уставки
   /// </summary>
   public class RotatingUstGroup : UstavkiBase
   { 
   }

   /// <summary>
   /// подгруппа в группе уставок
   /// </summary>
   public class SubGroupUst
   { 

   }
}

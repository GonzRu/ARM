/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: форма для отображения списка доступных на панели тек режима тегов
 *				для выбора их удаления
 *                                                                             
 *	Файл                     : X:\Projects\33_Virica\Server_new_Interface\crza\CRZADevices\CRZADevices\CRZADeviceEv.cs                                         
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 07.02.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HMI_MT
{
	public partial class dlgDeleteTagsFromCMP : Form
	{
		private XElement cMPTags;

		public dlgDeleteTagsFromCMP()
		{
			InitializeComponent();
		}

		public void Init(XElement xes)
		{
			cMPTags = xes;

			foreach (XElement xe in xes.Element("Adapters").Elements("formula"))
			{
				CheckBox chb = new CheckBox();
				chb.Text = xe.Attribute("Caption").Value;
				chb.Tag = xe;
				flp4DeleteTags.Controls.Add(chb);
				chb.CheckedChanged += new EventHandler(chb_CheckedChanged);
			}
		}

		void chb_CheckedChanged(object sender, EventArgs e)
		{
			if ((sender as CheckBox).Checked)
				((sender as CheckBox).Tag as XElement).Remove();

			// перерисуем
			flp4DeleteTags.Controls.Clear();
			Init(cMPTags);

		}

		private void btnDeleteTAgs_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}

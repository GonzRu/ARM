/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: frmEditDataServerFiles - форма для работы с конфигурационными файлами
 *				на DataServer
 *                                                                             
 *	Файл                     : Z:\Projects\MTConfigurator\MTConfigurator\MTConfigurator\frmEditDataServerFiles.cs                                         
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 11.03.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualBasic;
using CommonUtils;

namespace HMI_MTConfig
{
	public partial class frmEditDataServerFiles :  CustomizeForm
	{		
		#region Свойства
		public string LblInfoFileName_Text
		{
			get { return lblInfoFileName_Text; }
			set
			{
				lblInfoFileName_Text = lblInfoFileName.Text = value;
			}
		}
		string lblInfoFileName_Text;

		public string LblInfoIP_Text
		{
			get { return lblInfoIP_Text; }
			set { lblInfoIP_Text = lblInfoIP.Text = value; }
		}
		string lblInfoIP_Text;
		#endregion

		#region public
		#endregion

		#region private
		/// <summary>
		/// имя временного файла 
		/// для хранения текущего файла
		/// </summary>
		string path2f = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "tpm.cfg";
		CommonUtils.LongTimeAction lta;
		#endregion

		#region конструктор(ы)
		#endregion

		public frmEditDataServerFiles()
		{
			InitializeComponent();

			lblInfoIP_Text = string.Empty;
		}

		private void btnLoadFile_Click(object sender, EventArgs e)
		{
			switch ((sender as Button).Name)
			{ 
				case "btnLoadProject_cfg":
					rtbEdit.Clear();
					LblInfoFileName_Text = string.Empty;
					SendReq4LoadFile("Project.cfg");
					break;
				case "btnLoadPrjDevCfg_cdp":
					rtbEdit.Clear();
					LblInfoFileName_Text = string.Empty;
					SendReq4LoadFile("PrgDevCFG.cdp");
					break;
				case "btnSaveFile":
					SaveCurrentFile();
					break;
				default:
					break;
			}
		}

		#region загурзка файла для редактирования
		/// <summary>
		/// Запустить процесс получения 
		/// требуемого файла
		/// </summary>
		/// <param name="reqFile"></param>
		private void SendReq4LoadFile(string reqFile)
		{
			IPAddress ipds;
			string ipstr = string.Empty;

			while (true)
			{
				ipstr = Interaction.InputBox("Введите адрес Dataserver", "ip-адрес сервера данных", lblInfoIP_Text);

				if (ipstr == string.Empty)
					return;

				if (!IPAddress.TryParse(ipstr, out ipds))
					MessageBox.Show("Введен некорректный ip-адрес : " + ipstr, "frmEditDataServerFiles.cs : SendReq4LoadFile()", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
				{
					LblInfoFileName_Text = reqFile;
					LblInfoIP_Text = ipstr;
					break;
				}
			}

			using (TcpClient client = new TcpClient())
			{
				try
				{
					client.Connect(ipds, 9871);

					using (NetworkStream ns = client.GetStream())
					{
						using (BinaryWriter bw = new BinaryWriter(ns))
						{
							using (BinaryReader br = new BinaryReader(ns))
							{
								bw.Write("Req4EditFile");
								bw.Flush();
								string str = br.ReadString();
								if (str == "GetFileName")
								{
									bw.Write(reqFile);
									if (br.ReadString() == "FileExist")
										File_Receiver(ns, br);
									else
										MessageBox.Show("Запрошенный файл на сервере не существует : " + reqFile);
								}
								else
									throw new Exception("Неверный запрос");
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "frmEditDataServerFiles.cs : SendReq4LoadFile()");
				}
			}
		}

		/// <summary>
		/// Получить и загрузить файл
		/// для редактирования
		/// </summary>
		/// <param name="myns"></param>
		/// <param name="bb"></param>
		void File_Receiver(NetworkStream myns, BinaryReader bb)
		{
			try
			{
				lta = new LongTimeAction();
				lta.Start(this, "Загрузка файла ...");

				byte[] buffer = bb.ReadBytes(5000000);

				if (File.Exists(path2f))
					File.Delete(path2f);

				FileStream fss = new FileStream(path2f, FileMode.CreateNew, FileAccess.Write);//(@textBox1.Text + 
				fss.Write(buffer, 0, buffer.Length);
				fss.Close();

				lta.Stop();
				rtbEdit.LoadFile(path2f, RichTextBoxStreamType.PlainText);

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}		
		#endregion

		/// <summary>
		/// Обновить загруженный (текущий) файл
		/// при редактировании его внешним редактором
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReNew_Click(object sender, EventArgs e)
		{
			rtbEdit.Clear();
			rtbEdit.LoadFile(path2f,RichTextBoxStreamType.PlainText);
		}

		#region Сохранение файла
		/// <summary>
		/// Сохранить текущий файл на текущем Dataserver
		/// </summary>
		private void SaveCurrentFile()
		{
			IPAddress ipds;
			string ipstr = string.Empty;

			if (!IPAddress.TryParse(LblInfoIP_Text, out ipds))
			{
				MessageBox.Show("Введен некорректный ip-адрес : " + ipstr, "frmEditDataServerFiles.cs : SendReq4LoadFile()", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (TcpClient client = new TcpClient())
			{
				try
				{
					client.Connect(ipds, 9871);

					using (NetworkStream ns = client.GetStream())
					{
						using (BinaryReader br = new BinaryReader(ns))
						{
						using (BinaryWriter bw = new BinaryWriter(ns))
							{
								bw.Write("Req4SaveFile");
								bw.Flush();
								string str = br.ReadString();
								if (str == "GetFileName")
								{
									bw.Write(LblInfoFileName_Text);
									if (br.ReadString() == "FileExist")
										File_Send(ns, bw);
									else
										MessageBox.Show("Запрошенный файл на сервере не существует : " + LblInfoFileName_Text);
								}
								else
									throw new Exception("Неверный запрос");
							}
							MessageBox.Show("Файл " + LblInfoFileName_Text + " отправлен DataServer с адресом: " + ipstr); ;
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "frmEditDataServerFiles.cs : SendReq4LoadFile()");
				}
			}

		}
		/// <summary>
		/// Послать файл на DataServer
		/// </summary>
		/// <param name="ns"></param>
		/// <param name="br"></param>
		private void File_Send(NetworkStream ns, BinaryWriter bw)
		{
			FileStream fs = new FileStream(path2f, FileMode.Open);

			byte[] buffer = new byte[fs.Length];
			int len = (int)fs.Length;
			fs.Read(buffer, 0, len);
			fs.Close();

			lta = new LongTimeAction();
			lta.Start(this, "Сохранение файла ...");
			bw.Write(buffer);
			lta.Stop();
		} 
		#endregion

		private void btnSameExch_Click(object sender, EventArgs e)
		{
			rtbEdit.SaveFile(path2f,RichTextBoxStreamType.PlainText);
		}
	}
}

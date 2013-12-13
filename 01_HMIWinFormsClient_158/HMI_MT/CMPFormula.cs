using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using CRZADevices;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using InterfaceLibrary;

namespace HMI_MT
{
	public class CMPFormula
	{
		/// <summary>
		/// массив элементов в названиях для 
		/// вывода элементов формулы в TextBox
		/// синхронизирован с массивом arrFormulaElementsInNumbers
		/// </summary>
		ArrayList arrFormulaElementsInNames = new ArrayList();
		/// <summary>
		/// массив элементов в номерах тегов для 
		/// формирования вида формулы в ПОЛИЗ
		/// синхронизирован с массивом arrFormulaElementsInNames
		/// </summary>
		ArrayList arrFormulaElementsInNumbers = new ArrayList();
		/// <summary>
		/// список - задает соответсвие номера тега и его представления
		/// </summary>
		SortedList<int,object> slNumber2TCRZADev = new SortedList<int,object>();
		/// <summary>
		/// формула в ПОЛИЗ
		/// </summary>
		string formulaPOLIS = string.Empty;
		/// <summary>
		/// реализованные операции
		/// </summary>
		string[] strOperations = new string[] { "(",")","*","/","+","-" };

		/// <summary>
		/// выбарть тег из конфигурации
		/// </summary>
		/// <returns></returns>
		internal string SelectTag()
		{
			string rez = string.Empty;

			dlgOptionsFormEditor dlgOFE = new dlgOptionsFormEditor(-1);
			dlgOFE.ShowDialog();

            //TCRZAVariable tcrzavar = dlgOFE.SelectionVar;
            //if (tcrzavar != null)
            //{
            //    rez = tcrzavar.Name + "(" + CommonUtils.CommonUtils.GetDispCaptionForDevice(tcrzavar.Group.Device.NumFC * 256 + tcrzavar.Group.Device.NumDev) + ")";
            //    arrFormulaElementsInNames.Add(rez);
            //    arrFormulaElementsInNumbers.Add(slNumber2TCRZADev.Count()); 
            //    slNumber2TCRZADev.Add(slNumber2TCRZADev.Count(), tcrzavar);
            //}

			return rez;
		}

		/// <summary>
		/// Выбор константы
		/// </summary>
		/// <returns></returns>
		internal string SelectConst()
		{
            //dlgConstInput dci = new dlgConstInput();
            //dci.ShowDialog();
            //string rez = dci.inputConst;
            //if (string.IsNullOrWhiteSpace(rez))
            //    return string.Empty;

            ////string rez = Interaction.InputBox("Введите константу", "Задать константу в формулу", string.Empty);

            //arrFormulaElementsInNames.Add(rez);
            //arrFormulaElementsInNumbers.Add(slNumber2TCRZADev.Count()); 
            //slNumber2TCRZADev.Add(slNumber2TCRZADev.Count(), rez);

            //return rez;
            return string.Empty;
		}

		/// <summary>
		/// выбор операции
		/// </summary>
		/// <returns></returns>
		internal string SelectOperation()
		{
            //dlgSelectOperation dso = new dlgSelectOperation(strOperations);
            //dso.ShowDialog();

            //string rez = dso.SelectOperation;
            //arrFormulaElementsInNames.Add(rez);
            //arrFormulaElementsInNumbers.Add(rez); 

            //return rez;
            return string.Empty;
		}

		/// <summary>
		/// принять формулу для проверки 
		/// и включения в конфигурацию
		/// </summary>
		internal XElement ApplyFormula()
		{
            //dlgInputNameAndDimension diad = new dlgInputNameAndDimension();
            //diad.ShowDialog();

            //string rez = string.Empty;

            //#region отладка
            //int cntarrf = arrFormulaElementsInNames.Count;
            //int cntarrn = slNumber2TCRZADev.Count;
            //int cntarrfe = arrFormulaElementsInNumbers.Count;			
            //#endregion

            //for (int i = 0; i < arrFormulaElementsInNumbers.Count; i++)
            //    rez += arrFormulaElementsInNumbers[i] + " ";

            //KeyValuePair<bool,string> kvof2 = new KeyValuePair<bool,string>(false, string.Empty);

            //KeyValuePair<bool, string> kofp = CreatePOLIS(rez);

            //if (!string.IsNullOrWhiteSpace(kofp.Value))
            //    return CreateXMLSection4Formula(kofp.Key, kofp.Value, diad.TagName, diad.TagDimension);

			return null;
		}


		/// <summary>
		/// string CreatePOLIS(string f)
		/// перевод формулы f в ПОЛИЗ
		/// </summary>
		/// <param Name="f"></param>
		/// <returns></returns>
		KeyValuePair<bool, string> CreatePOLIS(string f)
		{
			StringBuilder POLIS = new StringBuilder();   // формула в ПОЛИЗ

			// признак того, что формула возвращает логич результат
			bool isLogicalFormula = false;
	
			try
			{
				// f - формула в инфиксной записи
				// расщепляем строку с формулой
				string[] pieces = f.Split(' ');

				Stack stkOp = new Stack(); // стек операций

				// операции и их приоритеты
				Hashtable OperationPriority = new Hashtable();
				OperationPriority.Add("f", 0);   // вычисление внешней функции
				OperationPriority.Add("|", 1);
				OperationPriority.Add("&", 2);
				OperationPriority.Add("&~", 2);	// операция И-НЕ ко второму операнду
				OperationPriority.Add("*", 2);
				OperationPriority.Add("/", 2);
				OperationPriority.Add("+", 1);
				OperationPriority.Add("-", 1);
				OperationPriority.Add("<", 1);	// операция отношения
				OperationPriority.Add("(", 0);
				OperationPriority.Add(")", 0);
				OperationPriority.Add("ъ", 0);

				stkOp.Push("ъ");   // первый пустой элемент

				for (int i = 0; i < pieces.Length; i++)
				{
					string tsp = pieces[i];

					if (string.IsNullOrWhiteSpace(tsp))
						continue;

					if (Char.IsDigit(tsp, 0))
					{  // анализ очередного символа - если цифра то в конечную формулу
						POLIS.Append(' ');
						POLIS.Append(pieces[i]);
						continue;
					}
					else if (pieces[0] == "f")
					{
						POLIS.Append(' ');
						POLIS.Append(pieces[i]);
						continue;
					}

					// анализируем операции и наличие скобок
					if (!OperationPriority.ContainsKey(pieces[i]))
						System.Windows.Forms.MessageBox.Show("Недопустимый символ операции", "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

					// если стек не пуст, то сравниваем приоритеты операций от вершины стека с приоритетом новой операции
					int tmpPrtOpFi;
					int tmpPrtTopStk;

					switch (pieces[i])
					{
						case "f":
							stkOp.Pop();
							stkOp.Push("f");
							break;
						case "|":
							while ((tmpPrtOpFi = (int)OperationPriority["|"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("|");
							isLogicalFormula = true;
							break;
						case "&":
							while ((tmpPrtOpFi = (int)OperationPriority["&"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("&");
							isLogicalFormula = true;
							break;
						case "&~":
							while ((tmpPrtOpFi = (int)OperationPriority["&~"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("&~");
							isLogicalFormula = true;
							break;
						case "*":
							while ((tmpPrtOpFi = (int)OperationPriority["*"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("*");
							break;
						case "/":
							while ((tmpPrtOpFi = (int)OperationPriority["/"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("/");
							break;
						case "<":
							while ((tmpPrtOpFi = (int)OperationPriority["<"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("<");
							isLogicalFormula = true;
							break;
						case "+":
							while ((tmpPrtOpFi = (int)OperationPriority["+"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("+");
							break;
						case "-":
							while ((tmpPrtOpFi = (int)OperationPriority["-"]) <= (tmpPrtTopStk = (int)OperationPriority[stkOp.Peek()]))
							{
								POLIS.Append(' ');
								POLIS.Append(stkOp.Pop());
							}
							stkOp.Push("-");
							break;
						case "(":
							stkOp.Push(tsp);   // поместить в стек
							break;
						case ")":
							// снять со стека
							while ((tsp = (string)stkOp.Pop()) != "(")
							{
								POLIS.Append(' ');
								POLIS.Append(tsp);
							}
							break;
						default:
							break;
					}
				}
				for (int i = 0; i < stkOp.Count; i++)
				{
					string t = (string)stkOp.Pop();
					if (t == "ъ")
						continue;
					POLIS.Append(' ');
					POLIS.Append(t);
				}
			}
			catch (Exception ex)
			{
				POLIS.Clear();
				string rezerr = string.Empty;
				// формирование сообщения
				foreach (var item in arrFormulaElementsInNames)
				{
					rezerr += item;
				}
				System.Windows.Forms.MessageBox.Show("Ошибка при преобразовании формулы : " + rezerr, "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);               
			}

			return new KeyValuePair<bool, string>(isLogicalFormula,POLIS.ToString());
		}
		
		/// <summary>
		/// создать секцию для формулы
		/// </summary>
		private XElement CreateXMLSection4Formula(bool isFormulaLogical, string polisformula, string tagName, string tagDimension)
		{
			StringBuilder sbid = new StringBuilder();

			XElement xe_formula = new XElement("formula");

			if (isFormulaLogical)
				xe_formula.Add(new XAttribute("typeadapter", "BitFieldAdapter"));
			else
				xe_formula.Add(new XAttribute("typeadapter", "FloatFieldAdapter"));

			xe_formula.Add(new XAttribute("name", tagName));
			xe_formula.Add(new XAttribute("Caption", tagName));

			if ( !string.IsNullOrWhiteSpace(tagDimension))
				xe_formula.Add(new XAttribute("Dim", tagDimension));
			else
				xe_formula.Add(new XAttribute("Dim", string.Empty));

			xe_formula.Add(new XAttribute("commentary", ""));

			StringBuilder sbrez = new StringBuilder();
			for (int i = 0; i < arrFormulaElementsInNumbers.Count; i++)
				sbrez.Append(arrFormulaElementsInNumbers[i] + " ");

			xe_formula.Add(new XAttribute("formula4calculatedRaw", sbrez.ToString()));

			xe_formula.Add(new XAttribute("express", polisformula));

			// формируем элементы секции value
			//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
			for (int i = 0; i < slNumber2TCRZADev.Count; i++)
			{
				object obj = slNumber2TCRZADev.ElementAt(i).Value;

				XElement xe_value = new XElement("value");
				xe_value.Add(new XAttribute("id", slNumber2TCRZADev.ElementAt(i).Key));

				if (slNumber2TCRZADev.ElementAt(i).Value is ITag)
				{
					ITag tv = slNumber2TCRZADev.ElementAt(i).Value as ITag ;
					sbid.Clear();
					//sbid.Append(tv.Group.Device.NumFC + "." + tv.Group.Device.NumDev + "." + tv.Group.Id + "." + tv.RegInDev + "." + tv.bitMask);
					xe_value.Add(new XAttribute("tag", sbid.ToString()));
				}
				else if (IsEleventConst(slNumber2TCRZADev.ElementAt(i).Value))
				{
					xe_value.Add(new XAttribute("tag", "const=.0f" + slNumber2TCRZADev.ElementAt(i).Value));
				}

				xe_formula.Add(xe_value);
			}
			//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

			return xe_formula;
		}

		private bool IsEleventConst(object p)
		{
			float prez = 0;

			return float.TryParse(p as string, out prez);
		}

		internal void ClearFormula()
		{
			slNumber2TCRZADev.Clear();
			arrFormulaElementsInNames.Clear();
			arrFormulaElementsInNumbers.Clear();			
		}
	}
}

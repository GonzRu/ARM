/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Реализация новой версии реализации класс вычисления выразения по формуле,
 *	            формула для вычисления задается в ПОЛИЗ
 *                                                                             
 *	Файл                     : X:\Projects\33_Virica\Server_new_Interface\Calculator\FormulaEvalEv.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 17.02.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Text;
using CRZADevices;
using InterfaceLibrary;

namespace AdapterLib
{
	/// <summary>
	/// Тип распознавания
	/// </summary>
	public enum TypeFormulaElement
	{
		/// <summary>
		/// неизвестный тип
		/// </summary>
		NonImplement = 0,
		/// <summary>
		/// тег в конфигурации
		/// </summary>
		TagInConfig = 1,
		/// <summary>
		/// константа заданная либо как коэф трансф. 
		/// либо непоср при описании элементов формулы
		/// </summary>
		Constant = 2,
	}

	// делегат для события извещения об изменении знач. тега
    public delegate void ChangeValForm(object valForm );

	/// <summary>
	/// класс при котором формула вычисления 
	/// описана в файле Tимя_блока.cs
	/// </summary>
	public class FormulaEvalEv
	{
		#region public
		// событие по изменению результата вычисленной формулы
		public event ChangeValForm OnChangeValForm;
		#endregion

		#region private
		/// <summary>
		/// для работы без string'ов
		/// </summary>
		StringBuilder sbStrfwoSpace = new StringBuilder();
		/// <summary>
		/// стек операндов и промежуточных результатов
		/// </summary>
		Stack stkOperands = new Stack();
		/// <summary>
		/// массив реалиованных операций
		/// </summary>
		string[] arrOperations = { "*", "/", "+", "-", "!", "&", "|", "|+"/*логическое сложение битовых полей*/,">="/*больше или равно*/ };
		#endregion

		#region protected
		/// <summary>
		/// массив элементов формулы в порядке ПОЛИЗ
		/// </summary>
		protected string[] formulaElements;
		/// <summary>
		/// ссылка на вычисляемый тег
		/// </summary>
		protected TCRZAVariable tCRZAVariable;
		/// <summary>
		/// массив тегов для вычисления формулы
		/// </summary>
		protected ArrayList alstr = new ArrayList();
		/// <summary>
		/// словарь тегов с их типом распознавания
		/// </summary>
		protected Dictionary<object, TypeFormulaElement> dictLink2FormulaEval;
		#endregion

		#region Конструкторы
		public FormulaEvalEv()
		{
		}

        ///// <summary>
        ///// конструктор для первого варианта вычисления формулы
        ///// когда коэф тр задаются в секции TransformationRatio
        ///// файла PrgDevCFG.cdp
        ///// </summary>
        ///// <param name="tcrzaVariable"></param>
        ///// <param name="alstr"></param>
        ///// <param name="forml"></param>
        //public FormulaEvalEv(TCRZAVariable tcrzaVariable, ArrayList alstr, string forml)
        //{
        //    tCRZAVariable = tcrzaVariable;
			
        //    this.alstr = (ArrayList)alstr.Clone();

        //    formulaElements = GetFormulaElements(forml);
        //}
		#endregion

		#region GetFormulaElements(forml) - сформировать массив элементов в ПОЛИЗ-порядке
		/// <summary>
		/// сформировать массив элементов в ПОЛИЗ-порядке
		/// </summary>
		/// <param name="forml">формула для вычисления</param>
		protected string[] GetFormulaElements(string formula)
		{
			// удалим лишние пробелы				
			formula = DeleteBlanks(formula);

			return formula.Split(new char[] { ' ' });
		}

		/// <summary>
		/// привести формулу к каноническому виду - без лишних пробелов
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
		private string DeleteBlanks(string formula)
		{
			string[] strf = formula.Trim().Split(new char[] { ' ' });

			sbStrfwoSpace.Clear();

			for (int i = 0; i < strf.Length; i++)
				if (!string.IsNullOrEmpty(strf[i]))
					sbStrfwoSpace.Append(strf[i] + " ");

			return sbStrfwoSpace.ToString().Trim();
		}
		#endregion

		/// <summary>
		/// настройка на элементы формулы
		/// </summary>
		public virtual void CustomLinks2FormulaTags()
		{
			string key = string.Empty;

			this.dictLink2FormulaEval = new Dictionary<object, TypeFormulaElement>();
			/*
			 * читаем элементы массива alstr, формируем
			 * массив ссылок на соответсвующие экземпляры 
			 * классов тегов
			 */
			try
			{
				foreach (string str in alstr)
				{
					if (str.Contains("="))
					{
						/*
						 * константа, определяем какая именно - внешняя или внутр.
						 * и помещаем ее значение в словарь
						 */
						key = TestAndGetConstant(str);

						if (!dictLink2FormulaEval.ContainsKey(key))
							dictLink2FormulaEval.Add(key, TypeFormulaElement.Constant);
					}
					else
					{
                        ITag tcv = GetLink2CRZAVariable(str);
                        //TCRZAVariable tcv = GetLink2CRZAVariable(str);
						if (tcv != null)
							dictLink2FormulaEval.Add(tcv, TypeFormulaElement.TagInConfig);
						else
						{
							throw new ArgumentNullException("TCRZAVariable tcv", "Попытка получить доступ к несуществующему тегу");
						}
					}
				}

				// для тегов привязываемся к изменениям
				Link2Changes();
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

		/// <summary>
		/// определить местонахождение и значение константы
		/// </summary>
		/// <param name="str"></param>
		protected string TestAndGetConstant(string str)
		{
			string[] strelems = str.Trim().Split(new[] { '=' });
			try
			{
				switch (strelems[0])
				{
					case "ext":
						sbStrfwoSpace.Clear();
						sbStrfwoSpace.Append(GetExternalConst(strelems));
						break;
					case "const":
					case "internal":
						sbStrfwoSpace.Clear();
						sbStrfwoSpace.Append(GetInternalConst(strelems));
						break;
					default:
						throw new ArgumentException("Тип константы =" + strelems[0] + "= не поддерживается");
				}
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			return sbStrfwoSpace.ToString();
		}

		private string GetInternalConst(string[] strelems)
		{
			return strelems[1];
		}

		private string GetExternalConst(string[] strelems)
		{
			if (tCRZAVariable.Group.Device.slKoefRatioValue.ContainsKey(strelems[1]))
				return tCRZAVariable.Group.Device.slKoefRatioValue[strelems[1]];

			return strelems[2];	// значение по умолчанию
		}

        /// <summary>
        /// извлечь тег для связи с ним
        /// </summary>
        /// <param name="str"></param>
        protected virtual ITag GetLink2CRZAVariable(string str)
        {
            ITag tag = null;
            string[] strEl = str.Split(new char[] { '.' });

            // ищем тег в текущем устройстве
            try
            {
                string[] arrstr = str.Split(new char[] { '.' });

                uint tagguid = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetTagGUID(0, "MOA_ECU", string.Format("{0}.{1}", arrstr[3], arrstr[4]));

                tag = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Tag(0, uint.Parse(arrstr[1]), (uint)tagguid);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return tag;
        }
        
        /// <summary>
		/// привязаться к обновлениям тегов
		/// </summary>
		protected void Link2Changes()
		{
            foreach (object obj in dictLink2FormulaEval.Keys)
                if (obj is ITag)
                    (obj as ITag).OnChangeVar += new ChVarNewDs(FormulaEvalEv_OnChangeVar);
		}

        /// <summary>
        /// функция вызываемая по событию изменения тега
        /// </summary>
        void FormulaEvalEv_OnChangeVar(Tuple<string, byte[], DateTime, VarQualityNewDs> var)
        {
            EvalFormula();
        }

		/// <summary>
		/// вычисление формулы
		/// </summary>
		public virtual void EvalFormula()
		{
			// проходим по ПОЛИЗ представлению формулы
			// если формула состоит из одного операнда
			// то нужно просто возвратить его значение
			// шаблон для исключений
			try
			{
			    if (formulaElements.Length == 1)
					//stkOperands.Push((dictLink2FormulaEval.Keys.ElementAt(0) as TCRZAVariable).ExtractTagValueAsString());
                    stkOperands.Push((dictLink2FormulaEval.Keys.ElementAt(0) as ITag).ValueAsString);
				else
				{
					for (int i = 0; i < formulaElements.Length; i++)
						if (IsFormulaElementOperation(formulaElements[i]))
							GetRezOperation(formulaElements[i]);
						else
							stkOperands.Push(formulaElements[i]);
				}

                // снимаем результат вычисления формулы с вершины стека
                if ((OnChangeValForm != null) && (stkOperands.Count != 0))
					OnChangeValForm(stkOperands.Pop());
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

		/// <summary>
		/// определение факта того, что очередной элемент формулы является обозначением операции
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		private bool IsFormulaElementOperation(string p)
		{
			return arrOperations.Contains(p);
		}

		/// <summary>
		/// получить результат операции
		/// </summary>
		/// <param name="op">строка обозначающая операцию</param>
		private void GetRezOperation(string op)
		{
        			try
			{
                switch (op)
                {
                    case "*":
                        DoMultiply();
                        break;
                    case "/":
                        DoDivision();
                        break;
                    case "+":
                        DoAddition();
                        break;
                    case "-":
                        DoSubtraction();
                        break;
                    case "!":
                        DoLogicalNo();
                        break;
                    case "|":
                        DoLogicalOR();
                        break;
                    case "&":
                        DoLogicalAND();
                        break;
                    case "|+":
                        DoLogicalSum();
                        break;
                    case ">=":
                        DoLogicalOpMoreOrEq();
                        break;
                    default:
                        break;
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}

		/// <summary>
		/// Извлечь очередной элемент типа Single с вершины стека		
		/// </summary>
		/// <returns></returns>
		private Single GetStackItem(Single singleReq)
		{
			int indx = 0;
            Single stemp = 0;

			try
			{
				if (stkOperands.Peek() is string)
				{
					indx = Convert.ToInt32(stkOperands.Pop());
                    if (dictLink2FormulaEval.Keys.ElementAt(indx) is ITag)// TCRZAVariable
                    {
                        if (!Single.TryParse((dictLink2FormulaEval.Keys.ElementAt(indx) as ITag).ValueAsString, out stemp))
                        stemp = 0;
                        //return Convert.ToSingle((dictLink2FormulaEval.Keys.ElementAt(indx) as TCRZAVariable).ExtractTagValueAsString()); 
                        //stemp = Convert.ToSingle((;//TCRZAVariable .ExtractTagValueAsString());
                    }
					else if (dictLink2FormulaEval.Keys.ElementAt(indx) is string)
					{
						string tmpS = dictLink2FormulaEval.Keys.ElementAt(indx).ToString();
						if (TestConst(ref tmpS))
							stemp = Convert.ToSingle(tmpS);
						else
							throw new ArgumentException("Недопустимый операнд формулы");
					}
					else
					{
						throw new ArgumentException("Недопустимый операнд формулы");
					}
				}
				else
					stemp = Convert.ToSingle(stkOperands.Pop());

                return stemp;
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}
		}

		/// <summary>
		/// Извлечь очередной элемент типа Boolean с вершины стека		
		/// </summary>
		/// <returns></returns>
		private Boolean GetStackItem(Boolean singleReq)
		{
			int indx = 0;

			try
			{
				if (stkOperands.Peek() is string)
				{
					indx = Convert.ToInt32(stkOperands.Pop());
					if (dictLink2FormulaEval.Keys.ElementAt(indx) is TCRZAVariable)
					{
						return Convert.ToBoolean((dictLink2FormulaEval.Keys.ElementAt(indx) as TCRZAVariable).ExtractTagValueAsString());
					}
					else if (dictLink2FormulaEval.Keys.ElementAt(indx) is string)
					{
						string tmpS = dictLink2FormulaEval.Keys.ElementAt(indx).ToString();
						if (TestConst(ref tmpS))
							return Convert.ToBoolean(tmpS);
						else
							throw new ArgumentException("Недопустимый операнд формулы");
					}
					else
					{
						throw new ArgumentException("Недопустимый операнд формулы");
					}
				}
				else
					return Convert.ToBoolean(stkOperands.Pop());
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw ex;
			}
		}

		/// <summary>
		/// string TestConst(string tmpS)
		/// проверка на константу с фиксир или плавающей точкой
		/// </summary>
		/// <param name="tmpS">правильная строка - числовая константа</param>
		/// <returns></returns>
		bool TestConst(ref string tmpS)
		{
            bool rez = false;

            try
			{
                if (tmpS.Contains("d"))
                {
                    // расщепляем строку с константой
                    string[] pieces = tmpS.Split('d');
                    tmpS = pieces[1];
                    rez = true;
                }
                else if (tmpS.Contains("f"))
                {
                    // расщепляем строку с константой
                    string[] pieces = tmpS.Split('f');
                    tmpS = pieces[1];

                    CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
                    newCInfo.NumberFormat.NumberDecimalSeparator = ".";
                    Thread.CurrentThread.CurrentCulture = newCInfo;

                    if (tmpS.Contains(","))
                        tmpS = tmpS.Replace(",", ".");

                    rez = true;
                }
                else
                    rez = false; // возвращаем строку неизменной
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            return rez;
		}

		#region реализованные операции
		/// <summary>
		/// умножение, результат на вершину стека
		/// </summary>
		private void DoMultiply()
		{
			Single op1=0, op2=0;

			/*
			 * извлекаем операнды с вершины стека и умножаем, для этого
			 * последовательно смотрим два верхних элемента стека, 
			 * если это строка то это индекс нужного тега в dictLink2FormulaEval, 
			 * если это что-то другое, то скорее всего это промежуточный результат
			 * предшествующего вычисления
			 */
			try
			{
				op1 = Convert.ToSingle(GetStackItem(op1)); // =1=

				op2 = Convert.ToSingle(GetStackItem(op2)); // =2=
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			// все нормально - перемножаем - рез в стек
			stkOperands.Push(op1 * op2);
		}

		/// <summary>
		/// деление, результат на вершину стека
		/// </summary>
		private void DoDivision()
		{
			Single op1=0, op2=0;

			/*
			 * извлекаем операнды с вершины стека и делим, для этого
			 * последовательно смотрим два верхних элемента стека, 
			 * если это строка то это индекс нужного тега в dictLink2FormulaEval, 
			 * если это что-то другое, то скорее всего это промежуточный результат
			 * предшествующего вычисления
			 */
			try
			{
				op1 = Convert.ToSingle(GetStackItem(op1)); // =1=

				op2 = Convert.ToSingle(GetStackItem(op2)); // =2=
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			// все нормально - делим - рез в стек
			stkOperands.Push(op2 / op1);
		}

		/// <summary>
		/// Сложение, результат на вершину стека
		/// </summary>
		private void DoAddition()
		{
			Single op1=0, op2=0;

			/*
			 * извлекаем операнды с вершины стека и складываем, для этого
			 * последовательно смотрим два верхних элемента стека, 
			 * если это строка то это индекс нужного тега в dictLink2FormulaEval, 
			 * если это что-то другое, то скорее всего это промежуточный результат
			 * предшествующего вычисления
			 */
			try
			{
				op1 = Convert.ToSingle(GetStackItem(op1)); // =1=

				op2 = Convert.ToSingle(GetStackItem(op2)); // =2=
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			// все нормально - складываем - рез в стек
			stkOperands.Push(op1 + op2);
		}

		/// <summary>
		/// Вычитание, результат на вершину стека
		/// </summary>
		private void DoSubtraction()
		{
			Single op1 = 0, op2 =0;

			/*
			 * извлекаем операнды с вершины стека и вычитаем, для этого
			 * последовательно смотрим два верхних элемента стека, 
			 * если это строка то это индекс нужного тега в dictLink2FormulaEval, 
			 * если это что-то другое, то скорее всего это промежуточный результат
			 * предшествующего вычисления
			 */
			try
			{
				op1 = Convert.ToSingle(GetStackItem(op1)); // =1=

				op2 = Convert.ToSingle(GetStackItem(op2)); // =2=
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			// все нормально - вычитаем - рез в стек
			stkOperands.Push(op2 - op1);
		}

		/// <summary>
		/// ! - Логическая операция отрицания,
		/// рез на вершину стека
		/// </summary>
		private void DoLogicalNo()
		{
			Boolean op1 = false;

			/*
			 * извлекаем операнд с вершины стека и 
			 * выплоняем на ним лог отрицание
			 */
			try
			{
				op1 = Convert.ToBoolean(GetStackItem(op1)); // =1=
				op1 = !op1;
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			// все нормально - вычитаем - рез в стек
			stkOperands.Push(op1);
		}
		/// <summary>
		/// | - Логическая операция ИЛИ,
		/// рез на вершину стека
		/// </summary>
		private void DoLogicalOR()
		{
			Boolean op1 = false, op2 = false;

			/*
			 * извлекаем операнды с вершины стека и 
			 * выполняем операцию логического ИЛИ
			 */
			try
			{
				op1 = Convert.ToBoolean(GetStackItem(op1)); // =1=

				op2 = Convert.ToBoolean(GetStackItem(op2)); // =2=
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			// все нормально - выполняем логическое ИЛИ
			stkOperands.Push(op1 || op2);
		}
		/// <summary>
		/// & - Логическая операция И,
		/// рез на вершину стека
		/// </summary>
		private void DoLogicalAND()
		{
			Boolean op1 = false, op2=false;

			/*
			 * извлекаем операнды с вершины стека и 
			 * выполняем операцию логического И
			 */
			try
			{
				op1 = Convert.ToBoolean(GetStackItem(op1)); // =1=

				op2 = Convert.ToBoolean(GetStackItem(op2)); // =2=
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw ex;
			}

			// все нормально - выполняем логическое И
			stkOperands.Push(op1 && op2);
		}
		/// <summary>
		/// |+ Логическое сложение
		/// </summary>
		private void DoLogicalSum()
		{
			throw new Exception("Операция |+ Логическое сложение НЕ РЕАЛИЗОВАНА");
		}

		/// <summary>
		/// больше или равно
		/// </summary>
		private void DoLogicalOpMoreOrEq()
		{
			Single op1 = 0, op2 = 0;

			/*
			 * извлекаем операнды с вершины стека и сравниваем, для этого
			 * последовательно смотрим два верхних элемента стека, 
			 * если это строка то это индекс нужного тега в dictLink2FormulaEval, 
			 * если это что-то другое, то скорее всего это промежуточный результат
			 * предшествующего вычисления
			 */
			try
			{
				op1 = Convert.ToSingle(GetStackItem(op1)); // =1=

				op2 = Convert.ToSingle(GetStackItem(op2)); // =2=
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw;
			}

			// все нормально - вычитаем - рез в стек
			stkOperands.Push(op2 >= op1);
		}
		#endregion
	}
}

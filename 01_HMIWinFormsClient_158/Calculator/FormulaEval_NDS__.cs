/*#############################################################################*
 *    Copyright (C) 2006 Mehanotronika Corporation.                            *
 *    All rights reserved.                                                     *
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 *                                                                             *
 *	Описание: Содержит классы представляющие вычисляемые значения. Используя    *
 *					эти классы ИЭ получают готовые для дальнейшего форматирования   *
 *					значения отдельных или комплексированных между собой значений   *
 *					регистров или адресов памяти устройства								 *
 *                                                                             *
 *	Файл                     : Calculator.cs                                    *
 *	Тип конечного файла      : Calculator.dll                                   *
 *	версия ПО для разработки : С#, Framework 2.0                                *
 *	Разработчик              : Юров В.И.                                        *
 *	Дата начала разработки   : 08.03.2007                                       *
 *	Дата (v1.0)              :                                                  *
 *******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using MtExceptionHandler;
using WindowsForms;
using InterfaceLibrary;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Calculator
{
	//public delegate void ChangeTag(object valTag, CRZADevices.VarQuality vq); // для события извещения об изменении знач. тега
	//public delegate void ChangeTagNewDS(object valTag, InterfaceLibrary.VarQualityNewDS vq); // для события извещения об изменении знач. тега

	//// Категории вкладок и панелей на них, куда д.б. помещен элемент управления
	//public enum TypeOfPanel
	//{
	//    no,                     // первый элемент - не привязывается к панелям
	//    CurrentStatusReg,       // текущая, регистр статуса
	//    Current_Analog,         // текущая, аналоговые сигналы
	//    Current_DiscretIn,      // текущая, входные дискретные сигналы
	//    Current_DiscretOut,     // текущая, выходные дискретные сигналы
	//    CurrentControlProgUst,  // текущая, управление программой уставок
	//    CurrentDirection_P,     // текущая, направление мощности
	//    CurrentCounters,        // текущая, счетчики
	//    Avar_AS_PSZ,            // аварийная, аналоговые сигналы, признаки срабатывания защиты
	//    Avar_AS_PPZ,            // аварийная, аналоговые сигналы, признаки пуска защиты
	//    Avar_AS_AsPZ,           // аварийная, аналоговые сигналы, аналоговые сигналы пуска защиты
	//    Avar_AS_AsSZ,           // аварийная, аналоговые сигналы, аналоговые сигналы срабатывания защиты
	//    Avar_DS_In,             // аварийная, дискретные сигналы, входы
	//    Avar_DS_InChange,       // аварийная, дискретные сигналы, входы-изменения
	//    Avar_DS_Out,            // аварийная, дискретные сигналы, выходы
	//    Avar_DS_OutChange,      // аварийная, дискретные сигналы, выходы-изменения
	//    System_Vizov,           // система, причина сигнала вызов
	//    System_Vizov_vkl,       // система, причина сигнала вызов на вкладке
	//    System_Test,            // система, результаты тестирования БМРЗ
	//    System_StatusDevice,    // система, состояние устройства
	//    System_StatusCommand,   // система, состояние выполнения команд
	//    Store_I_IntegralOtkl,
	//    Store_I_lastOtkl,
	//    Store_CountEvent,
	//    Store_Maxmeter,
	//    Config_Ustavki,           //!!!!! убрать !!!!!!
	//    Config_Configuration,     //!!!!! убрать !!!!!!
	//    Config_Ustavki_0,         // конфигурация и уставки - 4 вкладки
	//    Config_Ustavki_1,
	//    Config_Ustavki_2,
	//    Config_Ustavki_3,
	//    Config_Ustavki_4,
	//    Config_Ustavki_5,
	//    Status_Dev,            // состояние устройства и команд
	//    Status_Com,
	//    StatusFC,				//состояние ШАСУ
	//    StatusCmdFC,				// состояние команд ШАСУ
	//    System_BottomPanel,	// вкладка система - нижняя панель
	//    Avar_BottomPanel,	// вкладка аварий - нижняя панель
	//    Store_BottomPanel,	// вкладка накопительной - нижняя панель
	//     Config_BottomPanel,	// вкладка конфиг и уст - нижняя панель
	//     // ресурс для блока ТП
	//     TP_ResursUst,
	//     TP_ResursCount,
	//     TP_ResursOther,
	//     TP_ResursUst_Commut,
	//        SYMAP_Status1,
	//    SYMAP_Status2,
	//    SYMAP_Status_Bus2,
	//     SYMAP_Status2_2,
	//    SYMAP_Ident,
	//    SYMAP_Time,
	//    SYMAP_P,
	//    SYMAP_AbsVal,
	//    SYMAP_TempVal,
	//    SYMAP_AllSymap,
	//    SYMAP_Twork,
	//     NormModePanel_Analog,	// панель норм реж - аналог сигналы
	//     NormModePanel_Discret,	// панель норм реж - дискретн сигналы
	//};

    // категория тега - аналоговый, дискретный и т.д.
    public enum TypeOfTag
    {
        no,             // первый элемент - вне категории
        Analog,         // аналоговый
        Discret,        // дискретный
        Combo           //  ComboBox
    };

	//------------------------------------------------------------------------
   // /// <summary>
   // /// структура для хранения результата вычисления формулы
   // /// </summary>
   //public struct RezFormulaEval
   //{
   //   public string IdTagIE;			// строка, идентифицирующая интерфейсный элемент в формате "fc.dev.group.var.bitmask"
   //   public string CaptionIE;          // строка-название подписи интерфейсного элемента
   //   public string DimIE;              // строка-название размерности интерфейсного элемента
   //   public object bitMaskValue;       // для битовых значений - результат формулы в виде битовой маски
   //   public object Value;              // для битовых значений - результат формулы в виде bool-значения
   //   public string TypeTag;
   //   public bool IsValOutOfBounds;    // факт выхода за пределы диапазона допустимых значений
   //   public string strKTR;            // коэффициент трансформации в всиде строки (для уставок сириуса)
   //}

   #region public struct TagVal - структура для хранения значения тега
   //------------------------------------------------------------------------
   // структура для хранения значения тега
   //public struct TagVal
   //{
   //   public int indTag;	// номер тега в исходной последовательности arrTag
   //   public int typeDev;	// тип - от него может зависеть порядок трактовки содержимого полей структуры:
   //   // 0 - для БМРЗ
   //   public string strTagIdent;	// подстрока из исходного arrTag, для идентификации тега

   //   public string CaptionTag;  // строка-название подписи тега (из CRZADevice)
   //   public string DimTag;      // строка-название размерности тега (из CRZADevice)
   //   public string NameTag;      // строка-название имени тега (из CRZADevice)

   //   // выделенные из arrTag элементы, идентифицирующие тег
   //   public int FC;					// номер ФК
   //   public int FD;					// номер устройства
   //   public int FG;					// номер группы
   //   public int FV;					// номер переменной
   //   public string FB;				// битовое поле
   //   public TagEval linkTagEval;	// ссылка на TagEval данного тега (привязка)
   //   public object value;			// извлеченное значение тега
   //   public string notice;			// резерв для будущего использования

   //   // флаги, признаки
   //   public bool IsTagValValueIncorrect;
   //   /// <summary>
   //   /// признак что этот тег - константа
   //   /// </summary>
   //   public bool IsThisTagConst;
   //   /// <summary>
   //   /// значение тега - константы
   //   /// </summary>
   //   public string ValueConstAsString; 
   //}   
   #endregion

	/// <summary>
	/// структура для хранения результата вычисления формулы
	/// </summary>
	public struct RezFormulaEval
	{
		/// <summary>
		/// строка, идентифицирующая интерфейсный элемент 
		/// в формате "fc.dev.group.var.bitmask"
		/// </summary>
		public string IdTagIE;
		/// <summary>
		/// строка-название подписи интерфейсного элемента
		/// </summary>
		public string CaptionIE;
		/// <summary>
		/// строка-название размерности интерфейсного элемента
		/// </summary>
		public string DimIE;
		/// <summary>
		/// для битовых значений - 
		/// результат формулы в виде битовой маски
		/// </summary>
		public object bitMaskValue;
		/// <summary>
		/// для битовых значений - 
		/// результат формулы в виде bool-значения
		/// </summary>
		public object Value;
		/// <summary>
		/// тип тега в нотации верхнего уровня, 
		/// например TIntVariable
		/// </summary>
		public string TypeTag;
		/// <summary>
		/// факт выхода за пределы диапазона допустимых значений
		/// </summary>
		public bool IsValOutOfBounds;
		/// <summary>
		/// коэффициент трансформации в всиде строки (для уставок сириуса)
		/// </summary>
		public string strKTR;
	}

    /// <summary>
    /// class FormulaEval
    /// класс, представляющий вычисляемое значение отдельного тега или 
    /// комплексированное по заданной формуле
    /// </summary>	
   public class FormulaEvalNDS
	{
		/// <summary>
		/// тип панели куда помещается элемент
		/// </summary>
		public string ToP;
		/// <summary>
		/// категория тега
		/// </summary>
		public TypeOfTag ToT;
		public string SourceFormula;
		IConfiguration configuration;	//поле для связи с конфигурацией нового DS
		public ITag LinkVariableNewDS;   // привязка к переменной в новом DS

		int flFC = 0;
		int flDevice = 0;
		int flGroup = 0;
		int flVariable = 0;
		string flBitMsk = string.Empty;

		public object Value; // текущее значение тега
		public string NameFE;		// имя переменной (из конфигурации)
		public string CaptionFE;	// подпись переменной
		public string Dim;			// размерность переменной
		public ArrayList arrStrCB;  // массив строк для ComboBox
		public string TypeVar = String.Empty;   // тип переменной, понадобился в уставках для бмрз-100
		public RezFormulaEval tRezFormulaEval;

		#region старый код
		/// <summary>
		/// строка с тегами, участвующими в формуле
		/// </summary>
		//private string arrTag;
		/// <summary>
		/// формула в ПОЛИЗ
		/// </summary>
		//private string formula;

		/// <summary>
		/// массив тегов (структур TagVal)
		/// </summary>
		//public ArrayList arrTagVal;
		//public uint addrVar;
		//public string addrVarBitMask = null;
		/// <summary>
		/// битовая маска, если нужно выделять часть значения как с уставками
		/// </summary>
		//public string bitmask;
		/// <summary>
		/// доступ по чтению/записи
		/// </summary>
		//public string ReadWrite;
		/// <summary>
		/// строка формата, 
		/// можно задавать точность чисел с ПЗ
		/// </summary>
		//private string strFormat = String.Empty;
		//public string StrFormat
		// {
		//     set
		//     {
		//         if( value == "" )
		//             value = "0";
		//     else if (value == "-1")   // точность - по факту (важно для уставок Сириус и др.)
		//        value = "-1";
		//     else if (!Char.IsNumber((value.Trim()),0))
		//        throw new Exception(" (177) : FormulaEval: неправильное значение точности для чисел с плавающей точкой : StrFormat = " + value);

		//            strFormat = value;
		//     }
		//     get
		//     {
		//         return strFormat;
		//     }
		// }

		/// <summary>
		/// делегат для события извещения об изменении знач. тега
		/// </summary>
		/// <param name="valForm"></param>
		/// <param name="format"></param>
		//public delegate void ChangeValForm(object valForm, string format);
		/// <summary>
		/// событие по изменению результата вычисленной формулы
		/// </summary>
		//public event ChangeValForm OnChangeValForm;

		/// <summary>
		/// другой вариант делегата 
		/// для события извещения об изменении знач. тега
		/// </summary>
		/// <param name="strtagident"></param>
		/// <param name="valTag"></param>
		/// <returns></returns>
		public delegate bool ChangeValFormTI(string strtagident, object valTag);
		/// <summary>
		/// другой вариант события 
		/// по изменению результата вычисленной формулы
		/// </summary>
		public event ChangeValFormTI OnChangeValFormTI;		
		#endregion

		/// <summary>
		/// конструктор
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="aTag"></param>
		/// <param name="f"></param>
		/// <param name="aCaptionIE"></param>
		/// <param name="aDimIE"></param>
		/// <param name="tot"></param>
		/// <param name="toP"></param>
		public FormulaEvalNDS(IConfiguration configuration, string aTag, string f, string aCaptionIE, string aDimIE, TypeOfTag tot, string toP)
		{
			ToP = toP;
			ToT = tot;

			SourceFormula = aTag;

			/*--------------------------------------------------------------------
			 *	формат строки массива тэгов:
			 * 0(FC.FD.FG.FV.FB)1(xxx)..n(xxx), где
			 * цифры 0..n - номера тегов, кот. будут индексами в массиве со значениями этих тегов
			 * FC - номер ФК
			 * FD	- номер устройства
			 * FG - номер группы
			 * FV - адрес переменная
			 * FB - битовая маска
			 *--------------------------------------------------------------------*/
			/*
			 * используем регулярное выражение для выделения 
			 * подстрок вида цифры.цифры.цифры
			 */
			try
			{
				Regex re = new Regex(@"[\d]+\.[\d]+\.[\d]+.[\d]+.[\d]+");

				Match m = re.Match(SourceFormula);

				string[] stidt = m.Value.Split(new char[]{'.'});
				flFC = int.Parse(stidt[0]);
				flDevice= int.Parse(stidt[1]);
				flGroup = int.Parse(stidt[2]);
				flVariable = int.Parse(stidt[3]);
				flBitMsk = stidt[4];

				if (!TagControlSet(configuration, flFC, flDevice, flGroup, flVariable, flBitMsk, aCaptionIE, aDimIE))
					throw new Exception(string.Format("(377) : FormulaEval_NDS.cs : FormulaEvalNDS() : Несуществующий тег : aTag = {0};\n" + "f = {1};\n" + "aCaptionIE = {2}.\n", aTag, f, aCaptionIE));
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

        /// конструктор 2
        /// </summary>
        /// <param Name="aFB">- конфигурация</param>
        /// <param Name="aTag">- строка определенного формата, задающая массив тегов</param>
        /// <param Name="f">- строка, задающая формулу вычислений интегрального значения</param>
        /// <param Name="aCaptionIE">- строка задающая подпись интерфейсного элемента</param>
        /// <param Name="aDimIE">- строка задающая размерность интерфейсного элемента</param>
        /// <param Name="tot">- тип тега (дискретный, аналоговый, combobox)</param>
        /// <param Name="toP">- тип панели</param>
		  /// /// <param Name="advOptions">- доп. опции</param>		  
		 public FormulaEvalNDS( IConfiguration configuration, string aTag, string f, string aCaptionIE, string aDimIE, TypeOfTag tot, string toP, object advOptions )
			: this(configuration, aTag, f, aCaptionIE, aDimIE, tot, toP)
		 {
			 if( advOptions is string )
				 flBitMsk = (string)advOptions;
		 }

         /// <summary>
         /// конструктор для нового DataServer
         /// </summary>
         /// <param name="configuration"></param>
         /// <param name="aTag"></param>
         /// <param name="f"></param>
         /// <param name="aCaptionIE"></param>
         /// <param name="aDimIE"></param>
         /// <param name="tot"></param>
         /// <param name="toP"></param>
         public FormulaEvalNDS(IConfiguration configuration, string aTag, string aCaptionIE, string aDimIE, TypeOfTag tot, string toP)
         {
             ToP = toP;
             ToT = tot;

             SourceFormula = aTag;

             /*--------------------------------------------------------------------
              *	формат строки массива тэгов:
              * 0(FC.FD.FG.FV.FB)1(xxx)..n(xxx), где
              * цифры 0..n - номера тегов, кот. будут индексами в массиве со значениями этих тегов
              * FC - номер ФК
              * FD	- номер устройства
              * FG - номер группы
              * FV - адрес переменная
              * FB - битовая маска
              *--------------------------------------------------------------------*/
             /*
              * используем регулярное выражение для выделения 
              * подстрок вида цифры.цифры.цифры
              */
             try
             {
                 Regex re = new Regex(@"[\d]+\.[\d]+\.[\d]+");//.[\d]+.[\d]+;

                 Match m = re.Match(SourceFormula);

                 string[] stidt = m.Value.Split(new char[] { '.' });
                 uint flDS = uint.Parse(stidt[0]);
                 uint flDevice = uint.Parse(stidt[1]);
                 uint flTagGuid = uint.Parse(stidt[2]);

                 if (!TagControlSet(configuration, flDS, flDevice, flTagGuid, aCaptionIE, aDimIE))
                     throw new Exception(string.Format("(377) : FormulaEval_NDS.cs : FormulaEvalNDS() : Несуществующий тег : aTag = {0};\n" + "f = {1};\n" + "aCaptionIE = {2}.\n", aTag, aCaptionIE));
             }
             catch (Exception ex)
             {
                 TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
             }
         }
         /// <summary>
         /// связка с тегом 1
         /// </summary>
         /// <param name="configuration"></param>
         /// <param name="lFC"></param>
         /// <param name="lDevice"></param>
         /// <param name="lGroup"></param>
         /// <param name="lVariable"></param>
         /// <param name="lBitMsk"></param>
         /// <returns></returns>
         bool TagControlSet(IConfiguration configuration, int lFC, int lDevice, int lGroup, int lVariable, string lBitMsk, string aCaptionIE, string aDimIE)
		{
            try
			{
                this.configuration = configuration;
                flFC = lFC;
                flDevice = lDevice;
                flGroup = lGroup;
                flVariable = lVariable;
                flBitMsk = lBitMsk;

                uint tagguid = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetTagGUID(0, "MOA_ECU", string.Format("{0}.{1}", lVariable.ToString(), lBitMsk));

                uint guidevice = (uint)(lFC * 256 + lDevice);

                ITag tag = configuration.GetLink2Tag(0, guidevice, (uint)tagguid);//(uint)lDevice

                if (tag == null)
                    return false;	// тег не найден

                LinkVariableNewDS = tag;
                // для всех переменных присваиваем имена
                tRezFormulaEval.CaptionIE = NameFE = aCaptionIE;// LinkVariableNewDS.TagName;     // aVariable.Name;			// имя переменной (из конфигурации)
                //CaptionFE = aCaptionIE;//  LinkVariableNewDS.TagName;                              ///aVariable.Caption;	    // подпись переменной
                tRezFormulaEval.DimIE = CaptionFE = aDimIE;// Dim = LinkVariableNewDS.Unit;               // aVariable.Dim;			// размерность переменной

                // формируем строку идентифицирующую тег
                tRezFormulaEval.IdTagIE = string.Format("{0}.{1}.{2}.{3}.{4}", lFC, lDevice, lGroup, lVariable, lBitMsk);
                LinkVariableNewDS.OnChangeVar += new ChVarNewDs(LinkVariableNewDS_OnChangeVar);

                //Value = (object)tmpVInt.Value;
                //TypeVar = "TIntVariable";
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

			return true;
		}

        /// <summary>
        /// связка с тегом 2
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="lFC"></param>
        /// <param name="lDevice"></param>
        /// <param name="lGroup"></param>
        /// <param name="lVariable"></param>
        /// <param name="lBitMsk"></param>
        /// <returns></returns>
         bool TagControlSet(IConfiguration configuration, uint lds, uint lDevice, uint tagGuid, string aCaptionIE, string aDimIE)
        {
            this.configuration = configuration;
            //flFC = lFC;
            //flDevice = lDevice;
            //flGroup = lGroup;
            //flVariable = lVariable;
            //flBitMsk = lBitMsk;

            //uint tagguid = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetTagGUID(0, "MOA_ECU", string.Format("{0}.{1}", lVariable.ToString(), lBitMsk));

            //uint guidevice = (uint)(lFC * 256 + lDevice);

            ITag tag = configuration.GetLink2Tag(0, lDevice, (uint)tagGuid);//(uint)lDevice

            if (tag == null)
                return false;	// тег не найден

            LinkVariableNewDS = tag;
            // для всех переменных присваиваем имена
            tRezFormulaEval.CaptionIE = NameFE = aCaptionIE;// LinkVariableNewDS.TagName;     // aVariable.Name;			// имя переменной (из конфигурации)
            CaptionFE = LinkVariableNewDS.TagName;                              ///aVariable.Caption;	    // подпись переменной
            tRezFormulaEval.DimIE = Dim = aDimIE;// LinkVariableNewDS.Unit;               // aVariable.Dim;			// размерность переменной

            // формируем строку идентифицирующую тег
            //tRezFormulaEval.IdTagIE = string.Format("{0}.{1}.{2}.{3}.{4}", lFC, lDevice, lGroup, lVariable, lBitMsk);

            LinkVariableNewDS.OnChangeVar += new ChVarNewDs(LinkVariableNewDS_OnChangeVar);

            //Value = (object)tmpVInt.Value;
            //TypeVar = "TIntVariable";

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="var"></param>
        void LinkVariableNewDS_OnChangeVar(Tuple<string, byte[], DateTime, VarQualityNewDS> var)
		{
			/*
			 * OnChangeValFormTI - событие для тегов мнемосхемы
			 * у ВП воспринимаются значения как int32 - дискретные
			 * и аналоговое (состояние протокола из рег. 60013)
			 */
			if (OnChangeValFormTI != null)
			{
				if (var.Item2.Length == 1)
				{ 
					byte[] tmpb = new byte[4];
					tmpb[0] = var.Item2[0];

					OnChangeValFormTI(this.tRezFormulaEval.IdTagIE, BitConverter.ToInt32(tmpb, 0));
				}
				else if (var.Item2.Length == 4)
				{
					OnChangeValFormTI(this.tRezFormulaEval.IdTagIE, BitConverter.ToInt32(var.Item2, 0));

					//byte[] tmpb = new byte[4];
					//tmpb[0] = 4;// var.Item2[0];
					//OnChangeValFormTI(this.tRezFormulaEval.IdTagIE, BitConverter.ToInt32(tmpb, 0));
				}
			}
		}
  }
}

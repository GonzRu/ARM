using System;
using System.Collections;
using System.Collections.Generic;
using InterfaceLibrary;

namespace AdapterLib
{
	/// <summary>
	/// класс для вычисления формулы
	/// связанной с некоторым Адаптером
	/// </summary>
    public	class FormulaEvalEv4Adapter : FormulaEvalEv
	{
		ArrayList alstr_local = new ArrayList();
		ArrayList KB = new ArrayList();
		/// <summary>
		/// конструктор для вычисления формулы заданной для 
		/// некоторого адаптера
		/// </summary>
		/// <param name="tcrzaVariable"></param>
		/// <param name="alstr"></param>
		/// <param name="forml"></param>
        public FormulaEvalEv4Adapter(ArrayList alstr, string forml)//ArrayList kb, 
		{
			this.alstr_local = (ArrayList)alstr.Clone();

			formulaElements = GetFormulaElements(forml);
			//KB = kb;
			CustomLinks2FormulaTags();
		}
		/// <summary>
		/// настройка на элементы формулы
		/// </summary>
		public override void CustomLinks2FormulaTags()
		{
			string key = string.Empty;

			this.dictLink2FormulaEval = new Dictionary<object, TypeFormulaElement>();
			/*
			 * читаем элементы массива alstr_local, формируем
			 * массив ссылок на соответсвующие экземпляры 
			 * классов тегов
			 */
			try
			{
				foreach (string str in alstr_local)
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
		/// извлечь тег для связи с ним
		/// </summary>
		/// <param name="str"></param>
		protected override ITag GetLink2CRZAVariable(string str)
		{
            ITag tag = null;
            uint tagguid = 0;
            uint unidev = 0;

            try
			{
                string[] strEl = str.Split(new char[] { '.' });

                // ищем тег в текущей конфигурации
                string[] arrstr = str.Split(new char[] { '.' });

                if (arrstr.Length == 3)
                {
                    tagguid = uint.Parse(arrstr[2]);// HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetTagGUID(0, "MOA_ECU", string.Format("{0}.{1}", arrstr[3], arrstr[4]));

                    unidev = uint.Parse(arrstr[1]);

                    tag = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Tag(0, unidev, (uint)tagguid);

                    if ((tag == null) || (unidev == null))
                        throw new Exception(string.Format(@"(107) : X:\Projects\01_HMIWinFormsClient\AdapterLib\FormulaEvalEv4Adapter.cs : GetLink2CRZAVariable() : Некорректный идентификатор тега = {0}", str));
                }
                else if (arrstr.Length ==5)
                {
                    tagguid = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetTagGUID(0, "MOA_ECU", string.Format("{0}.{1}", arrstr[3], arrstr[4]));

                    unidev = uint.Parse(arrstr[0]) * 256 + uint.Parse(arrstr[1]);

                    if ((tagguid == null) || (unidev == null))
                        throw new Exception(string.Format(@"(111) : X:\Projects\01_HMIWinFormsClient\AdapterLib\FormulaEvalEv4Adapter.cs : GetLink2CRZAVariable() : Некорректный идентификатор тега = {0}", str));

                    tag = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Tag(0, unidev, (uint)tagguid);
                }
                else
                    throw new Exception(string.Format(@"(108) : X:\Projects\01_HMIWinFormsClient\AdapterLib\FormulaEvalEv4Adapter.cs : GetLink2CRZAVariable() : Некорректный идентификатор тега = {0}", str));

            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

			return tag;
		}

		public void FirstEval()
		{
			EvalFormula();
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AdapterLib
{
	public class BitField2IntAdapter : AdapterBase, Adapter
	{		
		#region конструкторы
		public BitField2IntAdapter() 
		{
			/*  
				<formula Typeadapter = "BitField2IntAdapter" Name = "GPSb08" Caption ="GPSb08" Dim = "" Commentary = "рег бит флагов ФК - состояние GPS - биты 8,9,10" Express = "0 |+ 1 |+ 2">
							<value id = "0" tag = "0.0.0.0.0100"/>
							<value id = "1" tag = "0.0.0.0.0200"/>
							<value id = "2" tag = "0.0.0.0.0400"/>
				</formula>
			 */
		}
		#endregion	
		public void Init(XElement xeinit, ArrayList kb)
		{
			Typeadapter = xeinit.Attribute("typeadapter").Value;
			Name = xeinit.Attribute("name").Value;
			base.Caption = xeinit.Attribute("Caption").Value;
			Dim = xeinit.Attribute("Dim").Value;
			Commentary = xeinit.Attribute("commentary").Value;
			Express = xeinit.Attribute("express").Value;

			// создаем формулу для вычисления и привязываемся к обновлению значения формулы
			ArrayList alstr = new ArrayList();
			IEnumerable<XElement> xevalues = xeinit.Elements("value");
			foreach (XElement xevalue in xevalues)
				alstr.Add(xevalue.Attribute("tag").Value);

			FEvalEv = new FormulaEvalEv4Adapter(kb, alstr, Express);
			FEvalEv.OnChangeValForm += new ChangeValForm(FEvalEv_OnChangeValForm);
		}

		/// <summary>
		/// реакция на событие вычисление нового значения 
		/// привязанной к тегу формулы
		/// </summary>
		/// <param Name="valForm"></param>
		void FEvalEv_OnChangeValForm(object valForm)
		{
			//try
			//{
			//    value = Convert.ToBoolean(valForm);
			//    PlaceVarTo(null, VarQuality.vqGood);
			//}
			//catch (Exception ex)
			//{
			//    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			//}
		}
	}
}

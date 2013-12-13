/*#############################################################################*
 *    Copyright (C) 2006 Mehanotronika Corporation.                            *
 *    All rights reserved.                                                     *
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 *                                                                             *
 *	��������: �������� ������ �������������� ����������� ��������. ���������    *
 *					��� ������ �� �������� ������� ��� ����������� ��������������   *
 *					�������� ��������� ��� ����������������� ����� ����� ��������   *
 *					��������� ��� ������� ������ ����������								 *
 *                                                                             *
 *	����                     : Calculator.cs                                    *
 *	��� ��������� �����      : Calculator.dll                                   *
 *	������ �� ��� ���������� : �#, Framework 2.0                                *
 *	�����������              : ���� �.�.                                        *
 *	���� ������ ����������   : 08.03.2007                                       *
 *	���� (v1.0)              :                                                  *
 *******************************************************************************
 * ���������:
 * 1. ����(�����): ...c���������...
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
	//public delegate void ChangeTag(object valTag, CRZADevices.VarQuality vq); // ��� ������� ��������� �� ��������� ����. ����
	//public delegate void ChangeTagNewDS(object valTag, InterfaceLibrary.VarQualityNewDS vq); // ��� ������� ��������� �� ��������� ����. ����

	//// ��������� ������� � ������� �� ���, ���� �.�. ������� ������� ����������
	//public enum TypeOfPanel
	//{
	//    no,                     // ������ ������� - �� ������������� � �������
	//    CurrentStatusReg,       // �������, ������� �������
	//    Current_Analog,         // �������, ���������� �������
	//    Current_DiscretIn,      // �������, ������� ���������� �������
	//    Current_DiscretOut,     // �������, �������� ���������� �������
	//    CurrentControlProgUst,  // �������, ���������� ���������� �������
	//    CurrentDirection_P,     // �������, ����������� ��������
	//    CurrentCounters,        // �������, ��������
	//    Avar_AS_PSZ,            // ���������, ���������� �������, �������� ������������ ������
	//    Avar_AS_PPZ,            // ���������, ���������� �������, �������� ����� ������
	//    Avar_AS_AsPZ,           // ���������, ���������� �������, ���������� ������� ����� ������
	//    Avar_AS_AsSZ,           // ���������, ���������� �������, ���������� ������� ������������ ������
	//    Avar_DS_In,             // ���������, ���������� �������, �����
	//    Avar_DS_InChange,       // ���������, ���������� �������, �����-���������
	//    Avar_DS_Out,            // ���������, ���������� �������, ������
	//    Avar_DS_OutChange,      // ���������, ���������� �������, ������-���������
	//    System_Vizov,           // �������, ������� ������� �����
	//    System_Vizov_vkl,       // �������, ������� ������� ����� �� �������
	//    System_Test,            // �������, ���������� ������������ ����
	//    System_StatusDevice,    // �������, ��������� ����������
	//    System_StatusCommand,   // �������, ��������� ���������� ������
	//    Store_I_IntegralOtkl,
	//    Store_I_lastOtkl,
	//    Store_CountEvent,
	//    Store_Maxmeter,
	//    Config_Ustavki,           //!!!!! ������ !!!!!!
	//    Config_Configuration,     //!!!!! ������ !!!!!!
	//    Config_Ustavki_0,         // ������������ � ������� - 4 �������
	//    Config_Ustavki_1,
	//    Config_Ustavki_2,
	//    Config_Ustavki_3,
	//    Config_Ustavki_4,
	//    Config_Ustavki_5,
	//    Status_Dev,            // ��������� ���������� � ������
	//    Status_Com,
	//    StatusFC,				//��������� ����
	//    StatusCmdFC,				// ��������� ������ ����
	//    System_BottomPanel,	// ������� ������� - ������ ������
	//    Avar_BottomPanel,	// ������� ������ - ������ ������
	//    Store_BottomPanel,	// ������� ������������� - ������ ������
	//     Config_BottomPanel,	// ������� ������ � ��� - ������ ������
	//     // ������ ��� ����� ��
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
	//     NormModePanel_Analog,	// ������ ���� ��� - ������ �������
	//     NormModePanel_Discret,	// ������ ���� ��� - �������� �������
	//};

    // ��������� ���� - ����������, ���������� � �.�.
    public enum TypeOfTag
    {
        no,             // ������ ������� - ��� ���������
        Analog,         // ����������
        Discret,        // ����������
        Combo           //  ComboBox
    };

	//------------------------------------------------------------------------
   // /// <summary>
   // /// ��������� ��� �������� ���������� ���������� �������
   // /// </summary>
   //public struct RezFormulaEval
   //{
   //   public string IdTagIE;			// ������, ���������������� ������������ ������� � ������� "fc.dev.group.var.bitmask"
   //   public string CaptionIE;          // ������-�������� ������� ������������� ��������
   //   public string DimIE;              // ������-�������� ����������� ������������� ��������
   //   public object bitMaskValue;       // ��� ������� �������� - ��������� ������� � ���� ������� �����
   //   public object Value;              // ��� ������� �������� - ��������� ������� � ���� bool-��������
   //   public string TypeTag;
   //   public bool IsValOutOfBounds;    // ���� ������ �� ������� ��������� ���������� ��������
   //   public string strKTR;            // ����������� ������������� � ����� ������ (��� ������� �������)
   //}

   #region public struct TagVal - ��������� ��� �������� �������� ����
   //------------------------------------------------------------------------
   // ��������� ��� �������� �������� ����
   //public struct TagVal
   //{
   //   public int indTag;	// ����� ���� � �������� ������������������ arrTag
   //   public int typeDev;	// ��� - �� ���� ����� �������� ������� ��������� ����������� ����� ���������:
   //   // 0 - ��� ����
   //   public string strTagIdent;	// ��������� �� ��������� arrTag, ��� ������������� ����

   //   public string CaptionTag;  // ������-�������� ������� ���� (�� CRZADevice)
   //   public string DimTag;      // ������-�������� ����������� ���� (�� CRZADevice)
   //   public string NameTag;      // ������-�������� ����� ���� (�� CRZADevice)

   //   // ���������� �� arrTag ��������, ���������������� ���
   //   public int FC;					// ����� ��
   //   public int FD;					// ����� ����������
   //   public int FG;					// ����� ������
   //   public int FV;					// ����� ����������
   //   public string FB;				// ������� ����
   //   public TagEval linkTagEval;	// ������ �� TagEval ������� ���� (��������)
   //   public object value;			// ����������� �������� ����
   //   public string notice;			// ������ ��� �������� �������������

   //   // �����, ��������
   //   public bool IsTagValValueIncorrect;
   //   /// <summary>
   //   /// ������� ��� ���� ��� - ���������
   //   /// </summary>
   //   public bool IsThisTagConst;
   //   /// <summary>
   //   /// �������� ���� - ���������
   //   /// </summary>
   //   public string ValueConstAsString; 
   //}   
   #endregion

	/// <summary>
	/// ��������� ��� �������� ���������� ���������� �������
	/// </summary>
	public struct RezFormulaEval
	{
		/// <summary>
		/// ������, ���������������� ������������ ������� 
		/// � ������� "fc.dev.group.var.bitmask"
		/// </summary>
		public string IdTagIE;
		/// <summary>
		/// ������-�������� ������� ������������� ��������
		/// </summary>
		public string CaptionIE;
		/// <summary>
		/// ������-�������� ����������� ������������� ��������
		/// </summary>
		public string DimIE;
		/// <summary>
		/// ��� ������� �������� - 
		/// ��������� ������� � ���� ������� �����
		/// </summary>
		public object bitMaskValue;
		/// <summary>
		/// ��� ������� �������� - 
		/// ��������� ������� � ���� bool-��������
		/// </summary>
		public object Value;
		/// <summary>
		/// ��� ���� � ������� �������� ������, 
		/// �������� TIntVariable
		/// </summary>
		public string TypeTag;
		/// <summary>
		/// ���� ������ �� ������� ��������� ���������� ��������
		/// </summary>
		public bool IsValOutOfBounds;
		/// <summary>
		/// ����������� ������������� � ����� ������ (��� ������� �������)
		/// </summary>
		public string strKTR;
	}

    /// <summary>
    /// class FormulaEval
    /// �����, �������������� ����������� �������� ���������� ���� ��� 
    /// ����������������� �� �������� �������
    /// </summary>	
   public class FormulaEvalNDS
	{
		/// <summary>
		/// ��� ������ ���� ���������� �������
		/// </summary>
		public string ToP;
		/// <summary>
		/// ��������� ����
		/// </summary>
		public TypeOfTag ToT;
		public string SourceFormula;
		IConfiguration configuration;	//���� ��� ����� � ������������� ������ DS
		public ITag LinkVariableNewDS;   // �������� � ���������� � ����� DS

		int flFC = 0;
		int flDevice = 0;
		int flGroup = 0;
		int flVariable = 0;
		string flBitMsk = string.Empty;

		public object Value; // ������� �������� ����
		public string NameFE;		// ��� ���������� (�� ������������)
		public string CaptionFE;	// ������� ����������
		public string Dim;			// ����������� ����������
		public ArrayList arrStrCB;  // ������ ����� ��� ComboBox
		public string TypeVar = String.Empty;   // ��� ����������, ����������� � �������� ��� ����-100
		public RezFormulaEval tRezFormulaEval;

		#region ������ ���
		/// <summary>
		/// ������ � ������, ������������ � �������
		/// </summary>
		//private string arrTag;
		/// <summary>
		/// ������� � �����
		/// </summary>
		//private string formula;

		/// <summary>
		/// ������ ����� (�������� TagVal)
		/// </summary>
		//public ArrayList arrTagVal;
		//public uint addrVar;
		//public string addrVarBitMask = null;
		/// <summary>
		/// ������� �����, ���� ����� �������� ����� �������� ��� � ���������
		/// </summary>
		//public string bitmask;
		/// <summary>
		/// ������ �� ������/������
		/// </summary>
		//public string ReadWrite;
		/// <summary>
		/// ������ �������, 
		/// ����� �������� �������� ����� � ��
		/// </summary>
		//private string strFormat = String.Empty;
		//public string StrFormat
		// {
		//     set
		//     {
		//         if( value == "" )
		//             value = "0";
		//     else if (value == "-1")   // �������� - �� ����� (����� ��� ������� ������ � ��.)
		//        value = "-1";
		//     else if (!Char.IsNumber((value.Trim()),0))
		//        throw new Exception(" (177) : FormulaEval: ������������ �������� �������� ��� ����� � ��������� ������ : StrFormat = " + value);

		//            strFormat = value;
		//     }
		//     get
		//     {
		//         return strFormat;
		//     }
		// }

		/// <summary>
		/// ������� ��� ������� ��������� �� ��������� ����. ����
		/// </summary>
		/// <param name="valForm"></param>
		/// <param name="format"></param>
		//public delegate void ChangeValForm(object valForm, string format);
		/// <summary>
		/// ������� �� ��������� ���������� ����������� �������
		/// </summary>
		//public event ChangeValForm OnChangeValForm;

		/// <summary>
		/// ������ ������� �������� 
		/// ��� ������� ��������� �� ��������� ����. ����
		/// </summary>
		/// <param name="strtagident"></param>
		/// <param name="valTag"></param>
		/// <returns></returns>
		public delegate bool ChangeValFormTI(string strtagident, object valTag);
		/// <summary>
		/// ������ ������� ������� 
		/// �� ��������� ���������� ����������� �������
		/// </summary>
		public event ChangeValFormTI OnChangeValFormTI;		
		#endregion

		/// <summary>
		/// �����������
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
			 *	������ ������ ������� �����:
			 * 0(FC.FD.FG.FV.FB)1(xxx)..n(xxx), ���
			 * ����� 0..n - ������ �����, ���. ����� ��������� � ������� �� ���������� ���� �����
			 * FC - ����� ��
			 * FD	- ����� ����������
			 * FG - ����� ������
			 * FV - ����� ����������
			 * FB - ������� �����
			 *--------------------------------------------------------------------*/
			/*
			 * ���������� ���������� ��������� ��� ��������� 
			 * �������� ���� �����.�����.�����
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
					throw new Exception(string.Format("(377) : FormulaEval_NDS.cs : FormulaEvalNDS() : �������������� ��� : aTag = {0};\n" + "f = {1};\n" + "aCaptionIE = {2}.\n", aTag, f, aCaptionIE));
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

        /// ����������� 2
        /// </summary>
        /// <param Name="aFB">- ������������</param>
        /// <param Name="aTag">- ������ ������������� �������, �������� ������ �����</param>
        /// <param Name="f">- ������, �������� ������� ���������� ������������� ��������</param>
        /// <param Name="aCaptionIE">- ������ �������� ������� ������������� ��������</param>
        /// <param Name="aDimIE">- ������ �������� ����������� ������������� ��������</param>
        /// <param Name="tot">- ��� ���� (����������, ����������, combobox)</param>
        /// <param Name="toP">- ��� ������</param>
		  /// /// <param Name="advOptions">- ���. �����</param>		  
		 public FormulaEvalNDS( IConfiguration configuration, string aTag, string f, string aCaptionIE, string aDimIE, TypeOfTag tot, string toP, object advOptions )
			: this(configuration, aTag, f, aCaptionIE, aDimIE, tot, toP)
		 {
			 if( advOptions is string )
				 flBitMsk = (string)advOptions;
		 }

         /// <summary>
         /// ����������� ��� ������ DataServer
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
              *	������ ������ ������� �����:
              * 0(FC.FD.FG.FV.FB)1(xxx)..n(xxx), ���
              * ����� 0..n - ������ �����, ���. ����� ��������� � ������� �� ���������� ���� �����
              * FC - ����� ��
              * FD	- ����� ����������
              * FG - ����� ������
              * FV - ����� ����������
              * FB - ������� �����
              *--------------------------------------------------------------------*/
             /*
              * ���������� ���������� ��������� ��� ��������� 
              * �������� ���� �����.�����.�����
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
                     throw new Exception(string.Format("(377) : FormulaEval_NDS.cs : FormulaEvalNDS() : �������������� ��� : aTag = {0};\n" + "f = {1};\n" + "aCaptionIE = {2}.\n", aTag, aCaptionIE));
             }
             catch (Exception ex)
             {
                 TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
             }
         }
         /// <summary>
         /// ������ � ����� 1
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
                    return false;	// ��� �� ������

                LinkVariableNewDS = tag;
                // ��� ���� ���������� ����������� �����
                tRezFormulaEval.CaptionIE = NameFE = aCaptionIE;// LinkVariableNewDS.TagName;     // aVariable.Name;			// ��� ���������� (�� ������������)
                //CaptionFE = aCaptionIE;//  LinkVariableNewDS.TagName;                              ///aVariable.Caption;	    // ������� ����������
                tRezFormulaEval.DimIE = CaptionFE = aDimIE;// Dim = LinkVariableNewDS.Unit;               // aVariable.Dim;			// ����������� ����������

                // ��������� ������ ���������������� ���
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
        /// ������ � ����� 2
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
                return false;	// ��� �� ������

            LinkVariableNewDS = tag;
            // ��� ���� ���������� ����������� �����
            tRezFormulaEval.CaptionIE = NameFE = aCaptionIE;// LinkVariableNewDS.TagName;     // aVariable.Name;			// ��� ���������� (�� ������������)
            CaptionFE = LinkVariableNewDS.TagName;                              ///aVariable.Caption;	    // ������� ����������
            tRezFormulaEval.DimIE = Dim = aDimIE;// LinkVariableNewDS.Unit;               // aVariable.Dim;			// ����������� ����������

            // ��������� ������ ���������������� ���
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
			 * OnChangeValFormTI - ������� ��� ����� ����������
			 * � �� �������������� �������� ��� int32 - ����������
			 * � ���������� (��������� ��������� �� ���. 60013)
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

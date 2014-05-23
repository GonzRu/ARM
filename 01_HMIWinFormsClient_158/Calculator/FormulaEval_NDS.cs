using System;
using System.Globalization;
using InterfaceLibrary;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Calculator
{
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
        /// результат формулы в виде bool-значения
        /// </summary>
        public object Value;
    }

    /// <summary>
    /// class FormulaEval
    /// класс, представляющий вычисляемое значение отдельного тега или 
    /// комплексированное по заданной формуле
    /// </summary>	
    public class FormulaEvalNds
    {
        private readonly string sourceFormula;
        public ITag LinkVariableNewDs; // привязка к переменной в новом DS
        private int flFc;
        private int flDevice;
        private int flGroup;
        private int flVariable;
        private string flBitMsk = string.Empty;

        public RezFormulaEval RezFormulaEval;

        #region старый код
        /// <summary>
        /// другой вариант делегата 
        /// для события извещения об изменении знач. тега
        /// </summary>
        /// <param name="strtagident"></param>
        /// <param name="valTag"></param>
        /// <returns></returns>
        public delegate bool ChangeValFormTI( string strtagident, object valTag, TypeOfTag type );

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
        public FormulaEvalNds( IConfiguration configuration, string aTag, string f, string aCaptionIE, string aDimIE )
        {
            this.sourceFormula = aTag;

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
                //Regex re = new Regex(@"[\d]+\.[\d]+\.[\d]+.[\d]+.[\d]+");

                //Match m = re.Match(SourceFormula);

                //string[] stidt = m.Value.Split(new char[]{'.'});
                string[] res1 = this.sourceFormula.Split( new[] { '(', ')' } );
                string[] stidt = res1[1].Split( new[] { '.' } );

                this.flFc = int.Parse( stidt[0] );
                flDevice = int.Parse( stidt[1] );
                flGroup = int.Parse( stidt[2] );
                flVariable = int.Parse( stidt[3] );
                flBitMsk = stidt[4];

                if ( !TagControlSet( configuration, this.flFc, flDevice, flGroup, flVariable, flBitMsk, aCaptionIE, aDimIE ) )
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Error,
                                                                          361,
                                                                          string.Format( "{0} : {1} : {2} : Запрос несуществующего тега :\n aTag = {3};\n f = {4};\n aCaptionIE = {5}.\n", DateTime.Now.ToString( ), "FormulaEval_NDS.cs", "FormulaEvalNDS()", aTag, f, aCaptionIE ) );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
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
        public FormulaEvalNds( IConfiguration configuration, string aTag, string aCaptionIE, string aDimIE )
        {
            this.sourceFormula = aTag;

            /*--------------------------------------------------------------------
              *	формат строки массива тэгов:
              * 0(DS.DEVGUID.TAGGUID), где
              * 
              * DS - номер DataServer
              * DEVGUID	- уник номер устройства
              * TAGGUID - уник номер тега
              *--------------------------------------------------------------------*/
            /*
              * используем регулярное выражение для выделения 
              * подстрок вида цифры.цифры.цифры
              */
            try
            {
                Regex re = new Regex( @"[\d]+\.[\d]+\.[\d]+" ); //.[\d]+.[\d]+;

                Match m = re.Match( this.sourceFormula );

                string[] stidt = m.Value.Split( new char[] { '.' } );
                uint flDS = uint.Parse( stidt[0] );
                uint flDevice = uint.Parse( stidt[1] );
                uint flTagGuid = uint.Parse( stidt[2] );

                if (flDevice == 0 && flTagGuid == 0)
                    return;

                if ( !TagControlSet( configuration, flDS, flDevice, flTagGuid, aCaptionIE, aDimIE ) ) 
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Error, 427, string.Format( "{0} : {1} : {2} : Запрос несуществующего тега :\n aTag = {3};\n aCaptionIE = {4}.\n", DateTime.Now.ToString( ), "FormulaEval_NDS.cs", "FormulaEvalNDS()", aTag, aCaptionIE ) );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
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
        private bool TagControlSet( IConfiguration configuration, int lFC, int lDevice, int lGroup, int lVariable, string lBitMsk, string aCaptionIE, string aDimIE )
        {
            try
            {
                this.flFc = lFC;
                flDevice = lDevice;
                flGroup = lGroup;
                flVariable = lVariable;
                flBitMsk = lBitMsk;

                var tagguid = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetTagGUID( 0, "MOA_ECU", string.Format( "{0}.{1}", lVariable.ToString( CultureInfo.InvariantCulture ), lBitMsk ) );

                var guidevice = (uint)( lFC * 256 + lDevice );

                var tag = configuration.GetLink2Tag( 0, guidevice, tagguid ); //(uint)lDevice

                if ( tag == null ) // тег не найден
                    return false;

                this.LinkVariableNewDs = tag;

                // для всех переменных присваиваем имена
                this.RezFormulaEval.CaptionIE = aCaptionIE;
                this.RezFormulaEval.DimIE = aDimIE; // размерность переменной

                // формируем строку идентифицирующую тег
                this.RezFormulaEval.IdTagIE = string.Format( "{0}.{1}.{2}.{3}.{4}", lFC, lDevice, lGroup, lVariable, lBitMsk );
                this.LinkVariableNewDs.OnChangeValue += LinkVariableNewDsOnOnChangeValue;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                return false;
            }

            return true;
        }
        /// <summary>
        /// связка с тегом - вариант для нового описания (БМРЗ-100)
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="lFC"></param>
        /// <param name="lDevice"></param>
        /// <param name="lGroup"></param>
        /// <param name="lVariable"></param>
        /// <param name="lBitMsk"></param>
        /// <returns></returns>
        private bool TagControlSet( IConfiguration configuration, uint lds, uint lDevice, uint tagGuid, string aCaptionIE, string aDimIE )
        {
            try
            {
                var tag = configuration.GetLink2Tag( 0, lDevice, tagGuid );

                if ( tag == null ) // тег не найден
                    return false;

                this.LinkVariableNewDs = tag;

                // для всех переменных присваиваем имена
                this.RezFormulaEval.CaptionIE = aCaptionIE;
                this.RezFormulaEval.DimIE = aDimIE;

                // формируем строку идентифицирующую тег(под старый формат)
                //tRezFormulaEval.IdTagIE = string.Format("0.{0}.0.0.{1}", tag.Device.UniObjectGUID, tag.TagGUID.ToString());

                // формируем строку идентифицирующую тег(под новый формат)
                this.RezFormulaEval.IdTagIE = string.Format( "{0}.{1}.{2}", tag.Device.UniDS_GUID, tag.Device.UniObjectGUID, tag.TagGUID.ToString( CultureInfo.InvariantCulture ) );
                this.LinkVariableNewDs.OnChangeValue += LinkVariableNewDsOnOnChangeValue;

                return true;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                return false;
            }

        }
        /// <summary>
        /// Событие изменения значения сигнала/тэга
        /// </summary>
        /// <param name="var">Кортеж с данными сигнала</param>
        /// <param name="type">Тип сигнала</param>
        private void LinkVariableNewDsOnOnChangeValue( Tuple<string, byte[], DateTime, VarQualityNewDs> @var, TypeOfTag type )
        {
            if ( OnChangeValFormTI == null ) return;

            try
            {
                switch ( type )
                {
                        //case TypeOfTag.Combo: // выводиться как стринг
                    case TypeOfTag.Discret: // преобразовывается из true/false
                        OnChangeValFormTI( this.RezFormulaEval.IdTagIE, Convert.ToBoolean( @var.Item1 ) ? 1 : 0, type );
                        break;
                        //case TypeOfTag.Analog: // выводиться как стринг
                        //    OnChangeValFormTI( this.tRezFormulaEval.IdTagIE, Convert.ToSingle( @var.Item1 ) );
                        //    break;
                        //case TypeOfTag.DateTime: // выводиться как стринг
                        //    OnChangeValFormTI( this.tRezFormulaEval.IdTagIE, DateTime.Parse( @var.Item1 ) );
                        //    break;
                    case TypeOfTag.Combo:
                        // Раньше приходил текст перечисления. Это не подходит. Сейчас конвертирую в Single (формат по-умолчанию для TagEnum), а затем в Int
                        // для мнемосхемы (по анологии с дискретным тегом).
                        Single singleValue = BitConverter.ToSingle(@var.Item2, 0);

                        OnChangeValFormTI(this.RezFormulaEval.IdTagIE, (int)singleValue, type);
                        break;
                    default: // выводиться как стринг
                        OnChangeValFormTI( this.RezFormulaEval.IdTagIE, @var.Item1, type );
                        break;
                }
            }
            catch ( Exception exception )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( exception );
            }
        }
    }
}
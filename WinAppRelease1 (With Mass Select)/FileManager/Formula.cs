using System;
using System.Diagnostics;
using System.Xml.Linq;

using LibraryElements.CalculationBlocks;

namespace FileManager
{
    public class BuildFormula : LinqStream
    {
        private uint _stateDSGuid = 0;
        private uint _stateDeviceGuid = 0;
        private bool _isDeviceFromDeviceBinding = false;

        public const string FormulaBlock = "Behaviour of the block.xml";
        public const string BlockImage = "Device.bmp";

        ElementCalculation calculation;

        /// <summary>
        /// Разбор данных из файла
        /// </summary>
        public void ParceDataFromFile()
        {
            if ( error_flag ) return;

            xroot = xdoc.Element( "namespace" );
            if (xroot != null)
                ParceDataFromNode( xroot.Element( "formulas" ) );
        }
        /// <summary>
        /// Разбор данных из ветки
        /// </summary>
        /// <param name="data">Ветвь с данными</param>
        public void ParceDataFromNode( XElement data )
        {
            if ( error_flag || data == null )
                return;

            var xAttr = data.Attribute( "calculation" );
            var calcName = ( xAttr == null ) ? "NaN" : xAttr.Value;

            calculation = ElementCalculation.DefineElement( calcName );

            if ( this.calculation == null )
                return;

            #region Parse IsDeviceFromDeviceBinding, StateDSGuid and StateDeviceGuid Attributes

            #region Parse attribute IsDeviceFromDeviceBinding
            bool isDeviceFromDeviceBindingTmp;
            if (data.Attribute("IsDevicefromDeviceBinding") == null)
                isDeviceFromDeviceBindingTmp = true;
            else
            {
                if (!bool.TryParse(data.Attribute("IsDevicefromDeviceBinding").Value, out isDeviceFromDeviceBindingTmp))
                {
                    isDeviceFromDeviceBindingTmp = true;
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, "Ошибка при разборе атрибута IsDevicefromDeviceBinding - неверное значение:" + data.Attribute("IsDevicefromDeviceBinding").Value);
                }
            }
            _isDeviceFromDeviceBinding = isDeviceFromDeviceBindingTmp;
            #endregion

            #region Parse StateDSGuid and StateDeviceGuid Attributes
            _stateDSGuid = 0;
            _stateDeviceGuid = 0;
            if (!_isDeviceFromDeviceBinding)
            {
                var dsAttribute = data.Attribute("StateDSGuid");
                if (dsAttribute != null)
                    if (!UInt32.TryParse(dsAttribute.Value, out _stateDSGuid))
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, "Ошибка при разборе атрибута StateDSGuid - неверное значение:" + dsAttribute.Value);

                var deviceAttribute = data.Attribute("StateDeviceGuid");
                if (deviceAttribute != null)
                    if (!UInt32.TryParse(deviceAttribute.Value, out _stateDeviceGuid))
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, "Ошибка при разборе атрибута StateDeviceGuid - неверное значение:" + deviceAttribute.Value);
            }
            #endregion
            #endregion

            this.calculation.ReadXRecords(data);
        }
        public CalculationContext GetData()
        {
            return (calculation == null)
                       ? null
                       : new CalculationContext(calculation)
                             {
                                 StateDSGuid = _stateDSGuid,
                                 StateDeviecGuid = _stateDeviceGuid,
                                 IsDeviceFromDeviceBinding = _isDeviceFromDeviceBinding
                             };
        }
    }
    public class CreateFormula : LinqStream
    {
        public void CreateCompleateTree( CalculationContext calculation )
        {
            var ns = CreateDeclaration();
            ns.Add( GetCreateNode( calculation ) );
        }
        public XElement GetCreateNode( CalculationContext calculation )
        {
            var root = new XElement( "formulas" );
            root.Add( new XAttribute( "calculation", ( calculation == null ) ? "NaN" : calculation.Context.ToString() ) );

            if (calculation != null)
            {
                root.Add(new XAttribute("IsDeviceFromDeviceBinding", calculation.IsDeviceFromDeviceBinding));
                root.Add(new XAttribute("StateDSGuid", calculation.StateDSGuid));
                root.Add(new XAttribute("StateDeviceGuid", calculation.StateDeviecGuid));

                root.Add(calculation.Context.CreateXRecordsNode());
            }

            return root;
        }
        public static XElement CreateEmptyNode( )
        {
            var root = new XElement( "formulas" );
            root.Add( new XAttribute( "calculation", "NaN" ) );
            return root;
        }
    }
}

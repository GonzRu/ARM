using System;
using System.Xml.Linq;

using LibraryElements.CalculationBlocks;

namespace FileManager
{
    public class BuildFormula : LinqStream
    {
        private uint _stateDSGuid = 0;
        private uint _stateDeviceGuid = 0;

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

            #region Parse StateDSGuid and StateDeviceGuid Attributes
            _stateDSGuid = 0;
            _stateDeviceGuid = 0;
            try
            {
                var dsAttribute = data.Attribute("StateDSGuid");
                if (dsAttribute != null)
                    _stateDSGuid = UInt32.Parse(dsAttribute.Value);

                var deviceAttribute = data.Attribute("StateDeviceGuid");
                if (deviceAttribute != null)
                    _stateDeviceGuid = UInt32.Parse(deviceAttribute.Value);
            }
            catch
            {
                Console.WriteLine(
                    "Formula.cs::ParceDataFromNode: ошибка при разборе атрибутов StateDSGuid={0} и StateDeviceGuid={1}",
                    data.Attribute("StateDSGuid").Value,
                    data.Attribute("StateDeviceGuid").Value);
            }
            #endregion


            this.calculation.ReadXRecords( data );
        }
        public CalculationContext GetData() 
        {
            return (calculation == null) ? null : new CalculationContext(calculation) { StateDSGuid = _stateDSGuid, StateDeviecGuid = _stateDeviceGuid };
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

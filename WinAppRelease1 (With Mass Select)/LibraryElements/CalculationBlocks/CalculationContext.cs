using System.Drawing;

using LibraryElements.Sources;

namespace LibraryElements.CalculationBlocks
{
    public class CalculationContext
    {
        public uint StateDSGuid { get; set; }
        public uint StateDeviecGuid { get; set; }
        public bool IsDeviceFromDeviceBinding { get; set; }

        public CalculationContext( ElementCalculation calculation )
        {
            this.Context = calculation;

            var calc = calculation as ICalculationContext;
            if (calc != null)
            {
                this.StateDSGuid = calc.CalculationContext.StateDSGuid;
                this.StateDeviecGuid = calc.CalculationContext.StateDeviecGuid;
                this.IsDeviceFromDeviceBinding = calc.CalculationContext.IsDeviceFromDeviceBinding;
            }
        }
        private CalculationContext( CalculationContext originalContext ) 
        {
            this.Context = ElementCalculation.GetCopy(originalContext.Context);
            this.StateDSGuid = originalContext.StateDSGuid;
            this.StateDeviecGuid = originalContext.StateDeviecGuid;
            this.IsDeviceFromDeviceBinding = originalContext.IsDeviceFromDeviceBinding;
        }
        public bool Execute( Element element, Graphics graphics )
        {
            var trunkCalculation = this.Context as IColorResult;
            if ( trunkCalculation != null )
            {
                element.ElementColor = trunkCalculation.GetResult( );
                return true;
            }

            var calcFigure = element as CalculationFigure;
            var calculation = this.Context as IFigureResult;
            if ( calcFigure != null && calculation != null )
            {
                calculation.DrawElement( graphics, calcFigure.GetPosition( ) );
                return true;
            }

            return false;
        }

        public static CalculationContext GetCopy( CalculationContext originalContext ) { return originalContext == null ? null : new CalculationContext( originalContext ); }
        /// <summary>
        /// Корректировака данных тэгов для конкретного элемента
        /// </summary>
        /// <param name="calculation">Элемент расчетных данных</param>
        /// <param name="dsGuid">Номер дата сервера</param>
        /// <param name="devGuid">Номер устройства</param>
        public static void AdjustmentTags( ElementCalculation calculation, uint dsGuid, uint devGuid )
        {
            var adg = calculation as IAdjustment;
            if ( adg == null || !adg.IsAdjustment ) return;

            foreach ( SignalMatchRecord tag in calculation.GetTags( ) )
            {
                if (tag.DsGuid == 0 && tag.DevGuid == 0)
                {
                    tag.DsGuid = dsGuid;
                    tag.DevGuid = devGuid;
                }
            }
        }

        public ElementCalculation Context { get; private set; }
    }
}
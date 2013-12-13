using System.Drawing;

using LibraryElements.Sources;

namespace LibraryElements.CalculationBlocks
{
    public class CalculationContext
    {
        public CalculationContext( ElementCalculation calculation ) { this.Context = calculation; }
        private CalculationContext( CalculationContext originalContext ) { this.Context = ElementCalculation.GetCopy( originalContext.Context ); }
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
                tag.DsGuid = dsGuid;
                tag.DevGuid = devGuid;
            }
        }

        public ElementCalculation Context { get; private set; }
    }
}
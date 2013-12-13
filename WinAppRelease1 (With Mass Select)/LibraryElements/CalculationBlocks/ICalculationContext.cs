namespace LibraryElements.CalculationBlocks
{
    public interface ICalculationContext
    {
        /// <summary>
        /// Расчетные данные 
        /// </summary>
        CalculationContext CalculationContext { get; set; }
    }
}
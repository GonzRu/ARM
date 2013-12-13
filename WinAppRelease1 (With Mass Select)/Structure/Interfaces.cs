using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Structure
{
   #region Program Work Interfaces
   /// <summary>
   /// Интерфейс элементов редактора (храняшие цвет фигуры)
   /// </summary>
   public interface IEditorColor
   {
      Color GetUpColor();
      Color GetDownColor();

      void SetColor(Color up, Color down);
   }
   /// <summary>
   /// Интерфейс общих методов
   /// </summary>
   public interface IElementLine
   {
      Color ElementColor { get; set; }
      DashStyle LineStyle { get; set; }
      int Thickness { get; set; }
      String GetPoints();
      void SetReadPoints(string _parse_str);
   }
   /// <summary>
   /// Базовый интерфейс для работы с текстом
   /// </summary>
   public interface IBaseFormText
   {
       String Text { get; set; }
   }
   /// <summary>
   /// Интерфейс для работы с текстом
   /// </summary>
   public interface IFormText : IBaseFormText
   {
      Font TextFont { get; set; }
      Color ElementColor { get; set; }
      bool VerticalView { get; set; }
   }
   /// <summary>
   /// Интерфейс формы загрузки или сохранения данных
   /// </summary>
   public interface IProcess
   {
      void SetMaxValue(int _quant);
      void SetPerformStep();
      void SetNewError();
   }
   /// <summary>
   /// Интерфейс формы отчета ошибок
   /// </summary>
   public interface IErrorLog
   {
      void SetErrorRecord(TreeNode _error_node);
   }
   #endregion
}

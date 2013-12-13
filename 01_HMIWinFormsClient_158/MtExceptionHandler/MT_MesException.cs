// класс для вывода сообщений при генерации исключений приложения
using System;
using System.Diagnostics;

namespace MtExceptionHandler
{
   public class MT_MesException : ApplicationException
   {
      // конструктор по умолчанию
      public MT_MesException() : base("Ошибка приложения MT_???")
      { }

      // конструктор для составления сообщения об ошибке
      public MT_MesException(string message) : base(message)
      {
         Console.WriteLine(message);
      }

      // конструктор для составления сообщения об ошибке с указанием объекта
      public MT_MesException(string message, Exception inner)
         : base(message, inner)
      { }

      public override string ToString()
      {
         return base.ToString();
      }
   }
}

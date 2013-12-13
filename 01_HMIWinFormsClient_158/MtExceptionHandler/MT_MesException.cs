// ����� ��� ������ ��������� ��� ��������� ���������� ����������
using System;
using System.Diagnostics;

namespace MtExceptionHandler
{
   public class MT_MesException : ApplicationException
   {
      // ����������� �� ���������
      public MT_MesException() : base("������ ���������� MT_???")
      { }

      // ����������� ��� ����������� ��������� �� ������
      public MT_MesException(string message) : base(message)
      {
         Console.WriteLine(message);
      }

      // ����������� ��� ����������� ��������� �� ������ � ��������� �������
      public MT_MesException(string message, Exception inner)
         : base(message, inner)
      { }

      public override string ToString()
      {
         return base.ToString();
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LexemParsing
{
   /// <summary>
   /// Лексема
   /// </summary>
   internal class Lexem
   {
      #region Parameters
      /// <summary>
      /// Строка
      /// </summary>
      public string Str = "";
      #endregion
   }
   /// <summary>
   /// Операция
   /// </summary>
   internal class Op : Lexem
   {
      #region Class Methods
      /// <summary>
      /// Проверка операции
      /// </summary>
      /// <param name="op">Строка с операцией</param>
      /// <returns>Является ли строка операцией</returns>
      static public bool IsOp(string op)
      {
         return (op == "*") ||
                (op == "/") ||
                (op == "+") ||
                (op == "-") ||
                (op == "&") ||
                (op == "|") ||
                (op == "<") ||
                (op == ">") ||
                (op == "=");
      }
      /// <summary>
      /// Проверка бинарной операции
      /// </summary>
      /// <param name="op">Строка с операцией</param>
      /// <returns>Является ли строка операцией</returns>
      static public bool IsBinOp(string op)
      {
         return (op == "&&") ||
                (op == "||");
      }
      /// <summary>
      /// Получить результат операции
      /// </summary>
      /// <param name="Digit1">Значение 1</param>
      /// <param name="Digit2">Значение 2</param>
      /// <returns>Результат</returns>
      /// <exception cref="Exception - Неверная операция" />
      public int CalcOp(int Digit1, int Digit2)
      {
         bool bDig1 = false, bDig2 = false;

         switch (Str)
         {
            case "*":
               return Digit1 * Digit2;
            case "/":
               return Digit1 / Digit2;
            case "+":
               return Digit1 + Digit2;
            case "-":
               return Digit1 - Digit2;
            case "&":
               return Digit1 & Digit2;
            case "|":
               return Digit1 | Digit2;
            case "&&":
               {
                  bDig1 = Convert.ToBoolean(Digit1);
                  bDig2 = Convert.ToBoolean(Digit2);
                  return Convert.ToInt32(bDig1 && bDig2);
               }
            case "||":
               {
                  bDig1 = Convert.ToBoolean(Digit1);
                  bDig2 = Convert.ToBoolean(Digit2);
                  return Convert.ToInt32(bDig1 || bDig2);
               }
            case "<":
               return Convert.ToInt32(Digit1 < Digit2);
            case ">":
               return Convert.ToInt32(Digit1 > Digit2);
            case "=":
               return Convert.ToInt32(Digit1 == Digit2);
            default:
               throw new Exception("Неверная операция");
         }
      }
      #endregion
   }
   /// <summary>
   /// Значение
   /// </summary>
   internal class Digit : Lexem
   {
      #region Parameters
      /// <summary>
      /// Значение
      /// </summary>
      public int Value = 0;
      #endregion
   }

   /// <summary>
   /// Класс выражение
   /// </summary>
   public class Expresion
   {
      #region Parameters
      /// <summary>
      /// Лексемы
      /// </summary>
      private List<Lexem> Lexems = new List<Lexem>();
      #endregion

      #region Class Methods
      #region Public Methods
      /// <summary>
      /// Задание выражения
      /// </summary>
      /// <param name="Exp">Выражение</param>
      public void SetExp(String Exp)
      {
         Exp = DelBlanks(Exp);
         List<string> lexems = GetLexems(Exp);
         CheckBracked(lexems);
         //SetPriorBrackets(lexems);
         ParsExp(lexems);
      }
      /// <summary>
      /// Получение результата
      /// </summary>
      /// <returns>Результат</returns>
      /// <exception cref="Exception - Не удалось расчитать результат" />
      public int GetResult()
      {
         var _Lexems = (from n in Lexems select n).ToList();

         for (int i = 0; i < _Lexems.Count; ++i)
         {
            Lexem lexem = _Lexems[i];

            if (lexem is Op)
            {
               Op op = (Op)lexem;

               Digit LexemDigit1 = (Digit)_Lexems[i - 2];
               int Digit1 = LexemDigit1.Value;

               Digit LexemDigit2 = (Digit)_Lexems[i - 1];
               int Digit2 = LexemDigit2.Value;

               _Lexems.RemoveAt(i);
               _Lexems.RemoveAt(i - 1);
               _Lexems.RemoveAt(i - 2);

               int Result = op.CalcOp(Digit1, Digit2);
               Digit ResultLexem = new Digit();
               ResultLexem.Value = Result;
               _Lexems.Insert(i - 2, ResultLexem);

               i -= 2;
            }//if
         }//for

         if (_Lexems.Count != 1)
            throw new Exception("Не удалось расчитать результат");
         if (!(_Lexems[0] is Digit))
            throw new Exception("Не удалось расчитать результат");

         return ((Digit)_Lexems[0]).Value;
      }
      /// <summary>
      /// Задание значения
      /// </summary>
      /// <param name="Index">Индекс значения</param>
      public void SetValue(int Index, int Value)
      {
         foreach (Lexem lexem in Lexems)
            if (lexem is Digit)
            {
               Digit digit = (Digit)lexem;
               if (digit.Str == Index.ToString())
                  digit.Value = Value;
            }
      }
      /// <summary>
      /// Получение значения по индексу
      /// </summary>
      /// <param name="Index">Индекс</param>
      /// <returns>Значение</returns>
      /// <exception cref="Exception - Неверно задан индекс" />
      public int GetValue(int Index)
      {
         foreach (Lexem lexem in Lexems)
            if (lexem is Digit)
            {
               Digit digit = (Digit)lexem;
               if (digit.Str == Index.ToString())
                  return digit.Value;
            }

         throw new Exception("Неверно задан индекс");
      }
      /// <summary>
      /// Получение массива индексов
      /// </summary>
      /// <returns>Массив индексов</returns>
      public IEnumerable<String> GetIndexes()
      {
         return Lexems.Where(n => n is Digit).Select(n => n.Str).Distinct();
      }
      #endregion

      #region Private Methods
      /// <summary>
      /// Удаление пробелов из выражения
      /// </summary>
      /// <param name="Exp">Выражение</param>
      private String DelBlanks(String Exp)
      {
         String _Exp = "";

         for (int i = 0; i < Exp.Length; ++i)
         {
            if (Exp[i] != ' ')
               _Exp += Exp[i];
         }

         return _Exp;
      }
      /// <summary>
      /// Проверка корректности скобок
      /// </summary>
      /// <param name="Exp">Выражение, разбитое на лексемы</param>
      /// <exception cref="Exception - В выражении неверно расставлены скобки" />
      private void CheckBracked(List<String> Exp)
      {
         Stack<String> Brackets = new Stack<String>();

         foreach (String lexem in Exp)
         {
            if (lexem == "(")
               Brackets.Push(lexem);
            else if (lexem == ")")
            {
               if (Brackets.Count == 0)
                  throw new Exception("В выражении неверно расставлены скобки");

               Brackets.Pop();
            }
         }

         if (Brackets.Count != 0)
            throw new Exception("В выражении неверно расставлены скобки");
      }
      /// <summary>
      /// Разбор выражения
      /// </summary>
      /// <param name="Exp">Выражение</param>
      private void ParsExp(List<String> Exp)
      {
         Stack<String> Ops = new Stack<String>();
         bool IsOpenBracket = false;

         for (int i = 0; i < Exp.Count; ++i)
         {
            
            if (Op.IsOp(Exp[i]))            //Если это операция
               Ops.Push(Exp[i]);
            else if (Op.IsBinOp(Exp[i]))    //Если это бинарная операция
               Ops.Push(Exp[i]);
            else if (Exp[i] == "(")         //Если это открывающая скобка
               IsOpenBracket = true;
            else if (Exp[i] == ")")         //Если это закрывающая скобка
            {
               while (Ops.Count > 0)
               {
                  Op op = new Op();
                  op.Str = Ops.Pop();
                  Lexems.Add(op);
               }
            }
            else                            //Если это значение
            {
               Digit digit = new Digit();
               digit.Str = Exp[i];
               Lexems.Add(digit);

               if ((Ops.Count > 0) && !IsOpenBracket)
               {
                  Op op = new Op();
                  op.Str = Ops.Pop();
                  Lexems.Add(op);
               }

               IsOpenBracket = false;
            }//if_else
         }//for

         while (Ops.Count > 0)              //Выталкивание всех операций
         {
            Op op = new Op();
            op.Str = Ops.Pop();
            Lexems.Add(op);
         }
      }
      /// <summary>
      /// Получение списка лексем
      /// </summary>
      /// <param name="Exp">Выражение</param>
      /// <returns>Список лексем</returns>
      private List<String> GetLexems(String Exp)
      {
         List<String> lexems = new List<String>();
         String DigitStr = "";

         for (int i = 0; i < Exp.Length; ++i)
         {
            String CharExp = Exp[i].ToString();

            if (Op.IsOp(CharExp))    //Если это операция
            {
               if (DigitStr.Length != 0)
               {
                  lexems.Add(DigitStr);
                  DigitStr = "";
               }

               if (i < Exp.Length - 1)
               {
                  String CharExp2 = Exp[i + 1].ToString();
                  if (Op.IsBinOp(CharExp + CharExp2))
                  {
                     lexems.Add(CharExp + CharExp2);
                     ++i;
                     continue;
                  }
               }

               lexems.Add(CharExp);
            }
            else if (CharExp == "(") //Если это открывающая скобка
            {
               lexems.Add(CharExp);
            }
            else if (CharExp == ")") //Если это закрывающая скобка
            {
               if (DigitStr.Length != 0)
               {
                  lexems.Add(DigitStr);
                  DigitStr = "";
               }

               lexems.Add(CharExp);
            }
            else                     //Если это значение
               DigitStr += CharExp;
         }

         if (DigitStr.Length != 0)
            lexems.Add(DigitStr);

         return lexems;
      }
      #endregion
      #endregion
   }
}
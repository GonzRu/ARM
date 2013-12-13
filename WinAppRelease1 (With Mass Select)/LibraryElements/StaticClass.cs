using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace LibraryElements
{
   /// <summary>
   /// Класс работы с метафайлами
   /// </summary>
   public static class WorkMetaFile
   {
      /// <summary>
      /// Создаем графику из контрола
      /// </summary>
      /// <returns>графига</returns>
      private static Graphics GetWindowGraphic()
      {
         System.Windows.Forms.PictureBox cntrl = new System.Windows.Forms.PictureBox();
         cntrl.ClientSize = new Size(200, 200); //размер рабочей области холста
         return cntrl.CreateGraphics();
      }
      /// <summary>
      /// Создание основы метафайла
      /// </summary>
      /// <returns>метафайл</returns>
      private static Metafile CreateBaseFile()
      {
         Metafile imgmf = null;
         Graphics wingraphics = GetWindowGraphic();
         IntPtr ip_hdc = wingraphics.GetHdc();
         try
         {
            imgmf = new Metafile(ip_hdc, new Rectangle(0, 0, 200, 200), MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
         }
         catch
         {
            imgmf = null;
         }
         finally
         {
            wingraphics.ReleaseHdc(ip_hdc);
            wingraphics.Dispose();
         }
         return imgmf;
      }
      /// <summary>
      /// Рисуем в метафайл элементы
      /// </summary>
      /// <param name="_mf">метафайл</param>
      /// <param name="_lst">список элементов</param>
      private static void DrawtoGraphic(Metafile _mf, List<Element> _lst)
      {
         Graphics metagraphics = Graphics.FromImage(_mf);
         foreach (Element elem in _lst)
         {
            elem.IsSelected = false;
            elem.DrawElement(metagraphics);
         }
         metagraphics.Dispose();
      }
      /// <summary>
      /// Рисуем в метафайл элементы
      /// </summary>
      /// <param name="_mf">метафайл</param>
      /// <param name="_lst">список элементов</param>
      /// <param name="_clr">цвет отрисовки</param>
      private static void DrawtoGraphic(Metafile _mf, List<Element> _lst, Color _clr)
      {
         Graphics metagraphics = Graphics.FromImage(_mf);
         foreach (Element elem in _lst)
         {
            Color clr = elem.ElementColor;  //подменяем цвет для отрисовки
            elem.ElementColor = _clr;

            elem.IsSelected = false;
            elem.DrawElement(metagraphics);

            elem.ElementColor = clr;        //возвращаем подменяемый цвет обратно
         }
         metagraphics.Dispose();
      }
      /// <summary>
      /// Создание метафайла из исписка элементов
      /// </summary>
      /// <param name="_lst">список элементов</param>
      /// <returns>метафайл</returns>      
      public static Metafile CreateMetaFile(List<Element> _lst)
      {
         Metafile imgmf = CreateBaseFile();
         if (imgmf != null)
            DrawtoGraphic(imgmf, _lst);
         return imgmf;
      }
      /// <summary>
      /// Создание метафайла из исписка элементов
      /// </summary>
      /// <param name="_lst">список элементов</param>
      /// <param name="_clr">цвет отрисовки</param>
      /// <returns>метафайл</returns>      
      public static Metafile CreateMetaFile(List<Element> _lst, Color _clr)
      {
         Metafile imgmf = CreateBaseFile();
         if (imgmf != null)
            DrawtoGraphic(imgmf, _lst, _clr);
         return imgmf;
      }
      /// <summary>
      /// Записать метафайл на диск
      /// </summary>
      /// <param name="_path">путь</param>
      /// <param name="_lst">список элементов</param>
      public static bool SaveMetaFile(String _path, List<Element> _lst)
      {
         Metafile imgmf = null;
         Graphics wingraphics = GetWindowGraphic();
         IntPtr ip_hdc = wingraphics.GetHdc();

         try
         {
            imgmf = new Metafile(_path, ip_hdc, new Rectangle(0, 0, 200, 200), MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
         }
         catch
         {
            wingraphics.ReleaseHdc(ip_hdc);
            wingraphics.Dispose();
            return false;
         }

         DrawtoGraphic(imgmf, _lst);

         wingraphics.ReleaseHdc(ip_hdc);
         wingraphics.Dispose();
         return true;
      }
   }
}

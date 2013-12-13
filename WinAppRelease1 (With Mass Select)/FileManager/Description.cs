using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using LibraryElements;
using Structure;

namespace FileManager
{
   /// <summary>
   /// Класс чтения файла описания вариантов изображения
   /// </summary>
   public class BuilderDescription : LinqStream
   {
      #region Parameters
      String fig_name, fig_type;
      Dictionary<String, List<Element>> dictlist;
      #endregion

      #region Class Methods
      #region Public Methods
      public BuilderDescription()
      {
         fig_name = fig_type = String.Empty;
         dictlist = new Dictionary<String, List<Element>>();
      }
      /// <summary>
      /// Получить список всех элементов из которых состоит изображение
      /// (вариант изображения - список элементов)
      /// </summary>
      public Dictionary<String, List<Element>> GetAllDiscription()
      {
         if (error_flag) return null;

         return dictlist;
      }
      /// <summary>
      /// Получить список элементов из которых состоит изображение
      /// (вариант изображения - список элементов)
      /// </summary>
      /// <param name="_name">вариант отображения</param>
      public List<Element> GetDescription(String _name)
      {
         List<Element> lst = null;
         if (error_flag || dictlist == null || dictlist.Count == 0)
            return null;

         try { lst = dictlist[_name]; }
         catch { lst = null; }

         return lst;
      }
      /// <summary>
      /// Получить мета файл по варианту отображения
      /// </summary>
      /// <param name="_tag_name">вариант отображения</param>
      /// <returns>собраная картинка</returns>
      public Image GetImage(String _tag_name)
      {
         if (error_flag || dictlist == null || dictlist.Count == 0)
            return null;

         return WorkMetaFile.CreateMetaFile(GetDescription(_tag_name));
      }
      /// <summary>
      /// Разбор файла
      /// </summary>
      public void ParceFile()
      {
         if (this.error_flag) return;

         xroot = xdoc.Element("namespace");
         
         XElement data = xroot.Element("custom_pics");
         ParcePicture(data.Element("pictures"));
      }
      #endregion

      #region Private Methods
      /// <summary>
      /// Получаем элемент по типу
      /// </summary>
      /// <param name="_type">тип элемента</param>
      /// <param name="_turn">поворот фигуры</param>
      /// <returns>фигура</returns>
      private Figure GetImageElem(string _type, string _turn)
      {
         switch (_type)
         {
            case "Arc": return new EditorArc(ConvertMethods.GetTurnPosition(_turn));
            case "Ellipse": return new PrimEllipse();
            case "Rectangle": return new PrimRectangle();
            case "FillEllipse": return new EditorFillEllipse();
            case "FillRectangle": return new EditorFillRectangle();
            default: return null;
         }
      }
      /// <summary>
      /// Получаем линию по типу
      /// </summary>
      /// <param name="_type">тип линии</param>
      /// <returns>линия</returns>
      private Line GetImageLine(string _type)
      {
         Line tmp = null;

         switch (_type)
         {
            case "SingleLine":
               {
                  Line ln = new Line();
                  ln.IsSelected = false;
                  tmp = ln;
               }
               break;
            case "PolyLine":
               {
                  PolyLine pln = new PolyLine();
                  pln.IsSelected = false;
                  tmp = pln;
               }
               break;
            default: break;
         }

         return tmp;
      }
      /// <summary>
      /// Составление списка элементов картинки
      /// </summary>
      /// <param name="_data">xml ветвь</param>
      private void ParcePicture(XElement _data)
      {
         IEnumerable<XElement> imgs = _data.Elements("image");

         foreach (XElement elem in imgs)
         {
            String attr_name;
            try { attr_name = elem.Attribute("status").Value; }
            catch { attr_name = String.Empty; }
            
            if (attr_name != String.Empty)
               dictlist.Add(attr_name, ParceImages(elem));
         }
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="_data"></param>
      /// <returns></returns>
      private List<Element> ParceImages(XElement _data)
      {
         IEnumerable<XElement> ieels = _data.Elements("elem");
         IEnumerable<XElement> ieellns = _data.Elements("line_elem");
         List<Element> lst = new List<Element>(ieellns.Count() + ieels.Count());

         #region Figures
         foreach (XElement elem in ieels)
         {
            XElement xrect, xtype, xstyle;
            string str_name, str_turn;
            Color clr1 = new Color(), clr2 = new Color();
            xrect = elem.Element("rect");
            int x = Convert.ToInt32(xrect.Attribute("x").Value);
            int y = Convert.ToInt32(xrect.Attribute("y").Value);
            int w = Convert.ToInt32(xrect.Attribute("width").Value);
            int h = Convert.ToInt32(xrect.Attribute("height").Value);
            
            xtype = elem.Element("type");
            str_name = xtype.Attribute("figure_name").Value;
            try { str_turn = xtype.Attribute("turn").Value; }
            catch { str_turn = String.Empty; }

            if (xtype.HasElements)
            {
               xstyle = xtype.Element("style");
               clr1 = ConvertMethods.GetParseColor(xstyle.Attribute("colorup").Value);
               clr2 = ConvertMethods.GetParseColor(xstyle.Attribute("colordown").Value);
            }

            Figure fgr = GetImageElem(str_name, str_turn);
            if (fgr != null)
            {
               fgr.SetPosition(new Point(x, y));
               fgr.SetSize(new Size(w, h));
               fgr.IsSelected = false;
               if (fgr is IEditorColor)
                  ((IEditorColor)fgr).SetColor(clr1, clr2);

               lst.Add(fgr);
            }//if
         }//foreach
         #endregion

         #region Lines
         foreach (XElement elem in ieellns)
         {
            int w = 1;
            string points, str_name;
            Color clr = new Color();
            DashStyle dstyle = DashStyle.Solid;
            XElement xline, xtype, xstyle;

            xline = elem.Element("line");
            points = xline.Attribute("line_points").Value;

            xtype = elem.Element("type");
            str_name = xtype.Attribute("figure_name").Value;

            xstyle = xtype.Element("style");
            if (xstyle != null)
            {
               clr = ConvertMethods.GetParseColor(xstyle.Attribute("color").Value);
               w = Convert.ToInt32(xstyle.Attribute("width").Value);
               dstyle = ConvertMethods.ParseLineStyle(xstyle.Attribute("ln_style").Value);
            }

            Line fgr_ln = GetImageLine(str_name);
            if (fgr_ln != null)
            {
               ((IElementLine)fgr_ln).SetReadPoints(points);
               fgr_ln.IsSelected = false;
               if (xstyle != null)
               {
                  fgr_ln.ElementColor = clr;
                  fgr_ln.Thickness = w;
                  fgr_ln.LineStyle = dstyle;
               }
               lst.Add(fgr_ln);
            }
         }//foreach
         #endregion

         return lst;
      }
      #endregion
      #endregion

      #region Override method
      public override void Dispose()
      {
         base.Dispose();

         dictlist.Clear();
         dictlist = null;
      }
      #endregion
   }
   /// <summary>
   /// Класс создания описания внешнего вида элемента
   /// </summary>
   public class CreaterDescription : LinqStream
   {
      #region Parameters
      List<XElement> lstnodes;
      #endregion

      #region Class Methods
      public CreaterDescription()
      {
         lstnodes = new List<XElement>();
      }
      public void CreateImageDescription(String _name, List<Element> _lst)
      {
         XElement root = new XElement("image");
         XAttribute xattr = new XAttribute("status", _name);
         root.Add(xattr);

         foreach (Element elem in _lst)
         {
            XElement xchild = null;
            if (elem is Figure) xchild = CreateElem(elem);
            if (elem is Line) xchild = CreateLine((Line)elem);

            if (xchild != null)
               root.Add(xchild);
         }//foreach

         lstnodes.Add(root);
      }
      public void CreateDescription(String _name_description)
      {
         CreateDescription(_name_description, String.Empty);
      }
      public void CreateDescription(String _name_description, String _category)
      {
         XElement ns = CreateDeclaration();
         XElement root = new XElement("custom_pics");
         ns.Add(root);
         XAttribute xtype = new XAttribute("figure_name", _name_description);         
         root.Add(xtype);         
         if (_category != String.Empty)
         {
            xtype = new XAttribute("category", _category);
            root.Add(xtype);
         }

         XElement xrics = new XElement("pictures");
         root.Add(xrics);

         foreach (XElement elem in lstnodes)
            xrics.Add(elem);
      }

      #region Create Figure
      private XElement CreateElem(Element _elem)
      {
         XElement xchild = new XElement("elem");
         XElement xnode1 = CreateRect((Figure)_elem);
         XElement xnode2 = CreateType(_elem);
         xchild.Add(xnode1);
         xchild.Add(xnode2);

         return xchild;
      }
      private XElement CreateRect(Figure _fgr)
      {
         Rectangle rect = _fgr.GetPosition();
         XElement xchild = new XElement("rect");
         
         XAttribute xattr = new XAttribute("x", rect.X); xchild.Add(xattr);
         xattr = new XAttribute("y", rect.Y); xchild.Add(xattr);
         xattr = new XAttribute("width", rect.Width); xchild.Add(xattr);
         xattr = new XAttribute("height", rect.Height); xchild.Add(xattr);

         return xchild;
      }
      private XElement CreateType(Element _elem)
      {
         XElement xchild = new XElement("type");
         XAttribute xattr = null;

         if (_elem is EditorArc)
         {
            xattr = new XAttribute("figure_name", "Arc");
            xchild.Add(xattr);
            xattr = new XAttribute("figure_type", "static");
            xchild.Add(xattr);
            xattr = new XAttribute("turn", ((EditorArc)_elem).TurnPosition.ToString());
            xchild.Add(xattr);
         }//if
         else if (_elem is PrimEllipse)
         {
            xattr = new XAttribute("figure_name", "Ellipse");
            xchild.Add(xattr);
            xattr = new XAttribute("figure_type", "static");
            xchild.Add(xattr);
         }//if
         else if (_elem is PrimRectangle)
         {
            xattr = new XAttribute("figure_name", "Rectangle");
            xchild.Add(xattr);
            xattr = new XAttribute("figure_type", "static");
            xchild.Add(xattr);
         }//if
         else if (_elem is EditorFillEllipse)
         {
            xattr = new XAttribute("figure_name", "FillEllipse");
            xchild.Add(xattr);
            xattr = new XAttribute("figure_type", "static");
            xchild.Add(xattr);
            xchild.Add(CreateColor((IEditorColor)_elem));
         }//if
         else if (_elem is EditorFillRectangle)
         {
            xattr = new XAttribute("figure_name", "FillRectangle");
            xchild.Add(xattr);
            xattr = new XAttribute("figure_type", "static");
            xchild.Add(xattr);
            xchild.Add(CreateColor((IEditorColor)_elem));
         }//if

         return xchild;
      }
      private XElement CreateColor(IEditorColor _clr_elem)
      {
         XElement xchild = new XElement("style");
         XAttribute xattr = new XAttribute("colorup", ConvertMethods.GetColorString(_clr_elem.GetUpColor()));
         xchild.Add(xattr);
         xattr = new XAttribute("colordown", ConvertMethods.GetColorString(_clr_elem.GetDownColor()));
         xchild.Add(xattr);
         return xchild;
      }
      #endregion

      #region Create Line
      private XElement CreateLine(Line _ln)
      {
         XElement xchild = new XElement("line_elem");
         XElement xnode1 = CreateLnLine(_ln);
         XElement xnode2 = CreateLnType(_ln);
         XElement xnode3 = CreateLnStyle(_ln);
         xchild.Add(xnode1);
         xchild.Add(xnode2);
         xchild.Add(xnode3);

         return xchild;
      }
      private XElement CreateLnLine(Line _ln)
      {
         XElement xchild = new XElement("line");
         xchild.Add(new XAttribute("line_points", _ln.GetPoints()));

         return xchild;
      }
      private XElement CreateLnType(Line _ln)
      {
         XElement xchild = new XElement("type");
         XAttribute xattr = null;

         if (_ln is PolyLine)
         {
            xattr = new XAttribute("figure_name", "PolyLine");
            xchild.Add(xattr);
         }
         else
         {
            xattr = new XAttribute("figure_name", "SingleLine");
            xchild.Add(xattr);
         }
         xattr = new XAttribute("figure_type", "static");
         xchild.Add(xattr);

         return xchild;
      }
      private XElement CreateLnStyle(Line _ln)
      {
         XElement xchild = new XElement("style");
         XAttribute xattr = new XAttribute("color", ConvertMethods.GetColorString(_ln.ElementColor));
         xchild.Add(xattr);
         xattr = new XAttribute("width", _ln.Thickness.ToString());
         xchild.Add(xattr);
         xattr = new XAttribute("ln_style", _ln.LineStyle.ToString());
         xchild.Add(xattr);

         return xchild;
      }
      #endregion
      #endregion

      #region Override method
      public override void Dispose()
      {
         base.Dispose();

         lstnodes.Clear();
         lstnodes = null;
      }
      #endregion
   }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using HMI_MT_Settings;

namespace HMI_MT
{
   public partial class frmSplashScreen : Form
   {
      public frmSplashScreen()
	  {
		  InitializeComponent();

		  // проверяем существование файла конфигурации устройств проекта
		  string PathToPrjFile = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";

		  if (!File.Exists(PathToPrjFile))
			  throw new Exception("Файл проекта отсутсвует: " + PathToPrjFile);

		  XDocument xdoc = XDocument.Load(PathToPrjFile);
		  HMI_Settings.PathToPrjFile = PathToPrjFile;

          this.LoadScreenTexts( xdoc );

		  // проверяем существование файла с изображением на заставку
		  LoadSplashScreen(xdoc);
	  }

       /// <summary>
       /// Загрузка текста на экран заставки
       /// </summary>
       /// <param name="xDoc"></param>
       private void LoadScreenTexts( XDocument xDoc )
       {
           var xElement = xDoc.Element( "Project" );
           if ( xElement != null )
           {
               var element = xElement.Element( "NamePTK" );
               if ( element != null )
               {
                   var xTexts = element.Elements( "Line" );
                   var count = xTexts.Count(); // кол-во линий текста
                   var height = ( count > 0 )  // вычисление размера высоты контрола текста
                                    ? this.splitContainer2.Panel1.Height / count
                                    : this.splitContainer2.Panel1.Height;

                   var fontHeight = height - height / 3;  // вычисление высоты шрифта текста
                   if ( count == 1 )                      // если строка 1
                       fontHeight = 38;                   // то назначаем стандартный размер

                   var font = new Font( "Arial", fontHeight, FontStyle.Bold );
                   for ( var i = count - 1; i >= 0; --i ) // добавляем контролы текста в обратном порядке
                   {                                      // для правильности отображения
                       var label = new Label
                           {
                               Text = xTexts.ElementAt( i ).Value,
                               TextAlign = ContentAlignment.MiddleCenter,
                               Dock = DockStyle.Top,
                               Font = font,
                               Height = height
                           };

                       this.splitContainer2.Panel1.Controls.Add( label );
                   }
               }
           }
       }

       /// <summary>
       /// загрузка картинки для заставки из 
       /// файла указ в Project.cfg
       /// </summary>
       /// <param name="xdoc"></param>
       private void LoadSplashScreen(XDocument xdoc)
	  {
		  if (!String.IsNullOrWhiteSpace((string)xdoc.Element("Project").Element("Foto4SplashScreen")))
			  try
			  {
				  string PathToFotoForSplashScreen = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar
					  + xdoc.Element("Project").Element("Foto4SplashScreen").Value;
				  if (File.Exists(PathToFotoForSplashScreen))
					  pictureBox1.Load(PathToFotoForSplashScreen);
			  }
			  catch (Exception ex)
			  {
				  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			  }
	  }
   }
}

using System;
using System.Windows.Forms;

using HelperControlsLibrary;
using InterfaceLibrary;

namespace DevicesLibrary
{
    /// <summary>
    /// Форма всех блоков
    /// </summary>
    public partial class FormBlock : Form, IDeviceForm, HelperControlsLibrary.ReportLibrary.IReport
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="typeBlock">Имя типа блока</param>
        /// <param name="unids">Идентификатор датасервера</param>
        /// <param name="unidev">Идентификатор устройства</param>
        public FormBlock( String typeBlock, UInt32 unids, UInt32 unidev )
        {
            InitializeComponent( );
            this.InitComponents( typeBlock, unids, unidev );
            this.Guid = unidev;
        }
        /// <summary>
        /// Инициализация вкладок
        /// </summary>
        /// <param name="typeBlock">Имя типа блока</param>
        /// <param name="unids">Идентификатор датасервера</param>
        /// <param name="unidev">Идентификатор устройства</param>
        private void InitComponents( String typeBlock, UInt32 unids, UInt32 unidev )
        {
            tabControl.Controls.Add( new BlockViewTagPage( this, unids, unidev ) );

            switch ( typeBlock )
            {
                case "BMRZ_DZ_01_11":
                case "BMRZ_DZSH_02_12":
                case "BMRZ_KL_33_12":
                case "BMRZ_SV_32_12":
                case "BMRZ_TD_30_11":
                case "BMRZ_TR_06_40_14":
                case "BMRZ_VV_14_31_12":
                case "BMRZ_DZSH_02_11":
                case "BMRZ_BRCN_100_A_01_250108":
                case "BMRZ_BRCN_100_A_01_111110":
                case "BMRZ_KL_09_33_12":
                case "BMRZ_SV_44_12":
                case "BMRZ_VV_43_13":                
                    {
                        tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.TwoPanel) );
                    }
                    break;
                case "Ekra_BE_2704V014_047":
                case "Ekra_BE_2704V015_045":
                case "Ekra_BE_2704V016_045":
                case "Ekra_BE_2704V016_052":
                case "Ekra_BE_2704V016_052v2":
                case "Ekra_BE_2704V021_200":
                case "Ekra_BE_2704V022_054":
                case "Ekra_BE_2704V061_045":
                case "Ekra_BE_2704V061_200":
                case "Ekra_BE_2704V081_062":
                case "Ekra_BE_2704V085_100":
                case "Ovod_MD":               
                case "БРЧН_100_B_01_191110":
                case "БМРЗ_104_TH_05_230511":
                case "БРЧН_100_A_01_111110":
                case "PNZP_M_80":
                case "Sirius_2_L_5A_311":
                case "Sirius_2_ML_5A_312":
                case "Sirius_2_OB_102":
                case "Sirius_2_RN_1_04":
                case "Sirius_2_S_202":
                case "Sirius_2_S_312":
                case "Sirius_2_V_310":
                case "Sirius_3_DFZ_01":
                case "Sirius_3_DZSH_115":
                case "Sirius_3_LV_03_200":
                case "Sirius_3_SV":
                case "Sirius_IMF_1R_102":
                case "Sirius_IMF_3R_503":
                case "Sirius_T3_306":
                case "Sirius_TN":
                case "Sirius_UV_312":
                    {                        
                        tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
                        tabControl.Controls.Add( new InformationTabPage( unidev ) );
                    }
                    break;
                case "Sirius_CS_300":
                    {
                        tabControl.Controls.Add( new InformationTabPage( unidev ) );
                    }
                    break;
                case "ITDS_VirtualDevice":
                    break;
                    //case "USO_MT_D_04":
                    //case "USO_MT_D_05":
                    //case "USO-MTR-2-0-24-8":
                    //case "USO-MTR-2-0-24-0":
                    //case "ITDS_DIN32C":
                    //case "ITDS_DOUT8":
                    //case "NOVAR_206":
                    //case "Sepam S4x":
                    //case "Sepam S8x":
                case "BMCS_10":
                    {                     
                       tabControl.Controls.Add( new InformationTabPage( unidev ) );
                    }
                    break;
                case "2704V073_040":
                case "2704V041_049":
                case "2704V015_045":
                case "2704V062_040":
                case "2704V021_055":
                case "2704V012_052":
                    {
                        tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms) );
                    }
                    break;
            }

            tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
            tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( unidev ) );
        }

        /// <summary>
        /// Активировать определенную вкладку
        /// </summary>
        public void ActivateTabPage( string typetabpage )
        {
            /*throw new NotImplementedException();*/
        }
        /// <summary>
        /// Действия по завершению чтения аварии 
        /// </summary>
        public void reqAvar_OnReqExecuted( IRequestData req )
        {
            /*throw new NotImplementedException();*/
        }
        /// <summary>
        /// Идентификатор блока/устройства
        /// </summary>
        public UInt32 Guid { get; private set; }
        /// <summary>
        /// Печать
        /// </summary>
        public void Print( ) { ( (BlockViewTagPage)tabControl.Controls[0] ).Print( ); }
    }
}
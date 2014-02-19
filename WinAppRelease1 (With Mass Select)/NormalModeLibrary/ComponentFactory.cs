using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.Integration;
using System.Xml.Linq;
using System.Windows.Forms;
using InterfaceLibrary;
using NormalModeLibrary.Sources;
using NormalModeLibrary.ViewModel;
using NormalModeLibrary.Windows;

namespace NormalModeLibrary
{
    public class ComponentFactory
    {
        static ComponentFactory factory;
        static readonly string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"Project\CurrentModePanel.xml";
        //private static readonly string PanelType = "windowAndWPFPanel";
        //private static readonly string PanelType = "controlAndWPFPanel";
        private static readonly string PanelType = "controlAndWinFormPanel";

        private Form mainMnemoHandle;
        private bool isLoad = false;

        readonly List<UserViewModel> users = new List<UserViewModel>();
        readonly  List<INormalModePanel> activeNormalModePanels = new List<INormalModePanel>(); 

        private ComponentFactory() { }
        private List<PanelViewModel> GetPanels( Places place )
        {
            var panels = new List<PanelViewModel>();
            foreach ( UserViewModel user in users )
                if ( user.Login == HMI_MT_Settings.HMI_Settings.UserName )
                    foreach ( ConfigurationViewModel config in user.Collection )
                        if ( config.ActiveTime == user.ActiveTime && config.IsActive && config.Place == place )
                            foreach ( PanelViewModel panel in config.Collection )
                                if ( panel.Collection.Count > 0 )
                                    panels.Add( panel );
            return panels;
        }        
        public void LoadXml()
        {
            if ( !isLoad )
            {
                var xdoc = XDocument.Load( FilePath );
                var nodes = xdoc.Element( "MTRA" ).Elements( "User" );

                foreach ( var node in nodes )
                {
                    var user = new User();
                    user.ParseXml( node );
                    users.Add( new UserViewModel( user ) );
                }

                isLoad = true;
            }
        }
        public bool SaveXml()
        {
            return SaveXml( users );
        }
        public void ActivatedMainMnemoForms( Form form )
        {
            if (activeNormalModePanels.Count > 0)
                DeactivatedMainMnemoForms();
           
            mainMnemoHandle = form;

            foreach ( var vmPanel in GetPanels( Places.MainMnemo ) )
            {
                if (!vmPanel.IsVisible)
                    continue;

                var panel = NormalModePanelFactory.CreatePanel(PanelType);
                panel.Component = vmPanel;
                panel.Place = Places.MainMnemo;
                panel.SetOwner(Factory.mainMnemoHandle);
                panel.ActivatedComponent();

                activeNormalModePanels.Add(panel);
            }

            Application.OpenForms[0].Activate();
        }
        public void DeactivatedMainMnemoForms()
        {
            if (activeNormalModePanels.Count > 0)
                foreach (INormalModePanel view in activeNormalModePanels.ToArray())
                    if ( view.Place == Places.MainMnemo )
                    {
                        view.DeactivatedComponent();
                        activeNormalModePanels.Remove(view);
                    }
        }
        public void SetStates( FormWindowState state )
        {
            //foreach (ViewElementHost view in activePanelControls)
            //    switch ( state )
            //    {
            //        case FormWindowState.Maximized: view.WindowState = FormWindowState.Normal; break;
            //        default: view.WindowState = state; break;
            //    }
        }

        public static void EditSignals( IDevice device, String login, Places place )
        {
            var win = new SelectSignalsWindow();

            var user = GetUser(Factory.users, login);
            var config = GetConfig(user, place);
            var panel = GetPanel(config, device);

            #region Find or create ViewWindow
            var view = Factory.activeNormalModePanels.FirstOrDefault(apf => apf.Component == panel);
            if (view == null)
            {
                view = NormalModePanelFactory.CreatePanel(PanelType);
                view.Component = panel;
                view.Place = place;
                view.SetOwner(Factory.mainMnemoHandle);

                Factory.activeNormalModePanels.Add(view);                
            }

            view.ActivatedComponent();
            #endregion

            #region SelectSignalWindow work
            win.AddComponents( device, view );

            if ( win.ShowDialog() == DialogResult.OK )
            {
                RemovePanelViewModelToConfigurationViewModel(config, win.GetOriginalWindowComponent());

                AddPanelViewModelFromConfigurationViewModel(config, view.Component);

                ComponentFactory.Factory.SaveXml();
            }
            #endregion

            if (view.Component.Collection.Count == 0)
            {
                view.DeactivatedComponent();
                Factory.activeNormalModePanels.Remove(view);
            }
        }
        public static void EditUserWindows()
        {
            var win = new Windows.UserWindows( Factory.users ) { Owner = Factory.mainMnemoHandle };
            win.Show();
        }

        internal static bool SaveXml( List<UserViewModel> users )
        {
            var xdoc = new XDocument( new XDeclaration( "1.0", "utf-8", "true" ) );
            var node = new XElement( "MTRA" );
            xdoc.Add( node );

            foreach ( UserViewModel userModel in users )
                node.Add( userModel.Core.CreateXml() );

            try { xdoc.Save( FilePath ); }
            catch { return false; }

            return true;
        }
        private static UserViewModel GetUser( List<UserViewModel> users, String login )
        {
            var userModel = users.FirstOrDefault( u => u.Login == login );
            if ( userModel == null )
            {
                var user = new User { Login = login };
                userModel = new UserViewModel( user );
                users.Add( userModel );
            }
            return userModel;
        }
        private static ConfigurationViewModel GetConfig( UserViewModel model, Places place )
        {
            ConfigurationViewModel configModel = null;
            foreach ( ConfigurationViewModel cfgModel in model.Collection )
                if ( cfgModel.IsActive && cfgModel.ActiveTime == model.ActiveTime && cfgModel.Place == place )
                {
                    configModel = cfgModel;
                    break;
                }

            if ( configModel == null )
            {
                var config = new Configuration { ActiveTime = model.ActiveTime, Place = place };

                ( (User)model.Core ).Collection.Add( config );
                configModel = new ConfigurationViewModel( config );
                model.Collection.Add( configModel );
            }

            return configModel;
        }
        private static PanelViewModel GetPanel( ConfigurationViewModel model, IDevice device )
        {
            PanelViewModel panel = null;
            foreach ( PanelViewModel panelModel in model.Collection )
                if ( panelModel.ObjectGuid == device.UniObjectGUID )
                {
                    panel = panelModel;
                    break;
                }

            if ( panel == null )
            {
                var pnl = new Sources.Panel { ObjectGuid = device.UniObjectGUID, Type = Sources.Panel.LinkType.Named, Caption = device.Description};

                ( (Configuration)model.Core ).Collection.Add( pnl );
                panel = new PanelViewModel( pnl );
                model.Collection.Add( panel );
            }

            return panel;
        }

        public static ComponentFactory Factory
        {
            get { return factory ?? ( factory = new ComponentFactory( ) ); }
        }

        private static void AddPanelViewModelFromConfigurationViewModel(ConfigurationViewModel model, PanelViewModel p)
        {
            ((Configuration)model.Core).Collection.Add(p.BasePanel);
            model.Collection.Add(p);
        }

        private static void RemovePanelViewModelToConfigurationViewModel(ConfigurationViewModel model, PanelViewModel p)
        {
            ((Configuration)model.Core).Collection.Remove(p.BasePanel);
            model.Collection.Remove(p);
        }
    }
}

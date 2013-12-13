using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace InterfaceLibrary
{
	public interface ISrcCfgManager
	{
		/// <summary>
		/// Инициировать работу источника
		/// </summary>
		IConfiguration BeginWork(string UniDS_GUID, XElement srcinfo, ref SortedList<int, IObjectTAgs> Sl4Access2TagsSetByObjectGUID, SortedList<string, string> slGlobalListTagsType);
		/// <summary> 
		/// получить название источника данных
		/// </summary>
		/// <returns></returns>
		string GetSrcName();
		///// <summary>
		///// Список контроллеров источника
		///// </summary>
		///// <returns></returns>
		//List<IController> GetSrcECUs();
		/// <summary>
		/// Реинициировать работу источника
		/// </summary>
		void WorkRestart();
		/// <summary>
		/// Получить состояние источника
		/// </summary>
		void GetState();
		/// <summary>
		/// Получить данные устройств источника (все)
		/// </summary>      
		ArrayList GetData();
		/// <summary>
		/// Передать данные
		/// </summary>      
		void SendData(byte[] arr);
		/// <summary>
		/// Получить данные устройств источника (только обновленные)
		/// </summary>      
		ArrayList GetDataReNew();
		/// <summary>
		/// получить конфигурацию от источника данных
		/// </summary>
		/// <returns></returns>
		XElement GetConfiguration(string namesrc);
		/// <summary>
		/// выполнить команду
		/// </summary>
		/// <param name="cmd"></param>
		void RunCMD(int objectGUID, int tagguid);
		/// <summary>
		/// получить доступ к конфигурации тегов, групп и т.п. источника
		/// </summary>
		/// <returns></returns>
		IConfiguration GetSourceConfiguration();
	}
}

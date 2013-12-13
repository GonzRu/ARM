using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace DSDecoy
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IWcfDataServer
	{
		[OperationContract]
		byte[] GetDataServerData(string value);
		[OperationContract]
		MemoryStream GetDSValueAsByteBuffer(byte[] arr);
		[OperationContract]
		void RunCMD(byte numksdu, ushort numvtu, int tagguid, byte[] arr);

		#region заготовка для асинхроннного обмена с сервером
		//[OperationContract(AsyncPattern = true, IsOneWay = true)]
		//IAsyncResult BeginRunReq(AsyncCallback callback);

		//void EndRunReq(IAsyncResult rezult); 
		#endregion

		#region может понадобится ?
		//[OperationContract]
		//MemoryStream GetFCState();
		//[OperationContract]
		//byte[] GetOscill(int idblock);

		//[OperationContract]
		//CompositeType GetDataUsingDataContract(CompositeType composite); 
		#endregion
	}

	#region Пример WCF-определений
	// Use a data contract as illustrated in the sample below to add composite types to service operations
	//[DataContract]
	//public class CompositeType
	//{
	//    bool boolValue = true;
	//    string stringValue = "Hello ";

	//    [DataMember]
	//    public bool BoolValue
	//    {
	//        get { return boolValue; }
	//        set { boolValue = value; }
	//    }

	//    [DataMember]
	//    public string StringValue
	//    {
	//        get { return stringValue; }
	//        set { stringValue = value; }
	//    }
	//} 
	#endregion
}

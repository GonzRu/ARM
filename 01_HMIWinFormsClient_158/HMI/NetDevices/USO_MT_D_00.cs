using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace NetCrzaDevices
{
/// <summary>
///     classUSO_MT_D_00
/// </summary>
public class USO_MT_D_00 : BMRZ
{
	public USO_MT_D_00(int nfc, int ndev, int size_memDev, string devname)
	{
		NFC			= nfc;
		NDev			= ndev;
		DeviceName	= devname;
		varDev		= new Hashtable();
		
		varDev.Add( 25, new Bit_FieldMT( 1, this, false ));	// (chanel0)
		varDev.Add( 26, new Bit_FieldMT( 1, this, false ));	// (chanel16)
		varDev.Add( 27, new Bit_FieldMT( 1, this, false ));	// (chanel32)
		varDev.Add( 31, new Bit_FieldMT( 1, this, false ));	// (chanel0)
		varDev.Add( 0, new BCD_FieldMT( 1, this, false ));	// (Reg_Block_ID)
		varDev.Add( 1, new Stringz_FieldMT( 16, this, false ));	// (Reg_Slave_ID)
		varDev.Add( 17, new BCD_FieldMT( 1, this, false ));	// (Reg_Version)
		varDev.Add( 19, new DateTime4_FieldMT( 4, this, false ));	// (Reg_Time)
		varDev.Add( 23, new UInt_FieldMT( 1, this, false ));	// (Reg_Status_Cnt)
		varDev.Add( 24, new Bit_FieldMT( 1, this, false ));	// (Reg_Status)
		varDev.Add( 30, new Bit_FieldMT( 1, this, false ));	// (Reg_Err)
		varDev.Add( 32, new Bit_FieldMT( 1, this, false ));	// (Reg_Err)
		varDev.Add( 139, new Int_FieldMT( 1, this, false ));	// (Reg_MaxMin_Asign)
		varDev.Add( 140, new Int_FieldMT( 1, this, false ));	// (Reg_MaxMin_Asign)
		varDev.Add( 141, new Int_FieldMT( 1, this, false ));	// (Reg_MaxMin_Asign)
		varDev.Add( 142, new Int_FieldMT( 1, this, false ));	// (Reg_MaxMin_Asign)
		varDev.Add( 143, new Int_FieldMT( 1, this, false ));	// (Reg_MaxMin_Asign)
		varDev.Add( 144, new Int_FieldMT( 1, this, false ));	// (Reg_Max_Time_BYKL)
		varDev.Add( 145, new DateTime4_FieldMT( 4, this, false ));	// (Reg_DateTime_ Asign1)
		varDev.Add( 149, new DateTime4_FieldMT( 4, this, false ));	// (Reg_DateTime_ Asign2)
		varDev.Add( 153, new DateTime4_FieldMT( 4, this, false ));	// (Reg_DateTime_ Asign3)
		varDev.Add( 157, new DateTime4_FieldMT( 4, this, false ));	// (Reg_DateTime_ Asign4)
		varDev.Add( 161, new DateTime4_FieldMT( 4, this, false ));	// (Reg_DateTime_ Asign5)
		varDev.Add( 165, new DateTime4_FieldMT( 4, this, false ));	// (Reg_DateTime_BYKL)
		varDev.Add( 169, new DateTime4_FieldMT( 4, this, false ));	// (Reg_DateTime_Sbros)
		varDev.Add( 60001, new Bit_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60002, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60003, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60004, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60005, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60006, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60007, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60008, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60009, new DateTimeUTC_FieldMT( 2, this, false ));	// (null)
		varDev.Add( 60011, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60012, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60013, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60014, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60015, new Stringz_FieldMT( 17, this, false ));	// (null)
		varDev.Add( 60050, new UDInt_FieldMT( 2, this, false ));	// (null)
		varDev.Add( 60052, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60053, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60054, new DateTimeUTC_FieldMT( 2, this, false ));	// (null)
		varDev.Add( 60056, new Int_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60057, new Int_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60058, new Stringz_FieldMT( 2, this, false ));	// (null)
		varDev.Add( 60060, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 60061, new UInt_FieldMT( 1, this, false ));	// (null)
		varDev.Add( 62006, new UInt_FieldMT( 1, this, false ));	// (Ustav0=Скорость 485=2,Скорость 485=)
		varDev.Add( 62007, new BCDPack_FieldMT( 1, this, false ));	// (Ustav1=Адрес 485=2,Адрес 485=)
		varDev.Add( 62008, new BCDPack_FieldMT( 1, this, false ));	// (Ustav10=Твоз Двых9=2,Твоз Двых9=)
		varDev.Add( 62009, new BCDPack_FieldMT( 1, this, false ));	// (Ustav11=Твоз Двых10=2,Твоз Двых10=)
		varDev.Add( 62010, new BCDPack_FieldMT( 1, this, false ));	// (Ustav12=Твоз Двых11=2,Твоз Двых11=)
		varDev.Add( 62011, new BCDPack_FieldMT( 1, this, false ));	// (Ustav13=Твоз Двых12=2,Твоз Двых12=)
		varDev.Add( 62012, new BCDPack_FieldMT( 1, this, false ));	// (Ustav14=Твоз Двых15=2,Твоз Двых15=)
		varDev.Add( 62013, new BCDPack_FieldMT( 1, this, false ));	// (Ustav15=Твоз Двых16=2,Твоз Двых16=)
		varDev.Add( 62014, new BCDPack_FieldMT( 1, this, false ));	// (Ustav16=Тсб=2,Тсб=)
		varDev.Add( 62015, new BCDPack_FieldMT( 1, this, false ));	// (Ustav2=Твоз Двых1=2,Твоз Двых1=)
		varDev.Add( 62016, new BCDPack_FieldMT( 1, this, false ));	// (Ustav3=Твоз Двых2=2,Твоз Двых2=)
		varDev.Add( 62017, new BCDPack_FieldMT( 1, this, false ));	// (Ustav4=Твоз Двых3=2,Твоз Двых3=)
		varDev.Add( 62018, new BCDPack_FieldMT( 1, this, false ));	// (Ustav5=Твоз Двых4=2,Твоз Двых4=)
		varDev.Add( 62019, new BCDPack_FieldMT( 1, this, false ));	// (Ustav6=Твоз Двых5=2,Твоз Двых5=)
		varDev.Add( 62020, new BCDPack_FieldMT( 1, this, false ));	// (Ustav7=Твоз Двых6=2,Твоз Двых6=)
		varDev.Add( 62021, new BCDPack_FieldMT( 1, this, false ));	// (Ustav8=Твоз Двых7=2,Твоз Двых7=)
		varDev.Add( 62022, new BCDPack_FieldMT( 1, this, false ));	// (Ustav9=Твоз Двых8=2,Твоз Двых8=)
		varDev.Add( 62023, new Bit_FieldMT( 1, this, false ));	// (byte0_0)
		varDev.Add( 62024, new Bit_FieldMT( 1, this, false ));	// (byte2_0)
		varDev.Add( 62002, new DateTime4_FieldMT( 4, this, false ));	// ()
		ConfigureMF(ref varDev, "");
	}
	public override string	ToString()
	{
		return "USO_MT_D_00";
	}
	public override void	ConfigureMF(ref Hashtable vD, string modfileName)
	{}
}
}


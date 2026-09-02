using System;
using System.Collections;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace M1.Core;

public class M1ProductCode
{
	private Hashtable licenseTable = new Hashtable();

	private M1DataDictionary dataDictionary;

	private DateTime? _ExpiryDate;

	private string baseModules = ",qm,ap,hd,ab,wg,pm,om,gl,pc,im,lo,dm,jm,vs,pa,rq,dc,pr,mc,ar,mw,ps,sm,we,qa,  ,  ,  ,  ,  ,";

	private string purchasedModules = string.Empty;

	private string string0 = "kv6HSamyAMYiw9N?n1FUgxDTh0IZq8Rl5QoBXtKfCc4b7eJsWGzpd*juEVOP3r2L";

	private string string1 = "flrx39FLRXbipw4BIPWckt1AJS?hs2DNYgu6HUev8OaqCTm5QnE*0Z7jMGzKdVyo";

	private string string2 = "hpx5DLT*ir09IR?jt3EOYgu6HUdq4JWkyCSfzGZoAVnBawNmKlMvX81ecs7QP2bF";

	private string string3 = "GNU18fmt?FOW4cks*IR0ajuBLX7ivDQ3grET9oASbqJ2nH5wPhKeMpZCyxzYlV6d";

	private DateTime baseDate = new DateTime(2000, 5, 31);

	private DateTime baseDate2 = new DateTime(2009, 5, 31);

	private DateTime customBaseDate = new DateTime(2009, 7, 31);

	private object[,] customModules;

	private short customModulesCount;

	private string lastLoadedProductCode = string.Empty;

	private AppContext currentContext;

	public short MaxUsers { get; private set; }

	public DateTime? ExpiryDate
	{
		get
		{
			return _ExpiryDate;
		}
		private set
		{
			_ExpiryDate = value;
		}
	}

	public int SerialNumber { get; private set; }

	public string LastLoadedProductCode
	{
		get
		{
			return lastLoadedProductCode;
		}
		private set
		{
			lastLoadedProductCode = value;
		}
	}

	public string AllModules => baseModules;

	public M1ProductCode(M1DataDictionary m1DataDictionary, AppContext context)
	{
		dataDictionary = m1DataDictionary;
		currentContext = context;
	}

	private bool loadLicenses()
	{
		XmlReader xmlReader = XmlReader.Create(currentContext.Client.Location + "Tools\\OCX\\Licenses.xml");
		while (xmlReader.Read())
		{
			if (xmlReader.Name == "License")
			{
				licenseTable.Add(xmlReader.GetAttribute("ProgID").ToLower(), xmlReader.GetAttribute("Key"));
			}
		}
		xmlReader.Close();
		return true;
	}

	public string GetDDProductCode(string dataDictionaryName)
	{
		object obj = currentContext.DDServerManager.ExecuteScalar(null, null, dataDictionaryName, "Select ddProductCode From DDInfo");
		if (obj != null)
		{
			return obj.ToString();
		}
		return string.Empty;
	}

	public string GetDDCustomProductCodes(string dataDictionaryName)
	{
		object obj = currentContext.DDServerManager.ExecuteScalar(null, null, dataDictionaryName, "Select ddCustomProductCodes From DDInfo");
		if (obj != null)
		{
			return obj.ToString();
		}
		return string.Empty;
	}

	public bool LoadCustomProductIDFromIni()
	{
		string text = GetDDCustomProductCodes(dataDictionary.ID).Trim();
		if (text.Length > 0)
		{
			string[] array = text.Split('|');
			foreach (string text2 in array)
			{
				LoadProductCodeCustom(text2.ToString());
			}
		}
		return true;
	}

	public void LoadProductCode(string productCode)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		int num12 = 0;
		int num13 = 0;
		int num14 = 0;
		int num15 = 0;
		int num16 = 0;
		int num17 = 0;
		int num18 = 0;
		productCode = productCode.Trim();
		if (productCode.Length == 0)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Product Code.");
		}
		if (productCode.Length != 12 && productCode.Length != 15 && productCode.Length != 16)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Product Code.");
		}
		num = convertCharToNum(productCode.Substring(0, 1), 0);
		num18 = 0;
		if ((num & 0x10) == 16)
		{
			num -= 16;
			num18 = 1;
		}
		if ((num & 0x20) == 32)
		{
			num -= 32;
			num18 += 2;
		}
		num14 = convertCharToNum(productCode.Substring(1, 1), num18 + 1);
		num3 = convertCharToNum(productCode.Substring(2, 1), num18 + 2);
		num12 = convertCharToNum(productCode.Substring(3, 1), num18 + 3);
		num4 = convertCharToNum(productCode.Substring(4, 1), num18 + 4);
		num8 = convertCharToNum(productCode.Substring(5, 1), num18 + 5);
		num15 = convertCharToNum(productCode.Substring(6, 1), num18 + 6);
		num5 = convertCharToNum(productCode.Substring(7, 1), num18 + 7);
		num13 = convertCharToNum(productCode.Substring(8, 1), num18 + 8);
		num6 = convertCharToNum(productCode.Substring(9, 1), num18 + 9);
		num16 = convertCharToNum(productCode.Substring(10, 1), num18 + 10);
		num2 = convertCharToNum(productCode.Substring(11, 1), num18 + 11);
		num7 = convertCharToNum(productCode.Substring(12, 1), num18 + 12);
		num9 = convertCharToNum(productCode.Substring(13, 1), num18 + 13);
		num10 = convertCharToNum(productCode.Substring(14, 1), num18 + 14);
		if (productCode.Length == 16)
		{
			num11 = convertCharToNum(productCode.Substring(15, 1), num18 + 15);
		}
		num17 = num3 + num12 + num4 + num8 + num9 + num5 + num13 + num6 + num7 + num14 + num15 + num16 + num10 + num11;
		if ((num2 & 0x20) == 32)
		{
			num2 -= 32;
			num += 16;
		}
		if (num * 32 + num2 != num17)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Product Code.");
		}
		if ((num12 & 0x20) == 32)
		{
			num12 -= 32;
			num8 += 64;
		}
		if ((num13 & 0x20) == 32)
		{
			num13 -= 32;
			num8 += 128;
		}
		MaxUsers = (short)(num12 * 32 + num13);
		if (num8 == 0)
		{
			ExpiryDate = null;
		}
		else
		{
			ExpiryDate = GetBaseDate(productCode.Length).AddMonths(num8);
			if (num9 > 0)
			{
				ExpiryDate = ExpiryDate.Value.AddDays(-num9);
			}
		}
		if (checkBit(1, num3))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(1);
		}
		if (checkBit(2, num3))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(2);
		}
		if (checkBit(3, num3))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(3);
		}
		if (checkBit(4, num3))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(4);
		}
		if (checkBit(5, num3))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(5);
		}
		if (checkBit(6, num3))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(6);
		}
		if (checkBit(1, num4))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(7);
		}
		if (checkBit(2, num4))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(8);
		}
		if (checkBit(3, num4))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(9);
		}
		if (checkBit(4, num4))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(10);
		}
		if (checkBit(5, num4))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(11);
		}
		if (checkBit(6, num4))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(12);
		}
		if (checkBit(1, num5))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(13);
		}
		if (checkBit(2, num5))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(14);
		}
		if (checkBit(3, num5))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(15);
		}
		if (checkBit(4, num5))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(16);
		}
		if (checkBit(5, num5))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(17);
		}
		if (checkBit(6, num5))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(18);
		}
		if (checkBit(1, num6))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(19);
		}
		if (checkBit(2, num6))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(20);
		}
		if (checkBit(3, num6))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(21);
		}
		if (checkBit(4, num6))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(22);
		}
		if (checkBit(5, num6))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(23);
		}
		if (checkBit(6, num6))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(24);
		}
		if (checkBit(1, num7))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(25);
		}
		if (checkBit(2, num7))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(26);
		}
		if (checkBit(3, num7))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(27);
		}
		if (checkBit(4, num7))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(28);
		}
		if (checkBit(5, num7))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(29);
		}
		if (checkBit(6, num7))
		{
			purchasedModules = purchasedModules + "," + getModuleFromNumber(30);
		}
		purchasedModules += ",";
		if (checkBit(1, num14))
		{
			num15 = setBit(7, num15, set: true);
		}
		if (checkBit(2, num14))
		{
			num15 = setBit(8, num15, set: true);
		}
		if (checkBit(3, num14))
		{
			num15 = setBit(9, num15, set: true);
		}
		SerialNumber = num15 * 63 + num16;
		if (SerialNumber > 0)
		{
			SerialNumber += 10000;
		}
		lastLoadedProductCode = productCode;
	}

	public string GetSecurityString(string securityCheck)
	{
		string empty = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		int num12 = 0;
		int num13 = 0;
		int num14 = 0;
		int num15 = 0;
		int num16 = 0;
		int num17 = 0;
		Random random = new Random();
		if (securityCheck != "M1BGID")
		{
			return string.Empty;
		}
		if (IsModulePurchased(getModuleFromNumber(1), null))
		{
			num3 = setBit(1, num3, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(2), null))
		{
			num3 = setBit(2, num3, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(3), null))
		{
			num3 = setBit(3, num3, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(4), null))
		{
			num3 = setBit(4, num3, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(5), null))
		{
			num3 = setBit(5, num3, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(6), null))
		{
			num3 = setBit(6, num3, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(7), null))
		{
			num4 = setBit(1, num4, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(8), null))
		{
			num4 = setBit(2, num4, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(9), null))
		{
			num4 = setBit(3, num4, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(10), null))
		{
			num4 = setBit(4, num4, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(11), null))
		{
			num4 = setBit(5, num4, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(12), null))
		{
			num4 = setBit(6, num4, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(13), null))
		{
			num5 = setBit(1, num5, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(14), null))
		{
			num5 = setBit(2, num5, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(15), null))
		{
			num5 = setBit(3, num5, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(16), null))
		{
			num5 = setBit(4, num5, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(17), null))
		{
			num5 = setBit(5, num5, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(18), null))
		{
			num5 = setBit(6, num5, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(19), null))
		{
			num6 = setBit(1, num6, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(20), null))
		{
			num6 = setBit(2, num6, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(21), null))
		{
			num6 = setBit(3, num6, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(22), null))
		{
			num6 = setBit(4, num6, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(23), null))
		{
			num6 = setBit(5, num6, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(24), null))
		{
			num6 = setBit(6, num6, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(25), null))
		{
			num7 = setBit(1, num7, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(26), null))
		{
			num7 = setBit(2, num7, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(27), null))
		{
			num7 = setBit(3, num7, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(28), null))
		{
			num7 = setBit(4, num7, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(29), null))
		{
			num7 = setBit(5, num7, set: true);
		}
		if (IsModulePurchased(getModuleFromNumber(30), null))
		{
			num7 = setBit(6, num7, set: true);
		}
		num12 = MaxUsers / 32;
		num13 = MaxUsers - num12 * 32;
		num9 = 0;
		if (!ExpiryDate.HasValue)
		{
			num8 = 0;
		}
		else
		{
			num8 = SqlMethods.DateDiffMonth(GetBaseDate(16), ExpiryDate.Value);
			if (num8 < 0)
			{
				num8 = 0;
			}
			num9 = SqlMethods.DateDiffDay(ExpiryDate.Value, GetBaseDate(16).AddMonths(num8));
			if (num9 < 0)
			{
				num9 = 0;
			}
			if (num9 > 31)
			{
				num9 = 31;
			}
			if ((num8 & 0x80) == 128)
			{
				num8 -= 128;
				num13 += 32;
			}
			if ((num8 & 0x40) == 64)
			{
				num12 += 32;
				num8 -= 64;
			}
		}
		if (SerialNumber > 10000)
		{
			long num18 = SerialNumber - 10000;
			num15 = (int)(num18 / 63);
			num16 = (int)num18 % 63;
			if (num15 > 63)
			{
				if (num15 >= 512)
				{
					throw new M1Exception("Invalid serial number.");
				}
				if (checkBit(7, num15))
				{
					num14 = setBit(1, num14, set: true);
					num15 = setBit(7, num15, set: false);
				}
				if (checkBit(8, num15))
				{
					num14 = setBit(2, num14, set: true);
					num15 = setBit(8, num15, set: false);
				}
				if (checkBit(9, num15))
				{
					num14 = setBit(3, num14, set: true);
					num15 = setBit(9, num15, set: false);
				}
			}
		}
		int num19 = num3 + num12 + num4 + num8 + num9 + num5 + num13 + num6 + num7 + num14 + num15 + num16 + num10 + num11;
		num = num19 / 32;
		num2 = num19 - num * 32;
		if ((num & 0x10) == 16)
		{
			num2 += 32;
			num -= 16;
		}
		num17 = (int)(4.0 * random.NextDouble());
		if (num17 != 0)
		{
			if (num17 == 1 || num17 == 3)
			{
				num += 16;
			}
			if (num17 == 2 || num17 == 3)
			{
				num += 32;
			}
		}
		empty = convertNumToChar(num14, num17 + 1) + convertNumToChar(num3, num17 + 2) + convertNumToChar(num12, num17 + 3) + convertNumToChar(num4, num17 + 4) + convertNumToChar(num8, num17 + 5) + convertNumToChar(num15, num17 + 6) + convertNumToChar(num5, num17 + 7) + convertNumToChar(num13, num17 + 8) + convertNumToChar(num6, num17 + 9) + convertNumToChar(num16, num17 + 10);
		empty = convertNumToChar(num, 0) + empty;
		return empty + convertNumToChar(num2, num17 + 11) + convertNumToChar(num7, num17 + 12) + convertNumToChar(num9, num17 + 13) + convertNumToChar(num10, num17 + 14) + (ExpiryDate.HasValue ? convertNumToChar(num11, num17 + 15) : string.Empty);
	}

	private DateTime GetBaseDate(int length)
	{
		if (length <= 15)
		{
			return baseDate;
		}
		return baseDate2;
	}

	public bool LoadProductCodeCustom(string productCode)
	{
		int customID = 0;
		object customExpiryDate = null;
		long customSerialNumber = 0L;
		bool result = false;
		int MaxUsers = 0;
		if ((productCode.Length <= 8) ? TestCustomSecurityString(productCode, ref customID, ref customExpiryDate, ref customSerialNumber) : TestCustomSecurityString(productCode, ref customID, ref customExpiryDate, ref customSerialNumber, ref MaxUsers))
		{
			if (customSerialNumber != SerialNumber)
			{
				throw new M1LoginProductCodeInvalidException("Invalid product Code - the serial number in the product code does not match the serial number of this installation of M1.");
			}
			int customModuleRow = getCustomModuleRow(customID);
			if (customModuleRow > -1)
			{
				customModules[customModuleRow, 1] = customExpiryDate;
			}
			else
			{
				customModulesCount++;
				object[,] destinationArray = new object[customModulesCount, 4];
				if (customModules != null)
				{
					Array.Copy(customModules, destinationArray, customModules.Length);
				}
				customModules = destinationArray;
				customModules[customModulesCount - 1, 0] = customID;
				customModules[customModulesCount - 1, 1] = customExpiryDate;
				if (productCode.Length > 8)
				{
					customModules[customModulesCount - 1, 2] = MaxUsers;
				}
				else
				{
					customModules[customModulesCount - 1, 2] = MaxUsers;
				}
				customModules[customModulesCount - 1, 3] = productCode;
			}
			result = true;
		}
		return result;
	}

	public bool LoadProductCodeCustomMaxUsers(string productCode)
	{
		int customID = 0;
		object customExpiryDate = null;
		long customSerialNumber = 0L;
		bool result = false;
		int MaxUsers = 0;
		if (TestCustomSecurityString(productCode, ref customID, ref customExpiryDate, ref customSerialNumber, ref MaxUsers))
		{
			if (customSerialNumber != SerialNumber)
			{
				throw new M1LoginProductCodeInvalidException("Invalid product Code - the serial number in the product code does not match the serial number of this installation of M1.");
			}
			int customModuleRow = getCustomModuleRow(customID);
			if (customModuleRow > -1)
			{
				customModules[customModuleRow, 1] = customExpiryDate;
			}
			else
			{
				customModulesCount++;
				object[,] destinationArray = new object[customModulesCount, 4];
				if (customModules != null)
				{
					Array.Copy(customModules, destinationArray, customModules.Length);
				}
				customModules = destinationArray;
				customModules[customModulesCount - 1, 0] = customID;
				customModules[customModulesCount - 1, 1] = customExpiryDate;
				customModules[customModulesCount - 1, 2] = MaxUsers;
				customModules[customModulesCount - 1, 3] = productCode;
			}
			result = true;
		}
		return result;
	}

	private int getCustomModuleRow(int customID)
	{
		if (customModules != null)
		{
			for (int i = 0; i < customModulesCount; i++)
			{
				if (customID == (int)customModules[i, 0])
				{
					return i;
				}
			}
		}
		return -1;
	}

	public bool TestCustomSecurityString(string productCode, ref int customID, ref object customExpiryDate, ref long customSerialNumber)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		productCode = productCode.Trim();
		if (productCode.Length == 0)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Custom Product Code.");
		}
		if (productCode.Length != 8)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Custom Product Code.");
		}
		num = convertCharToNum(productCode.Substring(0, 1), 0);
		num9 = 0;
		if ((num & 0x10) == 16)
		{
			num -= 16;
			num9 = 1;
		}
		if ((num & 0x20) == 32)
		{
			num -= 32;
			num9 += 2;
		}
		num5 = convertCharToNum(productCode.Substring(1, 1), num9 + 1);
		num3 = convertCharToNum(productCode.Substring(2, 1), num9 + 2);
		num6 = convertCharToNum(productCode.Substring(3, 1), num9 + 3);
		customID = convertCharToNum(productCode.Substring(4, 1), num9 + 4);
		num7 = convertCharToNum(productCode.Substring(5, 1), num9 + 5);
		num2 = convertCharToNum(productCode.Substring(6, 1), num9 + 6);
		num4 = convertCharToNum(productCode.Substring(7, 1), num9 + 7);
		num8 = num3 + num4 + num5 + num6 + num7 + customID;
		if ((num2 & 0x20) == 32)
		{
			num2 -= 32;
			num += 16;
		}
		if (num * 32 + num2 != num8)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Custom Product Code.");
		}
		num3 += 64;
		if ((num4 & 0x40) == 64)
		{
			num3 += 128;
			num4 -= 64;
		}
		if (num3 == 0)
		{
			customExpiryDate = null;
		}
		else
		{
			customExpiryDate = customBaseDate.AddMonths(num3);
			if (num4 > 0)
			{
				customExpiryDate = ((DateTime)customExpiryDate).AddDays(-num4);
			}
		}
		if (checkBit(1, num5))
		{
			num6 = setBit(7, num6, set: true);
		}
		if (checkBit(2, num5))
		{
			num6 = setBit(8, num6, set: true);
		}
		if (checkBit(3, num5))
		{
			num6 = setBit(9, num6, set: true);
		}
		if (checkBit(4, num5))
		{
			customID = setBit(7, customID, set: true);
		}
		if (checkBit(5, num5))
		{
			customID = setBit(8, customID, set: true);
		}
		if (checkBit(6, num5))
		{
			customID = setBit(9, customID, set: true);
		}
		customSerialNumber = num6 * 63 + num7;
		if ((int)customSerialNumber > 0)
		{
			customSerialNumber = (int)customSerialNumber + 10000;
		}
		return true;
	}

	public bool TestCustomSecurityString(string productCode, ref int customID, ref object customExpiryDate, ref long customSerialNumber, ref int MaxUsers)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		productCode = productCode.Trim();
		if (productCode.Length == 0)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Custom Product Code.");
		}
		if (productCode.Length != 10)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Custom Product Code.");
		}
		num = convertCharToNum(productCode.Substring(0, 1), 0);
		num9 = 0;
		if ((num & 0x10) == 16)
		{
			num -= 16;
			num9 = 1;
		}
		if ((num & 0x20) == 32)
		{
			num -= 32;
			num9 += 2;
		}
		num5 = convertCharToNum(productCode.Substring(1, 1), num9 + 1);
		num3 = convertCharToNum(productCode.Substring(2, 1), num9 + 2);
		num6 = convertCharToNum(productCode.Substring(3, 1), num9 + 3);
		customID = convertCharToNum(productCode.Substring(4, 1), num9 + 4);
		num7 = convertCharToNum(productCode.Substring(5, 1), num9 + 5);
		num2 = convertCharToNum(productCode.Substring(6, 1), num9 + 6);
		num4 = convertCharToNum(productCode.Substring(7, 1), num9 + 7);
		num10 = convertCharToNum(productCode.Substring(8, 1), num9 + 8);
		num11 = convertCharToNum(productCode.Substring(9, 1), num9 + 9);
		num8 = num3 + num4 + num5 + num6 + num7 + customID + num10 + num11;
		if ((num2 & 0x20) == 32)
		{
			num2 -= 32;
			num += 16;
		}
		if (num * 32 + num2 != num8)
		{
			throw new M1LoginProductCodeInvalidException("Invalid Custom Product Code.");
		}
		if ((num10 & 0x20) == 32)
		{
			num10 -= 32;
			num3 += 64;
		}
		if ((num11 & 0x20) == 32)
		{
			num11 -= 32;
			num3 += 128;
		}
		MaxUsers = (short)(num10 * 32 + num11);
		if (num3 == 0)
		{
			customExpiryDate = null;
		}
		else
		{
			customExpiryDate = customBaseDate.AddMonths(num3);
			if (num4 > 0)
			{
				customExpiryDate = ((DateTime)customExpiryDate).AddDays(-num4);
			}
		}
		if (checkBit(1, num5))
		{
			num6 = setBit(7, num6, set: true);
		}
		if (checkBit(2, num5))
		{
			num6 = setBit(8, num6, set: true);
		}
		if (checkBit(3, num5))
		{
			num6 = setBit(9, num6, set: true);
		}
		if (checkBit(4, num5))
		{
			customID = setBit(7, customID, set: true);
		}
		if (checkBit(5, num5))
		{
			customID = setBit(8, customID, set: true);
		}
		if (checkBit(6, num5))
		{
			customID = setBit(9, customID, set: true);
		}
		customSerialNumber = num6 * 63 + num7;
		if ((int)customSerialNumber > 0)
		{
			customSerialNumber = (int)customSerialNumber + 10000;
		}
		return true;
	}

	public bool IsModulePurchased(string module, M1Database database)
	{
		if (module.Equals("PL", StringComparison.CurrentCultureIgnoreCase) || module.Equals("PG", StringComparison.CurrentCultureIgnoreCase))
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(module))
		{
			return true;
		}
		bool flag = false;
		DDModule dDModule = dataDictionary.Modules.FirstOrDefault((DDModule item) => item.ModuleID.Equals(module, StringComparison.CurrentCultureIgnoreCase));
		if (dDModule != null)
		{
			if (dDModule.SecurityModulesArray.Length != 0)
			{
				string[] securityModulesArray = dDModule.SecurityModulesArray;
				foreach (string text in securityModulesArray)
				{
					if (purchasedModules.IndexOf("," + text + ",", StringComparison.CurrentCultureIgnoreCase) != -1)
					{
						flag = true;
						break;
					}
				}
			}
			else if (dDModule.Virtual || purchasedModules.IndexOf("," + dDModule.ModuleID + ",", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				flag = true;
			}
			if (flag && !string.IsNullOrWhiteSpace(dDModule.PropertiesFieldName) && database != null)
			{
				DataRow dataRow = database.Props("DatasetProperties");
				if (!dataRow.Table.Columns.Contains(dDModule.PropertiesFieldName) && !string.IsNullOrWhiteSpace(dDModule.PropertiesTable))
				{
					dataRow = database.Props(dDModule.ModuleID);
				}
				if (!dataRow.Field<bool>(dDModule.PropertiesFieldName).Equals(dDModule.PropertiesFieldValue))
				{
					flag = false;
				}
			}
		}
		else
		{
			flag = purchasedModules.IndexOf("," + module + ",", StringComparison.CurrentCultureIgnoreCase) != -1;
		}
		return flag;
	}

	public bool IsCustomModulePurchased(string customModule)
	{
		if (customModules != null)
		{
			int upperBound = customModules.GetUpperBound(0);
			for (int i = customModules.GetLowerBound(0); i <= upperBound; i++)
			{
				if (customModules[i, 0].ToString().Equals(customModule))
				{
					return true;
				}
				if (customModule.Equals("9") && (customModules[i, 0].ToString().Equals("15") || customModules[i, 0].ToString().Equals("9")))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsCustomModulePurchased(int customModule)
	{
		return IsCustomModulePurchased(customModule.ToString());
	}

	public bool HasCustomProductCodeExpired(string customModule)
	{
		if (customModules == null)
		{
			return false;
		}
		int upperBound = customModules.GetUpperBound(0);
		for (int i = customModules.GetLowerBound(0); i <= upperBound; i++)
		{
			if (customModule.Equals("9") && (customModules[i, 0].ToString().Equals("15") || customModules[i, 0].ToString().Equals("9")))
			{
				if (customModules[i, 1] != null)
				{
					return DateTime.Today > (DateTime)customModules[i, 1];
				}
				return false;
			}
			if (customModules[i, 0].ToString().Equals(customModule, StringComparison.CurrentCultureIgnoreCase))
			{
				if (customModules[i, 1] != null)
				{
					return DateTime.Today > (DateTime)customModules[i, 1];
				}
				return false;
			}
		}
		return false;
	}

	public DateTime GetCustomProductCodeExpiryDate(int customModule)
	{
		if (customModules == null)
		{
			return DateTime.MaxValue;
		}
		int upperBound = customModules.GetUpperBound(0);
		for (int i = customModules.GetLowerBound(0); i <= upperBound; i++)
		{
			if (customModules[i, 0].ToString().Equals(customModule.ToString(), StringComparison.CurrentCultureIgnoreCase) && customModules[i, 1] != null)
			{
				return (DateTime)customModules[i, 1];
			}
		}
		return DateTime.MaxValue;
	}

	public bool HasCustomProductCodeUserLimits(string customModule, ref int maxUsers)
	{
		int upperBound = customModules.GetUpperBound(0);
		maxUsers = 0;
		for (int i = customModules.GetLowerBound(0); i <= upperBound; i++)
		{
			if (customModules[i, 0].ToString().Equals(customModule, StringComparison.CurrentCultureIgnoreCase))
			{
				if (customModules[i, 2] != null && (int)customModules[i, 2] > 0)
				{
					maxUsers = (int)customModules[i, 2];
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private int convertCharToNum(string text, int type)
	{
		int num = 0;
		type %= 4;
		num = type switch
		{
			0 => string0.IndexOf(text, 0), 
			1 => string1.IndexOf(text, 0), 
			2 => string2.IndexOf(text, 0), 
			3 => string3.IndexOf(text, 0), 
			_ => throw new M1Exception("Unknown type '" + type + "' in ConvertCharToNum."), 
		};
		if (num >= 0)
		{
			return num;
		}
		return 0;
	}

	private string convertNumToChar(int number, int type)
	{
		string empty = string.Empty;
		if (number >= 0 && number <= 63)
		{
			type %= 4;
			return type switch
			{
				0 => string0.Substring(number, 1), 
				1 => string1.Substring(number, 1), 
				2 => string2.Substring(number, 1), 
				3 => string3.Substring(number, 1), 
				_ => throw new M1Exception("Unknown type '" + type + "' in ConvertNumToChar."), 
			};
		}
		throw new M1Exception("Invalid number '" + number + "' in ConvertNumToChar.");
	}

	private bool checkBit(int bit, int value)
	{
		bit--;
		if (((int)Math.Pow(2.0, bit) & value) != 0)
		{
			return true;
		}
		return false;
	}

	private string getModuleFromNumber(int module)
	{
		string result = string.Empty;
		if (module > 0 && module < 31)
		{
			result = baseModules.Substring((module - 1) * 3 + 1, 2);
		}
		return result;
	}

	private int setBit(int bit, int value, bool set)
	{
		int num = 0;
		bit--;
		if (((int)Math.Pow(2.0, bit) & value) == 0)
		{
			if (set)
			{
				return value + (int)Math.Pow(2.0, bit);
			}
			return value;
		}
		if (set)
		{
			return value;
		}
		return value - (int)Math.Pow(2.0, bit);
	}

	public bool IsProductCodeExpired()
	{
		DateTime? expiryDate = ExpiryDate;
		if (expiryDate.HasValue)
		{
			string text = currentContext.Server.IniSettings.Get("KeyInfo", string.Empty).Trim();
			DateTime today = DateTime.Today;
			DateTime? dateTime = expiryDate;
			if (today > dateTime || text.Length > 0)
			{
				return true;
			}
		}
		return false;
	}

	public int GetCustomModules(ref object[,] list)
	{
		if (customModulesCount >= 0)
		{
			list = new object[customModulesCount, 4];
			if (customModules != null)
			{
				Array.Copy(customModules, list, customModules.Length);
			}
		}
		return customModulesCount;
	}

	public string GetCustomModuleText(M1DataDictionary dataDictionary, int customModule)
	{
		string text = string.Empty;
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select dcCaption From DDCustomModules Where dcCustomID = @Module");
		sqlCommand.Parameters.Add(new SqlParameter("@Module", SqlDbType.Int)).Value = customModule;
		DataTable dataTable = dataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			text = dataTable.Rows[0].Field<string>("dcCaption").Trim();
		}
		dataTable = null;
		if (text.Length == 0)
		{
			text = "Custom Module " + customModule.ToString().Trim();
		}
		return text;
	}

	public string GetAllCustomSecurityStrings(string securityCheck)
	{
		string text = string.Empty;
		string empty = string.Empty;
		if (customModules != null)
		{
			for (int i = 0; i < customModulesCount; i++)
			{
				if ((int)customModules[i, 0] != 0)
				{
					empty = ((customModules[i, 2] == null) ? GetCustomSecurityString("M1BGID", (int)customModules[i, 0], (DateTime?)customModules[i, 1], SerialNumber) : GetCustomSecurityString("M1BGID", (int)customModules[i, 0], (DateTime?)customModules[i, 1], SerialNumber, (int)customModules[i, 2]));
					text = ((!string.IsNullOrEmpty(text)) ? (text + "|" + empty) : empty);
				}
			}
		}
		return text;
	}

	public string GetCustomSecurityString(string securityCheck, int customID, DateTime? customExpiryDate, long tempSerialNumber)
	{
		string empty = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		Random random = new Random();
		if (securityCheck != "M1BGID")
		{
			return string.Empty;
		}
		num4 = 0;
		if (!customExpiryDate.HasValue)
		{
			num3 = 0;
		}
		else
		{
			num3 = SqlMethods.DateDiffMonth(customBaseDate, customExpiryDate.Value);
			if (num3 < 0)
			{
				num3 = 0;
			}
			num4 = SqlMethods.DateDiffDay(customExpiryDate.Value, customBaseDate.AddMonths(num3));
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num4 > 31)
			{
				num4 = 31;
			}
			if ((num3 & 0x80) == 128)
			{
				num3 -= 128;
				num4 += 64;
			}
			if ((num3 & 0x40) == 64)
			{
				num3 -= 64;
			}
		}
		if (tempSerialNumber > 10000)
		{
			tempSerialNumber -= 10000;
			num6 = (int)(tempSerialNumber / 63);
			num7 = (int)tempSerialNumber % 63;
			if (num6 > 63)
			{
				if (num6 >= 512)
				{
					throw new M1Exception("Invalid serial number.");
				}
				if (checkBit(7, num6))
				{
					num5 = setBit(1, num5, set: true);
					num6 = setBit(7, num6, set: false);
				}
				if (checkBit(8, num6))
				{
					num5 = setBit(2, num5, set: true);
					num6 = setBit(8, num6, set: false);
				}
				if (checkBit(9, num6))
				{
					num5 = setBit(3, num5, set: true);
					num6 = setBit(9, num6, set: false);
				}
			}
		}
		if (customID > 63)
		{
			if (checkBit(7, customID))
			{
				num5 = setBit(4, num5, set: true);
				customID = setBit(7, customID, set: false);
			}
			if (checkBit(8, customID))
			{
				num5 = setBit(5, num5, set: true);
				customID = setBit(8, customID, set: false);
			}
			if (checkBit(9, customID))
			{
				num5 = setBit(6, num5, set: true);
				customID = setBit(9, customID, set: false);
			}
		}
		int num9 = num3 + num4 + num5 + num6 + num7 + customID;
		num = num9 / 32;
		num2 = num9 - num * 32;
		if ((num & 0x10) == 16)
		{
			num2 += 32;
			num -= 16;
		}
		num8 = (int)(4.0 * random.NextDouble());
		if (num8 != 0)
		{
			if (num8 == 1 || num8 == 3)
			{
				num += 16;
			}
			if (num8 == 2 || num8 == 3)
			{
				num += 32;
			}
		}
		empty = convertNumToChar(num5, num8 + 1) + convertNumToChar(num3, num8 + 2) + convertNumToChar(num6, num8 + 3) + convertNumToChar(customID, num8 + 4) + convertNumToChar(num7, num8 + 5);
		empty = convertNumToChar(num, 0) + empty;
		return empty + convertNumToChar(num2, num8 + 6) + convertNumToChar(num4, num8 + 7);
	}

	public string GetCustomSecurityString(string securityCheck, int customID, DateTime? customExpiryDate, long tempSerialNumber, int maxUsers)
	{
		string empty = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		Random random = new Random();
		int num9 = 0;
		int num10 = 0;
		if (securityCheck != "M1BGID")
		{
			return string.Empty;
		}
		num9 = maxUsers / 32;
		num10 = maxUsers - num9 * 32;
		num4 = 0;
		if (!customExpiryDate.HasValue)
		{
			num3 = 0;
		}
		else
		{
			num3 = SqlMethods.DateDiffMonth(customBaseDate, customExpiryDate.Value);
			if (num3 < 0)
			{
				num3 = 0;
			}
			num4 = SqlMethods.DateDiffDay(customExpiryDate.Value, customBaseDate.AddMonths(num3));
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num4 > 31)
			{
				num4 = 31;
			}
			if ((num3 & 0x80) == 128)
			{
				num3 -= 128;
				num10 += 32;
			}
			if ((num3 & 0x40) == 64)
			{
				num9 += 32;
				num3 -= 64;
			}
		}
		if (tempSerialNumber > 10000)
		{
			tempSerialNumber -= 10000;
			num6 = (int)(tempSerialNumber / 63);
			num7 = (int)tempSerialNumber % 63;
			if (num6 > 63)
			{
				if (num6 >= 512)
				{
					throw new M1Exception("Invalid serial number.");
				}
				if (checkBit(7, num6))
				{
					num5 = setBit(1, num5, set: true);
					num6 = setBit(7, num6, set: false);
				}
				if (checkBit(8, num6))
				{
					num5 = setBit(2, num5, set: true);
					num6 = setBit(8, num6, set: false);
				}
				if (checkBit(9, num6))
				{
					num5 = setBit(3, num5, set: true);
					num6 = setBit(9, num6, set: false);
				}
			}
		}
		if (customID > 63)
		{
			if (checkBit(7, customID))
			{
				num5 = setBit(4, num5, set: true);
				customID = setBit(7, customID, set: false);
			}
			if (checkBit(8, customID))
			{
				num5 = setBit(5, num5, set: true);
				customID = setBit(8, customID, set: false);
			}
			if (checkBit(9, customID))
			{
				num5 = setBit(6, num5, set: true);
				customID = setBit(9, customID, set: false);
			}
		}
		int num11 = num3 + num4 + num5 + num6 + num7 + customID + num9 + num10;
		num = num11 / 32;
		num2 = num11 - num * 32;
		if ((num & 0x10) == 16)
		{
			num2 += 32;
			num -= 16;
		}
		num8 = (int)(4.0 * random.NextDouble());
		if (num8 != 0)
		{
			if (num8 == 1 || num8 == 3)
			{
				num += 16;
			}
			if (num8 == 2 || num8 == 3)
			{
				num += 32;
			}
		}
		empty = convertNumToChar(num5, num8 + 1) + convertNumToChar(num3, num8 + 2) + convertNumToChar(num6, num8 + 3) + convertNumToChar(customID, num8 + 4) + convertNumToChar(num7, num8 + 5);
		empty = convertNumToChar(num, 0) + empty;
		return empty + convertNumToChar(num2, num8 + 6) + convertNumToChar(num4, num8 + 7) + convertNumToChar(num9, num8 + 8) + convertNumToChar(num10, num8 + 9);
	}

	public bool RemoveCustomModule(int customID)
	{
		int customModuleRow = getCustomModuleRow(customID);
		if (customModuleRow > -1)
		{
			customModules[customModuleRow, 0] = 0;
			customModules[customModuleRow, 1] = null;
			for (int i = customModuleRow + 1; i < customModulesCount; i++)
			{
				customModules[i - 1, 0] = customModules[i, 0];
				customModules[i - 1, 1] = customModules[i, 1];
				customModules[i - 1, 2] = customModules[i, 2];
			}
			customModulesCount--;
			if (customModulesCount >= 0)
			{
				object[,] array = new object[customModulesCount, 4];
				if (customModules != null)
				{
					Array.Copy(customModules, array, array.Length);
				}
				customModules = array;
			}
		}
		return true;
	}

	public void SetLicenseKeyOnControl(Control curControl, string curProgID)
	{
		string licenseKey = GetLicenseKey(curProgID);
		if (licenseKey != null && licenseKey.Length != 0)
		{
			typeof(AxHost).GetField("licenseKey", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(curControl, licenseKey);
		}
	}

	public string GetLicenseKey(string progID)
	{
		if (licenseTable.Count == 0)
		{
			loadLicenses();
		}
		string text = (string)licenseTable[progID.ToLower()];
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}
}

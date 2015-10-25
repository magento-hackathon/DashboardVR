using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class MagentoSales {

	public string ts;
	public float sales;
	public int orders;

	public MagentoProduct[] products;

	public MagentoSales()
	{
	}

	public static MagentoSales[] GetMagentoSalesFromJson(string json)
	{
		MagentoSales[] mSales = new MagentoSales[0];
		try{
			mSales = JsonReader.Deserialize<MagentoSales[]>(json);
		}catch(System.Exception e){
			Debug.Log(e.Message);
		}

		return mSales;
	}
}

public class MagentoProduct {

	public string name;
	public int sold;
	public float prize;

	public MagentoProduct()
	{
	}
}

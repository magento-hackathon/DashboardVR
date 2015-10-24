using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class MagentoSales {

	//{"ts":"1382639619-1382726019","sales":9270.47,"orders":43,"product_1":2,"product_2":2,"product_3":3,"product_4":4,"product_5":10}

	public string ts;
	public float sales;
	public int orders;

	public MagentoProduct[] products;

	private MagentoSales()
	{
	}

	public static MagentoSales[] GetMagentoSalesFromJson(string json)
	{
		MagentoSales[] mSales = new MagentoSales[0];

		try{
			Dictionary<string, object>[] resultList = JsonReader.Deserialize<Dictionary<string, object>[]>(json);

			mSales = GetMagentoSalesFromList(resultList);
		}catch(System.Exception e){
			Debug.Log(e.Message);
		}

		return mSales;
	}

	public static MagentoSales[] GetMagentoSalesFromList(Dictionary<string, object>[] list)
	{
		MagentoSales[] mSales = new MagentoSales[list.Length];
		for(int i=0; i<list.Length; i++){
			mSales[i] = GetMagentoSaleFromDictionary((list[i]));
		}

		return mSales;
	}

	public static MagentoSales GetMagentoSaleFromDictionary(Dictionary<string, object> dictionary)
	{
		MagentoSales mSale = new MagentoSales();

		mSale.ts = System.Convert.ToString(dictionary["ts"]);
		mSale.sales = (float)System.Convert.ToDouble(dictionary["sales"]);
		mSale.orders = System.Convert.ToInt32(dictionary["orders"]);

		mSale.products = MagentoProduct.GetMagentoProductFromList(dictionary["products"] as Dictionary<string, object>[]);

		return mSale;
	}
}

public class MagentoProduct {

	public string name;
	public int sold;

	private MagentoProduct()
	{
	}

	public static MagentoProduct[] GetMagentoProductFromList(Dictionary<string, object>[] list)
	{
		MagentoProduct[] products = new MagentoProduct[list.Length];
		for(int i=0; i<list.Length; i++){
			products[i] = GetMagentoProductsFromDictionary(list[i] as Dictionary<string, object>);
		}

		return products;
	}

	public static MagentoProduct GetMagentoProductsFromDictionary(Dictionary<string, object> dictionary)
	{
		MagentoProduct product = new MagentoProduct();
		product.name = System.Convert.ToString(dictionary["name"]);
		product.sold = System.Convert.ToInt32(dictionary["sold"]);

		return product;
	}
}

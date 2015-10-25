using UnityEngine;
using System.Collections;
using BestHTTP;

public class MagentoDataManager : MonoBehaviour {

	public Shader defaultShader;

	public MagentoSales[] sales;

	public float salesCruncFactor = 200f;
	public float orderCruncFactor = 10f;

	public ColloseumRoot colloseumRoot;

	// Use this for initialization
	void Start ()
	{
		HTTPRequest request = new HTTPRequest(new System.Uri(HostInfo.host), (req, resp)=>{

			switch(req.State){
			case HTTPRequestStates.Finished:
				if(resp.IsSuccess){
					GammaTimer timer = new GammaTimer(System.TimeSpan.FromSeconds(0.1f), ()=>{
						sales = MagentoSales.GetMagentoSalesFromJson(resp.DataAsText);
						PresentSales(sales);
					});
					timer.Start();
				}else{
					Debug.LogError("Response was not OK");
				}
				break;
			default:
				Debug.LogError("Request was not Finished succesfully");
				break;
			}
		});
		request.Send();
	}

	private Material default1Mat;
	private Material default2Mat;

	public void PresentSales(MagentoSales[] mySales)
	{
		float angle = 360f/sales.Length;
		float radius = sales.Length/(2*Mathf.PI) + 10;

		Transform rotationTransform = new GameObject("rotationHelper").transform;
		rotationTransform.position = Vector3.zero;
		rotationTransform.localScale = Vector3.one;

		default1Mat = new Material(defaultShader);
		default2Mat = new Material(defaultShader);

		for(int i=0; i<sales.Length; i++){

			rotationTransform.rotation = Quaternion.AngleAxis(angle*i, Vector3.up);
			Vector3 polPos =  rotationTransform.TransformPoint(new Vector3(0, 0, radius));

			SpawnSaleCube(polPos, i, default1Mat);

			SpawnOrderCube(polPos, i, default2Mat);

			float amount = 0;
			float sold = 0;
			for(int t=0; t<sales[i].products.Length; t++){
				polPos = rotationTransform.TransformPoint(new Vector3(0, 0, radius-5 + t));

				SpawnProductPrizeCube(polPos, i, t, default1Mat, ref amount);

				SpawnProductOrderCube(polPos, i, t, default2Mat, ref sold);
			}
		}

		colloseumRoot.AnimateUp();
	}

	public void SpawnSaleCube(Vector3 polPos, int i, Material mat)
	{
		GameObject salesCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		salesCube.transform.SetParent(colloseumRoot.transform);

		salesCube.transform.localScale = new Vector3(1, sales[i].sales/salesCruncFactor, 1);
		salesCube.transform.localPosition = polPos + Vector3.up*(salesCube.transform.localScale.y/2f);

		MeshRenderer mr = salesCube.GetComponent<MeshRenderer>();
		mr.sharedMaterial = mat;
		mr.material.color = new Color(0, 0, 0.6f + 0.4f*(i%2), 1);

		InfoField infoF = salesCube.AddComponent<InfoField>();

		string timestamps = TimestampHelper.Instance (sales [i].ts).FormatFromStr();

		infoF.displayInfo = string.Format("{0}: {1} € sales", timestamps, sales[i].sales);
		infoF.infoType = InfoField.InfoType.Money;
	}

	public void SpawnProductPrizeCube(Vector3 polPos, int i, int t, Material mat, ref float amount)
	{
		GameObject salesCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		salesCube.transform.SetParent(colloseumRoot.transform);

		amount = amount + sales[i].products[t].prize*sales[i].products[t].sold;

		salesCube.transform.localScale = new Vector3(1, amount/salesCruncFactor, 1);
		salesCube.transform.localPosition = polPos + Vector3.up*(salesCube.transform.localScale.y/2f);

		MeshRenderer mr = salesCube.GetComponent<MeshRenderer>();
		mr.sharedMaterial = mat;
		mr.material.color = new Color(0, 0, 0.6f + 0.4f*(i%2) - 0.15f*t, 1);

		InfoField infoF = salesCube.AddComponent<InfoField>();
		
		string timestamps = TimestampHelper.Instance (sales [i].ts).FormatFromToStr();
		infoF.displayInfo = string.Format("Sales volume of Product {0}", sales[i].products[t].name);
		infoF.infoType = InfoField.InfoType.Money;
	}

	public void SpawnOrderCube(Vector3 polPos, int i, Material mat)
	{
		GameObject oderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		oderCube.transform.SetParent(colloseumRoot.transform);

		oderCube.transform.localScale = new Vector3(1, sales[i].orders/orderCruncFactor, 1);
		oderCube.transform.localPosition = polPos - Vector3.up*(oderCube.transform.localScale.y/2f);

		MeshRenderer mr = oderCube.GetComponent<MeshRenderer>();
		mr.sharedMaterial = mat;
		mr.material.color = new Color(0.6f + 0.4f*(i%2), 0, 0, 1);

		InfoField infoF = oderCube.AddComponent<InfoField>();
		string timestamps = TimestampHelper.Instance (sales [i].ts).FormatFromStr();

		infoF.displayInfo = string.Format("{0}: {1} orders", timestamps, sales[i].orders);
		infoF.infoType = InfoField.InfoType.Quantity;
	}

	public void SpawnProductOrderCube(Vector3 polPos, int i, int t, Material mat, ref float sold)
	{
		GameObject oderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		oderCube.name = sales[i].products[t].name;
		oderCube.transform.SetParent(colloseumRoot.transform);

		sold = sold + sales[i].products[t].sold;

		oderCube.transform.localScale = new Vector3(1, sold/orderCruncFactor, 1);
		oderCube.transform.localPosition = polPos - Vector3.up*(oderCube.transform.localScale.y/2f);

		MeshRenderer mr = oderCube.GetComponent<MeshRenderer>();
		mr.sharedMaterial = mat;
		mr.material.color = new Color(0.6f + 0.4f*(i%2), 0, 0, 1);

		InfoField infoF = oderCube.AddComponent<InfoField>();
		string timestamps = TimestampHelper.Instance (sales [i].ts).FormatFromStr();
		
		infoF.displayInfo = string.Format("{0}: {1} {2} €", timestamps, sales[i].products[t].name, sales[i].products[t].prize);
		infoF.infoType = InfoField.InfoType.Quantity;
	}
}

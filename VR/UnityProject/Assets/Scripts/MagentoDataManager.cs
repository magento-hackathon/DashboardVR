using UnityEngine;
using System.Collections;
using BestHTTP;

public class MagentoDataManager : MonoBehaviour {

	public Shader defaultShader;

	public MagentoSales[] sales;

	public float salesCruncFactor = 200f;
	public float orderCruncFactor = 10f;

	// Use this for initialization
	void Start ()
	{
		HTTPRequest request = new HTTPRequest(new System.Uri(HostInfo.host), (req, resp)=>{

			switch(req.State){
			case HTTPRequestStates.Finished:
				if(resp.IsSuccess){
					sales = MagentoSales.GetMagentoSalesFromJson(resp.DataAsText);
					PresentSales(sales);
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
		float radius = sales.Length/(2*Mathf.PI);

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
		}
	}

	public void SpawnSaleCube(Vector3 polPos, int i, Material mat)
	{
		GameObject salesCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		salesCube.transform.localScale = new Vector3(1, sales[i].sales/salesCruncFactor, 1);
		salesCube.transform.position = polPos + Vector3.up*(salesCube.transform.localScale.y/2f);

		MeshRenderer mr = salesCube.GetComponent<MeshRenderer>();
		mr.sharedMaterial = mat;
		mr.material.color = new Color(0, 0, 0.6f + 0.4f*(i%2), 1);

		InfoField infoF = salesCube.AddComponent<InfoField>();

		string timestamps = TimestampHelper.Instance (sales [i].ts).FormatFromToStr();

		infoF.displayInfo = string.Format("{0} sales between {1}", sales[i].sales, timestamps);
	}

	public void SpawnOrderCube(Vector3 polPos, int i, Material mat)
	{
		GameObject salesCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		salesCube.transform.localScale = new Vector3(1, sales[i].orders/orderCruncFactor, 1);
		salesCube.transform.position = polPos - Vector3.up*(salesCube.transform.localScale.y/2f);

		MeshRenderer mr = salesCube.GetComponent<MeshRenderer>();
		mr.sharedMaterial = mat;
		mr.material.color = new Color(0.6f + 0.4f*(i%2), 0, 0, 1);

		InfoField infoF = salesCube.AddComponent<InfoField>();
		string timestamps = TimestampHelper.Instance (sales [i].ts).FormatFromToStr();

		infoF.displayInfo = string.Format("{0} orders between {1}", sales[i].orders, timestamps);
	}
}

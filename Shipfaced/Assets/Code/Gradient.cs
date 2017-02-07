using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

//Taken from three different sources I'm having trouble finding again. Modded to hell and back by me, really really inefficient but it works ;)

[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseMeshEffect
{
	public bool gradient;
	public Color32 topColor;
	public Color32 bottomColor;
	public bool movingGradient;
	public float gradientRate;
	public float bendSpeed = 1;
	public float bendAmount = 1;
	public float bendDepth;
	public float horBendWidth;
	public bool curveBend;
	public bool exponentBend;
	public bool horizontalBend;
	public bool depthBend;
	public bool rotation;
	public bool spinIn = false;

	new void Start()
	{
		base.Start();
		if (spinIn)
		{
			transform.rotation = Quaternion.Euler(new Vector3(16, 270, 0));
			StartCoroutine("InitialSpin");
		}

		if (curveForText[0].time != 0)
		{
			var tmpRect = curveForText[0];
			tmpRect.time = 0;
			curveForText.MoveKey(0, tmpRect);
		}
		if (rectTrans == null)
			rectTrans = GetComponent<RectTransform>();
		if (curveForText[curveForText.length - 1].time != rectTrans.rect.width)
			OnRectTransformDimensionsChange();
	}

	public void UpdatePlz()
	{
		curveMultiplier = Mathf.Sin(Time.time * bendSpeed) * bendAmount;
		Graphic g = GetComponent<Graphic>();
		g.SetVerticesDirty();

		if (rotation)
			Rotate();

		if (movingGradient)
			gradientRate = 1.2f + Mathf.Sin(Time.time / 3) * 0.4f;
	}
	public AnimationCurve curveForText = AnimationCurve.Linear(0, 0, 1, 10);
	public float curveMultiplier = 1;
	private RectTransform rectTrans;




	public override void ModifyMesh(VertexHelper vh)
	{
		if (!this.IsActive())
			return;


		List<UIVertex> vertexList = new List<UIVertex>();
		vh.GetUIVertexStream(vertexList);

		ModifyVertices(vertexList);

		vh.Clear();
		vh.AddUIVertexTriangleStream(vertexList);
	}

	public void ModifyVertices(List<UIVertex> verts)
	{
		if (!IsActive())
			return;


		float bottomY = verts[0].position.y;
		float topY = verts[0].position.y;

		for (int i = 1; i < verts.Count; i++)
		{
			float y = verts[i].position.y;
			if (y > topY)
			{
				topY = y;
			}
			else if (y < bottomY)
			{
				bottomY = y;
			}
		}

		float uiElementHeight = topY - bottomY;

		for (int index = 0; index < verts.Count; index++)
		{
			var uiVertex = verts[index];
			if (gradient)
			{
				uiVertex.color = Color32.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight * gradientRate);
			}
			if (curveBend)
			{
				uiVertex.position.y += curveForText.Evaluate(rectTrans.rect.width * rectTrans.pivot.x + uiVertex.position.x) * curveMultiplier;
			}
			if (exponentBend)
			{
				uiVertex.position.y += Mathf.Lerp(1, curveForText.Evaluate(rectTrans.rect.width * rectTrans.pivot.x + uiVertex.position.x) * curveMultiplier, (uiVertex.position.y - bottomY) / uiElementHeight);
			}
			if (depthBend)
			{
				uiVertex.position.z += curveForText.Evaluate(rectTrans.rect.width * rectTrans.pivot.x + uiVertex.position.x) * curveMultiplier * bendDepth;
			}
			if (horizontalBend)
			{
				uiVertex.position.x -= curveForText.Evaluate(rectTrans.rect.width * rectTrans.pivot.x + uiVertex.position.x) * curveMultiplier * horBendWidth;
			}
			verts[index] = uiVertex;
		}
	}

	void Rotate()
	{
		transform.rotation = Quaternion.Euler(transform.transform.eulerAngles + new Vector3(0, Mathf.Sin(Time.time / 2) / 4, 0));
	}

	IEnumerator InitialSpin()
	{
		while (transform.rotation.eulerAngles.y > 90)
		{
			transform.rotation = Quaternion.Euler(transform.transform.eulerAngles + new Vector3(0, 0.33f, 0));
			yield return new WaitForSeconds(0);
		}
	}


	protected override void OnRectTransformDimensionsChange()
	{
		var tmpRect = curveForText[curveForText.length - 1];
		//tmpRect.time = rectTrans.rect.width;
		curveForText.MoveKey(curveForText.length - 1, tmpRect);
	}
}
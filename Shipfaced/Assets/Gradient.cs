using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseMeshEffect
{
	public Color32 topColor = Color.white;
	public Color32 bottomColor = Color.black;
	public float valueee;
	public float lerp;
	VertexHelper newHelper = new VertexHelper();
	List<UIVertex> vertices = new List<UIVertex>();

	public void UpdatePlz()
	{
		curveMultiplier = Mathf.Sin(Time.time) * 3;
		Graphic g = GetComponent<Graphic>();
		g.SetVerticesDirty();
	}
	public AnimationCurve curveForText = AnimationCurve.Linear(0, 0, 1, 10);
	public float curveMultiplier = 1;
	private RectTransform rectTrans;


#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate();
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
#endif
	protected override void Awake()
	{
		base.Awake();
		rectTrans = GetComponent<RectTransform>();
		OnRectTransformDimensionsChange();
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		rectTrans = GetComponent<RectTransform>();
		OnRectTransformDimensionsChange();
	}
	public override void ModifyMesh(Mesh mesh)
	{
		if (!this.IsActive())
			return;

		List<UIVertex> list = new List<UIVertex>();
		using (VertexHelper vertexHelper = new VertexHelper(mesh))
		{
			vertexHelper.GetUIVertexStream(list);
		}

		ModifyVertices(list);  // calls the old ModifyVertices which was used on pre 5.2

		using (VertexHelper vertexHelper2 = new VertexHelper())
		{
			vertexHelper2.AddUIVertexTriangleStream(list);
			vertexHelper2.FillMesh(mesh);
		}

	}

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
			uiVertex.color = Color32.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
			uiVertex.position.y += curveForText.Evaluate(rectTrans.rect.width * rectTrans.pivot.x + uiVertex.position.x) * curveMultiplier;
			verts[index] = uiVertex;
		}
	}


	protected override void OnRectTransformDimensionsChange()
	{
		var tmpRect = curveForText[curveForText.length - 1];
		//tmpRect.time = rectTrans.rect.width;
		curveForText.MoveKey(curveForText.length - 1, tmpRect);
	}
}
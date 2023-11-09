using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[StripFromAssetBundleScene]
public class BezierSpline : MonoBehaviour
{
	[SerializeField]
	private List<Vector3> points = new List<Vector3>();

	[SerializeField]
	private List<Vector3> ups = new List<Vector3>();

	[SerializeField]
	private List<BezierControlPointMode> modes = new List<BezierControlPointMode>();

	[SerializeField]
	private bool loop;

	public List<Vector3> Points
	{
		get
		{
			return points;
		}
		set
		{
			points = Points;
		}
	}

	public bool Loop
	{
		get
		{
			return loop;
		}
		set
		{
			loop = value;
			if (value)
			{
				modes[modes.Count - 1] = modes[0];
				SetControlPoint(0, points[0]);
			}
		}
	}

	public int ControlPointCount => points.Count;

	public int CurveCount => (points.Count - 1) / 3;

	public Vector3 GetControlPoint(int index)
	{
		return points[index];
	}

	public void SetControlPointUp(int index, Vector3 up)
	{
		int index2 = (index + 1) / 3;
		ups[index2] = up;
	}

	public Vector3 GetUp(int index)
	{
		int index2 = (index + 1) / 3;
		return ups[index2];
	}

	public void SetControlPoint(int index, Vector3 point)
	{
		if (index % 3 == 0)
		{
			Vector3 vector = point - points[index];
			if (loop)
			{
				if (index == 0)
				{
					points[1] += vector;
					points[points.Count - 2] += vector;
					points[points.Count - 1] = point;
				}
				else if (index == points.Count - 1)
				{
					points[0] = point;
					points[1] += vector;
					points[index - 1] += vector;
				}
				else
				{
					points[index - 1] += vector;
					points[index + 1] += vector;
				}
			}
			else
			{
				if (index > 0)
				{
					points[index - 1] += vector;
				}
				if (index + 1 < points.Count)
				{
					points[index + 1] += vector;
				}
			}
		}
		points[index] = point;
		EnforceMode(index);
	}

	public BezierControlPointMode GetControlPointMode(int index)
	{
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode(int index, BezierControlPointMode mode)
	{
		int num = (index + 1) / 3;
		modes[num] = mode;
		if (loop)
		{
			if (num == 0)
			{
				modes[modes.Count - 1] = mode;
			}
			else if (num == modes.Count - 1)
			{
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	private void EnforceMode(int index)
	{
		int num = (index + 1) / 3;
		BezierControlPointMode bezierControlPointMode = modes[num];
		if (bezierControlPointMode == BezierControlPointMode.Free || (!loop && (num == 0 || num == modes.Count - 1)))
		{
			return;
		}
		int num2 = num * 3;
		int num3;
		int num4;
		if (index <= num2)
		{
			num3 = num2 - 1;
			if (num3 < 0)
			{
				num3 = points.Count - 2;
			}
			num4 = num2 + 1;
			if (num4 >= points.Count)
			{
				num4 = 1;
			}
		}
		else
		{
			num3 = num2 + 1;
			if (num3 >= points.Count)
			{
				num3 = 1;
			}
			num4 = num2 - 1;
			if (num4 < 0)
			{
				num4 = points.Count - 2;
			}
		}
		Vector3 vector = points[num2];
		Vector3 vector2 = vector - points[num3];
		if (bezierControlPointMode == BezierControlPointMode.Aligned)
		{
			vector2 = vector2.normalized * Vector3.Distance(vector, points[num4]);
		}
		points[num4] = vector + vector2;
	}

	public Vector3 GetPoint(float t)
	{
		int num;
		if (t >= 1f)
		{
			t = 1f;
			num = points.Count - 4;
		}
		else
		{
			t = Mathf.Clamp01(t) * (float)CurveCount;
			num = (int)t;
			t -= (float)num;
			num *= 3;
		}
		return base.transform.TransformPoint(Bezier.GetPoint(points[num], points[num + 1], points[num + 2], points[num + 3], t));
	}

	public Vector3 GetVelocity(float t)
	{
		int num;
		if (t >= 1f)
		{
			t = 1f;
			num = points.Count - 4;
		}
		else
		{
			t = Mathf.Clamp01(t) * (float)CurveCount;
			num = (int)t;
			t -= (float)num;
			num *= 3;
		}
		return base.transform.TransformPoint(Bezier.GetFirstDerivative(points[num], points[num + 1], points[num + 2], points[num + 3], t)) - base.transform.position;
	}

	public Vector3 GetDirection(float t)
	{
		return GetVelocity(t).normalized;
	}

	public bool IsNoHandle(int index)
	{
		return index % 3 == 0;
	}

	public bool IsLast(int index)
	{
		return index == points.Count - 1;
	}

	public bool IsFirst(int index)
	{
		return index == 0;
	}

	public Vector3 GetDirectionAtPoint(int index)
	{
		if (index < points.Count - 1 && index > 0)
		{
			return (points[index + 1] - points[index - 1]).normalized;
		}
		if (index == 0)
		{
			return (points[index + 1] - points[index]).normalized;
		}
		return (points[index] - points[index - 1]).normalized;
	}

	public Vector3 GetUp(float t)
	{
		int num;
		if (t >= 1f)
		{
			t = 1f;
			num = ups.Count - 2;
		}
		else
		{
			t = Mathf.Clamp01(t) * (float)CurveCount;
			num = (int)t;
			t -= (float)num;
		}
		return Vector3.Lerp(ups[num], ups[num + 1], t);
	}

	public void AddCurve()
	{
		Vector3 vector = points[points.Count - 1];
		Vector3 directionAtPoint = GetDirectionAtPoint(points.Count - 1);
		points.Add(vector);
		points.Add(vector);
		points.Add(vector);
		vector += directionAtPoint;
		points[points.Count - 3] = vector;
		vector += directionAtPoint;
		points[points.Count - 2] = vector;
		vector += directionAtPoint;
		points[points.Count - 1] = vector;
		modes.Add(modes[modes.Count - 1]);
		EnforceMode(points.Count - 4);
		ups.Add(ups[ups.Count - 1]);
		if (loop)
		{
			points[points.Count - 1] = points[0];
			ups[ups.Count - 1] = ups[0];
			modes[modes.Count - 1] = modes[0];
			EnforceMode(0);
		}
	}

	public void InsertCurve(int index)
	{
		Vector3 vector = points[index];
		Vector3 directionAtPoint = GetDirectionAtPoint(index);
		points.Insert(index, vector);
		points.Insert(index, vector);
		points.Insert(index, vector);
		vector += directionAtPoint;
		points[index + 1] = vector;
		vector += directionAtPoint;
		points[index + 2] = vector;
		vector += directionAtPoint;
		points[index + 3] = vector;
		modes.Insert((index + 1) / 3, modes[(index + 1) / 3]);
		EnforceMode(index);
		EnforceMode(index + 1);
		EnforceMode(index + 2);
		EnforceMode(index + 3);
		ups.Insert((index + 1) / 3, ups[(index + 1) / 3]);
	}

	public void RemoveCurve(int index)
	{
		bool num = IsLast(index);
		bool flag = IsFirst(index);
		if (!num)
		{
			points.RemoveAt(index + 1);
		}
		if (flag)
		{
			points.RemoveAt(index + 2);
		}
		points.RemoveAt(index);
		if (!flag)
		{
			points.RemoveAt(index - 1);
		}
		if (num)
		{
			points.RemoveAt(index - 2);
		}
		modes.RemoveAt((index + 1) / 3);
		ups.RemoveAt((index + 1) / 3);
		if (loop)
		{
			modes[modes.Count - 1] = modes[0];
			SetControlPoint(0, points[0]);
		}
	}

	public void Reset()
	{
		points.Clear();
		modes.Clear();
		ups.Clear();
		loop = false;
		points.Add(new Vector3(0f, 0f, 0f));
		points.Add(new Vector3(0f, 0f, 1f));
		points.Add(new Vector3(0f, 0f, 2f));
		points.Add(new Vector3(0f, 0f, 3f));
		modes.Add(BezierControlPointMode.Mirrored);
		modes.Add(BezierControlPointMode.Mirrored);
		ups.Add(Vector3.up);
		ups.Add(Vector3.up);
	}

	#if UNITY_EDITOR

	private void OnDrawGizmos() {
		if (Selection.Contains(gameObject)) return;
		Vector3 GetPoint(int index) {
			Vector3 point = transform.TransformPoint(GetControlPoint(index));
			return point;
		}
		Vector3 ShowPoint(Vector3 point, float scale = 1f) {
			var size = HandleUtility.GetHandleSize(point) * 0.2f * scale;
			Gizmos.DrawCube(point, new Vector3(size, size, size));
			return point;
		}
		Vector3 p0 = GetPoint(0);
		ShowPoint(p0);
		for (int i = 1; i < ControlPointCount; i += 3) {
			Vector3 p1 = GetPoint(i);
			Vector3 p2 = GetPoint(i + 1);
			Vector3 p3 = GetPoint(i + 2);
			ShowPoint(p3);

			Vector3 midpoint = Bezier.GetPoint(p0, p1, p2, p3, 0.5f);
			ShowPoint(midpoint, 0.8f);
			
			Handles.color = Color.gray;
			
			Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
			p0 = p3;
		}
	}

	public delegate void OnChangeHandler();
	public event OnChangeHandler OnChange;

	private void OnValidate() {
		// TODO this duplicates the DirtyCount call from OnInspectorGUI()
		CoroutineUtils.RunNextTick(EmitChange);
    }
	public void EmitChange() {
		OnChange?.Invoke();
	}
	#endif
}
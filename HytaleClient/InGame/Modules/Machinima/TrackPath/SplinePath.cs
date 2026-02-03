using System;
using System.Collections.Generic;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Machinima.TrackPath
{
	// Token: 0x02000915 RID: 2325
	internal class SplinePath : LinePath
	{
		// Token: 0x06004705 RID: 18181 RVA: 0x0010C837 File Offset: 0x0010AA37
		public SplinePath()
		{
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x0010C849 File Offset: 0x0010AA49
		public SplinePath(Vector3[] points, int segmentCount = 25)
		{
			this._segmentCount = segmentCount;
			this.UpdatePoints(points);
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x0010C86A File Offset: 0x0010AA6A
		public override void UpdatePoints(Vector3[] points)
		{
			base.ControlPoints = points;
			this.UpdateSegmentPoints();
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x0010C87C File Offset: 0x0010AA7C
		public override Vector3 GetPathPosition(int index, float progress, bool lengthCorrected = false, Easing.EasingType easingType = Easing.EasingType.Linear)
		{
			bool flag = base.ControlPoints == null || base.ControlPoints.Length < 1;
			Vector3 result;
			if (flag)
			{
				result = Vector3.NaN;
			}
			else
			{
				bool flag2 = index < 0;
				if (flag2)
				{
					result = base.ControlPoints[0];
				}
				else
				{
					bool flag3 = index >= base.ControlPoints.Length;
					if (flag3)
					{
						result = base.ControlPoints[base.ControlPoints.Length - 1];
					}
					else
					{
						int num = index + 1;
						int num2 = (index - 1 >= 0) ? (index - 1) : index;
						int num3 = (index + 2 < base.ControlPoints.Length) ? (index + 2) : num;
						Vector3 vector = base.ControlPoints[num2];
						Vector3 vector2 = base.ControlPoints[index];
						Vector3 vector3 = base.ControlPoints[num];
						Vector3 vector4 = base.ControlPoints[num3];
						bool flag4 = easingType > Easing.EasingType.Linear;
						if (flag4)
						{
							progress = Easing.Ease(easingType, progress, 0f, 1f, 1f);
						}
						if (lengthCorrected)
						{
							progress = this.GetAdjustedProgress(index, progress);
						}
						Vector3 vector5;
						Vector3.Spline(ref progress, ref vector, ref vector2, ref vector3, ref vector4, out vector5);
						result = vector5;
					}
				}
			}
			return result;
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x0010C9B0 File Offset: 0x0010ABB0
		private void UpdateSegmentPoints()
		{
			bool flag = base.ControlPoints.Length < 2;
			if (flag)
			{
				Array.Clear(base.SegmentInfo, 0, base.SegmentInfo.Length);
				Array.Clear(base.SegmentPoints, 0, base.SegmentPoints.Length);
			}
			else
			{
				List<Vector3[]> list = new List<Vector3[]>();
				List<Vector3[]> list2 = new List<Vector3[]>();
				List<Vector3> list3 = new List<Vector3>();
				List<Vector3> list4 = new List<Vector3>();
				for (int i = 0; i < base.ControlPoints.Length - 1; i++)
				{
					int num = i;
					int num2 = i + 1;
					int num3 = (i - 1 >= 0) ? (i - 1) : num;
					int num4 = (i + 2 < base.ControlPoints.Length) ? (i + 2) : num2;
					Vector3 vector = base.ControlPoints[num3];
					Vector3 vector2 = base.ControlPoints[num];
					Vector3 vector3 = base.ControlPoints[num2];
					Vector3 vector4 = base.ControlPoints[num4];
					float num5 = 0f;
					for (int j = 0; j <= this._segmentCount; j++)
					{
						float num6 = (float)j / (float)this._segmentCount;
						Vector3 vector5;
						Vector3.Spline(ref num6, ref vector, ref vector2, ref vector3, ref vector4, out vector5);
						float num7 = (j > 0) ? Vector3.Distance(list3[list3.Count - 1], vector5) : 0f;
						num5 += num7;
						list3.Add(vector5);
						list4.Add(new Vector3((float)i + num6, 0f, num5));
					}
					bool flag2 = num5 > 0f;
					if (flag2)
					{
						for (int k = 0; k < list4.Count; k++)
						{
							list4[k] = new Vector3(list4[k].X, list4[k].Z / num5, list4[k].Z);
						}
					}
					list.Add(list3.ToArray());
					list2.Add(list4.ToArray());
					list3.Clear();
					list4.Clear();
				}
				base.SegmentPoints = list.ToArray();
				base.SegmentInfo = list2.ToArray();
			}
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x0010CBEC File Offset: 0x0010ADEC
		public override float GetAdjustedProgress(int index, float progress)
		{
			float num = (float)index + progress;
			int i = 0;
			while (i < base.SegmentInfo[index].Length)
			{
				bool flag = i + 1 >= base.SegmentInfo[index].Length;
				float result;
				if (flag)
				{
					result = 1f;
				}
				else
				{
					float y = base.SegmentInfo[index][i].Y;
					float num2 = (y >= 1f) ? 1f : base.SegmentInfo[index][i + 1].Y;
					bool flag2 = progress >= y && progress < num2;
					if (!flag2)
					{
						i++;
						continue;
					}
					float num3 = num2 - y;
					float amount = (progress - y) / num3;
					float num4 = MathHelper.Lerp(base.SegmentInfo[index][i].X, base.SegmentInfo[index][i + 1].X, amount);
					result = num4 - (float)index;
				}
				return result;
			}
			return progress;
		}

		// Token: 0x040023B4 RID: 9140
		private readonly int _segmentCount = 25;
	}
}

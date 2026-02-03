using System;
using System.Collections.Generic;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Machinima.TrackPath
{
	// Token: 0x02000914 RID: 2324
	internal abstract class LinePath
	{
		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x060046F7 RID: 18167 RVA: 0x0010C699 File Offset: 0x0010A899
		// (set) Token: 0x060046F8 RID: 18168 RVA: 0x0010C6A1 File Offset: 0x0010A8A1
		public Vector3[] ControlPoints { get; protected set; }

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x0010C6AA File Offset: 0x0010A8AA
		// (set) Token: 0x060046FA RID: 18170 RVA: 0x0010C6B2 File Offset: 0x0010A8B2
		public Vector3[][] SegmentPoints { get; protected set; }

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060046FB RID: 18171 RVA: 0x0010C6BB File Offset: 0x0010A8BB
		// (set) Token: 0x060046FC RID: 18172 RVA: 0x0010C6C3 File Offset: 0x0010A8C3
		public Vector3[][] SegmentInfo { get; protected set; }

		// Token: 0x060046FD RID: 18173 RVA: 0x0010C6CC File Offset: 0x0010A8CC
		public LinePath()
		{
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x0010C6D6 File Offset: 0x0010A8D6
		public LinePath(Vector3[] points)
		{
			this.UpdatePoints(points);
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x0010C6E8 File Offset: 0x0010A8E8
		public virtual void UpdatePoints(Vector3[] points)
		{
			this.ControlPoints = points;
		}

		// Token: 0x06004700 RID: 18176
		public abstract Vector3 GetPathPosition(int index, float progress, bool lengthCorrected = false, Easing.EasingType easingType = Easing.EasingType.Linear);

		// Token: 0x06004701 RID: 18177 RVA: 0x0010C6F4 File Offset: 0x0010A8F4
		public virtual Vector3[] GetDrawPoints()
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < this.SegmentPoints.Length; i++)
			{
				foreach (Vector3 item in this.SegmentPoints[i])
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x0010C75C File Offset: 0x0010A95C
		public virtual float[] GetDrawFrames()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < this.SegmentInfo.Length; i++)
			{
				foreach (Vector3 vector in this.SegmentInfo[i])
				{
					list.Add(vector.X);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x0010C7C8 File Offset: 0x0010A9C8
		public virtual float[] GetSegmentLengths()
		{
			float[] array = new float[this.SegmentInfo.Length];
			for (int i = 0; i < this.SegmentInfo.Length; i++)
			{
				array[i] = this.SegmentInfo[i][this.SegmentInfo[i].Length - 1].Z;
			}
			return array;
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x0010C824 File Offset: 0x0010AA24
		public virtual float GetAdjustedProgress(int index, float progress)
		{
			return progress;
		}
	}
}

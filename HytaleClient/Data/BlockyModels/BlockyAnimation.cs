using System;
using System.Collections.Generic;
using HytaleClient.Math;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B5F RID: 2911
	internal class BlockyAnimation
	{
		// Token: 0x060059EC RID: 23020 RVA: 0x001BDD6C File Offset: 0x001BBF6C
		public static void GetInterpolationData<T>(List<BlockyAnimation.BlockyAnimKeyframe<T>> keyframes, float time, bool holdLastKeyframe, int duration, out T prevDelta, out T nextDelta, out float t)
		{
			BlockyAnimation.BlockyAnimKeyframe<T> blockyAnimKeyframe = keyframes[keyframes.Count - 1];
			BlockyAnimation.BlockyAnimKeyframe<T> blockyAnimKeyframe2 = null;
			for (int i = 0; i < keyframes.Count; i++)
			{
				blockyAnimKeyframe2 = keyframes[i];
				bool flag = (float)blockyAnimKeyframe2.Time > time;
				if (flag)
				{
					break;
				}
				blockyAnimKeyframe = keyframes[i];
			}
			bool flag2 = blockyAnimKeyframe == blockyAnimKeyframe2;
			if (flag2)
			{
				blockyAnimKeyframe2 = keyframes[0];
			}
			int num = blockyAnimKeyframe2.Time - blockyAnimKeyframe.Time;
			bool flag3 = num < 0 && !holdLastKeyframe;
			if (flag3)
			{
				num += duration;
			}
			float num2 = time - (float)blockyAnimKeyframe.Time;
			bool flag4 = num2 < 0f;
			if (flag4)
			{
				num2 += (float)duration;
			}
			t = ((num > 0) ? (num2 / (float)num) : 0f);
			bool flag5 = blockyAnimKeyframe.InterpolationType == BlockyAnimation.BlockyAnimNodeInterpolationType.Smooth;
			if (flag5)
			{
				t = Easing.CubicEaseInAndOut(t);
			}
			prevDelta = blockyAnimKeyframe.Delta;
			nextDelta = blockyAnimKeyframe2.Delta;
		}

		// Token: 0x040037F0 RID: 14320
		public const int FramesPerSecond = 60;

		// Token: 0x040037F1 RID: 14321
		public int Duration;

		// Token: 0x040037F2 RID: 14322
		public bool HoldLastKeyframe;

		// Token: 0x040037F3 RID: 14323
		public Dictionary<int, BlockyAnimation.BlockyAnimNodeAnim> NodeAnimationsByNameId = new Dictionary<int, BlockyAnimation.BlockyAnimNodeAnim>();

		// Token: 0x02000F57 RID: 3927
		public enum BlockyAnimNodeInterpolationType
		{
			// Token: 0x04004AA5 RID: 19109
			None,
			// Token: 0x04004AA6 RID: 19110
			Linear,
			// Token: 0x04004AA7 RID: 19111
			Smooth
		}

		// Token: 0x02000F58 RID: 3928
		public class BlockyAnimKeyframe<T>
		{
			// Token: 0x04004AA8 RID: 19112
			public int Time;

			// Token: 0x04004AA9 RID: 19113
			public T Delta;

			// Token: 0x04004AAA RID: 19114
			public BlockyAnimation.BlockyAnimNodeInterpolationType InterpolationType = BlockyAnimation.BlockyAnimNodeInterpolationType.None;
		}

		// Token: 0x02000F59 RID: 3929
		public class BlockyAnimNodeAnim
		{
			// Token: 0x04004AAB RID: 19115
			public BlockyAnimation.BlockyAnimNodeFrame[] Frames;

			// Token: 0x04004AAC RID: 19116
			public bool HasPosition;

			// Token: 0x04004AAD RID: 19117
			public bool HasOrientation;

			// Token: 0x04004AAE RID: 19118
			public bool HasShapeStretch;

			// Token: 0x04004AAF RID: 19119
			public bool HasShapeVisible;

			// Token: 0x04004AB0 RID: 19120
			public bool HasShapeUvOffset;
		}

		// Token: 0x02000F5A RID: 3930
		public struct BlockyAnimNodeFrame
		{
			// Token: 0x04004AB1 RID: 19121
			public Vector3 Position;

			// Token: 0x04004AB2 RID: 19122
			public Quaternion Orientation;

			// Token: 0x04004AB3 RID: 19123
			public Vector3 ShapeStretch;

			// Token: 0x04004AB4 RID: 19124
			public bool ShapeVisible;

			// Token: 0x04004AB5 RID: 19125
			public Point ShapeUvOffset;
		}
	}
}

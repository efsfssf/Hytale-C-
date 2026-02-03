using System;
using System.Collections.Generic;
using HytaleClient.Math;
using Utf8Json;
using Utf8Json.Resolvers;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B67 RID: 2919
	internal class BlockyAnimationInitializer
	{
		// Token: 0x060059EE RID: 23022 RVA: 0x001BDE78 File Offset: 0x001BC078
		public static void Parse(byte[] data, NodeNameManager nodeNameManager, ref BlockyAnimation blockyAnimation)
		{
			BlockyAnimationJson blockyAnimationJson = JsonSerializer.Deserialize<BlockyAnimationJson>(data, StandardResolver.CamelCase);
			blockyAnimation.Duration = blockyAnimationJson.Duration;
			blockyAnimation.HoldLastKeyframe = blockyAnimationJson.HoldLastKeyframe;
			foreach (KeyValuePair<string, AnimationNode> keyValuePair in blockyAnimationJson.NodeAnimations)
			{
				BlockyAnimation.BlockyAnimNodeAnim blockyAnimNodeAnim = new BlockyAnimation.BlockyAnimNodeAnim
				{
					Frames = new BlockyAnimation.BlockyAnimNodeFrame[blockyAnimation.Duration + 1]
				};
				int orAddNameId = nodeNameManager.GetOrAddNameId(keyValuePair.Key);
				blockyAnimation.NodeAnimationsByNameId[orAddNameId] = blockyAnimNodeAnim;
				AnimationNode value = keyValuePair.Value;
				List<BlockyAnimation.BlockyAnimKeyframe<Vector3>> list = new List<BlockyAnimation.BlockyAnimKeyframe<Vector3>>();
				for (int i = 0; i < value.Position.Length; i++)
				{
					ref PositionFrame ptr = ref value.Position[i];
					list.Add(new BlockyAnimation.BlockyAnimKeyframe<Vector3>
					{
						Time = ptr.Time,
						Delta = ptr.Delta,
						InterpolationType = ((ptr.InterpolationType == "smooth") ? BlockyAnimation.BlockyAnimNodeInterpolationType.Smooth : BlockyAnimation.BlockyAnimNodeInterpolationType.Linear)
					});
				}
				List<BlockyAnimation.BlockyAnimKeyframe<Quaternion>> list2 = new List<BlockyAnimation.BlockyAnimKeyframe<Quaternion>>();
				for (int j = 0; j < value.Orientation.Length; j++)
				{
					ref OrientationFrame ptr2 = ref value.Orientation[j];
					list2.Add(new BlockyAnimation.BlockyAnimKeyframe<Quaternion>
					{
						Time = ptr2.Time,
						Delta = ptr2.Delta,
						InterpolationType = ((ptr2.InterpolationType == "smooth") ? BlockyAnimation.BlockyAnimNodeInterpolationType.Smooth : BlockyAnimation.BlockyAnimNodeInterpolationType.Linear)
					});
				}
				List<BlockyAnimation.BlockyAnimKeyframe<Vector3>> list3 = new List<BlockyAnimation.BlockyAnimKeyframe<Vector3>>();
				for (int k = 0; k < value.ShapeStretch.Length; k++)
				{
					ref ShapeStretchFrame ptr3 = ref value.ShapeStretch[k];
					list3.Add(new BlockyAnimation.BlockyAnimKeyframe<Vector3>
					{
						Time = ptr3.Time,
						Delta = ptr3.Delta,
						InterpolationType = ((ptr3.InterpolationType == "smooth") ? BlockyAnimation.BlockyAnimNodeInterpolationType.Smooth : BlockyAnimation.BlockyAnimNodeInterpolationType.Linear)
					});
				}
				List<BlockyAnimation.BlockyAnimKeyframe<bool>> list4 = new List<BlockyAnimation.BlockyAnimKeyframe<bool>>();
				for (int l = 0; l < value.ShapeVisible.Length; l++)
				{
					ref ShapeVisibleFrame ptr4 = ref value.ShapeVisible[l];
					list4.Add(new BlockyAnimation.BlockyAnimKeyframe<bool>
					{
						Time = ptr4.Time,
						Delta = ptr4.Delta
					});
				}
				List<BlockyAnimation.BlockyAnimKeyframe<Point>> list5 = new List<BlockyAnimation.BlockyAnimKeyframe<Point>>();
				for (int m = 0; m < value.ShapeUvOffset.Length; m++)
				{
					ref ShapeUvOffsetFrame ptr5 = ref value.ShapeUvOffset[m];
					list5.Add(new BlockyAnimation.BlockyAnimKeyframe<Point>
					{
						Time = ptr5.Time,
						Delta = ptr5.Delta
					});
				}
				blockyAnimNodeAnim.HasPosition = (list.Count > 0);
				blockyAnimNodeAnim.HasOrientation = (list2.Count > 0);
				blockyAnimNodeAnim.HasShapeStretch = (list3.Count > 0);
				blockyAnimNodeAnim.HasShapeVisible = (list4.Count > 0);
				blockyAnimNodeAnim.HasShapeUvOffset = (list5.Count > 0);
				for (int n = 0; n <= blockyAnimation.Duration; n++)
				{
					bool hasPosition = blockyAnimNodeAnim.HasPosition;
					if (hasPosition)
					{
						Vector3 value2;
						Vector3 value3;
						float num;
						BlockyAnimation.GetInterpolationData<Vector3>(list, (float)n, blockyAnimation.HoldLastKeyframe, blockyAnimation.Duration, out value2, out value3, out num);
						blockyAnimNodeAnim.Frames[n].Position = Vector3.Lerp(value2, value3, num);
					}
					bool hasOrientation = blockyAnimNodeAnim.HasOrientation;
					if (hasOrientation)
					{
						float num;
						Quaternion quaternion;
						Quaternion quaternion2;
						BlockyAnimation.GetInterpolationData<Quaternion>(list2, (float)n, blockyAnimation.HoldLastKeyframe, blockyAnimation.Duration, out quaternion, out quaternion2, out num);
						blockyAnimNodeAnim.Frames[n].Orientation = Quaternion.Slerp(quaternion, quaternion2, num);
					}
					bool hasShapeStretch = blockyAnimNodeAnim.HasShapeStretch;
					if (hasShapeStretch)
					{
						Vector3 value2;
						Vector3 value3;
						float num;
						BlockyAnimation.GetInterpolationData<Vector3>(list3, (float)n, blockyAnimation.HoldLastKeyframe, blockyAnimation.Duration, out value2, out value3, out num);
						blockyAnimNodeAnim.Frames[n].ShapeStretch = Vector3.Lerp(value2, value3, num);
					}
					bool hasShapeVisible = blockyAnimNodeAnim.HasShapeVisible;
					if (hasShapeVisible)
					{
						float num;
						bool flag;
						bool flag2;
						BlockyAnimation.GetInterpolationData<bool>(list4, (float)n, blockyAnimation.HoldLastKeyframe, blockyAnimation.Duration, out flag, out flag2, out num);
						blockyAnimNodeAnim.Frames[n].ShapeVisible = ((num == 1f) ? flag2 : flag);
					}
					bool hasShapeUvOffset = blockyAnimNodeAnim.HasShapeUvOffset;
					if (hasShapeUvOffset)
					{
						float num;
						Point point;
						Point point2;
						BlockyAnimation.GetInterpolationData<Point>(list5, (float)n, blockyAnimation.HoldLastKeyframe, blockyAnimation.Duration, out point, out point2, out num);
						blockyAnimNodeAnim.Frames[n].ShapeUvOffset = ((num == 1f) ? point2 : point);
					}
				}
			}
		}
	}
}

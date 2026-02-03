using System;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B73 RID: 2931
	internal class ModelTransformHelper
	{
		// Token: 0x06005A0B RID: 23051 RVA: 0x001BF4D0 File Offset: 0x001BD6D0
		public static void Decompose(ModelTransform modelTransform, ref Vector3 position, ref Vector3 bodyOrientation, ref Vector3 lookOrientation)
		{
			Position position_ = modelTransform.Position_;
			bool flag = position_ != null;
			if (flag)
			{
				position = new Vector3((!double.IsNaN(position_.X) && !double.IsInfinity(position_.X)) ? ((float)position_.X) : position.X, (!double.IsNaN(position_.Y) && !double.IsInfinity(position_.Y)) ? ((float)position_.Y) : position.Y, (!double.IsNaN(position_.Z) && !double.IsInfinity(position_.Z)) ? ((float)position_.Z) : position.Z);
			}
			Direction bodyOrientation2 = modelTransform.BodyOrientation;
			bool flag2 = bodyOrientation2 != null;
			if (flag2)
			{
				bodyOrientation = new Vector3((!float.IsNaN(bodyOrientation2.Pitch) && !float.IsInfinity(bodyOrientation2.Pitch)) ? MathHelper.WrapAngle(bodyOrientation2.Pitch) : bodyOrientation.Pitch, (!float.IsNaN(bodyOrientation2.Yaw) && !float.IsInfinity(bodyOrientation2.Yaw)) ? MathHelper.WrapAngle(bodyOrientation2.Yaw) : bodyOrientation.Yaw, (!float.IsNaN(bodyOrientation2.Roll) && !float.IsInfinity(bodyOrientation2.Roll)) ? MathHelper.WrapAngle(bodyOrientation2.Roll) : bodyOrientation.Roll);
			}
			Direction lookOrientation2 = modelTransform.LookOrientation;
			bool flag3 = lookOrientation2 != null;
			if (flag3)
			{
				lookOrientation = new Vector3((!float.IsNaN(lookOrientation2.Pitch) && !float.IsInfinity(lookOrientation2.Pitch)) ? MathHelper.WrapAngle(lookOrientation2.Pitch) : lookOrientation.Pitch, (!float.IsNaN(lookOrientation2.Yaw) && !float.IsInfinity(lookOrientation2.Yaw)) ? MathHelper.WrapAngle(lookOrientation2.Yaw) : lookOrientation.Yaw, (!float.IsNaN(lookOrientation2.Roll) && !float.IsInfinity(lookOrientation2.Roll)) ? MathHelper.WrapAngle(lookOrientation2.Roll) : lookOrientation.Roll);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B69 RID: 2921
	internal struct BlockyModelNode
	{
		// Token: 0x060059FF RID: 23039 RVA: 0x001BEEB0 File Offset: 0x001BD0B0
		public static BlockyModelNode CreateMapBlockNode(int nodeNameId, float y, float height)
		{
			return new BlockyModelNode
			{
				NameId = nodeNameId,
				Position = new Vector3(0f, y, 0f),
				Orientation = Quaternion.Identity,
				Stretch = Vector3.One,
				Visible = true,
				Children = new List<int>(),
				Type = BlockyModelNode.ShapeType.Box,
				Size = new Vector3(32f, 32f * height, 32f),
				TextureLayout = new BlockyModelFaceTextureLayout[6]
			};
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x001BEF48 File Offset: 0x001BD148
		public BlockyModelNode Clone()
		{
			BlockyModelNode result = this;
			result.Children = new List<int>();
			bool flag = this.TextureLayout != null;
			if (flag)
			{
				result.TextureLayout = (BlockyModelFaceTextureLayout[])this.TextureLayout.Clone();
			}
			return result;
		}

		// Token: 0x04003814 RID: 14356
		public int NameId;

		// Token: 0x04003815 RID: 14357
		public CameraNode CameraNode;

		// Token: 0x04003816 RID: 14358
		public Vector3 Position;

		// Token: 0x04003817 RID: 14359
		public Quaternion Orientation;

		// Token: 0x04003818 RID: 14360
		public BlockyModelNode.ShapeType Type;

		// Token: 0x04003819 RID: 14361
		public Vector3 Offset;

		// Token: 0x0400381A RID: 14362
		public Vector3 ProceduralOffset;

		// Token: 0x0400381B RID: 14363
		public Vector3 ProceduralRotation;

		// Token: 0x0400381C RID: 14364
		public Vector3 Stretch;

		// Token: 0x0400381D RID: 14365
		public bool Visible;

		// Token: 0x0400381E RID: 14366
		public bool DoubleSided;

		// Token: 0x0400381F RID: 14367
		public ShadingMode ShadingMode;

		// Token: 0x04003820 RID: 14368
		public byte GradientId;

		// Token: 0x04003821 RID: 14369
		public Vector3 Size;

		// Token: 0x04003822 RID: 14370
		public bool IsPiece;

		// Token: 0x04003823 RID: 14371
		public BlockyModelNode.QuadNormal QuadNormalDirection;

		// Token: 0x04003824 RID: 14372
		public byte AtlasIndex;

		// Token: 0x04003825 RID: 14373
		public BlockyModelFaceTextureLayout[] TextureLayout;

		// Token: 0x04003826 RID: 14374
		public List<int> Children;

		// Token: 0x02000F5B RID: 3931
		public enum ShapeType
		{
			// Token: 0x04004AB7 RID: 19127
			None,
			// Token: 0x04004AB8 RID: 19128
			Box,
			// Token: 0x04004AB9 RID: 19129
			Quad
		}

		// Token: 0x02000F5C RID: 3932
		public enum QuadNormal
		{
			// Token: 0x04004ABB RID: 19131
			PlusZ,
			// Token: 0x04004ABC RID: 19132
			MinusZ,
			// Token: 0x04004ABD RID: 19133
			PlusX,
			// Token: 0x04004ABE RID: 19134
			MinusX,
			// Token: 0x04004ABF RID: 19135
			PlusY,
			// Token: 0x04004AC0 RID: 19136
			MinusY
		}
	}
}

using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Utf8Json;
using Utf8Json.Resolvers;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B71 RID: 2929
	internal class BlockyModelInitializer
	{
		// Token: 0x06005A01 RID: 23041 RVA: 0x001BEF94 File Offset: 0x001BD194
		public static void Parse(byte[] data, NodeNameManager nodeNameManager, ref BlockyModel blockyModel)
		{
			try
			{
				BlockyModelJson json = JsonSerializer.Deserialize<BlockyModelJson>(data, StandardResolver.CamelCase);
				BlockyModelInitializer.Parse(json, nodeNameManager, ref blockyModel);
			}
			catch (JsonParsingException ex)
			{
				throw new Exception(ex.GetUnderlyingStringUnsafe(), ex);
			}
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x001BEFDC File Offset: 0x001BD1DC
		public static void Parse(JObject jObject, NodeNameManager nodeNameManager, ref BlockyModel blockyModel)
		{
			BlockyModelJson json = jObject.ToObject<BlockyModelJson>(BlockyModelInitializer.JsonSerializerSettings);
			BlockyModelInitializer.Parse(json, nodeNameManager, ref blockyModel);
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x001BF000 File Offset: 0x001BD200
		private static void Parse(BlockyModelJson json, NodeNameManager nodeNameManager, ref BlockyModel blockyModel)
		{
			bool flag = json.Lod != null;
			if (flag)
			{
				blockyModel.Lod = BlockyModelInitializer.ParseLodMode(json.Lod);
			}
			for (int i = 0; i < json.Nodes.Length; i++)
			{
				BlockyModelInitializer.RecurseParseNode(ref json.Nodes[i], blockyModel, -1, nodeNameManager);
			}
			Array.Resize<BlockyModelNode>(ref blockyModel.AllNodes, blockyModel.NodeCount);
			Array.Resize<int>(ref blockyModel.ParentNodes, blockyModel.NodeCount);
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x001BF084 File Offset: 0x001BD284
		private static void RecurseParseNode(ref BlockyModelNodeJson jsonNode, BlockyModel model, int parentNodeIndex, NodeNameManager nodeNameManager)
		{
			ref NodeShape ptr = ref jsonNode.Shape;
			ref NodeShapeSettings ptr2 = ref ptr.Settings;
			int orAddNameId = nodeNameManager.GetOrAddNameId(jsonNode.Name);
			BlockyModelNode blockyModelNode = new BlockyModelNode
			{
				NameId = orAddNameId,
				Position = jsonNode.Position,
				Orientation = jsonNode.Orientation,
				Offset = ptr.Offset,
				Stretch = ptr.Stretch,
				Visible = ptr.Visible,
				DoubleSided = ptr.DoubleSided,
				ShadingMode = BlockyModelInitializer.ParseShadingMode(ptr.ShadingMode),
				Children = new List<int>()
			};
			Enum.TryParse<CameraNode>(jsonNode.Name.Replace("-", ""), out blockyModelNode.CameraNode);
			string type = ptr.Type;
			IDictionary<string, FaceLayout> textureLayout = ptr.TextureLayout;
			string text = type;
			string a = text;
			if (!(a == "none"))
			{
				if (!(a == "box"))
				{
					if (a == "quad")
					{
						blockyModelNode.Type = BlockyModelNode.ShapeType.Quad;
						blockyModelNode.Size = ptr2.Size;
						blockyModelNode.QuadNormalDirection = BlockyModelInitializer.ParseQuadNormal(ptr2.Normal);
						blockyModelNode.TextureLayout = new BlockyModelFaceTextureLayout[1];
						blockyModelNode.TextureLayout[0] = BlockyModelInitializer.GetFaceLayout(textureLayout, "front");
					}
				}
				else
				{
					blockyModelNode.Type = BlockyModelNode.ShapeType.Box;
					blockyModelNode.Size = ptr2.Size;
					blockyModelNode.TextureLayout = new BlockyModelFaceTextureLayout[6];
					blockyModelNode.TextureLayout[0] = BlockyModelInitializer.GetFaceLayout(textureLayout, "front");
					blockyModelNode.TextureLayout[1] = BlockyModelInitializer.GetFaceLayout(textureLayout, "back");
					blockyModelNode.TextureLayout[2] = BlockyModelInitializer.GetFaceLayout(textureLayout, "right");
					blockyModelNode.TextureLayout[3] = BlockyModelInitializer.GetFaceLayout(textureLayout, "bottom");
					blockyModelNode.TextureLayout[4] = BlockyModelInitializer.GetFaceLayout(textureLayout, "left");
					blockyModelNode.TextureLayout[5] = BlockyModelInitializer.GetFaceLayout(textureLayout, "top");
				}
			}
			else
			{
				blockyModelNode.Type = BlockyModelNode.ShapeType.None;
				blockyModelNode.IsPiece = ptr2.IsPiece;
			}
			int nodeCount = model.NodeCount;
			model.AddNode(ref blockyModelNode, parentNodeIndex);
			BlockyModelNodeJson[] children = jsonNode.Children;
			bool flag = children != null;
			if (flag)
			{
				for (int i = 0; i < children.Length; i++)
				{
					BlockyModelInitializer.RecurseParseNode(ref children[i], model, nodeCount, nodeNameManager);
				}
			}
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x001BF30C File Offset: 0x001BD50C
		private static LodMode ParseLodMode(string lodMode)
		{
			LodMode result;
			if (!(lodMode == "auto"))
			{
				if (!(lodMode == "billboard"))
				{
					if (!(lodMode == "disappear"))
					{
						result = LodMode.Off;
					}
					else
					{
						result = LodMode.Disappear;
					}
				}
				else
				{
					result = LodMode.Billboard;
				}
			}
			else
			{
				result = LodMode.Auto;
			}
			return result;
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x001BF358 File Offset: 0x001BD558
		private static ShadingMode ParseShadingMode(string shadingMode)
		{
			ShadingMode result;
			if (!(shadingMode == "flat"))
			{
				if (!(shadingMode == "fullbright"))
				{
					if (!(shadingMode == "reflective"))
					{
						if (!(shadingMode == "standard"))
						{
						}
						result = ShadingMode.Standard;
					}
					else
					{
						result = ShadingMode.Reflective;
					}
				}
				else
				{
					result = ShadingMode.Fullbright;
				}
			}
			else
			{
				result = ShadingMode.Flat;
			}
			return result;
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x001BF3B4 File Offset: 0x001BD5B4
		private static BlockyModelNode.QuadNormal ParseQuadNormal(string quadNormal)
		{
			BlockyModelNode.QuadNormal result;
			if (!(quadNormal == "-Z"))
			{
				if (!(quadNormal == "+X"))
				{
					if (!(quadNormal == "-X"))
					{
						if (!(quadNormal == "+Y"))
						{
							if (!(quadNormal == "-Y"))
							{
								if (!(quadNormal == "+Z"))
								{
								}
								result = BlockyModelNode.QuadNormal.PlusZ;
							}
							else
							{
								result = BlockyModelNode.QuadNormal.MinusY;
							}
						}
						else
						{
							result = BlockyModelNode.QuadNormal.PlusY;
						}
					}
					else
					{
						result = BlockyModelNode.QuadNormal.MinusX;
					}
				}
				else
				{
					result = BlockyModelNode.QuadNormal.PlusX;
				}
			}
			else
			{
				result = BlockyModelNode.QuadNormal.MinusZ;
			}
			return result;
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x001BF430 File Offset: 0x001BD630
		private static BlockyModelFaceTextureLayout GetFaceLayout(IDictionary<string, FaceLayout> jsonTextureLayout, string faceName)
		{
			FaceLayout faceLayout;
			bool flag = !jsonTextureLayout.TryGetValue(faceName, out faceLayout);
			BlockyModelFaceTextureLayout result;
			if (flag)
			{
				result = new BlockyModelFaceTextureLayout
				{
					Hidden = true
				};
			}
			else
			{
				result = new BlockyModelFaceTextureLayout
				{
					Angle = faceLayout.Angle,
					MirrorX = faceLayout.Mirror.X,
					MirrorY = faceLayout.Mirror.Y,
					Offset = faceLayout.Offset
				};
			}
			return result;
		}

		// Token: 0x04003843 RID: 14403
		private static readonly JsonSerializer JsonSerializerSettings = new JsonSerializer
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver()
		};
	}
}

using System;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Map;
using HytaleClient.Interface.InGame;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Utils
{
	// Token: 0x02000B92 RID: 2962
	internal static class ItemPreviewUtils
	{
		// Token: 0x06005B67 RID: 23399 RVA: 0x001C8BA0 File Offset: 0x001C6DA0
		public static void CreateBlockGeometry(ClientBlockType blockType, Texture texture)
		{
			float[] array = new float[16];
			float num = 0.05f;
			for (int i = 0; i < 16; i++)
			{
				array[i] = num + (float)Math.Pow((double)((float)i / 15f), 1.5) * (1f - num);
			}
			int[] cornerOcclusions = new int[4];
			UShortVector2[] texCoordsByCorner = new UShortVector2[4];
			UShortVector2[] sideMaskTexCoordsByCorner = new UShortVector2[4];
			ClientBlockType.ClientShaderEffect[] cornerShaderEffects = new ClientBlockType.ClientShaderEffect[4];
			uint[] array2 = new uint[1156];
			uint[] array3 = new uint[8];
			for (int j = 0; j < array3.Length; j++)
			{
				array3[j] = 301989887U;
			}
			int alphaTestedAnimatedLowLODIndicesStart = 0;
			int? num2 = new int?(0);
			int num3 = 32;
			int num4 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 1, 1);
			int[] array4 = new int[39304];
			ushort[] array5 = new ushort[39304];
			for (int k = 0; k < 39304; k++)
			{
				array5[k] = 61440;
			}
			uint num5 = (uint)blockType.SelfTintColorsBySide[0];
			array2[0] = num5;
			array2[1] = num5;
			array2[34] = num5;
			array2[35] = num5;
			array4[num4] = blockType.Id;
			ChunkGeometryBuilder.CreateBlockGeometry(new ClientBlockType[]
			{
				blockType
			}, array, blockType, num4, (float)num3, Vector3.Zero, 0, 0, 0, ref num2, byte.MaxValue, Matrix.Identity, blockType.RotationMatrix, blockType.CubeBlockInvertMatrix, texCoordsByCorner, sideMaskTexCoordsByCorner, cornerOcclusions, cornerShaderEffects, num5, array4, array5, array2, array3, texture.Width, texture.Height, blockType.VertexData, blockType.VertexData, alphaTestedAnimatedLowLODIndicesStart, ref alphaTestedAnimatedLowLODIndicesStart, true);
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x001C8D3C File Offset: 0x001C6F3C
		public static ClientBlockType ToClientBlockType(BlockType networkBlockType, JObject modelJson, NodeNameManager nodeNameManager)
		{
			ClientBlockType clientBlockType = new ClientBlockType
			{
				DrawType = networkBlockType.DrawType_,
				CubeTextures = new ClientBlockType.CubeTexture[6],
				RequiresAlphaBlending = networkBlockType.RequiresAlphaBlending,
				CubeSideMaskTexture = networkBlockType.CubeSideMaskTexture,
				VerticalFill = (byte)networkBlockType.VerticalFill,
				IsOccluder = false,
				CubeTextureWeights = new float[]
				{
					1f
				},
				BlockyModelScale = networkBlockType.ModelScale,
				SelfTintColorsBySide = new int[]
				{
					-1,
					-1,
					-1,
					-1,
					-1,
					-1
				},
				BiomeTintMultipliersBySide = new float[6],
				CubeShaderEffect = ClientBlockType.ClientShaderEffect.None,
				VertexData = new ChunkGeometryData()
			};
			clientBlockType.HasModel = (clientBlockType.DrawType == 3 || clientBlockType.DrawType == 4);
			clientBlockType.ShouldRenderCube = (clientBlockType.DrawType == 2 || clientBlockType.DrawType == 4 || clientBlockType.DrawType == 1);
			BlockTypeProtocolInitializer.ConvertShadingMode(networkBlockType.CubeShadingMode, out clientBlockType.CubeShadingMode);
			Tint tint_ = networkBlockType.Tint_;
			bool flag = tint_ != null;
			if (flag)
			{
				clientBlockType.SelfTintColorsBySide[0] = tint_.Top;
				clientBlockType.SelfTintColorsBySide[1] = tint_.Bottom;
				clientBlockType.SelfTintColorsBySide[2] = tint_.Left;
				clientBlockType.SelfTintColorsBySide[3] = tint_.Right;
				clientBlockType.SelfTintColorsBySide[4] = tint_.Front;
				clientBlockType.SelfTintColorsBySide[5] = tint_.Back;
			}
			bool flag2 = networkBlockType.CubeTextures != null;
			if (flag2)
			{
				int num = networkBlockType.CubeTextures.Length;
				for (int i = 0; i < 6; i++)
				{
					clientBlockType.CubeTextures[i] = new ClientBlockType.CubeTexture
					{
						Names = new string[num],
						TileLinearPositionsInAtlas = new int[Math.Max(1, num)]
					};
				}
				for (int j = 0; j < networkBlockType.CubeTextures.Length; j++)
				{
					BlockType.BlockTextures blockTextures = networkBlockType.CubeTextures[j];
					clientBlockType.CubeTextures[0].Names[j] = blockTextures.Top;
					clientBlockType.CubeTextures[1].Names[j] = blockTextures.Bottom;
					clientBlockType.CubeTextures[2].Names[j] = blockTextures.Left;
					clientBlockType.CubeTextures[3].Names[j] = blockTextures.Right;
					clientBlockType.CubeTextures[4].Names[j] = blockTextures.Front;
					clientBlockType.CubeTextures[5].Names[j] = blockTextures.Back;
				}
			}
			else
			{
				for (int k = 0; k < 6; k++)
				{
					clientBlockType.CubeTextures[k] = new ClientBlockType.CubeTexture
					{
						Names = new string[0],
						TileLinearPositionsInAtlas = new int[1]
					};
				}
			}
			bool hasModel = clientBlockType.HasModel;
			if (hasModel)
			{
				clientBlockType.BlockyTextures = new ClientBlockType.BlockyTexture[Math.Max(1, networkBlockType.ModelTexture_.Length)];
				clientBlockType.BlockyTextureWeights = new float[Math.Max(1, networkBlockType.ModelTexture_.Length)];
				bool flag3 = networkBlockType.ModelTexture_ != null;
				if (flag3)
				{
					for (int l = 0; l < networkBlockType.ModelTexture_.Length; l++)
					{
						BlockType.ModelTexture modelTexture = networkBlockType.ModelTexture_[l];
						bool flag4 = modelTexture == null;
						if (!flag4)
						{
							clientBlockType.BlockyTextures[l] = new ClientBlockType.BlockyTexture
							{
								Name = modelTexture.Texture
							};
							clientBlockType.BlockyTextureWeights[l] = modelTexture.Weight;
						}
					}
				}
				bool flag5 = networkBlockType.Model != null;
				if (flag5)
				{
					BlockyModel originalBlockyModel = new BlockyModel(BlockyModel.MaxNodeCount);
					BlockyModelInitializer.Parse(modelJson, nodeNameManager, ref originalBlockyModel);
					clientBlockType.OriginalBlockyModel = originalBlockyModel;
				}
				else
				{
					clientBlockType.OriginalBlockyModel = new BlockyModel(1);
					BlockyModelNode blockyModelNode = BlockyModelNode.CreateMapBlockNode(CharacterPartStore.BlockNameId, 16f, 1f);
					clientBlockType.OriginalBlockyModel.AddNode(ref blockyModelNode, -1);
				}
				clientBlockType.RenderedBlockyModel = new RenderedStaticBlockyModel(clientBlockType.OriginalBlockyModel);
				clientBlockType.VertexData.VerticesCount += clientBlockType.RenderedBlockyModel.AnimatedVertices.Length;
				clientBlockType.VertexData.IndicesCount += clientBlockType.RenderedBlockyModel.AnimatedIndices.Length;
			}
			clientBlockType.VertexData.VerticesCount += 24;
			clientBlockType.VertexData.IndicesCount += 36;
			clientBlockType.VertexData.Vertices = new ChunkVertex[clientBlockType.VertexData.VerticesCount];
			clientBlockType.VertexData.Indices = new uint[clientBlockType.VertexData.IndicesCount];
			bool flag6 = clientBlockType.RenderedBlockyModel != null;
			if (flag6)
			{
				for (int m = 0; m < clientBlockType.RenderedBlockyModel.AnimatedIndices.Length; m++)
				{
					clientBlockType.VertexData.Indices[clientBlockType.VertexData.IndicesOffset + m] = clientBlockType.VertexData.VerticesOffset + (uint)clientBlockType.RenderedBlockyModel.AnimatedIndices[m];
				}
				clientBlockType.VertexData.IndicesOffset += clientBlockType.RenderedBlockyModel.AnimatedIndices.Length;
			}
			clientBlockType.VertexData.VerticesOffset = 0U;
			clientBlockType.VertexData.IndicesOffset = 0;
			bool flag7 = clientBlockType.OriginalBlockyModel == null;
			if (flag7)
			{
				clientBlockType.OriginalBlockyModel = new BlockyModel(0);
			}
			clientBlockType.FinalBlockyModel = clientBlockType.OriginalBlockyModel;
			Matrix.CreateFromYawPitchRoll(MathHelper.RotationToRadians(clientBlockType.RotationYaw), MathHelper.RotationToRadians(clientBlockType.RotationPitch), 0f, out clientBlockType.RotationMatrix);
			Matrix.CreateScale(clientBlockType.BlockyModelScale * 0.03125f, out clientBlockType.BlockyModelTranslatedScaleMatrix);
			Matrix.AddTranslation(ref clientBlockType.BlockyModelTranslatedScaleMatrix, 0.5f, 0f, 0.5f);
			Matrix.Multiply(ref clientBlockType.BlockyModelTranslatedScaleMatrix, ref ChunkGeometryBuilder.NegativeHalfBlockOffsetMatrix, out clientBlockType.WorldMatrix);
			Matrix.Multiply(ref clientBlockType.WorldMatrix, ref clientBlockType.RotationMatrix, out clientBlockType.WorldMatrix);
			Matrix.Multiply(ref clientBlockType.WorldMatrix, ref ChunkGeometryBuilder.PositiveHalfBlockOffsetMatrix, out clientBlockType.WorldMatrix);
			Matrix.Invert(ref clientBlockType.WorldMatrix, out clientBlockType.CubeBlockInvertMatrix);
			Matrix.AddTranslation(ref clientBlockType.CubeBlockInvertMatrix, 0f, -16f, 0f);
			return clientBlockType;
		}

		// Token: 0x06005B69 RID: 23401 RVA: 0x001C9370 File Offset: 0x001C7570
		public static bool TryGetDefaultIconProperties(JObject json, out ClientItemIconProperties iconProperties)
		{
			try
			{
				ItemBase.ItemArmor.ItemArmorSlot? armorSlot = null;
				JToken jtoken = json["Armor"];
				bool flag;
				if (jtoken != null && jtoken.Type == 1)
				{
					JToken jtoken2 = json["Armor"]["ArmorSlot"];
					flag = (jtoken2 != null && jtoken2.Type == 8);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2)
				{
					ItemBase.ItemArmor.ItemArmorSlot value;
					bool flag3 = Enum.TryParse<ItemBase.ItemArmor.ItemArmorSlot>((string)json["Armor"]["ArmorSlot"], out value);
					if (flag3)
					{
						armorSlot = new ItemBase.ItemArmor.ItemArmorSlot?(value);
					}
				}
				JToken jtoken3 = json["Weapon"];
				bool isWeapon = jtoken3 != null && jtoken3.Type == 1;
				JToken jtoken4 = json["Tool"];
				iconProperties = IconHelper.GetDefaultIconProperties(isWeapon, jtoken4 != null && jtoken4.Type == 1, armorSlot != null, armorSlot);
			}
			catch (Exception exception)
			{
				ItemPreviewUtils.Logger.Error(exception, "Failed to get icon properties");
				iconProperties = null;
				return false;
			}
			return true;
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x001C947C File Offset: 0x001C767C
		public static bool TryGetIconProperties(JObject json, out ClientItemIconProperties iconProperties)
		{
			bool flag = !ItemPreviewUtils.TryGetDefaultIconProperties(json, out iconProperties);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				try
				{
					JToken jtoken = json["IconProperties"];
					bool flag2 = jtoken != null && jtoken.Type == 1;
					if (flag2)
					{
						JObject jobject = (JObject)json["IconProperties"];
						iconProperties.Scale = (float)jobject["Scale"];
						bool flag3 = jobject.ContainsKey("Rotation");
						if (flag3)
						{
							iconProperties.Rotation = new Vector3?(new Vector3((float)jobject["Rotation"][0], (float)jobject["Rotation"][1], (float)jobject["Rotation"][2]));
						}
						bool flag4 = jobject.ContainsKey("Translation");
						if (flag4)
						{
							iconProperties.Translation = new Vector2?(new Vector2((float)jobject["Translation"][0], (float)jobject["Translation"][1]));
						}
					}
				}
				catch (Exception exception)
				{
					ItemPreviewUtils.Logger.Error(exception, "Failed to get icon properties");
					iconProperties = null;
					return false;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0400393B RID: 14651
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}

using System;
using System.Collections.Generic;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Map;
using HytaleClient.Graphics.Particles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data.Map
{
	// Token: 0x02000AE0 RID: 2784
	internal class ClientBlockType
	{
		// Token: 0x060057A0 RID: 22432 RVA: 0x001A9CAC File Offset: 0x001A7EAC
		public static string GetOriginalBlockName(string name)
		{
			int num = name.IndexOf('|');
			return (num == -1) ? name : name.Substring(0, num);
		}

		// Token: 0x060057A1 RID: 22433 RVA: 0x001A9CD8 File Offset: 0x001A7ED8
		public static Dictionary<string, string> GetBlockVariantData(string name)
		{
			string[] array = name.Split(new char[]
			{
				'|'
			});
			bool flag = array.Length < 2;
			Dictionary<string, string> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				for (int i = 1; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[]
					{
						'='
					});
					bool flag2 = array2.Length < 2;
					if (flag2)
					{
						throw new Exception(string.Format("Invalid variant data for block '{0}' - Missing value separator character '{1}'", name, '='));
					}
					dictionary.Add(array2[0], array2[1]);
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x001A9D76 File Offset: 0x001A7F76
		public bool IsAnimated()
		{
			return this.BlockyAnimation != null;
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x001A9D84 File Offset: 0x001A7F84
		public bool IsConnectable()
		{
			BlockType.BlockConnections connections = this.Connections;
			return ((connections != null) ? connections.ConnectableBlocks : null) != null && this.Connections.ConnectableBlocks.Length != 0 && this.Connections.Outputs != null && this.Connections.Outputs.Count > 0;
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x001A9DD8 File Offset: 0x001A7FD8
		public int TryGetRotatedVariant(Rotation yaw = 0, Rotation pitch = 0, Rotation roll = 0)
		{
			string text = "";
			bool flag = yaw > 0;
			if (flag)
			{
				text += string.Format("Yaw={0}", yaw * 90);
			}
			bool flag2 = pitch > 0;
			if (flag2)
			{
				bool flag3 = text != "";
				if (flag3)
				{
					text += "|";
				}
				text += string.Format("Pitch={0}", pitch * 90);
			}
			bool flag4 = roll > 0;
			if (flag4)
			{
				bool flag5 = text != "";
				if (flag5)
				{
					text += "|";
				}
				text += string.Format("Roll={0}", roll * 90);
			}
			bool flag6 = text == "";
			int result;
			if (flag6)
			{
				result = this.Id;
			}
			else
			{
				int num;
				result = (this.Variants.TryGetValue(text, out num) ? num : this.Id);
			}
			return result;
		}

		// Token: 0x040035F6 RID: 13814
		public const int EmptyBlockId = 0;

		// Token: 0x040035F7 RID: 13815
		public const int UnknownBlockId = 1;

		// Token: 0x040035F8 RID: 13816
		public const int UndefinedBlockId = 2147483647;

		// Token: 0x040035F9 RID: 13817
		public const string EmptyBlockName = "Empty";

		// Token: 0x040035FA RID: 13818
		public const string UnknownBlockName = "Unknown";

		// Token: 0x040035FB RID: 13819
		public const char VariantTypeSeparator = '|';

		// Token: 0x040035FC RID: 13820
		public const char VariantValueSeparator = '=';

		// Token: 0x040035FD RID: 13821
		public const string NullStateId = "default";

		// Token: 0x040035FE RID: 13822
		public const float MaxBlockHealth = 1f;

		// Token: 0x040035FF RID: 13823
		public const byte MaxVerticalFill = 8;

		// Token: 0x04003600 RID: 13824
		public int Id;

		// Token: 0x04003601 RID: 13825
		public string Name;

		// Token: 0x04003602 RID: 13826
		public string Item;

		// Token: 0x04003603 RID: 13827
		public bool Unknown;

		// Token: 0x04003604 RID: 13828
		public BlockType.DrawType DrawType;

		// Token: 0x04003605 RID: 13829
		public int FillerX;

		// Token: 0x04003606 RID: 13830
		public int FillerY;

		// Token: 0x04003607 RID: 13831
		public int FillerZ;

		// Token: 0x04003608 RID: 13832
		public Rotation RotationYaw;

		// Token: 0x04003609 RID: 13833
		public Rotation RotationPitch;

		// Token: 0x0400360A RID: 13834
		public Rotation RotationRoll;

		// Token: 0x0400360B RID: 13835
		public BlockType.RandomRotation RandomRotation;

		// Token: 0x0400360C RID: 13836
		public Rotation RotationYawPlacementOffset;

		// Token: 0x0400360D RID: 13837
		public bool ShouldRenderCube;

		// Token: 0x0400360E RID: 13838
		public bool RequiresAlphaBlending;

		// Token: 0x0400360F RID: 13839
		public bool IsOccluder;

		// Token: 0x04003610 RID: 13840
		public bool HasModel;

		// Token: 0x04003611 RID: 13841
		public byte VerticalFill;

		// Token: 0x04003612 RID: 13842
		public byte MaxFillLevel;

		// Token: 0x04003613 RID: 13843
		public BlockType.Opacity Opacity;

		// Token: 0x04003614 RID: 13844
		public ClientBlockType.CubeTexture[] CubeTextures;

		// Token: 0x04003615 RID: 13845
		public float[] CubeTextureWeights;

		// Token: 0x04003616 RID: 13846
		public string CubeSideMaskTexture;

		// Token: 0x04003617 RID: 13847
		public int CubeSideMaskTextureAtlasIndex = -1;

		// Token: 0x04003618 RID: 13848
		public ShadingMode CubeShadingMode;

		// Token: 0x04003619 RID: 13849
		public string TransitionTexture;

		// Token: 0x0400361A RID: 13850
		public int TransitionTextureAtlasIndex = -1;

		// Token: 0x0400361B RID: 13851
		public int TransitionGroupId = -1;

		// Token: 0x0400361C RID: 13852
		public int[] TransitionToGroupIds;

		// Token: 0x0400361D RID: 13853
		public ClientBlockType.BlockyTexture[] BlockyTextures;

		// Token: 0x0400361E RID: 13854
		public float[] BlockyTextureWeights;

		// Token: 0x0400361F RID: 13855
		public string BlockyModelHash;

		// Token: 0x04003620 RID: 13856
		public float BlockyModelScale = 1f;

		// Token: 0x04003621 RID: 13857
		public BlockyModel OriginalBlockyModel;

		// Token: 0x04003622 RID: 13858
		public BlockyModel FinalBlockyModel;

		// Token: 0x04003623 RID: 13859
		public BlockyAnimation BlockyAnimation;

		// Token: 0x04003624 RID: 13860
		public ModelParticleSettings[] Particles;

		// Token: 0x04003625 RID: 13861
		public RenderedStaticBlockyModel RenderedBlockyModel;

		// Token: 0x04003626 RID: 13862
		public Vector2[] RenderedBlockyModelTextureOrigins;

		// Token: 0x04003627 RID: 13863
		public ChunkGeometryData VertexData;

		// Token: 0x04003628 RID: 13864
		public Matrix WorldMatrix;

		// Token: 0x04003629 RID: 13865
		public Matrix RotationMatrix;

		// Token: 0x0400362A RID: 13866
		public Matrix BlockyModelTranslatedScaleMatrix;

		// Token: 0x0400362B RID: 13867
		public Matrix CubeBlockInvertMatrix;

		// Token: 0x0400362C RID: 13868
		public int[] SelfTintColorsBySide;

		// Token: 0x0400362D RID: 13869
		public float[] BiomeTintMultipliersBySide;

		// Token: 0x0400362E RID: 13870
		public ColorRgb LightEmitted;

		// Token: 0x0400362F RID: 13871
		public BlockType.Material CollisionMaterial;

		// Token: 0x04003630 RID: 13872
		public int HitboxType;

		// Token: 0x04003631 RID: 13873
		public BlockType.BlockMovementSettings MovementSettings;

		// Token: 0x04003632 RID: 13874
		public bool IsUsable;

		// Token: 0x04003633 RID: 13875
		public string InteractionHint;

		// Token: 0x04003634 RID: 13876
		public BlockType.BlockGathering Gathering;

		// Token: 0x04003635 RID: 13877
		public ClientBlockType.ClientShaderEffect CubeShaderEffect;

		// Token: 0x04003636 RID: 13878
		public ClientBlockType.ClientShaderEffect BlockyModelShaderEffect;

		// Token: 0x04003637 RID: 13879
		public int FluidBlockId;

		// Token: 0x04003638 RID: 13880
		public int FluidFXIndex;

		// Token: 0x04003639 RID: 13881
		public readonly Dictionary<string, int> Variants = new Dictionary<string, int>();

		// Token: 0x0400363A RID: 13882
		public string BlockParticleSetId;

		// Token: 0x0400363B RID: 13883
		public UInt32Color ParticleColor = UInt32Color.Transparent;

		// Token: 0x0400363C RID: 13884
		public int BlockSoundSetIndex;

		// Token: 0x0400363D RID: 13885
		public Dictionary<InteractionType, int> Interactions;

		// Token: 0x0400363E RID: 13886
		public BlockType.VariantRotation VariantRotation;

		// Token: 0x0400363F RID: 13887
		public int VariantOriginalId;

		// Token: 0x04003640 RID: 13888
		public BlockType.BlockConnections Connections;

		// Token: 0x04003641 RID: 13889
		public uint SoundEventIndex;

		// Token: 0x04003642 RID: 13890
		public bool Looping;

		// Token: 0x04003643 RID: 13891
		public Dictionary<string, int> States;

		// Token: 0x04003644 RID: 13892
		public Dictionary<int, string> StatesReverse;

		// Token: 0x04003645 RID: 13893
		public Dictionary<int, int[]> TagIndexes;

		// Token: 0x02000F1C RID: 3868
		public enum ClientShaderEffect
		{
			// Token: 0x04004A1B RID: 18971
			None = 31,
			// Token: 0x04004A1C RID: 18972
			ParamOn,
			// Token: 0x04004A1D RID: 18973
			WindAttached = 0,
			// Token: 0x04004A1E RID: 18974
			WindAttachedMax = 14,
			// Token: 0x04004A1F RID: 18975
			Wind,
			// Token: 0x04004A20 RID: 18976
			Ice,
			// Token: 0x04004A21 RID: 18977
			Water,
			// Token: 0x04004A22 RID: 18978
			WaterEnvironmentColor,
			// Token: 0x04004A23 RID: 18979
			WaterEnvironmentTransition,
			// Token: 0x04004A24 RID: 18980
			Lava,
			// Token: 0x04004A25 RID: 18981
			Slime,
			// Token: 0x04004A26 RID: 18982
			Ripple
		}

		// Token: 0x02000F1D RID: 3869
		public class CubeTexture
		{
			// Token: 0x04004A27 RID: 18983
			public string[] Names;

			// Token: 0x04004A28 RID: 18984
			public int[] TileLinearPositionsInAtlas;

			// Token: 0x04004A29 RID: 18985
			public int Rotation;
		}

		// Token: 0x02000F1E RID: 3870
		public class BlockyTexture
		{
			// Token: 0x04004A2A RID: 18986
			public string Name;

			// Token: 0x04004A2B RID: 18987
			public string Hash;
		}
	}
}

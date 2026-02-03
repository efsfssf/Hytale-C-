using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Brush
{
	// Token: 0x02000990 RID: 2448
	internal class BrushData
	{
		// Token: 0x06004E15 RID: 19989 RVA: 0x001575A8 File Offset: 0x001557A8
		public BrushData(ClientItemStack item = null, BuilderTool builderTool = null, Action<string, string> onDataChange = null)
		{
			bool flag = item == null;
			if (!flag)
			{
				bool flag2 = item.Metadata == null;
				if (flag2)
				{
					JObject jobject = new JObject();
					jobject.Add("BrushData", new JObject());
					item.Metadata = jobject;
				}
				JToken jtoken;
				bool flag3 = item.Metadata.TryGetValue("BrushData", ref jtoken);
				if (flag3)
				{
					JToken jtoken2;
					if ((jtoken2 = jtoken["Width"]) == null)
					{
						jtoken2 = ((builderTool != null) ? builderTool.ToolItem.BrushData.Width.Default_ : 5);
					}
					this.Width = (int)jtoken2;
					JToken jtoken3;
					if ((jtoken3 = jtoken["Height"]) == null)
					{
						jtoken3 = ((builderTool != null) ? builderTool.ToolItem.BrushData.Height.Default_ : 5);
					}
					this.Height = (int)jtoken3;
					JToken jtoken4;
					if ((jtoken4 = jtoken["Thickness"]) == null)
					{
						jtoken4 = ((builderTool != null) ? builderTool.ToolItem.BrushData.Thickness.Default_ : this.Thickness);
					}
					this.Thickness = (int)jtoken4;
					JToken jtoken5;
					if ((jtoken5 = jtoken["Capped"]) == null)
					{
						jtoken5 = ((builderTool != null) ? builderTool.ToolItem.BrushData.Capped.Default_ : this.Capped);
					}
					this.Capped = (bool)jtoken5;
					BrushShape brushShape;
					this.Shape = (Enum.TryParse<BrushShape>((string)jtoken["Shape"], true, out brushShape) ? brushShape : ((builderTool != null) ? builderTool.ToolItem.BrushData.Shape.Default_ : this.Shape));
					BrushOrigin brushOrigin;
					this.Origin = (Enum.TryParse<BrushOrigin>((string)jtoken["Origin"], true, out brushOrigin) ? brushOrigin : ((builderTool != null) ? builderTool.ToolItem.BrushData.Origin.Default_ : this.Origin));
					JToken jtoken6;
					if ((jtoken6 = jtoken["OriginRotation"]) == null)
					{
						jtoken6 = ((builderTool != null) ? builderTool.ToolItem.BrushData.OriginRotation.Default_ : this.OriginRotation);
					}
					this.OriginRotation = (bool)jtoken6;
					BrushAxis brushAxis;
					this.RotationAxis = (Enum.TryParse<BrushAxis>((string)jtoken["RotationAxis"], true, out brushAxis) ? brushAxis : ((builderTool != null) ? builderTool.ToolItem.BrushData.RotationAxis.Default_ : this.RotationAxis));
					Rotation rotation;
					this.RotationAngle = (Enum.TryParse<Rotation>((string)jtoken["RotationAngle"], true, out rotation) ? rotation : ((builderTool != null) ? builderTool.ToolItem.BrushData.RotationAngle.Default_ : this.RotationAngle));
					BrushAxis brushAxis2;
					this.MirrorAxis = (Enum.TryParse<BrushAxis>((string)jtoken["MirrorAxis"], true, out brushAxis2) ? brushAxis2 : ((builderTool != null) ? builderTool.ToolItem.BrushData.MirrorAxis.Default_ : this.MirrorAxis));
					JToken jtoken7;
					if ((jtoken7 = jtoken["Material"]) == null)
					{
						jtoken7 = (((builderTool != null) ? builderTool.ToolItem.BrushData.Material.Default_ : null) ?? this.Material);
					}
					this.Material = (string)jtoken7;
					bool flag4 = jtoken["FavoriteMaterials"] != null;
					if (flag4)
					{
						this.FavoriteMaterials = jtoken["FavoriteMaterials"].ToObject<string[]>();
					}
					else
					{
						bool flag5 = builderTool != null && builderTool.ToolItem.BrushData.FavoriteMaterials.Length != 0;
						if (flag5)
						{
							this.FavoriteMaterials = Array.ConvertAll<BuilderToolBlockArg, string>(builderTool.ToolItem.BrushData.FavoriteMaterials, (BuilderToolBlockArg b) => b.ToString());
						}
					}
					JToken jtoken8;
					if ((jtoken8 = jtoken["Mask"]) == null)
					{
						jtoken8 = (((builderTool != null) ? builderTool.ToolItem.BrushData.Mask.Default_ : null) ?? this.Mask);
					}
					this.Mask = (string)jtoken8;
					JToken jtoken9;
					if ((jtoken9 = jtoken["MaskAbove"]) == null)
					{
						jtoken9 = (((builderTool != null) ? builderTool.ToolItem.BrushData.MaskAbove.Default_ : null) ?? this.MaskAbove);
					}
					this.MaskAbove = (string)jtoken9;
					JToken jtoken10;
					if ((jtoken10 = jtoken["MaskNot"]) == null)
					{
						jtoken10 = (((builderTool != null) ? builderTool.ToolItem.BrushData.MaskNot.Default_ : null) ?? this.MaskNot);
					}
					this.MaskNot = (string)jtoken10;
					JToken jtoken11;
					if ((jtoken11 = jtoken["MaskBelow"]) == null)
					{
						jtoken11 = (((builderTool != null) ? builderTool.ToolItem.BrushData.MaskBelow.Default_ : null) ?? this.MaskBelow);
					}
					this.MaskBelow = (string)jtoken11;
					JToken jtoken12;
					if ((jtoken12 = jtoken["MaskAdjacent"]) == null)
					{
						jtoken12 = (((builderTool != null) ? builderTool.ToolItem.BrushData.MaskAdjacent.Default_ : null) ?? this.MaskAdjacent);
					}
					this.MaskAdjacent = (string)jtoken12;
					JToken jtoken13;
					if ((jtoken13 = jtoken["MaskNeighbor"]) == null)
					{
						jtoken13 = (((builderTool != null) ? builderTool.ToolItem.BrushData.MaskNeighbor.Default_ : null) ?? this.MaskNeighbor);
					}
					this.MaskNeighbor = (string)jtoken13;
					bool flag6 = jtoken["MaskCommands"] != null;
					if (flag6)
					{
						this.MaskCommands = jtoken["MaskCommands"].ToObject<string[]>();
					}
					else
					{
						bool flag7 = builderTool != null && builderTool.ToolItem.BrushData.MaskCommands.Length != 0;
						if (flag7)
						{
							this.MaskCommands = Array.ConvertAll<BuilderToolStringArg, string>(builderTool.ToolItem.BrushData.MaskCommands, (BuilderToolStringArg b) => b.ToString());
						}
					}
					JToken jtoken14;
					if ((jtoken14 = jtoken["UseMaskCommands"]) == null)
					{
						jtoken14 = ((builderTool != null) ? builderTool.ToolItem.BrushData.UseMaskCommands.Default_ : this.UseMaskCommands);
					}
					this.UseMaskCommands = (bool)jtoken14;
				}
				this._onDataChange = onDataChange;
			}
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x00157BD7 File Offset: 0x00155DD7
		public void SetFavoriteMaterials(string[] materials)
		{
			Action<string, string> onDataChange = this._onDataChange;
			if (onDataChange != null)
			{
				onDataChange("FavoriteMaterials", string.Join(",", materials));
			}
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x00157BFC File Offset: 0x00155DFC
		public ClientItemStack[] GetFavoriteMaterialStacks()
		{
			bool flag = this.FavoriteMaterials == null;
			ClientItemStack[] result;
			if (flag)
			{
				result = this.EmptyClientItemStackArray;
			}
			else
			{
				ClientItemStack[] array = new ClientItemStack[this.FavoriteMaterials.Length];
				for (int i = 0; i < this.FavoriteMaterials.Length; i++)
				{
					array[i] = new ClientItemStack(this.FavoriteMaterials[i], 1);
				}
				result = array;
			}
			return result;
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x00157C60 File Offset: 0x00155E60
		public void SetBrushWidth(int width)
		{
			bool flag = this.Width != width;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("Width", width.ToString());
				}
			}
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x00157C9C File Offset: 0x00155E9C
		public void SetBrushHeight(int height)
		{
			bool flag = this.Height != height;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("Height", height.ToString());
				}
			}
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x00157CD8 File Offset: 0x00155ED8
		public void SetBrushThickness(int thickness)
		{
			bool flag = this.Thickness != thickness;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("Thickness", thickness.ToString());
				}
			}
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x00157D14 File Offset: 0x00155F14
		public void SetCapped(bool capped)
		{
			bool flag = this.Capped != capped;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("Capped", capped.ToString());
				}
			}
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x00157D50 File Offset: 0x00155F50
		public void SetBrushShape(BrushShape brushShape)
		{
			bool flag = this.Shape != brushShape;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("Shape", brushShape.ToString());
				}
			}
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x00157D94 File Offset: 0x00155F94
		public void SetBrushOrigin(BrushOrigin brushOrigin)
		{
			bool flag = this.Origin != brushOrigin;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("Origin", brushOrigin.ToString());
				}
			}
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x00157DD8 File Offset: 0x00155FD8
		public void SetOriginRotation(bool originRotation)
		{
			bool flag = this.OriginRotation != originRotation;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("OriginRotation", originRotation.ToString());
				}
			}
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x00157E14 File Offset: 0x00156014
		public void SetRotationAxis(BrushAxis brushAxis)
		{
			bool flag = this.RotationAxis != brushAxis;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("RotationAxis", brushAxis.ToString());
				}
			}
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x00157E58 File Offset: 0x00156058
		public void SetRotationAngle(Rotation rotation)
		{
			bool flag = this.RotationAngle != rotation;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("RotationAngle", rotation.ToString());
				}
			}
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x00157E9C File Offset: 0x0015609C
		public void SetMirrorAxis(BrushAxis brushAxis)
		{
			bool flag = this.MirrorAxis != brushAxis;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("MirrorAxis", brushAxis.ToString());
				}
			}
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x00157EE0 File Offset: 0x001560E0
		public void SetBrushMaterial(string material)
		{
			bool flag = material != this.Material;
			if (flag)
			{
				Action<string, string> onDataChange = this._onDataChange;
				if (onDataChange != null)
				{
					onDataChange("Material", material.ToString());
				}
			}
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x00157F1C File Offset: 0x0015611C
		public bool OffsetBrushWidth(int offset)
		{
			bool flag = offset != 0;
			if (flag)
			{
				int num = this.Width + offset;
				bool flag2 = num > 0;
				if (flag2)
				{
					this.SetBrushWidth(num);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x00157F58 File Offset: 0x00156158
		public bool OffsetBrushHeight(int offset)
		{
			bool flag = offset != 0;
			if (flag)
			{
				int num = this.Height + offset;
				bool flag2 = num > 0;
				if (flag2)
				{
					this.SetBrushHeight(num);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x00157F94 File Offset: 0x00156194
		public void InvertBrushShape()
		{
			switch (this.Shape)
			{
			case 3:
				this.SetBrushShape(4);
				break;
			case 4:
				this.SetBrushShape(3);
				break;
			case 5:
				this.SetBrushShape(6);
				break;
			case 6:
				this.SetBrushShape(5);
				break;
			}
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x00157FF0 File Offset: 0x001561F0
		public BrushOrigin NextBrushOrigin(bool moveForward = true)
		{
			bool flag = moveForward && this.Origin == 2;
			BrushOrigin brushOrigin;
			if (flag)
			{
				brushOrigin = 0;
			}
			else
			{
				bool flag2 = !moveForward && this.Origin == 0;
				if (flag2)
				{
					brushOrigin = 2;
				}
				else
				{
					brushOrigin = this.Origin + (moveForward ? 1 : -1);
				}
			}
			this.SetBrushOrigin(brushOrigin);
			return brushOrigin;
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x00158048 File Offset: 0x00156248
		public BrushShape NextBrushShape(bool moveForward = true)
		{
			bool flag = moveForward && this.Shape == 7;
			BrushShape brushShape;
			if (flag)
			{
				brushShape = 0;
			}
			else
			{
				bool flag2 = !moveForward && this.Shape == 0;
				if (flag2)
				{
					brushShape = 7;
				}
				else
				{
					brushShape = this.Shape + (moveForward ? 1 : -1);
				}
			}
			this.SetBrushShape(brushShape);
			return brushShape;
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x001580A0 File Offset: 0x001562A0
		public Dictionary<string, string> ToArgValues()
		{
			return new Dictionary<string, string>
			{
				{
					"Width",
					this.Width.ToString()
				},
				{
					"Height",
					this.Height.ToString()
				},
				{
					"Thickness",
					this.Thickness.ToString()
				},
				{
					"Capped",
					this.Capped.ToString()
				},
				{
					"Shape",
					this.Shape.ToString()
				},
				{
					"Origin",
					this.Origin.ToString()
				},
				{
					"OriginRotation",
					this.OriginRotation.ToString()
				},
				{
					"RotationAxis",
					this.RotationAxis.ToString()
				},
				{
					"RotationAngle",
					this.RotationAngle.ToString()
				},
				{
					"MirrorAxis",
					this.MirrorAxis.ToString()
				},
				{
					"Material",
					this.Material ?? ""
				},
				{
					"FavoriteMaterials",
					(this.FavoriteMaterials == null) ? "" : string.Join(",", this.FavoriteMaterials)
				},
				{
					"Mask",
					this.Mask ?? ""
				},
				{
					"MaskAbove",
					this.MaskAbove ?? ""
				},
				{
					"MaskNot",
					this.MaskNot ?? ""
				},
				{
					"MaskBelow",
					this.MaskBelow ?? ""
				},
				{
					"MaskAdjacent",
					this.MaskAdjacent ?? ""
				},
				{
					"MaskNeighbor",
					this.MaskNeighbor ?? ""
				},
				{
					"MaskCommands",
					(this.MaskCommands == null) ? "" : string.Join(Environment.NewLine, this.MaskCommands)
				},
				{
					"UseMaskCommands",
					this.UseMaskCommands.ToString()
				}
			};
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x00158310 File Offset: 0x00156510
		public bool Equals(BrushData other)
		{
			return other != null && other.Width == this.Width && other.Height == this.Height && other.Thickness == this.Thickness && other.Capped == this.Capped && other.Shape == this.Shape && other.Origin == this.Origin && other.OriginRotation == this.OriginRotation && other.RotationAxis == this.RotationAxis && other.RotationAngle == this.RotationAngle && other.MirrorAxis == this.MirrorAxis && other.Material == this.Material && other.FavoriteMaterials == this.FavoriteMaterials && other.Mask == this.Mask && other.MaskAbove == this.MaskAbove && other.MaskNot == this.MaskNot && other.MaskBelow == this.MaskBelow && other.MaskAdjacent == this.MaskAdjacent && other.MaskNeighbor == this.MaskNeighbor && other.MaskCommands == this.MaskCommands && other.UseMaskCommands == this.UseMaskCommands;
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x0015848C File Offset: 0x0015668C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Brush Args\n",
				string.Format("  {0}:  {1}\n", "Width", this.Width),
				string.Format("  {0}:  {1}\n", "Height", this.Height),
				string.Format("  {0}:  {1}\n", "Thickness", this.Thickness),
				string.Format("  {0}:  {1}\n", "Capped", this.Capped),
				string.Format("  {0}:  {1}\n", "Shape", this.Shape),
				string.Format("  {0}:  {1}\n", "Origin", this.Origin),
				string.Format("  {0}:  {1}\n", "OriginRotation", this.OriginRotation),
				string.Format("  {0}:  {1}\n", "RotationAxis", this.RotationAxis),
				string.Format("  {0}:  {1}\n", "RotationAngle", this.RotationAngle),
				string.Format("  {0}:  {1}\n", "MirrorAxis", this.MirrorAxis),
				"  Material:  ",
				this.Material,
				"\n",
				string.Format("  {0}:  {1}\n", "FavoriteMaterials", this.FavoriteMaterials),
				"  Mask:  ",
				this.Mask,
				"\n  MaskAbove:  ",
				this.MaskAbove,
				"\n  MaskNot:  ",
				this.MaskNot,
				"\n  MaskBelow:  ",
				this.MaskBelow,
				"\n  MaskAdjacent:  ",
				this.MaskAdjacent,
				"\n  MaskNeighbor:  ",
				this.MaskNeighbor,
				"\n",
				string.Format("  {0}:  {1}\n", "MaskCommands", this.MaskCommands),
				string.Format("  {0}:  {1}\n", "UseMaskCommands", this.UseMaskCommands)
			});
		}

		// Token: 0x04002930 RID: 10544
		public const string MaterialKey = "Material";

		// Token: 0x04002931 RID: 10545
		public const string FavoriteMaterialsKey = "FavoriteMaterials";

		// Token: 0x04002932 RID: 10546
		public const string WidthKey = "Width";

		// Token: 0x04002933 RID: 10547
		public const string HeightKey = "Height";

		// Token: 0x04002934 RID: 10548
		public const string ThicknessKey = "Thickness";

		// Token: 0x04002935 RID: 10549
		public const string CappedKey = "Capped";

		// Token: 0x04002936 RID: 10550
		public const string ShapeKey = "Shape";

		// Token: 0x04002937 RID: 10551
		public const string OriginKey = "Origin";

		// Token: 0x04002938 RID: 10552
		public const string OriginRotationKey = "OriginRotation";

		// Token: 0x04002939 RID: 10553
		public const string RotationAxisKey = "RotationAxis";

		// Token: 0x0400293A RID: 10554
		public const string RotationAngleKey = "RotationAngle";

		// Token: 0x0400293B RID: 10555
		public const string MirrorAxisKey = "MirrorAxis";

		// Token: 0x0400293C RID: 10556
		public const string MaskKey = "Mask";

		// Token: 0x0400293D RID: 10557
		public const string MaskAboveKey = "MaskAbove";

		// Token: 0x0400293E RID: 10558
		public const string MaskNotKey = "MaskNot";

		// Token: 0x0400293F RID: 10559
		public const string MaskBelowKey = "MaskBelow";

		// Token: 0x04002940 RID: 10560
		public const string MaskAdjacentKey = "MaskAdjacent";

		// Token: 0x04002941 RID: 10561
		public const string MaskNeighborKey = "MaskNeighbor";

		// Token: 0x04002942 RID: 10562
		public const string MaskCommandsKey = "MaskCommands";

		// Token: 0x04002943 RID: 10563
		public const string UseMaskCommandsKey = "UseMaskCommands";

		// Token: 0x04002944 RID: 10564
		public const int MaskMaxNumber = 7;

		// Token: 0x04002945 RID: 10565
		public readonly ClientItemStack[] EmptyClientItemStackArray = new ClientItemStack[0];

		// Token: 0x04002946 RID: 10566
		public readonly string Material;

		// Token: 0x04002947 RID: 10567
		public readonly string[] FavoriteMaterials;

		// Token: 0x04002948 RID: 10568
		public readonly int Width;

		// Token: 0x04002949 RID: 10569
		public readonly int Height;

		// Token: 0x0400294A RID: 10570
		public readonly int Thickness;

		// Token: 0x0400294B RID: 10571
		public readonly bool Capped;

		// Token: 0x0400294C RID: 10572
		public readonly BrushShape Shape;

		// Token: 0x0400294D RID: 10573
		public readonly BrushOrigin Origin;

		// Token: 0x0400294E RID: 10574
		public readonly bool OriginRotation;

		// Token: 0x0400294F RID: 10575
		public readonly BrushAxis RotationAxis;

		// Token: 0x04002950 RID: 10576
		public readonly Rotation RotationAngle;

		// Token: 0x04002951 RID: 10577
		public readonly BrushAxis MirrorAxis;

		// Token: 0x04002952 RID: 10578
		public readonly string Mask;

		// Token: 0x04002953 RID: 10579
		public readonly string MaskAbove;

		// Token: 0x04002954 RID: 10580
		public readonly string MaskNot;

		// Token: 0x04002955 RID: 10581
		public readonly string MaskBelow;

		// Token: 0x04002956 RID: 10582
		public readonly string MaskAdjacent;

		// Token: 0x04002957 RID: 10583
		public readonly string MaskNeighbor;

		// Token: 0x04002958 RID: 10584
		public readonly string[] MaskCommands;

		// Token: 0x04002959 RID: 10585
		public readonly bool UseMaskCommands;

		// Token: 0x0400295A RID: 10586
		private const int DefaultWidth = 5;

		// Token: 0x0400295B RID: 10587
		private const int DefaultHeight = 5;

		// Token: 0x0400295C RID: 10588
		private readonly Action<string, string> _onDataChange;
	}
}

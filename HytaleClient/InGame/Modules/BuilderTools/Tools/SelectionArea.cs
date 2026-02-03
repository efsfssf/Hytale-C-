using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Client;
using HytaleClient.Math;
using HytaleClient.Protocol;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools
{
	// Token: 0x0200097F RID: 2431
	public class SelectionArea : IEnumerable<Vector3>, IEnumerable
	{
		// Token: 0x06004D26 RID: 19750 RVA: 0x0014A85E File Offset: 0x00148A5E
		public bool NeedsDrawing()
		{
			return this.IsSelectionDefined() || this.IsAnySelectionDefined();
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0014A871 File Offset: 0x00148A71
		public bool NeedsTextDrawing()
		{
			return this.NeedsDrawing();
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x0014A87C File Offset: 0x00148A7C
		internal SelectionArea(GameInstance gameInstance)
		{
			GraphicsDevice graphics = gameInstance.Engine.Graphics;
			this._gameInstance = gameInstance;
			this.Renderer = new SelectionToolRenderer(graphics, gameInstance.App.Fonts.DefaultFontFamily.RegularFont);
			this.BoxRenderer = new BoxRenderer(graphics, graphics.GPUProgramStore.BasicProgram);
			this.SelectionColors = new Vector3[]
			{
				graphics.WhiteColor,
				graphics.RedColor,
				graphics.GreenColor,
				graphics.BlueColor,
				graphics.CyanColor,
				graphics.MagentaColor,
				graphics.YellowColor,
				graphics.BlackColor
			};
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x0014A980 File Offset: 0x00148B80
		public void Update()
		{
			bool flag = !this.IsSelectionDirty;
			if (!flag)
			{
				this.Renderer.UpdateSelection(this.Position1, this.Position2);
				this.IsSelectionDirty = false;
				int num = (int)MathHelper.Min(this.Position1.X, this.Position2.X);
				int num2 = (int)MathHelper.Min(this.Position1.Y, this.Position2.Y);
				int num3 = (int)MathHelper.Min(this.Position1.Z, this.Position2.Z);
				int num4 = (int)MathHelper.Max(this.Position1.X, this.Position2.X) + 1;
				int num5 = (int)MathHelper.Max(this.Position1.Y, this.Position2.Y) + 1;
				int num6 = (int)MathHelper.Max(this.Position1.Z, this.Position2.Z) + 1;
				int num7 = num4 - num;
				int num8 = num5 - num2;
				int num9 = num6 - num3;
				this.CenterPos = new Vector3((float)num + (float)num7 / 2f, (float)num2 + (float)num8 / 2f, (float)num3 + (float)num9 / 2f);
				this.SelectionSize = new Vector3((float)num7, (float)num8, (float)num9);
			}
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x0014AACB File Offset: 0x00148CCB
		public void DoDispose()
		{
			this.Renderer.Dispose();
			this.BoxRenderer.Dispose();
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x0014AAE8 File Offset: 0x00148CE8
		public BoundingBox GetBoundsExclusiveMax()
		{
			bool flag = !this.IsSelectionDefined();
			BoundingBox result;
			if (flag)
			{
				result = new BoundingBox(Vector3.Zero, Vector3.Zero);
			}
			else
			{
				Vector3 min = Vector3.Min(this.Position1, this.Position2);
				Vector3 max = Vector3.Max(this.Position1, this.Position2) + Vector3.One;
				result = new BoundingBox(min, max);
			}
			return result;
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x0014AB50 File Offset: 0x00148D50
		public BoundingBox GetBounds()
		{
			bool flag = !this.IsSelectionDefined();
			BoundingBox result;
			if (flag)
			{
				result = new BoundingBox(Vector3.Zero, Vector3.Zero);
			}
			else
			{
				Vector3 min = Vector3.Min(this.Position1, this.Position2);
				Vector3 max = Vector3.Max(this.Position1, this.Position2);
				result = new BoundingBox(min, max);
			}
			return result;
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x0014ABAC File Offset: 0x00148DAC
		public Vector3 GetSize()
		{
			bool flag = !this.IsSelectionDefined();
			Vector3 result;
			if (flag)
			{
				result = Vector3.Zero;
			}
			else
			{
				result = Vector3.Add(Vector3.Max(this.Position1, this.Position2) - Vector3.Min(this.Position1, this.Position2), Vector3.One);
			}
			return result;
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x0014AC04 File Offset: 0x00148E04
		public void OnSelectionChange()
		{
			Vector3 vector = Vector3.Min(this.Position1, this.Position2);
			Vector3 vector2 = Vector3.Max(this.Position1, this.Position2);
			this._gameInstance.Connection.SendPacket(new BuilderToolSelectionUpdate((int)vector.X, (int)vector.Y, (int)vector.Z, (int)vector2.X, (int)vector2.Y, (int)vector2.Z));
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x0014AC76 File Offset: 0x00148E76
		public void UpdateSelection(Vector3 pos1, Vector3 pos2)
		{
			this.Position1 = Vector3.Min(pos1, pos2);
			this.Position2 = Vector3.Max(pos1, pos2);
			this.IsSelectionDirty = true;
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x0014AC9C File Offset: 0x00148E9C
		public void ClearSelection()
		{
			this.Position1 = Vector3.NaN;
			this.Position2 = Vector3.NaN;
			this._gameInstance.BuilderToolsModule.PlaySelection.Mode = PlaySelectionTool.EditMode.None;
			this._gameInstance.BuilderToolsModule.PlaySelection.HoverMode = PlaySelectionTool.EditMode.None;
			this._gameInstance.BuilderToolsModule.SelectionTool.Mode = SelectionTool.EditMode.None;
			this._gameInstance.BuilderToolsModule.SelectionTool.HoverMode = SelectionTool.EditMode.None;
			this.SelectionData[this.SelectionIndex] = null;
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x0014AD2C File Offset: 0x00148F2C
		public void Shift(Vector3 shiftAmount)
		{
			bool flag = !this.IsSelectionDefined();
			if (!flag)
			{
				this.Position1 += shiftAmount;
				this.Position2 += shiftAmount;
				this.IsSelectionDirty = true;
			}
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x0014AD74 File Offset: 0x00148F74
		public bool IsSelectionDefined()
		{
			return !this.Position1.IsNaN() && !this.Position2.IsNaN();
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x0014ADA4 File Offset: 0x00148FA4
		public bool IsAnySelectionDefined()
		{
			for (int i = 0; i < 8; i++)
			{
				bool flag = this.SelectionData[i] != null;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x0014ADDC File Offset: 0x00148FDC
		public void CycleSelectionIndex(bool forward = true)
		{
			int selectionIndex = forward ? ((this.SelectionIndex == 7) ? 0 : (this.SelectionIndex + 1)) : ((this.SelectionIndex == 0) ? 7 : (this.SelectionIndex - 1));
			this.SetSelectionIndex(selectionIndex);
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0014AE20 File Offset: 0x00149020
		public void SetSelectionIndex(int index)
		{
			bool flag = index < 0 || index > 7 || this.SelectionIndex == index;
			if (!flag)
			{
				bool flag2 = this.IsSelectionDefined();
				if (flag2)
				{
					BoundingBox item = BoundingBox.CreateFromPoints(new Vector3[]
					{
						this.Position1,
						this.Position2
					});
					item.Max += Vector3.One;
					this.SelectionData[this.SelectionIndex] = (this.IsSelectionDefined() ? new Tuple<Vector3, Vector3, BoundingBox>(this.Position1, this.Position2, item) : null);
				}
				this.SelectionIndex = index;
				bool flag3 = this.SelectionData[this.SelectionIndex] == null;
				if (flag3)
				{
					this.ClearSelection();
				}
				else
				{
					this.Position1 = this.SelectionData[this.SelectionIndex].Item1;
					this.Position2 = this.SelectionData[this.SelectionIndex].Item2;
					this.IsSelectionDirty = true;
					this.OnSelectionChange();
				}
				string text = string.Format("Selection set to #{0} - ", this.SelectionIndex);
				bool flag4 = this.IsSelectionDefined();
				if (flag4)
				{
					Vector3 size = this.GetSize();
					text += string.Format("[{0} x {1} x {2}]", size.X, size.Y, size.Z);
				}
				else
				{
					text += "Empty";
				}
				this._gameInstance.Chat.Log(text);
			}
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x0014AFAC File Offset: 0x001491AC
		public void ListBlocks(bool clipobardOutput = false, string blockName = null)
		{
			bool flag = !this.IsSelectionDefined();
			if (flag)
			{
				this._gameInstance.Chat.Log("Selection not defined");
			}
			else
			{
				int num = int.MaxValue;
				bool flag2 = blockName != null;
				if (flag2)
				{
					ClientBlockType clientBlockTypeFromName = this._gameInstance.MapModule.GetClientBlockTypeFromName(blockName);
					bool flag3 = clientBlockTypeFromName != null;
					if (!flag3)
					{
						this._gameInstance.Chat.Log("Unable to find block type with id " + blockName);
						return;
					}
					num = clientBlockTypeFromName.Id;
				}
				bool flag4 = num != int.MaxValue;
				if (flag4)
				{
					int num2 = 0;
					foreach (Vector3 position in this)
					{
						bool flag5 = this._gameInstance.MapModule.GetBlock(position, int.MaxValue) == num;
						if (flag5)
						{
							num2++;
						}
					}
					Vector3 size = this.GetSize();
					float num3 = size.X * size.Y * size.Z;
					this._gameInstance.Chat.Log(string.Format("{0}[{1}]: {2} blocks | {3}%", new object[]
					{
						blockName,
						num,
						num2,
						(float)num2 / num3 * 100f
					}));
				}
				else
				{
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					foreach (Vector3 position2 in this)
					{
						int block = this._gameInstance.MapModule.GetBlock(position2, int.MaxValue);
						bool flag6 = !dictionary.ContainsKey(block);
						if (flag6)
						{
							dictionary.Add(block, 1);
						}
						else
						{
							Dictionary<int, int> dictionary2 = dictionary;
							int key = block;
							int num4 = dictionary2[key];
							dictionary2[key] = num4 + 1;
						}
					}
					List<KeyValuePair<int, int>> list = Enumerable.ToList<KeyValuePair<int, int>>(dictionary);
					list.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => b.Value.CompareTo(a.Value));
					Vector3 size2 = this.GetSize();
					float num5 = size2.X * size2.Y * size2.Z;
					string text = string.Format("Selection [{0} x {1} x {2}] {3} total blocks{4}", new object[]
					{
						size2.X,
						size2.Y,
						size2.Z,
						num5,
						Environment.NewLine
					});
					string text2 = "";
					if (clipobardOutput)
					{
						text2 += text;
					}
					else
					{
						this._gameInstance.Chat.Log(text);
					}
					foreach (KeyValuePair<int, int> keyValuePair in list)
					{
						int key2 = keyValuePair.Key;
						int num6 = key2;
						string text3;
						if (num6 != 1)
						{
							if (num6 != 2147483647)
							{
								text3 = this._gameInstance.MapModule.ClientBlockTypes[keyValuePair.Key].Name;
							}
							else
							{
								text3 = "Undefined";
							}
						}
						else
						{
							text3 = "Unknown";
						}
						string text4 = string.Format("{0}[{1}]: {2} blocks | {3}%", new object[]
						{
							text3,
							keyValuePair.Key,
							keyValuePair.Value,
							(float)keyValuePair.Value / num5 * 100f
						});
						if (clipobardOutput)
						{
							text2 = text2 + text4 + Environment.NewLine;
						}
						else
						{
							this._gameInstance.Chat.Log(text4);
						}
					}
					if (clipobardOutput)
					{
						SDL.SDL_SetClipboardText(text2);
						this._gameInstance.Chat.Log("Data output to clipboard.");
					}
				}
			}
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x0014B3C8 File Offset: 0x001495C8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x0014B3E0 File Offset: 0x001495E0
		public IEnumerator<Vector3> GetEnumerator()
		{
			SelectionArea.<GetEnumerator>d__31 <GetEnumerator>d__ = new SelectionArea.<GetEnumerator>d__31(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x0400286B RID: 10347
		public Tuple<Vector3, Vector3, BoundingBox>[] SelectionData = new Tuple<Vector3, Vector3, BoundingBox>[8];

		// Token: 0x0400286C RID: 10348
		public Vector3 Position1 = Vector3.NaN;

		// Token: 0x0400286D RID: 10349
		public Vector3 Position2 = Vector3.NaN;

		// Token: 0x0400286E RID: 10350
		public Vector3 SelectionSize;

		// Token: 0x0400286F RID: 10351
		public Vector3 CenterPos;

		// Token: 0x04002870 RID: 10352
		public const int SelectionCount = 8;

		// Token: 0x04002871 RID: 10353
		public int SelectionIndex;

		// Token: 0x04002872 RID: 10354
		public bool IsSelectionDirty = false;

		// Token: 0x04002873 RID: 10355
		internal readonly SelectionToolRenderer Renderer;

		// Token: 0x04002874 RID: 10356
		internal readonly BoxRenderer BoxRenderer;

		// Token: 0x04002875 RID: 10357
		private readonly GameInstance _gameInstance;

		// Token: 0x04002876 RID: 10358
		public SelectionArea.SelectionRenderMode RenderMode = SelectionArea.SelectionRenderMode.LegacySelection;

		// Token: 0x04002877 RID: 10359
		public Vector3[] SelectionColors;

		// Token: 0x02000E71 RID: 3697
		public enum SelectionRenderMode
		{
			// Token: 0x04004667 RID: 18023
			LegacySelection,
			// Token: 0x04004668 RID: 18024
			PlaySelection
		}
	}
}

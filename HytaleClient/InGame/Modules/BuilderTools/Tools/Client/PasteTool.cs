using System;
using System.Collections.Generic;
using HytaleClient.Common.Collections;
using HytaleClient.Core;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x0200098B RID: 2443
	internal class PasteTool : ClientTool
	{
		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06004D85 RID: 19845 RVA: 0x0014E4E5 File Offset: 0x0014C6E5
		public override string ToolId
		{
			get
			{
				return "Paste";
			}
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0014E4EC File Offset: 0x0014C6EC
		public PasteTool(GameInstance gameInstance) : base(gameInstance)
		{
			this._renderer = new BoxRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._shapeRenderer = new BlockShapeRenderer(this._graphics, (int)this._graphics.GPUProgramStore.BasicProgram.AttribPosition.Index, (int)this._graphics.GPUProgramStore.BasicProgram.AttribTexCoords.Index);
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0014E5FF File Offset: 0x0014C7FF
		protected override void DoDispose()
		{
			this._shapeRenderer.Dispose();
			this._renderer.Dispose();
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0014E61C File Offset: 0x0014C81C
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			bool drawBlocks = this._drawBlocks;
			if (drawBlocks)
			{
				for (int i = 0; i < this._previewEntities.Count; i++)
				{
					this._previewEntities[i].SetPosition(base.BrushTarget + this._previewOffsets[i] + this._offset + new Vector3(0.5f, 0f, 0.5f));
				}
			}
			else
			{
				GLFunctions gl = this._graphics.GL;
				BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
				Vector3 vector = (this._isPositionLocked ? this._lockedPosition : base.BrushTarget) + this._offset;
				Vector3 vector2 = this._gameInstance.SceneRenderer.Data.CameraPosition;
				vector2 += this._gameInstance.SceneRenderer.Data.CameraDirection * 0.06f;
				Vector3 vector3 = -vector2;
				Matrix matrix;
				Matrix.CreateTranslation(ref vector3, out matrix);
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ViewRotationMatrix, out matrix);
				Matrix matrix2;
				Matrix.CreateTranslation(ref vector, out matrix2);
				Matrix.Multiply(ref matrix2, ref viewProjectionMatrix, out matrix2);
				Matrix matrix3;
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ProjectionMatrix, out matrix3);
				Matrix matrix4;
				Matrix.CreateTranslation(ref vector, out matrix4);
				Matrix.Multiply(ref matrix4, ref matrix3, out matrix4);
				Vector3 zero = Vector3.Zero;
				float value = 0.3f;
				Vector3 one = Vector3.One;
				float value2 = 0.2f;
				basicProgram.MVPMatrix.SetValue(ref matrix2);
				basicProgram.Color.SetValue(zero);
				basicProgram.Opacity.SetValue(value);
				this._graphics.SaveColorMask();
				gl.DepthMask(true);
				gl.ColorMask(false, false, false, false);
				this._shapeRenderer.DrawBlockShape();
				gl.DepthMask(false);
				this._graphics.RestoreColorMask();
				this._shapeRenderer.DrawBlockShape();
				basicProgram.Color.SetValue(one);
				basicProgram.Opacity.SetValue(value2);
				basicProgram.MVPMatrix.SetValue(ref matrix4);
				this._shapeRenderer.DrawBlockShapeOutline();
			}
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0014E87C File Offset: 0x0014CA7C
		protected override void OnActiveStateChange(bool newState)
		{
			bool flag = !newState;
			if (flag)
			{
				for (int i = 0; i < this._previewEntities.Count; i++)
				{
					this._gameInstance.EntityStoreModule.Despawn(this._previewEntities[i].NetworkId);
				}
				this._previewEntities.Clear();
				this._previewOffsets.Clear();
				this._needsDrawBlockUpdate = true;
			}
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0014E8F4 File Offset: 0x0014CAF4
		public void SetDrawBlocks(bool drawBlocks)
		{
			bool flag = this._drawBlocks != drawBlocks || this._needsDrawBlockUpdate;
			if (flag)
			{
				bool flag2 = this._cachedBlockChanges != null && drawBlocks && this._needsDrawBlockUpdate;
				if (flag2)
				{
					int networkEffectIndex;
					this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue("PrototypePastePreview", out networkEffectIndex);
					Vector3[] positionOffsets;
					int[] adjacentLookup;
					NativeArray<int> blockIds = PasteTool.GenerateChunkArray(this._cachedBlockChanges, out positionOffsets, out adjacentLookup);
					List<int> list = PasteTool.FilterVisibleBlocks(blockIds, positionOffsets, adjacentLookup, this._gameInstance, this._previewOffsets);
					blockIds.Dispose();
					for (int i = 0; i < list.Count; i++)
					{
						int block = list[i];
						Entity entity;
						this._gameInstance.EntityStoreModule.Spawn(-1, out entity);
						entity.SetIsTangible(false);
						entity.SetBlock(block);
						entity.AddEffect(networkEffectIndex, null, null, null);
						this._previewEntities.Add(entity);
					}
					this._needsDrawBlockUpdate = false;
				}
				for (int j = 0; j < this._previewEntities.Count; j++)
				{
					this._previewEntities[j].IsVisible = drawBlocks;
					this._previewEntities[j].SetPositionTeleport(base.BrushTarget + this._previewOffsets[j] + this._offset + new Vector3(0.5f, 0f, 0.5f));
				}
			}
			this._drawBlocks = drawBlocks;
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0014EAA0 File Offset: 0x0014CCA0
		public static NativeArray<int> GenerateChunkArray(EditorBlocksChange.BlockChange[] blockChanges, out Vector3[] positionOffsets, out int[] adjacentLookup)
		{
			List<Vector3> list = new List<Vector3>(8);
			for (int i = 0; i < blockChanges.Length; i++)
			{
				EditorBlocksChange.BlockChange blockChange = blockChanges[i];
				bool flag = blockChange.Block <= 0;
				if (!flag)
				{
					list.Add(new Vector3((float)blockChanges[i].X, (float)blockChanges[i].Y, (float)blockChanges[i].Z));
				}
			}
			bool flag2 = list.Count == 0;
			if (flag2)
			{
				list.Add(Vector3.Zero);
			}
			BoundingBox boundingBox = BoundingBox.CreateFromPoints(list);
			Vector3 size = boundingBox.GetSize();
			IntVector3 intVector = new IntVector3((int)size.X, (int)size.Y, (int)size.Z);
			intVector += IntVector3.One;
			intVector += new IntVector3(2, 2, 2);
			adjacentLookup = new int[]
			{
				intVector.Z * intVector.X,
				-intVector.Z * intVector.X,
				-1,
				1,
				intVector.X,
				-intVector.X
			};
			int num = intVector.X * intVector.Y * intVector.Z;
			NativeArray<int> result = new NativeArray<int>(num, 0, 0);
			positionOffsets = new Vector3[num];
			IntVector3 intVector2 = new IntVector3((int)boundingBox.Min.X, (int)boundingBox.Min.Y, (int)boundingBox.Min.Z);
			foreach (EditorBlocksChange.BlockChange blockChange2 in blockChanges)
			{
				IntVector3 intVector3 = new IntVector3(blockChange2.X, blockChange2.Y, blockChange2.Z);
				intVector3.X += -intVector2.X;
				intVector3.Y += -intVector2.Y;
				intVector3.Z += -intVector2.Z;
				intVector3 += IntVector3.One;
				int num2 = intVector3.Y * intVector.Z * intVector.X + intVector3.Z * intVector.X + intVector3.X;
				result[num2] = blockChange2.Block;
				positionOffsets[num2] = new Vector3((float)blockChange2.X, (float)blockChange2.Y, (float)blockChange2.Z);
			}
			return result;
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x0014ED04 File Offset: 0x0014CF04
		public static NativeArray<int> GenerateChunkArray(SelectionArea selectionArea, GameInstance gameInstance, out Vector3[] positionOffsets, out int[] adjacentLookup)
		{
			BoundingBox bounds = selectionArea.GetBounds();
			Vector3 size = bounds.GetSize();
			IntVector3 intVector = new IntVector3((int)size.X, (int)size.Y, (int)size.Z);
			intVector += IntVector3.One;
			intVector += new IntVector3(2, 2, 2);
			adjacentLookup = new int[]
			{
				intVector.Z * intVector.X,
				-intVector.Z * intVector.X,
				-1,
				1,
				intVector.X,
				-intVector.X
			};
			int num = intVector.X * intVector.Y * intVector.Z;
			NativeArray<int> result = new NativeArray<int>(num, 0, 0);
			positionOffsets = new Vector3[num];
			IntVector3 intVector2 = new IntVector3((int)bounds.Min.X, (int)bounds.Min.Y, (int)bounds.Min.Z);
			foreach (Vector3 vector in selectionArea)
			{
				int block = gameInstance.MapModule.GetBlock(vector, int.MaxValue);
				bool flag = block <= 0;
				if (!flag)
				{
					IntVector3 intVector3 = new IntVector3((int)vector.X, (int)vector.Y, (int)vector.Z);
					intVector3.X += -intVector2.X;
					intVector3.Y += -intVector2.Y;
					intVector3.Z += -intVector2.Z;
					intVector3 += IntVector3.One;
					int num2 = intVector3.Y * intVector.Z * intVector.X + intVector3.Z * intVector.X + intVector3.X;
					result[num2] = block;
					positionOffsets[num2] = vector;
				}
			}
			return result;
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0014EF0C File Offset: 0x0014D10C
		public static List<int> FilterVisibleBlocks(NativeArray<int> blockIds, Vector3[] positionOffsets, int[] adjacentLookup, GameInstance gameInstance, List<Vector3> outPreviewOffsets)
		{
			List<int> list = new List<int>(8);
			for (int i = 0; i < blockIds.Length; i++)
			{
				int num = blockIds[i];
				ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[num];
				bool flag = clientBlockType.DrawType == 0;
				if (!flag)
				{
					bool flag2 = false;
					bool shouldRenderCube = clientBlockType.ShouldRenderCube;
					if (shouldRenderCube)
					{
						for (int j = 0; j < 6; j++)
						{
							int num2 = i + adjacentLookup[j];
							int num3 = blockIds[num2];
							bool flag3 = num3 == int.MaxValue;
							if (!flag3)
							{
								ClientBlockType clientBlockType2 = gameInstance.MapModule.ClientBlockTypes[num3];
								bool flag4 = num3 == 0 || (!clientBlockType2.ShouldRenderCube && clientBlockType2.VerticalFill == 8) || (clientBlockType2.RequiresAlphaBlending && !clientBlockType.RequiresAlphaBlending);
								bool flag5 = flag4;
								if (flag5)
								{
									flag2 = true;
									break;
								}
							}
						}
					}
					bool flag6 = flag2 || (clientBlockType.RenderedBlockyModel != null && (!clientBlockType.ShouldRenderCube || clientBlockType.RequiresAlphaBlending));
					if (flag6)
					{
						list.Add(num);
						outPreviewOffsets.Add(positionOffsets[i]);
					}
				}
			}
			return list;
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0014F058 File Offset: 0x0014D258
		public override bool NeedsDrawing()
		{
			BoundingBox blockSetBox = this._blockSetBox;
			return this._isPositionLocked || !base.BrushTarget.IsNaN();
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0014F090 File Offset: 0x0014D290
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = clickType == InteractionModule.ClickType.None;
			if (flag)
			{
				this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_PASTE_RELEASE");
			}
			else
			{
				bool flag2 = interactionType == 1;
				if (flag2)
				{
					bool flag3 = this._gameInstance.Input.IsShiftHeld();
					if (flag3)
					{
						this._isPositionLocked = !this._isPositionLocked;
						bool isPositionLocked = this._isPositionLocked;
						if (isPositionLocked)
						{
							this._lockedPosition = new IntVector3(base.BrushTarget);
						}
						this._gameInstance.Notifications.AddNotification("Paste tool position " + (this._isPositionLocked ? "locked" : "unlocked") + ".", null);
					}
					else
					{
						bool flag4 = !this._isPositionLocked && base.BrushTarget.IsNaN();
						if (flag4)
						{
							return;
						}
						Vector3 value = this._isPositionLocked ? this._lockedPosition : base.BrushTarget;
						this.OnClipboardPaste(value + this._offset);
						this._isPositionLocked = false;
					}
					if (firstRun)
					{
						this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_PASTE");
					}
				}
				else
				{
					bool flag5 = this._gameInstance.Input.IsShiftHeld();
					if (flag5)
					{
						this.OnClipboardRotate(90, 0);
					}
					else
					{
						bool flag6 = this._gameInstance.Input.IsAltHeld();
						if (flag6)
						{
							this.OnClipboardRotate(90, 2);
						}
						else
						{
							this.OnClipboardRotate(90, 1);
						}
					}
				}
			}
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0014F218 File Offset: 0x0014D418
		public override void Update(float deltaTime)
		{
			bool flag = this._gameInstance.Input.IsAnyKeyHeld(false);
			if (flag)
			{
				this.OnKeyDown();
			}
			this.SetDrawBlocks(this._gameInstance.Input.IsBindingHeld(this._gameInstance.App.Settings.InputBindings.PastePreview, false));
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x0014F274 File Offset: 0x0014D474
		private void OnKeyDown()
		{
			Input input = this._gameInstance.Input;
			bool flag = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftReset], false);
			if (flag)
			{
				this._offset = Vector3.Zero;
			}
			bool flag2 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftUp], false);
			if (flag2)
			{
				this._offset += Vector3.Up;
			}
			bool flag3 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftDown], false);
			if (flag3)
			{
				this._offset += Vector3.Down;
			}
			bool flag4 = input.IsKeyHeld(this._keybinds[PasteTool.Keybind.ShiftLeft], false) || input.IsKeyHeld(this._keybinds[PasteTool.Keybind.ShiftRight], false) || input.IsKeyHeld(this._keybinds[PasteTool.Keybind.ShiftForward], false) || input.IsKeyHeld(this._keybinds[PasteTool.Keybind.ShiftBackward], false);
			if (flag4)
			{
				Vector3 playerLookDirection = this.GetPlayerLookDirection();
				bool flag5 = input.IsAltHeld();
				if (flag5)
				{
					bool flag6 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftLeft], false);
					if (flag6)
					{
						bool flag7 = playerLookDirection.X != 0f;
						if (flag7)
						{
							this.OnClipboardRotate((playerLookDirection.X == 1f) ? -90 : 90, 0);
						}
						bool flag8 = playerLookDirection.Z != 0f;
						if (flag8)
						{
							this.OnClipboardRotate((playerLookDirection.Z == 1f) ? -90 : 90, 2);
						}
					}
					else
					{
						bool flag9 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftRight], false);
						if (flag9)
						{
							bool flag10 = playerLookDirection.X != 0f;
							if (flag10)
							{
								this.OnClipboardRotate((playerLookDirection.X == 1f) ? 90 : -90, 0);
							}
							bool flag11 = playerLookDirection.Z != 0f;
							if (flag11)
							{
								this.OnClipboardRotate((playerLookDirection.Z == 1f) ? 90 : -90, 2);
							}
						}
						else
						{
							bool flag12 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftForward], false);
							if (flag12)
							{
								bool flag13 = playerLookDirection.X != 0f;
								if (flag13)
								{
									this.OnClipboardRotate((playerLookDirection.X == 1f) ? -90 : 90, 2);
								}
								bool flag14 = playerLookDirection.Z != 0f;
								if (flag14)
								{
									this.OnClipboardRotate((playerLookDirection.Z == 1f) ? 90 : -90, 0);
								}
							}
							else
							{
								bool flag15 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftBackward], false);
								if (flag15)
								{
									bool flag16 = playerLookDirection.X != 0f;
									if (flag16)
									{
										this.OnClipboardRotate((playerLookDirection.X == 1f) ? 90 : -90, 2);
									}
									bool flag17 = playerLookDirection.Z != 0f;
									if (flag17)
									{
										this.OnClipboardRotate((playerLookDirection.Z == 1f) ? -90 : 90, 0);
									}
								}
							}
						}
					}
				}
				else
				{
					bool flag18 = input.IsShiftHeld();
					if (flag18)
					{
						bool flag19 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftLeft], false);
						if (flag19)
						{
							this.OnClipboardRotate(-90, 1);
						}
						bool flag20 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftRight], false);
						if (flag20)
						{
							this.OnClipboardRotate(90, 1);
						}
					}
					else
					{
						Vector3 vector = Vector3.Zero;
						bool flag21 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftLeft], false);
						if (flag21)
						{
							vector = new Vector3(playerLookDirection.Z, 0f, -playerLookDirection.X);
						}
						bool flag22 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftRight], false);
						if (flag22)
						{
							vector = new Vector3(-playerLookDirection.Z, 0f, playerLookDirection.X);
						}
						bool flag23 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftForward], false);
						if (flag23)
						{
							vector = playerLookDirection;
						}
						bool flag24 = input.ConsumeKey(this._keybinds[PasteTool.Keybind.ShiftBackward], false);
						if (flag24)
						{
							vector = -playerLookDirection;
						}
						bool flag25 = vector != Vector3.Zero;
						if (flag25)
						{
							this._offset += vector;
						}
					}
				}
			}
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x0014F6CC File Offset: 0x0014D8CC
		public void UpdateBlockSet(EditorBlocksChange.BlockChange[] blockChanges)
		{
			bool flag = blockChanges.Length == 0;
			if (!flag)
			{
				Vector3[] array = new Vector3[blockChanges.Length];
				for (int i = 0; i < blockChanges.Length; i++)
				{
					array[i] = new Vector3((float)blockChanges[i].X, (float)blockChanges[i].Y, (float)blockChanges[i].Z);
				}
				this._blockSetBox = BoundingBox.CreateFromPoints(array);
				this._blockSetBox.Max = this._blockSetBox.Max + Vector3.One;
				Vector3 size = this._blockSetBox.GetSize();
				bool[,,] array2 = new bool[(int)size.X, (int)size.Y, (int)size.Z];
				Vector3 min = this._blockSetBox.Min;
				for (int j = 0; j < blockChanges.Length; j++)
				{
					Vector3 vector = array[j] - min;
					array2[(int)vector.X, (int)vector.Y, (int)vector.Z] = (blockChanges[j].Block > 0);
				}
				this._shapeRenderer.UpdateModelData(array2, (int)min.X, (int)min.Y, (int)min.Z);
				for (int k = 0; k < this._previewEntities.Count; k++)
				{
					this._gameInstance.EntityStoreModule.Despawn(this._previewEntities[k].NetworkId);
				}
				this._previewEntities.Clear();
				this._previewOffsets.Clear();
				this._cachedBlockChanges = blockChanges;
				this._needsDrawBlockUpdate = true;
			}
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x0014F878 File Offset: 0x0014DA78
		private void OnClipboardPaste(Vector3 position)
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolPasteClipboard((int)position.X, (int)position.Y, (int)position.Z));
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0014F8A6 File Offset: 0x0014DAA6
		private void OnClipboardRotate(int angle, Axis axis)
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolRotateClipboard(angle, axis));
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x0014F8C4 File Offset: 0x0014DAC4
		private Vector3 GetPlayerLookDirection()
		{
			float num = MathHelper.SnapValue(this._gameInstance.LocalPlayer.LookOrientation.Yaw, 1.5707964f);
			bool flag = num == 0f;
			Vector3 result;
			if (flag)
			{
				result = Vector3.Forward;
			}
			else
			{
				bool flag2 = num == -1.5707964f;
				if (flag2)
				{
					result = Vector3.Right;
				}
				else
				{
					bool flag3 = num == 1.5707964f;
					if (flag3)
					{
						result = Vector3.Left;
					}
					else
					{
						result = Vector3.Backward;
					}
				}
			}
			return result;
		}

		// Token: 0x040028C8 RID: 10440
		private readonly BoxRenderer _renderer;

		// Token: 0x040028C9 RID: 10441
		private readonly BlockShapeRenderer _shapeRenderer;

		// Token: 0x040028CA RID: 10442
		private BoundingBox _blockSetBox;

		// Token: 0x040028CB RID: 10443
		private Vector3 _offset = new Vector3(0f, 1f, 0f);

		// Token: 0x040028CC RID: 10444
		private bool _isPositionLocked = false;

		// Token: 0x040028CD RID: 10445
		private IntVector3 _lockedPosition = IntVector3.Zero;

		// Token: 0x040028CE RID: 10446
		private bool _drawBlocks;

		// Token: 0x040028CF RID: 10447
		private EditorBlocksChange.BlockChange[] _cachedBlockChanges;

		// Token: 0x040028D0 RID: 10448
		private bool _needsDrawBlockUpdate;

		// Token: 0x040028D1 RID: 10449
		private List<Entity> _previewEntities = new List<Entity>(16);

		// Token: 0x040028D2 RID: 10450
		private List<Vector3> _previewOffsets = new List<Vector3>(16);

		// Token: 0x040028D3 RID: 10451
		private Dictionary<PasteTool.Keybind, SDL.SDL_Scancode> _keybinds = new Dictionary<PasteTool.Keybind, SDL.SDL_Scancode>
		{
			{
				PasteTool.Keybind.ShiftLeft,
				SDL.SDL_Scancode.SDL_SCANCODE_LEFT
			},
			{
				PasteTool.Keybind.ShiftRight,
				SDL.SDL_Scancode.SDL_SCANCODE_RIGHT
			},
			{
				PasteTool.Keybind.ShiftForward,
				SDL.SDL_Scancode.SDL_SCANCODE_UP
			},
			{
				PasteTool.Keybind.ShiftBackward,
				SDL.SDL_Scancode.SDL_SCANCODE_DOWN
			},
			{
				PasteTool.Keybind.ShiftUp,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP
			},
			{
				PasteTool.Keybind.ShiftDown,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN
			},
			{
				PasteTool.Keybind.ShiftReset,
				SDL.SDL_Scancode.SDL_SCANCODE_HOME
			}
		};

		// Token: 0x02000E75 RID: 3701
		private enum Keybind
		{
			// Token: 0x04004677 RID: 18039
			ShiftLeft,
			// Token: 0x04004678 RID: 18040
			ShiftRight,
			// Token: 0x04004679 RID: 18041
			ShiftForward,
			// Token: 0x0400467A RID: 18042
			ShiftBackward,
			// Token: 0x0400467B RID: 18043
			ShiftUp,
			// Token: 0x0400467C RID: 18044
			ShiftDown,
			// Token: 0x0400467D RID: 18045
			ShiftReset
		}
	}
}

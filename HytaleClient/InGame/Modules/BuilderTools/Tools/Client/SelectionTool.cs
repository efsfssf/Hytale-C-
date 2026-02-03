using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x0200098D RID: 2445
	internal class SelectionTool : ClientTool
	{
		// Token: 0x06004DD2 RID: 19922 RVA: 0x001547EF File Offset: 0x001529EF
		public override bool NeedsDrawing()
		{
			return this.SelectionArea.NeedsDrawing();
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x001547FC File Offset: 0x001529FC
		public override bool NeedsTextDrawing()
		{
			return this.SelectionArea.NeedsTextDrawing();
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x00154809 File Offset: 0x00152A09
		public override string ToolId
		{
			get
			{
				return "Selection";
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06004DD5 RID: 19925 RVA: 0x00154810 File Offset: 0x00152A10
		// (set) Token: 0x06004DD6 RID: 19926 RVA: 0x00154818 File Offset: 0x00152A18
		public bool IsCursorOverSelection { get; private set; } = false;

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x00154821 File Offset: 0x00152A21
		// (set) Token: 0x06004DD8 RID: 19928 RVA: 0x00154829 File Offset: 0x00152A29
		public SelectionTool.EditMode Mode { get; set; } = SelectionTool.EditMode.None;

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06004DD9 RID: 19929 RVA: 0x00154832 File Offset: 0x00152A32
		// (set) Token: 0x06004DDA RID: 19930 RVA: 0x0015483A File Offset: 0x00152A3A
		public SelectionTool.EditMode HoverMode { get; set; } = SelectionTool.EditMode.None;

		// Token: 0x06004DDB RID: 19931 RVA: 0x00154844 File Offset: 0x00152A44
		public SelectionTool(GameInstance gameInstance) : base(gameInstance)
		{
			this.SelectionArea = gameInstance.BuilderToolsModule.SelectionArea;
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x00154908 File Offset: 0x00152B08
		public override void Update(float deltaTime)
		{
			switch (this.Mode)
			{
			case SelectionTool.EditMode.MoveSide:
				this.OnMove();
				break;
			case SelectionTool.EditMode.MovePos1:
			case SelectionTool.EditMode.MovePos2:
			case SelectionTool.EditMode.ResizePos1:
			case SelectionTool.EditMode.ResizePos2:
			{
				Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
				Vector3 vector = lookRay.Position + lookRay.Direction * this._resizeDistance;
				vector.X = (float)((int)Math.Floor((double)vector.X));
				vector.Y = (float)((int)Math.Floor((double)vector.Y));
				vector.Z = (float)((int)Math.Floor((double)vector.Z));
				bool flag = this.Mode == SelectionTool.EditMode.ResizePos1;
				if (flag)
				{
					this.SelectionArea.Position1 = vector;
				}
				else
				{
					bool flag2 = this.Mode == SelectionTool.EditMode.ResizePos2;
					if (flag2)
					{
						this.SelectionArea.Position2 = vector;
					}
					else
					{
						bool flag3 = this.Mode == SelectionTool.EditMode.MovePos1;
						if (flag3)
						{
							this.SelectionArea.Position1 = vector;
							this.SelectionArea.Position2 = this.SelectionArea.Position1 + this._resizePosition2 - this._resizePosition1;
						}
						else
						{
							bool flag4 = this.Mode == SelectionTool.EditMode.MovePos2;
							if (flag4)
							{
								this.SelectionArea.Position2 = vector;
								this.SelectionArea.Position1 = this.SelectionArea.Position2 + this._resizePosition1 - this._resizePosition2;
							}
						}
					}
				}
				this.SelectionArea.IsSelectionDirty = true;
				break;
			}
			case SelectionTool.EditMode.ResizeSide:
				this.OnResize();
				break;
			}
			this.UpdateSelectionHighlight(this._gameInstance.Input.IsAltHeld() && !this._gameInstance.Input.IsShiftHeld());
			bool flag5 = this._gameInstance.Input.IsAnyKeyHeld(false);
			if (flag5)
			{
				this.OnKeyDown();
			}
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x00154AFC File Offset: 0x00152CFC
		protected override void OnActiveStateChange(bool newState)
		{
			if (newState)
			{
				this.SelectionArea.RenderMode = SelectionArea.SelectionRenderMode.LegacySelection;
			}
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x00154B1C File Offset: 0x00152D1C
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			bool flag = this.SelectionArea.IsSelectionDefined();
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				SceneRenderer.SceneData data = this._gameInstance.SceneRenderer.Data;
				Vector3 value = this._gameInstance.SceneRenderer.Data.CameraDirection * 0.1f;
				Vector3 vector = -value;
				Matrix matrix;
				Matrix.CreateTranslation(ref vector, out matrix);
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ViewRotationMatrix, out matrix);
				Matrix matrix2;
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ProjectionMatrix, out matrix2);
				this._graphics.SaveColorMask();
				gl.DepthMask(true);
				gl.ColorMask(false, false, false, false);
				gl.DepthFunc(GL.ALWAYS);
				this.SelectionArea.Renderer.DrawOutlineBox(ref data.ViewRotationProjectionMatrix, ref data.ViewRotationMatrix, -data.CameraPosition, data.ViewportSize, this._graphics.BlackColor, this._graphics.BlackColor, 0f, 1f, true);
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
				this.SelectionArea.Renderer.DrawOutlineBox(ref data.ViewRotationProjectionMatrix, ref data.ViewRotationMatrix, -data.CameraPosition, data.ViewportSize, this._graphics.BlackColor, this._graphics.BlackColor, 0f, 1f, true);
				gl.DepthMask(false);
				this._graphics.RestoreColorMask();
				float num = (float)this._builderTools.builderToolsSettings.SelectionOpacity * 0.01f;
				this.SelectionArea.Renderer.DrawGrid(ref matrix2, -data.CameraPosition, this.Color, num, this._selectionDrawMode);
				gl.DepthFunc(GL.ALWAYS);
				this.SelectionArea.Renderer.DrawOutlineBox(ref matrix2, ref data.ViewRotationMatrix, -data.CameraPosition, data.ViewportSize, this._graphics.WhiteColor, this._graphics.BlackColor, num, num * 0.25f, this._builderTools.DrawHighlightAndUndergroundColor);
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
				bool flag2 = this.Mode == SelectionTool.EditMode.ResizePos1 || this.Mode == SelectionTool.EditMode.MovePos1;
				if (flag2)
				{
					this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.4f, 0.05f);
				}
				else
				{
					bool flag3 = this.Mode == SelectionTool.EditMode.ResizePos2 || this.Mode == SelectionTool.EditMode.MovePos2;
					if (flag3)
					{
						this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.05f, 0.4f);
					}
					else
					{
						bool flag4 = this.HoverMode == SelectionTool.EditMode.ResizePos1 || this.HoverMode == SelectionTool.EditMode.MovePos1;
						if (flag4)
						{
							this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.2f, 0.05f);
						}
						else
						{
							bool flag5 = this.HoverMode == SelectionTool.EditMode.ResizePos2 || this.HoverMode == SelectionTool.EditMode.MovePos2;
							if (flag5)
							{
								this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.05f, 0.2f);
							}
							else
							{
								this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.05f, 0.05f);
							}
						}
					}
				}
			}
			bool flag6 = this.FaceHighlightNeedsDrawing();
			if (flag6)
			{
				Vector3 selectionNormal = (this.Mode == SelectionTool.EditMode.MoveSide || this.Mode == SelectionTool.EditMode.ResizeSide) ? this._resizeNormal : this._rayBoxHit.Normal;
				Vector3 color = (this.Mode == SelectionTool.EditMode.MoveSide) ? this._graphics.MagentaColor : ((this.Mode == SelectionTool.EditMode.ResizeSide) ? this._graphics.BlueColor : this._graphics.CyanColor);
				this.SelectionArea.Renderer.DrawFaceHighlight(ref this._gameInstance.SceneRenderer.Data.ViewRotationProjectionMatrix, selectionNormal, color, -this._gameInstance.SceneRenderer.Data.CameraPosition);
			}
			bool flag7 = this.SelectionArea.IsAnySelectionDefined();
			if (flag7)
			{
				GLFunctions gl2 = this._graphics.GL;
				gl2.DepthFunc(GL.ALWAYS);
				for (int i = 0; i < 8; i++)
				{
					bool flag8 = this.SelectionArea.SelectionData[i] != null && i != this.SelectionArea.SelectionIndex;
					if (flag8)
					{
						Vector3 vector2 = this.SelectionArea.SelectionColors[i];
						this.SelectionArea.BoxRenderer.Draw(Vector3.Zero, this.SelectionArea.SelectionData[i].Item3, viewProjectionMatrix, vector2, 0.4f, vector2, 0.03f);
					}
				}
				gl2.DepthFunc((!this._graphics.UseReverseZ) ? GL.GEQUAL : GL.LEQUAL);
			}
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x001550F0 File Offset: 0x001532F0
		public override void DrawText(ref Matrix viewProjectionMatrix)
		{
			base.DrawText(ref viewProjectionMatrix);
			this.SelectionArea.Renderer.DrawText(ref viewProjectionMatrix, this._gameInstance.CameraModule.Controller);
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x00155120 File Offset: 0x00153320
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			long num = DateTime.UtcNow.Ticks / 10000L;
			bool flag = num - this._toolDelayTime < 350L;
			if (!flag)
			{
				this._toolDelayTime = num;
				Input input = this._gameInstance.Input;
				bool flag2 = this.Mode > SelectionTool.EditMode.None;
				if (flag2)
				{
					this.Mode = SelectionTool.EditMode.None;
					bool flag3 = interactionType == 1;
					if (flag3)
					{
						this.SelectionArea.Position1 = this._resizePosition1;
						this.SelectionArea.Position2 = this._resizePosition2;
						this.SelectionArea.IsSelectionDirty = true;
					}
					this.SelectionArea.OnSelectionChange();
				}
				else
				{
					bool flag4 = !this.IsCursorOverSelection && input.IsAltHeld();
					if (flag4)
					{
						float num2 = -1f;
						int num3 = -1;
						Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
						for (int i = 0; i < 8; i++)
						{
							bool flag5 = this.SelectionArea.SelectionData[i] != null;
							if (flag5)
							{
								HitDetection.RayBoxCollision rayBoxCollision;
								bool flag6 = HitDetection.CheckRayBoxCollision(this.SelectionArea.SelectionData[i].Item3, lookRay.Position, lookRay.Direction, out rayBoxCollision, true);
								if (flag6)
								{
									float num4 = Vector3.Distance(lookRay.Position, rayBoxCollision.Position);
									bool flag7 = num3 == -1 || num4 < num2;
									if (flag7)
									{
										num3 = i;
										num2 = num4;
									}
								}
							}
						}
						bool flag8 = num3 > -1;
						if (flag8)
						{
							this.SelectionArea.SetSelectionIndex(num3);
						}
					}
					else
					{
						bool flag9 = this.IsCursorOverSelection && (input.IsShiftHeld() || input.IsAltHeld());
						if (flag9)
						{
							bool flag10 = interactionType == 0;
							if (flag10)
							{
								bool flag11 = this.HoverMode == SelectionTool.EditMode.ResizePos1 || this.HoverMode == SelectionTool.EditMode.ResizePos2;
								if (flag11)
								{
									bool flag12 = input.IsShiftHeld() && input.IsAltHeld();
									if (flag12)
									{
										this.Mode = ((this.HoverMode == SelectionTool.EditMode.ResizePos1) ? SelectionTool.EditMode.MovePos1 : SelectionTool.EditMode.MovePos2);
									}
									else
									{
										bool flag13 = input.IsShiftHeld();
										if (flag13)
										{
											this.Mode = ((this.HoverMode == SelectionTool.EditMode.ResizePos1) ? SelectionTool.EditMode.ResizePos1 : SelectionTool.EditMode.ResizePos2);
										}
									}
									Vector3 vector = (this.Mode == SelectionTool.EditMode.ResizePos1 || this.Mode == SelectionTool.EditMode.MovePos1) ? this.SelectionArea.Position1 : this.SelectionArea.Position2;
									float resizeDistance = Vector3.Distance(this._gameInstance.CameraModule.Controller.Position, vector);
									this._resizeDistance = resizeDistance;
								}
								else
								{
									bool flag14 = input.IsShiftHeld() && input.IsAltHeld();
									if (flag14)
									{
										this.Mode = SelectionTool.EditMode.MoveSide;
									}
									else
									{
										this.Mode = SelectionTool.EditMode.ResizeSide;
									}
									this._resizeOrigin = this._rayBoxHit.Position;
									this._resizeNormal = this._rayBoxHit.Normal;
									this._resizeDirection = SelectionTool.GetVectorDirection(this._resizeNormal);
								}
								this._resizePosition1 = this.SelectionArea.Position1;
								this._resizePosition2 = this.SelectionArea.Position2;
							}
						}
						else
						{
							bool flag15 = !input.IsAnyModifierHeld();
							if (flag15)
							{
								Vector3 brushTarget = base.BrushTarget;
								bool flag16 = brushTarget.IsNaN();
								if (!flag16)
								{
									bool flag17 = !this.SelectionArea.IsSelectionDefined();
									if (flag17)
									{
										this.SelectionArea.Position1 = (this.SelectionArea.Position2 = brushTarget);
									}
									else
									{
										bool flag18 = interactionType == 0;
										if (flag18)
										{
											this.SelectionArea.Position1 = brushTarget;
										}
										else
										{
											this.SelectionArea.Position2 = brushTarget;
										}
									}
									this.SelectionArea.IsSelectionDirty = true;
									this.SelectionArea.OnSelectionChange();
								}
							}
							else
							{
								bool flag19 = !this.IsCursorOverSelection && input.IsShiftHeld();
								if (flag19)
								{
									Vector3 brushTarget2 = base.BrushTarget;
									bool flag20 = brushTarget2.IsNaN();
									if (!flag20)
									{
										bool flag21 = !this.SelectionArea.IsSelectionDefined();
										if (flag21)
										{
											this.SelectionArea.Position1 = (this.SelectionArea.Position2 = brushTarget2);
										}
										this._resizePosition1 = this.SelectionArea.Position1;
										this._resizePosition2 = this.SelectionArea.Position2;
										this._resizeDistance = Vector3.Distance(this._gameInstance.CameraModule.Controller.Position, brushTarget2);
										bool flag22 = interactionType == 0;
										if (flag22)
										{
											this.SelectionArea.Position1 = brushTarget2;
											this.Mode = SelectionTool.EditMode.ResizePos1;
										}
										else
										{
											this.SelectionArea.Position2 = brushTarget2;
											this.Mode = SelectionTool.EditMode.ResizePos2;
										}
										this.SelectionArea.IsSelectionDirty = true;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x001555D8 File Offset: 0x001537D8
		private void OnKeyDown()
		{
			Input input = this._gameInstance.Input;
			bool flag = input.IsAltHeld();
			if (flag)
			{
				bool flag2 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionNextSet], false);
				if (flag2)
				{
					this.SelectionArea.CycleSelectionIndex(true);
				}
				bool flag3 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionPreviousSet], false);
				if (flag3)
				{
					this.SelectionArea.CycleSelectionIndex(false);
				}
			}
			bool flag4 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionShiftUp], false);
			if (flag4)
			{
				this.SelectionArea.Shift(Vector3.Up);
			}
			bool flag5 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionShiftDown], false);
			if (flag5)
			{
				this.SelectionArea.Shift(Vector3.Down);
			}
			bool flag6 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionNextDrawMode], false);
			if (flag6)
			{
				this.NextDrawMode();
			}
			bool flag7 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionClear], false);
			if (flag7)
			{
				this.SelectionArea.ClearSelection();
			}
			bool flag8 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionPosOne], false);
			if (flag8)
			{
				this.OnGeneralAction(0);
			}
			bool flag9 = input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionPosTwo], false);
			if (flag9)
			{
				this.OnGeneralAction(1);
			}
			bool flag10 = input.IsShiftHeld() && input.ConsumeKey(this._keybinds[SelectionTool.Keybind.SelectionCopy], false);
			if (flag10)
			{
				this.OnGeneralAction(2);
			}
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x00155750 File Offset: 0x00153950
		private void OnResize()
		{
			bool flag = !this.SelectionArea.IsSelectionDefined();
			if (!flag)
			{
				Vector3 projectedCursorPosition = this.GetProjectedCursorPosition();
				bool flag2 = this._resizeDirection == SelectionTool.Direction.Up;
				if (flag2)
				{
					bool flag3 = this.SelectionArea.Position1.Y > this.SelectionArea.Position2.Y;
					if (flag3)
					{
						this.SelectionArea.Position1.Y = MathHelper.Min(MathHelper.Max((float)SelectionTool.FloorInt(projectedCursorPosition.Y), 0f), (float)(ChunkHelper.Height - 1));
					}
					else
					{
						this.SelectionArea.Position2.Y = MathHelper.Min(MathHelper.Max((float)SelectionTool.FloorInt(projectedCursorPosition.Y), 0f), (float)(ChunkHelper.Height - 1));
					}
				}
				else
				{
					bool flag4 = this._resizeDirection == SelectionTool.Direction.Down;
					if (flag4)
					{
						bool flag5 = this.SelectionArea.Position1.Y < this.SelectionArea.Position2.Y;
						if (flag5)
						{
							this.SelectionArea.Position1.Y = MathHelper.Min(MathHelper.Max((float)SelectionTool.FloorInt(projectedCursorPosition.Y), 0f), (float)(ChunkHelper.Height - 1));
						}
						else
						{
							this.SelectionArea.Position2.Y = MathHelper.Min(MathHelper.Max((float)SelectionTool.FloorInt(projectedCursorPosition.Y), 0f), (float)(ChunkHelper.Height - 1));
						}
					}
					else
					{
						bool flag6 = this._resizeDirection == SelectionTool.Direction.Left;
						if (flag6)
						{
							bool flag7 = this.SelectionArea.Position1.X < this.SelectionArea.Position2.X;
							if (flag7)
							{
								this.SelectionArea.Position1.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
							}
							else
							{
								this.SelectionArea.Position2.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
							}
						}
						else
						{
							bool flag8 = this._resizeDirection == SelectionTool.Direction.Right;
							if (flag8)
							{
								bool flag9 = this.SelectionArea.Position1.X > this.SelectionArea.Position2.X;
								if (flag9)
								{
									this.SelectionArea.Position1.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
								}
								else
								{
									this.SelectionArea.Position2.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
								}
							}
							else
							{
								bool flag10 = this._resizeDirection == SelectionTool.Direction.Forward;
								if (flag10)
								{
									bool flag11 = this.SelectionArea.Position1.Z > this.SelectionArea.Position2.Z;
									if (flag11)
									{
										this.SelectionArea.Position2.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
									}
									else
									{
										this.SelectionArea.Position1.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
									}
								}
								else
								{
									bool flag12 = this._resizeDirection == SelectionTool.Direction.Backward;
									if (flag12)
									{
										bool flag13 = this.SelectionArea.Position1.Z < this.SelectionArea.Position2.Z;
										if (flag13)
										{
											this.SelectionArea.Position2.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
										}
										else
										{
											this.SelectionArea.Position1.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
										}
									}
								}
							}
						}
					}
				}
				this.SelectionArea.IsSelectionDirty = true;
			}
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x00155AB0 File Offset: 0x00153CB0
		private void OnMove()
		{
			bool flag = !this.SelectionArea.IsSelectionDefined();
			if (!flag)
			{
				Vector3 projectedCursorPosition = this.GetProjectedCursorPosition();
				Vector3 size = this.SelectionArea.GetSize();
				bool flag2 = this._resizeDirection == SelectionTool.Direction.Up;
				if (flag2)
				{
					bool flag3 = this.SelectionArea.Position1.Y > this.SelectionArea.Position2.Y;
					if (flag3)
					{
						this.SelectionArea.Position1.Y = (float)SelectionTool.FloorInt(projectedCursorPosition.Y);
						this.SelectionArea.Position2.Y = this.SelectionArea.Position1.Y - size.Y + 1f;
					}
					else
					{
						this.SelectionArea.Position2.Y = (float)SelectionTool.FloorInt(projectedCursorPosition.Y);
						this.SelectionArea.Position1.Y = this.SelectionArea.Position2.Y - size.Y + 1f;
					}
				}
				else
				{
					bool flag4 = this._resizeDirection == SelectionTool.Direction.Down;
					if (flag4)
					{
						bool flag5 = this.SelectionArea.Position1.Y < this.SelectionArea.Position2.Y;
						if (flag5)
						{
							this.SelectionArea.Position1.Y = (float)SelectionTool.FloorInt(projectedCursorPosition.Y);
							this.SelectionArea.Position2.Y = this.SelectionArea.Position1.Y + size.Y - 1f;
						}
						else
						{
							this.SelectionArea.Position2.Y = (float)SelectionTool.FloorInt(projectedCursorPosition.Y);
							this.SelectionArea.Position1.Y = this.SelectionArea.Position2.Y - size.Y + 1f;
						}
					}
					else
					{
						bool flag6 = this._resizeDirection == SelectionTool.Direction.Left;
						if (flag6)
						{
							bool flag7 = this.SelectionArea.Position1.X < this.SelectionArea.Position2.X;
							if (flag7)
							{
								this.SelectionArea.Position1.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
								this.SelectionArea.Position2.X = this.SelectionArea.Position1.X + size.X - 1f;
							}
							else
							{
								this.SelectionArea.Position2.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
								this.SelectionArea.Position1.X = this.SelectionArea.Position2.X - size.X + 1f;
							}
						}
						else
						{
							bool flag8 = this._resizeDirection == SelectionTool.Direction.Right;
							if (flag8)
							{
								bool flag9 = this.SelectionArea.Position1.X > this.SelectionArea.Position2.X;
								if (flag9)
								{
									this.SelectionArea.Position1.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
									this.SelectionArea.Position2.X = this.SelectionArea.Position1.X - size.X + 1f;
								}
								else
								{
									this.SelectionArea.Position2.X = (float)SelectionTool.FloorInt(projectedCursorPosition.X);
									this.SelectionArea.Position1.X = this.SelectionArea.Position2.X - size.X + 1f;
								}
							}
							else
							{
								bool flag10 = this._resizeDirection == SelectionTool.Direction.Forward;
								if (flag10)
								{
									bool flag11 = this.SelectionArea.Position1.Z > this.SelectionArea.Position2.Z;
									if (flag11)
									{
										this.SelectionArea.Position2.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
										this.SelectionArea.Position1.Z = this.SelectionArea.Position2.Z + size.Z - 1f;
									}
									else
									{
										this.SelectionArea.Position1.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
										this.SelectionArea.Position2.Z = this.SelectionArea.Position1.Z - size.Z + 1f;
									}
								}
								else
								{
									bool flag12 = this._resizeDirection == SelectionTool.Direction.Backward;
									if (flag12)
									{
										bool flag13 = this.SelectionArea.Position1.Z < this.SelectionArea.Position2.Z;
										if (flag13)
										{
											this.SelectionArea.Position2.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
											this.SelectionArea.Position1.Z = this.SelectionArea.Position2.Z - size.Z + 1f;
										}
										else
										{
											this.SelectionArea.Position1.Z = (float)SelectionTool.FloorInt(projectedCursorPosition.Z);
											this.SelectionArea.Position2.Z = this.SelectionArea.Position1.Z - size.Z + 1f;
										}
									}
								}
							}
						}
					}
				}
				this.SelectionArea.IsSelectionDirty = true;
			}
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x00156003 File Offset: 0x00154203
		protected override void DoDispose()
		{
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x00156008 File Offset: 0x00154208
		private bool UpdateSelectionHighlight(bool reverse = false)
		{
			Vector3 position = this._gameInstance.CameraModule.Controller.Position;
			Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
			Quaternion rotation2 = Quaternion.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f);
			Vector3 vector = Vector3.Transform(Vector3.Forward, rotation2);
			if (reverse)
			{
				vector = -vector;
			}
			bool flag = this._gameInstance.Input.IsShiftHeld();
			if (flag)
			{
				BoundingBox box = new BoundingBox(this.SelectionArea.Position1, this.SelectionArea.Position1 + Vector3.One);
				bool flag2 = HitDetection.CheckRayBoxCollision(box, position, vector, out this._rayBoxHit, true);
				if (flag2)
				{
					this.HoverMode = SelectionTool.EditMode.ResizePos1;
				}
				else
				{
					box = new BoundingBox(this.SelectionArea.Position2, this.SelectionArea.Position2 + Vector3.One);
					bool flag3 = HitDetection.CheckRayBoxCollision(box, position, vector, out this._rayBoxHit, true);
					if (flag3)
					{
						this.HoverMode = SelectionTool.EditMode.ResizePos2;
					}
					else
					{
						this.HoverMode = SelectionTool.EditMode.None;
					}
				}
			}
			else
			{
				this.HoverMode = SelectionTool.EditMode.None;
			}
			bool flag4 = this.SelectionArea.IsSelectionDefined();
			if (flag4)
			{
				bool flag5 = HitDetection.CheckRayBoxCollision(this.SelectionArea.GetBoundsExclusiveMax(), position, vector, out this._rayBoxHit, true);
				if (flag5)
				{
					return this.IsCursorOverSelection = true;
				}
			}
			return this.IsCursorOverSelection = false;
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0015618C File Offset: 0x0015438C
		public bool FaceHighlightNeedsDrawing()
		{
			bool flag = this.Mode == SelectionTool.EditMode.MoveSide || this.Mode == SelectionTool.EditMode.ResizeSide;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2;
				if (this.Mode == SelectionTool.EditMode.None && this.HoverMode == SelectionTool.EditMode.None)
				{
					ToolInstance activeTool = this._builderTools.ActiveTool;
					string a;
					if (activeTool == null)
					{
						a = null;
					}
					else
					{
						BuilderTool builderTool = activeTool.BuilderTool;
						a = ((builderTool != null) ? builderTool.Id : null);
					}
					flag2 = (a != this.ToolId);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				result = (!flag3 && (this._rayBoxHit.Normal != Vector3.Zero && this.IsCursorOverSelection) && (this._gameInstance.Input.IsShiftHeld() || this._gameInstance.Input.IsAltHeld()));
			}
			return result;
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x00156250 File Offset: 0x00154450
		private Vector3 GetProjectedCursorPosition()
		{
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			Vector3 vector = this._resizeOrigin - lookRay.Position;
			bool flag = this._resizeDirection == SelectionTool.Direction.Up || this._resizeDirection == SelectionTool.Direction.Down;
			Vector2 ray1Position;
			Vector2 ray1Direction;
			Vector2 ray2Position;
			Vector2 ray2Direction;
			if (flag)
			{
				float num = vector.X;
				vector.X = vector.Z;
				vector.Z = -num;
				ray1Position = new Vector2(lookRay.Position.X, lookRay.Position.Z);
				ray1Direction = new Vector2(lookRay.Direction.X, lookRay.Direction.Z);
				ray2Position = new Vector2(this._resizeOrigin.X, this._resizeOrigin.Z);
				ray2Direction = new Vector2(vector.X, vector.Z);
			}
			else
			{
				bool flag2 = this._resizeDirection == SelectionTool.Direction.Left || this._resizeDirection == SelectionTool.Direction.Right;
				if (flag2)
				{
					float num = vector.Y;
					vector.Y = vector.Z;
					vector.Z = -num;
					ray1Position = new Vector2(lookRay.Position.Y, lookRay.Position.Z);
					ray1Direction = new Vector2(lookRay.Direction.Y, lookRay.Direction.Z);
					ray2Position = new Vector2(this._resizeOrigin.Y, this._resizeOrigin.Z);
					ray2Direction = new Vector2(vector.Y, vector.Z);
				}
				else
				{
					float num = vector.X;
					vector.X = vector.Y;
					vector.Y = -num;
					ray1Position = new Vector2(lookRay.Position.X, lookRay.Position.Y);
					ray1Direction = new Vector2(lookRay.Direction.X, lookRay.Direction.Y);
					ray2Position = new Vector2(this._resizeOrigin.X, this._resizeOrigin.Y);
					ray2Direction = new Vector2(vector.X, vector.Y);
				}
			}
			Vector2 vector2;
			bool flag3 = HitDetection.Get2DRayIntersection(ray1Position, ray1Direction, ray2Position, ray2Direction, out vector2);
			Vector3 result;
			if (flag3)
			{
				bool flag4 = this._resizeDirection == SelectionTool.Direction.Up || this._resizeDirection == SelectionTool.Direction.Down;
				if (flag4)
				{
					float num2 = (vector2.X - lookRay.Position.X) / lookRay.Direction.X;
					result = new Vector3(vector2.X, num2 * lookRay.Direction.Y + lookRay.Position.Y, vector2.Y);
				}
				else
				{
					bool flag5 = this._resizeDirection == SelectionTool.Direction.Left || this._resizeDirection == SelectionTool.Direction.Right;
					if (flag5)
					{
						float num2 = (vector2.Y - lookRay.Position.Z) / lookRay.Direction.Z;
						result = new Vector3(num2 * lookRay.Direction.X + lookRay.Position.X, vector2.X, vector2.Y);
					}
					else
					{
						float num2 = (vector2.X - lookRay.Position.X) / lookRay.Direction.X;
						result = new Vector3(vector2.X, vector2.Y, num2 * lookRay.Direction.Z + lookRay.Position.Z);
					}
				}
			}
			else
			{
				result = lookRay.Position;
			}
			return result;
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x001565CC File Offset: 0x001547CC
		public void NextDrawMode()
		{
			bool flag = this._selectionDrawMode == SelectionToolRenderer.SelectionDrawMode.Subtract;
			if (flag)
			{
				this._selectionDrawMode = SelectionToolRenderer.SelectionDrawMode.Normal;
			}
			else
			{
				this._selectionDrawMode++;
			}
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x001565FE File Offset: 0x001547FE
		private void OnGeneralAction(BuilderToolAction action)
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolGeneralAction(action));
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x00156618 File Offset: 0x00154818
		private static int FloorInt(float v)
		{
			return (int)Math.Floor((double)v);
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x00156624 File Offset: 0x00154824
		private static SelectionTool.Direction GetVectorDirection(Vector3 vector)
		{
			bool flag = vector == Vector3.Up;
			SelectionTool.Direction result;
			if (flag)
			{
				result = SelectionTool.Direction.Up;
			}
			else
			{
				bool flag2 = vector == Vector3.Down;
				if (flag2)
				{
					result = SelectionTool.Direction.Down;
				}
				else
				{
					bool flag3 = vector == Vector3.Left;
					if (flag3)
					{
						result = SelectionTool.Direction.Left;
					}
					else
					{
						bool flag4 = vector == Vector3.Right;
						if (flag4)
						{
							result = SelectionTool.Direction.Right;
						}
						else
						{
							bool flag5 = vector == Vector3.Forward;
							if (flag5)
							{
								result = SelectionTool.Direction.Forward;
							}
							else
							{
								bool flag6 = vector == Vector3.Backward;
								if (flag6)
								{
									result = SelectionTool.Direction.Backward;
								}
								else
								{
									result = SelectionTool.Direction.None;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04002909 RID: 10505
		public Vector3 Color = Vector3.One;

		// Token: 0x0400290D RID: 10509
		private SelectionToolRenderer.SelectionDrawMode _selectionDrawMode = SelectionToolRenderer.SelectionDrawMode.Normal;

		// Token: 0x0400290E RID: 10510
		private HitDetection.RayBoxCollision _rayBoxHit;

		// Token: 0x0400290F RID: 10511
		private Vector3 _resizePosition1;

		// Token: 0x04002910 RID: 10512
		private Vector3 _resizePosition2;

		// Token: 0x04002911 RID: 10513
		private Vector3 _resizeNormal;

		// Token: 0x04002912 RID: 10514
		private Vector3 _resizeOrigin;

		// Token: 0x04002913 RID: 10515
		private float _resizeDistance;

		// Token: 0x04002914 RID: 10516
		private SelectionTool.Direction _resizeDirection = SelectionTool.Direction.None;

		// Token: 0x04002915 RID: 10517
		private readonly Dictionary<SelectionTool.Keybind, SDL.SDL_Scancode> _keybinds = new Dictionary<SelectionTool.Keybind, SDL.SDL_Scancode>
		{
			{
				SelectionTool.Keybind.SelectionShiftUp,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP
			},
			{
				SelectionTool.Keybind.SelectionShiftDown,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN
			},
			{
				SelectionTool.Keybind.SelectionPosOne,
				SDL.SDL_Scancode.SDL_SCANCODE_LEFTBRACKET
			},
			{
				SelectionTool.Keybind.SelectionPosTwo,
				SDL.SDL_Scancode.SDL_SCANCODE_RIGHTBRACKET
			},
			{
				SelectionTool.Keybind.SelectionCopy,
				SDL.SDL_Scancode.SDL_SCANCODE_C
			},
			{
				SelectionTool.Keybind.SelectionClear,
				SDL.SDL_Scancode.SDL_SCANCODE_DELETE
			},
			{
				SelectionTool.Keybind.SelectionNextDrawMode,
				SDL.SDL_Scancode.SDL_SCANCODE_COMMA
			},
			{
				SelectionTool.Keybind.SelectionNextSet,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP
			},
			{
				SelectionTool.Keybind.SelectionPreviousSet,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN
			}
		};

		// Token: 0x04002916 RID: 10518
		public SelectionArea SelectionArea;

		// Token: 0x04002917 RID: 10519
		private long _toolDelayTime = 0L;

		// Token: 0x02000E7A RID: 3706
		private enum Keybind
		{
			// Token: 0x040046A3 RID: 18083
			SelectionShiftUp,
			// Token: 0x040046A4 RID: 18084
			SelectionShiftDown,
			// Token: 0x040046A5 RID: 18085
			SelectionPosOne,
			// Token: 0x040046A6 RID: 18086
			SelectionPosTwo,
			// Token: 0x040046A7 RID: 18087
			SelectionCopy,
			// Token: 0x040046A8 RID: 18088
			SelectionClear,
			// Token: 0x040046A9 RID: 18089
			SelectionNextDrawMode,
			// Token: 0x040046AA RID: 18090
			SelectionNextSet,
			// Token: 0x040046AB RID: 18091
			SelectionPreviousSet
		}

		// Token: 0x02000E7B RID: 3707
		private enum Direction
		{
			// Token: 0x040046AD RID: 18093
			None,
			// Token: 0x040046AE RID: 18094
			Up,
			// Token: 0x040046AF RID: 18095
			Down,
			// Token: 0x040046B0 RID: 18096
			Left,
			// Token: 0x040046B1 RID: 18097
			Right,
			// Token: 0x040046B2 RID: 18098
			Forward,
			// Token: 0x040046B3 RID: 18099
			Backward
		}

		// Token: 0x02000E7C RID: 3708
		public enum EditMode
		{
			// Token: 0x040046B5 RID: 18101
			None,
			// Token: 0x040046B6 RID: 18102
			MoveSide,
			// Token: 0x040046B7 RID: 18103
			MovePos1,
			// Token: 0x040046B8 RID: 18104
			MovePos2,
			// Token: 0x040046B9 RID: 18105
			ResizeSide,
			// Token: 0x040046BA RID: 18106
			ResizePos1,
			// Token: 0x040046BB RID: 18107
			ResizePos2
		}
	}
}

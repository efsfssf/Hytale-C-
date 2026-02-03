using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Core;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Gizmos.Models;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Brush
{
	// Token: 0x02000991 RID: 2449
	internal class BrushTool : Disposable
	{
		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06004E2B RID: 20011 RVA: 0x001586BD File Offset: 0x001568BD
		public bool UseBlockShapeRendering
		{
			get
			{
				return this._gameInstance.BuilderToolsModule.builderToolsSettings.EnableBrushShapeRendering;
			}
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x001586D4 File Offset: 0x001568D4
		public BrushTool(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._graphics = gameInstance.Engine.Graphics;
			this._renderer = new BrushToolRenderer(this._graphics);
			this._shapeRenderer = new BlockShapeRenderer(this._graphics, (int)this._graphics.GPUProgramStore.BuilderToolProgram.AttribPosition.Index, (int)this._graphics.GPUProgramStore.BuilderToolProgram.AttribTexCoords.Index);
			this._boxRenderer = new BoxRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			Vector3 value = new Vector3(0.05f, 0.05f, 0.05f);
			this._blockBox = new BoundingBox(Vector3.Zero - value, Vector3.One + value);
			this._brushAxisLockPlane = new BrushAxisLockPlane(gameInstance);
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x00158838 File Offset: 0x00156A38
		protected override void DoDispose()
		{
			this._boxRenderer.Dispose();
			this._shapeRenderer.Dispose();
			this._renderer.Dispose();
			this._brushAxisLockPlane.Dispose();
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x0015886C File Offset: 0x00156A6C
		public void Update(float deltaTime)
		{
			bool flag = this._gameInstance.GameMode != 1;
			if (!flag)
			{
				this._brushAxisLockPlane.Update(deltaTime);
			}
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x001588A0 File Offset: 0x00156AA0
		public void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this._brushAxisLockPlane.OnInteract(interactionType, clickType, firstRun);
			if (!flag)
			{
				this.InitializeBrushOnInteraction(context, firstRun);
				bool flag2 = this.CheckForClickHoldRelease(interactionType, clickType, context, firstRun);
				if (!flag2)
				{
					bool flag3 = this.UseBrush(interactionType, clickType, context, firstRun);
					if (flag3)
					{
					}
				}
			}
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x001588F0 File Offset: 0x00156AF0
		private void InitializeBrushOnInteraction(InteractionContext context, bool firstRun)
		{
			bool flag = !firstRun || this._gameInstance.BuilderToolsModule.BrushTargetPosition.IsNaN();
			if (!flag)
			{
				this._gameInstance.LocalPlayer.UsePrimaryItem();
				this.isHoldingDownBrush = true;
				bool flag2 = this.lockMode > BrushTool.LockMode.None;
				if (flag2)
				{
					this.lockModeActive = true;
					bool flag3 = this.lockMode == BrushTool.LockMode.OnHold;
					if (flag3)
					{
						this.initialBlockPosition = this._gameInstance.BuilderToolsModule.BrushTargetPosition;
					}
				}
				context.State.State = 4;
			}
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x00158984 File Offset: 0x00156B84
		private bool CheckForClickHoldRelease(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = !this.isHoldingDownBrush;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = clickType == InteractionModule.ClickType.None;
				if (flag2)
				{
					this.isHoldingDownBrush = false;
					context.State.State = 0;
					this.lockModeActive = (this.lockMode == BrushTool.LockMode.Always);
					string soundEventId = (interactionType == null || this.useServerRaytrace) ? "CREATE_BRUSH_RELEASE" : "CREATE_BRUSH_STAMP_RELEASE";
					this._gameInstance.AudioModule.PlayLocalSoundEvent(soundEventId);
					result = true;
				}
				else
				{
					context.State.State = 4;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x00158A10 File Offset: 0x00156C10
		private bool UseBrush(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			BuilderToolsModule builderToolsModule = this._gameInstance.BuilderToolsModule;
			bool flag = builderToolsModule.BrushTargetPosition.IsNaN();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool enableBrushSpacing = this._gameInstance.App.Settings.EnableBrushSpacing;
				if (enableBrushSpacing)
				{
					float num = Vector3.DistanceSquared(this.lastSuccessfulPosition, builderToolsModule.BrushTargetPosition);
					bool flag2 = num < (float)(this._gameInstance.App.Settings.BrushSpacingBlocks * this._gameInstance.App.Settings.BrushSpacingBlocks);
					if (flag2)
					{
						builderToolsModule.TimeOfLastToolInteraction = this.timeOfLastSuccessfulPlace;
						return true;
					}
					long num2 = DateTime.UtcNow.Ticks / 10000L;
					this.timeOfLastSuccessfulPlace = num2;
					this.lastSuccessfulPosition = new Vector3(builderToolsModule.BrushTargetPosition.X, builderToolsModule.BrushTargetPosition.Y, builderToolsModule.BrushTargetPosition.Z);
				}
				bool isAltPlaySculptBrushModDown = this._gameInstance.Input.IsBindingHeld(this._gameInstance.App.Settings.InputBindings.AlternatePlaySculptBrushModeModifier, false);
				this.SendServerInteraction(interactionType, clickType, context, isAltPlaySculptBrushModDown, firstRun);
				result = true;
			}
			return result;
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x00158B50 File Offset: 0x00156D50
		private void SendServerInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool isAltPlaySculptBrushModDown, bool firstRun)
		{
			BuilderToolsModule builderToolsModule = this._gameInstance.BuilderToolsModule;
			Vector3 brushTargetPosition = builderToolsModule.BrushTargetPosition;
			bool flag = (!this._gameInstance.Input.IsAltHeld() || !this._gameInstance.App.Settings.PlaceBlocksAtRange(this._gameInstance.GameMode)) && this.useServerRaytrace && !this.lockModeActive;
			this._gameInstance.Connection.SendPacket(new BuilderToolOnUseInteraction(interactionType, (int)brushTargetPosition.X, (int)brushTargetPosition.Y, (int)brushTargetPosition.Z, builderToolsModule.ToolVectorOffset.X, builderToolsModule.ToolVectorOffset.Y, builderToolsModule.ToolVectorOffset.Z, isAltPlaySculptBrushModDown, clickType == InteractionModule.ClickType.Held, flag, this._gameInstance.App.Settings.BuilderToolsSettings.ShowBuilderToolsNotifications, this._gameInstance.App.Settings.PaintOperationsIgnoreHistoryLength));
			if (firstRun)
			{
				bool flag2 = interactionType == 0;
				string soundEventId;
				if (flag2)
				{
					soundEventId = "CREATE_BRUSH_ERASE";
				}
				else
				{
					soundEventId = (flag ? "CREATE_BRUSH_PAINT" : "CREATE_BRUSH_STAMP");
				}
				this._gameInstance.AudioModule.PlayLocalSoundEvent(soundEventId);
			}
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x00158C80 File Offset: 0x00156E80
		public void Draw(ref Matrix viewProjectionMatrix, Vector3 position, float opacity)
		{
			bool flag = position.IsNaN();
			if (!flag)
			{
				GLFunctions gl = this._graphics.GL;
				bool flag2 = !this.UseBlockShapeRendering;
				if (flag2)
				{
					ForceFieldProgram builderToolProgram = this._graphics.GPUProgramStore.BuilderToolProgram;
					gl.UseProgram(builderToolProgram);
					this._renderer.Draw(ref viewProjectionMatrix, ref this._gameInstance.SceneRenderer.Data.ViewRotationMatrix, this._gameInstance.SceneRenderer.Data.ViewportSize, position, this._graphics.BlackColor, opacity, this._gameInstance.BuilderToolsModule.DrawHighlightAndUndergroundColor);
					gl.UseProgram(this._graphics.GPUProgramStore.BasicProgram);
				}
				else
				{
					this._boxRenderer.Draw(position, this._blockBox, viewProjectionMatrix, this.BrushColor, 0.25f, this._graphics.WhiteColor, 0.1f);
					Vector3 value = this._gameInstance.SceneRenderer.Data.CameraDirection * 0.06f;
					Vector3 vector = -value;
					Matrix matrix;
					Matrix.CreateTranslation(ref vector, out matrix);
					Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ViewRotationMatrix, out matrix);
					Matrix matrix2;
					Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ProjectionMatrix, out matrix2);
					ForceFieldProgram builderToolProgram2 = this._graphics.GPUProgramStore.BuilderToolProgram;
					gl.UseProgram(builderToolProgram2);
					Matrix matrix3;
					Matrix.CreateTranslation(ref position, out matrix3);
					builderToolProgram2.ModelMatrix.SetValue(ref matrix3);
					builderToolProgram2.ViewMatrix.SetValue(ref this._gameInstance.SceneRenderer.Data.ViewRotationMatrix);
					builderToolProgram2.ViewProjectionMatrix.SetValue(ref viewProjectionMatrix);
					builderToolProgram2.CurrentInvViewportSize.SetValue(this._gameInstance.SceneRenderer.Data.InvViewportSize);
					Matrix matrix4 = Matrix.Transpose(Matrix.Invert(matrix3));
					builderToolProgram2.NormalMatrix.SetValue(ref matrix4);
					builderToolProgram2.UVAnimationSpeed.SetValue(0f, 0f);
					builderToolProgram2.OutlineMode.SetValue(builderToolProgram2.OutlineModeNone);
					builderToolProgram2.DrawAndBlendMode.SetValue(builderToolProgram2.DrawModeColor, builderToolProgram2.BlendModeLinear);
					Vector4 value2 = new Vector4(1f, 1f, 1f, 0.5f);
					float value3 = this._gameInstance.BuilderToolsModule.DrawHighlightAndUndergroundColor ? 0.2f : 0f;
					builderToolProgram2.IntersectionHighlightColorOpacity.SetValue(value2);
					builderToolProgram2.IntersectionHighlightThickness.SetValue(value3);
					builderToolProgram2.ColorOpacity.SetValue(1f, 1f, 1f, 0f);
					this._shapeRenderer.DrawBlockShape();
					this._graphics.SaveColorMask();
					gl.DepthMask(true);
					gl.ColorMask(false, false, false, false);
					builderToolProgram2.ColorOpacity.SetValue(Vector4.One);
					matrix3.M42 -= 0.1f;
					builderToolProgram2.ModelMatrix.SetValue(ref matrix3);
					gl.DepthFunc(GL.ALWAYS);
					this._shapeRenderer.DrawBlockShape();
					gl.DepthFunc(GL.LEQUAL);
					this._shapeRenderer.DrawBlockShape();
					gl.DepthMask(false);
					this._graphics.RestoreColorMask();
					builderToolProgram2.IntersectionHighlightColorOpacity.SetValue(1f, 1f, 1f, 0f);
					builderToolProgram2.ColorOpacity.SetValue(0f, 0f, 0f, opacity);
					this._shapeRenderer.DrawBlockShape();
					gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
					builderToolProgram2.ViewProjectionMatrix.SetValue(ref matrix2);
					builderToolProgram2.ColorOpacity.SetValue(1f, 1f, 1f, opacity - 0.1f);
					this._shapeRenderer.DrawBlockShapeOutline();
					gl.UseProgram(this._graphics.GPUProgramStore.BasicProgram);
				}
			}
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x001590B8 File Offset: 0x001572B8
		public void UpdateBrushData(BrushData brushData, bool force = false)
		{
			bool flag = !force && (brushData == null || brushData.Equals(this._brushData));
			if (!flag)
			{
				this._brushData = brushData;
				bool flag2 = !this.UseBlockShapeRendering;
				if (flag2)
				{
					this._renderer.UpdateBrushData(brushData, null);
				}
				else
				{
					int num = (int)((float)brushData.Width * 0.5f);
					int num2 = (int)((float)brushData.Height * 0.5f);
					int num3 = (int)((float)brushData.Width * 0.5f);
					bool[,,] blockData = new bool[0, 0, 0];
					switch (brushData.Shape)
					{
					case 0:
						blockData = CubeModel.BuildVoxelData(num, num2, num3);
						break;
					case 1:
						blockData = SphereModel.BuildVoxelData(num, num2, num3);
						break;
					case 2:
						blockData = CylinderModel.BuildVoxelData(num, num2 * 2, num3);
						break;
					case 3:
						blockData = ConeModel.BuildVoxelData(num, (num2 == 0) ? 1 : (num2 * 2), num3);
						break;
					case 4:
						blockData = ConeModel.BuildInvertedVoxelData(num, (num2 == 0) ? 1 : (num2 * 2), num3);
						break;
					case 5:
						blockData = PyramidModel.BuildVoxelData(num, (num2 == 0) ? 1 : (num2 * 2), num3);
						break;
					case 6:
						blockData = PyramidModel.BuildInvertedVoxelData(num, (num2 == 0) ? 1 : (num2 * 2), num3);
						break;
					}
					int num4 = 0;
					bool flag3 = this._brushData.Origin == 1;
					if (flag3)
					{
						num4 = num2 + 1;
					}
					else
					{
						bool flag4 = this._brushData.Origin == 2;
						if (flag4)
						{
							num4 = -num2;
						}
					}
					this._shapeRenderer.UpdateModelData(blockData, -num, -num2 + num4, -num3);
				}
			}
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x0015924C File Offset: 0x0015744C
		public void OnKeyDown()
		{
			BrushTool.<>c__DisplayClass36_0 CS$<>8__locals1;
			CS$<>8__locals1.input = this._gameInstance.Input;
			this._brushAxisLockPlane.OnKeyDown();
			bool flag = CS$<>8__locals1.input.ConsumeBinding(this._gameInstance.App.Settings.InputBindings.NextBrushLockAxisOrPlane, false);
			if (flag)
			{
				bool flag2 = this.lockMode != BrushTool.LockMode.Always;
				if (flag2)
				{
					BrushTool.AxisAndPlanes[] array = (BrushTool.AxisAndPlanes[])Enum.GetValues(typeof(BrushTool.AxisAndPlanes));
					int num = Array.IndexOf<BrushTool.AxisAndPlanes>(array, this.unlockedAxis);
					this.unlockedAxis = array[(num + 1) % array.Length];
					this._gameInstance.Chat.Log(string.Format("Set unlocked axis or plane to '{0}'.", this.unlockedAxis));
				}
				else
				{
					this._gameInstance.Chat.Log("Cannot change locked axis/plane while in Lock Mode: 'Always'");
				}
			}
			bool flag3 = this._gameInstance.Input.ConsumeBinding(this._gameInstance.App.Settings.InputBindings.UsePaintModeForBrush, false);
			if (flag3)
			{
				this.useServerRaytrace = !this.useServerRaytrace;
				this._gameInstance.Chat.Log("Set paint mode for PlayPaint Brush to: " + this.useServerRaytrace.ToString());
				this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_BRUSH_MODE");
			}
			bool flag4 = CS$<>8__locals1.input.ConsumeBinding(this._gameInstance.App.Settings.InputBindings.NextBrushLockMode, false);
			if (flag4)
			{
				BrushTool.LockMode[] array2 = (BrushTool.LockMode[])Enum.GetValues(typeof(BrushTool.LockMode));
				int num2 = Array.IndexOf<BrushTool.LockMode>(array2, this.lockMode);
				this.lockMode = array2[(num2 + 1) % array2.Length];
				this._gameInstance.Chat.Log(string.Format("Brush lock mode set to '{0}'", this.lockMode));
				bool flag5 = this.lockMode == BrushTool.LockMode.Always;
				if (flag5)
				{
					this.initialBlockPosition = this._gameInstance.BuilderToolsModule.BrushTargetPosition;
					this.lockModeActive = true;
				}
				else
				{
					this.lockModeActive = false;
				}
			}
			bool flag6 = BrushTool.<OnKeyDown>g__CheckKey|36_0(this._keybinds[BrushTool.Keybind.BrushIncreaseWidth], ref CS$<>8__locals1);
			if (flag6)
			{
				this.OffsetBrushWidth(2);
			}
			bool flag7 = BrushTool.<OnKeyDown>g__CheckKey|36_0(this._keybinds[BrushTool.Keybind.BrushDecreaseWidth], ref CS$<>8__locals1);
			if (flag7)
			{
				this.OffsetBrushWidth(-2);
			}
			bool flag8 = BrushTool.<OnKeyDown>g__CheckKey|36_0(this._keybinds[BrushTool.Keybind.BrushIncreaseHeight], ref CS$<>8__locals1);
			if (flag8)
			{
				this.OffsetBrushHeight(2);
			}
			bool flag9 = BrushTool.<OnKeyDown>g__CheckKey|36_0(this._keybinds[BrushTool.Keybind.BrushDecreaseHeight], ref CS$<>8__locals1);
			if (flag9)
			{
				this.OffsetBrushHeight(-2);
			}
			bool flag10 = CS$<>8__locals1.input.ConsumeKey(this._keybinds[BrushTool.Keybind.BrushNextShapeOrigin], false);
			if (flag10)
			{
				bool flag11 = CS$<>8__locals1.input.IsShiftHeld();
				if (flag11)
				{
					BrushOrigin brushOrigin = this._brushData.NextBrushOrigin(true);
					this._gameInstance.Chat.Log(string.Format("Set brush origin to: {0}", brushOrigin));
				}
				else
				{
					BrushShape brushShape = this._brushData.NextBrushShape(true);
					this._gameInstance.Chat.Log(string.Format("Set brush shape to: {0}", brushShape));
				}
				this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_BRUSH_SHAPE");
			}
			bool flag12 = CS$<>8__locals1.input.ConsumeKey(this._keybinds[BrushTool.Keybind.BrushPreviousShapeOrigin], false);
			if (flag12)
			{
				bool flag13 = CS$<>8__locals1.input.IsShiftHeld();
				if (flag13)
				{
					BrushOrigin brushOrigin2 = this._brushData.NextBrushOrigin(false);
					this._gameInstance.Chat.Log(string.Format("Set brush origin to: {0}", brushOrigin2));
				}
				else
				{
					BrushShape brushShape2 = this._brushData.NextBrushShape(false);
					this._gameInstance.Chat.Log(string.Format("Set brush shape to: {0}", brushShape2));
				}
				this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_BRUSH_SHAPE");
			}
			bool flag14 = CS$<>8__locals1.input.ConsumeKey(this._keybinds[BrushTool.Keybind.BrushToggleReachLock], false);
			if (flag14)
			{
				this._gameInstance.BuilderToolsModule.builderToolsSettings.ToolReachLock = !this._gameInstance.BuilderToolsModule.builderToolsSettings.ToolReachLock;
				this._gameInstance.App.Settings.Save();
			}
			bool flag15 = CS$<>8__locals1.input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_P, false);
			if (flag15)
			{
				BuilderToolsModule builderToolsModule = this._gameInstance.BuilderToolsModule;
				bool flag16 = !builderToolsModule.HasActiveBrush || builderToolsModule.BrushTargetPosition.IsNaN();
				if (!flag16)
				{
					int block = this._gameInstance.MapModule.GetBlock(builderToolsModule.BrushTargetPosition, int.MaxValue);
					bool flag17 = block == int.MaxValue;
					if (!flag17)
					{
						ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
						string text = (clientBlockType != null) ? clientBlockType.Name : null;
						string[] favoriteMaterials = this._brushData.FavoriteMaterials;
						bool flag18 = favoriteMaterials != null && Array.IndexOf<string>(favoriteMaterials, text) != -1;
						if (flag18)
						{
							List<string> list = new List<string>(favoriteMaterials);
							list.Remove(text);
							this._brushData.SetFavoriteMaterials(list.ToArray());
							this._gameInstance.Chat.Log("Removed from favorite materials: " + text);
						}
						else
						{
							bool flag19 = favoriteMaterials == null || favoriteMaterials.Length < 5;
							if (flag19)
							{
								List<string> list2 = (favoriteMaterials != null) ? new List<string>(favoriteMaterials) : new List<string>();
								list2.Add(text);
								this._brushData.SetFavoriteMaterials(list2.ToArray());
								this._gameInstance.Chat.Log("Added to favorite materials: " + text);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x0015982D File Offset: 0x00157A2D
		private void SetBrushShape(BrushShape shape)
		{
			this._brushData.SetBrushShape(shape);
			this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_BRUSH_SHAPE");
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x00159854 File Offset: 0x00157A54
		private void OffsetBrushHeight(int amount)
		{
			bool flag = this._brushData.OffsetBrushHeight(amount);
			if (flag)
			{
				this.OnBrushDimensionChanged(amount);
			}
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x0015987C File Offset: 0x00157A7C
		private void OffsetBrushWidth(int amount)
		{
			bool flag = this._brushData.OffsetBrushWidth(amount);
			if (flag)
			{
				this.OnBrushDimensionChanged(amount);
			}
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x001598A2 File Offset: 0x00157AA2
		private void OnBrushDimensionChanged(int amount)
		{
			this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_SCALE");
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x001598BC File Offset: 0x00157ABC
		public Vector3 GetLockedBrushPosition(out int distance)
		{
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			bool flag = this._brushAxisLockPlane.IsEnabled();
			if (flag)
			{
				Vector3 vector;
				bool flag2 = HitDetection.CheckRayPlaneIntersection(this._brushAxisLockPlane.GetPosition(), this._brushAxisLockPlane.GetNormal(), lookRay.Position, lookRay.Direction, out vector, true);
				bool flag3 = flag2;
				if (flag3)
				{
					distance = (int)Vector3.Distance(vector, lookRay.Position);
					return vector;
				}
			}
			Vector3 vector2 = this._gameInstance.InteractionModule.HasFoundTargetBlock ? this._gameInstance.InteractionModule.TargetBlockHit.BlockNormal : Vector3.Zero;
			Vector3 vector3 = this.initialBlockPosition;
			distance = int.MaxValue;
			Vector3 naN = Vector3.NaN;
			switch (this.unlockedAxis)
			{
			case BrushTool.AxisAndPlanes.X:
			{
				bool flag4 = vector2 == Vector3.Right || vector2 == Vector3.Left;
				if (flag4)
				{
					vector3.X -= vector2.X;
				}
				Vector3 planeNormal = Vector3.Subtract(lookRay.Position, new Vector3(lookRay.Position.X, vector3.Y, vector3.Z));
				planeNormal.Normalize();
				HitDetection.CheckRayPlaneIntersection(vector3, planeNormal, lookRay.Position, lookRay.Direction, out naN, false);
				naN.Y = vector3.Y;
				naN.Z = vector3.Z;
				break;
			}
			case BrushTool.AxisAndPlanes.Y:
			{
				bool flag5 = vector2 == Vector3.Up || vector2 == Vector3.Down;
				if (flag5)
				{
					vector3.Y -= vector2.Y;
				}
				Vector3 planeNormal = Vector3.Subtract(lookRay.Position, new Vector3(vector3.X, lookRay.Position.Y, vector3.Z));
				planeNormal.Normalize();
				HitDetection.CheckRayPlaneIntersection(vector3, planeNormal, lookRay.Position, lookRay.Direction, out naN, false);
				naN.X = vector3.X;
				naN.Z = vector3.Z;
				break;
			}
			case BrushTool.AxisAndPlanes.Z:
			{
				bool flag6 = vector2 == Vector3.Backward || vector2 == Vector3.Forward;
				if (flag6)
				{
					vector3.Z -= vector2.Z;
				}
				Vector3 planeNormal = Vector3.Subtract(lookRay.Position, new Vector3(vector3.X, vector3.Y, lookRay.Position.Z));
				planeNormal.Normalize();
				HitDetection.CheckRayPlaneIntersection(vector3, planeNormal, lookRay.Position, lookRay.Direction, out naN, false);
				naN.X = vector3.X;
				naN.Y = vector3.Y;
				break;
			}
			case BrushTool.AxisAndPlanes.XY:
			{
				bool flag7 = vector2 == Vector3.Right || vector2 == Vector3.Left;
				if (flag7)
				{
					vector3.X -= vector2.X;
				}
				else
				{
					bool flag8 = vector2 == Vector3.Up || vector2 == Vector3.Down;
					if (flag8)
					{
						vector3.Y -= vector2.Y;
					}
				}
				HitDetection.CheckRayPlaneIntersection(vector3, new Vector3(0f, 0f, 1f), lookRay.Position, lookRay.Direction, out naN, false);
				naN.Z = vector3.Z;
				break;
			}
			case BrushTool.AxisAndPlanes.XZ:
			{
				bool flag9 = vector2 == Vector3.Right || vector2 == Vector3.Left;
				if (flag9)
				{
					vector3.X -= vector2.X;
				}
				else
				{
					bool flag10 = vector2 == Vector3.Backward || vector2 == Vector3.Forward;
					if (flag10)
					{
						vector3.Z -= vector2.Z;
					}
				}
				HitDetection.CheckRayPlaneIntersection(vector3, new Vector3(0f, 1f, 0f), lookRay.Position, lookRay.Direction, out naN, false);
				naN.Y = vector3.Y;
				break;
			}
			case BrushTool.AxisAndPlanes.ZY:
			{
				bool flag11 = vector2 == Vector3.Up || vector2 == Vector3.Down;
				if (flag11)
				{
					vector3.Y -= vector2.Y;
				}
				else
				{
					bool flag12 = vector2 == Vector3.Backward || vector2 == Vector3.Forward;
					if (flag12)
					{
						vector3.Z -= vector2.Z;
					}
				}
				HitDetection.CheckRayPlaneIntersection(vector3, new Vector3(1f, 0f, 0f), lookRay.Position, lookRay.Direction, out naN, false);
				naN.X = vector3.X;
				break;
			}
			}
			bool flag13 = !naN.IsNaN();
			if (flag13)
			{
				distance = (int)Vector3.Distance(naN, lookRay.Position);
			}
			return Vector3.Floor(naN);
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x00159D91 File Offset: 0x00157F91
		[CompilerGenerated]
		internal static bool <OnKeyDown>g__CheckKey|36_0(SDL.SDL_Scancode code, ref BrushTool.<>c__DisplayClass36_0 A_1)
		{
			return A_1.input.IsShiftHeld() ? A_1.input.IsKeyHeld(code, false) : A_1.input.ConsumeKey(code, false);
		}

		// Token: 0x0400295D RID: 10589
		private const string CREATE_BRUSH_RELEASE_SOUNDID = "CREATE_BRUSH_RELEASE";

		// Token: 0x0400295E RID: 10590
		private const string CREATE_BRUSH_STAMP_RELEASE_SOUNDID = "CREATE_BRUSH_STAMP_RELEASE";

		// Token: 0x0400295F RID: 10591
		private const string CREATE_BRUSH_ERASE_SOUNDID = "CREATE_BRUSH_ERASE";

		// Token: 0x04002960 RID: 10592
		private const string CREATE_BRUSH_PAINT_SOUNDID = "CREATE_BRUSH_PAINT";

		// Token: 0x04002961 RID: 10593
		private const string CREATE_BRUSH_STAMP_SOUNDID = "CREATE_BRUSH_STAMP";

		// Token: 0x04002962 RID: 10594
		private const string CREATE_BRUSH_MODE_SOUNDID = "CREATE_BRUSH_MODE";

		// Token: 0x04002963 RID: 10595
		private readonly GameInstance _gameInstance;

		// Token: 0x04002964 RID: 10596
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002965 RID: 10597
		private readonly BrushToolRenderer _renderer;

		// Token: 0x04002966 RID: 10598
		private readonly BlockShapeRenderer _shapeRenderer;

		// Token: 0x04002967 RID: 10599
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x04002968 RID: 10600
		private readonly BoundingBox _blockBox;

		// Token: 0x04002969 RID: 10601
		public Vector3 BrushColor = Vector3.One;

		// Token: 0x0400296A RID: 10602
		public BrushData _brushData;

		// Token: 0x0400296B RID: 10603
		public Vector3 initialBlockPosition;

		// Token: 0x0400296C RID: 10604
		public BrushTool.AxisAndPlanes unlockedAxis = BrushTool.AxisAndPlanes.X;

		// Token: 0x0400296D RID: 10605
		public BrushTool.LockMode lockMode = BrushTool.LockMode.None;

		// Token: 0x0400296E RID: 10606
		public bool lockModeActive;

		// Token: 0x0400296F RID: 10607
		public Vector3 lastSuccessfulPosition;

		// Token: 0x04002970 RID: 10608
		public long timeOfLastSuccessfulPlace;

		// Token: 0x04002971 RID: 10609
		public bool isHoldingDownBrush;

		// Token: 0x04002972 RID: 10610
		public BrushAxisLockPlane _brushAxisLockPlane;

		// Token: 0x04002973 RID: 10611
		public bool useServerRaytrace = true;

		// Token: 0x04002974 RID: 10612
		private Dictionary<BrushTool.Keybind, SDL.SDL_Scancode> _keybinds = new Dictionary<BrushTool.Keybind, SDL.SDL_Scancode>
		{
			{
				BrushTool.Keybind.BrushToggleReachLock,
				SDL.SDL_Scancode.SDL_SCANCODE_KP_PERIOD
			},
			{
				BrushTool.Keybind.BrushToggleInvert,
				SDL.SDL_Scancode.SDL_SCANCODE_KP_MULTIPLY
			},
			{
				BrushTool.Keybind.BrushIncreaseWidth,
				SDL.SDL_Scancode.SDL_SCANCODE_RIGHT
			},
			{
				BrushTool.Keybind.BrushDecreaseWidth,
				SDL.SDL_Scancode.SDL_SCANCODE_LEFT
			},
			{
				BrushTool.Keybind.BrushIncreaseHeight,
				SDL.SDL_Scancode.SDL_SCANCODE_UP
			},
			{
				BrushTool.Keybind.BrushDecreaseHeight,
				SDL.SDL_Scancode.SDL_SCANCODE_DOWN
			},
			{
				BrushTool.Keybind.BrushNextShapeOrigin,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP
			},
			{
				BrushTool.Keybind.BrushPreviousShapeOrigin,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN
			}
		};

		// Token: 0x02000E80 RID: 3712
		private enum Keybind
		{
			// Token: 0x040046C7 RID: 18119
			BrushToggleReachLock,
			// Token: 0x040046C8 RID: 18120
			BrushToggleInvert,
			// Token: 0x040046C9 RID: 18121
			BrushIncreaseWidth,
			// Token: 0x040046CA RID: 18122
			BrushDecreaseWidth,
			// Token: 0x040046CB RID: 18123
			BrushIncreaseHeight,
			// Token: 0x040046CC RID: 18124
			BrushDecreaseHeight,
			// Token: 0x040046CD RID: 18125
			BrushIncreaseParam,
			// Token: 0x040046CE RID: 18126
			BrushDecreaseParam,
			// Token: 0x040046CF RID: 18127
			BrushSphereShape,
			// Token: 0x040046D0 RID: 18128
			BrushCubeShape,
			// Token: 0x040046D1 RID: 18129
			BrushCylinderShape,
			// Token: 0x040046D2 RID: 18130
			BrushConeShape,
			// Token: 0x040046D3 RID: 18131
			BrushPyramidShape,
			// Token: 0x040046D4 RID: 18132
			BrushNextShapeOrigin,
			// Token: 0x040046D5 RID: 18133
			BrushPreviousShapeOrigin
		}

		// Token: 0x02000E81 RID: 3713
		public enum LockMode
		{
			// Token: 0x040046D7 RID: 18135
			None,
			// Token: 0x040046D8 RID: 18136
			OnHold,
			// Token: 0x040046D9 RID: 18137
			Always
		}

		// Token: 0x02000E82 RID: 3714
		public enum AxisAndPlanes
		{
			// Token: 0x040046DB RID: 18139
			X,
			// Token: 0x040046DC RID: 18140
			Y,
			// Token: 0x040046DD RID: 18141
			Z,
			// Token: 0x040046DE RID: 18142
			XY,
			// Token: 0x040046DF RID: 18143
			XZ,
			// Token: 0x040046E0 RID: 18144
			ZY
		}
	}
}

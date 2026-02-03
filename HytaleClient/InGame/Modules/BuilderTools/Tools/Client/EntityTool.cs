using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x02000983 RID: 2435
	internal class EntityTool : ClientTool
	{
		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06004D50 RID: 19792 RVA: 0x0014B632 File Offset: 0x00149832
		public override string ToolId
		{
			get
			{
				return "Entity";
			}
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x0014B63C File Offset: 0x0014983C
		public EntityTool(GameInstance gameInstance) : base(gameInstance)
		{
			this._rotationGizmo = new RotationGizmo(this._graphics, this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont, new RotationGizmo.OnRotationChange(this.OnRotationChange), 0.2617994f);
			this._translationGizmo = new TranslationGizmo(this._graphics, new TranslationGizmo.OnPositionChange(this.OnPositionChange));
			this._boxRenderer = new BoxRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._textRenderer = new TextRenderer(this._graphics, this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont, this.ToolId, uint.MaxValue, 4278190080U);
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x0014B75E File Offset: 0x0014995E
		protected override void DoDispose()
		{
			this._boxRenderer.Dispose();
			this._translationGizmo.Dispose();
			this._rotationGizmo.Dispose();
			this._textRenderer.Dispose();
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0014B794 File Offset: 0x00149994
		public override void Update(float deltaTime)
		{
			Input input = this._gameInstance.Input;
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			float targetBlockHitDistance = this._gameInstance.InteractionModule.HasFoundTargetBlock ? this._gameInstance.InteractionModule.TargetBlockHit.Distance : 0f;
			this._translationGizmo.Tick(lookRay);
			this._rotationGizmo.Tick(lookRay, targetBlockHitDistance);
			this._rotationGizmo.UpdateRotation(input.IsShiftHeld());
			bool flag = this._selectedEntity != null && this._editMode == ToolMode.FreeMove;
			if (flag)
			{
				bool flag2 = this._lockToSurface && this._gameInstance.InteractionModule.HasFoundTargetBlock;
				Vector3 position;
				if (flag2)
				{
					position = this._gameInstance.InteractionModule.TargetBlockHit.HitPosition;
				}
				else
				{
					position = lookRay.Position + lookRay.Direction * this._targetDistance - this._moveOffset;
				}
				this.OnPositionChange(position);
			}
			HitDetection.EntityHitData entityHitData;
			this._gameInstance.HitDetection.RaycastEntity(lookRay.Position, lookRay.Direction, 100f, false, out entityHitData);
			this._hoveredEntity = entityHitData.Entity;
			bool flag3 = this._hoveredEntity != null && this._hoveredEntity.Type == Entity.EntityType.Character;
			if (flag3)
			{
				BoundingBox headBox = EntityTool._headBox;
				headBox.Translate(this._hoveredEntity.Position + new Vector3(0f, this._hoveredEntity.EyeOffset, 0f));
				HitDetection.RayBoxCollision rayBoxCollision;
				this._headSelected = HitDetection.CheckRayBoxCollision(headBox, lookRay.Position, lookRay.Direction, out rayBoxCollision, false);
			}
			else
			{
				this._headSelected = false;
			}
			this._textRenderer.Text = this.GetDisplayText();
			bool flag4 = this._gameInstance.Input.IsAnyKeyHeld(false);
			if (flag4)
			{
				this.OnKeyDown();
			}
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0014B98C File Offset: 0x00149B8C
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			GLFunctions gl = this._graphics.GL;
			gl.DepthMask(true);
			Vector3 cameraPosition = this._gameInstance.SceneRenderer.Data.CameraPosition;
			bool visible = this._rotationGizmo.Visible;
			if (visible)
			{
				this._rotationGizmo.Draw(ref viewProjectionMatrix, this._gameInstance.CameraModule.Controller, -cameraPosition);
			}
			bool visible2 = this._translationGizmo.Visible;
			if (visible2)
			{
				this._translationGizmo.Draw(ref viewProjectionMatrix, -cameraPosition);
			}
			bool flag = (this._selectedEntity != null || this._hoveredEntity != null) && (this._editMode == ToolMode.None || this._editMode == ToolMode.FreeMove);
			if (flag)
			{
				Entity entity = this._hoveredEntity;
				Vector3 outlineColor = this._graphics.BlueColor;
				float quadOpacity = 0.3f;
				bool flag2 = this._selectedEntity != null;
				if (flag2)
				{
					entity = this._selectedEntity;
					outlineColor = this._graphics.RedColor;
					quadOpacity = 0.15f;
				}
				bool flag3 = entity.ModelRenderer == null || (entity.NetworkId == this._gameInstance.LocalPlayerNetworkId && this._gameInstance.CameraModule.Controller.IsFirstPerson);
				if (flag3)
				{
					return;
				}
				BoundingBox hitbox = entity.Hitbox;
				hitbox.Min -= EntityTool._boxOffset;
				hitbox.Max += EntityTool._boxOffset;
				bool headSelected = this._headSelected;
				if (headSelected)
				{
					this._boxRenderer.Draw(entity.Position + new Vector3(0f, entity.EyeOffset, 0f) - cameraPosition, EntityTool._headBox, viewProjectionMatrix, outlineColor, 0.5f, this._graphics.WhiteColor, quadOpacity);
				}
				else
				{
					this._boxRenderer.Draw(entity.Position - cameraPosition, hitbox, viewProjectionMatrix, outlineColor, 0.7f, this._graphics.WhiteColor, quadOpacity);
				}
			}
			gl.DepthMask(false);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0014BBC0 File Offset: 0x00149DC0
		public override void DrawText(ref Matrix viewProjectionMatrix)
		{
			base.DrawText(ref viewProjectionMatrix);
			this.PrepareForTextDraw(ref viewProjectionMatrix);
			bool flag = this._editMode == ToolMode.RotateBody || this._editMode == ToolMode.RotateHead;
			if (flag)
			{
				this._rotationGizmo.DrawText();
			}
			else
			{
				GLFunctions gl = this._graphics.GL;
				TextProgram textProgram = this._graphics.GPUProgramStore.TextProgram;
				textProgram.AssertInUse();
				textProgram.Position.SetValue(this._textPosition);
				textProgram.FillBlurThreshold.SetValue(this._fillBlurThreshold);
				textProgram.MVPMatrix.SetValue(ref this._textMatrix);
				gl.DepthFunc(GL.ALWAYS);
				this._textRenderer.Draw();
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			}
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0014BC9C File Offset: 0x00149E9C
		public override bool NeedsDrawing()
		{
			return this._selectedEntity != null || this._hoveredEntity != null;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0014BCC4 File Offset: 0x00149EC4
		public override bool NeedsTextDrawing()
		{
			return this.NeedsDrawing() || this._editMode == ToolMode.RotateBody || this._editMode == ToolMode.RotateHead;
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x0014BCF4 File Offset: 0x00149EF4
		private void PrepareForTextDraw(ref Matrix viewProjectionMatrix)
		{
			float scale = 0.2f / (float)this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont.BaseSize;
			int spread = this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont.Spread;
			float num = 1f / (float)spread;
			Vector3 vector = this._gameInstance.LocalPlayer.Position + new Vector3(0f, this._gameInstance.LocalPlayer.EyeOffset, 0f);
			Vector3 vector2 = (this._selectedEntity != null) ? this._selectedEntity.Position : ((this._hoveredEntity != null) ? this._hoveredEntity.Position : this._lastEntityPosition);
			vector2.Y = vector2.Y - 0.5f + ((!this._headSelected) ? 0f : ((this._selectedEntity != null) ? this._selectedEntity.EyeOffset : this._hoveredEntity.EyeOffset));
			this._textPosition = vector2 - vector;
			float num2 = Vector3.Distance(vector2, vector);
			this._fillBlurThreshold = MathHelper.Clamp(2f * num2 * 0.1f, 1f, (float)spread) * num;
			Matrix.CreateTranslation(-this._textRenderer.GetHorizontalOffset(TextRenderer.TextAlignment.Center), -this._textRenderer.GetVerticalOffset(TextRenderer.TextVerticalAlignment.Middle), 0f, out this._tempMatrix);
			Matrix.CreateScale(scale, out this._drawMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref this._drawMatrix, out this._drawMatrix);
			Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
			Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f, out this._tempMatrix);
			Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
			Matrix.AddTranslation(ref this._drawMatrix, vector2.X, vector2.Y, vector2.Z);
			Matrix.Multiply(ref this._drawMatrix, ref viewProjectionMatrix, out this._textMatrix);
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0014BF0C File Offset: 0x0014A10C
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = clickType == InteractionModule.ClickType.None;
			if (!flag)
			{
				Input input = this._gameInstance.Input;
				Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
				bool flag2 = this._rotationGizmo.Visible && (!input.IsAnyModifierHeld() || interactionType == 1 || this._rotationGizmo.InUse());
				if (flag2)
				{
					this._rotationGizmo.OnInteract(interactionType);
					bool flag3 = !this._rotationGizmo.Visible && (this._editMode == ToolMode.RotateBody || this._editMode == ToolMode.RotateHead);
					if (flag3)
					{
						this._selectedEntity = null;
						this._editMode = ToolMode.None;
					}
				}
				else
				{
					bool flag4 = this._translationGizmo.Visible && (!input.IsAnyModifierHeld() || interactionType == 1 || this._translationGizmo.InUse());
					if (flag4)
					{
						this._translationGizmo.OnInteract(lookRay, interactionType);
						bool flag5 = !this._translationGizmo.Visible && this._editMode == ToolMode.Translate;
						if (flag5)
						{
							this._selectedEntity = null;
							this._editMode = ToolMode.None;
						}
					}
					else
					{
						bool flag6 = interactionType == 0;
						if (flag6)
						{
							bool flag7 = this._selectedEntity != null && !input.IsAnyModifierHeld();
							if (flag7)
							{
								this._selectedEntity = null;
								this._editMode = ToolMode.None;
							}
							else
							{
								bool flag8 = this._hoveredEntity != null;
								if (flag8)
								{
									this._selectedEntity = this._hoveredEntity;
									bool flag9 = input.IsAltHeld();
									if (flag9)
									{
										this._translationGizmo.Hide();
										bool headSelected = this._headSelected;
										if (headSelected)
										{
											this._rotationGizmo.Show(this._selectedEntity.Position + new Vector3(0f, this._selectedEntity.EyeOffset, 0f), new Vector3?(this._selectedEntity.LookOrientation), null, null);
											this._editMode = ToolMode.RotateHead;
										}
										else
										{
											this._rotationGizmo.Show(this._selectedEntity.Position, new Vector3?(this._selectedEntity.BodyOrientation), null, null);
											this._editMode = ToolMode.RotateBody;
										}
									}
									else
									{
										bool flag10 = input.IsShiftHeld();
										if (flag10)
										{
											this._rotationGizmo.Hide();
											bool headSelected2 = this._headSelected;
											if (headSelected2)
											{
												this._translationGizmo.Show(this._selectedEntity.Position, new Vector3(this._selectedEntity.LookOrientation.Pitch, this._selectedEntity.LookOrientation.Yaw, 0f), null);
											}
											else
											{
												this._translationGizmo.Show(this._selectedEntity.Position, this._selectedEntity.BodyOrientation, null);
											}
											this._editMode = ToolMode.Translate;
										}
										else
										{
											this._editMode = ToolMode.FreeMove;
											BoundingBox hitbox = this._hoveredEntity.Hitbox;
											hitbox.Translate(this._hoveredEntity.Position);
											HitDetection.RayBoxCollision rayBoxCollision;
											bool flag11 = HitDetection.CheckRayBoxCollision(hitbox, lookRay.Position, lookRay.Direction, out rayBoxCollision, false);
											if (flag11)
											{
												this._moveOffset = rayBoxCollision.Position - this._hoveredEntity.Position;
											}
										}
									}
									this._targetDistance = Vector3.Distance(lookRay.Position, this._selectedEntity.Position + this._moveOffset);
									this._lastEntityPosition = this._selectedEntity.Position;
								}
							}
						}
						else
						{
							bool flag12 = this._selectedEntity != null;
							if (flag12)
							{
								this._selectedEntity.SetPosition(this._lastEntityPosition);
								this.OnPositionChange(this._lastEntityPosition);
								this._selectedEntity = null;
							}
							this._editMode = ToolMode.None;
						}
					}
				}
			}
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x0014C2D0 File Offset: 0x0014A4D0
		protected override void OnActiveStateChange(bool newState)
		{
			bool flag = !newState;
			if (flag)
			{
				bool flag2 = this._editMode == ToolMode.RotateBody || this._editMode == ToolMode.RotateHead;
				if (flag2)
				{
					this._rotationGizmo.Hide();
				}
				bool flag3 = this._editMode == ToolMode.Translate;
				if (flag3)
				{
					this._translationGizmo.Hide();
				}
				this._selectedEntity = null;
				this._editMode = ToolMode.None;
			}
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x0014C334 File Offset: 0x0014A534
		private void OnKeyDown()
		{
			Input input = this._gameInstance.Input;
			bool flag = input.ConsumeKey(this._keybinds[Keybind.ToggleSurface], false);
			if (flag)
			{
				this._lockToSurface = !this._lockToSurface;
			}
			Entity entity = (this._selectedEntity == null) ? this._hoveredEntity : this._selectedEntity;
			bool flag2 = entity != null;
			if (flag2)
			{
				bool flag3 = input.ConsumeKey(this._keybinds[Keybind.Remove], false);
				if (flag3)
				{
					this.OnEntityAction(0, entity);
				}
				else
				{
					bool flag4 = input.ConsumeKey(this._keybinds[Keybind.ToggleFreeze], false);
					if (flag4)
					{
						this.OnEntityAction(2, entity);
					}
					else
					{
						bool flag5 = input.ConsumeKey(this._keybinds[Keybind.Clone], false);
						if (flag5)
						{
							this.OnEntityAction(1, entity);
						}
					}
				}
			}
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x0014C400 File Offset: 0x0014A600
		private void OnRotationChange(Vector3 rotation)
		{
			bool flag = this._editMode == ToolMode.RotateBody;
			if (flag)
			{
				this.OnMoveEntity(this._selectedEntity, this._selectedEntity.Position, rotation, this._selectedEntity.LookOrientation);
			}
			bool flag2 = this._editMode == ToolMode.RotateHead;
			if (flag2)
			{
				this.OnMoveEntity(this._selectedEntity, this._selectedEntity.Position, this._selectedEntity.BodyOrientation, rotation);
			}
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0014C470 File Offset: 0x0014A670
		private void OnPositionChange(Vector3 position)
		{
			bool flag = this._editMode != ToolMode.FreeMove && this._editMode != ToolMode.Translate;
			if (!flag)
			{
				this.OnMoveEntity(this._selectedEntity, position, this._selectedEntity.BodyOrientation, this._selectedEntity.LookOrientation);
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0014C4C0 File Offset: 0x0014A6C0
		private void OnMoveEntity(Entity entity, Vector3 position, Vector3 bodyRotation, Vector3 headRotation)
		{
			bool flag = entity == null;
			if (!flag)
			{
				entity.PositionProgress = 1f;
				entity.SetTransform(position, bodyRotation, headRotation);
				BuilderToolSetEntityTransform packet = new BuilderToolSetEntityTransform(entity.NetworkId, new ModelTransform(new Position((double)position.X, (double)position.Y, (double)position.Z), new Direction(bodyRotation.Y, bodyRotation.X, bodyRotation.Z), new Direction(headRotation.Y, headRotation.X, headRotation.Z)));
				this._gameInstance.Connection.SendPacket(packet);
			}
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x0014C55F File Offset: 0x0014A75F
		private void OnEntityAction(EntityToolAction action, Entity entity)
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolEntityAction(entity.NetworkId, action));
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x0014C580 File Offset: 0x0014A780
		private string GetDisplayText()
		{
			string result = "";
			bool flag = this._selectedEntity != null;
			if (!flag)
			{
				bool flag2 = this._hoveredEntity != null;
				if (flag2)
				{
					bool flag3 = this._gameInstance.Input.IsAltHeld();
					if (flag3)
					{
						bool headSelected = this._headSelected;
						if (headSelected)
						{
							result = "Rotate Head";
						}
						else
						{
							result = "Rotate";
						}
					}
					else
					{
						bool flag4 = this._gameInstance.Input.IsShiftHeld();
						if (flag4)
						{
							result = "Translate";
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04002883 RID: 10371
		private static readonly BoundingBox _headBox = new BoundingBox(new Vector3(-0.25f, -0.25f, -0.25f), new Vector3(0.25f, 0.25f, 0.25f));

		// Token: 0x04002884 RID: 10372
		private static readonly Vector3 _boxOffset = new Vector3(0.01f, 0.01f, 0.01f);

		// Token: 0x04002885 RID: 10373
		private readonly RotationGizmo _rotationGizmo;

		// Token: 0x04002886 RID: 10374
		private readonly TranslationGizmo _translationGizmo;

		// Token: 0x04002887 RID: 10375
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x04002888 RID: 10376
		private readonly TextRenderer _textRenderer;

		// Token: 0x04002889 RID: 10377
		private Entity _selectedEntity;

		// Token: 0x0400288A RID: 10378
		private Entity _hoveredEntity;

		// Token: 0x0400288B RID: 10379
		private Matrix _tempMatrix;

		// Token: 0x0400288C RID: 10380
		private Matrix _drawMatrix;

		// Token: 0x0400288D RID: 10381
		private Matrix _textMatrix;

		// Token: 0x0400288E RID: 10382
		private Vector3 _textPosition = Vector3.Zero;

		// Token: 0x0400288F RID: 10383
		private float _fillBlurThreshold;

		// Token: 0x04002890 RID: 10384
		private Vector3 _lastEntityPosition = Vector3.Zero;

		// Token: 0x04002891 RID: 10385
		private Vector3 _moveOffset = Vector3.Zero;

		// Token: 0x04002892 RID: 10386
		private ToolMode _editMode = ToolMode.None;

		// Token: 0x04002893 RID: 10387
		private float _targetDistance;

		// Token: 0x04002894 RID: 10388
		private bool _lockToSurface;

		// Token: 0x04002895 RID: 10389
		private bool _headSelected;

		// Token: 0x04002896 RID: 10390
		private readonly Dictionary<Keybind, SDL.SDL_Scancode> _keybinds = new Dictionary<Keybind, SDL.SDL_Scancode>
		{
			{
				Keybind.Clone,
				SDL.SDL_Scancode.SDL_SCANCODE_INSERT
			},
			{
				Keybind.Remove,
				SDL.SDL_Scancode.SDL_SCANCODE_DELETE
			},
			{
				Keybind.ToggleFreeze,
				SDL.SDL_Scancode.SDL_SCANCODE_PAUSE
			},
			{
				Keybind.ToggleSurface,
				SDL.SDL_Scancode.SDL_SCANCODE_HOME
			}
		};
	}
}

using System;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Brush
{
	// Token: 0x0200098E RID: 2446
	internal class BrushAxisLockPlane : Disposable
	{
		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001566AF File Offset: 0x001548AF
		// (set) Token: 0x06004DED RID: 19949 RVA: 0x001566B7 File Offset: 0x001548B7
		public bool Enabled { get; private set; } = false;

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06004DEE RID: 19950 RVA: 0x001566C0 File Offset: 0x001548C0
		// (set) Token: 0x06004DEF RID: 19951 RVA: 0x001566C8 File Offset: 0x001548C8
		public BrushAxisLockPlane.EditMode Mode { get; private set; } = BrushAxisLockPlane.EditMode.None;

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06004DF0 RID: 19952 RVA: 0x001566D1 File Offset: 0x001548D1
		// (set) Token: 0x06004DF1 RID: 19953 RVA: 0x001566D9 File Offset: 0x001548D9
		public Vector3 Position { get; private set; } = Vector3.Zero;

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x001566E2 File Offset: 0x001548E2
		// (set) Token: 0x06004DF3 RID: 19955 RVA: 0x001566EA File Offset: 0x001548EA
		public Matrix Rotation { get; private set; } = Matrix.Identity;

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06004DF4 RID: 19956 RVA: 0x001566F3 File Offset: 0x001548F3
		// (set) Token: 0x06004DF5 RID: 19957 RVA: 0x001566FB File Offset: 0x001548FB
		public Vector3 Normal { get; private set; } = Vector3.Forward;

		// Token: 0x06004DF6 RID: 19958 RVA: 0x00156704 File Offset: 0x00154904
		public BrushAxisLockPlane(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			GraphicsDevice graphics = gameInstance.Engine.Graphics;
			this._rotationGizmo = new RotationGizmo(graphics, gameInstance.App.Fonts.DefaultFontFamily.RegularFont, new RotationGizmo.OnRotationChange(this.OnRotationChange), 0.2617994f);
			this._translationGizmo = new TranslationGizmo(graphics, new TranslationGizmo.OnPositionChange(this.OnPositionChange));
			this._planeRenderer = new BrushAxisLockPlaneRenderer(graphics);
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x001567B1 File Offset: 0x001549B1
		protected override void DoDispose()
		{
			this._rotationGizmo.Dispose();
			this._translationGizmo.Dispose();
			this._planeRenderer.Dispose();
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x001567D8 File Offset: 0x001549D8
		private void UpdatePlane()
		{
			this.Normal = Vector3.TransformNormal(Vector3.Forward, this.Rotation);
			this._planeRenderer.UpdatePlane(this.Position, this.Rotation);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x0015680C File Offset: 0x00154A0C
		public void Update(float deltaTime)
		{
			bool flag = !this.Enabled;
			if (!flag)
			{
				Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
				float targetBlockHitDistance = this._gameInstance.InteractionModule.HasFoundTargetBlock ? this._gameInstance.InteractionModule.TargetBlockHit.Distance : 0f;
				this._translationGizmo.Tick(lookRay);
				this._rotationGizmo.Tick(lookRay, targetBlockHitDistance);
				this._rotationGizmo.UpdateRotation(true);
			}
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x00156894 File Offset: 0x00154A94
		public void Draw(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.Enabled;
			if (!flag)
			{
				SceneRenderer.SceneData data = this._gameInstance.SceneRenderer.Data;
				bool visible = this._rotationGizmo.Visible;
				if (visible)
				{
					this._rotationGizmo.Draw(ref viewProjectionMatrix, this._gameInstance.CameraModule.Controller, -data.CameraPosition);
				}
				bool visible2 = this._translationGizmo.Visible;
				if (visible2)
				{
					this._translationGizmo.Draw(ref viewProjectionMatrix, -data.CameraPosition);
				}
				this._planeRenderer.Draw(ref viewProjectionMatrix, -this._gameInstance.SceneRenderer.Data.CameraPosition, Vector3.One, 1f);
			}
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x00156954 File Offset: 0x00154B54
		public bool OnInteract(InteractionType interactionType, InteractionModule.ClickType clickType, bool firstRun)
		{
			bool flag = !this.Enabled;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.PerformInteractions(interactionType, clickType, firstRun);
				bool isCurrentlyInteractingWithPlane = this._isCurrentlyInteractingWithPlane;
				if (isCurrentlyInteractingWithPlane)
				{
					bool flag3 = clickType == InteractionModule.ClickType.None;
					if (flag3)
					{
						this._isCurrentlyInteractingWithPlane = false;
					}
					result = true;
				}
				else
				{
					result = flag2;
				}
			}
			return result;
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x001569A4 File Offset: 0x00154BA4
		private bool PerformInteractions(InteractionType interactionType, InteractionModule.ClickType clickType, bool firstRun)
		{
			bool flag = this.CancelTransform(interactionType, firstRun);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.TranslationGizmoInteraction(interactionType, clickType, firstRun);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = this.RotationGizmoInteraction(interactionType, clickType, firstRun);
					result = flag3;
				}
			}
			return result;
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x001569EC File Offset: 0x00154BEC
		private bool CancelTransform(InteractionType interactionType, bool firstRun)
		{
			bool flag = interactionType != 1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !firstRun;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this.Mode == BrushAxisLockPlane.EditMode.Rotate || this.Mode == BrushAxisLockPlane.EditMode.Translate;
					if (flag3)
					{
						this._isCurrentlyInteractingWithPlane = true;
						this.ExitGizmoTransformMode();
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x00156A48 File Offset: 0x00154C48
		private bool TranslationGizmoInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, bool firstRun)
		{
			bool flag = this.Mode != BrushAxisLockPlane.EditMode.Translate;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !firstRun && clickType != InteractionModule.ClickType.None;
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._isCurrentlyInteractingWithPlane = true;
					this._translationGizmo.OnInteract(this._gameInstance.CameraModule.GetLookRay(), interactionType);
					bool flag3 = clickType == InteractionModule.ClickType.None;
					if (flag3)
					{
						this._translationGizmo.Show(BrushAxisLockPlane.EpsilonFloorVector3(this.Position, 0.1f), Vector3.Forward, null);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x00156AD4 File Offset: 0x00154CD4
		private bool RotationGizmoInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, bool firstRun)
		{
			bool flag = this.Mode != BrushAxisLockPlane.EditMode.Rotate;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !firstRun && clickType != InteractionModule.ClickType.None;
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._isCurrentlyInteractingWithPlane = true;
					this._rotationGizmo.OnInteract(interactionType);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x00156B24 File Offset: 0x00154D24
		private void OnRotationChange(Vector3 rotation)
		{
			this.Rotation = Matrix.CreateFromYawPitchRoll(rotation.Yaw, rotation.Pitch, rotation.Roll);
			this.UpdatePlane();
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x00156B4F File Offset: 0x00154D4F
		private void OnPositionChange(Vector3 translatedTo)
		{
			this.Position = BrushAxisLockPlane.EpsilonFloorVector3(translatedTo, 0.1f);
			this.UpdatePlane();
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x00156B6C File Offset: 0x00154D6C
		private void EnterAxisLockMode(BrushAxisLockPlane.Gizmo gizmo)
		{
			this.Enabled = true;
			this.Mode = ((gizmo == BrushAxisLockPlane.Gizmo.Rotation) ? BrushAxisLockPlane.EditMode.Rotate : BrushAxisLockPlane.EditMode.Translate);
			this._gameInstance.Chat.Log("Entered Brush Plane Lock Mode");
			this._gameInstance.BuilderToolsModule.Brush.useServerRaytrace = false;
			Vector3 value = new Vector3(MathHelper.SnapRadianTo90Degrees(this._gameInstance.LocalPlayer.LookOrientation.X), MathHelper.SnapRadianTo90Degrees(this._gameInstance.LocalPlayer.LookOrientation.Y), MathHelper.SnapRadianTo90Degrees(this._gameInstance.LocalPlayer.LookOrientation.Z));
			this._rotationGizmo.Show(this.Position, new Vector3?(value), null, null);
			this._rotationGizmo.Hide();
			this.Rotation = Matrix.CreateFromYawPitchRoll(value.Yaw, value.Pitch, value.Roll);
			this.UpdateGizmoPosition(gizmo, false);
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x00156C6A File Offset: 0x00154E6A
		private void ExitGizmoTransformMode()
		{
			this.Mode = BrushAxisLockPlane.EditMode.None;
			this._rotationGizmo.Hide();
			this._translationGizmo.Hide();
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x00156C90 File Offset: 0x00154E90
		private void SwapAxisLockMode()
		{
			bool flag = this.Mode == BrushAxisLockPlane.EditMode.Rotate;
			if (flag)
			{
				this._rotationGizmo.Hide();
				this.UpdateGizmoPosition(BrushAxisLockPlane.Gizmo.Translation, true);
			}
			else
			{
				bool flag2 = this.Mode == BrushAxisLockPlane.EditMode.Translate;
				if (flag2)
				{
					this._translationGizmo.Hide();
					this.UpdateGizmoPosition(BrushAxisLockPlane.Gizmo.Rotation, true);
				}
			}
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x00156CE8 File Offset: 0x00154EE8
		private void UpdateGizmoPosition(BrushAxisLockPlane.Gizmo gizmo, bool updateUsingPlaneIntersection = false)
		{
			Vector3 translatedTo = this._gameInstance.InteractionModule.TargetBlockHit.BlockPosition;
			if (updateUsingPlaneIntersection)
			{
				translatedTo = this.GetIntersectionPointOnPlane();
			}
			bool flag = translatedTo.IsNaN();
			if (flag)
			{
				translatedTo = Vector3.Floor(this._gameInstance.LocalPlayer.Position);
			}
			this.OnPositionChange(translatedTo);
			bool flag2 = gizmo == BrushAxisLockPlane.Gizmo.Rotation;
			if (flag2)
			{
				this._rotationGizmo.Show(this.Position, null, null, null);
			}
			else
			{
				this._translationGizmo.Show(this.Position, Vector3.Forward, null);
			}
			bool flag3 = gizmo == BrushAxisLockPlane.Gizmo.Rotation;
			if (flag3)
			{
				this.Mode = BrushAxisLockPlane.EditMode.Rotate;
			}
			else
			{
				this.Mode = BrushAxisLockPlane.EditMode.Translate;
			}
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x00156DAC File Offset: 0x00154FAC
		private Vector3 GetIntersectionPointOnPlane()
		{
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			Vector3 vector;
			bool flag = HitDetection.CheckRayPlaneIntersection(this.Position, this.Normal, lookRay.Position, lookRay.Direction, out vector, true);
			bool flag2 = flag;
			Vector3 result;
			if (flag2)
			{
				result = vector;
			}
			else
			{
				result = Vector3.NaN;
			}
			return result;
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x00156E04 File Offset: 0x00155004
		public void OnKeyDown()
		{
			bool flag = this._gameInstance.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_E, false);
			if (flag)
			{
				bool flag2 = !this.Enabled;
				if (flag2)
				{
					this.EnterAxisLockMode(BrushAxisLockPlane.Gizmo.Translation);
				}
				bool flag3 = this.Mode == BrushAxisLockPlane.EditMode.None || this.Mode == BrushAxisLockPlane.EditMode.Translate;
				if (flag3)
				{
					this.UpdateGizmoPosition(BrushAxisLockPlane.Gizmo.Translation, true);
				}
				else
				{
					bool flag4 = this.Mode == BrushAxisLockPlane.EditMode.Rotate;
					if (flag4)
					{
						this.SwapAxisLockMode();
					}
				}
			}
			bool flag5 = this._gameInstance.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_R, false);
			if (flag5)
			{
				bool flag6 = !this.Enabled;
				if (flag6)
				{
					this.EnterAxisLockMode(BrushAxisLockPlane.Gizmo.Rotation);
				}
				else
				{
					bool flag7 = this.Mode == BrushAxisLockPlane.EditMode.None || this.Mode == BrushAxisLockPlane.EditMode.Rotate;
					if (flag7)
					{
						this.UpdateGizmoPosition(BrushAxisLockPlane.Gizmo.Rotation, true);
					}
					else
					{
						bool flag8 = this.Mode == BrushAxisLockPlane.EditMode.Translate;
						if (flag8)
						{
							this.SwapAxisLockMode();
						}
					}
				}
			}
			bool flag9 = !this._gameInstance.Input.IsShiftHeld() && this._gameInstance.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_Z, false);
			if (flag9)
			{
				this.Disable();
			}
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x00156F1C File Offset: 0x0015511C
		private static Vector3 EpsilonFloorVector3(Vector3 vector3, float epsilon = 0.1f)
		{
			return new Vector3((float)((int)Math.Floor((double)(vector3.X + epsilon))), (float)((int)Math.Floor((double)(vector3.Y + epsilon))), (float)((int)Math.Floor((double)(vector3.Z + epsilon))));
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x00156F64 File Offset: 0x00155164
		public void Disable()
		{
			bool flag = !this.Enabled;
			if (!flag)
			{
				this.ExitGizmoTransformMode();
				this._gameInstance.BuilderToolsModule.Brush.useServerRaytrace = true;
				this.Enabled = false;
				this._gameInstance.Chat.Log("Exited Brush Plane Lock Mode");
			}
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x00156FBC File Offset: 0x001551BC
		public Vector3 GetPosition()
		{
			return this.Position;
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x00156FD4 File Offset: 0x001551D4
		public Matrix GetRotation()
		{
			return this.Rotation;
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x00156FEC File Offset: 0x001551EC
		public Vector3 GetNormal()
		{
			return this.Normal;
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x00157004 File Offset: 0x00155204
		public bool IsEnabled()
		{
			return this.Enabled;
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x0015701C File Offset: 0x0015521C
		public BrushAxisLockPlane.EditMode GetMode()
		{
			return this.Mode;
		}

		// Token: 0x04002918 RID: 10520
		private readonly GameInstance _gameInstance;

		// Token: 0x04002919 RID: 10521
		private readonly RotationGizmo _rotationGizmo;

		// Token: 0x0400291A RID: 10522
		private readonly TranslationGizmo _translationGizmo;

		// Token: 0x0400291B RID: 10523
		private readonly BrushAxisLockPlaneRenderer _planeRenderer;

		// Token: 0x04002921 RID: 10529
		private bool _isCurrentlyInteractingWithPlane;

		// Token: 0x02000E7D RID: 3709
		public enum EditMode
		{
			// Token: 0x040046BD RID: 18109
			None,
			// Token: 0x040046BE RID: 18110
			Translate,
			// Token: 0x040046BF RID: 18111
			Rotate
		}

		// Token: 0x02000E7E RID: 3710
		public enum Gizmo
		{
			// Token: 0x040046C1 RID: 18113
			Translation,
			// Token: 0x040046C2 RID: 18114
			Rotation
		}
	}
}

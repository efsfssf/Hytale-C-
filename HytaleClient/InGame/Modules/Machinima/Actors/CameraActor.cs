using System;
using Coherent.UI.Binding;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Gizmos.Models;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Machinima.Settings;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.InGame.Modules.Machinima.Actors
{
	// Token: 0x02000926 RID: 2342
	[CoherentType]
	internal class CameraActor : SceneActor, ICameraController
	{
		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x0600474E RID: 18254 RVA: 0x0010D7FD File Offset: 0x0010B9FD
		public float SpeedModifier { get; } = 1f;

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x0600474F RID: 18255 RVA: 0x0010D805 File Offset: 0x0010BA05
		public bool AllowPitchControls
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x06004750 RID: 18256 RVA: 0x0010D808 File Offset: 0x0010BA08
		public bool DisplayCursor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x06004751 RID: 18257 RVA: 0x0010D80B File Offset: 0x0010BA0B
		public bool DisplayReticle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x06004752 RID: 18258 RVA: 0x0010D80E File Offset: 0x0010BA0E
		public bool SkipCharacterPhysics
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x06004753 RID: 18259 RVA: 0x0010D811 File Offset: 0x0010BA11
		public bool IsFirstPerson
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x06004754 RID: 18260 RVA: 0x0010D814 File Offset: 0x0010BA14
		public bool IsAttachedToCharacter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x06004755 RID: 18261 RVA: 0x0010D817 File Offset: 0x0010BA17
		public bool CanMove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06004756 RID: 18262 RVA: 0x0010D81A File Offset: 0x0010BA1A
		public Entity AttachedTo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x06004757 RID: 18263 RVA: 0x0010D81D File Offset: 0x0010BA1D
		[CoherentProperty("active")]
		public bool Active
		{
			get
			{
				return this._gameInstance.CameraModule.Controller == this;
			}
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06004758 RID: 18264 RVA: 0x0010D832 File Offset: 0x0010BA32
		// (set) Token: 0x06004759 RID: 18265 RVA: 0x0010D83A File Offset: 0x0010BA3A
		public Vector3 Offset { get; private set; }

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x0600475A RID: 18266 RVA: 0x0010D843 File Offset: 0x0010BA43
		public Vector3 MovementForceRotation
		{
			get
			{
				return this._rotation;
			}
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x0600475B RID: 18267 RVA: 0x0010D84B File Offset: 0x0010BA4B
		public Vector3 PositionOffset
		{
			get
			{
				return Vector3.Zero;
			}
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x0600475C RID: 18268 RVA: 0x0010D852 File Offset: 0x0010BA52
		public Vector3 RotationOffset
		{
			get
			{
				return Vector3.Zero;
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x0600475D RID: 18269 RVA: 0x0010D859 File Offset: 0x0010BA59
		public new Vector3 Position
		{
			get
			{
				return this.Position;
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x0600475E RID: 18270 RVA: 0x0010D861 File Offset: 0x0010BA61
		public new Vector3 Rotation
		{
			get
			{
				return this._rotation;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x0600475F RID: 18271 RVA: 0x0010D869 File Offset: 0x0010BA69
		public Vector3 MovementLook
		{
			get
			{
				return this.Look;
			}
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x0010D871 File Offset: 0x0010BA71
		protected override ActorType GetActorType()
		{
			return ActorType.Camera;
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x0010D874 File Offset: 0x0010BA74
		public CameraActor(GameInstance gameInstance, string name) : base(gameInstance, name)
		{
			this._gameInstance = gameInstance;
			this._modelRenderer = new PrimitiveModelRenderer(gameInstance.Engine.Graphics, gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram);
			this._modelRenderer.UpdateModelData(CameraModel.BuildModelData());
			this._lineRenderer = new LineRenderer(gameInstance.Engine.Graphics, gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram);
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x0010D934 File Offset: 0x0010BB34
		protected override void DoDispose()
		{
			this._modelRenderer.Dispose();
			base.DoDispose();
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x06004763 RID: 18275 RVA: 0x0010D94A File Offset: 0x0010BB4A
		public bool InteractFromEntity { get; }

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x06004764 RID: 18276 RVA: 0x0010D952 File Offset: 0x0010BB52
		public Vector3 AttachmentPosition { get; }

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06004765 RID: 18277 RVA: 0x0010D95A File Offset: 0x0010BB5A
		public Vector3 LookAt { get; }

		// Token: 0x06004766 RID: 18278 RVA: 0x0010D962 File Offset: 0x0010BB62
		public void SetRotation(Vector3 rotation)
		{
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0010D965 File Offset: 0x0010BB65
		public void ApplyLook(float deltaTime, Vector2 look)
		{
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0010D968 File Offset: 0x0010BB68
		public void OnMouseInput(SDL.SDL_Event evt)
		{
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0010D96B File Offset: 0x0010BB6B
		public void ApplyMove(Vector3 movementOffset)
		{
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0010D96E File Offset: 0x0010BB6E
		public void Reset(GameInstance gameInstance, ICameraController cameraController)
		{
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0010D974 File Offset: 0x0010BB74
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			bool active = this.Active;
			if (!active)
			{
				Vector3 position = this.Position;
				this._modelMatrix = Matrix.Identity;
				Matrix.CreateRotationX(-this.Look.Roll, out this._tempMatrix);
				Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
				Matrix.CreateFromYawPitchRoll(this.Look.Yaw + 1.5707964f, 0f, this.Look.Pitch, out this._tempMatrix);
				Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
				Matrix.CreateTranslation(ref position, out this._tempMatrix);
				Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
				float opacity = 0.4f;
				bool flag = this._gameInstance.MachinimaModule.ActiveActor == this;
				if (flag)
				{
					opacity = 0.8f;
				}
				this._modelRenderer.Draw(viewProjectionMatrix, this._modelMatrix, this._cameraColor, opacity, GL.ONE);
				bool flag2 = this._gameInstance.MachinimaModule.ActiveActor == this && this._gameInstance.MachinimaModule.ShowCameraFrustum;
				if (flag2)
				{
					this.DrawViewFrustum(ref viewProjectionMatrix);
				}
			}
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x0010DAC0 File Offset: 0x0010BCC0
		private void DrawViewFrustum(ref Matrix viewProjectionMatrix)
		{
			float opacity = 0.5f;
			Vector3 whiteColor = this._gameInstance.Engine.Graphics.WhiteColor;
			Matrix matrix;
			this._gameInstance.Engine.Graphics.CreatePerspectiveMatrix(MathHelper.ToRadians(this.FieldOfView), (float)this._gameInstance.Engine.Window.AspectRatio, 0.1f, 1024f, out matrix);
			Matrix matrix2;
			Matrix.CreateRotationX(-this.Look.X, out matrix2);
			Matrix matrix3;
			Matrix.CreateRotationY(-this.Look.Y, out matrix3);
			Matrix matrix4;
			Matrix.Multiply(ref matrix3, ref matrix2, out matrix4);
			Matrix.CreateRotationZ(-this.Look.Z, out matrix3);
			Matrix.Multiply(ref matrix4, ref matrix3, out matrix4);
			Matrix matrix5;
			Matrix.Multiply(ref matrix4, ref matrix, out matrix5);
			Matrix matrix6 = Matrix.Invert(matrix5);
			Matrix matrix7 = Matrix.Invert(matrix4);
			Matrix matrix8 = Matrix.Invert(matrix);
			Vector3 vector = this._gameInstance.LocalPlayer.Position - this.Position;
			Vector3 vector2 = -this.Position;
			Matrix.CreateTranslation(ref vector2, out matrix3);
			Matrix.Multiply(ref matrix3, ref matrix4, out matrix2);
			Matrix matrix9;
			Matrix.Multiply(ref matrix2, ref matrix, out matrix9);
			Matrix invViewProjection = Matrix.Invert(matrix9);
			Vector3 vector3;
			Vector3 value;
			Vector3.ScreenToWorldRay(new Vector2(-1f, -1f), this.Position, invViewProjection, out vector3, out value);
			Vector3 vector4;
			Vector3 value2;
			Vector3.ScreenToWorldRay(new Vector2(1f, -1f), this.Position, invViewProjection, out vector4, out value2);
			Vector3 vector5;
			Vector3 value3;
			Vector3.ScreenToWorldRay(new Vector2(1f, 1f), this.Position, invViewProjection, out vector5, out value3);
			Vector3 vector6;
			Vector3 value4;
			Vector3.ScreenToWorldRay(new Vector2(-1f, 1f), this.Position, invViewProjection, out vector6, out value4);
			int num = 5;
			Vector3 vector7 = this.Position + value * (float)num;
			Vector3 vector8 = this.Position + value2 * (float)num;
			Vector3 vector9 = this.Position + value3 * (float)num;
			Vector3 vector10 = this.Position + value4 * (float)num;
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				vector3,
				vector4,
				vector5,
				vector6,
				vector3
			});
			this._lineRenderer.Draw(ref viewProjectionMatrix, whiteColor, opacity);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				this.Position,
				vector7
			});
			this._lineRenderer.Draw(ref viewProjectionMatrix, whiteColor, opacity);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				this.Position,
				vector8
			});
			this._lineRenderer.Draw(ref viewProjectionMatrix, whiteColor, opacity);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				this.Position,
				vector9
			});
			this._lineRenderer.Draw(ref viewProjectionMatrix, whiteColor, opacity);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				this.Position,
				vector10
			});
			this._lineRenderer.Draw(ref viewProjectionMatrix, whiteColor, opacity);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				vector7,
				vector8,
				vector9,
				vector10,
				vector7
			});
			this._lineRenderer.Draw(ref viewProjectionMatrix, whiteColor, opacity);
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x0010DE57 File Offset: 0x0010C057
		public void Update(float deltaTime)
		{
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x0010DE5C File Offset: 0x0010C05C
		public void SetState(bool newState)
		{
			bool flag = newState && !this.Active;
			if (flag)
			{
				bool flag2 = this._gameInstance.CameraModule.Controller is CameraActor;
				if (flag2)
				{
					this._gameInstance.CameraModule.ResetCameraController();
				}
				this._gameInstance.CameraModule.SetCustomCameraController(this);
				this._previousFieldOfView = this._gameInstance.ActiveFieldOfView;
				this._gameInstance.SetFieldOfView(this.FieldOfView);
			}
			else
			{
				bool flag3 = !newState && this.Active;
				if (flag3)
				{
					this._gameInstance.CameraModule.ResetCameraController();
					float previousFieldOfView = this._previousFieldOfView;
					this._previousFieldOfView = this._gameInstance.ActiveFieldOfView;
					this._gameInstance.SetFieldOfView(previousFieldOfView);
				}
			}
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x0010DF2C File Offset: 0x0010C12C
		public override void LoadKeyframe(TrackKeyframe keyframe)
		{
			base.LoadKeyframe(keyframe);
			this._rotation = this.Look;
			this.Offset = this.Position - this._gameInstance.LocalPlayer.Position;
			KeyframeSetting<float> setting = keyframe.GetSetting<float>("FieldOfView");
			bool flag = setting != null;
			if (flag)
			{
				this.FieldOfView = setting.Value;
				bool active = this.Active;
				if (active)
				{
					this._previousFieldOfView = this._gameInstance.ActiveFieldOfView;
					this._gameInstance.SetFieldOfView(this.FieldOfView);
				}
			}
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x0010DFC4 File Offset: 0x0010C1C4
		public override SceneActor Clone(GameInstance gameInstance)
		{
			SceneActor sceneActor = new CameraActor(gameInstance, "clone");
			base.Track.CopyToActor(ref sceneActor);
			return sceneActor as CameraActor;
		}

		// Token: 0x040023DB RID: 9179
		private GameInstance _gameInstance;

		// Token: 0x040023DE RID: 9182
		private Vector3 _rotation;

		// Token: 0x040023DF RID: 9183
		public float FieldOfView = 70f;

		// Token: 0x040023E0 RID: 9184
		private float _previousFieldOfView = 70f;

		// Token: 0x040023E1 RID: 9185
		private PrimitiveModelRenderer _modelRenderer;

		// Token: 0x040023E2 RID: 9186
		private LineRenderer _lineRenderer;

		// Token: 0x040023E3 RID: 9187
		private Vector3 _cameraColor = new Vector3(0f, 1f, 1f);
	}
}

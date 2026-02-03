using System;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.InGame.Modules.Camera.Controllers
{
	// Token: 0x02000973 RID: 2419
	internal class FreeRotateCameraController : ThirdPersonCameraController
	{
		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06004C3D RID: 19517 RVA: 0x001444A9 File Offset: 0x001426A9
		public override bool DisplayReticle
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x001444AC File Offset: 0x001426AC
		public override bool ApplyHeadRotation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x001444AF File Offset: 0x001426AF
		public override bool InteractFromEntity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x001444B2 File Offset: 0x001426B2
		public override bool IsFirstPerson
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06004C41 RID: 19521 RVA: 0x001444B5 File Offset: 0x001426B5
		public override Vector3 MovementForceRotation
		{
			get
			{
				return base.AttachedTo.LookOrientation;
			}
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x001444C2 File Offset: 0x001426C2
		public FreeRotateCameraController(GameInstance gameInstance) : base(gameInstance)
		{
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x001444D0 File Offset: 0x001426D0
		public override void Reset(GameInstance gameInstance, ICameraController previousCameraController)
		{
			base.Reset(gameInstance, previousCameraController);
			base.PositionOffset = this.CalcualtePositionOffsetForHitbox(gameInstance.ActiveFieldOfView);
			this._horizontalCollisionDistanceOffset = base.PositionOffset.X;
			this._verticalCollisionDistanceOffset = base.PositionOffset.Z;
			bool flag = !(previousCameraController is FreeRotateCameraController);
			if (flag)
			{
				this._rotation = new Vector3(-0.31415927f, base.AttachedTo.LookOrientation.Y + 3.1415927f, 0f);
				this._inTransition = false;
			}
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x00144560 File Offset: 0x00142760
		public override void ApplyLook(float deltaTime, Vector2 look)
		{
			this._rotation = new Vector3(MathHelper.Clamp(base.Rotation.X + look.X, -1.5707964f, 1.5707964f), MathHelper.WrapAngle(base.Rotation.Y + look.Y), base.Rotation.Roll);
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x001445C0 File Offset: 0x001427C0
		private Vector3 CalcualtePositionOffsetForHitbox(float fov)
		{
			float eyeOffset = base.GetEyeOffset();
			Vector3 hitboxSize = base.GetHitboxSize();
			float num = MathHelper.Max(eyeOffset, hitboxSize.Y - eyeOffset);
			float num2 = MathHelper.Max(hitboxSize.X, hitboxSize.Z) / 2f;
			float num3 = (float)Math.Sqrt((double)(2f * num2 * num2));
			float num4 = (float)Math.Sqrt((double)(num * num + num3 * num3));
			float z = num4 * 1.3f / (float)Math.Tan((double)MathHelper.ToRadians(fov * 0.5f));
			return new Vector3(0f, 0f, z);
		}

		// Token: 0x040027FE RID: 10238
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}

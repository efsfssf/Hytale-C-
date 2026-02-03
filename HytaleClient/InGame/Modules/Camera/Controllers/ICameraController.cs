using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.InGame.Modules.Camera.Controllers
{
	// Token: 0x02000974 RID: 2420
	internal interface ICameraController
	{
		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06004C47 RID: 19527
		float SpeedModifier { get; }

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06004C48 RID: 19528
		bool AllowPitchControls { get; }

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06004C49 RID: 19529
		bool DisplayCursor { get; }

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06004C4A RID: 19530
		bool DisplayReticle { get; }

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06004C4B RID: 19531
		bool SkipCharacterPhysics { get; }

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06004C4C RID: 19532
		bool IsFirstPerson { get; }

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06004C4D RID: 19533
		bool InteractFromEntity { get; }

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06004C4E RID: 19534
		Vector3 MovementForceRotation { get; }

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06004C4F RID: 19535
		Entity AttachedTo { get; }

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06004C50 RID: 19536
		Vector3 AttachmentPosition { get; }

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06004C51 RID: 19537
		Vector3 PositionOffset { get; }

		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06004C52 RID: 19538
		Vector3 RotationOffset { get; }

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06004C53 RID: 19539
		Vector3 Position { get; }

		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06004C54 RID: 19540
		Vector3 Rotation { get; }

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06004C55 RID: 19541
		Vector3 LookAt { get; }

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06004C56 RID: 19542
		bool CanMove { get; }

		// Token: 0x06004C57 RID: 19543
		void Reset(GameInstance gameInstance, ICameraController previousCameraController);

		// Token: 0x06004C58 RID: 19544
		void Update(float deltaTime);

		// Token: 0x06004C59 RID: 19545
		void ApplyLook(float deltaTime, Vector2 lookOffset);

		// Token: 0x06004C5A RID: 19546
		void SetRotation(Vector3 rotation);

		// Token: 0x06004C5B RID: 19547
		void ApplyMove(Vector3 movementOffset);

		// Token: 0x06004C5C RID: 19548
		void OnMouseInput(SDL.SDL_Event evt);
	}
}

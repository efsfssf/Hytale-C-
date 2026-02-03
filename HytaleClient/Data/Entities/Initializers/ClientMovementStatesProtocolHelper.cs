using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Entities.Initializers
{
	// Token: 0x02000B0F RID: 2831
	internal static class ClientMovementStatesProtocolHelper
	{
		// Token: 0x0600589A RID: 22682 RVA: 0x001B0AF4 File Offset: 0x001AECF4
		public static void Parse(MovementStates networkMovementStates, ref ClientMovementStates movementStates)
		{
			movementStates.IsIdle = networkMovementStates.Idle;
			movementStates.IsHorizontalIdle = networkMovementStates.HorizontalIdle;
			movementStates.IsJumping = networkMovementStates.Jumping;
			movementStates.IsFlying = networkMovementStates.Flying;
			movementStates.IsWalking = networkMovementStates.Walking;
			movementStates.IsSprinting = networkMovementStates.Sprinting;
			movementStates.IsCrouching = networkMovementStates.Crouching;
			movementStates.IsForcedCrouching = networkMovementStates.ForcedCrouching;
			movementStates.IsFalling = networkMovementStates.Falling;
			movementStates.IsClimbing = networkMovementStates.Climbing;
			movementStates.IsInFluid = networkMovementStates.InFluid;
			movementStates.IsSwimming = networkMovementStates.Swimming;
			movementStates.IsSwimJumping = networkMovementStates.SwimJumping;
			movementStates.IsOnGround = networkMovementStates.OnGround;
			movementStates.IsMantling = networkMovementStates.Mantling;
			movementStates.IsSliding = networkMovementStates.Sliding;
			movementStates.IsMounting = networkMovementStates.Mounting;
			movementStates.IsRolling = networkMovementStates.Rolling;
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x001B0BDC File Offset: 0x001AEDDC
		public static MovementStates ToPacket(ref ClientMovementStates movementStates)
		{
			return new MovementStates
			{
				Idle = movementStates.IsIdle,
				HorizontalIdle = movementStates.IsHorizontalIdle,
				Jumping = movementStates.IsJumping,
				Flying = movementStates.IsFlying,
				Walking = movementStates.IsWalking,
				Running = (!movementStates.IsIdle && !movementStates.IsHorizontalIdle && !movementStates.IsWalking && !movementStates.IsSprinting),
				Sprinting = movementStates.IsSprinting,
				Crouching = movementStates.IsCrouching,
				ForcedCrouching = movementStates.IsForcedCrouching,
				Falling = movementStates.IsFalling,
				Climbing = movementStates.IsClimbing,
				InFluid = movementStates.IsInFluid,
				Swimming = movementStates.IsSwimming,
				SwimJumping = movementStates.IsSwimJumping,
				OnGround = movementStates.IsOnGround,
				Mantling = movementStates.IsMantling,
				Sliding = movementStates.IsSliding,
				Mounting = movementStates.IsMounting,
				Rolling = movementStates.IsRolling
			};
		}
	}
}

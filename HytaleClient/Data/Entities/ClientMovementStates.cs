using System;

namespace HytaleClient.Data.Entities
{
	// Token: 0x02000B0C RID: 2828
	internal struct ClientMovementStates
	{
		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x0600588E RID: 22670 RVA: 0x001B0509 File Offset: 0x001AE709
		public static ClientMovementStates Idle
		{
			get
			{
				return ClientMovementStates._idle;
			}
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x001B0510 File Offset: 0x001AE710
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is ClientMovementStates)
			{
				ClientMovementStates other = (ClientMovementStates)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x001B053C File Offset: 0x001AE73C
		public bool Equals(ClientMovementStates other)
		{
			return this.IsIdle == other.IsIdle && this.IsHorizontalIdle == other.IsHorizontalIdle && this.IsJumping == other.IsJumping && this.IsFlying == other.IsFlying && this.IsSprinting == other.IsSprinting && this.IsWalking == other.IsWalking && this.IsCrouching == other.IsCrouching && this.IsForcedCrouching == other.IsForcedCrouching && this.IsFalling == other.IsFalling && this.IsClimbing == other.IsClimbing && this.IsInFluid == other.IsInFluid && this.IsSwimming == other.IsSwimming && this.IsSwimJumping == other.IsSwimJumping && this.IsOnGround == other.IsOnGround && this.IsEntityCollided == other.IsEntityCollided && this.IsMantling == other.IsMantling && this.IsSliding == other.IsSliding && this.IsMounting == other.IsMounting && this.IsRolling == other.IsRolling;
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x001B067C File Offset: 0x001AE87C
		public override int GetHashCode()
		{
			int num = 0;
			num |= this.IsIdle.GetHashCode();
			num |= this.IsHorizontalIdle.GetHashCode() << 1;
			num |= this.IsJumping.GetHashCode() << 2;
			num |= this.IsFlying.GetHashCode() << 3;
			num |= this.IsSprinting.GetHashCode() << 4;
			num |= this.IsWalking.GetHashCode() << 5;
			num |= this.IsCrouching.GetHashCode() << 6;
			num |= this.IsForcedCrouching.GetHashCode() << 7;
			num |= this.IsFalling.GetHashCode() << 8;
			num |= this.IsClimbing.GetHashCode() << 9;
			num |= this.IsInFluid.GetHashCode() << 10;
			num |= this.IsSwimming.GetHashCode() << 11;
			num |= this.IsSwimJumping.GetHashCode() << 12;
			num |= this.IsOnGround.GetHashCode() << 13;
			num |= this.IsEntityCollided.GetHashCode() << 14;
			num |= this.IsMantling.GetHashCode() << 15;
			num |= this.IsSliding.GetHashCode() << 16;
			num |= this.IsMounting.GetHashCode() << 17;
			return num | this.IsRolling.GetHashCode() << 18;
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x001B07CC File Offset: 0x001AE9CC
		public static bool operator ==(ClientMovementStates value1, ClientMovementStates value2)
		{
			bool flag = value1.IsIdle != value2.IsIdle;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = value1.IsHorizontalIdle != value2.IsHorizontalIdle;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = value1.IsJumping != value2.IsJumping;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = value1.IsFlying != value2.IsFlying;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = value1.IsSprinting != value2.IsSprinting;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = value1.IsWalking != value2.IsWalking;
								if (flag6)
								{
									result = false;
								}
								else
								{
									bool flag7 = value1.IsCrouching != value2.IsCrouching;
									if (flag7)
									{
										result = false;
									}
									else
									{
										bool flag8 = value1.IsForcedCrouching != value2.IsForcedCrouching;
										if (flag8)
										{
											result = false;
										}
										else
										{
											bool flag9 = value1.IsFalling != value2.IsFalling;
											if (flag9)
											{
												result = false;
											}
											else
											{
												bool flag10 = value1.IsClimbing != value2.IsClimbing;
												if (flag10)
												{
													result = false;
												}
												else
												{
													bool flag11 = value1.IsInFluid != value2.IsInFluid;
													if (flag11)
													{
														result = false;
													}
													else
													{
														bool flag12 = value1.IsSwimming != value2.IsSwimming;
														if (flag12)
														{
															result = false;
														}
														else
														{
															bool flag13 = value1.IsSwimJumping != value2.IsSwimJumping;
															if (flag13)
															{
																result = false;
															}
															else
															{
																bool flag14 = value1.IsOnGround != value2.IsOnGround;
																if (flag14)
																{
																	result = false;
																}
																else
																{
																	bool flag15 = value1.IsEntityCollided != value2.IsEntityCollided;
																	if (flag15)
																	{
																		result = false;
																	}
																	else
																	{
																		bool flag16 = value1.IsMantling != value2.IsMantling;
																		if (flag16)
																		{
																			result = false;
																		}
																		else
																		{
																			bool flag17 = value1.IsSliding != value2.IsSliding;
																			if (flag17)
																			{
																				result = false;
																			}
																			else
																			{
																				bool flag18 = value1.IsMounting != value2.IsMounting;
																				if (flag18)
																				{
																					result = false;
																				}
																				else
																				{
																					bool flag19 = value1.IsRolling != value2.IsRolling;
																					result = !flag19;
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x001B0A04 File Offset: 0x001AEC04
		public static bool operator !=(ClientMovementStates value1, ClientMovementStates value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x04003709 RID: 14089
		public bool IsIdle;

		// Token: 0x0400370A RID: 14090
		public bool IsHorizontalIdle;

		// Token: 0x0400370B RID: 14091
		public bool IsJumping;

		// Token: 0x0400370C RID: 14092
		public bool IsFlying;

		// Token: 0x0400370D RID: 14093
		public bool IsSprinting;

		// Token: 0x0400370E RID: 14094
		public bool IsWalking;

		// Token: 0x0400370F RID: 14095
		public bool IsCrouching;

		// Token: 0x04003710 RID: 14096
		public bool IsForcedCrouching;

		// Token: 0x04003711 RID: 14097
		public bool IsFalling;

		// Token: 0x04003712 RID: 14098
		public bool IsClimbing;

		// Token: 0x04003713 RID: 14099
		public bool IsInFluid;

		// Token: 0x04003714 RID: 14100
		public bool IsSwimming;

		// Token: 0x04003715 RID: 14101
		public bool IsSwimJumping;

		// Token: 0x04003716 RID: 14102
		public bool IsOnGround;

		// Token: 0x04003717 RID: 14103
		public bool IsEntityCollided;

		// Token: 0x04003718 RID: 14104
		public bool IsMantling;

		// Token: 0x04003719 RID: 14105
		public bool IsSliding;

		// Token: 0x0400371A RID: 14106
		public bool IsMounting;

		// Token: 0x0400371B RID: 14107
		public bool IsRolling;

		// Token: 0x0400371C RID: 14108
		private static ClientMovementStates _idle = new ClientMovementStates
		{
			IsIdle = true
		};
	}
}

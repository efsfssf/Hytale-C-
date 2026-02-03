using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000935 RID: 2357
	public class Cooldown
	{
		// Token: 0x060047F7 RID: 18423 RVA: 0x00111A4B File Offset: 0x0010FC4B
		public Cooldown(float cooldownMax, float[] charges, bool interruptRecharge)
		{
			this.SetCooldownMax(cooldownMax);
			this.SetCharges(charges);
			this.ResetCharges();
			this._interruptRecharge = interruptRecharge;
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x00111A74 File Offset: 0x0010FC74
		public void SetCooldownMax(float cooldownMax)
		{
			this._cooldownMax = cooldownMax;
			bool flag = this._remainingCooldown > cooldownMax;
			if (flag)
			{
				this._remainingCooldown = cooldownMax;
			}
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x00111AA0 File Offset: 0x0010FCA0
		public void SetCharges(float[] charges)
		{
			this._charges = charges;
			bool flag = this._chargeCount > charges.Length;
			if (flag)
			{
				this._chargeCount = charges.Length;
			}
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x00111AD0 File Offset: 0x0010FCD0
		public bool HasCooldown(bool deduct)
		{
			bool flag = this._remainingCooldown <= 0f && this._chargeCount > 0;
			bool result;
			if (flag)
			{
				if (deduct)
				{
					this.DeductCharge();
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x00111B14 File Offset: 0x0010FD14
		public float GetCooldownRemainingTime()
		{
			return this._remainingCooldown;
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x00111B2C File Offset: 0x0010FD2C
		public float GetCooldownMax()
		{
			return this._cooldownMax;
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x00111B44 File Offset: 0x0010FD44
		public float GetChargeTimer()
		{
			return this._chargeTimer;
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x00111B5C File Offset: 0x0010FD5C
		public int GetChargeCount()
		{
			return this._chargeCount;
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x00111B74 File Offset: 0x0010FD74
		public float[] GetCharges()
		{
			return this._charges;
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x00111B8C File Offset: 0x0010FD8C
		public bool HasMaxCharges()
		{
			return this._chargeCount >= this._charges.Length;
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x00111BB1 File Offset: 0x0010FDB1
		public void ResetCharges()
		{
			this._chargeCount = this._charges.Length;
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x00111BC2 File Offset: 0x0010FDC2
		public void ResetCooldown()
		{
			this._remainingCooldown = this._cooldownMax;
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x00111BD4 File Offset: 0x0010FDD4
		public void DeductCharge()
		{
			bool flag = this._chargeCount > 0;
			if (flag)
			{
				this._chargeCount--;
			}
			bool interruptRecharge = this._interruptRecharge;
			if (interruptRecharge)
			{
				this._chargeTimer = 0f;
			}
			this.ResetCooldown();
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x00111C20 File Offset: 0x0010FE20
		public bool Tick(float dt)
		{
			bool flag = !this.HasMaxCharges();
			if (flag)
			{
				float num = this._charges[this._chargeCount];
				this._chargeTimer += dt;
				bool flag2 = this._chargeTimer >= num;
				if (flag2)
				{
					this._chargeCount++;
					this._chargeTimer = 0f;
				}
			}
			this._remainingCooldown -= dt;
			return (this.HasMaxCharges() || this._charges.Length <= 1) && this._remainingCooldown <= 0f;
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x00111CBC File Offset: 0x0010FEBC
		public float GetCooldown()
		{
			return this._cooldownMax;
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x00111CD4 File Offset: 0x0010FED4
		public bool InterruptRecharge()
		{
			return this._interruptRecharge;
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x00111CEC File Offset: 0x0010FEEC
		public void ReplenishCharge(int amount, bool interrupt)
		{
			this._chargeCount = MathHelper.Clamp(this._chargeCount + amount, 0, this._charges.Length);
			bool flag = interrupt && amount != 0;
			if (flag)
			{
				this._chargeTimer = 0f;
			}
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00111D31 File Offset: 0x0010FF31
		public void IncreaseTime(float time)
		{
			this._remainingCooldown = MathHelper.Clamp(this._remainingCooldown + time, 0f, this._cooldownMax);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x00111D54 File Offset: 0x0010FF54
		public void IncreaseChargeTime(float time)
		{
			bool flag = this.HasMaxCharges();
			if (!flag)
			{
				bool flag2 = this._charges.Length <= 1;
				if (!flag2)
				{
					float max = this._charges[this._chargeCount];
					this._chargeTimer = MathHelper.Clamp(this._chargeTimer + time, 0f, max);
				}
			}
		}

		// Token: 0x04002443 RID: 9283
		private float _cooldownMax;

		// Token: 0x04002444 RID: 9284
		private float[] _charges;

		// Token: 0x04002445 RID: 9285
		private float _remainingCooldown;

		// Token: 0x04002446 RID: 9286
		private float _chargeTimer;

		// Token: 0x04002447 RID: 9287
		private int _chargeCount;

		// Token: 0x04002448 RID: 9288
		private bool _interruptRecharge;
	}
}

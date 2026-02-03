using System;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;

namespace HytaleClient.Application.Auth
{
	// Token: 0x02000C01 RID: 3073
	public class AesCipher
	{
		// Token: 0x0600620B RID: 25099 RVA: 0x00205198 File Offset: 0x00203398
		public AesCipher(byte[] keyBytes, byte[] ivBytes)
		{
			bool isInfoEnabled = AesCipher.Logger.IsInfoEnabled;
			if (isInfoEnabled)
			{
				AesCipher.Logger.Info<string, string>("{0} {1}", Hex.ToHexString(keyBytes), Hex.ToHexString(ivBytes));
			}
			this._cipherParameters = new ParametersWithIV(new KeyParameter(keyBytes), ivBytes);
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x00205204 File Offset: 0x00203404
		public byte[] Encrypt(byte[] input)
		{
			this._cipher.Reset();
			this._cipher.Init(true, this._cipherParameters);
			byte[] array = new byte[this._cipher.GetOutputSize(input.Length)];
			int num = this._cipher.ProcessBytes(input, 0, input.Length, array, 0);
			num += this._cipher.DoFinal(array, num);
			byte[] array2 = new byte[num];
			Array.Copy(array, array2, num);
			return array2;
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x00205280 File Offset: 0x00203480
		public byte[] Decrypt(byte[] input)
		{
			bool isInfoEnabled = AesCipher.Logger.IsInfoEnabled;
			if (isInfoEnabled)
			{
				AesCipher.Logger.Info(Hex.ToHexString(input));
			}
			this._cipher.Reset();
			this._cipher.Init(false, this._cipherParameters);
			byte[] array = new byte[this._cipher.GetOutputSize(input.Length)];
			int num = this._cipher.ProcessBytes(input, 0, input.Length, array, 0);
			num += this._cipher.DoFinal(array, num);
			byte[] array2 = new byte[num];
			Array.Copy(array, array2, num);
			return array2;
		}

		// Token: 0x04003D01 RID: 15617
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003D02 RID: 15618
		private readonly BufferedBlockCipher _cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());

		// Token: 0x04003D03 RID: 15619
		private readonly ICipherParameters _cipherParameters;
	}
}

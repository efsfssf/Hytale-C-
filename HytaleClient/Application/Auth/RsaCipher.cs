using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;

namespace HytaleClient.Application.Auth
{
	// Token: 0x02000C04 RID: 3076
	public class RsaCipher
	{
		// Token: 0x06006233 RID: 25139 RVA: 0x00205F30 File Offset: 0x00204130
		public RsaCipher(AsymmetricKeyParameter publicKey, AsymmetricKeyParameter privateKey)
		{
			this._publicKey = publicKey;
			this._privateKey = privateKey;
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x00205F48 File Offset: 0x00204148
		public byte[] Encrypt(byte[] data)
		{
			IAsymmetricBlockCipher asymmetricBlockCipher = new OaepEncoding(new RsaEngine());
			asymmetricBlockCipher.Init(true, this._publicKey);
			byte[] array = new byte[data.Length];
			Buffer.BlockCopy(data, 0, array, 0, data.Length);
			return asymmetricBlockCipher.ProcessBlock(array, 0, data.Length);
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x00205F94 File Offset: 0x00204194
		public sbyte[] EncryptSigned(sbyte[] data)
		{
			return (sbyte[])this.Encrypt((byte[])data);
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x00205FB8 File Offset: 0x002041B8
		public byte[] Decrypt(byte[] data)
		{
			IAsymmetricBlockCipher asymmetricBlockCipher = new OaepEncoding(new RsaEngine());
			asymmetricBlockCipher.Init(false, this._privateKey);
			byte[] array = new byte[data.Length];
			Buffer.BlockCopy(data, 0, array, 0, data.Length);
			return asymmetricBlockCipher.ProcessBlock(array, 0, data.Length);
		}

		// Token: 0x06006237 RID: 25143 RVA: 0x00206004 File Offset: 0x00204204
		public sbyte[] DecryptSigned(sbyte[] data)
		{
			return (sbyte[])this.Decrypt((byte[])data);
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x00206028 File Offset: 0x00204228
		public sbyte[] DecryptLong(sbyte[] data)
		{
			sbyte[] result;
			using (MemoryStream memoryStream = new MemoryStream((byte[])data))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					int num = (int)binaryReader.ReadInt16();
					byte[] array = binaryReader.ReadBytes(num);
					byte[] sourceArray = (byte[])this.DecryptSigned((sbyte[])array);
					byte[] array2 = new byte[16];
					Array.Copy(sourceArray, 0, array2, 0, array2.Length);
					byte[] array3 = new byte[16];
					Array.Copy(sourceArray, 16, array3, 0, array3.Length);
					byte[] input = binaryReader.ReadBytes(data.Length - (2 + num));
					AesCipher aesCipher = new AesCipher(array2, array3);
					result = (sbyte[])aesCipher.Decrypt(input);
				}
			}
			return result;
		}

		// Token: 0x04003D1B RID: 15643
		private readonly AsymmetricKeyParameter _publicKey;

		// Token: 0x04003D1C RID: 15644
		private readonly AsymmetricKeyParameter _privateKey;
	}
}

using System;
using System.IO;
using System.Text;
using HytaleClient.Application.Auth;
using HytaleClient.Auth.Proto.Protocol;
using HytaleClient.AuthHandshake.Proto.Protocol;
using NLog;
using Org.BouncyCastle.Pkix;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BFA RID: 3066
	internal class ServicesAuthState
	{
		// Token: 0x060061A9 RID: 25001 RVA: 0x00201A79 File Offset: 0x001FFC79
		public ServicesAuthState(AuthManager authManager, ServicesClient parent)
		{
			this._parent = parent;
			this._certPathBytes = authManager.CertPathBytes;
			this._cipher = authManager.Cipher;
		}

		// Token: 0x060061AA RID: 25002 RVA: 0x00201AAC File Offset: 0x001FFCAC
		public void ProcessAuth0(Auth0 packet)
		{
			bool flag = packet.ProtocolHash != "6faac88713fa024f591f1576afa06d7387364727cf1bd8841bbe8cfd78587";
			if (flag)
			{
				ServicesAuthState.Logger.Error("Got incompatible protocol version from server {0} timestamp {1} vs our {2} timestamp {3}", new object[]
				{
					packet.ProtocolHash,
					packet.ProtocolCompileTimestamp,
					"6faac88713fa024f591f1576afa06d7387364727cf1bd8841bbe8cfd78587",
					1551618658016L
				});
			}
			else
			{
				this._parent.Write(new Auth1((sbyte[])this._certPathBytes, "6faac88713fa024f591f1576afa06d7387364727cf1bd8841bbe8cfd78587", 1551618658016L));
			}
		}

		// Token: 0x060061AB RID: 25003 RVA: 0x00201B44 File Offset: 0x001FFD44
		public void ProcessAuth2(Auth2 packet)
		{
			ServicesAuthState.Logger.Info("Starting handling of auth2");
			sbyte[] data = this._cipher.DecryptSigned(packet.NonceA);
			sbyte[] array = this._cipher.DecryptLong(packet.Cert);
			string @string = Encoding.UTF8.GetString((byte[])array);
			ServicesAuthState.Logger.Info("Got server cert string {0}", @string);
			this._serverCert = new PkixCertPath(new MemoryStream((byte[])array), "PEM");
			PkixCertPathValidator pkixCertPathValidator = new PkixCertPathValidator();
			PkixCertPathValidatorResult pkixCertPathValidatorResult = pkixCertPathValidator.Validate(this._serverCert, this.getValidationParameters());
			ServicesAuthState.Logger.Info<PkixPolicyNode>("Validation result: {0}", pkixCertPathValidatorResult.PolicyTree);
			this._nonceB = new sbyte[36];
			ServicesAuthState.SecureRandom.NextBytes((byte[])this._nonceB);
			RsaCipher rsaCipher = new RsaCipher(((X509Certificate)this._serverCert.Certificates[0]).GetPublicKey(), null);
			sbyte[] array2 = rsaCipher.EncryptSigned(data);
			sbyte[] array3 = rsaCipher.EncryptSigned(this._nonceB);
			this._parent.Write(new Auth3(array2, array3));
		}

		// Token: 0x060061AC RID: 25004 RVA: 0x00201C68 File Offset: 0x001FFE68
		public void ProcessAuth4(Auth4 packet)
		{
			this._sharedSecretIv = this._cipher.DecryptSigned(packet.Secret);
			sbyte[] array = this._cipher.DecryptSigned(packet.NonceB);
			bool flag = !Arrays.AreEqual((byte[])this._nonceB, (byte[])array);
			if (flag)
			{
				throw new Exception("NonceB mismatch!");
			}
			this._parent.Write(new Auth5());
			this.Authed = true;
		}

		// Token: 0x060061AD RID: 25005 RVA: 0x00201CE0 File Offset: 0x001FFEE0
		public void ProcessAuthFinished(ClientAuth6 packet)
		{
			ServicesAuthState.Logger.Info<ServicesClient>("Received authentication finished for {0}", this._parent);
		}

		// Token: 0x060061AE RID: 25006 RVA: 0x00201CFC File Offset: 0x001FFEFC
		private PkixParameters getValidationParameters()
		{
			return new PkixParameters(AuthManager.TrustAnchors)
			{
				IsRevocationEnabled = false
			};
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x00201D24 File Offset: 0x001FFF24
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}", new object[]
			{
				"_certPathBytes",
				this._certPathBytes,
				"_cipher",
				this._cipher,
				"_serverCert",
				this._serverCert,
				"_nonceB",
				this._nonceB,
				"_sharedSecretIv",
				this._sharedSecretIv
			});
		}

		// Token: 0x04003CBB RID: 15547
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003CBC RID: 15548
		private static readonly SecureRandom SecureRandom = new SecureRandom();

		// Token: 0x04003CBD RID: 15549
		private readonly ServicesClient _parent;

		// Token: 0x04003CBE RID: 15550
		private readonly byte[] _certPathBytes;

		// Token: 0x04003CBF RID: 15551
		private readonly RsaCipher _cipher;

		// Token: 0x04003CC0 RID: 15552
		private PkixCertPath _serverCert;

		// Token: 0x04003CC1 RID: 15553
		private sbyte[] _nonceB;

		// Token: 0x04003CC2 RID: 15554
		private sbyte[] _sharedSecretIv;

		// Token: 0x04003CC3 RID: 15555
		public bool Authed = false;
	}
}

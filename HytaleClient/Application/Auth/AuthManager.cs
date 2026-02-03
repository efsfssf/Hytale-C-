using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using NLog;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkix;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;

namespace HytaleClient.Application.Auth
{
	// Token: 0x02000C02 RID: 3074
	internal class AuthManager
	{
		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x0600620F RID: 25103 RVA: 0x00205326 File Offset: 0x00203526
		public AuthManager.AuthSettings Settings { get; }

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06006210 RID: 25104 RVA: 0x0020532E File Offset: 0x0020352E
		// (set) Token: 0x06006211 RID: 25105 RVA: 0x00205336 File Offset: 0x00203536
		public JObject Metadata { get; private set; }

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06006212 RID: 25106 RVA: 0x0020533F File Offset: 0x0020353F
		// (set) Token: 0x06006213 RID: 25107 RVA: 0x00205347 File Offset: 0x00203547
		public byte[] CertPathBytes { get; private set; }

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x06006214 RID: 25108 RVA: 0x00205350 File Offset: 0x00203550
		// (set) Token: 0x06006215 RID: 25109 RVA: 0x00205358 File Offset: 0x00203558
		public X509Certificate Cert { get; private set; }

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x06006216 RID: 25110 RVA: 0x00205361 File Offset: 0x00203561
		// (set) Token: 0x06006217 RID: 25111 RVA: 0x00205369 File Offset: 0x00203569
		public RsaCipher Cipher { get; private set; }

		// Token: 0x06006218 RID: 25112 RVA: 0x00205374 File Offset: 0x00203574
		public AuthManager()
		{
			this.Settings = new AuthManager.AuthSettings();
			string certificatePath = OptionsHelper.CertificatePath;
			string privateKeyPath = OptionsHelper.PrivateKeyPath;
			bool flag = OptionsHelper.InsecureUsername != null || certificatePath == null || !File.Exists(certificatePath) || privateKeyPath == null || !File.Exists(privateKeyPath);
			if (flag)
			{
				this.Settings.IsInsecure = true;
				this.Settings.Username = (OptionsHelper.InsecureUsername ?? "UnauthenticatedGuest");
				AuthManager.Logger.Info("Insecure mode with username {0}", this.Settings.Username);
			}
			else
			{
				string cert = File.ReadAllText(certificatePath, Encoding.UTF8);
				string privateKey = File.ReadAllText(privateKeyPath, Encoding.UTF8);
				this.Setup(cert, privateKey);
				DerBitString derBitString = (DerBitString)Asn1Object.FromByteArray(this.Cert.GetExtensionValue(AuthManager.Uuid_).GetOctets());
				byte[] octets = derBitString.GetOctets();
				bool isLittleEndian = BitConverter.IsLittleEndian;
				if (isLittleEndian)
				{
					AuthManager.Logger.Info("Converting GUID because little endian!");
					this._flipOctets(octets, 0, 4);
					this._flipOctets(octets, 4, 2);
					this._flipOctets(octets, 6, 2);
				}
				Guid guid = new Guid(octets);
				AuthManager.Logger.Info<Guid>("Got GUID loaded: {0}", guid);
				this.Settings.Uuid = guid;
				this.Settings.Username = this.Metadata.GetValue("name").ToString();
				AuthManager.Logger.Info<Guid, string>("Authenticated as: {0} ({1})", this.Settings.Uuid, this.Settings.Username);
			}
		}

		// Token: 0x06006219 RID: 25113 RVA: 0x00205508 File Offset: 0x00203708
		private void _flipOctets(byte[] source, int index, int length)
		{
			byte[] array = new byte[length];
			Array.Copy(source, index, array, 0, length);
			array = Arrays.Reverse(array);
			Array.Copy(array, 0, source, index, length);
		}

		// Token: 0x0600621A RID: 25114 RVA: 0x0020553A File Offset: 0x0020373A
		public void WritePemDataSp(string pathKey, string pathCert)
		{
			this.WritePem(pathKey, this._privateKey);
			File.WriteAllBytes(pathCert, this.CertPathBytes);
		}

		// Token: 0x0600621B RID: 25115 RVA: 0x00205558 File Offset: 0x00203758
		private void WritePem(string path, object o)
		{
			using (TextWriter textWriter = new StringWriter())
			{
				new PemWriter(textWriter).WriteObject(new MiscPemGenerator(o));
				textWriter.Flush();
				byte[] bytes = Encoding.UTF8.GetBytes(textWriter.ToString());
				File.WriteAllBytes(path, bytes);
			}
		}

		// Token: 0x0600621C RID: 25116 RVA: 0x002055C0 File Offset: 0x002037C0
		public void UpdateCertificate(byte[] data)
		{
			string certificatePath = OptionsHelper.CertificatePath;
			bool flag = OptionsHelper.InsecureUsername != null || certificatePath == null || !File.Exists(certificatePath);
			if (!flag)
			{
				this.SetupCertificate(data);
				File.WriteAllBytes(certificatePath, data);
			}
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x00205604 File Offset: 0x00203804
		public void Setup(string cert, string privateKey)
		{
			this.SetupCertificate(Encoding.UTF8.GetBytes(cert));
			AsymmetricKeyParameter privateKey2;
			using (TextReader textReader = new StringReader(privateKey))
			{
				privateKey2 = (AsymmetricKeyParameter)new PemReader(textReader).ReadObject();
			}
			this.Cipher = new RsaCipher(null, privateKey2);
			this._privateKey = privateKey2;
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x00205670 File Offset: 0x00203870
		private void SetupCertificate(byte[] cert)
		{
			this.CertPathBytes = cert;
			PkixCertPath pkixCertPath = new PkixCertPath(new MemoryStream(this.CertPathBytes), "PEM");
			this.Cert = (X509Certificate)pkixCertPath.Certificates[0];
			bool flag = this.Cert.GetExtensionValue(AuthManager.BsonSchema) != null;
			if (flag)
			{
				DerBitString derBitString = (DerBitString)Asn1Object.FromByteArray(this.Cert.GetExtensionValue(AuthManager.BsonSchema).GetOctets());
				using (MemoryStream memoryStream = new MemoryStream(derBitString.GetBytes()))
				{
					using (BsonDataReader bsonDataReader = new BsonDataReader(memoryStream))
					{
						this.Metadata = new JsonSerializer().Deserialize<JObject>(bsonDataReader);
					}
				}
			}
			else
			{
				this.Metadata = new JObject();
			}
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x00205760 File Offset: 0x00203960
		public Guid GetPlayerUuid()
		{
			return this.Settings.IsInsecure ? AuthManager.<GetPlayerUuid>g__MakeNameUuidFromString|35_0("NO_AUTH|" + this.Settings.Username) : this.Settings.Uuid;
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x002057A8 File Offset: 0x002039A8
		public void HandleAuth2(sbyte[] nonceAEncrypted, sbyte[] cert, out sbyte[] encryptedNonceA, out sbyte[] encryptedNonceB)
		{
			AuthManager.Logger.Info("Starting handling of auth2");
			sbyte[] data = this.Cipher.DecryptSigned(nonceAEncrypted);
			sbyte[] array = this.Cipher.DecryptLong(cert);
			string @string = Encoding.UTF8.GetString((byte[])array);
			AuthManager.Logger.Info("Got server cert string {0}", @string);
			this._serverCert = new PkixCertPath(new MemoryStream((byte[])array), "PEM");
			PkixCertPathValidator pkixCertPathValidator = new PkixCertPathValidator();
			PkixCertPathValidatorResult pkixCertPathValidatorResult = pkixCertPathValidator.Validate(this._serverCert, AuthManager.GetValidationParameters());
			AuthManager.Logger.Info<PkixPolicyNode>("Validation result: {0}", pkixCertPathValidatorResult.PolicyTree);
			this._nonceB = new sbyte[36];
			AuthManager.SecureRandom.NextBytes((byte[])this._nonceB);
			RsaCipher rsaCipher = new RsaCipher(((X509Certificate)this._serverCert.Certificates[0]).GetPublicKey(), null);
			encryptedNonceA = rsaCipher.EncryptSigned(data);
			encryptedNonceB = rsaCipher.EncryptSigned(this._nonceB);
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x002058B0 File Offset: 0x00203AB0
		public void HandleAuth4(sbyte[] secret, sbyte[] nonceBEncrypted)
		{
			this._sharedSecretIv = this.Cipher.DecryptSigned(secret);
			sbyte[] array = this.Cipher.DecryptSigned(nonceBEncrypted);
			bool flag = !Arrays.AreEqual((byte[])this._nonceB, (byte[])array);
			if (flag)
			{
				throw new Exception("NonceB mismatch!");
			}
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x00205906 File Offset: 0x00203B06
		public void HandleAuth6()
		{
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x0020590C File Offset: 0x00203B0C
		static AuthManager()
		{
			X509Certificate x509Certificate;
			using (TextReader textReader = new StringReader(AuthManager._rootCert))
			{
				x509Certificate = (X509Certificate)new PemReader(textReader).ReadObject();
			}
			TrustAnchor trustAnchor = new TrustAnchor(x509Certificate, null);
			AuthManager.TrustAnchors = new HashSet();
			AuthManager.TrustAnchors.Add(trustAnchor);
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x002059C8 File Offset: 0x00203BC8
		private static PkixParameters GetValidationParameters()
		{
			return new PkixParameters(AuthManager.TrustAnchors)
			{
				IsRevocationEnabled = false
			};
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x002059F0 File Offset: 0x00203BF0
		[CompilerGenerated]
		internal static Guid <GetPlayerUuid>g__MakeNameUuidFromString|35_0(string name)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(name);
			byte[] array = MD5.Create().ComputeHash(bytes);
			byte[] array2 = array;
			int num = 6;
			array2[num] &= 15;
			byte[] array3 = array;
			int num2 = 6;
			array3[num2] |= 48;
			byte[] array4 = array;
			int num3 = 8;
			array4[num3] &= 63;
			byte[] array5 = array;
			int num4 = 8;
			array5[num4] |= 128;
			byte b = array[6];
			array[6] = array[7];
			array[7] = b;
			b = array[4];
			array[4] = array[5];
			array[5] = b;
			b = array[0];
			array[0] = array[3];
			array[3] = b;
			b = array[1];
			array[1] = array[2];
			array[2] = b;
			return new Guid(array);
		}

		// Token: 0x04003D04 RID: 15620
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003D07 RID: 15623
		private static readonly SecureRandom SecureRandom = new SecureRandom();

		// Token: 0x04003D08 RID: 15624
		private static readonly DerObjectIdentifier OfficialHypixelIncOidIdentifier = new DerObjectIdentifier("1.3.6.1.4.1.47901");

		// Token: 0x04003D09 RID: 15625
		private static readonly DerObjectIdentifier BsonSchema = AuthManager.OfficialHypixelIncOidIdentifier.Branch("4");

		// Token: 0x04003D0A RID: 15626
		private static readonly DerObjectIdentifier Uuid_ = AuthManager.OfficialHypixelIncOidIdentifier.Branch("2");

		// Token: 0x04003D0E RID: 15630
		private AsymmetricKeyParameter _privateKey;

		// Token: 0x04003D0F RID: 15631
		private PkixCertPath _serverCert;

		// Token: 0x04003D10 RID: 15632
		private sbyte[] _nonceB;

		// Token: 0x04003D11 RID: 15633
		private sbyte[] _sharedSecretIv;

		// Token: 0x04003D12 RID: 15634
		private static readonly string _rootCert = "-----BEGIN CERTIFICATE-----\n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\n7qCV3X+cKVkD8yo59dZUfw==\n-----END CERTIFICATE-----";

		// Token: 0x04003D13 RID: 15635
		public static readonly ISet TrustAnchors;

		// Token: 0x02001087 RID: 4231
		public class AuthSettings
		{
			// Token: 0x1700147D RID: 5245
			// (get) Token: 0x06006B53 RID: 27475 RVA: 0x00227128 File Offset: 0x00225328
			public string UuidString
			{
				get
				{
					return this.Uuid.ToString();
				}
			}

			// Token: 0x04004E93 RID: 20115
			public bool IsInsecure;

			// Token: 0x04004E94 RID: 20116
			public Guid Uuid;

			// Token: 0x04004E95 RID: 20117
			public string Username;
		}
	}
}

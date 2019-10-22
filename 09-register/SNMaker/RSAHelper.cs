using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Framework.Version
{
    /// <summary>
    /// RSA 加密帮助管理类
    /// </summary>
    public class RSAHelper
    {
        #region RSA

        /// <summary>
        /// 生成密钥对
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        public static void CreateKey(ref string publicKey, ref string privateKey)
        {
            //生成共约私钥
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);

                publicKey = DESHelper.Encrypt(publicKey);
                privateKey = DESHelper.Encrypt(privateKey);
            }
        }

        /// <summary>
        /// 创建注册码
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="targetKey"></param>
        /// <returns></returns>
        public static string CreateRegistCode(string privateKey, string targetKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                privateKey = DESHelper.Decrypt(privateKey);
                rsa.FromXmlString(privateKey);
                // 加密对象 
                RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsa);
                f.SetHashAlgorithm("SHA1");
                byte[] source = System.Text.ASCIIEncoding.ASCII.GetBytes(targetKey);
                SHA1Managed sha = new SHA1Managed();
                byte[] result = sha.ComputeHash(source);
                byte[] b = f.CreateSignature(result);
                var resp = Convert.ToBase64String(b);
                return DESHelper.Encrypt(resp);
            }
        }


        #endregion RSA

        #region 授权文件验证

        /// <summary>
        /// 验证注册
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="registCode"></param>
        /// <param name="targetKey"></param>
        /// <returns></returns>
        public static bool VerifyRegistCode(string publicKey, string registCode, string targetKey)
        {
            try
            {
                publicKey = DESHelper.Decrypt(publicKey);
                registCode = DESHelper.Decrypt(registCode);
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsa);

                    f.SetHashAlgorithm("SHA1");

                    byte[] key = Convert.FromBase64String(registCode);

                    SHA1Managed sha = new SHA1Managed();
                    byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(targetKey));
                    return f.VerifySignature(name, key);
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}

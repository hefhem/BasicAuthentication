using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using WebForTraining.Database;

namespace WebForTraining
{
    public class TokenIssuer
    {
        public string GenerateKey(string audience)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyBytes = new Byte[32];
                provider.GetBytes(secretKeyBytes);
                return Convert.ToBase64String(secretKeyBytes);
            }
        }

        public string GetToken(ClsReturnValues audience, int UserGroupID)
        {
            // TODO - Authenticate credentials here
            // TODO - Based on the audience passed in, pick the shared key from key store
            // Just hard-coding a key heress
            string key = "qqO5yXcbijtAdYmS2Otyzeze2XQedqy+Tp37wQ3sgTQ=";
            
            SimpleWebToken token = new SimpleWebToken(key) { Issuer = "tdoCloud" };
            //verify the user from the database 
            token.AddClaim(ClaimTypes.Name, audience.ID.ToString());
            //token.AddClaim(ClaimTypes.Email, "jqhuman@somewhere.world");
            using (tdoEntities db = new tdoEntities())
            {
                var groupName = db.uspGetUserGroups().ToList<ClsUserGroups>().Where(p => p.userGroupID == UserGroupID).First().groupName;
                token.AddClaim(ClaimTypes.Role, groupName);
                token.AddClaim("GroupID", UserGroupID.ToString());
            }
            
            //token.AddClaim(ClaimTypes.Role, "Administrator");
            return token.ToString();
        }
    }

    public class SimpleWebToken
    {
        private static readonly TimeSpan lifeTime = new TimeSpan(24, 0, 0);
        private static readonly DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
        private NameValueCollection nameValuePairs;
        private byte[] keyBytes = null;

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public byte[] Signature { get; set; }
        public ulong ExpiresOn { get; private set; }

        public SimpleWebToken(string key)
        {
            TimeSpan ts = DateTime.UtcNow - epochStart + lifeTime;
            this.ExpiresOn = Convert.ToUInt64(ts.TotalSeconds);
            this.nameValuePairs = new NameValueCollection();
            keyBytes = Convert.FromBase64String(key);
        }

        public IList<Claim> Claims
        {
            get
            {
                return this.nameValuePairs.AllKeys
                .SelectMany(key =>
                this.nameValuePairs[key].Split(',')
                .Select(value => new Claim(key, value))
                ).ToList();
            }
        }

        public void AddClaim(string name, string value)
        {
            this.nameValuePairs.Add(name, value);
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            content.Append("Issuer=").Append(this.Issuer);
            foreach (string key in this.nameValuePairs.AllKeys)
            {
                content.Append('&').Append(key).Append('=').Append(this.nameValuePairs[key]);
            }
            content.Append("&ExpiresOn=").Append(this.ExpiresOn);
            if (!string.IsNullOrWhiteSpace(this.Audience))
            {
                content.Append("&Audience=").Append(this.Audience);
            }
            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(content.ToString()));
                string signature =  HttpUtility.UrlEncode(Convert.ToBase64String(signatureBytes));
                content.Append("&HMACSHA256=").Append(signature);
            }
            return content.ToString();
        }

        public static SimpleWebToken Parse(string token, string secretKey)
        {
            var items = HttpUtility.ParseQueryString(token);
            var swt = new SimpleWebToken(secretKey);
            foreach (string key in items.AllKeys)
            {
                string item = items[key];
                switch (key)
                {
                    case "Issuer": swt.Issuer = item; break;
                    case "Audience": swt.Audience = item; break;
                    case "ExpiresOn": swt.ExpiresOn = ulong.Parse(item); break;
                    case "HMACSHA256": swt.Signature =
                    Convert.FromBase64String(item); break;
                    default: swt.AddClaim(key, items[key]); break;
                }
            }
            string rawToken = swt.ToString(); // Computes HMAC inside ToString()
            string computedSignature = HttpUtility.ParseQueryString(rawToken)
            ["HMACSHA256"];
            if (!computedSignature.Equals(Convert.ToBase64String(swt.Signature),
            StringComparison.Ordinal))
               throw new SecurityTokenValidationException("Signature is invalid");
            TimeSpan ts = DateTime.UtcNow - epochStart;
            if (swt.ExpiresOn < Convert.ToUInt64(ts.TotalSeconds))
                throw new SecurityTokenException("Token has expired");
            return swt;
        }
    }
}
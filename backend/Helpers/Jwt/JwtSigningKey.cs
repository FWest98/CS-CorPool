using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CorPool.BackEnd.Helpers.Jwt {
    public class JwtSigningKey : SymmetricSecurityKey {
        public JwtSigningKey(byte[] key) : base(key) { }
        public JwtSigningKey(string key) : this(Encoding.ASCII.GetBytes(key)) { }
    }
}

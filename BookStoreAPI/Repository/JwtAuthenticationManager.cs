using BookStoreAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data.SqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace BookStoreAPI.Repository
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string key;

        public JwtAuthenticationManager(string key) {
            this.key = key;
        }
        
        public string Authenticate(string username, string password)
        {
            List<UserCred> users = new List<UserCred>();

            using (SqlConnection connection = new SqlConnection("Data Source=PG02S177\\SQLEXPRESS;Initial Catalog=MyDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    UserCred user = new UserCred();
                    command.CommandText = "[dbo].[CheckUserCred]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;
                    command.Connection = connection;
                    connection.Open();
                    object data;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data = reader.GetValue(reader.GetOrdinal(nameof(UserCred.Username)));
                            if (data != null)
                            {
                                user.Username = data.ToString();
                            }
                            data = reader.GetValue(reader.GetOrdinal(nameof(UserCred.Password)));
                            if (data != null)
                            {
                                user.Password = data.ToString();
                            }

                            data = reader.GetValue(reader.GetOrdinal(nameof(UserCred.Roles)));
                            if (data != null)
                            {
                                user.Roles = data.ToString();
                            }

                            users.Add(user);
                        }
                    }
                    connection.Close();
                }

            }


            if (users.Count < 0)
            {
                return null;
            }
        

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, users[0].Username),
                    new Claim(ClaimTypes.Role, users[0].Roles)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), 
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}

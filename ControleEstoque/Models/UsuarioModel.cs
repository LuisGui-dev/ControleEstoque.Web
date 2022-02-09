using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ControleEstoque.Helpers;

namespace ControleEstoque.Models
{
    public class UsuarioModel
    {
        public static bool ValidarUsuario(string login, string senha)
        {
            var ret = false;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select count(*) from usuario where login=@login and senha=@senha";

                    commando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    commando.Parameters.Add("@senha", SqlDbType.VarChar).Value =  CriptoHelper.HashMD5(senha);
                    
                    ret = (int)commando.ExecuteScalar() > 0;
                }
            }

            return ret;
        }
    }
}
using System.Data.SqlClient;

namespace ControleEstoque.Models
{
    public class UsuarioModel
    {
        public static bool ValidarUsuario(string login, string senha)
        {
            var ret = false;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString =
                    "Data Source=DESKTOP-LISRUQV;Initial Catalog=controle-estoque; User Id=sa; Password=lg301091";
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = string.Format(
                        "select count(*) from usuario where login='{0}' and senha='{1}'", login, senha);
                    ret = (int)commando.ExecuteScalar() > 0;
                }
            }

            return ret;
        }
    }
}
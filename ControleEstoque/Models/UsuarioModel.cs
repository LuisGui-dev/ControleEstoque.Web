using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ControleEstoque.Helpers;

namespace ControleEstoque.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Infome o login")]
        public string login { get; set; }
        
        [Required(ErrorMessage = "Informe a senha")]
        public string Senha { get; set; }
        
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
            UsuarioModel ret = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select * from usuario where login=@login and senha=@senha";

                    commando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    commando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);

                    var reader = commando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            login = (string)reader["login"],
                            Senha = (string)reader["senha"],
                            Nome = (string)reader["nome"]
                        };
                    }
                }
            }

            return ret;
        }

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from usuario";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }
        
        public static List<UsuarioModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<UsuarioModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;
                    
                    commando.Connection = conexao;
                    commando.CommandText = string.Format(
                        "select * from usuario  order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                    var reader = commando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UsuarioModel()
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            login = (string)reader["login"]
                        });
                    }
                }
            }

            return ret;
        }

        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select * from usuario where (id = @id)";

                    commando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = commando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel()
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            login = (string)reader["login"]
                        };
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var commando = new SqlCommand())
                    {
                        commando.Connection = conexao;
                        commando.CommandText = "delete from usuario where (id = @id)";

                        commando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = commando.ExecuteNonQuery() > 0;
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;
            var model = RecuperarPeloId(Id);
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    if (model == null)
                    {
                        commando.CommandText =
                            "insert into usuario (nome,login,senha) values (@nome, @login, @senha); select convert(int, scope_identity()) ";

                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                        commando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(Senha);


                        ret = (int)commando.ExecuteScalar();
                    }
                    else
                    {
                        commando.CommandText =
                            "update usuario set nome=@nome, login=@login" +
                            (!string.IsNullOrEmpty(Senha) ? ", senha=@senha" : "") +
                            " where id = @id ";

                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;

                        if (!string.IsNullOrEmpty(Senha))
                        {
                            commando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(Senha);
                        }

                        commando.Parameters.Add("@id", SqlDbType.Int).Value = Id;

                        if (commando.ExecuteNonQuery() > 0)
                        {
                            ret = Id;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
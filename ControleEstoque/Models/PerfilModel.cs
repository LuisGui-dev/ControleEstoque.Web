using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControleEstoque.Models
{
    public class PerfilModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public List<UsuarioModel> Usuarios { get; set; }

        public PerfilModel()
        {
            Usuarios = new List<UsuarioModel>();
        }

        public static List<PerfilModel> RecuperarListaAtivos()
        {
            var ret = new List<PerfilModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {

                    commando.Connection = conexao;
                    commando.CommandText = "select * from perfil where ativo=1 order by nome";
                    var reader = commando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
                        });
                    }
                }
            }

            return ret;
        }

        public static List<PerfilModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<PerfilModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    commando.Connection = conexao;
                    commando.CommandText = string.Format(
                        "select * from perfil  order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                    var reader = commando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
                        });
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
                    comando.CommandText = "select count(*) from perfil";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static PerfilModel RecuperarPeloId(int id)
        {
            PerfilModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select * from perfil where (id = @id)";

                    commando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = commando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new PerfilModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
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
                        commando.CommandText = "delete from perfil where (id = @id)";

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
                            "insert into perfil (nome, ativo) values (@nome, @ativo); select convert(int, scope_identity()) ";

                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@ativo", SqlDbType.Bit).Value = Ativo ? 1 : 0;

                        ret = (int)commando.ExecuteScalar();
                    }
                    else
                    {
                        commando.CommandText = "update perfil set nome=@nome, ativo=@ativo where id = @id ";

                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@ativo", SqlDbType.Bit).Value = Ativo ? 1 : 0;
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

        public void CarregarUsuarios()
        {
            Usuarios.Clear();
            
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText =
                        "select u. * " +
                        "from perfil_usuario pu, usuario u " +
                        "where (pu.id_perfil = @id_perfil) and (pu.id_usuario = u.id)";

                    commando.Parameters.Add("@id_perfil", SqlDbType.Int).Value = Id;

                    var reader = commando.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuarios.Add(new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            login = (string)reader["login"]
                        });
                    }
                }
            }

        }
    }
}
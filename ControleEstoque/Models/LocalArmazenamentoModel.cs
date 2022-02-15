using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControleEstoque.Models
{
    public class LocalArmazenamentoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

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
                    comando.CommandText = "select count(*) from local_armazenamento";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static List<LocalArmazenamentoModel> RecuperarLista(int pagina, int tamPagina, string filtro = "")
        {
            var ret = new List<LocalArmazenamentoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    var filtroWhere = string.Empty;
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(nome) like '%{0}%'", filtro.ToLower());
                    }

                    commando.Connection = conexao;
                    commando.CommandText = string.Format(
                        "select *" +
                        " from local_armazenamento" +
                        filtroWhere +
                        " order by nome" +
                        " offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                    var reader = commando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new LocalArmazenamentoModel
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

        public static LocalArmazenamentoModel RecuperarPeloId(int id)
        {
            LocalArmazenamentoModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select * from local_armazenamento where (id = @id)";

                    commando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = commando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new LocalArmazenamentoModel
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
                        commando.CommandText = "delete from local_armazenamento where (id = @id)";

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
                            "insert into local_armazenamento (nome, ativo) values (@nome, @ativo); select convert(int, scope_identity()) ";

                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@ativo", SqlDbType.Bit).Value = Ativo ? 1 : 0;

                        ret = (int)commando.ExecuteScalar();
                    }
                    else
                    {
                        commando.CommandText = "update local_armazenamento set nome=@nome, ativo=@ativo where id = @id ";

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
    }
}
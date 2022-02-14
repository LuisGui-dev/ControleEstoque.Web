using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControleEstoque.Models
{
    public class UnidadeMedidaModel
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "Preencha a sigla.")]
        public string Sigla { get; set; }

        public bool Ativo { get; set; }

        public static List<UnidadeMedidaModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<UnidadeMedidaModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;
                    
                    commando.Connection = conexao;
                    commando.CommandText = string.Format(
                        "select * from unidade_medida  order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                    
                    var reader = commando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UnidadeMedidaModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Sigla = (string)reader["sigla"],
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
                    comando.CommandText = "select count(*) from unidade_medida";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static UnidadeMedidaModel RecuperarPeloId(int id)
        {
            UnidadeMedidaModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select * from unidade_medida where (id = @id)";

                    commando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = commando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UnidadeMedidaModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Sigla = (string)reader["sigla"],
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
                        commando.CommandText = "delete from unidade_medida where (id = @id)";

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
                            "insert into unidade_medida (nome,sigla, ativo) values (@nome, @sigla, @ativo); select convert(int, scope_identity()) ";

                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@sigla", SqlDbType.VarChar).Value = Sigla;
                        commando.Parameters.Add("@ativo", SqlDbType.Bit).Value = Ativo ? 1 : 0;
                        
                        ret = (int)commando.ExecuteScalar();
                    }
                    else
                    {
                        commando.CommandText = "update unidade_medida set nome=@nome, sigla=@sigla, ativo=@ativo where id = @id ";
                        
                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@sigla", SqlDbType.VarChar).Value = Sigla;
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
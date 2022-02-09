using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ControleEstoque.Helpers;

namespace ControleEstoque.Models
{
    public class GrupoProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public static List<GrupoProdutoModel> RecuperarLista()
        {
            var ret = new List<GrupoProdutoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select * from grupo_produto  order by nome";
                    var reader = commando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new GrupoProdutoModel
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

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var commando = new SqlCommand())
                {
                    commando.Connection = conexao;
                    commando.CommandText = "select * from grupo_produto where (id = @id)";

                    commando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = commando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new GrupoProdutoModel
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
                        commando.CommandText = "delete from grupo_produto where (id = @id)";

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
                            "insert into grupo_produto (nome, ativo) values (@nome, @ativo); select convert(int, scope_identity()) ";

                        commando.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome;
                        commando.Parameters.Add("@ativo", SqlDbType.Bit).Value = Ativo ? 1 : 0;
                        
                        ret = (int)commando.ExecuteScalar();
                    }
                    else
                    {
                        commando.CommandText = "update grupo_produto set nome=@nome, ativo=@ativo where id = @id ";
                        
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
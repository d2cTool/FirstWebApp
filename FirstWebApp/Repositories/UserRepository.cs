using FirstWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace FirstWebApp.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private IDbConnection _connection;
        public UserRepository()
        {
            _connection = new SQLiteConnection(_connectionString);
            _connection.Open();
        }
        public bool Delete(User author)
        {
            throw new NotImplementedException();
        }

        public List<User> Fetch(int? authorId = null)
        {
            var authors = new Dictionary<long, User>();
            dynamic parameters;
            var result = _connection.Query<User, Article, User>(
                "select * from author left join article on article.authorid=author.id" + Fetch_ComposeWhereClause(out parameters, authorId: authorId) + " order by author.name",
                (author, article) => {
                    if (!authors.ContainsKey(author.Id))
                    {
                        authors[author.Id] = author;
                        if (article != null)
                        {
                            author.Articles = new List<Article>();
                        }
                    }
                    else
                    {
                        author = authors[author.Id];
                    }
                    if (article != null)
                    {
                        author.Articles.Add(article);
                    }
                    return author;
                }, (object)parameters);

            if (authors.Count > 0)
            {
                return authors.Values.ToList();
            }
            return null;
        }

        public User RetrieveById(int userId)
        {
            var result = Fetch(authorId: userId);
            if (result != null)
            {
                return result[0];
            }
            return null;
        }

        public bool Save(User author)
        {
            if (author.Id == 0)
            {
                author.Id = _connection.Insert<Author>(author);
            }
            else
            {
                _connection.Update<Author>(author);
            }
            return true;
        }

        private string Fetch_ComposeWhereClause(out dynamic parameters, int? authorId = null)
        {
            parameters = new ExpandoObject();
            var whereClauses = new List<string>();
            var whereClauseCount = 0;
            if (authorId != null)
            {
                whereClauses.Add("author.id=@authorId");
                whereClauseCount++;
                parameters.authorId = authorId.Value;
            }
            StringBuilder whereClause = new StringBuilder();
            for (int i = 0, count = whereClauses.Count; i < count; i++)
            {
                if (i == 0)
                {
                    whereClause.Append(" where ");
                }
                else // Not necessary at the moment, but maybe will change if more where clauses are added in the future.
                {
                    whereClause.Append(" and ");
                }
                whereClause.Append(whereClauses[i]);
            }
            if (whereClauseCount == 0)
            {
                parameters = null;
            }
            return whereClause.ToString();
        }
    }
}

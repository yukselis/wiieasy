using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Arwend;
using Dalowe.Data.Infrastructure;
using Dalowe.Domain.Base;

namespace Dalowe.View.Web.Framework.Services.API.Base
{
    public abstract class BaseApi
    {
        private string _sModuleName;

        protected BaseApi(Facade facade)
        {
            Facade = facade;
        }

        internal Client Client => Facade.Services.Client;

        protected Facade Facade { get; }

        public string ModuleName
        {
            get
            {
                if (!string.IsNullOrEmpty(_sModuleName))
                    return _sModuleName;

                var memberInfo = GetType().BaseType;
                if (memberInfo != null) _sModuleName = memberInfo.Name.Replace("Api", "");
                return _sModuleName;
            }
        }

        protected UnitOfWork DbTransaction(bool autoDetectChangesEnabled = true)
        {
            var connectionString = ConfigurationManager.GetParameter("ConnectionString");
            var transaction = UnitOfWork.GetInstance(connectionString, autoDetectChangesEnabled);
            return transaction;
        }

        internal List<TEntity> GetAll<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") where TEntity : DbEntity
        {
            return DbTransaction().Repository<TEntity>().Get(filter, orderBy, includeProperties).ToList();
        }

        internal void Add<TEntity>(TEntity dbEntity) where TEntity : DbEntity
        {
            using (var transaction = DbTransaction())
            {
                if (dbEntity is Entity entity)
                {
                    entity.UserModifiedID = Client.CurrentUser?.ID;
                    entity.DateModified = DateTime.Now;
                }

                dbEntity.UserCreatedID = Client.CurrentUser?.ID;
                dbEntity.DateCreated = DateTime.Now;

                var repository = transaction.Repository<TEntity>();
                repository.Insert(dbEntity);
            }
        }

        internal void Update<TEntity>(TEntity dbEntity) where TEntity : DbEntity
        {
            using (var transaction = DbTransaction())
            {
                if (dbEntity is Entity entity)
                {
                    entity.UserModifiedID = Client.CurrentUser?.ID;
                    entity.DateModified = DateTime.Now;
                }

                var repository = transaction.Repository<TEntity>();
                repository.Update(dbEntity);
            }
        }

        internal void Delete<TEntity>(TEntity dbEntity) where TEntity : DbEntity
        {
            using (var transaction = DbTransaction())
            {
                if (dbEntity is Entity entity)
                {
                    entity.UserModifiedID = Client.CurrentUser?.ID;
                    entity.DateModified = DateTime.Now;
                }

                var repository = transaction.Repository<TEntity>();
                repository.Delete(dbEntity);
            }
        }

        public string GetResponse(string url)
        {
            var result = string.Empty;
            try
            {
                if (url.IndexOf("http://", StringComparison.InvariantCultureIgnoreCase) == -1 &&
                    url.IndexOf("https://", StringComparison.InvariantCultureIgnoreCase) == -1)
                    url = "http://" + url;
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    var webRequest = (HttpWebRequest) WebRequest.Create(new Uri(url));
                    webRequest.KeepAlive = false;
                    webRequest.UserAgent =
                        "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)";
                    webRequest.Accept = "text/html;";
                    using (var response = (HttpWebResponse) webRequest.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                            using (var reader = new StreamReader(
                                response.GetResponseStream() ?? throw new InvalidOperationException(),
                                Encoding.GetEncoding("UTF-8")))
                            {
                                result = reader.ReadToEnd();
                                reader.Close();
                            }

                        response.Close();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return result;
        }

        public string ToUrlSlug(string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
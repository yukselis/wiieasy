using Arwend.Cryptography;
using Arwend.Web.Application.Server;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc.Controllers
{
    public class CacheController : Controller
    {
        public string ClearAllCache(string username, string password)
        {
            string result = string.Empty;
            try
            {
                if (this.CheckPermission(username, password, out result))
                {
                    if (CacheManager.ClearAll())
                        result = "Answer: Caches Cleared";
                }
            }
            catch (Exception exc)
            {
                result = "Error:" + exc.Message;
            }
            return result;
        }
        public string ClearCache(string username, string password, string keys)
        {
            string result = string.Empty;
            try
            {
                if (this.CheckPermission(username, password, out result))
                {
                    keys = HttpUtility.UrlDecode(keys);
                    if (!string.IsNullOrEmpty(keys))
                    {
                        string decryptedKeys = CryptoManager.Decrypt(keys);
                        if (!decryptedKeys.Equals(keys))
                        {
                            string[] Caches = decryptedKeys.Split(';');
                            for (int i = 0; i <= Caches.Length - 1; i++)
                                CacheManager.Remove(Caches[i]);
                            result = "Answer: Caches Cleared";
                        }
                        else
                            result = "Error: Keys Encryption Error";
                    }
                    else
                        result = "Error: Missing Parameter";
                }
            }
            catch (Exception exc)
            {
                result = "Error: " + exc.Message;
            }
            return result;
        }

        public string RecycleApplicationPool(string username, string password)
        {
            string result = string.Empty;
            try
            {
                if (this.CheckPermission(username, password, out result))
                {
                    string content = string.Empty;
                    string path = base.HttpContext.Server.MapPath("Web.config");
                    using (StreamReader read = new StreamReader(path))
                    {
                        content = read.ReadToEnd();
                        read.Close();
                    }
                    content += "\r\n";
                    using (StreamWriter write = new StreamWriter(path))
                    {
                        write.WriteLine(content);
                        write.Close();
                    }
                }

                result = "Application Pool Recycled.";
            }
            catch (Exception exc)
            {
                result = "Error:" + exc.Message;
            }
            return result;
        }
        private bool CheckPermission(string username, string password, out string message)
        {
            message = string.Empty;
            bool isPermitted = false;
            try
            {
                username = HttpUtility.UrlDecode(username);
                password = HttpUtility.UrlDecode(password);
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    string decryptedUsername = CryptoManager.Decrypt(username);
                    string decryptedPassword = CryptoManager.Decrypt(password);
                    if (!decryptedUsername.Equals(username) && !decryptedPassword.Equals(password))
                        isPermitted = true;
                    else
                        message = "Error: Encryption Error";
                }
                else
                    message = "Error: Missing Parameter";
            }
            catch (Exception exc)
            {
                message = "Error:" + exc.Message;
            }
            return isPermitted;
        }
    }
}

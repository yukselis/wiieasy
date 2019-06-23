using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Reflection;
using Arwend.Json;
using System.Runtime.Remoting;
using System.Dynamic;
namespace Arwend
{
    public static class ConfigurationManager
    {
        public static T GetParameter<T>(string key, T defaultValue = default(T)) where T : struct
        {
            try
            {
                object value = GetParameter(key);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch { return defaultValue; }
        }
        public static string GetParameter(string key, string defaultValue = "")
        {
            try
            {
                string returnValue = defaultValue;
                if (!string.IsNullOrEmpty(key) && System.Configuration.ConfigurationManager.AppSettings[key] != null)
                    returnValue = System.Configuration.ConfigurationManager.AppSettings[key];
                return returnValue;
            }
            catch { return defaultValue; }
        }
        public static dynamic GetParameterCollection(string key)
        {
            dynamic returnValue = default(dynamic);
            try
            {
                if (!string.IsNullOrEmpty(key) && System.Configuration.ConfigurationManager.AppSettings[key] != null)
                    returnValue = System.Configuration.ConfigurationManager.AppSettings[key].ToJson();
            }
            catch { }
            return returnValue;
        }
        public static TSection GetSection<TSection>(string sectionName) where TSection : class
        {
            return System.Configuration.ConfigurationManager.GetSection(sectionName) as TSection;
        }
        public static TSectionGroup GetSectionGroup<TSectionGroup>(string sectionGroupName) where TSectionGroup : class
        {
            var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            var sectionGroups = config.SectionGroups;

            foreach (System.Configuration.ConfigurationSectionGroup sectionGroup in sectionGroups)
            {
                if (sectionGroup.Name == sectionGroupName)
                    return sectionGroup as TSectionGroup;
            }
            return null;
        }
        public static void SetParameter(string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] == null)
                        config.AppSettings.Settings.Add(key, value);
                    else
                        config.AppSettings.Settings[key].Value = value;
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch { }
        }
        public static void AddKey(string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] == null)
                        config.AppSettings.Settings.Add(key, value);
                    else
                        config.AppSettings.Settings[key].Value = value;
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception ex)
            {
                var s = ex.StackTrace;
            }
        }
        public static void RemoveKey(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] != null)
                    {
                        config.AppSettings.Settings.Remove(key);
                        config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                        System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                    }
                }
            }
            catch { }
        }
        public static Uri DomainUrl
        {
            get
            {
                string domainUrl = GetParameter("DomainUrl").ToString();
                if (Uri.IsWellFormedUriString(domainUrl, UriKind.Absolute)) return new Uri(domainUrl);
                return null;
            }
        }
        public static string CdnUrl
        {
            get { return GetParameter("CdnUrl").ToString(); }
        }
        public static string SiteCdnUrl
        {
            get { return GetParameter("SiteCdnUrl").ToString(); }
        }
        public static long DigiboxCategoryID
        {
            get { return Convert.ToInt64(GetParameter("DigiboxCategoryID").ToString()); }
        }
        public static long NewQuestionsCategoryID
        {
            get { return Convert.ToInt64(GetParameter("NewQuestionsCategoryID").ToString()); }
        }
        public static long TopQuestionsCategoryID
        {
            get { return Convert.ToInt64(GetParameter("TopQuestionsCategoryID").ToString()); }
        }
        public static ApplicationEnvironment ApplicationEnvironment
        {
            get
            {
                ApplicationEnvironment environment = ApplicationEnvironment.Live;
                string value = GetParameter("ApplicationEnvironment", "Live");
                if (!string.IsNullOrEmpty(value) && Enum.IsDefined(typeof(ApplicationEnvironment), value))
                    environment = (ApplicationEnvironment)Enum.Parse(typeof(ApplicationEnvironment), value);

                if (!IsTestMode && environment == ApplicationEnvironment.Live && HttpContext.Current.Request.Url.Authority.IndexOf("localhost", StringComparison.InvariantCultureIgnoreCase) > -1)
                    environment = ApplicationEnvironment.Localhost;
                return environment;
            }
        }
        public static string ApplicationBase
        {
            get { return GetParameter("ApplicationBase", "/"); }
        }
        public static string ApplicationVersion
        {
            get { return GetParameter("ApplicationVersion"); }
        }
        public static string CompanyInfo
        {
            get { return GetParameter("CompanyInfo"); }
        }
        public static string Culture
        {
            get { return GetParameter("Culture", "tr-TR"); }
        }
        public static string Language
        {
            get { return GetParameter("Language", "tr"); }
        }
        public static string EncryptKey
        {
            get { return GetParameter("EncryptKey"); }
        }
        public static bool IgnoreSecurity
        {
            get { return GetParameter("IgnoreSecurity", "false").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool RemoveW3Prefix
        {
            get { return GetParameter("RemoveW3Prefix", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool IsTestMode
        {
            get { return GetParameter("TestMode", "off").ToString().Equals("on", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static string SuperUserPassword
        {
            get { return GetParameter("SuperUserPassword").ToString(); }
        }
        public static string ClusterUrls
        {
            get { return GetParameter("ClusterUrls").ToString(); }
        }
        public static string SmtpServer
        {
            get { return GetParameter("SmtpServer"); }
        }
        public static int SmtpPort
        {
            get { return int.Parse(GetParameter("SmtpPort", "25")); }
        }
        public static string SmtpUser
        {
            get { return GetParameter("SmtpUser"); }
        }
        public static string SmtpPassword
        {
            get { return GetParameter("SmtpPassword"); }
        }
        public static bool SmtpRequiresAuthentication
        {
            get { return GetParameter("SmtpRequiresAuthentication", "no").ToString().Equals("yes", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool SmtpRequiresSsl
        {
            get { return GetParameter("SmtpRequiresSsl", "no").ToString().Equals("yes", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static string MailFrom
        {
            get { return GetParameter("MailFrom"); }
        }
        public static string MailRecipients
        {
            get { return GetParameter("MailRecipients"); }
        }
        public static bool MailingEnabled
        {
            get { return GetParameter("MailingEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool CachingEnabled
        {
            get { return GetParameter("CachingEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool GoogleRemarketingEnabled
        {
            get { return !IsTestMode && GetParameter("GoogleRemarketingEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool GoogleAnalyticsEnabled
        {
            get { return !IsTestMode && GetParameter("GoogleAnalyticsEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool TagManagerEnabled
        {
            get { return !IsTestMode && GetParameter("TagManagerEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static string GoogleAnalyticsAccountID
        {
            get { return GetParameter("GoogleAnalyticsAccountID").ToString(); }
        }
        public static string TagManagerAccountID
        {
            get { return GetParameter("TagManagerAccountID").ToString(); }
        }
        public static string GoogleConversionID
        {
            get { return GetParameter("GoogleConversionID").ToString(); }
        }
        public static string GoogleSiteVerificationKey
        {
            get { return GetParameter("GoogleSiteVerificationKey").ToString(); }
        }
        public static string RecaptchaSiteKey
        {
            get { return GetParameter("RecaptchaSiteKey").ToString(); }
        }
        public static string RecaptchaSecretKey
        {
            get { return GetParameter("RecaptchaSecretKey").ToString(); }
        }
        public static int UploadLimit { get { return int.Parse(GetParameter("UploadLimit", "256")); } }

        public static bool XssProtectionEnabled { get { return GetParameter("XssProtectionEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); } }
    }
}

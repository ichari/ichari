using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Collections;

namespace URLRewriter
{

    /// <summary>
    /// HttpModule类
    /// </summary>
    public abstract class HttpModule : System.Web.IHttpModule
    {

        /// <summary>
        /// Executes when the module is initialized.
        /// </summary>
        /// <param name="app">A reference to the HttpApplication object processing this request.</param>
        /// <remarks>Wires up the HttpApplication's AuthorizeRequest event to the
        /// <see cref="BaseModuleRewriter_AuthorizeRequest"/> event handler.</remarks>
        public virtual void Init(HttpApplication app)
        {
            // WARNING!  This does not work with Windows authentication!
            // If you are using Windows authentication, change to app.BeginRequest
            //app.AuthorizeRequest += new EventHandler(this.BaseModuleRewriter_AuthorizeRequest);
            app.BeginRequest += new EventHandler(this.BaseModuleRewriter_AuthorizeRequest);
        }

        public virtual void Dispose() { }

        /// <summary>
        /// Called when the module's AuthorizeRequest event fires.
        /// </summary>
        /// <remarks>This event handler calls the <see cref="Rewrite"/> method, passing in the
        /// <b>RawUrl</b> and HttpApplication passed in via the <b>sender</b> parameter.</remarks>
        protected virtual void BaseModuleRewriter_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            Rewrite(app.Request.Path, app);
        }

        /// <summary>
        /// The <b>Rewrite</b> method must be overriden.  It is where the logic for rewriting an incoming
        /// URL is performed.
        /// </summary>
        /// <param name="requestedRawUrl">The requested RawUrl.  (Includes full path and querystring.)</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected abstract void Rewrite(string requestedPath, HttpApplication app);
    }

    /// <summary>
    /// Provides a rewriting HttpModule.
    /// </summary>
    public class ModuleRewriter : HttpModule
    {
        /// <summary>
        /// This method is called during the module's BeginRequest event.
        /// </summary>
        /// <param name="requestedRawUrl">The RawUrl being requested (includes path and querystring).</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected override void Rewrite(string requestedPath, System.Web.HttpApplication app)
        {
            // log information to the Trace object.
            app.Context.Trace.Write("ModuleRewriter", "Entering ModuleRewriter");

            // get the configuration rules
            RewriterRuleCollection rules = RewriterConfiguration.GetConfig().Rules;

            // iterate through each rule...
            for (int i = 0; i < rules.Count; i++)
            {
                // get the pattern to look for, and Resolve the Url (convert ~ into the appropriate directory)
                string lookFor = "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, rules[i].LookFor) + "$";

                // Create a regex (note that IgnoreCase is set...)
                Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                // See if a match is found
                if (re.IsMatch(requestedPath))
                {
                    // match found - do any replacement needed
                    string sendToUrl = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(requestedPath, rules[i].SendTo));

                    // log rewriting information to the Trace object
                    app.Context.Trace.Write("ModuleRewriter", "Rewriting URL to " + sendToUrl);

                    // Rewrite the URL
                    RewriterUtils.RewriteUrl(app.Context, sendToUrl);
                    break;		// exit the for loop
                }
            }

            // Log information to the Trace object
            app.Context.Trace.Write("ModuleRewriter", "Exiting ModuleRewriter");
        }
    }

    /// <summary>
    /// Provides utility helper methods for the rewriting HttpModule and HttpHandler.
    /// </summary>
    /// <remarks>This class is marked as internal, meaning only classes in the same assembly will be
    /// able to access its methods.</remarks>
    internal class RewriterUtils
    {
        #region RewriteUrl
        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        internal static void RewriteUrl(HttpContext context, string sendToUrl)
        {
            string x, y;
            RewriteUrl(context, sendToUrl, out x, out y);
        }

        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        /// <param name="sendToUrlLessQString">Returns the value of sendToUrl stripped of the querystring.</param>
        /// <param name="filePath">Returns the physical file path to the requested page.</param>
        internal static void RewriteUrl(HttpContext context, string sendToUrl, out string sendToUrlLessQString, out string filePath)
        {
            // see if we need to add any extra querystring information
            if (context.Request.QueryString.Count > 0)
            {
                if (sendToUrl.IndexOf('?') != -1)
                    sendToUrl += "&" + context.Request.QueryString.ToString();
                else
                    sendToUrl += "?" + context.Request.QueryString.ToString();
            }

            // first strip the querystring, if any
            string queryString = String.Empty;
            sendToUrlLessQString = sendToUrl;
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
            }

            // grab the file's physical path
            filePath = string.Empty;
            filePath = context.Server.MapPath(sendToUrlLessQString);

            // rewrite the path...
            context.RewritePath(sendToUrlLessQString, String.Empty, queryString);

            // NOTE!  The above RewritePath() overload is only supported in the .NET Framework 1.1
            // If you are using .NET Framework 1.0, use the below form instead:
            // context.RewritePath(sendToUrl);
        }
        #endregion

        /// <summary>
        /// Converts a URL into one that is usable on the requesting client.
        /// </summary>
        /// <remarks>Converts ~ to the requesting application path.  Mimics the behavior of the 
        /// <b>Control.ResolveUrl()</b> method, which is often used by control developers.</remarks>
        /// <param name="appPath">The application path.</param>
        /// <param name="url">The URL, which might contain ~.</param>
        /// <returns>A resolved URL.  If the input parameter <b>url</b> contains ~, it is replaced with the
        /// value of the <b>appPath</b> parameter.</returns>
        internal static string ResolveUrl(string appPath, string url)
        {
            if (url.Length == 0 || url[0] != '~')
                return url;		// there is no ~ in the first character position, just return the url
            else
            {
                if (url.Length == 1)
                    return appPath;  // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }
    }

    /// <summary>
    /// Specifies the configuration settings in the Web.config for the RewriterRule.
    /// </summary>
    /// <remarks>This class defines the structure of the Rewriter configuration file in the Web.config file.
    /// Currently, it allows only for a set of rewrite rules; however, this approach allows for customization.
    /// For example, you could provide a ruleset that <i>doesn't</i> use regular expression matching; or a set of
    /// constant names and values, which could then be referenced in rewrite rules.
    /// <p />
    /// The structure in the Web.config file is as follows:
    /// <code>
    /// &lt;configuration&gt;
    /// 	&lt;configSections&gt;
    /// 		&lt;section name="RewriterConfig" 
    /// 		            type="URLRewriter.Config.RewriterConfigSerializerSectionHandler, URLRewriter" /&gt;
    ///		&lt;/configSections&gt;
    ///		
    ///		&lt;RewriterConfig&gt;
    ///			&lt;Rules&gt;
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///				...
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///			&lt;/Rules&gt;
    ///		&lt;/RewriterConfig&gt;
    ///		
    ///		&lt;system.web&gt;
    ///			...
    ///		&lt;/system.web&gt;
    ///	&lt;/configuration&gt;
    /// </code>
    /// </remarks>
    [Serializable()]
    [XmlRoot("RewriterConfig")]
    public class RewriterConfiguration
    {
        // private member variables
        private RewriterRuleCollection rules;			// an instance of the RewriterRuleCollection class...

        /// <summary>
        /// GetConfig() returns an instance of the <b>RewriterConfiguration</b> class with the values populated from
        /// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
        /// a <b>RewriterConfiguration</b> instance.
        /// </summary>
        /// <returns>A <see cref="RewriterConfiguration"/> instance.</returns>
        public static RewriterConfiguration GetConfig()
        {
            if (HttpContext.Current.Cache["RewriterConfig"] == null)
                HttpContext.Current.Cache.Insert("RewriterConfig", ConfigurationSettings.GetConfig("RewriterConfig"));

            return (RewriterConfiguration)HttpContext.Current.Cache["RewriterConfig"];
        }

        #region Public Properties
        /// <summary>
        /// A <see cref="RewriterRuleCollection"/> instance that provides access to a set of <see cref="RewriterRule"/>s.
        /// </summary>
        public RewriterRuleCollection Rules
        {
            get
            {
                return rules;
            }
            set
            {
                rules = value;
            }
        }
        #endregion
    }

    /// <summary>
    /// The RewriterRuleCollection models a set of RewriterRules in the Web.config file.
    /// </summary>
    /// <remarks>
    /// The RewriterRuleCollection is expressed in XML as:
    /// <code>
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// ...
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// </code>
    /// </remarks>
    [Serializable()]
    public class RewriterRuleCollection : CollectionBase
    {
        /// <summary>
        /// Adds a new RewriterRule to the collection.
        /// </summary>
        /// <param name="r">A RewriterRule instance.</param>
        public virtual void Add(RewriterRule r)
        {
            this.InnerList.Add(r);
        }

        /// <summary>
        /// Gets or sets a RewriterRule at a specified ordinal index.
        /// </summary>
        public RewriterRule this[int index]
        {
            get
            {
                return (RewriterRule)this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = value;
            }
        }
    }

    /// <summary>
    /// Represents a rewriter rule.  A rewriter rule is composed of a pattern to search for and a string to replace
    /// the pattern with (if matched).
    /// </summary>
    [Serializable()]
    public class RewriterRule
    {
        // private member variables...
        private string lookFor, sendTo;

        #region Public Properties
        /// <summary>
        /// Gets or sets the pattern to look for.
        /// </summary>
        /// <remarks><b>LookFor</b> is a regular expression pattern.  Therefore, you might need to escape
        /// characters in the pattern that are reserved characters in regular expression syntax (., ?, ^, $, etc.).
        /// <p />
        /// The pattern is searched for using the <b>System.Text.RegularExpression.Regex</b> class's <b>IsMatch()</b>
        /// method.  The pattern is case insensitive.</remarks>
        public string LookFor
        {
            get
            {
                return lookFor;
            }
            set
            {
                lookFor = value;
            }
        }

        /// <summary>
        /// The string to replace the pattern with, if found.
        /// </summary>
        /// <remarks>The replacement string may use grouping symbols, like $1, $2, etc.  Specifically, the
        /// <b>System.Text.RegularExpression.Regex</b> class's <b>Replace()</b> method is used to replace
        /// the match in <see cref="LookFor"/> with the value in <b>SendTo</b>.</remarks>
        public string SendTo
        {
            get
            {
                return sendTo;
            }
            set
            {
                sendTo = value;
            }
        }
        #endregion
    }

    /// <summary>
    /// Deserializes the markup in Web.config into an instance of the <see cref="RewriterConfiguration"/> class.
    /// </summary>
    public class RewriterConfigSerializerSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates an instance of the <see cref="RewriterConfiguration"/> class.
        /// </summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the Web.config file into an
        /// <see cref="RewriterConfiguration"/> instance.</remarks>
        /// <returns>An instance of the <see cref="RewriterConfiguration"/> class.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // Create an instance of XmlSerializer based on the RewriterConfiguration type...
            XmlSerializer ser = new XmlSerializer(typeof(RewriterConfiguration));

            // Return the Deserialized object from the Web.config XML
            return ser.Deserialize(new XmlNodeReader(section));
        }

    }

}



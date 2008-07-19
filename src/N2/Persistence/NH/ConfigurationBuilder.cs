using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using N2.Definitions;
using NHibernate;
using NHibernate.Mapping;
using N2.Configuration;
using System.Configuration;
using NHibernate.Cfg;
using System.Text;

namespace N2.Persistence.NH
{
	/// <summary>
	/// Builds NHibernate configuration by reading hbm files and generating 
	/// mappings for item types without hbm.xml mappings files.
	/// </summary>
	public class ConfigurationBuilder : IConfigurationBuilder
	{
        private bool tryLocatingHbmResources = false;
		private readonly IDefinitionManager definitions;
		private IDictionary<string, string> properties = new Dictionary<string, string>();
		private IList<Assembly> assemblies = new List<Assembly>();
		private string defaultMapping = "N2.Mappings.Default.hbm.xml,N2";
        private string mappingFormat = @"<?xml version=""1.0"" encoding=""utf-16""?>
<hibernate-mapping xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:nhibernate-mapping-2.2"">
{0}
</hibernate-mapping>
";
        private string classFormat = @"<subclass name=""{0}"" extends=""{1}"" discriminator-value=""{2}"" lazy=""false""/>";

		/// <summary>Creates a new instance of the <see cref="ConfigurationBuilder"/>.</summary>
		public ConfigurationBuilder(IDefinitionManager definitions)
		{
			this.definitions = definitions;
		}

		/// <summary>Creates a new instance of the <see cref="ConfigurationBuilder"/>.</summary>
		public ConfigurationBuilder(IDefinitionManager definitions, DatabaseSection config)
		{
			this.definitions = definitions;

			if (config == null) config = new DatabaseSection();

            if (!string.IsNullOrEmpty(config.HibernateMapping))
                DefaultMapping = config.HibernateMapping;
            else if (config.Flavour == DatabaseFlavour.MySql)
                DefaultMapping = "N2.Mappings.MySQL.hbm.xml, N2";

			SetupProperties(config);

            TryLocatingHbmResources = config.TryLocatingHbmResources;
		}

		/// <summary>Sets properties configuration dictionary based on configuration in the database section.</summary>
		/// <param name="config">The database section configuration.</param>
		protected void SetupProperties(DatabaseSection config)
		{
			Properties["connection.connection_string_name"] = config.ConnectionStringName;
			Properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";

			switch (config.Flavour)
			{
				case DatabaseFlavour.SqlServer2000:
					Properties["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver";
					Properties["dialect"] = "NHibernate.Dialect.MsSql2000Dialect";
					break;
				case DatabaseFlavour.SqlServer2005:
					Properties["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver";
					Properties["dialect"] = "NHibernate.Dialect.MsSql2005Dialect";
					break;
                case DatabaseFlavour.SqlCe:
                    Properties["connection.driver_class"] = "NHibernate.Driver.SqlServerCeDriver";
					Properties["dialect"] = "NHibernate.Dialect.MsSqlCeDialect";
					break;
                case DatabaseFlavour.MySql:
                    Properties["connection.driver_class"] = "NHibernate.Driver.MySqlDataDriver";
                    Properties["dialect"] = "NHibernate.Dialect.MySQL5Dialect";
                    break;
                case DatabaseFlavour.SqLite:
                    Properties["connection.driver_class"] = "NHibernate.Driver.SQLite20Driver";
                    Properties["dialect"] = "NHibernate.Dialect.SQLiteDialect";
                    break;
                case DatabaseFlavour.Firebird:
                    Properties["connection.driver_class"] = "NHibernate.Driver.FirebirdDriver";
                    Properties["dialect"] = "NHibernate.Dialect.FirebirdDialect";
                    break;
                case DatabaseFlavour.Generic:
                    Properties["connection.driver_class"] = "NHibernate.Driver.OleDbDriver";
                    Properties["dialect"] = "NHibernate.Dialect.GenericDialect";
                    break;
                case DatabaseFlavour.Jet:
                    Properties["connection.driver_class"] = "NHibernate.JetDriver.JetDriver, NHibernate.JetDriver";
                    Properties["dialect"] = "NHibernate.JetDriver.JetDialect, NHibernate.JetDriver";
                    break;
                default:
					throw new ConfigurationErrorsException("Couldn't determine database flavour. Please check the 'flavour' attribute of the n2/database configuration section.");
			}

            Properties["cache.use_second_level_cache"] = config.Caching.ToString();
            Properties["cache.use_query_cache"] = config.Caching.ToString();
			Properties["cache.provider_class"] = config.CacheProviderClass;

            foreach (string key in config.HibernateProperties.AllKeys)
            {
                Properties[key] = config.HibernateProperties[key].Value;
            }
		}

        #region Properties

        public bool TryLocatingHbmResources
        {
            get { return tryLocatingHbmResources; }
            set { tryLocatingHbmResources = value; }
        }

		/// <summary>Gets assemblies that will be added to the NHibernate configuration.</summary>
		public IList<Assembly> Assemblies
		{
			get { return assemblies; }
			set { assemblies = value; }
		}

		/// <summary>Gets NHibernate configuration properties.</summary>
		public IDictionary<string,string> Properties
		{
			get { return properties; }
			set { properties = value; }
		}

		/// <summary>Gets or sets a path to an embedded mapping file that are always added to the NHibernate configuration. Items should be in manifest resource stream format followed by a comma and the assembly name, e.g. "N2.Mappings.MySQL.hbm.xml,N2".</summary>
		public string DefaultMapping
		{
			get { return defaultMapping; }
			set { defaultMapping = value; }
		}

		#endregion

		#region Methods

		/// <summary>Builds a <see cref="NHibernate.Cfg.Configuration"/> by adding properties, default assemblies and generating class mappings for unmapped types.</summary>
		/// <returns></returns>
		public virtual NHibernate.Cfg.Configuration BuildConfiguration()
		{
			NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
			AddProperties(cfg);
			AddDefaultMappings(cfg);
			AddAssemblies(cfg);
			GenerateMappings(cfg);

			return cfg;
		}

		/// <summary>Adds known mappings to the configuration.</summary>
		/// <param name="cfg">The configuration to add the mappings to.</param>
		protected virtual void AddDefaultMappings(NHibernate.Cfg.Configuration cfg)
		{
			if(!string.IsNullOrEmpty(DefaultMapping))
			{
				string[] pathAssemblyPair = DefaultMapping.Split(',');
				if (pathAssemblyPair.Length != 2) throw new ArgumentException( "Expected the property DefaultMapping to be in the format [manifest resource path],[assembly name] but was: " + DefaultMapping);

				Assembly a = Assembly.Load(pathAssemblyPair[1]);
				cfg.AddInputStream(a.GetManifestResourceStream(pathAssemblyPair[0]));
			}
		}

        private IEnumerable<Type> EnumerateDefinedTypes()
        {
            List<Type> types = new List<Type>();
            foreach (ItemDefinition definition in definitions.GetDefinitions())
            {
                foreach (Type t in EnumerateBaseTypes(definition.ItemType))
                {
                    if (t.IsSubclassOf(typeof(ContentItem)) && !types.Contains(t))
                    {
                        int index = types.IndexOf(t.BaseType);
                        types.Insert(index + 1, t);
                    }
                }
            }
            return types;
        }

		/// <summary>Generates subclasses nhibernate xml configuration as an alternative to NHibernate definition file and adds them to the configuration.</summary>
		/// <param name="cfg">The nhibernate configuration to build.</param>
		protected virtual void GenerateMappings(NHibernate.Cfg.Configuration cfg)
		{
            Debug.Write("Adding ");
            //Mappings mappings = cfg.CreateMappings();
            StringBuilder mappings = new StringBuilder();
            foreach (Type t in EnumerateDefinedTypes())
            {
                //PersistentClass superClass = mappings.GetClass(t.BaseType);
                //PersistentClass contentClass = new SingleTableSubclass(superClass);
                //contentClass.EntityName = t.FullName;
                //contentClass.IsLazy = false;
                //contentClass.ClassName = GetName(t);
                //contentClass.DiscriminatorValue = GetDiscriminator(t);
                //mappings.AddClass(contentClass);

                if (!TryLocatingHbmResources)
                {
                    AddGeneratedClassMapping(mappings, t);
                }
                else
                {
                    Stream hbmXmlStream = t.Assembly.GetManifestResourceStream(t.FullName + ".hbm.xml");
                    if (hbmXmlStream == null)
                    {
                        AddGeneratedClassMapping(mappings, t);
                    }
                    else
                    {
                        using (hbmXmlStream)
                        {
                            cfg.AddInputStream(hbmXmlStream);
                        }
                    }
                }

                Debug.Write(t.Name + " ");
            }
            cfg.AddXml(string.Format(mappingFormat, mappings));
		}

        private static string GetName(Type t)
        {
            return t.FullName + ", " + t.Assembly.FullName.Split(',')[0];
        }

		/// <summary>Enumerates base type chain of the supplied type.</summary>
		/// <param name="t">The type whose base types will be enumerated.</param>
		/// <returns>An enumerator of the supplied item and all it's base types.</returns>
		protected static IEnumerable<Type> EnumerateBaseTypes(Type t)
		{
			if (t != null)
			{
				foreach (Type baseType in EnumerateBaseTypes(t.BaseType))
					yield return baseType;
				yield return t;
			}
		}

		/// <summary>Adds default assemblies to NHibernate configuration.</summary>
		/// <param name="cfg"></param>
		protected virtual void AddAssemblies(NHibernate.Cfg.Configuration cfg)
		{
			foreach (Assembly a in Assemblies)
				cfg.AddAssembly(a);
			Debug.WriteLine(String.Format("Added {0} assemblies to configuration", Assemblies.Count));
		}

		/// <summary>Adds properties to NHibernate configuration.</summary>
		/// <param name="cfg"></param>
		protected virtual void AddProperties(NHibernate.Cfg.Configuration cfg)
		{
			foreach (KeyValuePair<string, string> pair in Properties)
			{
				cfg.SetProperty(pair.Key, pair.Value);
			}
		}

		/// <summary>Builds a configuration and returns a new <see cref="NHibernate.ISessionFactory"/></summary>
		/// <returns>A new <see cref="NHibernate.ISessionFactory"/>.</returns>
		public ISessionFactory BuildSessionFactory()
		{
			Debug.WriteLine("Building Session Factory " + DateTime.Now);
			return BuildConfiguration().BuildSessionFactory();
		}

		/// <summary>Checks whether a type's mapping is added to the NHibernate configuration.</summary>
		/// <param name="cfg">The nhibernate configuration to investigate.</param>
		/// <param name="type">The type to look for</param>
		/// <returns>True if the type is mapped</returns>
		protected virtual bool IsMapped(NHibernate.Cfg.Configuration cfg, Type type)
		{
			foreach (PersistentClass mapping in cfg.ClassMappings)
				if (mapping.MappedClass == type)
					return true;
			return false;
		}

		/// <summary>Generates the configuration xml for a subclass without any properties and adds it to the NHibernate configuration.</summary>
		/// <param name="cfg">The current nhhibernate configuration.</param>
		/// <param name="itemType">The type to to generate a subclassed NHibernate hbm xml for.</param>
		protected virtual void AddGeneratedClassMapping(StringBuilder mappings, Type itemType)
		{
            mappings.AppendFormat(classFormat, GetName(itemType), GetName(itemType.BaseType), GetDiscriminator(itemType));
		}

		private string GetDiscriminator(Type itemType)
		{
			ItemDefinition definition = definitions.GetDefinition(itemType);
			if (definition != null)
				return definition.Discriminator;
			else
				return itemType.Name;
		}

		#endregion
	}
}
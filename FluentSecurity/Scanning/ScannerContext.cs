using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Scanning
{
	public class ScannerContext
	{
		private readonly List<Assembly> _assemblies = new List<Assembly>();
		private readonly List<ITypeScanner> _typeScanners = new List<ITypeScanner>();

		private readonly IList<Func<Type, bool>> _matchOneTypeFilters = new List<Func<Type, bool>>();
		private readonly IList<Func<Type, bool>> _matchAllTypeFilters = new List<Func<Type, bool>>();
		private readonly IList<Func<string, bool>> _matchOneFileFilters = new List<Func<string, bool>>();
		private readonly IList<Func<string, bool>> _matchAllFileFilters = new List<Func<string, bool>>();

		public IEnumerable<Assembly> AssembliesToScan => _assemblies;

        public IEnumerable<ITypeScanner> TypeScanners => _typeScanners;

        public IEnumerable<Func<Type, bool>> MatchOneTypeFilters => _matchOneTypeFilters;

        public IEnumerable<Func<Type, bool>> MatchAllTypeFilters => _matchAllTypeFilters;

        public IEnumerable<Func<string, bool>> MatchOneFileFilters => _matchOneFileFilters;

        public IEnumerable<Func<string, bool>> MatchAllFileFilters => _matchAllFileFilters;

        internal void AddAssembly(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			if (!_assemblies.Contains(assembly))
				_assemblies.Add(assembly);
		}

		internal void AddTypeScanner(ITypeScanner typeScanner)
		{
			if (typeScanner == null) throw new ArgumentNullException(nameof(typeScanner));
			if (!_typeScanners.Contains(typeScanner))
				_typeScanners.Add(typeScanner);
		}

		internal void AddMatchOneTypeFilter(Func<Type, bool> filter)
		{
			if (filter == null) throw new ArgumentNullException(nameof(filter));
			 _matchOneTypeFilters.Add(filter);
		}

		internal void AddMatchAllTypeFilter(Func<Type, bool> filter)
		{
			if (filter == null) throw new ArgumentNullException(nameof(filter));
			_matchAllTypeFilters.Add(filter);
		}

		internal void AddMatchOneFileFilter(Func<string, bool> filter)
		{
			if (filter == null) throw new ArgumentNullException(nameof(filter));
			_matchOneFileFilters.Add(filter);
		}

		internal void AddMatchAllFileFilter(Func<string, bool> filter)
		{
			if (filter == null) throw new ArgumentNullException(nameof(filter));
			_matchAllFileFilters.Add(filter);
		}

		internal bool FiltersMatchFile(string file)
		{
			var extension = Path.GetExtension(file);
			var isValidExtension = extension != null &&
				(
					extension.Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
					extension.Equals(".dll", StringComparison.OrdinalIgnoreCase)
				);

			if (!isValidExtension) return false;

			var emptyOrMatchesOne = (!MatchOneFileFilters.Any() || MatchOneFileFilters.Any(filter => filter.Invoke(file)));
			var emptyOrMatchesAll = (!MatchAllFileFilters.Any() || MatchAllFileFilters.All(filter => filter.Invoke(file)));
			return emptyOrMatchesOne && emptyOrMatchesAll;
		}

		internal bool FiltersMatchType(Type type)
		{
			var emptyOrMatchesOne = (!MatchOneTypeFilters.Any() || MatchOneTypeFilters.Any(filter => filter.Invoke(type)));
			var emptyOrMatchesAll = (!MatchAllTypeFilters.Any() || MatchAllTypeFilters.All(filter => filter.Invoke(type)));
			return emptyOrMatchesOne && emptyOrMatchesAll;
		}
	}
}
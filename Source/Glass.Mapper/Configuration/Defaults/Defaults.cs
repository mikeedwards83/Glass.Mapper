using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Configuration
{
	public struct Defaults
	{
		public struct OverallConfiguration
		{
			public static bool InferType = false;
			public static bool IsLazy = true;
		}

		public struct ChildrenConfiguration
		{
			public static bool InferType = OverallConfiguration.InferType;
			public static bool IsLazy = OverallConfiguration.IsLazy;
		}

		public struct QueryConfiguration
		{
			public static bool IsRelative = false;
			public static bool InferType = OverallConfiguration.InferType;
			public static bool IsLazy = OverallConfiguration.IsLazy;
		}

		public struct LinkedConfiguration
		{
			public static bool InferType = OverallConfiguration.InferType;
			public static bool IsLazy = OverallConfiguration.IsLazy;
		}

		public struct ParentConfiguration
		{
			public static bool InferType = OverallConfiguration.InferType;
			public static bool IsLazy = OverallConfiguration.IsLazy;
		}

		public struct SelfConfiguration
		{
			public static bool InferType = OverallConfiguration.InferType;
			public static bool IsLazy = OverallConfiguration.IsLazy;
		}

		public struct NodeConfiguration
		{
			public static bool IsLazy = OverallConfiguration.IsLazy;
			public static bool InferType = OverallConfiguration.InferType;

		}
	}
}

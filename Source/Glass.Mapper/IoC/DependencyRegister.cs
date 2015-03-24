using System;

namespace Glass.Mapper.IoC
{
    public class DependencyRegister : IDependencyRegister
    {
        public DependencyRegister(string key, Action<IDependencyRegistrar> action)
        {
            Key = key;
            Action = action;
        }

        /// <summary>
        /// Gets the key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the action
        /// </summary>
        public Action<IDependencyRegistrar> Action { get; private set; }
    }
}

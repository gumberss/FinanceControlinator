using Cli.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cli
{
    class ClassesFinder
    {
        private readonly List<INotification> _notifications;

        public ClassesFinder()
        {
            _notifications = new List<INotification>();
        }

        public ClassesFinder Subscribe(INotification notification)
        {
            if (!_notifications.Contains(notification))
                _notifications.Add(notification);

            return this;
        }

        public ClassesFinder Unsubscribe(INotification notification)
        {
            if (_notifications.Contains(notification))
                _notifications.Remove(notification);

            return this;
        }

        public List<T> GetClassesAssignableFrom<T>()
        {
            var classesTypes = GetClassesTypesAssignableFrom<T>();

            return CreateInstances<T>(classesTypes).ToList();
        }

        private void Notify(String message)
            => _notifications.ForEach(x => x.Notify(message));

        private List<Type> GetClassesTypesAssignableFrom<T>()
        {
            return Assembly.GetAssembly(typeof(Program))
                .GetTypes()
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .ToList();
        }

        private IEnumerable<T> CreateInstances<T>(List<Type> types)
        {
            foreach (var type in types)
            {
                if (!ExistDefaultConstructor(type))
                {
                    Notify($"There is no constructor with no parameters in: '{type.Name}' assignable from '{typeof(T).Name}'");
                    continue;
                }

                yield return (T)Activator.CreateInstance(type);
            }
        }

        private bool ExistDefaultConstructor(Type type)
            => type.GetConstructors().Any(x => x.GetParameters().Length == 0 && x.IsPublic);
    }
}

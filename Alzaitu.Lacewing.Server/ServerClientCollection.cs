using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Alzaitu.Lacewing.Server
{
    public class ServerClientCollection : IReadOnlyCollection<ServerClient>, INotifyCollectionChanged
    {
        private readonly List<ServerClient> _clients = new List<ServerClient>();

        internal void Add(ServerClient client)
        {
            _clients.Add(client);
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] {client}));
        }

        internal void Remove(ServerClient client)
        {
            _clients.Remove(client);
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] {client}));
        }

        public IEnumerator<ServerClient> GetEnumerator() => _clients.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _clients).GetEnumerator();

        public int Count => _clients.Count;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}

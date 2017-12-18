using System.Collections;
using System.Collections.Generic;

namespace Alzaitu.Lacewing.Server
{
    public class ServerChannel : IList<ServerClient>
    {
        private readonly List<ServerClient> _joinedClients;

        public short Id { get; }
        public string Name { get; }

        public ServerChannel()
        {
            _joinedClients = new List<ServerClient>();
        }

        #region IList

        public IEnumerator<ServerClient> GetEnumerator() => _joinedClients.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_joinedClients).GetEnumerator();

        public void Add(ServerClient item) => _joinedClients.Add(item);

        public void Clear() => _joinedClients.Clear();

        public bool Contains(ServerClient item) => _joinedClients.Contains(item);

        public void CopyTo(ServerClient[] array, int arrayIndex) => _joinedClients.CopyTo(array, arrayIndex);

        public bool Remove(ServerClient item) => _joinedClients.Remove(item);

        public int Count => _joinedClients.Count;

        public bool IsReadOnly => false;

        public int IndexOf(ServerClient item) => _joinedClients.IndexOf(item);

        public void Insert(int index, ServerClient item) => _joinedClients.Insert(index, item);

        public void RemoveAt(int index) => _joinedClients.RemoveAt(index);

        public ServerClient this[int index]
        {
            get => _joinedClients[index];
            set => _joinedClients[index] = value;
        }

        #endregion
    }
}

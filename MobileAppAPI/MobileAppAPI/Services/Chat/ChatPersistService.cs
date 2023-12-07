using Cassandra;
using ISession = Cassandra.ISession;

namespace MobileAppAPI.Services.Chat
{
    public class ChatPersistService : IChatPersistService
    {
        private Cluster _cluster;
        private ISession _session;
        private PreparedStatement _insertStatement;
        public ChatPersistService()
        {
            _cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            _session = _cluster.Connect("mobilerts");

            _insertStatement = _session.Prepare("INSERT INTO chat_messages (chat_id, timestamp, message_id, message_text,sender_id) VALUES (?, ?, ?, ?, ?)");
        }
        public async Task<bool> WriteMessage(string chat_id, string message_text, string sender_id)
        {
            var timestamp = DateTime.UtcNow;
            var message_id = Guid.NewGuid();
            var boundStatement = _insertStatement.Bind(chat_id, timestamp, message_id, message_text, sender_id);
            var ret = await _session.ExecuteAsync(boundStatement);
            return ret != null;
        }
        public void Shutdown()
        {
            // Shutdown the cluster when no longer needed
            _cluster.Shutdown();
        }
    }
}

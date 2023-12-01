/*Create keyspace for real-time services*/

CREATE KEYSPACE mobileRTS WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1}; /*This is for development, use NetworkTopologyStrategy for production*/

/*Switch to new keyspace*/
USE mobileRTS;

/*Create table for storing message history*/

CREATE TABLE chat_messages (
    chat_id UUID,
    timestamp TIMESTAMP,
    sender_id UUID,
    message_id UUID,
    message_text TEXT,
    PRIMARY KEY ((chat_id), timestamp, message_id)
) WITH CLUSTERING ORDER BY (timestamp DESC);
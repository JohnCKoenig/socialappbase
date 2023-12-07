@ECHO OFF
SET server=<server name>
SET database=<database name>
SET user=<username>
SET password=<password>

REM Create the database
SQLCMD -S %server% -U %user% -P %password% -i "CoreDatabaseCreate.sql"

REM Connect to database to create tables
SQLCMD -S %server% -U %user% -P %password% -d %database% -i "CreateUsersTable.sql"
SQLCMD -S %server% -U %user% -P %password% -d %database% -i "CreateRefreshTokensTable.sql"
SQLCMD -S %server% -U %user% -P %password% -d %database% -i "CreatePostsTable.sql"
SQLCMD -S %server% -U %user% -P %password% -d %database% -i "CreateChatSessionsTable.sql"
SQLCMD -S %server% -U %user% -P %password% -d %database% -i "CreateChatParticipantsTable.sql"
SQLCMD -S %server% -U %user% -P %password% -d %database% -i "CreateSQLAgentJobs.sql"

ECHO Database has been setup succesfully.
PAUSE
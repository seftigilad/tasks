Installation:
Run SqlTables.sql to create tables, and users seed.
Run .net web api application TaskCore. 
Run React TaskUI application (NPM Install, npm run dev )


Extensebility
To add new task type: 
You need to add new state class that inherits ITaskState. Also you need to add it 
to list of tasks in correct ITaskTypeHandler.
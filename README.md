# UberLogger-StructuredFile
Structured log file format for UberLogger

This provides a much more detailed log file than either Unity's own output log, or UberLoggerFile does.

The intention is to include much more information in the logs, thereby making it easier to diagnose errors which occur in deployed builds.

Main features:

* UTC timestamps for each log message (useful when correlating logs from multiple machines in the same MP match)
* Channel information present in logs
* One log message per line, except when callstacks are included
* Ability to have full callstacks in logs - either for all messages, or only for warnings/errors

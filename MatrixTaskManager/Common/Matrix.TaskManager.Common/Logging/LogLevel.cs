using System.Runtime.Serialization;

namespace Matrix.TaskManager.Common.Logging
{
    [DataContract]
    public enum LogLevel
    {
        [EnumMember]
        Trace,
        [EnumMember]
        Debug,
        [EnumMember]
        Info,
        [EnumMember]
        Warning,
        [EnumMember]
        Error,
        [EnumMember]
        Critical,
        [EnumMember]
        Fatal
    }
}

namespace HMI_MT
{
    //Структуры и перечисления пространства имен HMI_MT
    enum DataTimeFormat
    {
        ShowClock,
        ShowDay,
        ShowStorageStat,
    }

    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }
}

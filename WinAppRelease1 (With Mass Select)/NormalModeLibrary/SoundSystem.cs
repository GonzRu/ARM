using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

namespace NormalModeLibrary
{
    public class SoundSystem
    {
        static SoundSystem system;
        SoundPlayer player = new SoundPlayer();
        private uint _countOfSoundRequest = 0;

        private SoundSystem()
        {
            player.SoundLocation = Environment.CurrentDirectory + @"\Project\Alarm.wav";
        }
        ~SoundSystem()
        {
            Stop();
        }
        /// <summary>
        /// Запуск воспроизведения
        /// </summary>
        /// <exception cref="SoundSystemException" />
        public void Play()
        {
            try
            {
                player.PlayLooping();
                IsPlaying = true;
                _countOfSoundRequest++;
            }
            catch ( Exception ex )
            {
                IsPlaying = false;

                Console.WriteLine("=================");
                Console.WriteLine(ex.Message);
                Console.WriteLine(string.Format("Load file: {0}; Sound location: {1}", player.IsLoadCompleted, string.Format("Playback error: {0}", player.SoundLocation)));
                Console.WriteLine(string.Format("Source: {0}", ex.Source));
                Console.WriteLine("=================");
            }
        }
        /// <summary>
        /// Остановка воспроизведения
        /// </summary>
        public void Stop()
        {
            _countOfSoundRequest--;

            if (_countOfSoundRequest == 0)
            {
                player.Stop();
                IsPlaying = false;
            }
        }

        /// <summary>
        /// Получить признак воспроизведения
        /// </summary>
        public Boolean IsPlaying { get; private set; }
        /// <summary>
        /// Звуковая система
        /// </summary>
        public static SoundSystem System
        {
            get
            {
                if ( system == null )
                    system = new SoundSystem();
                return system;
            }
        }
    }

    public class SoundSystemException : Exception
    {
        internal SoundSystemException( Exception ex, SoundPlayer player )
            : base( ex.Message, ex )
        {
            this.Source = "SoundSystem";
            SoundLocation = string.Format( "Playback error: {0}", player.SoundLocation );
            IsLoadCompleted = player.IsLoadCompleted;
        }
        /// <summary>
        /// Получить путь к файлу
        /// </summary>
        public String SoundLocation { get; private set; }
        /// <summary>
        /// Получить признак загружен ли звуковой файл
        /// </summary>
        public bool IsLoadCompleted { get; private set; }
    }
}

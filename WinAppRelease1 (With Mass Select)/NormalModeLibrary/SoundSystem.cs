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
            }
            catch ( Exception ex )
            {
                IsPlaying = false;
                throw new SoundSystemException( ex, player );
            }
        }
        /// <summary>
        /// Остановка воспроизведения
        /// </summary>
        public void Stop()
        {
            player.Stop();
            IsPlaying = false;
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

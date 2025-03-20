using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace DodgeTheSquares
{
    public class SoundManager
    {
        private Song _backgroundMusic;
        private Song _menuMusic;
        private float _backgroundMusicVolume;
        private bool _isMusicPlaying;

        private SoundEffect _collisionSound;
        private SoundEffect _menuHover;
        private SoundEffect _menuClickSound;

        public SoundManager()
        {
            _backgroundMusicVolume = 1.0f;  // Default volume for music (range 0 to 1)
            _isMusicPlaying = false;
        }

        // Load all the sound assets
        public void LoadContent(ContentManager content)
        {
            // Load background music and sound effects
            _backgroundMusic = content.Load<Song>("BackgroundMusic");
            _menuMusic = content.Load<Song>("MenuMusic");

            // Load sound effects (short sounds)
            _collisionSound = content.Load<SoundEffect>("CollisionSound");
            _menuHover = content.Load<SoundEffect>("MenuHover");
            _menuClickSound = content.Load<SoundEffect>("MenuClick");
        }

        // Play background music
        public void PlayBackgroundMusic()
        {
            if (!_isMusicPlaying)
            {
                MediaPlayer.Play(_backgroundMusic);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = _backgroundMusicVolume;
                _isMusicPlaying = true;
            }
        }

        // Stop the background music
        public void StopBackgroundMusic()
        {
            MediaPlayer.Stop();
            _isMusicPlaying = false;
        }

        public void PlayMenuMusic()
        {
            if (!_isMusicPlaying)
            {
                MediaPlayer.Play(_menuMusic);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.3f;
                _isMusicPlaying = true;
            }
        }


        // Play the collision sound effect
        public void PlayCollisionSound(float volume)
        {
            _collisionSound.Play(volume: MathHelper.Clamp(volume, 0.0f, 1.0f), 0f, 0f); // Volume is optional, defaults to 1.0f
        }
        public void PlayHoverSound(float volume)
        {
            _menuHover.Play(volume: MathHelper.Clamp(volume, 0.0f, 1.0f), 0f, 0f); // Volume is optional, defaults to 1.0f
        }


        // Play the menu click sound effect
        public void PlayMenuClickSound(float volume)
        {
            _menuClickSound.Play(volume: MathHelper.Clamp(volume, 0.0f, 1.0f), 0f, 0f);
        }

        // Adjust the background music volume
        public void SetBackgroundMusicVolume(float volume)
        {
            _backgroundMusicVolume = MathHelper.Clamp(volume, 0.0f, 1.0f); // Volume should be between 0 and 1
            if (_isMusicPlaying)
            {
                MediaPlayer.Volume = _backgroundMusicVolume;
            }
        }

        // Get current background music status
        public bool IsMusicPlaying()
        {
            return _isMusicPlaying;
        }

    }
}

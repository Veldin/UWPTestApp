using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace EindopdrachtUWP.Classes
{
    class Batarang : Weapon
    {
        public List<string> tags { get => tags; }
        public string name { get { return name; } set { name = value; } }
        public string description { get { return description; } set { description = value; } }
        public int currentClip { get { return currentClip; } set { currentClip = value; } }
        public int clipAmount { get { return clipAmount; } set { clipAmount = value; } }
        public int clipMax { get { return clipMax; } set { clipMax = value; } }
        public int damage { get { return damage; } set { damage = value; } }
        public float fireTime { get { return fireTime; } set { fireTime = value; } }
        public float fireTimer { get { return fireTimer; } set { fireTimer = value; } }
        public double critChance { get { return critChance; } set { critChance = value; } }
        public double critMultiplier { get { return critMultiplier; } set { critMultiplier = value; } }
        public int weaponLevel { get { return weaponLevel; } set { weaponLevel = value; } }
        public MediaElement shotsound { get { return shotsound; } set { shotsound = value; } }
        public MediaElement reloadSound { get { return reloadSound; } set { reloadSound = value; } }
        public float reloadTime { get { return reloadTime; } set { reloadTime = value; } }
        public float reloadTimer { get { return reloadTimer; } set { reloadTimer = value; } }

        Batarang()
        {
            // constructor for the Batarang class
            name = "Batarang";
            description = "The Batarang is a throwable weapon which has similarities to a boomerang";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 1;
            damage = 70;
            fireTime = 0.5f;
            critChance = 0.5;
            critMultiplier = 2;
            weaponLevel = 1;
            reloadTime = 1;
        }

        public void AddTag(string tag)
        {
            // add a tag to the tags list
            tags.Add(tag);
        }

        public void Display()
        {
            // show sprite
            throw new NotImplementedException();
        }

        public void Fire(float fromTop, float fromLeft)
        {
            // fire one bullet
            if (currentClip > 0)
            {
                fireTimer = 0;
                new Projectile(10, 5, fromLeft, fromTop, 0, 0, 0, 0, damage);
                currentClip--;
                PlayShotSound();
            }
        }

        public Boolean HasTag(string searchTag)
        {
            // check if the weapon has a certain tag
            foreach (string tag in tags)
            {
                if (tag.Equals(searchTag))
                {
                    return true;
                }
            }
            return false;
        }

        public void PlayReloadSound()
        {
            // play the sound the gun makes when it reloads
            throw new NotImplementedException();
        }

        public void PlayShotSound()
        {
            // play the sound the gun makes when it fires
            throw new NotImplementedException();
        }

        public void Reload()
        {
            // reload this weapon, but only if you have enough clips
            if (clipAmount > 0)
            {
                reloadTimer = 0;
                clipAmount--;
                currentClip = clipMax;
                PlayReloadSound();
            }
        }

        public Boolean RemoveTag(String searchTag)
        {
            // searches for a specific tag and removes it if it's found
            foreach (string tag in tags)
            {
                if (tag.Equals(searchTag))
                {
                    tags.Remove(searchTag);
                    return true;
                }
            }
            return false;
        }

        public void Upgrade()
        {
            // upgrade weapon level for a stronger weapon
            weaponLevel++;
            damage += 5;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Racetrack
{
    class Program
    {
        static void Main(string[] args)
        {
            var AvalibelHorses = LoadHorses();
            var race = new Race()
                .AddHorse(7, AvalibelHorses)
                .StartRace();
            Console.ReadLine();
        }

        private static List<Horse> LoadHorses()
        {
            var horses = new List<Horse>();
            var data = File.ReadAllLines("Hoseses.txt");
            foreach (string horse in data)
            {
                var parts = horse.Split(',');
                horses.Add(new Horse()
                {
                    Name = parts[0],
                    Age = Int32.Parse(parts[1]),
                    Speed =Int32.Parse(parts[2])
                });

            }
            //Console.WriteLine(data);
            return horses;
        }
    }

    class Race
    {
        public List<string> Place { get; set; }
        public List<Horse> Horses { get; set; }
        public Race()
        {
            Place = new List<string>();

        }
        public void Result(string name)
        {
            Place.Add(name);
        }
        public Race AddHorse(int v, List<Horse> horses)
        {
            if (horses.Count <v)
            {
                throw new Exception("Not enuff horses");
            }
            Random rand = new Random();
            Horses= horses.OrderBy(x => rand.Next()).Take(v).ToList();
            Console.WriteLine("The horsese in whe race");
            Horses.ForEach(x => Console.WriteLine($"{x.Name} Has a age of {x.Age} " ));
            return this;
        }

        public Race StartRace()
        {
            Horses.ForEach(x => x.StartRunning(this));
            while (true)
            {
                Thread.Sleep(3000);
                if (Horses.Where(h=>h.HasFinnish==true).Count()==Horses.Count)
                {
                    Console.WriteLine("The race is finish ");
                    Console.WriteLine("The winner is "+Place.First());
                    
                    Thread.Sleep(2000);
                    Environment.Exit(0);
        
                }
            }
            return this;
        }
    }

     class Horse
    {
        public Race Race { get; set; }
        public int Distance { get; set; }
        public bool HasFinnish { get; set; }
        public Thread HorseThread { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Speed { get; set; }

        public Horse ()
        {
            HorseThread = new Thread(DoWork);
            HasFinnish = false;
            Distance = 0;

        }

        private void DoWork()
        {
            bool pastCheckpoint = false;
            while (true)
            {
                if (Distance>=100 && pastCheckpoint==false)
                {
                    Console.WriteLine(Name+" has passede 500 meter");
                    pastCheckpoint = true;
                }
                if (Distance>=300)
                {
                    Console.WriteLine(Name+" Has finished the race");
                    HasFinnish = true;
                    Race.Result(Name);
                    return;
                }
                Thread.Sleep(1000);
                Distance += Speed;
            }
            
        }


        internal void StartRunning(Race race)
        {
            Race = race;
            Console.WriteLine(Name+" Starts running");
            HorseThread.Start();
        }
    }
}

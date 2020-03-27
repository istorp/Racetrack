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
                .AddHorse(5 , AvalibelHorses)
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
        }//snyggt anders
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
                    Console.ForegroundColor = ConsoleColor.Red;
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
            Random rand = new Random();
            bool pastCheckpoint1 = false;
            bool pastCheckpoint2 = false;
            bool pastCheckpoint = false;
            while (true)
            {
                if (Distance >= 50 && pastCheckpoint == false)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Speed +=  rand.Next(1, 10);
                    Console.WriteLine(Name + " has passede 200 meter, new speed is "+Speed);
                    pastCheckpoint = true;
                }

                if (Distance>=100 && pastCheckpoint1==false)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Speed += rand.Next(1, 15);
                    Console.WriteLine(Name+ " has passede 500 meter, new speed is " + Speed);
                    pastCheckpoint1 = true;
                }
                if (Distance >= 250 && pastCheckpoint2 == false)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Speed += rand.Next(1, 15);
                    Console.WriteLine(Name + " has passede 700 meter, new speed is "+Speed);
                    pastCheckpoint2 = true;
                }

                if (Distance>=300)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
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

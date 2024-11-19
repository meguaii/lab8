using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.IO.Compression;

namespace Lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("=== Сериализация и десериализация ===");

            //список животных
            List<Animal> animals = new List<Animal>
            {
                new Cow("Мурка", "Россия", "в лесу", "Корова"),
                new Lion("Симба", "Кения", "в саванне", "Лев"),
                new Pig("Пеппа", "США", "в хлеву", "Свинка")
            };

            string filePath = "animals.xml";

            // Сериализация
            SerializeAnimals(filePath, animals);

            // Десериализация
            List<Animal> deserializedAnimals = DeserializeAnimals(filePath);
            Console.WriteLine("\nДесериализованные объекты:");
            foreach (var animal in deserializedAnimals)
            {
                animal.SayHello();
                animal.GetFavouriteFood();
                animal.GetClassificationAnimal(animal.WhatAnimal);
            }

            
            Console.WriteLine("\n=== Работа с файлами ===");
            Console.Write("Введите путь для поиска: ");
            string searchPath = Console.ReadLine();

            Console.Write("Введите имя файла для поиска: ");
            string fileName = Console.ReadLine();

            SearchAndProcessFile(searchPath, fileName);
        }

        //Метод сериализации
        static void SerializeAnimals(string filePath, List<Animal> animals)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Animal>), new Type[] { typeof(Cow), typeof(Lion), typeof(Pig) });

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, animals);
            }

            Console.WriteLine($"Список животных сериализован и сохранен в файл: {filePath}");
        }

        //Метод десериализации
        static List<Animal> DeserializeAnimals(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Animal>), new Type[] { typeof(Cow), typeof(Lion), typeof(Pig) });

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return (List<Animal>)serializer.Deserialize(fs);
            }
        }

        //Метод поиска и работы с файлами
        static void SearchAndProcessFile(string searchPath, string fileName)
        {
            try
            {
                string[] files = Directory.GetFiles(searchPath, fileName, SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    Console.WriteLine("Файл не найден.");
                }
                else
                {
                    foreach (string file in files)
                    {
                        Console.WriteLine($"Файл найден: {file}");

                        // Чтение содержимого файла
                        Console.WriteLine("Содержимое файла:");
                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            Console.WriteLine(sr.ReadToEnd());
                        }

                        // Сжатие файла
                        string compressedFile = file + ".gz";
                        using (FileStream originalFileStream = new FileStream(file, FileMode.Open))
                        using (FileStream compressedFileStream = new FileStream(compressedFile, FileMode.Create))
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }

                        Console.WriteLine($"Файл сжат и сохранен как: {compressedFile}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    //Классы животных
    [XmlInclude(typeof(Cow))]
    [XmlInclude(typeof(Lion))]
    [XmlInclude(typeof(Pig))]
    public abstract class Animal
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string HideFromOtherAnimals { get; set; }
        public string WhatAnimal { get; set; }

        protected Animal() { }

        protected Animal(string name, string country, string hideFromOtherAnimals, string whatAnimal)
        {
            Name = name;
            Country = country;
            HideFromOtherAnimals = hideFromOtherAnimals;
            WhatAnimal = whatAnimal;
        }

        public virtual void GetClassificationAnimal(string whatAnimal)
        {
            switch (whatAnimal)
            {
                case "Корова":
                    Console.WriteLine($"Классификация: {eClassificationAnimal.Herbivores}.");
                    break;
                case "Лев":
                    Console.WriteLine($"Классификация: {eClassificationAnimal.Carnivores}.");
                    break;
                case "Свинка":
                    Console.WriteLine($"Классификация: {eClassificationAnimal.Omnivores}.");
                    break;
                default:
                    Console.WriteLine("Неизвестная классификация.");
                    break;
            }
        }

        public virtual void SayHello() { }
        public virtual void GetFavouriteFood() { }
    }

    public class Cow : Animal
    {
        public Cow() { }

        public Cow(string name, string country, string hideFromOtherAnimals, string whatAnimal)
            : base(name, country, hideFromOtherAnimals, whatAnimal)
        {
        }

        public override void SayHello()
        {
            Console.WriteLine($"Корова {Name} из страны {Country} прячется {HideFromOtherAnimals} и говорит: Муу!");
        }

        public override void GetFavouriteFood()
        {
            Console.WriteLine("Любимая еда: трава.");
        }
    }

    public class Lion : Animal
    {
        public Lion() { }

        public Lion(string name, string country, string hideFromOtherAnimals, string whatAnimal)
            : base(name, country, hideFromOtherAnimals, whatAnimal)
        {
        }

        public override void SayHello()
        {
            Console.WriteLine($"Лев {Name} из страны {Country} прячется {HideFromOtherAnimals} и рычит: Ррр!");
        }

        public override void GetFavouriteFood()
        {
            Console.WriteLine("Любимая еда: мясо.");
        }
    }

    public class Pig : Animal
    {
        public Pig() { }

        public Pig(string name, string country, string hideFromOtherAnimals, string whatAnimal)
            : base(name, country, hideFromOtherAnimals, whatAnimal)
        {
        }

        public override void SayHello()
        {
            Console.WriteLine($"Свинка {Name} из страны {Country} прячется {HideFromOtherAnimals} и хрюкает: Хрю!");
        }

        public override void GetFavouriteFood()
        {
            Console.WriteLine("Любимая еда: все, что жуется.");
        }
    }

    public enum eClassificationAnimal
    {
        Herbivores,
        Carnivores,
        Omnivores
    }
}

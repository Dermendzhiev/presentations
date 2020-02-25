namespace SystemTextJson
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using System;
    using System.Threading.Tasks;

    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var person = new Person
            {
                FirstName = "Christopher",
                LastName = "Nolan",
                Age = 50,
                Address = "Sofia"
            };

            // 1. Basic serialization
            string jsonString = JsonSerializer.Serialize(person);
            Console.WriteLine(jsonString);


            // 2. Basic serialization with options
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            jsonString = JsonSerializer.Serialize(person, options);
            Console.WriteLine(jsonString);


            // 3. Basic deserialization
            Person deserializedPerson = JsonSerializer.Deserialize<Person>(jsonString, options);


            // 4. Async deserialization
            //using (FileStream fileStream = File.OpenRead("<PATH>"))
            //{
            //    Person asyncDeserialization = await JsonSerializer.DeserializeAsync<Person>(fileStream);
            //}
        }

        internal class Person
        {
            //[JsonPropertyName("_name__")]
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }

            // Exclude individual properties
            //[JsonIgnore]
            public string Address { get; set; }
        }
    }
}
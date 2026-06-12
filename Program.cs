using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Monster
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int ArmorClass { get; set; }

    public List<string> Abilities { get; set; } = new List<string>();
    public List<string> Attacks { get; set; } = new List<string>();
    public List<string> DamageTypes { get; set; } = new List<string>();
    public List<string> Weaknesses { get; set; } = new List<string>();
    public List<string> Conditions { get; set; } = new List<string>();
}

class Program
{
    static List<Monster> monsters = new List<Monster>();
    static string saveFile = "monsters.json";

    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== MONSTER TRACKER ===");
            Console.WriteLine("1. Add Monster");
            Console.WriteLine("2. View Monsters");
            Console.WriteLine("3. View Monster Details");
            Console.WriteLine("4. Add Ability");
            Console.WriteLine("5. Add Attack");
            Console.WriteLine("6. Add Damage Type");
            Console.WriteLine("7. Add Weakness");
            Console.WriteLine("8. Add Condition");
            Console.WriteLine("9. Damage Monster");
            Console.WriteLine("10. Heal Monster");
            Console.WriteLine("11. Remove Monster");
            Console.WriteLine("12. Save Monsters");
            Console.WriteLine("13. Load Monsters");
            Console.WriteLine("14. Exit");
            Console.Write("\nChoose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1": AddMonster(); break;
                case "2": ViewMonsters(); break;
                case "3": ViewMonsterDetails(); break;
                case "4": AddItemToMonster("ability"); break;
                case "5": AddItemToMonster("attack"); break;
                case "6": AddItemToMonster("damage type"); break;
                case "7": AddItemToMonster("weakness"); break;
                case "8": AddItemToMonster("condition"); break;
                case "9": DamageMonster(); break;
                case "10": HealMonster(); break;
                case "11": RemoveMonster(); break;
                case "12": SaveMonsters(); break;
                case "13": LoadMonsters(); break;
                case "14": running = false; break;
                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    // Adds a new monster to the tracker
    static void AddMonster()
    {
        Monster monster = new Monster();

        Console.Write("Monster Name: ");
        monster.Name = Console.ReadLine() ?? "";

        Console.Write("Monster Type: ");
        monster.Type = Console.ReadLine() ?? "";

        Console.Write("Max HP: ");
        monster.MaxHP = ReadNumber();

        monster.CurrentHP = monster.MaxHP;

        Console.Write("Armor Class: ");
        monster.ArmorClass = ReadNumber();

        monsters.Add(monster);

        Console.WriteLine("\nMonster added successfully!");
        Pause();
    }

    // Shows a short list of all monsters
    static void ViewMonsters()
    {
        Console.Clear();

        if (monsters.Count == 0)
        {
            Console.WriteLine("No monsters found.");
        }
        else
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                Monster m = monsters[i];
                Console.WriteLine($"{i + 1}. {m.Name} | {m.Type} | HP: {m.CurrentHP}/{m.MaxHP} | AC: {m.ArmorClass}");
            }
        }

        Pause();
    }

    // Shows full details for one selected monster
    static void ViewMonsterDetails()
    {
        Monster? monster = SelectMonster();

        if (monster == null)
        {
            return;
        }

        Console.Clear();
        Console.WriteLine($"Name: {monster.Name}");
        Console.WriteLine($"Type: {monster.Type}");
        Console.WriteLine($"HP: {monster.CurrentHP}/{monster.MaxHP}");
        Console.WriteLine($"Armor Class: {monster.ArmorClass}");

        PrintList("Abilities", monster.Abilities);
        PrintList("Attacks", monster.Attacks);
        PrintList("Damage Types", monster.DamageTypes);
        PrintList("Weaknesses", monster.Weaknesses);
        PrintList("Conditions", monster.Conditions);

        Pause();
    }

    // Adds an ability, attack, damage type, weakness, or condition
    static void AddItemToMonster(string itemType)
    {
        Monster? monster = SelectMonster();

        if (monster == null)
        {
            return;
        }

        Console.Write($"Enter {itemType}: ");
        string item = Console.ReadLine() ?? "";

        if (itemType == "ability")
        {
            monster.Abilities.Add(item);
        }
        else if (itemType == "attack")
        {
            monster.Attacks.Add(item);
        }
        else if (itemType == "damage type")
        {
            monster.DamageTypes.Add(item);
        }
        else if (itemType == "weakness")
        {
            monster.Weaknesses.Add(item);
        }
        else if (itemType == "condition")
        {
            monster.Conditions.Add(item);
        }

        Console.WriteLine($"\n{itemType} added successfully!");
        Pause();
    }

    // Applies damage to a monster and doubles damage if weakness matches
    static void DamageMonster()
    {
        Monster? monster = SelectMonster();

        if (monster == null)
        {
            return;
        }

        Console.Write("Damage amount: ");
        int damage = ReadNumber();

        Console.Write("Damage type: ");
        string damageType = Console.ReadLine() ?? "";

        bool weaknessTriggered = false;

        foreach (string weakness in monster.Weaknesses)
        {
            if (weakness.Equals(damageType, StringComparison.OrdinalIgnoreCase))
            {
                weaknessTriggered = true;
                break;
            }
        }

        if (weaknessTriggered)
        {
            damage *= 2;
            Console.WriteLine("\nWeakness triggered! Damage doubled.");
        }

        monster.CurrentHP -= damage;

        if (monster.CurrentHP < 0)
        {
            monster.CurrentHP = 0;
        }

        Console.WriteLine($"\n{monster.Name} took {damage} {damageType} damage.");
        Console.WriteLine($"{monster.Name} HP: {monster.CurrentHP}/{monster.MaxHP}");

        if (monster.CurrentHP == 0)
        {
            Console.WriteLine($"{monster.Name} has been defeated!");
        }

        Pause();
    }

    // Restores HP to a selected monster
    static void HealMonster()
    {
        Monster? monster = SelectMonster();

        if (monster == null)
        {
            return;
        }

        Console.Write("Healing amount: ");
        int healing = ReadNumber();

        monster.CurrentHP += healing;

        if (monster.CurrentHP > monster.MaxHP)
        {
            monster.CurrentHP = monster.MaxHP;
        }

        Console.WriteLine($"\n{monster.Name} healed for {healing} HP.");
        Console.WriteLine($"{monster.Name} HP: {monster.CurrentHP}/{monster.MaxHP}");

        Pause();
    }

    // Removes a monster from the tracker
    static void RemoveMonster()
    {
        Monster? monster = SelectMonster();

        if (monster == null)
        {
            return;
        }

        monsters.Remove(monster);

        Console.WriteLine($"\n{monster.Name} removed from the tracker.");
        Pause();
    }

    // Saves all monsters to a JSON file
    static void SaveMonsters()
    {
        string json = JsonSerializer.Serialize(monsters, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(saveFile, json);

        Console.WriteLine($"\nMonsters saved to {saveFile}.");
        Pause();
    }

    // Loads monsters from a JSON file
    static void LoadMonsters()
    {
        if (!File.Exists(saveFile))
        {
            Console.WriteLine("\nNo save file found.");
            Pause();
            return;
        }

        string json = File.ReadAllText(saveFile);
        List<Monster>? loadedMonsters = JsonSerializer.Deserialize<List<Monster>>(json);

        if (loadedMonsters != null)
        {
            monsters = loadedMonsters;
            Console.WriteLine("\nMonsters loaded successfully!");
        }
        else
        {
            Console.WriteLine("\nCould not load monsters.");
        }

        Pause();
    }

    // Lets the user select a monster from the list
    static Monster? SelectMonster()
    {
        Console.Clear();

        if (monsters.Count == 0)
        {
            Console.WriteLine("No monsters available.");
            Pause();
            return null;
        }

        Console.WriteLine("Select a monster:");

        for (int i = 0; i < monsters.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {monsters[i].Name}");
        }

        Console.Write("\nEnter number: ");
        int index = ReadNumber() - 1;

        if (index < 0 || index >= monsters.Count)
        {
            Console.WriteLine("Invalid monster selection.");
            Pause();
            return null;
        }

        return monsters[index];
    }

    // Prints a list section for monster details
    static void PrintList(string title, List<string> items)
    {
        Console.WriteLine($"\n{title}:");

        if (items.Count == 0)
        {
            Console.WriteLine("- None");
        }
        else
        {
            foreach (string item in items)
            {
                Console.WriteLine($"- {item}");
            }
        }
    }

    // Safely reads a number from the user
    static int ReadNumber()
    {
        int number;

        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.Write("Please enter a valid number: ");
        }

        return number;
    }

    // Pauses the program until the user presses Enter
    static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
} 